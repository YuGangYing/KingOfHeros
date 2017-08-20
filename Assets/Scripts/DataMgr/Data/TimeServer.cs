using System;
using System.Collections.Generic;
using Network;
using UnityEngine;
using Packet;

namespace DataMgr
{
	public class TimeServer
	{
        private Int64 ltc;

		public Int64 ServerTime { get { return ltc; } }		
		
        public TimeServer()
        {

        }

        public bool init()
        {
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_SERVER_TIME_EVENT>(this.OnMsgServerTime);
            return true;
        }

        private void OnMsgServerTime(ushort id, object ar)
        {
            MSG_CLIENT_SERVER_TIME_EVENT e = (MSG_CLIENT_SERVER_TIME_EVENT)ar;
            ltc = (Int64)e.unServerTime - (Int64)Time.realtimeSinceStartup;
        }

        public Int64 EstimateServerTime(Int64 t)
        {
            return t - (Int64)Time.realtimeSinceStartup - ltc;
        }
	}
}
