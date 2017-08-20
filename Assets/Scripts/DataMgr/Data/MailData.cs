using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using Packet;
using Network;
using SLG;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
namespace DataMgr
{
	public class MailData
	{

		public class Mail
		{
			public uint idMail;              // 邮件ID
			public uint idSender;            // 发送玩家ID
			public string szSenderName;      // 发送玩家名字
			public byte nMailType;           // 邮件类型
			public string szMailTitle;       // 邮件主题
			public string szMailContent;     // 邮件内容
			public uint nMoney;              // 金币
			public uint nStone;              // 魔石
			public uint nDiamond;            // 钻石
			public long nCreateTime;         // 创建时间
		}
		
		//邮件类型: 找策划具体再定
		enum MAIL_TYPE
		{
			MAIL_TYPE_NONE = 0,
			MAIL_TYPE_USER = 1,                       //玩家邮件
			MAIL_TYPE_SYSTEM_BEGIN = 2,
			MAIL_TYPE_SYSTEM = MAIL_TYPE_SYSTEM_BEGIN,//系统邮件
			MAIL_TYPE_SYSTEM_RES_PLUNDER = 3,//系统邮件资源掠夺反击
			MAIL_TYPE_SYSTEM_ARENA = 4,//系统邮件竞技场反击
			MAIL_TYPE_SYSTEM_END,
		};
		
		Dictionary<uint, Mail> m_dicMail;
		uint idDelMail;
		
		private Mail m_createNewMail = null;



		public MailData()
		{
		}
		
		public bool init()
		{
			// 注册消息处理函数
			NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_SENDMAIL_RESPONSE>(this.onSendMail);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_DELMAIL_RESPONSE>(this.onDelMail);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_MAIL_INFO>(this.onMailInfo);
			
			// 初始化数据
			m_dicMail = new Dictionary<uint, Mail>();
			
			return true;
		}
		
		public void release()
		{
			m_dicMail.Clear();
		}

		public void reload()
		{
		}

		public void TextDate()
		{
			//测试
			for (int i = 0; i < 10; ++i)
			{
				Mail mail = new Mail();
				mail.idMail = (uint)(123 + i);
				mail.idSender = (uint)(300 + i);
				mail.nCreateTime = (uint)DataManager.getTimeServer().ServerTime;
				mail.nDiamond = 9900;
				mail.nMailType = (byte)DataMgr.MailData.MAIL_TYPE.MAIL_TYPE_USER;
				mail.nMoney = 99900;
				mail.nStone = 99900;
				mail.szMailContent = "aaaaaaa";
				mail.szMailTitle = "bbbbb";
				mail.szSenderName = "ccc" + i.ToString();
				
				if (m_dicMail.ContainsKey(mail.idMail))
				{
					m_dicMail.Remove(mail.idMail);
				}
				
				m_dicMail.Add(mail.idMail, mail);
			}
			
			for (int i = 0; i < 10; ++i)
			{
				Mail mail = new Mail();
				mail.idMail = (uint)(1001 + i);
				mail.idSender = (uint)(300 + i);
				mail.nCreateTime = DataManager.getTimeServer().ServerTime;
				mail.nDiamond = 9900;
				mail.nMailType = (byte)DataMgr.MailData.MAIL_TYPE.MAIL_TYPE_SYSTEM_RES_PLUNDER;
				mail.nMoney = 99900;
				mail.nStone = 99900;
				mail.szMailContent = "abcdefg";
				mail.szMailTitle = "bbbbb";
				mail.szSenderName = "ddd" + i.ToString();
				
				if (m_dicMail.ContainsKey(mail.idMail))
				{
					m_dicMail.Remove(mail.idMail);
				}
				
				m_dicMail.Add(mail.idMail, mail);
				
			}
		}
		
		public void SetShowToMailPanel()
		{
			MailPanel mailPanel = UI.PanelManage.me.GetPanel<MailPanel>(PanelID.MailPanel);
			
			mailPanel.ClearUI();
			
			foreach (KeyValuePair<uint, Mail> kvi in m_dicMail)
			{
				if (kvi.Value.nMailType == (byte)DataMgr.MailData.MAIL_TYPE.MAIL_TYPE_USER
				    || kvi.Value.nMailType == (byte)DataMgr.MailData.MAIL_TYPE.MAIL_TYPE_SYSTEM_BEGIN
				    || kvi.Value.nMailType == (byte)DataMgr.MailData.MAIL_TYPE.MAIL_TYPE_SYSTEM)
				{
					mailPanel.AddMailItem(kvi.Value);
				}
				
				if (kvi.Value.nMailType == (byte)DataMgr.MailData.MAIL_TYPE.MAIL_TYPE_SYSTEM_RES_PLUNDER
				    || kvi.Value.nMailType == (byte)DataMgr.MailData.MAIL_TYPE.MAIL_TYPE_SYSTEM_ARENA)
				{
					mailPanel.AddWarItem(kvi.Value);
				}
			}
		}
		
		public void SendMail(uint idAccepter, string szMailTitle, string szMailContent)
		{
			MSG_CLIENT_SENDMAIL_REQUEST msg = new MSG_CLIENT_SENDMAIL_REQUEST();
			msg.idAccepter = idAccepter;
			msg.szMailTitle = szMailTitle;
			msg.szMailContent = szMailContent;
			NetworkMgr.me.getClient().Send(ref msg);
		}
		
		public void SendMailByName(string szName, string szMailTitle, string szMailContent)
		{
			//         MSG_CLIENT_SENDMAIL_BY_NAME_REQUEST msg = new MSG_CLIENT_SENDMAIL_BY_NAME_REQUEST();
			//         msg.szAccepterName = szName;
			//         msg.szMailTitle = szMailTitle;
			//         msg.szMailContent = szMailContent;
			//         NetworkMgr.me.getClient().Send(ref msg);
		}
		
		public void onSendMail(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_SENDMAIL_RESPONSE>(id);
			//MSG_CLIENT_SENDMAIL_RESPONSE msg_struct = (MSG_CLIENT_SENDMAIL_RESPONSE)ar;
			
		}
		
		public void DelMail(uint idMail)
		{
			if (m_dicMail.ContainsKey(idMail))
			{
				
				if (m_dicMail[idMail].nMailType != (byte)MAIL_TYPE.MAIL_TYPE_USER)
				{
					return;
				}
				
				MSG_CLIENT_DELMAIL_REQUEST msg = new MSG_CLIENT_DELMAIL_REQUEST();
				msg.idMail = idMail;
				NetworkMgr.me.getClient().Send(ref msg);
				
				idDelMail = idMail;
				
			}
		}
		
		public void onDelMail(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_DELMAIL_RESPONSE>(id);
			//MSG_CLIENT_DELMAIL_RESPONSE msg_struct = (MSG_CLIENT_DELMAIL_RESPONSE)ar;
			
			DelDicMailByID(idDelMail);
			SetShowToMailPanel();
		}
		
		public void onMailInfo(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_CLIENT_MAIL_INFO>(id);
			MSG_CLIENT_MAIL_INFO msg_struct = (MSG_CLIENT_MAIL_INFO)ar;
			
			for (int i = 0; i < msg_struct.usCnt; i++)
			{
				Mail mail = new Mail();
				
				mail.idMail = msg_struct.lst[i].idMail;
				mail.idSender = msg_struct.lst[i].idSender;
				mail.szSenderName = msg_struct.lst[i].szSenderName;
				mail.nMailType = msg_struct.lst[i].nMailType;
				mail.szMailTitle = msg_struct.lst[i].szMailTitle;
				mail.szMailContent = msg_struct.lst[i].szMailContent;
				mail.nMoney = msg_struct.lst[i].nMoney;
				mail.nStone = msg_struct.lst[i].nStone;
				mail.nDiamond = msg_struct.lst[i].nDiamond;
				mail.nCreateTime = msg_struct.lst[i].nCreateTime;
				
				m_dicMail.Add(msg_struct.lst[i].idMail, mail);
			}
		}
		
		public void AddMail(Mail m)
		{
			m_dicMail.Add(m.idMail, m);
		}
		
		public Mail GetMailByID(uint unID)
		{
			if (m_dicMail.ContainsKey(unID))
			{
				return m_dicMail[unID];
			}
			
			return null;
		}
		
		public void DelDicMailByID(uint unID)
		{
			if (m_dicMail.ContainsKey(unID))
			{
				m_dicMail.Remove(unID);
			}
		}
		
		public void WriteMail(string name, uint id)
		{
			MailPanel mailPanel = UI.PanelManage.me.GetPanel<MailPanel>(PanelID.MailPanel);
			
			mailPanel.SetVisible(true);
			
			mailPanel.Write(name, id);
		}
		
		public void WriteMailById(uint id)
		{
			MailPanel mailPanel = UI.PanelManage.me.GetPanel<MailPanel>(PanelID.MailPanel);
			
			mailPanel.SetVisible(true);
		}
	}
}