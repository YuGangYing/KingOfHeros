using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Packet;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
namespace Network
{
	public enum NET_RESULT
	{
		TIMEOUT,
		SUCC,
		FAILED,
		UNKNOW
	}
	
	public enum NET_STATUS:int
	{
		NOCNNT,
		CONNETING,
		CONNTED,
		DISCONNT,
        CONNTFAIL,
		CLOSING,
	}

    public class NetClient
	{
		string m_strIp;
		int m_nPort;
		TcpClient m_client = null;
		NetReader m_reader = null;
		NetSender m_sender = null;

		private int m_nStatus;
        private System.Object statusLock = new System.Object();

		string m_strErrMsg;

        Thread m_thdMsgThread;
        bool m_thdStop = false;
	
		public NetClient()
		{
			m_reader = new NetReader();
			m_sender = new NetSender();
		}

        public void RegProxy(String px)
        {
            Type t = Type.GetType(px);
            Proxy p = new Proxy(t);

            if (p.CanB2M)
            {
                m_reader.RegistProxy(p);
            }

            if (p.CanM2B)
            {
                m_sender.RegistProxy(p);
            }
            
        }

        public void RegMsgFunc<T>(MessageFunc handle) where T : new()
        {
            m_reader.RegMsgFunc<T>(handle);
        }

        public void UnRegMsgFunc<T>(MessageFunc handle) where T : new()
        {
            m_reader.UnRegMsgFunc<T>(handle);
        }

		public bool connect()
		{
			if(m_strIp==null || m_strIp.Length==0)
				return false;
			if(m_nPort <=0 || m_nPort > 65536)
				return false;

			try
			{
				m_client = new TcpClient();
				setStatus(NET_STATUS.CONNETING);
				m_client.BeginConnect(m_strIp,m_nPort,new AsyncCallback(this.OnConnect),m_client);
			}
			catch(Exception e)
			{
				setStatus(NET_STATUS.NOCNNT);
				this.m_strErrMsg = e.Message;
				return false;
			}
			return true;
		}

		public bool connect(string strIp,int nPort)
		{
			Debug.Log("Connect Server:" + strIp+":"+nPort);
			if(strIp==null || strIp.Length==0)
				return false;
			if(nPort <=0 || nPort > 65536)
				return false;

            if (isConn()&& (m_strIp == strIp && m_nPort == nPort))
                return true;

            m_strIp = strIp;
            m_nPort = nPort;
            this.close();
            return this.connect();
		}

		void OnConnect(IAsyncResult ar)
		{
			try
			{
				TcpClient client = ar.AsyncState as TcpClient;
                client.EndConnect(ar);

                m_reader.init(client.GetStream());
                m_sender.init(client.GetStream());

                m_strErrMsg = "connect(" + m_strIp + ":" + m_nPort + ")SUCC";
                Debug.Log(m_strErrMsg);
                setStatus(NET_STATUS.CONNTED);

                if (m_thdMsgThread == null)
                {
                    m_thdMsgThread = new Thread(new ThreadStart(this.RunMsgThread));
                    m_thdMsgThread.Start();
                }
			}
			catch(Exception e)
			{
                setStatus(NET_STATUS.CONNTFAIL);
                m_strErrMsg = "connect(" + m_strIp + ":" + m_nPort + ")Failed:" + e.Message;
                Debug.Log(m_strErrMsg);
			}
		}

        public void DisConnect()
        {
            Debug.Log("disConnect:" + m_strIp + ":" + m_nPort);
            close();
            //自动重连
            connect();
        }

		public bool isConn()
		{
			//if(this.m_status==NET_STATUS.CONNTED)
			//	return true;
            if (m_client != null && m_client.GetStream().CanRead)
                return true;
            else
                return false;
		}

        public void Send<T>(ref T msg)
        {
            m_sender.AddMessage(new NetMsgBase(MSG.Sgt.GetTypeCode(typeof(T)), msg));
        }

        public void update()
        {
            m_reader.DispatchNetMsg();

            switch ((NET_STATUS)m_nStatus)
            {
                case NET_STATUS.CONNTED:
                    {
                        SLG.GlobalEventSet.FireEvent(SLG.eEventType.NetConntSucc, new SLG.EventArgs(m_strErrMsg));
                        setStatus(NET_STATUS.NOCNNT);
                        break;
                    }
                case NET_STATUS.CONNTFAIL:
                    {
                        SLG.GlobalEventSet.FireEvent(SLG.eEventType.NetConntFailed, new SLG.EventArgs(m_strErrMsg));
                        setStatus(NET_STATUS.NOCNNT);
                        break;
                    }
                case NET_STATUS.DISCONNT:
                    {
                        DisConnect();
                        setStatus(NET_STATUS.NOCNNT);
                        SLG.GlobalEventSet.FireEvent(SLG.eEventType.NetDiscon, new SLG.EventArgs(null));
                        break;
                    }
                default:
                    break;
            }
        }

        private void RunMsgThread ()
        {
            while (!m_thdStop)
            {
                Thread.Sleep(1);
                try
                {
                    m_sender.UpdateSend();
                    m_reader.UpdateRead();
                }
                catch (System.Exception ex)
                {

                }
            }
        }

		public void setStatus(NET_STATUS status)
		{
            lock (statusLock)
            {
                m_nStatus = (int)status;
            }
		}

		public void close()
		{
			m_reader.init(null);
			m_sender.init(null);

			this.setStatus(NET_STATUS.NOCNNT);
			if(m_client!=null)
				m_client.Close();
			m_client = null;
		}

        public void release()
        {
            if (m_thdMsgThread != null)
            {
                m_thdStop = true;
                m_thdMsgThread.Abort();
                this.close();
            }
        }
	}
}

