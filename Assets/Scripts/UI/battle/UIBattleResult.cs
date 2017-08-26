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
#pragma warning disable 0649
namespace UI
{
    public class UIBattleResult : PanelBase
    {
        GameObject m_root;
        public const PanelID id = PanelID.BattlefieldPanel;

        public override string GetResPath()
        {
            return "BattleResult.prefab";
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

        GameObject loseList;
        GameObject loseGrid;
        GameObject loseItem;

        GameObject expList;
        GameObject expGrid;
        GameObject expItem;
        GameObject Info;

        UIScrollBar loseScrollBar;
        UILabel goldLabel;
        UILabel blueLabel;
        UILabel cfmBtnLabel;
        UILabel goodLabel;

        UISprite bg;
        UISprite victory;
        UISprite defeat;

        GameObject Stars;

        public void init(GameObject root)
        {
            m_root = root;
            UIEventListener.Get(PanelTools.FindChild(m_root, "confirmBtn")).onClick = OnRetrun;

            loseList = UISoldierPanel.findChild(m_root, "loseList");
            loseGrid = UISoldierPanel.findChild(m_root, "loseList,Grid");
            loseItem = UISoldierPanel.findChild(m_root, "loseList,Item");

            expList = UISoldierPanel.findChild(m_root, "expList");
            expGrid = UISoldierPanel.findChild(expList, "Grid");
            expItem = UISoldierPanel.findChild(expList, "Item");
            loseScrollBar = PanelTools.Find<UIScrollBar>(m_root, "loseScrollBar");
            loseScrollBar.value = 0.5f;

            Info = UISoldierPanel.findChild(m_root, "Info");
            goldLabel = PanelTools.Find<UILabel>(Info, "goldLabel");
            blueLabel = PanelTools.Find<UILabel>(Info, "blueLabel");
            cfmBtnLabel = PanelTools.Find<UILabel>(m_root, "confirmBtn/Label");
            goodLabel = PanelTools.Find<UILabel>(Info, "goodLabel");
            string strContent = "66360000";
            cfmBtnLabel.text = DataManager.getLanguageMgr().getString(strContent);

            bg = PanelTools.Find<UISprite>(m_root, "bg");
            victory = PanelTools.Find<UISprite>(m_root, "bg/victory");
            defeat = PanelTools.Find<UISprite>(m_root, "bg/defeatbg");

            Stars = UISoldierPanel.findChild(m_root, "Stars");

            m_root.SetActive(false);
        }

        protected void OnRetrun(GameObject go)
        {
			DataManager.getBattleUIData().battleResult.SetVisible(false);
            DataManager.getBattleUIData().isBattleReturn = true;
            Application.LoadLevel("Main");
            //SLG.GlobalEventSet.FireEvent(SLG.eEventType.ChangeScene, new SLG.EventArgs(MainController.SCENE_MAINCITY));
        }

        public class ItemInfo
        {
            public int number;
            public string icon;              //头像
        }

        public class LostItem
        {
            public GameObject root;
            public UISprite icon;
            public UILabel number;

            public void Release()
            {
                if (root != null)
                {
                    GameObject.Destroy(root);
                }
            }

            public void Update()
            {

            }
        }

        public void AddLostItem(ItemInfo itemData)
        {
            LostItem item = new LostItem();
            item.root = NGUITools.AddChild(loseGrid, loseItem);
            item.root.SetActive(true);

            item.number = PanelTools.Find<UILabel>(item.root, "number");
            item.number.text = "X" + itemData.number.ToString();
            item.icon = PanelTools.Find<UISprite>(item.root, "icon");
            item.icon.spriteName = itemData.icon;
        }

        public override void SetVisible(bool value)
        {
            m_root.SetActive(value);

            if (value)
            {
                DataManager.getBattleUIData().SendBattlePveEndMsg();
            }
        }

        public class HeroExp
        {
            public int id;
            public int exp;
            public string icon;              //头像
        }

        public class ExpItem
        {
            public GameObject root;
            public UISprite icon;
            public UILabel exp;
            public UILabel id;
            public HeroExp he;

            public void Release()
            {
                if (root != null)
                {
                    GameObject.Destroy(root);
                }
            }

            public void Update()
            {

            }
        }

        public List<ExpItem> expItemList = new List<ExpItem>();

        public void AddExpItem(HeroExp he)
        {
            ExpItem item = new ExpItem();
            item.root = NGUITools.AddChild(expGrid, expItem);
            item.root.SetActive(true);

            item.he = he;
            item.exp = PanelTools.Find<UILabel>(item.root, "exp");
            item.exp.text = "+" + he.exp.ToString();
            item.icon = PanelTools.Find<UISprite>(item.root, "icon");
            item.icon.spriteName = he.icon;
            item.id = PanelTools.Find<UILabel>(item.root, "id");
            item.exp.text = he.id.ToString();


            expItemList.Add(item);
        }

        public void UpdateRewardData()
        {
            if (DataManager.getBattleUIData().m_PveReward.cbResult == 0)
            {
                victory.spriteName = "CombatResultUIDefeatedBg";
                defeat.gameObject.SetActive(true);
            }
            else
            {
                victory.spriteName = "CombatResultUIVictoryBg";
                defeat.gameObject.SetActive(false);
            }
            
            
            goldLabel.text = "X" + DataManager.getBattleUIData().m_PveReward.u32Coin.ToString();
            blueLabel.text = "X" + DataManager.getBattleUIData().m_PveReward.u32Stone.ToString();
            goodLabel.text = "X" + DataManager.getBattleUIData().m_PveReward.u32Diamond.ToString();

            showStar(DataManager.getBattleUIData().m_PveReward.cbStar);

            //箭
            if (BattleController.SingleTon().PlayerDeadArcherCount > 0)
            {
                ItemInfo info = new ItemInfo();
                info.number = BattleController.SingleTon().PlayerDeadArcherCount;
                info.icon = "archer";

                AddLostItem(info);
            }
            //骑
            if (BattleController.SingleTon().PlayerDeadCavalryCount > 0)
            {
                ItemInfo info = new ItemInfo();
                info.number = BattleController.SingleTon().PlayerDeadCavalryCount;
                info.icon = "cavalry";

                AddLostItem(info);
            }
            //枪
            if (BattleController.SingleTon().PlayerDeadSpearmenCount > 0)
            {
                ItemInfo info = new ItemInfo();
                info.number = BattleController.SingleTon().PlayerDeadSpearmenCount;
                info.icon = "pikeMan";

                AddLostItem(info);
            }
            //法
            if (BattleController.SingleTon().PlayerDeadMagicCount > 0)
            {
                ItemInfo info = new ItemInfo();
                info.number = BattleController.SingleTon().PlayerDeadMagicCount;
                info.icon = "wizard";

                AddLostItem(info);
            }
            //盾
            if (BattleController.SingleTon().PlayerDeadPeltastCount != 0)
            {
                ItemInfo info = new ItemInfo();
                info.number = BattleController.SingleTon().PlayerDeadPeltastCount;
                info.icon = "shield";

                AddLostItem(info);
            }


            expItemList.Clear();
            ExpToLose etl = m_root.AddComponent<ExpToLose>();

            foreach (KeyValuePair<uint, BattleUIData.FightHeroInfo> kvi in DataManager.getBattleUIData().dicFightHero)
            {
                HeroExp info = new HeroExp();

                if (DataManager.getBattleUIData().m_PveReward.dicHeroExp.ContainsKey(kvi.Value.idHero))
                {
                    info.exp = (int)DataManager.getBattleUIData().m_PveReward.dicHeroExp[kvi.Value.idHero];
                }
                else
                {
                    info.exp = 0;//临时处理
                }
                info.icon = kvi.Value.icon;

                AddExpItem(info);
            }

            UIGrid grid = expGrid.GetComponent<UIGrid>();
            grid.repositionNow = true;

            etl.isTiming = true;
        }


        public void showStar(int star)
        {
            UISprite star1 = PanelTools.Find<UISprite>(Stars, "star1");
            UISprite star2 = PanelTools.Find<UISprite>(Stars, "star2");
            UISprite star3 = PanelTools.Find<UISprite>(Stars, "star3");

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
    }

}