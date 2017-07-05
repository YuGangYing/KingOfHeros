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
    public class BattlefieldPanel : PanelBase
    {
        public const PanelID id = PanelID.BattlefieldPanel;

        public override string GetResPath()
        {
            return "Battlefield.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        GameObject m_root;
        GameObject BattleFieldList;
        GameObject BattleItem;
        GameObject BattleGrid;
        GameObject BattleFieldItem;
        GameObject BattleFieldGrid;

        UILabel m_expValue;
        UILabel m_goldValue;
        UILabel m_des;
        UILabel m_title;
        UILabel m_reward;

        UISprite RightSprite;
        UISprite LeftSprite;
        UISprite map;

        UIScrollBar BattleScrollBar;
        UIScrollBar BattleFieldScrollBar;
        int m_SelectFieldId = 0;
        int m_SelectBattleId = 0;

        protected override void Initimp(List<GameObject> prefabs)
        {
            UIEventListener.Get(PanelTools.FindChild(Root, "closeBtn")).onClick = OnClose;
            
            m_root = Root;

            GameObject BattleList = UISoldierPanel.findChild(m_root, "BattleList1");

            BattleItem = UISoldierPanel.findChild(BattleList, "BattleItem1");
            BattleGrid = UISoldierPanel.findChild(BattleList, "BattleGrid1");

            BattleFieldList = UISoldierPanel.findChild(m_root, "BattleFieldList");
            BattleFieldItem = UISoldierPanel.findChild(BattleFieldList, "BattleFieldItem");
            BattleFieldGrid = UISoldierPanel.findChild(BattleFieldList, "BattleFieldGrid");

            GameObject Info = UISoldierPanel.findChild(m_root, "Info");
            m_expValue = PanelTools.Find<UILabel>(Info, "expValue");
            m_goldValue = PanelTools.Find<UILabel>(Info, "goldValue");
            m_des = PanelTools.Find<UILabel>(Info, "des");
            m_reward = PanelTools.Find<UILabel>(Info, "reward");
            m_reward.text = DataManager.getLanguageMgr().getString(m_reward.text);
            m_title = PanelTools.Find<UILabel>(m_root, "bg/title");
            m_title.text = DataManager.getLanguageMgr().getString(m_title.text);

            RightSprite = PanelTools.Find<UISprite>(m_root, "RightSprite");
            LeftSprite = PanelTools.Find<UISprite>(m_root, "LeftSprite");
            map = PanelTools.Find<UISprite>(m_root, "Info/map");
            RightSprite.gameObject.SetActive(false);
            BattleScrollBar = PanelTools.Find<UIScrollBar>(m_root, "BattleScrollBar");
            BattleFieldScrollBar = PanelTools.Find<UIScrollBar>(m_root, "BattleFieldScrollBar");

            SetVisible(false);
        }

        protected override void onShow()
        {
            DataManager.getBattleUIData().SendPveEntertimesRequest();
            DataManager.getBattleUIData()._LoadBattleFieldData();
			DataManager.getBattleUIData()._LoadBattleEnemyData();
            AddBattleItem();
            ShowDefFiled();
            base.onShow();
        }

        protected void OnClose(GameObject go)
        {
            SetVisible(false);
        }

        protected void OnDragBattle(GameObject go, Vector2 delta)
        {
            if (BattleScrollBar.value > 0.9)
            {
                LeftSprite.gameObject.SetActive(false);
            }
            else if(BattleScrollBar.value < 0.1)
            {
                RightSprite.gameObject.SetActive(false);
            }
            else
            {
                RightSprite.gameObject.SetActive(true);
                LeftSprite.gameObject.SetActive(true);
            }
        }

        public class BattleBtn
        {
            public GameObject root;
            public UILabel name;
            public UILabel id;
            public UISprite icon;
        }

        public class BattleFieldBtn
        {
            public GameObject root;
            public UILabel name;
            public UILabel battleId;
            public UILabel fieldId;
            public UILabel times;
            public UISprite icon;
            public UISprite star1;
            public UISprite star2;
            public UISprite star3;

            public void ShowStar(uint star)
            {
                if (star > 0)
                {
                    star1.color = Color.white;
                }
                else
                {
                    star1.color = Color.black;
                }

                if (star > 1)
                {
                    star2.color = Color.white;
                }
                else
                {
                    star2.color = Color.black;
                }

                if (star > 2)
                {
                    star3.color = Color.white;
                }
                else
                {
                    star3.color = Color.black;
                }
            }
            
            public void Release()
            {
                if (root != null)
                {
                    GameObject.Destroy(root);
                }
            }
        }

        List<BattleFieldBtn> fieldBtnList = new List<BattleFieldBtn>();

        public void AddBattleItem()
        {
            //普通战役开启
            int idBttle1 = (int)DataManager.getUserData().Data.idBttle1;
            
            if (BattleGrid.transform.childCount > 0)
            {
                return;
            }

            int nBattleID = 0;

            foreach (KeyValuePair<int, DataMgr.BattleUIData.BattleField> kvi in DataManager.getBattleUIData().dic_BattleField)
            {
                if (nBattleID == kvi.Value.nBattleID)
                {
                    continue;
                }

                nBattleID = kvi.Value.nBattleID;
                BattleBtn bb = new BattleBtn();

                bb.root = NGUITools.AddChild(BattleGrid, BattleItem);
                bb.root.name = kvi.Value.nBattleID.ToString();
                bb.name = PanelTools.Find<UILabel>(bb.root, "Label");

                if (idBttle1 >= kvi.Value.nBattleID)
                {
                    bb.name.text = DataManager.getLanguageMgr().getString(kvi.Value.strBattleName);
                }
                else
                {
                    bb.name.text = DataManager.getLanguageMgr().getString("66360003");
                }

                bb.id = PanelTools.Find<UILabel>(bb.root, "id");
                bb.id.text = kvi.Value.nBattleID.ToString();
                UIEventListener.Get(bb.root).onClick = OnBattleBtn;
                //UIEventListener.Get(bb.root).onDrag = OnDragBattle;

                bb.root.SetActive(true);
            }

            UIGrid grid = BattleGrid.GetComponent<UIGrid>();
            grid.repositionNow = true;
        }

        protected void OnBattleBtn(GameObject go)
        {
            UILabel idLabel = PanelTools.Find<UILabel>(go, "id");

            SelectBattle(int.Parse(idLabel.text));
        }

        protected void OnBattleFieldBtn(GameObject go)
        {
            UILabel idLabel = PanelTools.Find<UILabel>(go, "fieldId");

            int nCharge = 0;
            
            if (DataManager.getBattleUIData().dic_BattleField.ContainsKey(m_SelectFieldId))
            {
                nCharge = DataManager.getBattleUIData().dic_BattleField[m_SelectFieldId].nCharge;
            }

            //需要消耗宝石
            string strText = "66360018";
            strText = DataManager.getLanguageMgr().getString(strText);
            strText += nCharge.ToString();

            if (m_SelectFieldId == int.Parse(idLabel.text))
            {
                if (OverTimes(m_SelectFieldId))
                {
                    MessageBoxMgr.ShowMessageBox("BuyTicket", strText, _enterFieldById, null);
                }
                else
                {
                    EnterFieldById(m_SelectFieldId);
                }

            }
            else
            {
                m_SelectFieldId = int.Parse(idLabel.text);
                SelectBattleField(m_SelectFieldId);
            }
        }

        public bool OverTimes(int FieldId)
        {
            uint uHasTimes = GetEnterTimesByFieldID((uint)FieldId);
            int nFreeTime = DataManager.getBattleUIData().dic_BattleField[FieldId].nFreeTime;

            if (uHasTimes < nFreeTime)
            {
                return false;
            }
            
            return true;
        }

        protected void EnterFieldById(int id)
        {
            ManeuverPanel mp = UI.PanelManage.me.GetPanel<ManeuverPanel>(PanelID.ManeuverPanel);

            mp.SetFieldId(id);
            mp.SetVisible(true);
        }

        public bool _enterFieldById(SLG.EventArgs obj)
        {
            ManeuverPanel mp = UI.PanelManage.me.GetPanel<ManeuverPanel>(PanelID.ManeuverPanel);

            mp.SetFieldId(m_SelectFieldId, 1);
            mp.SetVisible(true);
            return true;
        }

        protected void SelectBattle(int id)
        {
            ClearfieldBtns();
            ShowChosenBattle(id);

            //普通关卡开启
            int idField1 = (int)DataManager.getUserData().Data.idField1;
            
			foreach (KeyValuePair<int, DataMgr.BattleUIData.BattleField> kvi in DataManager.getBattleUIData().dic_BattleField)
            {
                if (id == kvi.Value.nBattleID)
                {
                    MoveFieldItem();
                    BattleFieldBtn bb = new BattleFieldBtn();

                    bb.root = NGUITools.AddChild(BattleFieldGrid, BattleFieldItem);
                    bb.root.name = kvi.Value.nBattlefieldID.ToString();
                    bb.name = PanelTools.Find<UILabel>(bb.root, "Label");

                    if (idField1 >= kvi.Key)
                    {
                        bb.name.text = DataManager.getLanguageMgr().getString(kvi.Value.strFieldName);
                        UIEventListener.Get(bb.root).onClick = OnBattleFieldBtn;
                    }
                    else
                    {
                        bb.name.text = DataManager.getLanguageMgr().getString("66360003");
                    }

                    bb.battleId = PanelTools.Find<UILabel>(bb.root, "battleId");
                    bb.battleId.text = kvi.Value.nBattleID.ToString();
                    bb.fieldId = PanelTools.Find<UILabel>(bb.root, "fieldId");
                    bb.fieldId.text = kvi.Value.nBattlefieldID.ToString();
                    bb.times = PanelTools.Find<UILabel>(bb.root, "times");

                    uint enterTimes = GetEnterTimesByFieldID((uint)kvi.Value.nBattlefieldID);
                    int MaxEnterTimes = kvi.Value.nFreeTime;

                    if (MaxEnterTimes == 65535 || idField1 >= kvi.Key)
                    {
                        bb.times.text = "";
                    }
                    else
                    {
                        bb.times.text = enterTimes.ToString() + "/" + MaxEnterTimes.ToString();
                    }

                    bb.star1 = PanelTools.Find<UISprite>(bb.root, "star1");
                    bb.star2 = PanelTools.Find<UISprite>(bb.root, "star2");
                    bb.star3 = PanelTools.Find<UISprite>(bb.root, "star3");

                    uint star = GetFieldStar((uint)kvi.Value.nBattlefieldID);
                    bb.ShowStar(star);

                    bb.root.SetActive(true);
                    fieldBtnList.Add(bb);
                }

            }

            UIGrid grid = BattleFieldGrid.GetComponent<UIGrid>();
            grid.repositionNow = true;

            m_SelectBattleId = id;
        }

        public uint GetEnterTimesByFieldID(uint fieldID)
        {
            if (DataManager.getBattleUIData().dicEnterTime.ContainsKey(fieldID))
            {
                return DataManager.getBattleUIData().dicEnterTime[fieldID].enterTimes;
            }
            
            return 0;
        }

        public uint GetFieldStar(uint fieldID)
        {
            if (DataManager.getBattleUIData().dicEnterTime.ContainsKey(fieldID))
            {
                return DataManager.getBattleUIData().dicEnterTime[fieldID].star;
            }

            return 0;
        }

        protected void SelectBattleField(int id)
        {
			if (DataManager.getBattleUIData().dic_BattleField.ContainsKey(id))
            {
				m_expValue.text = DataManager.getBattleUIData().dic_BattleField[id].nAwardExp.ToString();
				m_goldValue.text = DataManager.getBattleUIData().dic_BattleField[id].nAwardGold.ToString();
				string strKey = DataManager.getBattleUIData().dic_BattleField[id].strDesc;
                m_des.text = DataManager.getLanguageMgr().getString(strKey);
                map.spriteName = DataManager.getBattleUIData().dic_BattleField[id].strBattleDesc;
                ShowChosenField(id);

                m_SelectFieldId = id;
            }
        }

        protected void ClearfieldBtns()
        {
            foreach(BattleFieldBtn bf in fieldBtnList)
            {
                bf.Release();
            }

            fieldBtnList.Clear();
        }

        protected void MoveFieldItem()
        {
            foreach (Transform child in BattleFieldGrid.transform)
            {
                Vector3 mPos = child.position;
                mPos.y = mPos.y - 0.3f;
                child.position = mPos;
            }
        }

        protected void ShowChosenBattle(int id)
        {
            foreach(Transform child in BattleGrid.transform)
            {
                UILabel idLabel = PanelTools.Find<UILabel>(child.gameObject, "id");
                UISprite bg = PanelTools.Find<UISprite>(child.gameObject, "Background");

                if (idLabel.text == id.ToString())
                {
                    bg.spriteName = "ChooseBattleUITabBgHighLight";
                }
                else
                {
                    bg.spriteName = "ChooseBattleUITabBg";
                }
            }
        }

        protected void ShowChosenField(int id)
        {
            foreach (Transform child in BattleFieldGrid.transform)
            {
                UILabel idLabel = PanelTools.Find<UILabel>(child.gameObject, "fieldId");
                UISprite bg = PanelTools.Find<UISprite>(child.gameObject, "Background");

                if (idLabel.text == id.ToString())
                {
                    bg.spriteName = "ChooseBattleUIItemBgHighLight";
                }
                else
                {
                    bg.spriteName = "ChooseBattleUIItemBg1";
                }
            }
        }

        public void ShowDefFiled()
        {
            int idBttle1 = (int)DataManager.getUserData().Data.idBttle1;
            int idField1 = (int)DataManager.getUserData().Data.idField1;

            if (m_SelectBattleId != 0 && m_SelectFieldId != 0)
            {
                idBttle1 = m_SelectBattleId;
                idField1 = m_SelectFieldId;
            }

            SelectBattle(idBttle1);
            SelectBattleField(idField1);
        }

    }
}
