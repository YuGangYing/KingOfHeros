using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using Packet;
using Network;


namespace DataMgr
{
	public class QueueData
	{
		public QueueData()
		{
		}
		
		public bool init()
		{
			NetworkMgr.me.getClient().RegMsgFunc<MSG_QUEUE_LIST>(OnRecQueueList);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_QUEUE_UPDATE>(OnRecQueueUpdate);
			
			m_dicQueueInfo = new Dictionary<QUEUE_TYPE, QUEUE_INFO>();			
			return true;
		}
		
		public void release()
		{
		}
		
		
		public enum QUEUE_TYPE
		{
			TYPE_BUILD = 1,
			TYPE_TECHNOLOGY,
		}
		
		Dictionary<QUEUE_TYPE, QUEUE_INFO> m_dicQueueInfo;
		
		
		public void OnRecQueueList(ushort id, object ar)
		{
			MSG_QUEUE_LIST msg_struct = (MSG_QUEUE_LIST)ar;
			if (msg_struct.usCnt > 0)
			{
				QUEUE_INFO info = msg_struct.lst[0];
				m_dicQueueInfo[(QUEUE_TYPE)msg_struct.cbProgressType] = info;
			}
			
			//通知界面更新数据
			SLG.GlobalEventSet.FireEvent(SLG.eEventType.UpdateQueueTime, null);
		}
		
		void OnRecQueueUpdate(ushort id, object ar)
		{
			MSG_QUEUE_UPDATE msg_struct = (MSG_QUEUE_UPDATE)ar;
			QUEUE_TYPE type = (QUEUE_TYPE)msg_struct.cbProgressType;
			
			if (m_dicQueueInfo.ContainsKey(type))
			{
				QUEUE_INFO info;
				info.idProgress = msg_struct.idProgress;
				info.u32DueTime = msg_struct.u32DueTime;
				m_dicQueueInfo[type] = info;
				
				//通知界面更新数据
				SLG.GlobalEventSet.FireEvent(SLG.eEventType.UpdateQueueTime, null);
			}
		}
		
		public uint GetDueTime (QUEUE_TYPE type)
		{
			if (m_dicQueueInfo.ContainsKey(type))
			{
				QUEUE_INFO info = m_dicQueueInfo[type];
				if (info.idProgress <= 0)
				{
					return 0;
				}
				
				return info.u32DueTime;
			}
			
			return 0;
		}
		
		public uint GetProgressID(QUEUE_TYPE type)
		{
			if (m_dicQueueInfo.ContainsKey(type))
			{
				QUEUE_INFO info = m_dicQueueInfo[type];
				return info.idProgress;
			}
			
			return 0;
		}		
	}
}