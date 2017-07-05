using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataMgr;
using System.Text;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
namespace UI
{
    public class AchievenmentPanel : PanelBase
    {
        public const PanelID id = PanelID.AchievenmentPanel;

        public AchievenmentPanel()
        {

        }

        public override string GetResPath()
        {
            return "AchievenmentUI.prefab";
        }

        UIGrid m_achievenmentListGrid;
        private GameObject m_achievenmentListGridItemPrefab;

        protected override void Initimp(List<GameObject> prefabs)
        {
            UIEventListener.Get(PanelTools.FindChild(Root, "close")).onClick = OnClose;
            GameObject objAchievementList = PanelTools.FindChild(Root, "AchievenmentList");
            m_achievenmentListGridItemPrefab = prefabs[1];
            m_achievenmentListGrid = PanelTools.FindChild(Root, "AchievenmentListGrid").AddComponent<UIGrid>();
            m_achievenmentListGrid.arrangement = UIGrid.Arrangement.Vertical;
            Bounds itemBounds = NGUIMath.CalculateRelativeWidgetBounds(m_achievenmentListGridItemPrefab.transform, m_achievenmentListGridItemPrefab.transform, true);
            m_achievenmentListGrid.cellHeight = itemBounds.size.y;
            m_achievenmentListGrid.cellWidth = itemBounds.size.x;
            SetVisible(false);

            _FlushTableText();
        }

        void _FlushTableText()
        {
            UICardMgr.setLabelText(Root, "Child,AchievenmentUI,name", 14023);
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        protected void OnClose(GameObject go)
        {
            SetVisible(false);
        }

        protected override void onShow()
        {
            base.onShow();
			DataManager.getAchievementData().SendQueryAchievenment();
        }

        protected override void onHide()
        {
            base.onHide();
            ReleaseItem();
        }

        public void OnAwardAchievenment(GameObject go)
        {
            UIEventListener listener = go.GetComponent<UIEventListener>();

            if (listener == null)
            {
                Logger.LogError("OnAchievenmentListItemClick! UIEventListener == null");
                return;
            }

            uint idAchievenmentType = (uint)listener.parameter;
			DataManager.getAchievementData().AwardAchievenment(idAchievenmentType);
        }

        public class AchievenmentListItem
        {
            public GameObject root; // 根结点
            public UILabel labelAchievenmentName; // 名字
            public UILabel labelAchievenmentCondiction; // 条件
            public UILabel labelAchievenmentReward; // 奖励
            public UISprite spriteProcess; // 进度条
            public UILabel labelProcessTip; // 进度提示
            public UIButton btnAward; // 领取
            public uint idAchievenmentType; // 成就类型ID

            public void Release()
            {
                if (root != null)
                {
                    GameObject.Destroy(root);
                }
            }

            public void Update()
            {
				DataMgr.Achievenment achievenment = DataManager.getAchievementData().QueryAchievenment(idAchievenmentType);

                if (null == achievenment)
                {
                    return;
                }

                ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_ACHIEVENMENT);

                if (config == null)
                {
                    return;
                }

                ConfigRow row = config.getRow(CFG_ACHIEVENMENT.ACHIEVENMENT_TYPEID, (int)this.idAchievenmentType,
                    CFG_ACHIEVENMENT.STEP, achievenment.nStep);

                if (row == null)
                {
                    return;
                }

                string str = "";
                int strId = row.getIntValue(CFG_ACHIEVENMENT.NAME_ID);
                labelAchievenmentName.text = DataMgr.DataManager.getLanguageMgr().getString(strId);
                strId = row.getIntValue(CFG_ACHIEVENMENT.DESC_ID);
                string strAchievenmentCondictionFormat = DataMgr.DataManager.getLanguageMgr().getString(strId);

                int nNeedAccumulateValue = row.getIntValue(CFG_ACHIEVENMENT.NEED_ACCUMULATE_VALUE);
                labelAchievenmentCondiction.text = string.Format(strAchievenmentCondictionFormat, achievenment.nAccumulateValue, nNeedAccumulateValue);

                int nRewardMoney = row.getIntValue(CFG_ACHIEVENMENT.REWARD_MONEY);
                int nRewardStone = row.getIntValue(CFG_ACHIEVENMENT.REWARD_MAGICSTONE);
                int nRewardRMB = row.getIntValue(CFG_ACHIEVENMENT.REWARD_RMB);
                string strRewardFormat = "{0}:{1} ";
                StringBuilder strReward = new StringBuilder();

                if (nRewardMoney > 0)
                {
                    strReward.Append(string.Format(strRewardFormat, DataManager.getLanguageMgr().getString("money"), nRewardMoney));
                }

                if (nRewardStone > 0)
                {
                    strReward.Append(string.Format(strRewardFormat, DataManager.getLanguageMgr().getString("stone"), nRewardStone));
                }

                if (nRewardRMB > 0)
                {
                    strReward.Append(string.Format(strRewardFormat, DataManager.getLanguageMgr().getString("diamond"), nRewardRMB));
                }

                labelAchievenmentReward.text = strReward.ToString();
                spriteProcess.fillAmount = Mathf.Min(nNeedAccumulateValue <= 0 ? 0.0f : (float)achievenment.nAccumulateValue / nNeedAccumulateValue, 1.0f);

                string strProcessTipFormat = "{0}/{1}";
                labelProcessTip.text = string.Format(strProcessTipFormat, achievenment.nAccumulateValue, nNeedAccumulateValue);

                if (achievenment.CanAward())
                {
                    btnAward.enabled = true;
                }
                else
                {
                    btnAward.enabled = false;
                }

                UICardMgr.setLabelText(btnAward.gameObject, "Label", 14024);
            }
        }

        private List<AchievenmentListItem> m_itemList = new List<AchievenmentListItem>();

        protected AchievenmentListItem QueryAchievenmentListItem(uint idAchievenmentType)
        {
            return m_itemList.Find(delegate(AchievenmentListItem item)
                {
                    return item.idAchievenmentType == idAchievenmentType;
                });
        }

        // 添加一个选项
        public void AddItem(uint idAchievenmentType)
        {
            AchievenmentListItem item = new AchievenmentListItem();
            item.root = NGUITools.AddChild(m_achievenmentListGrid.gameObject, m_achievenmentListGridItemPrefab);
            item.idAchievenmentType = idAchievenmentType;
            item.labelAchievenmentName = PanelTools.Find<UILabel>(item.root, "AchievenmentName");
            item.labelAchievenmentCondiction = PanelTools.Find<UILabel>(item.root, "AchievenmentCondiction");
            item.labelAchievenmentReward = PanelTools.Find<UILabel>(item.root, "AchievenmentReward");
            item.spriteProcess = PanelTools.Find<UISprite>(item.root, "ProcessBar/Process");
            item.labelProcessTip = PanelTools.Find<UILabel>(item.root, "ProcessBar/Tip");
            item.btnAward = PanelTools.Find<UIButton>(item.root, "Award");
            UIEventListener listener = UIEventListener.Get(item.btnAward.gameObject);
            listener.onClick = OnAwardAchievenment;
            listener.parameter = idAchievenmentType;

            m_itemList.Add(item);
            item.Update();
            m_achievenmentListGrid.repositionNow = true;
        }

        public void UpdateItem(uint idAchievenmentType)
        {
            AchievenmentListItem item = QueryAchievenmentListItem(idAchievenmentType);

            if (null == item)
            {
                return;
            }

            item.Update();
        }

        public void ReleaseItem()
        {
            ReleaseImp();
        }

        protected override void ReleaseImp()
        {
            foreach (AchievenmentListItem item in m_itemList)
            {
                item.Release();
            }

            m_itemList.Clear();
        }
    }
}