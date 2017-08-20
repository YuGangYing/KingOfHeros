using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
namespace UI
{
    public class BuildMallPanel : PanelBase
    {
        public const PanelID id = PanelID.BuildMallPanel;

        private GameObject itemPrefab;

        public BuildMallPanel()
        {

        }

        public override string GetResPath()
        {
            return "BuildMallUI.prefab";
        }

        UIGrid m_grid;
        UILabel m_labelGold;
        UILabel m_labelStone;
        UILabel m_labelDiamond;
        UILabel m_labelTitle;
        UILabel m_BuildTitle;

        GameObject ItemUI;

        protected override void Initimp(List<GameObject> prefabs)
        {
            UIEventListener.Get(PanelTools.FindChild(Root, "close")).onClick = OnClose;
            UIEventListener.Get(PanelTools.FindChild(Root, "BuidingUIreturn")).onClick = OnBuidingUIreturn;

            m_labelGold = PanelTools.FindChild(Root, "goldNo").GetComponent<UILabel>();
            m_labelStone = PanelTools.FindChild(Root, "stoneNo").GetComponent<UILabel>();
            m_labelDiamond = PanelTools.FindChild(Root, "diamondNo").GetComponent<UILabel>();
            m_labelTitle = PanelTools.FindChild(Root, "diamondNo").GetComponent<UILabel>();
            m_BuildTitle = PanelTools.FindChild(Root, "BuildTitle").GetComponent<UILabel>();
            m_BuildTitle.text = DataManager.getLanguageMgr().getString(m_BuildTitle.text);
            ItemUI = PanelTools.FindChild(Root, "BuildMallItemUI");
            m_grid = PanelTools.FindChild(Root, "BuildGrid").GetComponent<UIGrid>();

            SetVisible(false);
        }

        protected void OnClose(GameObject go)
        {
            SetVisible(false);
        }

        protected void OnBuidingUIreturn(GameObject go)
        {
            SetVisible(false);

            PanelBase panelBase = PanelManage.me.getPanel(PanelID.ShopPanel);
            if (panelBase != null)
            {
                panelBase.SetVisible(true);
            }
        }

        void OnItemClick(GameObject go)
        {
            UIEventListener listener = go.GetComponent<UIEventListener>();
            if (listener == null)
            {
                Logger.LogError("OnItemClick! UIEventListener == null");
                return;
            }

            Item item = (Item)listener.parameter;

            if (item == null)
            {
                Logger.LogError("OnItemClick! item == null");
                return;
            }

            if (item.itemData.count < item.itemData.maxCount && DataManager.getBuildData().GetLordHallLevel() >= item.itemData.CastleLevel)
            {
                SetVisible(false);
                PutBuild.me.OnPutBuild(item.itemData.id);
            }
            else
            {
				//DataManager.getBuildData().ShowBuildOperaRetText(Packet.BUILDING_RET.BUILDING_RET_BUILD_MORE);
            }
        }

        public class Item
        {
            public GameObject root; // 根结点
            public UILabel name; // 名字
            public UILabel money; // 钱1
            public UISprite icon; // 图标
            public UISprite cover; //遮盖
            public UILabel time; // 消耗的时间
            public UILabel count;// 拥有建筑的数量
            public UILabel desc;//建筑描述

            public void Release()
            {
                if (root != null)
                {
                    GameObject.Destroy(root);
                }
            }

            public ItemData itemData;

            public void Update()
            {
                if (itemData != null)
                {
                    name.text = DataManager.getLanguageMgr().getString(itemData.name);
                    money.text = itemData.money.ToString();
                    time.text = itemData.time.ToString() + "s";
                    count.text = itemData.count.ToString() + "/" + itemData.maxCount.ToString();
                    desc.text = DataManager.getLanguageMgr().getString(itemData.desc);
                    icon.spriteName = itemData.icon;

                    if (itemData.count < itemData.maxCount)
                    {
                        cover.gameObject.SetActive(false);
                    }
                    else
                    {
                        cover.gameObject.SetActive(true);
                    }

                    int nLevel =  DataManager.getBuildData().GetLordHallLevel();

                    if (nLevel < itemData.CastleLevel)
                    {
                        cover.gameObject.SetActive(true);
                    }
                    
                }
            }
        }

        public class ItemData
        {
            public int id; // 建筑ID
            public int name;
            public int money;
            public string icon;
            public float time;
            public int count;
            public int maxCount;
            public int desc;
            public int CastleLevel;
        }

        // 添加一个选项
        public void AddItem(ItemData itemData)
        {
            Item item = new Item();
            item.root = NGUITools.AddChild(m_grid.gameObject, ItemUI);
            item.name = PanelTools.Find<UILabel>(item.root, "buildName");

            item.itemData = itemData;
            item.money = PanelTools.Find<UILabel>(item.root, "goldNo");
            item.time = PanelTools.Find<UILabel>(item.root, "timeNo");
            item.icon = PanelTools.Find<UISprite>(item.root, "Icon");
            item.cover = PanelTools.Find<UISprite>(item.root, "BuildItemCover");
            item.count = PanelTools.Find<UILabel>(item.root, "count");
            item.desc = PanelTools.Find<UILabel>(item.root, "particulars");

            item.root.SetActive(true);
            UIEventListener listener = UIEventListener.Get(item.root);
            listener.onClick = OnItemClick;
            listener.parameter = item;

            m_itemList.Add(item);

            item.Update();
            m_grid.repositionNow = true;
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        protected override void onShow()
        {
            base.onShow();

            /*GameInstance.Item usx = Game.Sgt.QueryItem(0x8000);
            m_labelGold.text = string.Format("{0}", Game.Sgt.QueryItemData(1, usx).Data[0]);
            m_labelStone.text = string.Format("{0}", Game.Sgt.QueryItemData(2, usx).Data[0]);
            m_labelDiamond.text = string.Format("{0}", Game.Sgt.QueryItemData(3, usx).Data[0]);*/

			m_labelGold.text = string.Format("{0}", RewardPanel.getCoins().ToString());
			m_labelStone.text = string.Format("{0}", RewardPanel.getMagicStones().ToString());
			m_labelDiamond.text = string.Format("{0}", RewardPanel.getDimonds().ToString());				
			
        }

        protected override void ReleaseImp()
        {
            foreach (Item item in m_itemList)
            {
                item.Release();
            }

            m_itemList.Clear();
        }

        private List<Item> m_itemList = new List<Item>();
    }
}