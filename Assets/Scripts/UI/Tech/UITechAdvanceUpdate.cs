using UnityEngine;
using System.Collections;
using System;
using DataMgr;
using Packet;

namespace UI
{
	class UITechAdvanceUpdate
	{
        const float starWidth = 0.2f;

        UILabel m_atkBefore = null;
        UILabel m_defBefore = null;
        UILabel m_hpBefore = null;
        UISprite m_starBefore = null;
        UILabel m_nameBefore = null;

        UILabel m_atkAfter = null;
        UILabel m_defAfter = null;
        UILabel m_hpAfter = null;
        UISprite m_starAfter = null;
        UILabel m_nameAfter = null;

        UILabel m_time = null;
        UILabel m_coin = null;
        UILabel m_level = null;
        UILabel m_Special = null;
        UILabel m_SpecialValue = null;
        UIImageButton m_updateBtn = null;
        UILabel m_result = null;
        ARMY_TYPE m_armyType;
		TechItem _curArmyInfo = null;

        public UITechAdvanceUpdate()
        {
            SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyTechAcce, this.onUpdate);
            SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyTechUpdate, this.onUpdate);
        }

        public void init(ARMY_TYPE type)
        {
            m_armyType = type;
        }

        bool onUpdate(SLG.EventArgs obj)
        {
            if (this._curArmyInfo == null || obj == null || obj.m_obj == null)
                return true;

            TechResult ret = (TechResult)obj.m_obj;
            if(ret==null)
                return true;
            if (ret.nTechId != this._curArmyInfo.nId)
                return true;
            DataManager.checkErrcode(ret.nRet);
           return true;
        }

		public void showCurArmyInfo()
        {
			ConfigRow armyInfo = userTechInfo.getArmyLevelCfg(_curArmyInfo.armyType,_curArmyInfo.nLevel);
			if (armyInfo == null)
                return;
            if (m_nameBefore != null)
				m_nameBefore.text = DataManager.getLanguageMgr().getString(armyInfo.getIntValue(CFG_ARMY_LEVEL.NAME_ID));
            if (m_atkBefore != null)
				m_atkBefore.text = armyInfo.getStringValue(CFG_ARMY_LEVEL.ATTACK);
            if (m_defBefore != null)
				m_defBefore.text = armyInfo.getStringValue(CFG_ARMY_LEVEL.DEFENCE);
            if (m_hpBefore != null)
				m_hpBefore.text = armyInfo.getStringValue(CFG_ARMY_LEVEL.HP);
            if (m_starBefore != null)
				m_starBefore.fillAmount = starWidth * _curArmyInfo.nStarLevel;
		}

		public void showNextArmyInfo()
		{
			ConfigRow armyNextLevelInfo  = userTechInfo.getArmyLevelCfg(_curArmyInfo.armyType,_curArmyInfo.nLevel+1);
			if (armyNextLevelInfo == null)
			{
				return;
			}
			if (m_nameAfter != null)
				m_nameAfter.text = DataManager.getLanguageMgr().getString(armyNextLevelInfo.getIntValue(CFG_ARMY_LEVEL.NAME_ID));
			if (m_atkAfter != null)
				m_atkAfter.text = armyNextLevelInfo.getStringValue(CFG_ARMY_LEVEL.ATTACK);
			if (m_defAfter != null)
				m_defAfter.text = armyNextLevelInfo.getStringValue(CFG_ARMY_LEVEL.DEFENCE);
			if (m_hpAfter != null)
				m_hpAfter.text = armyNextLevelInfo.getStringValue(CFG_ARMY_LEVEL.HP);
			if (m_starAfter != null)
				m_starAfter.fillAmount = starWidth * userTechInfo.getArmyStar(_curArmyInfo.nLevel+1);
		}

		public void showTechTime(float fDueTime)
		{
			if(m_updateBtn)
				m_updateBtn.enabled = false;
            if (m_time != null)
            {
                string strTime = UITechPanel.getTimeFormat((int)fDueTime);
                m_time.text = strTime;
            }
		}

		public void showUpdateInfo()
		{
			ConfigRow nextTechInfo = userTechInfo.getArmyTechLevelCfg(_curArmyInfo.armyType,_curArmyInfo.nLevel+1);
			if(nextTechInfo==null)
				return;
			if (m_level != null)
				m_level.text = string.Format(DataManager.getLanguageMgr().getString(29012),nextTechInfo.getStringValue(CFG_TECHNOLOGY.BUILDING_LEVEL));
			if (m_coin != null)
                m_coin.text = string.Format(DataManager.getLanguageMgr().getString(29013), nextTechInfo.getStringValue(CFG_TECHNOLOGY.STONE));
            if (m_time != null)
                m_time.text = string.Format(DataManager.getLanguageMgr().getString(29014), nextTechInfo.getIntValue(CFG_TECHNOLOGY.COST_TIME) / 60);
            int nLevel = DataManager.getBuildData().GetBuildLev(DataMgr.BuildType.SMITHY);
            if (m_Special!=null)
                m_Special.text = string.Format(DataManager.getLanguageMgr().getString(29016),"");
            if (m_SpecialValue != null)
                m_SpecialValue.text = DataManager.getLanguageMgr().getString(nextTechInfo.getIntValue(CFG_TECHNOLOGY.DESC_ID));

			if (nextTechInfo.getIntValue(CFG_TECHNOLOGY.BUILDING_LEVEL) > nLevel)
			{
				if(m_updateBtn)
					m_updateBtn.enabled = false;
			}
 			long lStone = DataManager.getUserData().Data.stone;
			if (lStone < nextTechInfo.getIntValue(CFG_TECHNOLOGY.STONE))
			{
				if (m_coin != null)
					m_coin.color = Color.red;
				if(m_updateBtn)
					m_updateBtn.enabled = false;
			}
			if(m_updateBtn)
				m_updateBtn.enabled = true;
		} 

        public void setRoot(Transform pageObj)
        {
            if (pageObj == null)
                return;

            Transform after = UISoldierPanel.findChild(pageObj.transform, "afterLayer");
            Transform before = UISoldierPanel.findChild(pageObj.transform, "beforeLayer");
            Transform updateInfo = UISoldierPanel.findChild(pageObj.transform, "updateInfo");
            if (after == null || before == null || updateInfo==null)
                return;

            m_nameBefore = UISoldierPanel.setLabelText(before, "name02", "0");
            m_atkBefore = UISoldierPanel.setLabelText(before, "atcNo02", "0");
            m_defBefore = UISoldierPanel.setLabelText(before, "defNo02", "0");
            m_hpBefore = UISoldierPanel.setLabelText(before, "hpNo02", "0");

            Transform stars = UISoldierPanel.findChild(before, "stars02");
            if (stars != null)
                m_starBefore = stars.gameObject.GetComponent<UISprite>();
            if (m_starBefore != null)
                m_starBefore.fillAmount = 0.4f;

            PanelTools.setLabelText(before.gameObject, "hp02", 29008);
            PanelTools.setLabelText(before.gameObject, "atc02", 29009);
            PanelTools.setLabelText(before.gameObject, "def02", 29010);
            PanelTools.setLabelText(after.gameObject, "hp", 29008);
            PanelTools.setLabelText(after.gameObject, "atk", 29009);
            PanelTools.setLabelText(after.gameObject, "def", 29010);
            PanelTools.setLabelText(after.gameObject, "def", 29010);

            m_nameAfter = UISoldierPanel.setLabelText(after, "name", "0");
            m_atkAfter = UISoldierPanel.setLabelText(after, "atcNo", "0");
            m_defAfter = UISoldierPanel.setLabelText(after, "defNo", "0");
            m_hpAfter = UISoldierPanel.setLabelText(after, "hpNo", "0");
            m_level = UISoldierPanel.setLabelText(updateInfo, "level", "Technical college need to be in lv1.");
            m_result = UISoldierPanel.setLabelText(updateInfo, "warn", "");
            m_Special = UISoldierPanel.setLabelText(updateInfo, "updateinfo1", "");
            m_SpecialValue = UISoldierPanel.setLabelText(updateInfo, "updateinfo2", "");

            stars = UISoldierPanel.findChild(after, "stars");
            if (stars != null)
                m_starAfter = stars.gameObject.GetComponent<UISprite>();
            if (m_starAfter != null)
                m_starAfter.fillAmount = 0.8f;

            m_coin = UISoldierPanel.setLabelText(updateInfo, "coin", "MagicStone  0G");
            Transform updateTime = UISoldierPanel.findChild(updateInfo, "time01");
            if (updateTime != null)
            {
                m_time = UISoldierPanel.setLabelText(updateTime, "time", "0 mins");
                m_updateBtn = updateTime.gameObject.GetComponent<UIImageButton>();
                if (m_updateBtn != null)
                {
                    UIEventListener.Get(m_updateBtn.gameObject).onClick = this.onClick;
                }
            }
        }

        public void onClick(GameObject go)
        {
            if(this._curArmyInfo != null)
            {
				if(this._curArmyInfo.nState==2)
				{
					string strText = "update cost 0.20";
					MessageBoxMgr.ShowMessageBox("SpeedUp", strText, onConfirmAcce, null,false);
                }
                else
				{
                    ConfigRow nextTechInfo = userTechInfo.getArmyTechLevelCfg(_curArmyInfo.armyType, _curArmyInfo.nLevel + 1);
                    if (nextTechInfo == null)
                        return;
                    int nLevel = DataManager.getBuildData().GetBuildLev(DataMgr.BuildType.SMITHY);
                    if (nextTechInfo.getIntValue(CFG_TECHNOLOGY.BUILDING_LEVEL) > nLevel)
                    {
                        MessageBoxMgr.ShowConfirm("updateError",
                            string.Format(DataManager.getLanguageMgr().getString(29012), nextTechInfo.getStringValue(CFG_TECHNOLOGY.BUILDING_LEVEL)));
                        return;
                    }
                    long lStone = DataManager.getUserData().Data.stone;
                    if (lStone < nextTechInfo.getIntValue(CFG_TECHNOLOGY.STONE))
                    {
                        MessageBoxMgr.ShowConfirm("updateError",
                            string.Format(DataManager.getLanguageMgr().getString(29013), nextTechInfo.getStringValue(CFG_TECHNOLOGY.STONE)));
                        return;
                    }
                    DataManager.getTechData().update(this._curArmyInfo.nId);
                }
            }
        }
		
        public bool onConfirmAcce(SLG.EventArgs obj)
        {
			if(this._curArmyInfo!=null)
            {
				DataManager.getTechData().upadteAcce(this._curArmyInfo.nId, 0);
                return true;
            }
            return false;
        }

        public void update()
        {
			TechItem techItem = userTechInfo.getUserArmyInfo(m_armyType);
			if(techItem==null)
				return;
			if(this._curArmyInfo==null || 
               this._curArmyInfo.armyType != techItem.armyType||
			   this._curArmyInfo.nLevel!= techItem.nLevel ||
			   this._curArmyInfo.nState != techItem.nState)
			{
				this._curArmyInfo = techItem.Clone();
				this.showCurArmyInfo();
				this.showNextArmyInfo();
                this.showUpdateInfo();
			}

			uint deadTime = DataManager.getQueueData().GetDueTime(DataMgr.QueueData.QUEUE_TYPE.TYPE_TECHNOLOGY);
            float fDueTime = (float)DataManager.getTimeServer().EstimateServerTime((long)deadTime);
            if (fDueTime > 0)
            {
                if (techItem.nState == 1)
                    showMsg("list not empty");
                else
                    showTechTime(fDueTime);
            }
            else
                showMsg("");
        }

        private void showMsg(string msg,bool bRed = true)
        {
            if (m_result != null)
            {
                if (bRed)
                    m_result.color = Color.red;
                else
                    m_result.color = Color.green;
                m_result.text = msg;
            }
        }
	}
}
