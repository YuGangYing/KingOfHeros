using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataMgr;

namespace UI
{
    public class MailPanel : PanelBase
    {
        public const PanelID id = PanelID.MailPanel;

        public enum PAGE
        {
            WARCOMMUNIQUE = 0,
            MAIL = 1,
        }
        
        GameObject m_root;
        GameObject m_warItem;
        GameObject m_mailItem;
        GameObject WarCommunique;
        GameObject Mail;
        GameObject ReadMail;
        GameObject WriteMail;
        GameObject MailView;
        GameObject WarView;
        UIGrid m_warGrid;
        //UIGrid m_mailGrid;
        UISprite warBtnBg;
        UISprite mailBtnBg;

        PAGE m_page = PAGE.MAIL;

        public override string GetResPath()
        {
            return "MailUI.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        protected override void Initimp(List<GameObject> prefabs)
        {
            UIEventListener.Get(PanelTools.FindChild(Root, "closeBtn")).onClick = OnClose;
            UIEventListener.Get(PanelTools.FindChild(Root, "WarBtn")).onClick = OnWarBtn;
            UIEventListener.Get(PanelTools.FindChild(Root, "MailBtn")).onClick = OnMailBtn;

            m_root = Root;
            GameObject warBtn = UISoldierPanel.findChild(m_root, "WarBtn");
            warBtnBg = PanelTools.Find<UISprite>(warBtn, "Background");
            GameObject mailBtn = UISoldierPanel.findChild(m_root, "MailBtn");
            mailBtnBg = PanelTools.Find<UISprite>(mailBtn, "Background");

            Mail = UISoldierPanel.findChild(Root, "Mail");
            MailView = UISoldierPanel.findChild(Mail, "view");
            //m_mailGrid = Find<UIGrid>(Mail, "MailGrid");
            m_mailItem = UISoldierPanel.findChild(Mail, "MailItem");

            WarCommunique = UISoldierPanel.findChild(Root, "WarCommunique");
            WarView = UISoldierPanel.findChild(WarCommunique, "view");
            m_warGrid = PanelTools.Find<UIGrid>(WarCommunique, "WarGrid");
            m_warItem = UISoldierPanel.findChild(WarCommunique, "WarItem");

            ReadMail = UISoldierPanel.findChild(Root, "ReadMail");
            UIEventListener.Get(PanelTools.FindChild(ReadMail, "returnBtn")).onClick = OnDelReturn;

            WriteMail = UISoldierPanel.findChild(Root, "WriteMail");
            UIEventListener.Get(PanelTools.FindChild(WriteMail, "returnBtn")).onClick = OnReturn;
            UIEventListener.Get(PanelTools.FindChild(WriteMail, "sendBtn")).onClick = OnSendMailBtn;

            SetVisible(false);  
        }

        protected void OnClose(GameObject go)
        {
            ClearUI();
            
            SetVisible(false);
        }

        protected void OnWarBtn(GameObject go)
        {
            SetPage(PAGE.WARCOMMUNIQUE);

            Mail.SetActive(false);
            ReadMail.SetActive(false);
            WriteMail.SetActive(false);
            WarCommunique.SetActive(true);
        }

        protected void OnMailBtn(GameObject go)
        {
            SetPage(PAGE.MAIL);

            ReadMail.SetActive(false);
            WarCommunique.SetActive(false);
            Mail.SetActive(true);
        }

        protected void OnReadMail(GameObject go)
        {
            UILabel idLable = PanelTools.Find<UILabel>(go, "id");
            uint id = uint.Parse(idLable.text);

            UILabel sender = PanelTools.Find<UILabel>(go, "name");
            UILabel sendTitle = PanelTools.Find<UILabel>(go, "title");
            UILabel content = PanelTools.Find<UILabel>(ReadMail, "content");
            UILabel name = PanelTools.Find<UILabel>(ReadMail, "fromName");
            name.text = sender.text;
            UILabel title = PanelTools.Find<UILabel>(ReadMail, "title");
            title.text = sendTitle.text;
            UILabel idMail = PanelTools.Find<UILabel>(ReadMail, "idMail");
            idMail.text = idLable.text;

			DataMgr.MailData.Mail mail = DataManager.getMailData().GetMailByID(id);

            content.text = mail.szMailContent;

            Mail.SetActive(false);
            WarCommunique.SetActive(false);
            ReadMail.SetActive(true);
        }

        protected void OnSendMailBtn(GameObject go)
        {
            UILabel id = PanelTools.Find<UILabel>(WriteMail, "id");

            uint idAccepter = 0;
            if (id != null)
            {
                idAccepter = uint.Parse(id.text);
            }


            UILabel name = PanelTools.Find<UILabel>(WriteMail, "receiverName");
            string AccepterName = name.text;

            UILabel title = PanelTools.Find<UILabel>(WriteMail, "title");
            string strTitle = title.text;
            UILabel content = PanelTools.Find<UILabel>(WriteMail, "content");
            string strContent = content.text;

            if (id != null && strTitle != null && strContent != null)
            {
				DataManager.getMailData().SendMail(idAccepter, strTitle, strContent);
                return;
            }

            if (AccepterName != null && strTitle != null && strContent != null)
            {
				DataManager.getMailData().SendMailByName(AccepterName, strTitle, strContent);
            }

            SetVisible(false);
        }

        protected void OnReturn(GameObject go)
        {
            WarCommunique.SetActive(false);
            ReadMail.SetActive(false);
            WriteMail.SetActive(false);
            Mail.SetActive(true);
        }

        protected void OnDelReturn(GameObject go)
        {
            UILabel idLabel = PanelTools.Find<UILabel>(ReadMail, "idMail");
            uint id = uint.Parse(idLabel.text);

			DataManager.getMailData().DelMail(id);

            WarCommunique.SetActive(false);
            ReadMail.SetActive(false);
            WriteMail.SetActive(false);
            Mail.SetActive(true);
        }

        protected override void onShow()
        {
            UpdateUI();
            base.onShow();
        }

        protected void SetPage(PAGE page)
        {
            m_page = page;

            if (m_page == PAGE.MAIL)
            {
                warBtnBg.spriteName = "mailBtnBg";
                mailBtnBg.spriteName = "mailChosenBtnBg";
            }

            if (m_page == PAGE.WARCOMMUNIQUE)
            {
                warBtnBg.spriteName = "mailChosenBtnBg";
                mailBtnBg.spriteName = "mailBtnBg";
            }
        }

        public class MailItem
        {
            public GameObject root;
            public UILabel name;
            public UILabel title;
            public UILabel time;
            public UILabel timeX;
            public UILabel id;
            public UISprite icon;
			public DataMgr.MailData.Mail itemData;
            public bool isFill = false;

            public void Release()
            {
                if (root != null)
                {
                    GameObject.Destroy(root);
                }
            }

            public void Update()
            {
                if (itemData != null)
                {
                    id.text = itemData.idMail.ToString();
                    name.text = itemData.szSenderName;
                    title.text = itemData.szMailTitle;

                    DateTime currentDate = DateTime.Now;
                    //long nTime = Game.Sgt.EstimateServerTime(itemData.nCreateTime);
					long nTime = DataManager.getTimeServer().EstimateServerTime(itemData.nCreateTime);
                    timeX.text = nTime.ToString();
                    long ltime = currentDate.Ticks + (nTime * 10000000);
                    time.text = new DateTime(ltime).ToString("MM/dd/yyyy HH:mm");
                }
            }
        }

        public class WarItem
        {
            public GameObject root;
            public UILabel gold;
            public UILabel blue;
            public UILabel time;
            public UILabel name;
            public UILabel result;
            public UILabel id;
			public DataMgr.MailData.Mail itemData;
            public bool isFill = false;

            public void Release()
            {
                if (root != null)
                {
                    GameObject.Destroy(root);
                }
            }

            public void Update()
            {
                if (itemData != null)
                {
                    name.text = itemData.szSenderName;
                    gold.text = itemData.nMoney.ToString();
                    blue.text = itemData.nStone.ToString();

                    //long nTime = -Game.Sgt.EstimateServerTime(itemData.nCreateTime);
					long nTime = -DataManager.getTimeServer().EstimateServerTime(itemData.nCreateTime);
                    time.text = ShowTimeAgo(nTime);
                }
            }
        }

        private List<WarItem> m_warItemList = new List<WarItem>();
        private List<MailItem> m_mailItemList = new List<MailItem>();

        protected void MoveItem(GameObject view)
        {
            foreach (Transform child in view.transform)
            {
                Vector3 mPos = child.position;
                mPos.y = mPos.y - 0.35f;
                child.position = mPos;
            }
        }

        protected void MoveWarItem()
        {
            foreach (Transform child in WarView.transform)
            {
                Vector3 mPos = child.position;
                mPos.y = mPos.y - 0.52f;
                child.position = mPos;
            }
        }
        
		public void AddMailItem(DataMgr.MailData.Mail itemData)
        {
            MoveItem(MailView);
            
            MailItem item = new MailItem();

            item.itemData = itemData;
            item.root = NGUITools.AddChild(MailView, m_mailItem);
            item.root.SetActive(true);
            UIEventListener.Get(item.root).onClick = OnReadMail;

            item.name = PanelTools.Find<UILabel>(item.root, "name");
            item.title = PanelTools.Find<UILabel>(item.root, "title");
            item.id = PanelTools.Find<UILabel>(item.root, "id");
            item.time = PanelTools.Find<UILabel>(item.root, "time");
            item.timeX = PanelTools.Find<UILabel>(item.root, "timeX");
            item.Update();

            m_mailItemList.Add(item);
        }

		public void AddWarItem(DataMgr.MailData.Mail itemData)
        {

            MoveWarItem();
            WarItem item = new WarItem();
            
            item.itemData = itemData;
            item.root = NGUITools.AddChild(WarView, m_warItem);
            item.root.SetActive(true);
            item.name = PanelTools.Find<UILabel>(item.root, "name");
            item.gold = PanelTools.Find<UILabel>(item.root, "gold");
            item.blue = PanelTools.Find<UILabel>(item.root, "blue");
            item.time = PanelTools.Find<UILabel>(item.root, "time");
            item.result = PanelTools.Find<UILabel>(item.root, "result");
            item.Update();

            //m_warGrid.repositionNow = true;

            m_warItemList.Add(item);
        }
        
        protected override void ReleaseImp()
        {
            foreach (WarItem item in m_warItemList)
            {
                item.Release();
            }

            m_warItemList.Clear();

            foreach (WarItem item in m_warItemList)
            {
                item.Release();
            }

            m_mailItemList.Clear();
        }

        public void ClearUI() 
        {
            foreach (Transform child in MailView.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (Transform child in m_warGrid.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void UpdateUI()
        {
			DataManager.getMailData().SetShowToMailPanel();
        }

        public void Write(string name)
        {
            m_root.SetActive(true);
            Mail.SetActive(false);
            ReadMail.SetActive(false);
            WarCommunique.SetActive(false);
            WriteMail.SetActive(true);

            UpdateUI();

            UILabel receiverName = PanelTools.Find<UILabel>(WriteMail, "receiverName");
            receiverName.text = name;

        }

        public void Write(string name, uint id)
        {
            m_root.SetActive(true);
            Mail.SetActive(false);
            ReadMail.SetActive(false);
            WarCommunique.SetActive(false);
            WriteMail.SetActive(true);

            UpdateUI();

            UILabel receiverName = PanelTools.Find<UILabel>(WriteMail, "receiverName");
            receiverName.text = name;

            UILabel idLable = PanelTools.Find<UILabel>(WriteMail, "id");
            idLable.text = name;

        }

        public static string ShowTimeAgo(long time)
        {
            string strTime = "";

            int nDay = (int)(time / 86400);
            int temp = (int)(time % 86400);
            int nHour = (int)(temp / 3600);
            temp = (int)(temp % 3600);
            int nMinute = (int)(temp / 60);

            if (nDay != 0)
            {
                strTime += nDay.ToString() + "D ";
            }

            if (nHour != 0)
            {
                strTime += nHour.ToString() + "H ";
            }

            strTime += nMinute.ToString() + "M AGO";
            

            return strTime;
        }
    }
}
