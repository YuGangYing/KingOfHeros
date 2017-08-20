using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using Packet;
using Network;
using SLG;

namespace DataMgr
{
	public class ChatmsgData
	{
        enum TXT_ATTRIBUTE
        {
            TXT_ATTRIBUTE_WORLD = 0,    //世界频道
            TXT_ATTRIBUTE_PRIVATE = 1,  //私聊频道
        };

        public class TalkInfo
        {
            public uint unTxtAttribute;             // 类型
            public string szSender;                 // 发送人
            public string szReceiver;               // 接受人
            public string szWords;                  // 内容   
        }

        public List<TalkInfo> talkList = new List<TalkInfo>();
        
        public ChatmsgData()
		{
		}
		
		public bool init()
		{
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_TALK>(OnRecTalk);
            
            return true;
		}
		
		public void release()
		{
            talkList.Clear();
		}

        public void OnRecTalk(ushort id, object ar)
        {
            MSG_CLIENT_TALK msg_struct = (MSG_CLIENT_TALK)ar;
            MSG.Sgt.CheckMessageId<MSG_CLIENT_TALK>(id);

            TalkInfo talk = new TalkInfo();
            talk.unTxtAttribute = msg_struct.unTxtAttribute;
            talk.szSender = msg_struct.szSender;
            talk.szWords = msg_struct.szWords;
            talk.szReceiver = msg_struct.szReceiver;

            talkList.Add(talk);

            ChatPanel chatPanel = UI.PanelManage.me.GetPanel<ChatPanel>(PanelID.ChatPanel);

            if (talk.unTxtAttribute == (uint)TXT_ATTRIBUTE.TXT_ATTRIBUTE_WORLD)
            {
                chatPanel.AddWorldItem(talk);
            }

            if (talk.unTxtAttribute == (uint)TXT_ATTRIBUTE.TXT_ATTRIBUTE_PRIVATE)
            {
                chatPanel.AddPrivateItem(talk);

                if (chatPanel.m_page != ChatPanel.PAGE.PRIVATE)
                {
                    chatPanel.SetPrivateCount(++chatPanel.m_mCount);
                }

            }
        }

        public void SendTalk(ushort atribute, string szSender, string szReceiver, string szWords)
        {
            MSG_CLIENT_TALK msg = new MSG_CLIENT_TALK();
            msg.unTxtAttribute = atribute;
            msg.szSender = szSender;
            msg.szReceiver = szReceiver;
            msg.szWords = szWords;
            NetworkMgr.me.getClient().Send(ref msg);
        }
	}
}
