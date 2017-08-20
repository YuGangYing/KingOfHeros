using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using stw;
using SLG;
using DataMgr;

namespace UI
{
    public class ChatPanel : PanelBase
    {
        public const PanelID id = PanelID.ChatPanel;

        public enum PAGE
        {
            WORLD = 0,
            PRIVATE = 1,
            FRIENDS = 2,
            UNION = 3,
        }

        GameObject m_root;
        GameObject Details;
        GameObject World;
        GameObject Private;
        GameObject m_worldItem;
        GameObject m_privateItem;
        GameObject worldView;
        GameObject privateView;
        GameObject Friend;
        UILabel m_worldInput;
        UILabel m_privateInput;
        UILabel m_Receiver;
        UILabel m_lableError;
        UILabel m_privateCount;
        UISprite worldBtnBg;
        UISprite privateBtnBg;
        UISprite friendBtnBg;

        bool isDetailShow = false;
        public PAGE m_page = PAGE.WORLD;
        Int64 WorldTalkTime = 30;
        Int64 PrivateTalkTime = 5;
        public int m_mCount = 0;

        public override string GetResPath()
        {
            return "ChatUI.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        protected override void Initimp(List<GameObject> prefabs)
        {
            UIEventListener.Get(PanelTools.FindChild(Root, "closeBtn")).onClick = OnClose;
            UIEventListener.Get(PanelTools.FindChild(Root, "WorldBtn")).onClick = OnWorldBtn;
            UIEventListener.Get(PanelTools.FindChild(Root, "PrivatBtn")).onClick = OnPrivateBtn;
            UIEventListener.Get(PanelTools.FindChild(Root, "FriendBtn")).onClick = OnFriendBtn;

            m_root = Root;
            m_lableError = PanelTools.Find<UILabel>(m_root, "LabelError");
            m_privateCount = PanelTools.Find<UILabel>(m_root, "Mcount");
            GameObject WorldBtn = UISoldierPanel.findChild(m_root, "WorldBtn");
            worldBtnBg = PanelTools.Find<UISprite>(WorldBtn, "Background");
            GameObject PrivateBtn = UISoldierPanel.findChild(m_root, "PrivatBtn");
            privateBtnBg = PanelTools.Find<UISprite>(PrivateBtn, "Background");
            GameObject FriendBtn = UISoldierPanel.findChild(m_root, "FriendBtn");
            friendBtnBg = PanelTools.Find<UISprite>(FriendBtn, "Background");

            Details = UISoldierPanel.findChild(m_root, "Details");
            UIEventListener.Get(PanelTools.FindChild(Details, "PrivateBtn")).onClick = OnWhisper;
            UIEventListener.Get(PanelTools.FindChild(Details, "ViewBtn")).onClick = OnViewBtn;
            UIEventListener.Get(PanelTools.FindChild(Details, "AddBtn")).onClick = OnAddBtn;

            World = UISoldierPanel.findChild(Root, "World");
            UIEventListener.Get(PanelTools.FindChild(World, "SendBtn")).onClick = OnWorldSendBtn;
            m_worldInput = PanelTools.Find<UILabel>(World, "input");
            GameObject worldScrollView = UISoldierPanel.findChild(World, "ScrollView");
            worldView = UISoldierPanel.findChild(worldScrollView, "view");
            m_worldItem = UISoldierPanel.findChild(worldScrollView, "ChatInfo");

            Private = UISoldierPanel.findChild(Root, "Private");
            UIEventListener.Get(PanelTools.FindChild(Private, "SendBtn")).onClick = OnPrivateSendBtn;
            m_privateInput = PanelTools.Find<UILabel>(Private, "input");
            m_Receiver = PanelTools.Find<UILabel>(Private, "ToName");
            GameObject privateScrollView = UISoldierPanel.findChild(Private, "ScrollView");
            privateView = UISoldierPanel.findChild(privateScrollView, "view");
            m_privateItem = UISoldierPanel.findChild(privateScrollView, "ChatInfo");

            Friend = NGUITools.AddChild(m_root, prefabs[1]);
            Friend.SetActive(false);
			DataManager.getFriendData().m_friendPanel.init(Friend);

            GlobalEventSet.SubscribeEvent(eEventType.ShowError, id, this.OnShowError);

            ResetDate();
            SetVisible(false);
        }

        protected void OnClose(GameObject go)
        {
            SetVisible(false);
        }

        protected void OnFriendBtn(GameObject go)
        {
//             FriendPanel friendPanel = UI.PanelManage.me.GetPanel<FriendPanel>(PanelID.FriendPanel);
// 
//             friendPanel.SetVisible(true);
            SetPage(PAGE.FRIENDS);
        }

        protected void OnWorldBtn(GameObject go)
        {
            SetPage(PAGE.WORLD);
        }
        
        protected void OnPrivateBtn(GameObject go)
        {
            SetPage(PAGE.PRIVATE);

            SetPrivateCount(0);
        }

        protected void OnDetails(GameObject go)
        {
            Details.transform.position = go.transform.position;

            UILabel name = PanelTools.Find<UILabel>(go, "name");

            if (name.text == "")
            {
                return;
            }

            UILabel nameDetails = PanelTools.Find<UILabel>(Details, "name");

            nameDetails.text = name.text;

            if (isDetailShow)
            {
                isDetailShow = false;
            }
            else
            {
                isDetailShow = true;
            }
            
            Details.SetActive(isDetailShow);
        }

        protected void OnWhisper(GameObject go)
        {
            UILabel name = PanelTools.Find<UILabel>(go.transform.parent.gameObject, "name");

            SetPage(PAGE.PRIVATE);

            m_Receiver.text = name.text;
            Details.SetActive(false);
        }

        protected void OnViewBtn(GameObject go)
        {
//             UILabel name = Find<UILabel>(go.transform.parent.gameObject, "name");
// 
//             uint id = uint.Parse(name.text);
//             MailMgr.me.WriteMail(name.text        
        }
        protected void OnAddBtn(GameObject go)
        {
            UILabel name = PanelTools.Find<UILabel>(go.transform.parent.gameObject, "name");

            uint id = uint.Parse(name.text);

			DataManager.getFriendData().ApplyFriend(id);
        }

        protected void SetPage(PAGE page)
        {
            m_page = page;
            Details.SetActive(false);

            switch (m_page)
            {
                case PAGE.WORLD:
                    worldBtnBg.spriteName = "ChatUIChosenTagBg";
                    privateBtnBg.spriteName = "ChatUITagBg";
                    friendBtnBg.spriteName = "ChatUITagBg";
                    World.SetActive(true);
                    Private.SetActive(false);
                    Friend.SetActive(false);
                    break;
                case PAGE.PRIVATE:
                    worldBtnBg.spriteName = "ChatUITagBg";
                    privateBtnBg.spriteName = "ChatUIChosenTagBg";
                    friendBtnBg.spriteName = "ChatUITagBg";
                    World.SetActive(false);
                    Private.SetActive(true);
                    Friend.SetActive(false);
                    break;
                case PAGE.FRIENDS:
                    worldBtnBg.spriteName = "ChatUITagBg";
                    privateBtnBg.spriteName = "ChatUITagBg";
                    friendBtnBg.spriteName = "ChatUIChosenTagBg";
                    World.SetActive(false);
                    Private.SetActive(false);
                    Friend.SetActive(true);
					DataManager.getFriendData().QueryFriend();
                    break;
            }
        }

        protected void OnWorldSendBtn(GameObject go)
        {
			int nLevel = DataManager.getBuildData().GetBuildLev(DataMgr.BuildType.LORD_HALL);

            if (nLevel < 5)
            {
                SLG.GlobalEventSet.FireEvent(SLG.eEventType.ShowError, PanelID.ChatPanel, new SLG.EventArgs("城堡等级必须达到5级"));
                return;
            }

            Int64 nowTime = DataManager.getTimeServer().EstimateServerTime(DataManager.getTimeServer().ServerTime);

            if (WorldTalkTime - nowTime < 30)
            {
                SLG.GlobalEventSet.FireEvent(SLG.eEventType.ShowError, PanelID.ChatPanel, new SLG.EventArgs("发言间隔30秒"));
                return;
            }

            string strContent = m_worldInput.text;

            if (strContent == "" || strContent == null)
            {
                return;
            }

            m_worldInput.text = "";
            
            MoveWorldItem();

            string name = RewardPanel.UserName;// DataCenterMgr.UserInfo.szName;

            ChatmsgData.TalkInfo itemData = new ChatmsgData.TalkInfo();
            itemData.szSender = name;
            itemData.szWords = strContent;
            //itemData.time = (uint)DataCenterMgr.ServerTime;

            WorldItem item = new WorldItem();
            item.itemData = itemData;
            item.root = NGUITools.AddChild(worldView.gameObject, m_worldItem);
            item.root.SetActive(true);
            UIEventListener.Get(item.root).onClick = OnDetails;
            item.name = PanelTools.Find<UILabel>(item.root, "name");
            item.me = PanelTools.Find<UILabel>(item.root, "name2");
            item.timeLeft = PanelTools.Find<UILabel>(item.root, "sendTime1");
            item.timeRight = PanelTools.Find<UILabel>(item.root, "sendTime2");
            item.content = PanelTools.Find<UILabel>(item.root, "content");
            item.Update();

            DataManager.getChatmsgData().SendTalk(0, name, "", strContent);

            WorldTalkTime = nowTime;
        }

        protected void OnPrivateSendBtn(GameObject go)
        {
            Int64 nowTime = DataManager.getTimeServer().EstimateServerTime(DataManager.getTimeServer().ServerTime);

            if (PrivateTalkTime - nowTime < 5)
            {
                SLG.GlobalEventSet.FireEvent(SLG.eEventType.ShowError, PanelID.ChatPanel, new SLG.EventArgs("发言间隔5秒"));
                return;
            }
            
            string strContent = m_privateInput.text;
            string strName = m_Receiver.text;
            string name = RewardPanel.UserName;// DataCenterMgr.UserInfo.szName;

            if (strContent == "" || strContent == null || strName == "" || strName == null)
            {
                return;
            }

            m_worldInput.text = "";

            MovePrivateItem();

            ChatmsgData.TalkInfo itemData = new ChatmsgData.TalkInfo();
            itemData.szSender = RewardPanel.UserName;// DataCenterMgr.UserInfo.szName;
            itemData.szWords = strContent;

            WorldItem item = new WorldItem();
            item.itemData = itemData;
            item.root = NGUITools.AddChild(privateView.gameObject, m_privateItem);
            item.root.SetActive(true);
            UIEventListener.Get(item.root).onClick = OnDetails;
            item.name = PanelTools.Find<UILabel>(item.root, "name");
            item.me = PanelTools.Find<UILabel>(item.root, "name2");
            item.timeLeft = PanelTools.Find<UILabel>(item.root, "sendTime1");
            item.timeRight = PanelTools.Find<UILabel>(item.root, "sendTime2");
            item.content = PanelTools.Find<UILabel>(item.root, "content");
            item.Update();

            DataManager.getChatmsgData().SendTalk(1, name, strName, strContent);

            PrivateTalkTime = nowTime;
        }

        public class ChatData
        {
            public uint id;
            public string name;
            public string content;
            public uint time;
        }
        
        public class WorldItem
        {
            public GameObject root;
            public UILabel name;
            public UILabel me;
            public UILabel content;
            public UILabel timeLeft;
            public UILabel timeRight;
            public UILabel id;
            public UISprite icon;
            public DataMgr.ChatmsgData.TalkInfo itemData;

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
                    if (itemData.szSender == RewardPanel.UserName)// DataCenterMgr.UserInfo.szName)
                    {
                        me.text = itemData.szSender;
                        name.text = "";
                        name.gameObject.SetActive(false);
                        timeLeft.gameObject.SetActive(false);
                    }
                    else
                    {
                        name.text = itemData.szSender;
                        me.gameObject.SetActive(false);
                        timeRight.gameObject.SetActive(false);
                    }
                    
                    content.text = itemData.szWords;
                    DateTime dt = DateTime.Now;
                    timeLeft.text = dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();
                    timeRight.text = dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();
                }
            }
        }

        private List<WorldItem> m_worldItemList = new List<WorldItem>();

        public void AddWorldItem(DataMgr.ChatmsgData.TalkInfo itemData)
        {
            MoveWorldItem();
            
            WorldItem item = new WorldItem();

            item.itemData = itemData;
            item.root = NGUITools.AddChild(worldView.gameObject, m_worldItem);
            item.root.SetActive(true);
            UIEventListener.Get(item.root).onClick = OnDetails;
            item.name = PanelTools.Find<UILabel>(item.root, "name");
            item.me = PanelTools.Find<UILabel>(item.root, "name2");
            item.timeLeft = PanelTools.Find<UILabel>(item.root, "sendTime1");
            item.timeRight = PanelTools.Find<UILabel>(item.root, "sendTime2");
            item.content = PanelTools.Find<UILabel>(item.root, "content");
            item.Update();

            //m_worldItemList.Add(item);
        }

        public void AddPrivateItem(DataMgr.ChatmsgData.TalkInfo itemData)
        {
            MovePrivateItem();

            WorldItem item = new WorldItem();

            item.itemData = itemData;
            item.root = NGUITools.AddChild(privateView.gameObject, m_privateItem);
            item.root.SetActive(true);
            UIEventListener.Get(item.root).onClick = OnDetails;
            item.name = PanelTools.Find<UILabel>(item.root, "name");
            item.me = PanelTools.Find<UILabel>(item.root, "name2");
            item.timeLeft = PanelTools.Find<UILabel>(item.root, "sendTime1");
            item.timeRight = PanelTools.Find<UILabel>(item.root, "sendTime2");
            item.content = PanelTools.Find<UILabel>(item.root, "content");
            item.Update();

            //m_worldItemList.Add(item);
        }

        protected void MoveWorldItem()
        {
            int nCount = worldView.transform.childCount;

            bool isDel = true;

            if (nCount < 50)
            {
                isDel = false;
            }

            foreach (Transform child in worldView.transform)
            {
                Vector3 mPos = child.position;

                mPos.y = mPos.y - 0.4f;

                child.position = mPos;

                if (isDel && child)
                {
                    GameObject.Destroy(child.gameObject);
                    isDel = false;
                }
            }
        }

        protected void MovePrivateItem()
        {
            int nCount = privateView.transform.childCount;

            bool isDel = true;

            if (nCount < 50)
            {
                isDel = false;
            }

            foreach (Transform child in privateView.transform)
            {
                Vector3 mPos = child.position;

                mPos.y = mPos.y - 0.4f;

                child.position = mPos;

                if (isDel && child)
                {
                    GameObject.Destroy(child.gameObject);
                    isDel = false;
                }
            }
        }

        protected void ClearWorldList()
        {
            foreach (WorldItem item in m_worldItemList)
            {
                item.Release();
            }

            m_worldItemList.Clear();
        }

        public bool OnShowError(SLG.EventArgs obj)
        {
            string strError = (string)obj.m_obj;
            m_lableError.text = strError;
            m_lableError.gameObject.SetActive(true);
            return false;
        }

        public void SetPrivateCount(int nCount)
        {
            m_mCount = nCount;

            if (m_mCount > 0)
            {
                m_privateCount.text = m_mCount.ToString();
                m_privateCount.gameObject.SetActive(true);
            }
            else
            {
                m_privateCount.gameObject.SetActive(false);
            }
        }

        public void ResetDate()
        {
            if (0 == DataManager.getChatmsgData().talkList.Count)
            {
                return;
            }

            foreach (ChatmsgData.TalkInfo ti in DataManager.getChatmsgData().talkList)
            {
                if (ti.unTxtAttribute == 0)
                {
                    AddWorldItem(ti);
                }

                if (ti.unTxtAttribute == 1)
                {
                    AddPrivateItem(ti);
                }
            }
        }
    }
}
