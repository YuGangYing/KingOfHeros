using System;
using System.Text;
using Packet;

namespace Network
{
    public class NetworkMgr : SingletonMonoBehaviourNoCreate<NetworkMgr>
	{
        public static NetworkMgr getInstance()
		{
            return me;
		}

		NetClient m_client = null;

        protected override void Init()
        {
            base.Init();
            m_client = new NetClient();
            MSG.Sgt.Initialize();
            gameObject.AddComponent<DataMgr.DataManager>();
        }

        public NetClient getClient()
		{
			return m_client;
		}

        void Update()
        {
            m_client.update();
        }

        void OnDestroy()
        {
        }
	}
}

