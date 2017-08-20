using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using HeroSkill;
using DataMgr;
using Packet;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
namespace UI
{
	class UISkillUpdate
	{
        int m_nHeroId = 0;
        int m_nSkillId = 0;

        GameObject m_root;

        UILabel m_costLbl = null;
        UISprite m_processBar = null;
        UILabel m_beforLvLbl = null;
        UILabel m_afterLvLbl = null;
        UILabel m_beforeDesc = null;
        UILabel m_afterDesc = null;
        UILabel m_coinLbl = null;
        UILabel m_stoneLbl = null;
        UILabel m_processLbl = null;
        UIButton m_btnUpdate = null;

        UILabel m_beforeName = null;
        UILabel m_afterName = null;

        UIButton m_selBtn1 = null;
        UIButton m_selBtn2 = null;

        UIChooseHero m_choose = new UIChooseHero();
        UIHeroSelItem m_card1 = new UIHeroSelItem();
        UIHeroSelItem m_card2 = new UIHeroSelItem();
        GameObject m_upgradeProgress;

        class stSKillLevel
        {
            public int nId;
            public int nLevel;
            public int nExp;
        }

        public UISkillUpdate()
        {
        }

        public int nHeroId
        {
            set { m_nHeroId = value; }
        }

        public int nSkillid
        {
            set { m_nSkillId = value; }
        }

        public void setRoot(GameObject root)
        {
            Transform choosPage = UISoldierPanel.findChild(root.transform, "chooseHero");
            if (choosPage!=null)
                m_choose.setRoot(choosPage.gameObject);

            Transform updatePage = UISoldierPanel.findChild(root.transform, "HeroSkill_Upgrade");
            if (updatePage == null)
                return;
            this.m_root = updatePage.gameObject;
            this.show(false);
            m_choose.show(false);

            m_coinLbl = UISoldierPanel.setLabelText(updatePage, "goldCoin,goldNo", "");
            m_stoneLbl = UISoldierPanel.setLabelText(updatePage, "magicstone,stoneNo", "");
            //左边
            Transform effect = UISoldierPanel.findChild(updatePage, "currentEffect");
            if(effect==null)
                return;          
            m_beforLvLbl = UISoldierPanel.setLabelText(effect,"currenLv","");
            m_afterLvLbl = UISoldierPanel.setLabelText(effect,"nextLv","");
            m_beforeName = UISoldierPanel.setLabelText(effect, "Skillname", "");
            m_afterName =  UISoldierPanel.setLabelText(effect, "NextSkillname", "");
            m_beforeDesc = UISoldierPanel.setLabelText(effect, "SkillDetail", "");
            m_afterDesc = UISoldierPanel.setLabelText(effect, "NextSkillDetail", "");

            //右边
            Transform process = UISoldierPanel.findChild(updatePage, "upgradeProgress");
            if (process == null)
                return;

            m_upgradeProgress = process.gameObject;
            m_processLbl = UISoldierPanel.setLabelText(process, "UpgradeProgressNO", "");
            m_costLbl = UISoldierPanel.setLabelText(process, "coldNo", "");
            m_processBar = UISoldierPanel.findChild<UISprite>(process, "UpgradeProgressBar,UpProcessBar");

            m_btnUpdate = UISoldierPanel.setBtnFunc(process, "upgrade,upgradeBtn", update);

            m_card1.setRoot(UISoldierPanel.findChild(process.gameObject,"buttonList,AddCard1"));
            m_card2.setRoot(UISoldierPanel.findChild(process.gameObject,"buttonList,AddCard2"));
            m_card2.show(false);

            m_selBtn1 = UISoldierPanel.setBtnFunc(process, "buttonList,AddCard1", selCard);
            m_selBtn1 = UISoldierPanel.setBtnFunc(process, "buttonList,AddCard1", selCard);
        }

        public bool init()
        {
            ConfigBase skillBase = DataManager.getConfig(CONFIG_MODULE.CFG_SKILL);
            if (skillBase == null)
                return false;

            ConfigRow skill = skillBase.getRow(CFG_SKILL.ID,m_nSkillId);
            if (skill == null)
            {
                MessageBoxMgr.ShowMessageBox("Update", "暂无该技能信息，请等待新版本更新", null, null, false);
                return false;
            }
            stSKillLevel level = this.getHeroSkill();
            if (level == null)
            {
                MessageBoxMgr.ShowMessageBox("Update", "获取英雄技能信息异常", null, null, false);
                return false;
            }
            SkillLevelItem levelItem = HeroSkillMgr.me.levelCfg.getItem(m_nSkillId, level.nLevel);
            if (levelItem == null)
            {
                MessageBoxMgr.ShowMessageBox("Update", "暂无该技能等级信息，请等待新版本更新", null, null, false);
                return false;
            }
            //下一等级信息
            SkillLevelItem nextLevelItem = HeroSkillMgr.me.levelCfg.getItem(m_nSkillId, level.nLevel + 1);

            this.show(true);
            m_choose.show(false);
            m_choose.heroId = m_nHeroId;
            //显示用户金钱
            //用户魔石数量判断
            long lCoin = DataManager.getUserData().Data.coin;
            long lStone = DataManager.getUserData().Data.stone;

            setLabel(m_coinLbl, lCoin.ToString());
            setLabel(m_stoneLbl, lStone.ToString());

            //当前级别信息
            
            setLabel(m_beforLvLbl,"LV " + levelItem.nLevel + " CURRENT EFFECT");
            setLabel(m_beforeName, skill.getStringValue(CFG_SKILL.NAME));
            string strCurEffect = levelItem.nEffect_value.ToString();
            setLabel(m_beforeDesc, strCurEffect);

            if (nextLevelItem != null)
            {
                //下一级别信息展示
                setLabel(m_afterLvLbl, "LV " + nextLevelItem.nLevel + " NEXT EFFECT");
                setLabel(m_afterName, skill.getStringValue(CFG_SKILL.NAME));
                string strNextEffect = nextLevelItem.nEffect_value.ToString();
                setLabel(m_afterDesc, strNextEffect);
                int nMaxExp = nextLevelItem.nExp;
                int nCurExp = level.nExp;
                setLabel(m_processLbl, nCurExp + "/" + nMaxExp);
                if (m_processBar != null && nMaxExp != 0)
                    m_processBar.fillAmount = (nCurExp * 1.0f) / (nMaxExp * 1.0f);

                //升级所需的资源
                setLabel(m_costLbl, "Exp:" + nextLevelItem.nExp);

                //显示升级按钮
                if (m_btnUpdate != null)
                    m_btnUpdate.enabled = (nCurExp >= nMaxExp);
            }
            else
            {
                setLabel(m_afterLvLbl, "没有更高等级信息");
                setLabel(m_costLbl, "不能升级了");
                m_btnUpdate.enabled = false;
            }

            return true;
        }

        stSKillLevel getHeroSkill()
        {            
            //当前等级信息
            HERO_INFO heroInfo = new HERO_INFO();
            if(!DataManager.getHeroData().getItemById((uint)m_nHeroId,ref heroInfo))
                return null;
            //技能1
            long nSkill1 = heroInfo.unSkill1;
            if((int)nSkill1==m_nSkillId)
            {
                stSKillLevel level = new stSKillLevel();
                level.nId = (int)nSkill1;
                level.nLevel = heroInfo.usSkillLvl1;
                level.nExp = (int)heroInfo.unSkillExp1;
                return level;
            }
            //技能2
            long nSkill2 = heroInfo.unSkill2;
            if((int)nSkill2==m_nSkillId)
            {
                stSKillLevel level = new stSKillLevel();
                level.nId = (int)nSkill2;
                level.nLevel = heroInfo.usSkillLvl2;
                level.nExp = (int)heroInfo.unSkillExp2;
                return level;
            }
            //技能3
            long nSkill3 = heroInfo.unSkill3;
            if((int)nSkill3==m_nSkillId)
            {
                stSKillLevel level = new stSKillLevel();
                level.nId = (int)nSkill3;
                level.nLevel = heroInfo.usSkillLvl3;
                level.nExp = (int)heroInfo.unSkillExp3;
                return level;
            }
            return null;
        }

        public void reset()
        {
            m_nHeroId = 0;
            m_nSkillId = 0;
            if (m_processBar != null)
                m_processBar.fillAmount = 0f;

            m_card1.clear();
            m_card2.clear();

            setLabel(m_costLbl, "");
            setLabel(m_beforLvLbl,"");
            setLabel(m_afterLvLbl,"");
            setLabel(m_beforeDesc,"");
            setLabel(m_afterDesc,"");
            setLabel(m_coinLbl,"");
            setLabel(m_stoneLbl,"");
        }

        void setLabel(UILabel lbl,string msg)
        {
            if (lbl != null)
                lbl.text = msg;
        }

        public void selCard(GameObject obj)
        {
            m_choose.selectNum = 1;
            m_choose.addSelHero(m_card1.hero);
            m_choose.addSelHero(m_card2.hero);
            m_choose.init(m_nHeroId, onSelCard);
            m_choose.show(true);
            m_card1.show(false);
            m_upgradeProgress.SetActive(false);
        }

        public void onSelCard(int nCount, selHero[] heroList)
        {
            if (nCount == 1)
            {
                if (heroList[0].nHeroid == m_nHeroId)
                    return;
                m_card1.hero = heroList[0];
            }
            m_card1.show(true);
        }

        public void onUpdate(int nRet)
        {
            //错误提示
            if (DataManager.checkErrcode((uint)nRet))
            {
                m_card1.clear();
                m_card2.clear(); 
                init();//重新初始化
            }
        }

        public void update(GameObject obj)
        {
            if (m_card1.hero == null)
                return;
            HeroSkillMgr.me.sendUpdate(m_nHeroId, m_nSkillId, m_card1.hero.nHeroid,onUpdate);
        }

        public void show(bool bFlag)
        {
            if (m_root != null)
                m_root.active = bFlag;
        }
    }
}
