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
    public class FriendPanel : PanelBase
    {
        public const PanelID id = PanelID.FriendPanel;

        GameObject m_root;
        GameObject friendItem;
        GameObject viewFriend;

        UILabel input;

        public override string GetResPath()
        {
            return "FriendUI.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        protected override void Initimp(List<GameObject> prefabs)
        {
            m_root = Root;


            SetVisible(false);
        }

        public void init(GameObject root)
        {

            m_root = root;
            UIEventListener.Get(PanelTools.FindChild(m_root, "findBtn")).onClick = OnFindBtn;

            input = PanelTools.Find<UILabel>(m_root, "input");
            friendItem = UISoldierPanel.findChild(m_root, "ScrollView,friendItem");
            viewFriend = UISoldierPanel.findChild(m_root, "ScrollView,view");


//             for (int i = 0; i < 30; ++i)
//             {
//                 FriendMgr.FriendInfo item = new FriendMgr.FriendInfo();
//                 item.szFriendName = "测试" + i.ToString();
//                 item.idFriendUser = (uint)i;
//                 item.nFriendStatus = 1;
// 
//                 AddItem(item);
//             }

        }


        protected void OnFindBtn(GameObject go)
        {
            string strName = input.text;

            if (strName == "" || strName == null)
            {
                return;
            }

            ClearItem();

			foreach (DataMgr.FriendData.FriendInfo info in m_fInfoList)
            {
                int nIndex = info.szFriendName.IndexOf(strName);
                if (nIndex != -1)
                {
                    ReAddItem(info);
                }
            }
        }

        protected void OnViewBtn(GameObject go)
        {
            UILabel idLabel = PanelTools.Find<UILabel>(go.transform.parent.gameObject, "id");
            uint id = uint.Parse(idLabel.text);

			DataManager.getFriendData().EnterCity(id);
        }

        protected void OnMailBtn(GameObject go)
        {
            UILabel idLabel = PanelTools.Find<UILabel>(go.transform.parent.gameObject, "id");
            uint id = uint.Parse(idLabel.text);

            UILabel nameLabel = PanelTools.Find<UILabel>(go.transform.parent.gameObject, "name");
            string name = nameLabel.text;

			DataManager.getMailData().WriteMail(name, id);
        }

        protected void OnDelBtn(GameObject go)
        {
            UILabel idLabel = PanelTools.Find<UILabel>(go.transform.parent.gameObject, "id");
            uint id = uint.Parse(idLabel.text);

			DataManager.getFriendData().DelFriend(id);
        }

        protected void acceptBtn(GameObject go)
        {
            UILabel idLabel = PanelTools.Find<UILabel>(go.transform.parent.gameObject, "id");
            uint id = uint.Parse(idLabel.text);

			DataManager.getFriendData().AcceptApply(id);
        }

        protected void refuseBtn(GameObject go)
        {
            UILabel idLabel = PanelTools.Find<UILabel>(go.transform.parent.gameObject, "id");
            uint id = uint.Parse(idLabel.text);

			DataManager.getFriendData().RefuseApply(id);
        }

        public class FriendItem
        {
            public GameObject root;
            public UILabel name;
            public UILabel fc;
            public UILabel id;
            public UILabel apply;
            public GameObject accept;
            public GameObject refuse;
			public DataMgr.FriendData.FriendInfo itemData;

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
                    name.text = itemData.szFriendName;
                    fc.text = itemData.idFriendUser.ToString();
                    id.text = itemData.idFriendUser.ToString();

                    if (itemData.nFriendStatus == (byte)DataMgr.FriendData.FRIEND_STATUS.FRIEND_STATUS_BE_APPLIED)
                    {
                        accept.SetActive(true);
                        refuse.SetActive(true);
                    }
                    else
                    {
                        accept.SetActive(false);
                        refuse.SetActive(false);
                    }

                    if (itemData.nFriendStatus == (byte)DataMgr.FriendData.FRIEND_STATUS.FRIEND_STATUS_APPLY)
                    {
                        apply.text = "apply";
                    }
                    else
                    {
                        apply.text = "";
                    }
                }
            }
        }

        protected void MoveItem()
        {
            viewFriend = UISoldierPanel.findChild(m_root, "ScrollView,view");
            
            if (0 == viewFriend.transform.childCount)
            {
                return;
            }
            
            foreach (Transform child in viewFriend.transform)
            {
                Vector3 mPos = child.position;
                mPos.y = mPos.y - 0.4f;
                child.position = mPos;
            }
        }

		private List<DataMgr.FriendData.FriendInfo> m_fInfoList = new List<DataMgr.FriendData.FriendInfo>();

        //public void AddItem(FriendMgr.FriendInfo itemData)
		public void AddItem(DataMgr.FriendData.FriendInfo itemData)
        {
            MoveItem();

            friendItem = UISoldierPanel.findChild(m_root, "ScrollView,friendItem");

            FriendItem item = new FriendItem();
            item.itemData = itemData;
            item.root = NGUITools.AddChild(viewFriend, friendItem);
            item.root.SetActive(true);
            UIEventListener.Get(PanelTools.FindChild(item.root, "viewBtn")).onClick = OnViewBtn;
            UIEventListener.Get(PanelTools.FindChild(item.root, "mailBtn")).onClick = OnMailBtn;
            UIEventListener.Get(PanelTools.FindChild(item.root, "delBtn")).onClick = OnDelBtn;
            UIEventListener.Get(PanelTools.FindChild(item.root, "acceptBtn")).onClick = acceptBtn;
            UIEventListener.Get(PanelTools.FindChild(item.root, "refuseBtn")).onClick = refuseBtn;
            item.name = PanelTools.Find<UILabel>(item.root, "name");
            item.fc = PanelTools.Find<UILabel>(item.root, "fcNO");
            item.id = PanelTools.Find<UILabel>(item.root, "id");
            item.accept = PanelTools.FindChild(item.root, "acceptBtn");
            item.refuse = PanelTools.FindChild(item.root, "refuseBtn");
            item.apply = PanelTools.Find<UILabel>(item.root, "apply");
            item.Update();

            m_fInfoList.Add(itemData);
        }

		private void ReAddItem(DataMgr.FriendData.FriendInfo itemData)
        {
            MoveItem();

            FriendItem item = new FriendItem();
            item.itemData = itemData;
            item.root = NGUITools.AddChild(viewFriend.gameObject, friendItem);
            item.root.SetActive(true);
            UIEventListener.Get(PanelTools.FindChild(item.root, "viewBtn")).onClick = OnViewBtn;
            UIEventListener.Get(PanelTools.FindChild(item.root, "mailBtn")).onClick = OnMailBtn;
            UIEventListener.Get(PanelTools.FindChild(item.root, "delBtn")).onClick = OnDelBtn;
            UIEventListener.Get(PanelTools.FindChild(item.root, "acceptBtn")).onClick = acceptBtn;
            UIEventListener.Get(PanelTools.FindChild(item.root, "refuseBtn")).onClick = refuseBtn;
            item.name = PanelTools.Find<UILabel>(item.root, "name");
            item.fc = PanelTools.Find<UILabel>(item.root, "fcNO");
            item.id = PanelTools.Find<UILabel>(item.root, "id");
            item.accept = PanelTools.FindChild(item.root, "acceptBtn");
            item.refuse = PanelTools.FindChild(item.root, "refuseBtn");
            item.Update();
        }

        public void ClearItem()
        {
            if (0 == viewFriend.transform.childCount)
            {
                return;
            }
            
            foreach (Transform child in viewFriend.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void UpdateUI()
        {
            ClearItem();
            
			DataManager.getFriendData().SetShowToPanel();
        }

        protected override void onShow()
        {
			DataManager.getFriendData().QueryFriend();

            base.onShow();
        }
    }
}
