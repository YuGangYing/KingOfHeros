using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataMgr;

namespace UI
{
    public class UIBattlePanel : PanelBase
    {
        public const PanelID id = PanelID.UIBattlePanel;

        GameObject m_root;
        GameObject Hero1;
        GameObject Hero2;
        GameObject Hero3;
        GameObject Hero4;
        GameObject Hero5;
        UISprite blueBar;
        UISprite redBar;
        UILabel redLabel;
        UILabel blueLabel;
        UILabel engryLabel;
        GameObject SkillState;
        GameObject Engry;
        UIScrollBar engryBar;
        UIScrollBar blueScrollBar;

        UISprite blueLight;
        UISprite redLight;
        UISprite engryLight;

        public float nMaxSelfHP = 0;
        public float nMaxEnemyHP = 0;

        public float nCurSelfHP = 0;
        public float nCurEnemyHP = 0;

        List<GameObject> HeroItems = new List<GameObject>();

        public override string GetResPath()
        {
            return "ALineup.prefab";
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

            //UIEventListener.Get(PanelTools.FindChild(m_root, "Retrun")).onClick = OnRetrun;
            UIEventListener.Get(PanelTools.FindChild(m_root, "SkipBtn")).onClick = OnRetrun;

            GameObject Heros = UISoldierPanel.findChild(m_root, "Heros");
            Hero1 = UISoldierPanel.findChild(Heros, "Hero1");
            Hero2 = UISoldierPanel.findChild(Heros, "Hero2");
            Hero3 = UISoldierPanel.findChild(Heros, "Hero3");
            Hero4 = UISoldierPanel.findChild(Heros, "Hero4");
            Hero5 = UISoldierPanel.findChild(Heros, "Hero5");

            HeroItems.Clear();
            
            HeroItems.Add(Hero1);
            HeroItems.Add(Hero2);
            HeroItems.Add(Hero3);
            HeroItems.Add(Hero4);
            HeroItems.Add(Hero5);

            GameObject Blood = UISoldierPanel.findChild(m_root, "Blood");
            blueBar = PanelTools.Find<UISprite>(Blood, "blueBar");
            blueScrollBar = PanelTools.Find<UIScrollBar>(Blood, "blueScrollBar");
            redBar = PanelTools.Find<UISprite>(Blood, "redBar");
            redLabel = PanelTools.Find<UILabel>(Blood, "redLabel");
            blueLabel = PanelTools.Find<UILabel>(Blood, "blueLabel");

            SkillState = UISoldierPanel.findChild(m_root, "SkillState");
            Engry = UISoldierPanel.findChild(m_root, "Engry");
            engryLabel = PanelTools.Find<UILabel>(Engry, "engryLabel");
            engryBar = PanelTools.Find<UIScrollBar>(Engry, "EngryScrollBar");
            engryLight = PanelTools.Find<UISprite>(Engry, "engryLight");

            blueLight = PanelTools.Find<UISprite>(Blood, "blueLight");
            redLight = PanelTools.Find<UISprite>(Blood, "redLight");

            initHero();
        }

        protected void OnRetrun(GameObject go)
        {
            DataManager.getBattleUIData().SendBattlePveEndMsg();
            
            DataManager.getBattleUIData().battleResult.SetVisible(true);
        }

        public List<int> heroID = new List<int>();

        private void initHero()
        {

            ClearHeroItems();

			foreach (KeyValuePair<uint, DataMgr.BattleUIData.FightHeroInfo> kvi in DataManager.getBattleUIData().dicFightHero)
            {
                foreach (GameObject hero in HeroItems)
                {
                    UILabel id = PanelTools.Find<UILabel>(hero, "idHero");

                    if (0 == int.Parse(id.text))
                    {
                        id.text = kvi.Key.ToString();
                        UISprite icon = PanelTools.Find<UISprite>(hero, "heroimg");
                        icon.spriteName = kvi.Value.icon;

                        //兵种
                        UISprite army = PanelTools.Find<UISprite>(hero, "army");

                        switch (kvi.Value.armyType)
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

                        army.MakePixelPerfect();

                        hero.SetActive(true);
                        break;
                    }
                }
            }

// 			SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Line, DataManager.getBattleUIData().GetSkillTouchLine);
// 			SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Arc, DataManager.getBattleUIData().GetSkillTouchArc);
// 			SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Circle, DataManager.getBattleUIData().GetSkillTouchCircle);
// 			SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_DoubleTap, DataManager.getBattleUIData().GetSkillTouchDoubleTap);
// 			SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_LongTap, DataManager.getBattleUIData().GetSkillTouchLongTap);
//             SimpleTouch.me.enabled = false;
        }

        public void ClearHeroItems()
        {
            foreach (GameObject hero in HeroItems)
            {
                UILabel id = PanelTools.Find<UILabel>(hero, "idHero");

                id.text = "0";

                hero.SetActive(false);
            }
        }

        Vector3 redVector3 = new Vector3(0, 1, 0);
        Vector3 bludVector3 = new Vector3(0, 1, 0);

        public void UpdateSlider()
        {
            if (nMaxSelfHP != 0 && nMaxEnemyHP != 0)
            {
                blueBar.fillAmount = (float)nCurEnemyHP / (float)nMaxEnemyHP;
                redBar.fillAmount = (float)nCurSelfHP / (float)nMaxSelfHP;

                //blueScrollBar.value = blueBar.fillAmount;
                //redScrollBar.value = redBar.fillAmount;


                bludVector3.x = blueBar.fillAmount * 320;
                blueLight.transform.localPosition = bludVector3;
                redVector3.x = redBar.fillAmount * -320;
                redLight.transform.localPosition = redVector3;
            }

            //redLabel.text = nCurSelfHP.ToString() + "/" + nMaxSelfHP.ToString();
            //blueLabel.text = nCurEnemyHP.ToString() + "/" + nMaxEnemyHP.ToString();

            if (nCurSelfHP == nMaxSelfHP || nCurSelfHP == 0)
            {
                //redScrollBar.gameObject.SetActive(false);
                redLight.gameObject.SetActive(false);
            }
            else
            {
                //redScrollBar.gameObject.SetActive(true);
                redLight.gameObject.SetActive(true);
            }

            if (nCurEnemyHP == nMaxEnemyHP || nCurEnemyHP == 0)
            {
                //blueScrollBar.gameObject.SetActive(false);
                blueLight.gameObject.SetActive(false);
            }
            else
            {
                //blueScrollBar.gameObject.SetActive(true);
                blueLight.gameObject.SetActive(true);
            }

        }

        public void UpdateSkillCD(int index, float time, float leftTime)
        {
            SkillState ss = SkillState.GetComponent<SkillState>();

            ss.UpdateSkillCD(index, time, leftTime);
         
        }

        public void ClearSkillCD()
        {
            SkillState ss = SkillState.GetComponent<SkillState>();

            ss.ClearCDTime();
        }

        public void InitSkillCDCover(int nIndex)
        {
            SkillState ss = SkillState.GetComponent<SkillState>();

            ss.InitCDTime(nIndex);
        }

        public void UpdateSkillIcon(int nHero, int nSkill, int nStatus, int nType)
        {
            foreach (GameObject hero in HeroItems)
            {
                SelectHero sh = hero.GetComponent<SelectHero>();

                if (sh.Index == nHero)
                {
                    UISprite skillIcon = null;
                    switch (nSkill)
                    {
                        case 0:
                            skillIcon = PanelTools.Find<UISprite>(hero, "skill1icon");
                            break;
                        case 1:
                            skillIcon = PanelTools.Find<UISprite>(hero, "skill2icon");
                            break;
                        case 2:
                            skillIcon = PanelTools.Find<UISprite>(hero, "skill3icon");
                            break;
                    }

                    if (skillIcon == null)
                    {
                        break;
                    }

                    if (nType == 1)
                    {
                        if (nStatus == 1)
                        {
                            skillIcon.spriteName = "ActiveSkillIcon";
                        }
                        else
                        {
                            skillIcon.spriteName = "ActiveSkillIconGrey";
                        }
                    }

                    if (nType == 2)
                    {
                        if (nStatus == 1)
                        {
                            skillIcon.spriteName = "PassiveSkillIcon";
                        }
                        else
                        {
                            skillIcon.spriteName = "PassiveSkillIconGrey";
                        }
                    }

                    skillIcon.gameObject.SetActive(true);

                }
            }
        }

        Vector3 engryVector3 = new Vector3(0, -1, 0);

        public void SetEngryValue(float nCurValue, float nMaxValue)
        {
            UISprite bar = PanelTools.Find<UISprite>(Engry, "bar");
            bar.fillAmount = (float)nCurValue / (float)nMaxValue;

            //engryLabel.text = nCurValue.ToString() + "/" + nMaxValue.ToString();
            //engryBar.value = bar.fillAmount;

            if (bar.fillAmount > 0.5f)
            {
                engryVector3.x = (bar.fillAmount - 0.5f) * 560;
            }
            else
            {
                engryVector3.x = (bar.fillAmount * 560) - 280;
            }

            engryLight.transform.localPosition = engryVector3;

            if (nCurValue == nMaxValue || nCurValue == 0)
            {
                //engryBar.gameObject.SetActive(false);
                engryLight.gameObject.SetActive(false);
            }
            else
            {
                //engryBar.gameObject.SetActive(true);
                engryLight.gameObject.SetActive(true);

            }
        }

        public void SetHeroHP(int nIndex, int nCurValue, int nMaxValue)
        {
            foreach (GameObject hero in HeroItems)
            {
                SelectHero sh = hero.GetComponent<SelectHero>();

                if (sh.Index == nIndex)
                {
                    UISprite hp = PanelTools.Find<UISprite>(hero, "hp");
                    if (nMaxValue != 0)
                    {
                        hp.fillAmount = (float)nCurValue / (float)nMaxValue;
                    }

                    break;
                }
            }
        }

        public void UpdateHeroItem(Fight.Hero hero)
        {
            foreach (GameObject heroItem in HeroItems)
            {
                UILabel id = PanelTools.Find<UILabel>(heroItem, "idHero");
                SelectHero sh = heroItem.GetComponent<SelectHero>();

                if (int.Parse(id.text) == hero.servId)
                {
                    //血量更新
                    UISprite hp = PanelTools.Find<UISprite>(heroItem, "hp");
                    hp.fillAmount = (float)hero.curHp / (float)hero.maxHp;

                    UILabel hpLabel = PanelTools.Find<UILabel>(heroItem, "hpLabel");
                    hpLabel.text = hero.curHp.ToString() + "/" + hero.maxHp.ToString();

                    //是否死亡 头像处理
                    UISprite heroImg = PanelTools.Find<UISprite>(heroItem, "heroimg");
                    if (hero.curHp <= 0 && sh.isDead == false)
                    {
                        heroImg.color = Color.black;
                        sh.isDead = true;
                    }

                    if (hero.curHp > 0 && sh.isDead == true)
                    {
                        heroImg.color = Color.white;
                        sh.isDead = false;
                    }

                    
                    //技能图标更新
                    //Fight.SkillBase[] skills = hero.skillMgr.getSkillList();

                    //for (int i = 0; i < skills.Length; ++i)
                    //{
                    //    UpdateSkillIcon(sh.Index, i, (int)skills[i].status, (int)skills[i].execMode);
                    //}

                    break;
                }
            }
        }

//         public void UpdateSkillState(Fight.Hero hero)
//         {
//             Fight.SkillBase[] skills = hero.skillMgr.getSkillList();
//             SkillState ss = SkillState.GetComponent<SkillState>();
//             ss.ClearSkillItems();
// 
//             for (int i = 0; i < skills.Length; ++i)
//             {
//                 ss.UpdateSkillIcon(i, skills[i]);
//             }
//         }

        public void SetSkillIcon(int nIndex, string strIcon)
        {
            SkillState ss = SkillState.GetComponent<SkillState>();
            ss.SetSkillIcon(nIndex, strIcon);
        }

        public void SetSkillGesture(int nIndex, string strGesture)
        {
            SkillState ss = SkillState.GetComponent<SkillState>();
            ss.SetGetsure(nIndex, strGesture);
        }

        public void GraySkill()
        {
            SkillState ss = SkillState.GetComponent<SkillState>();
            ss.GrayIcon();
            ClearSkillCD();
        }

        public void GarySkillByIndex(int nIndex)
        {
            SkillState ss = SkillState.GetComponent<SkillState>();
            ss.GrayIconByIndex(nIndex);
        }

        public void RestoreSkill()
        {
            SkillState ss = SkillState.GetComponent<SkillState>();
            ss.RestoreIcon();
        }

        public override void SetVisible(bool value)
        {
            m_root.SetActive(value);
        }


        //血条
        public void UpdateHeroHP(int id, float curHP, float maxHP)
        {
            foreach (GameObject heroItem in HeroItems)
            {
                UILabel idLabel = PanelTools.Find<UILabel>(heroItem, "idHero");
                SelectHero sh = heroItem.GetComponent<SelectHero>();

                if (id == int.Parse(idLabel.text))
                {
                    UISprite hp = PanelTools.Find<UISprite>(heroItem, "hp");
                    hp.fillAmount = curHP / maxHP;

                    UILabel hpLabel = PanelTools.Find<UILabel>(heroItem, "hpLabel");
                    hpLabel.text = curHP.ToString() + "/" + maxHP.ToString();


                    //是否死亡 头像处理
                    UISprite heroImg = PanelTools.Find<UISprite>(heroItem, "heroimg");
                    if (curHP <= 0 && sh.isDead == false)
                    {
                        heroImg.color = Color.black;
                        sh.isDead = true;
                    }

                    if (curHP > 0 && sh.isDead == true)
                    {
                        heroImg.color = Color.white;
                        sh.isDead = false;
                    }
                }
            }
        }

    }

}