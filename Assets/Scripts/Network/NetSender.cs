using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Network
{
	//Line End
	public class NetSender
	{
        /// <summary>
        /// "消息包打包接口"
        /// </summary>
        private Dictionary<UInt16, Proxy> proxymap = new Dictionary<UInt16, Proxy>();

        /// <summary>
        /// "发送消息队列"
        /// </summary>
        private List<NetMsgBase> m_sendMessageList = new List<NetMsgBase>();

        /// <summary>
        /// "发送消息临时队列"
        /// </summary>
        private List<NetMsgBase> m_tempSendMessageList = new List<NetMsgBase>();

		NetworkStream m_stream = null;

        public void RegistProxy(Proxy p)
        {
            if ( p != null )
            {
                proxymap[p.Key] = p;
            }
        }

        public void AddMessage(NetMsgBase msg)
		{
            lock (m_sendMessageList)
            {
                m_sendMessageList.Add(msg);
            }
		}

		public void init(NetworkStream stream)
		{
            if (m_stream != null)
                m_stream.Close();
			m_stream = stream;
		}

		public void UpdateSend()
        {
            if (m_sendMessageList.Count < 1)
                return;
            if (m_stream == null || !m_stream.CanWrite)
				return;

            lock (m_sendMessageList)
            {
                m_tempSendMessageList.AddRange(m_sendMessageList);
                m_sendMessageList.Clear();
            }

            foreach (NetMsgBase msg in this.m_tempSendMessageList)
            {
                Proxy p = null;
                if (proxymap.TryGetValue(msg.m_msg, out p))
                {
                    try
                    {
                        byte[] btMsg = null;

                        p.M2B(ref msg.m_obj, ref btMsg);
                        Logger.LogDebug("send message [id] = {0}", msg.m_msg);
                        m_stream.BeginWrite(btMsg, 0, btMsg.Length, OnSend, null);   
                    }
                    catch (System.Exception ex)
                    {
                        Logger.LogError("exception message {0} error {1}!", msg.m_msg, ex.Message);
                    }
                }
                else
                {
                    Logger.LogError("error! can not find related message type", msg.m_msg);
                }
            }

            m_tempSendMessageList.Clear();
		}

		void OnSend(IAsyncResult ar)
        {
			try
			{
                m_stream.EndWrite(ar);
			}
			catch(Exception e)
			{
                Logger.LogError("sender:" + e.Message);
                NetworkMgr.getInstance().getClient().setStatus(NET_STATUS.DISCONNT);

				m_stream = null;
			}
		}
	}
}

