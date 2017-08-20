using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using EventArgs = SLG.EventArgs;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
namespace UI
{
    public class HeroUpgradePanel : PanelBase
    {
        public HeroUpgradePanel()
        {

        }

        public override string GetResPath()
        {
            return "heroUpgrade.prefab";
        }

        // 英雄选项的prefab
        GameObject m_heroItemPrefab;

        protected override void Initimp(List<GameObject> prefabs)
        {
            m_heroItemPrefab = prefabs[1];
            m_upgradeData = new UpgradeData();
            m_upgradeData.init(PanelTools.Find(Root, "Upgrade"));

            m_goldProgress = PanelTools.Find<UISprite>(Root, "goldCoin/goldProcessBar"); // 金钱进度
            m_goldText = PanelTools.Find<UILabel>(Root, "goldCoin/goldNo"); // 金钱文本

            m_heroGridRoot = PanelTools.Find<UIGrid>(Root, "HeroDitailBg1/HeroIcon"); // 英雄上面根结点

            m_upgradeRoot = PanelTools.Find(Root, "Upgrade"); // 升级根窗口
            m_chooseRoot = PanelTools.Find(Root, "chooseHero"); // 卡牌选择根窗口

            UIEventListener.Get(PanelTools.Find(Root, "Upgrade/upgrade/upgrade")).onClick = OnUpgradeBtnClick; // 点击升级按钮
            UIEventListener.Get(PanelTools.Find(Root, "close")).onClick = OnCloseBtn;
            //UIEventListener.Get(Find(Root, "")).onClick = ;

            UIEventListener.Get(PanelTools.Find(Root, "Upgrade/Button1/AddCard")).onClick = OnSelect1BtnClick;
            UIEventListener.Get(PanelTools.Find(Root, "Upgrade/Button2/AddCard")).onClick = OnSelect2BtnClick;

            m_chooseHero = new ChooseHero();
            m_chooseHero.m_itemRoot = PanelTools.Find<UIGrid>(Root, "chooseHero/chooseHeroTit/ItemRoot");

            UIEventListener.Get(PanelTools.Find(Root, "chooseHero/close")).onClick = OnCloseChooseBtn;

            UIEventListener.Get(PanelTools.Find(Root, "chooseHero/Sortord")).onClick = OnSortordBtnClick;
            UIEventListener.Get(PanelTools.Find(Root, "chooseHero/confirm")).onClick = OnConfirmBtnClick;            

            ShowUpgrade();

//             for (int i = 0; i < 10; ++i)
//             {
//                 Hero hero = new Hero();
//                 hero.id = i + 1;
//                 hero.start = UnityEngine.Random.Range(0, 5);
//                 hero.money = UnityEngine.Random.Range(0, 10000);
//                 hero.atk = UnityEngine.Random.Range(0, 10000);
//                 hero.hp = UnityEngine.Random.Range(0, 10000);
//                 hero.def = UnityEngine.Random.Range(0, 10000);
//                 hero.led = UnityEngine.Random.Range(0, 10000);
// 
//                 hero.nextAtt = UnityEngine.Random.Range(0, 10000);
//                 hero.nextHp = UnityEngine.Random.Range(0, 10000);
//                 hero.nextDef = UnityEngine.Random.Range(0, 10000);
//                 hero.nextLed = UnityEngine.Random.Range(0, 10000);
// 
//                 hero.icon = string.Format("{0}:{1}", "AtlasCard", "iron card");
// 
//                 m_heroList.Add(hero);
//             }
// 
//             UpdateShow();
            SetVisible(false);
        }

        protected void OnCloseChooseBtn(GameObject go)
        {
            ShowUpgrade();
        }

        protected void OnCloseBtn(GameObject go)
        {
            SetVisible(false);
        }

        public class UpgradeEvent
        {
            public int id; // 升级的卡牌ID
            public List<int> eatids; // 吃掉的卡牌ID

            public UpgradeEvent(int id, List<int> eatids)
            {
                this.id = id;
                this.eatids = eatids;
            }
        }

        protected void OnUpgradeBtnClick(GameObject go)
        {
            if (m_current == null)
                return;

            List<int> eatids = new List<int>();
            for (int i = 0; i < m_selectHero.Length; ++i)
            {
                if (m_selectHero[i] != null)
                    eatids.Add(m_selectHero[i].id);
            }

            if (eatids.Count >= 2)
            {
                object obj = new UpgradeEvent(m_current.id, eatids);
                GlobalEventSet.FireEvent(eEventType.HeroUpgradeEvent, new EventArgs(obj));
            }
            else
            {
                ConsoleSelf.me.addText("请选择卡牌");
            }
        }

        protected void OnSelect1BtnClick(GameObject go)
        {
            ShowChoose();
        }

        protected void OnSelect2BtnClick(GameObject go)
        {
            ShowChoose();
        }

        public override PanelID GetPanelID()
        {
            return PanelID.HeroUpgradePanel;
        }

        protected void AddHeroItem(string str_icon)
        {
            HeroIcon icon = new HeroIcon();
            icon.icon = NGUITools.AddChild(m_heroGridRoot.gameObject).AddComponent<UISprite>();
            icon.icon.gameObject.name = string.Format("Icon {0}", m_iconList.Count+1);
            icon.toggle = icon.icon.gameObject.AddComponent<UIToggle>();
            icon.toggle.group = 1;
            icon.toggle.startsActive = false;
            icon.icon.gameObject.AddComponent<UIDragScrollView>();
            icon.icon.width = 285;
            icon.icon.height = 395;
            NGUITools.AddWidgetCollider(icon.icon.gameObject);
            icon.icon.autoResizeBoxCollider = true;
            icon.icon.depth = 2;
            PanelTools.SetSpriteIcon(icon.icon, str_icon);
            m_iconList.Add(icon);
        }

        // 一个英雄
        public class Hero
        {
            public int id; // id
            public string icon; // 图标""
            public int start; // 当前星级
            public int money; // 升级所需要的金钱

            // 当前等级
            public int level;
            public int power;

            // 当前属性
            public int atk;
            public int hp;
            public int def;
            public int led;

            // 下一级属性
            public int nextAtt;
            public int nextHp;
            public int nextDef;
            public int nextLed;
        }

        public List<Hero> m_heroList = new List<Hero>();

        // 设置显示的数据
        public void SetData(List<Hero> dataList)
        {
            m_heroList = dataList;
            UpdateShow();

            if (m_heroList != null && m_heroList.Count != 0)
            {
                SetCurrent(m_heroList[0]);
            }

            for (int i = 0; i < m_iconList.Count; ++i)
            {
                EventDelegate.Add(m_iconList[i].toggle.onChange, delegate() { OnActiveToggle(m_iconList[i]); });
            }
//             for (m_heroList)
//             {
//             }
        }

        // 当前显示的英雄
        protected Hero m_current;

        protected void SetCurrent(Hero current)
        {
            if (m_current == current)
            {
                string icon1 = m_selectHero[0] == null ? "" : m_selectHero[0].icon;
                string icon2 = m_selectHero[1] == null ? "" : m_selectHero[1].icon;
                m_upgradeData.SetData(m_current, icon1, icon2);
                return;
            }
            else
            {
                m_current = current;
                for (int i = 0; i < m_heroList.Count; ++i)
                {
                    if (m_heroList[i] == m_current)
                    {
                        m_iconList[i].toggle.value = true; // 激活
                        break;
                    }
                }

                string icon1 = m_selectHero[0] == null ? "" : m_selectHero[0].icon;
                string icon2 = m_selectHero[1] == null ? "" : m_selectHero[1].icon;
                m_upgradeData.SetData(m_current, icon1, icon2);
            }
        }

        protected void OnActiveToggle(HeroIcon param)
        {
            if (param.toggle.value == false)
            {
                SetCurrent(null);
            }
            else
            {
                int index = m_iconList.IndexOf(param);
                if (index == -1 || index >= m_heroList.Count)
                    SetCurrent(null);
                else
                    SetCurrent(m_heroList[index]);
            }
        }

        // 更新显示
        void UpdateShow()
        {
            Release();
            foreach (Hero hero in m_heroList)
                AddHeroItem(hero.icon);
            m_heroGridRoot.repositionNow = true;

            m_upgradeData.SetData(m_current, "", "");
        }

        void OnSortordBtnClick(GameObject go)
        {
            m_selectHero[0] = null;
            m_selectHero[1] = null;
        }

        void OnConfirmBtnClick(GameObject go)
        {
            // 确定按钮
            int index = 0;
            m_selectHero[0] = null;
            m_selectHero[1] = null;
            foreach (ChooseHero.Item item in m_chooseHero.m_itemList)
            {
                if (item.toggle != null && item.toggle.value == true)
                {
                    m_selectHero[index] = item.hero;
                    ++index;
                    if (index >= 2)
                        break;
                }
            }

            ShowUpgrade();
        }

        protected override void ReleaseImp()
        {
            foreach (HeroIcon icon in m_iconList)
            {
                UnityEngine.Object.Destroy(icon.icon.gameObject);
            }

            m_iconList.Clear();
            m_chooseHero.Release();
        }

        // 英雄图标
        public class HeroIcon
        {
            public UISprite icon; // 图标
            public UIToggle toggle;
        }

        List<HeroIcon> m_iconList = new List<HeroIcon>();
        Hero[] m_selectHero = new Hero[2] { null, null };

        // 显示升级
        void ShowUpgrade()
        {
            m_upgradeRoot.SetActive(true);
            m_chooseRoot.SetActive(false);
            SetCurrent(m_current);
        }

        // 显示卡牌选择
        void ShowChoose()
        {
            m_upgradeRoot.SetActive(false);
            m_chooseRoot.SetActive(true);
            m_chooseHero.Release();

            foreach (Hero hero in m_heroList)
            {
                m_chooseHero.AddItem(hero, m_heroItemPrefab, m_current == hero ? false : true);
            }
            m_chooseHero.m_itemRoot.repositionNow = true;
        }

        UISprite m_goldProgress; // 金钱进度
        UILabel m_goldText; // 金钱文本

        UIGrid m_heroGridRoot; // 英雄上面根结点

        GameObject m_upgradeRoot; // 升级根窗口
        GameObject m_chooseRoot; // 卡牌选择根窗口

        class UpgradeData
        {
            public UISprite m_currentStart; // 当前星级
            public UISprite m_nextStart; // 下一星级

            public UILabel m_curAtk; // 当前属性
            public UILabel m_curHp;
            public UILabel m_curDef;
            public UILabel m_curLed;

            public UILabel m_nextAtk; // 下一级属性
            public UILabel m_nextHp;
            public UILabel m_nextDef;
            public UILabel m_nextLed;

            // 选择的卡牌图标
            public UISprite m_selIcon1;
            public UISprite m_selIcon2;

            public UILabel m_upMoney; // 升级所需要的金钱

            public void init(GameObject Root)
            {
                m_currentStart = PanelTools.Find<UISprite>(Root, "startCurrent");
                m_nextStart = PanelTools.Find<UISprite>(Root, "startUpgarde");

                m_curAtk = PanelTools.Find<UILabel>(Root, "detailsNo/atcNo");
                m_curHp = PanelTools.Find<UILabel>(Root, "detailsNo/hpNo");
                m_curDef = PanelTools.Find<UILabel>(Root, "detailsNo/defNo");
                m_curLed = PanelTools.Find<UILabel>(Root, "detailsNo/ledNo");

                m_nextAtk = PanelTools.Find<UILabel>(Root, "higherLeveldetailsNo/atcNo");
                m_nextHp = PanelTools.Find<UILabel>(Root, "higherLeveldetailsNo/hpNo");
                m_nextDef = PanelTools.Find<UILabel>(Root, "higherLeveldetailsNo/defNo");
                m_nextLed = PanelTools.Find<UILabel>(Root, "higherLeveldetailsNo/ledNo");

                m_selIcon1 = PanelTools.Find<UISprite>(Root, "Button1/AddCard/Background");
                m_selIcon2 = PanelTools.Find<UISprite>(Root, "Button2/AddCard/Background");

                m_upMoney = PanelTools.Find<UILabel>(Root, "coldNo");
            }

            // 设置显示数据
            public void SetData(Hero hero, string selIcon1, string selIcon2)
            {
                if (hero == null)
                {
                    m_currentStart.fillAmount = 0f;
                    m_nextStart.fillAmount = 0f;

                    m_curAtk.text = "";
                    m_curHp.text = "";
                    m_curDef.text = "";
                    m_curLed.text = "";

                    m_nextAtk.text = "";
                    m_nextHp.text = "";
                    m_nextDef.text = "";
                    m_nextLed.text = "";

                    m_upMoney.text = "";
                }
                else
                {
                    m_currentStart.fillAmount = hero.start * 0.2f;
                    m_nextStart.fillAmount = (hero.start + 1) * 0.2f;

                    m_curAtk.text = hero.atk.ToString();
                    m_curHp.text = hero.hp.ToString();
                    m_curDef.text = hero.def.ToString();
                    m_curLed.text = hero.led.ToString();

                    m_nextAtk.text = hero.nextAtt.ToString();
                    m_nextHp.text = hero.nextHp.ToString();
                    m_nextDef.text = hero.nextDef.ToString();
                    m_nextLed.text = hero.nextLed.ToString();

                    m_upMoney.text = hero.money.ToString();
                }

                PanelTools.SetSpriteIcon(m_selIcon1, selIcon1);
                PanelTools.SetSpriteIcon(m_selIcon2, selIcon2);
            }
        }

        UpgradeData m_upgradeData;

        // 选择英雄
        class ChooseHero
        {
            public UIGrid m_itemRoot; // 英雄项根窗口

            // 英雄项
            public class Item
            {
                public UISprite icon;
                public UILabel lv;
                public UILabel power;
                public UILabel atk;
                public UILabel def;
                public UILabel hp;
                public Hero hero;

                public GameObject root; // 根窗口
                public UIToggle toggle; // 是否选中

                public void Update()
                {
                    PanelTools.SetSpriteIcon(icon, hero.icon);
                    lv.text = hero.level.ToString();
                    power.text = hero.power.ToString();
                    atk.text = hero.atk.ToString();
                    def.text = hero.def.ToString();
                    hp.text = hero.hp.ToString();
                }
            }

            public List<Item> m_itemList = new List<Item>();

            public void AddItem(Hero hero, GameObject prefab, bool bToggle)
            {
                Item item = new Item();
                item.hero = hero;

                item.root = NGUITools.AddChild(m_itemRoot.gameObject, prefab);
                if (bToggle == true)
                {
                    item.toggle = item.root.GetComponent<UIToggle>();
                    item.toggle.enabled = true;
                }
                else
                {
                    UIToggle toggle = item.root.GetComponent<UIToggle>();
                    toggle.value = false;
                    toggle.enabled = false;
                }

                item.icon = PanelTools.Find<UISprite>(item.root, "icon");
                item.lv = PanelTools.Find<UILabel>(item.root, "lvNo");
                item.power = PanelTools.Find<UILabel>(item.root, "powerNo");
                item.atk = PanelTools.Find<UILabel>(item.root, "atcNo");
                item.def = PanelTools.Find<UILabel>(item.root, "defNo");
                item.hp = PanelTools.Find<UILabel>(item.root, "defNo");
                
                item.Update();
                m_itemList.Add(item);
            }

            public void Release()
            {
                foreach (Item item in m_itemList)
                {
                    UnityEngine.Object.Destroy(item.root);
                }

                m_itemList.Clear();
            }
        }

        ChooseHero m_chooseHero;

        bool SetCoin()
        {
            m_goldText.text = RewardPanel.getCoins().ToString();

            return true;
        }

        public override void SetVisible(bool value)
        {
            base.SetVisible(value);

            if(value)
                SetCoin();

            GameObject go = PanelTools.Find(Root, "Upgrade/Button1/AddCard");
            //UISprite uis = go.GetComponent<UISprite>();
            //uis.spriteName = "";
        }
    }

}