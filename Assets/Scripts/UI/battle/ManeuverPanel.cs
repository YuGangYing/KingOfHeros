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
    public class ManeuverPanel : PanelBase
    {
        public const PanelID id = PanelID.ManeuverPanel;

        public override string GetResPath()
        {
            return "ALineup.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        GameObject m_root;
        GameObject ScrollView;
        GameObject Grid;
        UIGrid itemGrid;
        GameObject Item;
        GameObject Ground;
        GameObject WeInfo;
        GameObject EnemyGround;
        GameObject EnemyInfo;
        GameObject DragRoot;
        GameObject SortBtn;
        GameObject ArmyBtn;
        UILabel title;
        UILabel pointLabel;
        UILabel heroLabel;
        UILabel powerLeftLabel;
        UILabel powerRightLabel;
        UILabel startBtnLabel;
        UILabel sortBtnLabel;
        UILabel armyBtnLabel;

        int nSortFun = 0;
        int nFilter = 0;
        const int MaxHeroOnGround = 5;
        int CountHeroOnGround = 0;
        int CountCombatValueOnGround = 0;
        int HeroAmount;
        Int64 sortTime = 1;
        int CurFiledId = 0;
        int CurBattleId = 0;
        int m_cbBuy = 0;

        AudioMgr.AudioItem audioItem = null;

        public Dictionary<uint, int> dicHeroSoldier = new Dictionary<uint, int>();
        public Dictionary<uint, int> dicMaxHeroSoldier = new Dictionary<uint, int>();
        //public Dictionary<uint, int> dicHeroPosition = new Dictionary<uint, int>();
        public List<EnemyHero> EnemyHeroList = new List<EnemyHero>();

        protected override void Initimp(List<GameObject> prefabs)
        {
            UIEventListener.Get(PanelTools.FindChild(Root, "ReturnBtn")).onClick = OnClose;
            UIEventListener.Get(PanelTools.FindChild(Root, "StartBtn")).onClick = StartBtn;
            UIEventListener.Get(PanelTools.FindChild(Root, "SortBtn")).onClick = OnSortBtn;
            UIEventListener.Get(PanelTools.FindChild(Root, "ArmyBtn")).onClick = OnArmyBtn;

            m_root = Root;
            DragRoot = UISoldierPanel.findChild(m_root, "DragRoot");
            SortBtn = UISoldierPanel.findChild(m_root, "SortBtn");
            ArmyBtn = UISoldierPanel.findChild(m_root, "ArmyBtn");

            ScrollView = UISoldierPanel.findChild(m_root, "ScrollView");
            Grid = UISoldierPanel.findChild(ScrollView, "Grid");
            itemGrid = PanelTools.Find<UIGrid>(ScrollView, "Grid");
            Item = UISoldierPanel.findChild(ScrollView, "Item");

            Ground = UISoldierPanel.findChild(m_root, "Ground");
            WeInfo = UISoldierPanel.findChild(m_root, "WeInfo");

            EnemyGround = UISoldierPanel.findChild(m_root, "EnemyGround");
            EnemyInfo = UISoldierPanel.findChild(m_root, "EnemyInfo");


            title = PanelTools.Find<UILabel>(m_root, "BG/Title/Label");
            pointLabel = PanelTools.Find<UILabel>(m_root, "pointLabel");
            pointLabel.text = DataManager.getLanguageMgr().getString(pointLabel.text);
            pointLabel.gameObject.SetActive(false);

            heroLabel = PanelTools.Find<UILabel>(m_root, "BG/Heroes/Label");
            heroLabel.text = DataManager.getLanguageMgr().getString(heroLabel.text);

            powerLeftLabel = PanelTools.Find<UILabel>(m_root, "BG/Left/power");
            powerLeftLabel.text = DataManager.getLanguageMgr().getString(powerLeftLabel.text);

            powerRightLabel = PanelTools.Find<UILabel>(m_root, "BG/Right/power");
            powerRightLabel.text = DataManager.getLanguageMgr().getString(powerRightLabel.text);

            startBtnLabel = PanelTools.Find<UILabel>(m_root, "StartBtn/Label");
            startBtnLabel.text = DataManager.getLanguageMgr().getString(startBtnLabel.text);

            sortBtnLabel = PanelTools.Find<UILabel>(m_root, "SortBtn/Label");
            sortBtnLabel.text = DataManager.getLanguageMgr().getString(sortBtnLabel.text);

            armyBtnLabel = PanelTools.Find<UILabel>(m_root, "ArmyBtn/Label");
            armyBtnLabel.text = DataManager.getLanguageMgr().getString(armyBtnLabel.text);

            SetVisible(false);
        }

        protected override void onShow()
        {
            InitHeroList();
            dicMaxHeroSoldier.Clear();
            dicHeroSoldier.Clear();
            ClearGround();
            ClearEnemyGround();
            ShowEnemyInfo();
            SetLastGround();

            audioItem = AudioCenter.me.play(AudioMgr.AudioName.BATTLE_FIELD);
            DataManager.getBattleUIData().m_nCurHeroId = -1;

            base.onShow();
        }

        protected void OnClose(GameObject go)
        {
            if (audioItem != null)
            {
                audioItem.stop();
            }

            SetVisible(false);
            
            BattlefieldPanel bp = UI.PanelManage.me.GetPanel<BattlefieldPanel>(PanelID.BattlefieldPanel);
            bp.SetVisible(true);
        }

        protected void OnSortBtn(GameObject go)
        {
            Int64 nowTime = DataManager.getTimeServer().EstimateServerTime(DataManager.getTimeServer().ServerTime);

            if (sortTime - nowTime < 1)
            {
                return;
            }
            
            if (nSortFun < 3)
            {
                ++nSortFun;
            }
            else
            {
                nSortFun = 0;
            }

            UILabel btnName = PanelTools.Find<UILabel>(SortBtn, "Label");

            switch (nSortFun)
            {
                case 0:
                    btnName.text = "66360008"; //排序
                    break;
                case 1:
                    btnName.text = "66360009"; //等级
                    break;
                case 2:
                    btnName.text = "66360010"; //星级
                    break;
                case 3:
                    btnName.text = "66360011"; //品质
                    break;
            }

            btnName.text = DataManager.getLanguageMgr().getString(btnName.text);

            sortGrid();

            sortTime = nowTime;
        }

        protected void OnArmyBtn(GameObject go)
        {
            Int64 nowTime = DataManager.getTimeServer().EstimateServerTime(DataManager.getTimeServer().ServerTime);

            if (sortTime - nowTime < 1)
            {
                return;
            }
            
            if (nFilter < 5)
            {
                ++nFilter;
            }
            else
            {
                nFilter = 0;
            }

            UILabel btnName = PanelTools.Find<UILabel>(ArmyBtn, "Label");

            switch (nFilter)
            {
                case 0:
                    btnName.text = "66360012"; //分类
                    break;
                case 1:
                    btnName.text = "66360013"; //盾兵
                    break;
                case 2:
                    btnName.text = "66360014"; //枪兵
                    break;
                case 3:
                    btnName.text = "66360015"; //弓兵
                    break;
                case 4:
                    btnName.text = "66360016"; //骑兵
                    break;
                case 5:
                    btnName.text = "66360017"; //法师
                    break;
            }

            btnName.text = DataManager.getLanguageMgr().getString(btnName.text);

            HeroGrid hg = Grid.GetComponent<HeroGrid>();
            hg.Filterfun(nFilter);
            itemGrid.repositionNow = true;
            sortTime = nowTime;
        }

        protected void StartBtn(GameObject go)
        {
			DataManager.getBattleUIData().ClearFightHero();
			DataManager.getBattleUIData().HeroList.Clear();
            DataManager.getBattleUIData().dicHeroPosition.Clear();
            int nHero = 0;

            foreach (Transform child in Ground.transform)
            {
                GroundDragItem gdi = child.GetComponent<GroundDragItem>();

                UILabel id = PanelTools.Find<UILabel>(gdi.gameObject, "idHero");
                int nid = int.Parse(id.text);

                if (gdi != null && nid != 0)
                {
                    HeroSelect hs = new  HeroSelect();

                    hs.nHeroId = nid;
                    hs.nLocation = gdi.position;

                    if (dicHeroSoldier.ContainsKey((uint)hs.nHeroId))
                    {
                        hs.nSoldierNum = dicHeroSoldier[(uint)hs.nHeroId];

                        //临时处理
                        hs.nSoldierNum = 16;
                    }
                    
					DataManager.getBattleUIData().AddFightHero((uint)nid);
					DataManager.getBattleUIData().HeroList.Add(hs);
                    DataManager.getBattleUIData().dicHeroPosition.Add((uint)nid, gdi.position);
                    ++nHero;
                }
            }

            if (nHero == 0)
            {
                pointLabel.gameObject.SetActive(true);
                return;
            }

            DataManager.getBattleUIData().SendHeroPos();
            SetEnemyToCharacterMgr();

            if (CurFiledId != 0 && CurBattleId != 0)
            {
                DataManager.getBattleUIData().SendPVEBattleStartMsg((uint)CurBattleId, (uint)CurFiledId, (byte)m_cbBuy);
            }

            if (audioItem != null)
            {
                audioItem.stop();
            }

            //SLG.GlobalEventSet.FireEvent(SLG.eEventType.ChangeScene, new SLG.EventArgs(MainController.SCENE_BATTLE));
            DataManager.getBattleUIData().dicHeroDieArmy.Clear();
        }



        public class HeroInfo
        {
            public uint idHero;              // ID
            public uint idType;              // 英雄类型
            public byte u8Star;              // 星级
            public ushort usLevel;           // 等级
            public int quality;
            public int armyType;             //部队类型
            public string icon;              //头像
        }

        public class HeroItem
        {
            public GameObject root;
            public UISprite army;
            public UILabel armyType;
            public UISprite icon;
            public UILabel idHero;
            public UILabel level;
            public UISprite quality;
            public UILabel qualityLable;
            public UILabel starLable;
            public UISprite starSprite;
            public HeroInfo itemData;

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
                    //id
                    idHero.text = itemData.idHero.ToString();

                    //兵种
                    armyType.text = itemData.armyType.ToString();

                    switch (itemData.armyType)
                    {
                        case 1:
                            army.spriteName = "Infantry";
                            break;
                        case 2:
                            army.spriteName = "Pike";
                            break;
                        case 3:
                            army.spriteName = "Archers";
                            break;
                        case 4:
                            army.spriteName = "Cavalry";
                            break;
                        case 5:
                            army.spriteName = "Wizard";
                            break;
                    }

                    armyType.MakePixelPerfect();

                    //等级
                    level.text = itemData.usLevel.ToString();

                    //品质
                    qualityLable.text = itemData.quality.ToString();

                    switch (itemData.quality)
                    {
                        case 1:
                            quality.spriteName = "iron";
                            break;
                        case 2:
                            quality.spriteName = "copper";
                            break;
                        case 3:
                            quality.spriteName = "silver";
                            break;
                        case 4:
                            quality.spriteName = "gold";
                            break;
                    }

                    //星级
                    starLable.text = itemData.u8Star.ToString();

                    switch (itemData.u8Star)
                    {
                        case 0:
                            starSprite.spriteName = "0";
                            break;
                        case 1:
                            starSprite.spriteName = "1";
                            break;
                        case 2:
                            starSprite.spriteName = "2";
                            break;
                        case 3:
                            starSprite.spriteName = "3";
                            break;
                        case 4:
                            starSprite.spriteName = "4";
                            break;
                        case 5:
                            starSprite.spriteName = "5";
                            break;
                        case 6:
                            starSprite.spriteName = "6";
                            break;

                    }

                    //头像
                    icon.spriteName = itemData.icon;
                }
            }
        }

        Dictionary<uint, HeroInfo> m_dicHeroInfo = new Dictionary<uint, HeroInfo>();

        public void InitHeroList()
        {
            m_dicHeroInfo.Clear();
            m_dicHeroItem.Clear();
            ClearGrid();

            HeroAmount = DataManager.getHeroData().Amount;

            for (int i = 0; i < HeroAmount; ++i)
            {
                Packet.HERO_INFO hi = new Packet.HERO_INFO();
                if(!DataManager.getHeroData().getItemByIndex(i,ref hi))
                    continue;

                HeroInfo info = new HeroInfo();

                info.idHero = hi.idHero;
                info.idType = hi.idType;
                info.u8Star = hi.u8Star;
                info.usLevel = hi.usLevel;

                //战斗位置
                //uint pos = DataManager.getHeroData().ItemExData[info.idHero].mdwBattlePos;
                uint pos = hi.u8BattlePos;

                if (pos > 0 && pos <= 9)
                {
                    DataManager.getBattleUIData().dicHeroPosition[info.idHero] = (int)pos;
                }

                DataMgr.ConfigRow cr = null;
                DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)info.idType, out cr);
                if (cr == null)
                    return;
                info.quality = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.QUALITY);
                info.armyType = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.ARMY_TYPE);
                info.icon = cr.getStringValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.PORTARAIT);


                if (m_dicHeroInfo.ContainsKey(info.idHero))
                {
                    m_dicHeroInfo.Remove(info.idHero);
                }

                m_dicHeroInfo.Add(info.idHero, info);

                sortGrid();
                AddItem(info);
            }

        }

        Dictionary<uint, HeroItem> m_dicHeroItem = new Dictionary<uint, HeroItem>();
        
        public void AddItem(HeroInfo itemData)
        {
            HeroItem item = new HeroItem();

            item.itemData = itemData;
            item.root = NGUITools.AddChild(Grid, Item);
            item.root.name = itemData.idHero.ToString();
            item.root.SetActive(true);

            item.army = PanelTools.Find<UISprite>(item.root, "army");
            item.armyType = PanelTools.Find<UILabel>(item.root, "armyType");
            item.idHero = PanelTools.Find<UILabel>(item.root, "idHero");
            item.level = PanelTools.Find<UILabel>(item.root, "level");
            item.quality = PanelTools.Find<UISprite>(item.root, "quality");
            item.qualityLable = PanelTools.Find<UILabel>(item.root, "qualityLable");
            item.starLable = PanelTools.Find<UILabel>(item.root, "starLable");
            item.starSprite = PanelTools.Find<UISprite>(item.root, "starSprite");
            item.icon = PanelTools.Find<UISprite>(item.root, "icon");

            item.Update();

            m_dicHeroItem.Add(itemData.idHero,item);
        }

        protected void ClearGrid()
        {
            foreach (Transform child in Grid.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        protected void MoveItem()
        {
            foreach (Transform child in Grid.transform)
            {
                Vector3 mPos = child.position;
                mPos.y = mPos.y - 0.4f;
                child.position = mPos;
            }
        }

        protected void sortGrid()
        {
            HeroGrid hg = Grid.GetComponent<HeroGrid>();

            hg.sortFun = nSortFun;

            itemGrid.repositionNow = true;
        }

        private void ClearDragRoot()
        {
            if (DragRoot.transform.childCount > 0)
            {
                foreach (Transform child in DragRoot.transform)
                {
                    NGUITools.Destroy(child.gameObject);
                }
            }
        }

        public void HeroOnGround(uint idHero) 
        {
            foreach (Transform child in Grid.transform)
            {
                UILabel id = PanelTools.Find<UILabel>(child.gameObject, "idHero");

                if (id.text == idHero.ToString())
                {
                    child.gameObject.SetActive(false);
                }
            }

            sortGrid();
            ++CountHeroOnGround;
			CountCombatValueOnGround += DataManager.getBattleUIData().GetCombatValue(idHero);
            CountGround();

            if (dicMaxHeroSoldier.ContainsKey(idHero))
            {
                dicMaxHeroSoldier.Remove(idHero);
            }
            
            dicMaxHeroSoldier.Add(idHero, 16);
            AssortSoldier();
            ClearDragRoot();
        }

        public void HeroLeaveGround(uint idHero)
        {
            foreach (Transform child in Grid.transform)
            {
                UILabel id = PanelTools.Find<UILabel>(child.gameObject, "idHero");

                if (id.text == idHero.ToString())
                {
                    child.gameObject.SetActive(true);
                }
            }
            --CountHeroOnGround;
			CountCombatValueOnGround -= DataManager.getBattleUIData().GetCombatValue(idHero);
            CountGround();
            
            dicMaxHeroSoldier.Remove(idHero);
            AssortSoldier();
            ClearDragRoot();
            ClearArrow();

            HeroGrid hg = Grid.GetComponent<HeroGrid>();
            hg.Filterfun(nFilter);

            sortGrid();
        }

        public void CountGround()
        {
            UILabel number = PanelTools.Find<UILabel>(WeInfo, "number");
            number.text = CountHeroOnGround.ToString() + "/5";

            UILabel power = PanelTools.Find<UILabel>(WeInfo, "power");
            power.text = CountCombatValueOnGround.ToString();
        }

        public bool IsOverCount()
        {
            if (CountHeroOnGround >= MaxHeroOnGround)
            {
                return true;
            }

            return false;
        }

        public void AssortSoldier()
        {
            long lRecruitCur = (long)DataManager.getUserData().Data.unReserveSoldier;

            if (lRecruitCur == 0)
            {
                lRecruitCur = 100;
            }

            if (lRecruitCur > 0)
            {
                InitHeroSoldier();
                
                while (lRecruitCur > 0 && dicMaxHeroSoldier.Count > 0)
                {
                    
                    foreach (KeyValuePair<uint, int> kvi in dicMaxHeroSoldier)
                    {
                        int temp = dicHeroSoldier[kvi.Key];

                        if (temp < kvi.Value && lRecruitCur > 0)
                        {
                            dicHeroSoldier[kvi.Key] = temp + 1;
                        }

                        --lRecruitCur;
                    }
                }

                foreach (KeyValuePair<uint, int> kvi in dicHeroSoldier)
                {
                    foreach (Transform child in Ground.transform)
                    {
                        UILabel idLable = PanelTools.Find<UILabel>(child.gameObject, "idHero");

                        if (kvi.Key == uint.Parse(idLable.text))
                        {
                            UILabel armsLable = PanelTools.Find<UILabel>(child.gameObject, "arms");
                            UISlider slider = PanelTools.Find<UISlider>(child.gameObject, "Slider");

                            armsLable.text = kvi.Value.ToString() + "/" + dicMaxHeroSoldier[kvi.Key].ToString();
                            slider.value = (float)kvi.Value / (float)dicMaxHeroSoldier[kvi.Key];
                        }
                    }
                }

            }
        }

        private void InitHeroSoldier()
        {
            dicHeroSoldier.Clear();
            
            if (dicMaxHeroSoldier.Count > 0)
            {
                foreach (KeyValuePair<uint, int> kvi in dicMaxHeroSoldier)
                {
                    dicHeroSoldier[kvi.Key] = 0;
                }
            }
        }

        public void ClearGround()
        {
            CountHeroOnGround = 0;
            CountCombatValueOnGround = 0;

            UILabel number = PanelTools.Find<UILabel>(WeInfo, "number");
            number.text = CountHeroOnGround.ToString() + "/5";

            UILabel power = PanelTools.Find<UILabel>(WeInfo, "power");
            power.text = CountCombatValueOnGround.ToString();

            foreach(Transform child in Ground.transform)
            {
                GroundDragItem gdi = child.gameObject.GetComponent<GroundDragItem>();
                if (gdi != null)
                {
                    gdi.idHero = 0;
                }
                
                UILabel idLable = PanelTools.Find<UILabel>(child.gameObject, "idHero");

                if (idLable != null)
                {
                    idLable.text = "0";
                    idLable.gameObject.SetActive(false);
                }

                //带兵数
                UILabel armsLable = PanelTools.Find<UILabel>(child.gameObject, "arms");

                if (idLable != null)
                {
                    armsLable.text = "";
                    armsLable.gameObject.SetActive(false);
                }

                //士兵类型
                UISprite army = PanelTools.Find<UISprite>(child.gameObject, "army");
                UILabel armyLable = PanelTools.Find<UILabel>(child.gameObject, "armyLabel");

                if (army != null)
                {
                    army.spriteName = "";
                    army.gameObject.SetActive(false);
                    armyLable.text = "0";
                    armyLable.gameObject.SetActive(false);
                }

                //英雄头像
                UISprite icon = PanelTools.Find<UISprite>(child.gameObject, "icon");

                if (icon != null)
                {
                    icon.spriteName = "";
                    icon.gameObject.SetActive(false);
                }

                //英雄等级
                UILabel levelLable = PanelTools.Find<UILabel>(child.gameObject, "level");
                if (levelLable != null)
                {
                    levelLable.text = "";
                    levelLable.gameObject.SetActive(false);
                }

                //英雄品质
                UISprite quality = PanelTools.Find<UISprite>(child.gameObject, "quality");
                if (quality != null)
                {
                    quality.spriteName = "";
                    quality.gameObject.SetActive(false);
                }

                //带兵条
                UISlider slider = PanelTools.Find<UISlider>(child.gameObject, "Slider");

                if (slider != null)
                {
                    slider.value = 0;
                    slider.gameObject.SetActive(false);
                }

                //英雄星级

                UILabel starLable = PanelTools.Find<UILabel>(child.gameObject, "starLable");
                UISprite star = PanelTools.Find<UISprite>(child.gameObject, "star");
                UISprite starSprite = PanelTools.Find<UISprite>(child.gameObject, "starSprite");
                if (starLable != null)
                {
                    starLable.text = "";
                    starLable.gameObject.SetActive(false);
                }

                if (star != null)
                {
                    star.gameObject.SetActive(false);
                }

                if (starSprite != null)
                {
                    starSprite.spriteName = "";
                    starSprite.gameObject.SetActive(false);
                }
            }
        }

        public void ShowEnemyInfo()
        {
            ClearEnemyGround();
            int combatValue = 0;

            foreach (EnemyHero eh in EnemyHeroList)
            {
                foreach (Transform child in EnemyGround.transform)
                {
                    if (child.gameObject.name == eh.nLocation.ToString())
                    {
                        DataMgr.ConfigRow cr = null;
                        DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)eh.nHeroType, out cr);
                        if (cr == null)
                            continue;

                        //CardData cd = CsvConfigMgr.me.getHeroDetailByTypeId(eh.nHeroType);

                        //if (cd != null)
                        {
                            int quality = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.QUALITY); 
                            int armyType = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.ARMY_TYPE); 
                            string strIcon = cr.getStringValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.PORTARAIT); 

                            //士兵数
                            UILabel arms = PanelTools.Find<UILabel>(child.gameObject, "arms");
                            arms.text = eh.nSoldierNum.ToString();
                            arms.gameObject.SetActive(true);

                            UISlider slider = PanelTools.Find<UISlider>(child.gameObject, "Slider");
                            slider.value = 1;
                            slider.gameObject.SetActive(true);

                            //士兵类型
                            UISprite army = PanelTools.Find<UISprite>(child.gameObject, "army");
                            UILabel armyLabel = PanelTools.Find<UILabel>(child.gameObject, "armyLabel");
                            army.gameObject.SetActive(true);
                            armyLabel.text = armyType.ToString();
                            switch (armyType)
                            {
                                case 1:
                                    army.spriteName = "Infantry";
                                    break;
                                case 2:
                                    army.spriteName = "Pike";
                                    break;
                                case 3:
                                    army.spriteName = "Archers";
                                    break;
                                case 4:
                                    army.spriteName = "Cavalry";
                                    break;
                                case 5:
                                    army.spriteName = "Wizard";
                                    break;
                            }

                            //头像
                            UISprite icon = PanelTools.Find<UISprite>(child.gameObject, "icon");
                            icon.spriteName = strIcon;
                            icon.gameObject.SetActive(true);

                            //等级
                            UILabel level = PanelTools.Find<UILabel>(child.gameObject, "level");
                            level.text = eh.nHeroLevel.ToString();
                            level.gameObject.SetActive(true);

                            //品质
                            UISprite qualitySprite = PanelTools.Find<UISprite>(child.gameObject, "quality");
                            qualitySprite.gameObject.SetActive(true);
                            switch (quality)
                            {
                                case 1:
                                    qualitySprite.spriteName = "iron";
                                    break;
                                case 2:
                                    qualitySprite.spriteName = "copper";
                                    break;
                                case 3:
                                    qualitySprite.spriteName = "silver";
                                    break;
                                case 4:
                                    qualitySprite.spriteName = "gold";
                                    break;
                            }

                            //星级
                            UILabel starLable = PanelTools.Find<UILabel>(child.gameObject, "starLable");
                            starLable.text = eh.nHeroStar.ToString();
                            UISprite star = PanelTools.Find<UISprite>(child.gameObject, "star");
                            UISprite starSprite = PanelTools.Find<UISprite>(child.gameObject, "starSprite");

                            switch (eh.nHeroStar)
                            {
                                case 0:
                                    starSprite.spriteName = "0";
                                    break;
                                case 1:
                                    starSprite.spriteName = "1";
                                    break;
                                case 2:
                                    starSprite.spriteName = "2";
                                    break;
                                case 3:
                                    starSprite.spriteName = "3";
                                    break;
                                case 4:
                                    starSprite.spriteName = "4";
                                    break;
                                case 5:
                                    starSprite.spriteName = "5";
                                    break;
                            }

                            star.gameObject.SetActive(true);
                            starSprite.gameObject.SetActive(true);

                            //战斗力
                            int atk = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_ATTACK);// cd.getAttributeIntValue(CardData.enAttributeName.enAN_AttackPower);
                            int hp = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_HP);// cd.getAttributeIntValue(CardData.enAttributeName.enAN_HP);
                            int def = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_DEF);// cd.getAttributeIntValue(CardData.enAttributeName.enAN_DefensePower);
                            int lead = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_LEADER);// cd.getAttributeIntValue(CardData.enAttributeName.enAN_LeadPower);
                            int crit = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_VIOLENCE);// cd.getAttributeIntValue(CardData.enAttributeName.enAN_ViolencePower);

                            int lLv = eh.nHeroLevel;
                            int lStar = eh.nHeroStar;
                            DataMgr.ConfigRow crstar = null;
                            DataMgr.CHerroTalbeAttribute.getHeroStar((int)eh.nHeroType, lStar, out crstar);
                            if (crstar == null)
                                continue;

                            //CsvConfigMgr.me.getHeroStarDetailByTypeId((int)eh.nHeroType, (int)lStar, out cths);
                            int nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_ATTACK);// cths.getAttributeIntValue(CTableHerolStar.enAttributeName.enAN_Attack);
                            int nCurAtc = CardQualityUpdatePanel.GetValue((float)atk, (int)lStar, (int)lLv, nFactor);

                            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_HP);// cths.getAttributeIntValue(CTableHerolStar.enAttributeName.enAN_HP);
                            int nCurHP = CardQualityUpdatePanel.GetValue((float)hp, (int)lStar, (int)lLv, nFactor);

                            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_DEF); // cths.getAttributeIntValue(CTableHerolStar.enAttributeName.enAN_Defence);
                            int nCurDef = CardQualityUpdatePanel.GetValue((float)def, (int)lStar, (int)lLv, nFactor);

                            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_LEADER);// cths.getAttributeIntValue(CTableHerolStar.enAttributeName.enAN_Leader);
                            int nCurLead = CardQualityUpdatePanel.GetValue((float)lead, (int)lStar, (int)lLv, nFactor);

                            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_VIOLENCE);// cths.getAttributeIntValue(CTableHerolStar.enAttributeName.enAN_Violence);
                            int nCurViolence = CardQualityUpdatePanel.GetValue((float)crit, (int)lStar, (int)lLv, nFactor);

                            uint ad = 0; //远程为0，近战为1  暂无

                            //int skill1 = Convert.ToInt32(cd.getAttributeStringValue(CardData.enAttributeName.enAN_Skill01));
                            int skill2 = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL2_TYPEID);// Convert.ToInt32(cd.getAttributeStringValue(CardData.enAttributeName.enAN_Skill02));

                            int SNum = 1;

                            if (skill2 != 0)
                            {
                                SNum = 2;
                            }

                            combatValue += (int)(1 * nCurHP + 40 * nCurAtc + 20 * nCurDef + 30 * nCurLead + 10 * nCurViolence) * (int)(1 + 0.04 * ad)
                            * (int)(1 + 0.02 * SNum + 0.01 * 1 + 0.02 * 2);

                        }               
                    }
                     
                }

            }

            UILabel number = PanelTools.Find<UILabel>(EnemyInfo, "number");
            number.text = EnemyHeroList.Count.ToString() + "/5";

            UILabel power = PanelTools.Find<UILabel>(EnemyInfo, "power");
            power.text = combatValue.ToString();
        }

        public void ClearEnemyGround()
        {
            foreach (Transform p in EnemyGround.transform)
            {
                UILabel armyLabel = PanelTools.Find<UILabel>(p.gameObject, "armyLabel");

                if (armyLabel != null)
                {
                    armyLabel.text = "0";
                }
                
                foreach (Transform child in p)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        List<GameObject> positions = new List<GameObject>();
        
        public void ShowArrow(int armyType,int position)
        {
            if (positions.Count > 0)
            {
                ClearArrow();
                positions.Clear();
            }
            
            GetFightPosition(position);

            foreach (GameObject go in positions)
            {
                UILabel type = PanelTools.Find<UILabel>(go, "armyLabel");
                UISprite sprite = PanelTools.Find<UISprite>(go, "arrow");
                int enemyType = int.Parse(type.text);

                if (enemyType != 0)
                {
					DataMgr.BattleUIData.Effect result = DataManager.getBattleUIData().Compare(armyType - 1, enemyType - 1);

					if (result == DataMgr.BattleUIData.Effect.NA)
                    {
                        sprite.spriteName = "";
                        sprite.gameObject.SetActive(false);
                    }
					else if (result < DataMgr.BattleUIData.Effect.NA)
                    {
                        sprite.spriteName = "GreenArrow";
                        sprite.gameObject.SetActive(true);
                    }
					else if (result > DataMgr.BattleUIData.Effect.NA)
                    {
                        sprite.spriteName = "RedArrow";
                        sprite.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void ClearArrow()
        {
            foreach (GameObject go in positions)
            {
                UISprite arrowSprite = PanelTools.Find<UISprite>(go, "arrow");
                if (arrowSprite != null)
                {
                    arrowSprite.gameObject.SetActive(false);
                }
            }
        }

        private void GetFightPosition(int position)
        {
            if (position < 4)
            {
                positions.Add(UISoldierPanel.findChild(EnemyGround, "1"));
                positions.Add(UISoldierPanel.findChild(EnemyGround, "2"));
                positions.Add(UISoldierPanel.findChild(EnemyGround, "3"));
                return;
            }
            
            if (position < 7)
            {
                positions.Add(UISoldierPanel.findChild(EnemyGround, "4"));
                positions.Add(UISoldierPanel.findChild(EnemyGround, "5"));
                positions.Add(UISoldierPanel.findChild(EnemyGround, "6"));
                return;
            }
            
            if (position < 10)
            {
                positions.Add(UISoldierPanel.findChild(EnemyGround, "7"));
                positions.Add(UISoldierPanel.findChild(EnemyGround, "8"));
                positions.Add(UISoldierPanel.findChild(EnemyGround, "9"));
                return;
            }
        }

        public void SetFieldId(int id, int cbBuy = 0)
        {
            EnemyHeroList.Clear();
            
			foreach (DataMgr.BattleUIData.BattleEnemy be in DataManager.getBattleUIData().battleEnemyList)
            {
                if (be.nBattlefieldID == id)
                {
                    EnemyHero eh = new EnemyHero();

                    eh.nHeroLevel = be.nHeroLevel;
                    eh.nHeroType = be.nHeroType;
                    eh.nLocation = be.nLocation;
                    eh.nSoldierLevel = be.nSoldierLevel;
                    eh.nSoldierNum = be.nSoldierNum;
                    eh.nHeroStar = be.nHeroStar;
                    eh.nSoldierArmyLevel = be.nSoldierArmyLevel;

                    if (be.nHeroSkill1 != 0)
                    {
                        EnemyHero.SkillInfo skill = new EnemyHero.SkillInfo();
                        skill.nId = be.nHeroSkill1;
                        skill.nLevel = be.nHeroSkill1Level;
                        eh.addSkill(skill);
                    }

                    if (be.nHeroSkill2 != 0)
                    {
                        EnemyHero.SkillInfo skill = new EnemyHero.SkillInfo();
                        skill.nId = be.nHeroSkill2;
                        skill.nLevel = be.nHeroSkill2Level;
                        eh.addSkill(skill);
                    }

                    if (be.nHeroSkill3 != 0)
                    {
                        EnemyHero.SkillInfo skill = new EnemyHero.SkillInfo();
                        skill.nId = be.nHeroSkill3;
                        skill.nLevel = be.nHeroSkill3Level;
                        eh.addSkill(skill);
                    }

                    EnemyHeroList.Add(eh);
                    //Fight.CharacterMgr.me.addEnemyHero(eh);
                }

                m_cbBuy = cbBuy;
            }

            CurFiledId = id;
            CurBattleId = DataManager.getBattleUIData().dic_BattleField[id].nBattleID;
            DataManager.getBattleUIData().m_BattleId = CurBattleId;
            DataManager.getBattleUIData().m_FieldId = CurFiledId;

            string strTitle = DataManager.getBattleUIData().dic_BattleField[id].strFieldName;
            title.text = DataManager.getLanguageMgr().getString(strTitle);

            UILabel name = PanelTools.Find<UILabel>(EnemyInfo, "name");
            string strName = DataManager.getBattleUIData().dic_BattleField[id].strEnemyName;
            name.text = DataManager.getLanguageMgr().getString(strName);
        }

        protected void SetEnemyToCharacterMgr()
        {
			DataManager.getBattleUIData().EnemyHeroList.Clear();
            foreach (EnemyHero eh in EnemyHeroList)
            {
                //Fight.CharacterMgr.me.addEnemyHero(eh);
				DataManager.getBattleUIData().EnemyHeroList.Add(eh);
            }
        }


        public void GroundSwap(int nPosition, uint id)
        {
            HeroItem hi = new HeroItem();
            if (m_dicHeroItem.ContainsKey(id))
            {
                hi = m_dicHeroItem[id];
            }
            else
            {
                return;
            }

            foreach(Transform position in Ground.transform)
            {
               GroundDragItem gdi = position.GetComponent<GroundDragItem>();

               if (gdi.position == nPosition)
                {
                   //ID
                    UILabel idLable = PanelTools.Find<UILabel>(position.gameObject, "idHero");
                   idLable.text = hi.idHero.text;

                   //头像
                   UISprite icon = PanelTools.Find<UISprite>(position.gameObject, "icon");
                   icon.spriteName = hi.icon.spriteName;

                   //部队类型
                   UISprite armyIcon = PanelTools.Find<UISprite>(position.gameObject, "army");
                   armyIcon.spriteName = hi.army.spriteName;
                   UILabel armyLabel = PanelTools.Find<UILabel>(position.gameObject, "armyLabel");
                   armyLabel.text = hi.armyType.text;

                   //英雄等级
                   UILabel levelLable = PanelTools.Find<UILabel>(position.gameObject, "level");
                   levelLable.text = hi.level.text;

                   //英雄品质
                   UISprite quality = PanelTools.Find<UISprite>(position.gameObject, "quality");
                   quality.spriteName = hi.quality.spriteName;

                   //英雄星级
                   UILabel starLable = PanelTools.Find<UILabel>(position.gameObject, "starLable");
                   starLable.text = hi.starLable.text;

                   UISprite starSprite = PanelTools.Find<UISprite>(position.gameObject, "starSprite");
                   starSprite.spriteName = hi.starSprite.spriteName;

                   //士兵数
                   UILabel armsLable = PanelTools.Find<UILabel>(position.gameObject, "arms");
                   armsLable.text = "0";

                   gdi.UpdateInfo();
                   break;
                }
            }

            AssortSoldier();
        }

        public void SetLastGround()
        {

            if (DataManager.getBattleUIData().dicHeroPosition.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<uint, int> kvi in DataManager.getBattleUIData().dicHeroPosition)
            {
                HeroItem hi = new HeroItem();
                
                if (m_dicHeroItem.ContainsKey(kvi.Key))
                {
                    hi = m_dicHeroItem[kvi.Key];
                }

                hi.root = NGUITools.AddChild(Grid, hi.root);
                PutDragItem pdi = hi.root.GetComponent<PutDragItem>();

                foreach(Transform child in Ground.transform)
                {
                    GroundDragItem gdi = child.GetComponent<GroundDragItem>();

                    if (gdi != null && gdi.position == kvi.Value)
                    {
                        pdi.RePutCloneItem(child.gameObject);
                    }
                }

            }
        }

    }
}
