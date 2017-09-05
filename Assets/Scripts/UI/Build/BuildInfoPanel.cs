using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
namespace UI
{
    public class BuildInfoPanel : PanelBase
    {
        public const PanelID id = PanelID.BuildInfoPanel;

        private GameObject itemPrefab;
        public Build m_build;

        MyUIGrid m_grid;
        UILabel m_labelNameLev;
        GameObject m_go;
        GameObject m_Buttons;
        GameObject m_enterBtn;
        GameObject m_moveBtn;
        GameObject m_levelBtn;
        GameObject m_otherBtn;


        public BuildInfoPanel()
        {

        }

        public override string GetResPath()
        {
            return "buildInfoUI.prefab";
        }

        protected override void onShow()
        {
            return;
        }

        protected override void onHide()
        {
            if (m_build != null)
                m_build.ShowSelectedPanel(false);

            m_build = null;
            ReleaseItem();
        }

        protected override void Initimp(List<GameObject> prefabs)
        {
            m_go = Root;
            m_grid = PanelTools.FindChild(Root, "grid").GetComponent<MyUIGrid>();
            m_labelNameLev = PanelTools.FindChild(Root, "namelev").GetComponent<UILabel>();
            m_Buttons = PanelTools.FindChild(Root, "Buttons");
            m_enterBtn = PanelTools.FindChild(Root, "enter");
            m_moveBtn = PanelTools.FindChild(Root, "info");
            m_levelBtn = PanelTools.FindChild(Root, "level");
            m_otherBtn = PanelTools.FindChild(Root, "other");

            GlobalEventSet.SubscribeEvent(eEventType.ClickBuild, id, this.OnClickBuild);
            GlobalEventSet.SubscribeEvent(eEventType.UpdateQueueTime, this.UpdateHideSelf);
            GlobalEventSet.SubscribeEvent(eEventType.OpenedPanel, this.UpdateHideSelf);


            //UIEventListener.Get(PanelTools.FindChild(Root, "level")).onClick = OnUpLevClick;
            UIEventListener.Get(PanelTools.FindChild(Root, "info")).onClick = OnMoveClick;

            itemPrefab = prefabs[1];
            SetVisible(false);
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        protected void OnRecruitClick(GameObject go)
        {
            SetVisible(false);

            UISoldierPanel soldier = UI.PanelManage.me.GetPanel<UISoldierPanel>(PanelID.SoldierRecruitPanel);
            if (soldier != null)
                soldier.SetVisible(true);
        }

        protected void OnCollegeClick(GameObject go)
        {
            SetVisible(false);
            UITechPanel tech = UI.PanelManage.me.GetPanel<UITechPanel>(PanelID.TechPanel);
            if (tech != null)
                tech.SetVisible(true);
        }

        protected void OnPubClick(GameObject go)
        {
            SetVisible(false);

            RewardPanel rp = PanelManage.me.GetPanel<RewardPanel>(PanelID.RewardPanel);
            if (rp != null)
            {
                rp.ToggleVisible();
            }
        }

        protected void OnHeroTowerClick(GameObject go)
        {
            SetVisible(false);
        }

        protected void OnAccelerateClick(GameObject go)
        {
            if (m_build != null)
            {
				uint idBuildingInQueue = DataManager.getQueueData().GetProgressID(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD);

                if (idBuildingInQueue != 0 && m_build.m_idBuilding == idBuildingInQueue)
                {
					uint deadTime = DataManager.getQueueData().GetDueTime(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD);
                    //float fDueTime = (float)GameInstance.Game.Sgt.EstimateServerTime((long)deadTime);
					float fDueTime = (float)DataManager.getTimeServer().EstimateServerTime((long)deadTime);

                    if (fDueTime > 0)
                    {
                        int nSpendTime = Convert.ToInt32(fDueTime);
                        int nSpendTimeForMinute = (nSpendTime - 1 + 60) / 60;
                        const int nSpendRmbForAccMinute = 2;
                        int nTotalSpendRmb = nSpendRmbForAccMinute * nSpendTimeForMinute;
                        string strTextFormat = "Speed up minute:{0}, spend diamond:{1}";
                        string strText = string.Format(strTextFormat, nSpendTimeForMinute, nTotalSpendRmb);

                        MessageBoxMgr.ShowMessageBox("SpeedUp", strText, OnAccelerateConfirm, null);
                    }
                }
            }

            SetVisible(false);
        }

        protected bool OnAccelerateConfirm(SLG.EventArgs obj)
        {
            // 加速需要的魔石检测暂无
			DataManager.getBuildData().SendBuildAccelerate(DataManager.getQueueData().GetProgressID(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD), 0);
            SetVisible(false);

            return true;
        }

        protected void OnAchievementClick(GameObject go)
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.AchievenmentPanel);

            if (panelBase != null)
            {
                panelBase.ToggleVisible();
            }
        }

        public void OnIllustratedClick(GameObject go)
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.CardHeroListPanel);

            if (panelBase != null)
            {
                panelBase.ToggleVisible();
            }
        }

        protected void OnUpLevClick(GameObject go)
        {
            if (m_build != null)
            {
                PanelID id = PanelID.Null;
                if (m_build.m_idBuildingType == (uint)BuildType.WALL)
                {
                    id = PanelID.WallUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.MINT)
                {
                    id = PanelID.GoldUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.STONE)
                {
                    id = PanelID.MagicStoneUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.LORD_HALL)
                {
                    id = PanelID.mainCityUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.TREASURY)
                {
                    id = PanelID.StoreUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.BARRACKS)
                {
                    id = PanelID.TrainingUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.CELLAR)
                {
                    id = PanelID.CellarUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.TOWN_HALL)
                {
                    id = PanelID.TownHallUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.PANTHEON)
                {
                    id = PanelID.PantheonUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.PUB)
                {
                    id = PanelID.PubUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.SMITHY)
                {
                    id = PanelID.SmithyUpgradePanel;
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.HERO_TOWER)
                {
                    id = PanelID.AuneauUpgradePanel;
                }

                if (id != PanelID.Null)
                {
                    BuildUpgradePanelBase panel = (BuildUpgradePanelBase)PanelManage.me.getPanel(id);
                    if (panel != null)
                    {
                        panel.ShowPanel(m_build);

                        PutBuild.GetInstance.m_selectedBuild = null;
                    }
                }
            }

            SetVisible(false);
        }

        protected void OnMoveClick(GameObject go)
        {
            m_build.ShowMoveTitlePanel();

            SetVisible(false);
        }

        protected void OnCancelClick(GameObject go)
        {
            if (m_build != null)
            {
				uint idBuildingInQueue = DataManager.getQueueData().GetProgressID(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD);

                if (idBuildingInQueue != 0 && m_build.m_idBuilding == idBuildingInQueue)
                {
					DataManager.getBuildData().SendBuildUplevCancel(idBuildingInQueue);
                }
            }

            SetVisible(false);
        }

        public class Item
        {
            public GameObject root; // 根结点
            public UISprite icon; // 图标

            public void Release()
            {
                if (root != null)
                {
                    root.transform.parent = null;
                    GameObject.Destroy(root);
                }
            }
        }

        void AddItem(string icon, UIEventListener.VoidDelegate click)
        {
            Item item = new Item();
            item.root = NGUITools.AddChild(m_grid.gameObject, itemPrefab);
            item.icon = PanelTools.Find<UISprite>(item.root, "Background");
            item.icon.spriteName = icon;
            UIEventListener.Get(item.root).onClick = click;

            m_itemList.Add(item);
        }

        bool OnClickBuild(SLG.EventArgs go)
        {
            Build build = (Build)go.m_obj;

            ShowPanel(build);
            return true;
        }

        bool UpdateHideSelf(SLG.EventArgs go)
        {
            SetVisible(false);
            return true;
        }

        void ShowPanel(Build build)
        {
            if (build != null)
            {
                if (m_build != null)
                    m_build.ShowSelectedPanel(false);

                m_build = build;
                ClearBtns();
                Reposition();
                SetVisible(true);
                m_build.ShowSelectedPanel(true);

                DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_build.m_idBuildingType);

                if (config == null)
                {
                    return;
                }

                m_labelNameLev.text = DataManager.getLanguageMgr().getString(config.name) + "(Level " + m_build.m_cbLev.ToString() + ")";
                ReleaseItem();
                AddItem("infor", OnMoveClick);
                addMoveBtn();
                if (!config.isMove || m_build.m_cbLev == 0)
                {
                    m_moveBtn.SetActive(false);
                }

				uint deadTime = DataManager.getQueueData().GetDueTime(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD);
                //float fDueTime = (float)GameInstance.Game.Sgt.EstimateServerTime((long)deadTime);
				float fDueTime = (float)DataManager.getTimeServer().EstimateServerTime((long)deadTime);
				uint idBuildInQueue = DataManager.getQueueData().GetProgressID(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD);

                if (fDueTime > 0 && idBuildInQueue == m_build.m_idBuilding)
                {
                    AddItem("speedUp", OnAccelerateClick);
                    AddItem("cancel", OnCancelClick);

                    addAccBtn();
                    addCancelBtn();
                }
                else if (m_build.m_idBuildingType != (uint)BuildType.COVENANT_TOWER && m_build.m_idBuildingType != (uint)BuildType.MONUMENT)
                {
                    if (m_build.m_cbLev < config.levels.Length - 1)
                    {
                        AddItem("levelUp", OnUpLevClick);

                        addLevelBtn();
                    }
                    else
                    {
                        AddItem("Maxlevel", OnUpLevClick);
                    }
                }

                if (m_build.m_cbLev == 0)
                {
                    return;
                }

                if (m_build.m_idBuildingType == (uint)BuildType.BARRACKS && !(m_build.m_cbLev < 1 && m_build.m_cbState == 2))
                {
                    AddItem("training", OnRecruitClick);

                    addRecruitBtn();
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.SMITHY)
                {
                    AddItem("college", OnCollegeClick);
                    addSmithyBtn();
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.PUB)
                {
                    AddItem("pub", OnPubClick);
                    addPubBtn();
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.MONUMENT)
                {
                    addMonumentBtn();
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.HERO_TOWER)
                {
                    AddItem("heroTower", OnHeroTowerClick);
                    addDevelopmentBtn();
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.WALL)
                {
                    addDevelopmentBtn();
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.PANTHEON)
                {
                    addDevelopmentBtn();
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.COVENANT_TOWER)
                {
                    addDevelopmentBtn();
                }
                else if (m_build.m_idBuildingType == (uint)BuildType.LORD_HALL)
                {
                    addLordHallBtn();
                }


                m_grid.Reposition();
                m_grid.transform.localPosition = new Vector3(0, -110, 0);
                TweenPosition.Begin(m_grid.gameObject, 0.1f, new Vector3(-400, 80, 0));
                
            }
        }

        protected void ReleaseItem()
        {
            foreach (Item item in m_itemList)
            {
                item.Release();
            }

            m_itemList.Clear();
        }

        private List<Item> m_itemList = new List<Item>();


        public void Reposition()
        {
            if (m_build.m_collider == null)
            {
                return;
            }

            Vector3 posOffset = new Vector3(m_build.transform.position.x, m_build.transform.position.y, m_build.transform.position.z);
            Vector3 posTemp = Camera.main.WorldToViewportPoint(posOffset);
            posTemp = UICamera.currentCamera.ViewportToWorldPoint(posTemp);
            posTemp.z = 0;

            m_Buttons.transform.position = posTemp;
        }

        public void ClearBtns()
        {
            m_levelBtn.gameObject.SetActive(false);
            m_enterBtn.gameObject.SetActive(false);
            m_otherBtn.gameObject.SetActive(false);
        }

        public void addMoveBtn()
        {
            m_moveBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_moveBtn).onClick = OnMoveClick;
            UILabel label = PanelTools.Find<UILabel>(m_moveBtn, "Label");
            label.text = "66350008";
            label.text = DataManager.getLanguageMgr().getString(label.text);
        }
        
        public void addAccBtn()
        {
            m_levelBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_levelBtn).onClick = OnAccelerateClick;
            UILabel label = PanelTools.Find<UILabel>(m_levelBtn, "Label");
            label.text = "66350001";
            label.text = DataManager.getLanguageMgr().getString(label.text);
        }

        public void addCancelBtn()
        {
            m_otherBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_otherBtn).onClick = OnCancelClick;
            UILabel label = PanelTools.Find<UILabel>(m_otherBtn, "Label");
            label.text = "66350002";
            label.text = DataManager.getLanguageMgr().getString(label.text);
        }

        public void addLevelBtn()
        {
            m_levelBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_levelBtn).onClick = OnUpLevClick;
            UILabel label = PanelTools.Find<UILabel>(m_levelBtn, "Label");
            label.text = "66350003";
            label.text = DataManager.getLanguageMgr().getString(label.text);
        }

        public void addRecruitBtn()
        {
            m_enterBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_enterBtn).onClick = OnRecruitClick;
            UILabel label = PanelTools.Find<UILabel>(m_enterBtn, "Label");
            label.text = "66350004";
            label.text = DataManager.getLanguageMgr().getString(label.text);
        }

        public void addPubBtn()
        {
            m_enterBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_enterBtn).onClick = OnPubClick;
            UILabel label = PanelTools.Find<UILabel>(m_enterBtn, "Label");
            label.text = "66350004";
            label.text = DataManager.getLanguageMgr().getString(label.text);
        }

        public void addSmithyBtn()
        {
            m_enterBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_enterBtn).onClick = OnCollegeClick;
            UILabel label = PanelTools.Find<UILabel>(m_enterBtn, "Label");
            label.text = "66350004";
            label.text = DataManager.getLanguageMgr().getString(label.text);
        }

        public void addMonumentBtn()
        {
            m_levelBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_levelBtn).onClick = OnAchievementClick;
            UILabel label = PanelTools.Find<UILabel>(m_levelBtn, "Label");
            label.text = "66350005";
            label.text = DataManager.getLanguageMgr().getString(label.text);

            m_otherBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_otherBtn).onClick = OnDevelopmentClick;
            UILabel label2 = PanelTools.Find<UILabel>(m_otherBtn, "Label");
            label2.text = "66350006";
            label2.text = DataManager.getLanguageMgr().getString(label2.text);


            m_enterBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_enterBtn).onClick = OnDevelopmentClick;
            UILabel label3 = PanelTools.Find<UILabel>(m_enterBtn, "Label");
            label3.text = "66350007";
            label3.text = DataManager.getLanguageMgr().getString(label3.text);
        }

        public void addLordHallBtn()
        {
            m_enterBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_enterBtn).onClick = OnHeroClick;
            UILabel label2 = PanelTools.Find<UILabel>(m_enterBtn, "Label");
            label2.text = "66350004";
            label2.text = DataManager.getLanguageMgr().getString(label2.text);

            m_otherBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_otherBtn).onClick = OnDevelopmentClick;
            UILabel label = PanelTools.Find<UILabel>(m_otherBtn, "Label");
            label.text = "66350010";
            label.text = DataManager.getLanguageMgr().getString(label.text);
        }



        public void addDevelopmentBtn()
        {
            m_enterBtn.gameObject.SetActive(true);
            UIEventListener.Get(m_enterBtn).onClick = OnDevelopmentClick;
            UILabel label = PanelTools.Find<UILabel>(m_enterBtn, "Label");
            label.text = "66350004";
            label.text = DataManager.getLanguageMgr().getString(label.text);
        }

        protected void OnDevelopmentClick(GameObject go)
        {
            //under crazy development！
            string strContent = "66350009";
            strContent = DataManager.getLanguageMgr().getString(strContent);

            MessageBoxMgr.ShowMessageBoxEvent showMessageBoxEvent = new MessageBoxMgr.ShowMessageBoxEvent();
            showMessageBoxEvent.strTitleText = "King of Heroes (Alpha.)";
            showMessageBoxEvent.strContentText = strContent;
            showMessageBoxEvent.bHidePrevious = false;
            showMessageBoxEvent.btnFlag = MESSBOX_FLAG.MB_CONFIRM;
            showMessageBoxEvent.ContentTextClolor = new Color(1.0f, 248.0f / 255.0f, 185.0f / 255.0f);
            MessageBoxMgr.ShowMessageBox(showMessageBoxEvent);
        }

        protected void OnHeroClick(GameObject go)
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.CardIllustratedListPanel);
            if (panelBase != null)
            {
                panelBase.ToggleVisible();
            }
        }
    }
}
