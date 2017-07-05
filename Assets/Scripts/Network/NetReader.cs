using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.IO;
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
    /// <summary>
    /// "消息处理回调接口"
    /// </summary>
    ///

	public class NetReader
	{
		enum READ_STEP
		{
			READ_NOREAD,
			READ_LEN,
			READ_MSGID,
			READ_BODY
		}

        private Dictionary<UInt16, MessageFunc> relaymap = new Dictionary<UInt16, MessageFunc>();

        /// <summary>
        /// "消息包解包接口"
        /// </summary>
        private Dictionary<UInt16, Proxy> proxymap = new Dictionary<UInt16, Proxy>();

        /// <summary>
        /// "IO线程和解包线程共享的消息包队列"
        /// </summary>
        private List<NetPacket> m_readPacketList = new List<NetPacket>();

        /// <summary>
        /// "解包线程临时消息包队列"
        /// </summary>
        private List<NetPacket> m_tempReadPacketList = new List<NetPacket>();

        /// <summary>
        /// "逻辑线程和解包线程共享的接收消息队列"
        /// </summary>
        private List<NetMsgBase> m_readMessageList = new List<NetMsgBase>();

        /// <summary>
        /// "逻辑线程临时接收消息队列"
        /// </summary>
        private List<NetMsgBase> m_tempReadMessageList = new List<NetMsgBase>();

		NetworkStream m_stream =  null;

		const int MAX_LEN = 1024;
		const int LEN_PACKLEN = 2;
		const int LEN_MSGID = 2;

		int m_nMsgid = 0;
		int m_nBodyLen = 0;
		int m_nRecvBodyLen = 0;

		READ_STEP m_step;
		byte[] m_recvData = new Byte[MAX_LEN];

        public void OnMessage(ushort id, object ar)
        {
            // leave this message empty
        }

        public void RegistProxy(Proxy p)
        {
            if (p != null)
            {
                proxymap[p.Key] = p;
                relaymap[p.Key] = OnMessage;
            }
        }

        public void RegMsgFunc<T>(MessageFunc func) where T : new()
        {
            UInt16 code = MSG.Sgt.GetTypeCode(typeof(T).FullName);
            relaymap[code] += func;
        }

        public void UnRegMsgFunc<T>(MessageFunc func) where T : new()
        {
            UInt16 code = MSG.Sgt.GetTypeCode(typeof(T).FullName);
            relaymap[code] -= func;
        }

		public void init(NetworkStream stream)
		{
            if (m_stream != null)
            {
                if (!m_stream.CanRead)
                {
                    m_stream.Close();
                }
            }

            m_stream = stream;
            clear();
			beginRead();
		}

		void beginRead()
		{
			if(m_stream==null)
				return;
			if(m_step!=READ_STEP.READ_NOREAD)
                return;
            clear();
			m_step = READ_STEP.READ_LEN;
			readPack(LEN_PACKLEN);
		}

		void clear()
		{
			m_nMsgid = 0;
			Array.Clear(m_recvData,0,m_recvData.Length);
			m_nBodyLen = 0;
			m_nRecvBodyLen = 0;
			m_step = READ_STEP.READ_NOREAD;
		}

		void readPack(int nLen)
		{
			if(m_stream==null)
				return;
            if (!m_stream.CanRead)
                return;
			try
			{
				m_stream.BeginRead(m_recvData,m_nRecvBodyLen,nLen,onRecv,m_stream);
			}
			catch(Exception e)
			{
				Debug.Log(e.Message);
			}
		}

        void onRecv(IAsyncResult ar)
        {
            NetworkStream stream = (NetworkStream)ar.AsyncState;
            try
            {
                if (stream != m_stream)
                {
                    stream.Close();
                    return;
                }

                int nRecvLen = stream.EndRead(ar);
                if (nRecvLen == 0)
                    return;

                m_nRecvBodyLen += nRecvLen;

                switch (m_step)
                {
                    case READ_STEP.READ_LEN:
                        m_nBodyLen = BitConverter.ToUInt16(m_recvData, 0);
                        m_step = READ_STEP.READ_MSGID;
                        readPack(LEN_MSGID);
                        break;
                    case READ_STEP.READ_MSGID:
                        m_nMsgid = BitConverter.ToUInt16(m_recvData, 2);
                        m_step = READ_STEP.READ_BODY;
                        int nLen = m_nBodyLen - LEN_PACKLEN - LEN_MSGID;
                        Logger.LogDebug("receive packet identifier = " + m_nMsgid + "; length = " + m_nBodyLen);
                        if (nLen < 1)
                        {
                            goto case READ_STEP.READ_BODY;
                        }
                        else
                        {
                            readPack(nLen);
                        }
                        break;
                    case READ_STEP.READ_BODY:
                        if (m_nRecvBodyLen < m_nBodyLen)
                            readPack(m_nBodyLen - m_nRecvBodyLen);
                        else
                        {   
                            NetPacket recvPacket = new NetPacket();
                            recvPacket.msgId = m_nMsgid;
                            recvPacket.length = m_nBodyLen;
                            recvPacket.data = MSG.Sgt.Truncate(m_recvData);
                            lock (m_readPacketList)
                            {
                                m_readPacketList.Add(recvPacket);
                            }
                            m_step = READ_STEP.READ_NOREAD;
                            this.beginRead();
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.Log("read Err:" + e.Message);
                if (stream == m_stream)
                {
                    NetworkMgr.getInstance().getClient().setStatus(NET_STATUS.DISCONNT);
                }

                m_stream = null;
            }
        }

        public void UpdateRead()
        {
            if (m_readPacketList.Count == 0)
                return;
            //each recv message , call handler for app process
            lock (m_readPacketList)
            {                
                m_tempReadPacketList.AddRange(m_readPacketList);
                m_readPacketList.Clear();
            }

            foreach (NetPacket m in m_tempReadPacketList)
            {
                Proxy proxy = null;
                if (proxymap.TryGetValue((ushort)m.msgId, out proxy))
                {
                    NetMsgBase msg = new NetMsgBase();
                    msg.m_obj = proxy.CreateObject();
                    msg.m_msg = (ushort)m.msgId;
                    proxy.B2M(ref msg.m_obj, ref m.data);

                    lock (this.m_readMessageList)
                    {
                        m_readMessageList.Add(msg);
                    }
                }
                else
                {
                    Debug.Log("UpdateRead::Process msg Error Not found Callback Func: msgid = " + m.msgId);
                }
            }

            m_tempReadPacketList.Clear();
        }

        public void DispatchNetMsg()
        {
            lock (m_readMessageList)
            {
                m_tempReadMessageList.AddRange(m_readMessageList);
                m_readMessageList.Clear();
            }

            foreach (NetMsgBase msg in m_tempReadMessageList)
            {
                if (!relaymap.ContainsKey(msg.m_msg))
                {
                    continue;
                }

                try
                {
                    if( relaymap.ContainsKey(msg.m_msg) )
                        relaymap[msg.m_msg](msg.m_msg, msg.m_obj);
                }
                catch (System.Exception e)
                {
                    // Process msg Error
                    Debug.Log("DispatchNetMsg::Process msg Error: msgid = " + msg.m_msg);
                }
            }

            m_tempReadMessageList.Clear();
        }
	}
}

