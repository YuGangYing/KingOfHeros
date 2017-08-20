using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using Packet;
using Network;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
namespace DataMgr
{
	public class FriendData
	{
		public enum FRIEND_STATUS
		{
			FRIEND_STATUS_APPLY,//申请
			FRIEND_STATUS_BE_APPLIED,//被申请
			FRIEND_STATUS_FRIEND,//好友
			FRIEND_STATUS_DEL,//删除
		};
		
		public class FriendInfo
		{
			public uint idFriendUser;
			public string szFriendName;
			public uint nGoldLevyTime;
			public uint nStoneLevyTime;
			public byte nFriendStatus;
		}
		
		//Dictionary<uint, FriendInfo> m_dicFriendInfo = new Dictionary<uint, FriendInfo>();
		Dictionary<uint, DataMgr.FriendData.FriendInfo> m_dicFriendInfo = new Dictionary<uint, DataMgr.FriendData.FriendInfo>();
		public FriendPanel m_friendPanel = new FriendPanel();

		public FriendData()
		{
		}
		
		public bool init()
		{
			// 注册消息处理函数
			NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_FRIEND_INFO>(OnFriendInfo);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_FRIEND_INFO_UPDATE>(OnFriendInfoUpdate);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_FRIEND_ENTER_CITY_RESPONSE>(OnEnterCity);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_FRIEND_APPLY_RESPONSE>(OnApplyFriend);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_FRIEND_REFUSE_APPLY_RESPONSE>(OnRefuseApply);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_FRIEND_ACCEPT_APPLY_RESPONSE>(OnAcceptApply);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_FRIEND_DEL_RESPONSE>(OnDelFriend);
			
			//QueueMgr.CreateInstance();
			return true;
		}
		
		public void release()
		{
			m_dicFriendInfo.Clear();
		}

		public void reload()
		{
		}

		public void QueryFriend()
		{
			MSG_CLIENT_FRIEND_INFO_QUERY_REQUEST msg = new MSG_CLIENT_FRIEND_INFO_QUERY_REQUEST();
			NetworkMgr.me.getClient().Send(ref msg);
		}
		
		public void OnFriendInfo(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_FRIEND_INFO>(id);
			MSG_CLIENT_FRIEND_INFO msg_struct = (MSG_CLIENT_FRIEND_INFO)ar;
			
			m_dicFriendInfo.Clear();
			
			for (int i = 0; i < msg_struct.usCnt; i++)
			{
				FriendInfo info = new FriendInfo();
				
				info.idFriendUser = msg_struct.lst[i].idFriendUser;
				info.szFriendName = msg_struct.lst[i].szFriendName;
				info.nGoldLevyTime = msg_struct.lst[i].nGoldLevyTime;
				info.nStoneLevyTime = msg_struct.lst[i].nStoneLevyTime;
				info.nFriendStatus = msg_struct.lst[i].nFriendStatus;
				
				m_dicFriendInfo.Add(info.idFriendUser, info);
			}
			
			SetShowToPanel();
		}
		
		public void OnFriendInfoUpdate(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_FRIEND_INFO_UPDATE>(id);
			MSG_CLIENT_FRIEND_INFO_UPDATE msg_struct = (MSG_CLIENT_FRIEND_INFO_UPDATE)ar;
			
			if (m_dicFriendInfo.ContainsKey(msg_struct.idFriendUser))
			{
				m_dicFriendInfo.Remove(msg_struct.idFriendUser);
			}
			
			FriendInfo info = new FriendInfo();
			
			info.idFriendUser = msg_struct.idFriendUser;
			info.szFriendName = msg_struct.szFriendName;
			info.nGoldLevyTime = msg_struct.nGoldLevyTime;
			info.nStoneLevyTime = msg_struct.nStoneLevyTime;
			info.nFriendStatus = msg_struct.nFriendStatus;
			
			m_dicFriendInfo.Add(info.idFriendUser, info);
			
			SetShowToPanel();
		}
		
		public void EnterCity(uint id)
		{
			MSG_CLIENT_FRIEND_ENTER_CITY_REQUEST msg = new MSG_CLIENT_FRIEND_ENTER_CITY_REQUEST();
			
			msg.idTarget = id;
			NetworkMgr.me.getClient().Send(ref msg);
		}
		
		public void OnEnterCity(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_FRIEND_ENTER_CITY_RESPONSE>(id);
			//MSG_CLIENT_FRIEND_ENTER_CITY_RESPONSE msg_struct = (MSG_CLIENT_FRIEND_ENTER_CITY_RESPONSE)ar;
		}
		
		public void ApplyFriend(uint id)
		{
			MSG_CLIENT_FRIEND_APPLY_REQUEST msg = new MSG_CLIENT_FRIEND_APPLY_REQUEST();
			
			msg.idTarget = id;
			NetworkMgr.me.getClient().Send(ref msg);
		}
		
		public void OnApplyFriend(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_FRIEND_APPLY_RESPONSE>(id);
			MSG_CLIENT_FRIEND_APPLY_RESPONSE msg_struct = (MSG_CLIENT_FRIEND_APPLY_RESPONSE)ar;
		}
		
		public void RefuseApply(uint id)
		{
			MSG_CLIENT_FRIEND_REFUSE_APPLY_REQUEST msg = new MSG_CLIENT_FRIEND_REFUSE_APPLY_REQUEST();
			
			msg.idTarget = id;
			NetworkMgr.me.getClient().Send(ref msg);
		}
		
		public void OnRefuseApply(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_FRIEND_REFUSE_APPLY_RESPONSE>(id);
			//MSG_CLIENT_FRIEND_REFUSE_APPLY_RESPONSE msg_struct = (MSG_CLIENT_FRIEND_REFUSE_APPLY_RESPONSE)ar;
		}
		
		public void AcceptApply(uint id)
		{
			MSG_CLIENT_FRIEND_ACCEPT_APPLY_REQUEST msg = new MSG_CLIENT_FRIEND_ACCEPT_APPLY_REQUEST();
			
			msg.idTarget = id;
			NetworkMgr.me.getClient().Send(ref msg);
		}
		
		public void OnAcceptApply(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_FRIEND_ACCEPT_APPLY_RESPONSE>(id);
			//MSG_CLIENT_FRIEND_ACCEPT_APPLY_RESPONSE msg_struct = (MSG_CLIENT_FRIEND_ACCEPT_APPLY_RESPONSE)ar;
		}
		
		public void DelFriend(uint id)
		{
			MSG_CLIENT_FRIEND_DEL_REQUEST msg = new MSG_CLIENT_FRIEND_DEL_REQUEST();
			
			msg.idTarget = id;
			NetworkMgr.me.getClient().Send(ref msg);
		}
		
		public void OnDelFriend(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_FRIEND_DEL_RESPONSE>(id);
			//MSG_CLIENT_FRIEND_DEL_RESPONSE msg_struct = (MSG_CLIENT_FRIEND_DEL_RESPONSE)ar;
		}
		
		public void LevyResource(uint id, byte type)
		{
			MSG_CLIENT_FRIEND_LEVY_RESOURCE_REQUEST msg = new MSG_CLIENT_FRIEND_LEVY_RESOURCE_REQUEST();
			
			msg.idTarget = id;
			msg.nResourceType = type;
			NetworkMgr.me.getClient().Send(ref msg);
		}
		
		public void OnLevyResource(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_FRIEND_LEVY_RESOURCE_RESPONSE>(id);
			//MSG_CLIENT_FRIEND_LEVY_RESOURCE_RESPONSE msg_struct = (MSG_CLIENT_FRIEND_LEVY_RESOURCE_RESPONSE)ar;
		}
		
		public void SetShowToPanel()
		{
			
			m_friendPanel.ClearItem();
			
			foreach (KeyValuePair<uint, DataMgr.FriendData.FriendInfo> kvi in m_dicFriendInfo)
			{
				if (kvi.Value.nFriendStatus != (byte)FRIEND_STATUS.FRIEND_STATUS_DEL)
				{
					m_friendPanel.AddItem(kvi.Value);
				}
			}
		}

	}
}