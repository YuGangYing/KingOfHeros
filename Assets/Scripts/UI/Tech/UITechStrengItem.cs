using System;
using System.Collections.Generic;
using UnityEngine;
using Packet;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
namespace UI
{
	class UITechStrengItem
	{
        ARMY_TYPE m_armyType;
		int m_techId = 0;
        int m_nTechTypeId = 0;
        bool m_bLearn = false;
        ConfigRow _curStrengCfg;

        GameObject m_root = null;
        UILabel m_effectLbl = null;
        UILabel m_namelLbl = null;
        UILabel m_levelLbl = null;
        UILabel m_timeLbl = null;
        UILabel m_costLbl = null;
        UILabel m_processLbl = null;
        UISprite m_process = null;
        UIImageButton m_updateBtn = null;
        UILabel m_result = null;
		TechItem _curStrengInfo = null;
        
        public UITechStrengItem(int nType)
        {
            m_nTechTypeId = nType;
            SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyTechLearn, this.onUpdate);
            SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyTechAcce, this.onUpdate);
            SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyTechUpdate, this.onUpdate);
        }

        bool onUpdate(SLG.EventArgs obj)
        {
            if (obj == null || obj.m_obj == null)
                return true;

            TechResult ret = (TechResult)obj.m_obj;
            if(ret==null)
                return true;
            if (ret.nTechId != this._curStrengInfo.nTechId)
                return true;
            if (ret.nRet != 0)
            {
                MessageBoxMgr.ShowConfirm("Failed","Errcode="+ret.nRet.ToString() + ":" + DataManager.getLanguageMgr().getString((int)ret.nRet));
            }
           return true;
        }

        public void setRoot(GameObject root)
        {
            m_root = root;
            if (root == null)
                return;

            Transform detail = UISoldierPanel.findChild(root.transform,"StrenthenItem1,detailsNo");
            if(detail!=null)
            {
                m_effectLbl = UISoldierPanel.setLabelText(detail,"effectNo","");
                m_namelLbl = UISoldierPanel.setLabelText(detail,"nameValue","");
                m_levelLbl = UISoldierPanel.setLabelText(detail,"lvrNo","");
                m_timeLbl = UISoldierPanel.setLabelText(detail, "effectNo", ""); 
                m_costLbl = UISoldierPanel.setLabelText(detail,"cost","");
                m_result = UISoldierPanel.setLabelText(detail, "ErrMsg", "");
            }
            m_processLbl = UISoldierPanel.setLabelText(root.transform, "StrenthenItem1,strengthenBarLayer,processNo", "");

            Transform processBar = UISoldierPanel.findChild(root.transform, "StrenthenItem1,strengthenBarLayer,strengthenBar");
            if (processBar != null)
                m_process = processBar.gameObject.GetComponent<UISprite>();

            Transform updateTime = UISoldierPanel.findChild(root.transform, "timeBtn,timeBtnchilde");
            if (updateTime != null)
            {
                m_timeLbl = UISoldierPanel.setLabelText(updateTime, "time", "0 mins");
                m_updateBtn = updateTime.gameObject.GetComponent<UIImageButton>();
            }
            if (m_updateBtn != null)
            {
                UIEventListener.Get(m_updateBtn.gameObject).onClick = this.onClick;
            }
        }
        
        public void update()
        {
            TechItem item = userTechInfo.getUserStrengInfo(this.m_nTechTypeId);
            if (item == null)
            {
                if (this._curStrengCfg == null)
                {
                    this._curStrengCfg = userTechInfo.getStrengTechCfg(this.m_nTechTypeId, 1);
                    showUpdateInfo();
                }
            }
            else
            {
                if (_curStrengInfo == null ||
                    _curStrengInfo.nLevel != item.nLevel ||
                    _curStrengInfo.nState != item.nState)
                {
                    this._curStrengInfo = item.Clone();
                    this._curStrengCfg = userTechInfo.getStrengTechCfg(this.m_nTechTypeId, item.nLevel);
                    showUpdateInfo();
                }
            }

            uint deadTime = DataManager.getQueueData().GetDueTime(DataMgr.QueueData.QUEUE_TYPE.TYPE_TECHNOLOGY);
            float fDueTime = (float)DataManager.getTimeServer().EstimateServerTime((long)deadTime);
            if (fDueTime > 0)
            {
                if(_curStrengInfo!=null && _curStrengInfo.nState ==2)
                {
                    if (m_costLbl != null)
                        m_costLbl.text = "0.20元/分钟";
                    if (m_timeLbl != null)
                        m_timeLbl.text = UITechPanel.getTimeFormat((int)fDueTime);
                }
                else
                    showMsg("list not empty");
            }
            else
                showMsg("");
        }
        
        private void showMsg(string msg, bool bRed = true)
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

        void showUpdateInfo()
        {
            if (this._curStrengCfg == null)
                return;
            int nCurLv = 0;
            if(_curStrengInfo==null)//没有该技能
                nCurLv = 0;
            else
                nCurLv = _curStrengInfo.nLevel;

            m_levelLbl.text = nCurLv + " --> " + (nCurLv + 1);
            if (m_processLbl != null)
                m_processLbl.text = nCurLv + "/25" + "-->" + (nCurLv+1) + "/25";
            if (m_process != null)
                m_process.fillAmount = (float)(nCurLv / 25.0f);

            if (m_namelLbl != null)
                m_namelLbl.text = DataManager.getLanguageMgr().getString(_curStrengCfg.getStringValue(CFG_TECHNOLOGY.SERVICE_ID));
            if (m_costLbl != null)
                m_costLbl.text = _curStrengCfg.getIntValue(CFG_TECHNOLOGY.STONE) + " G";
            if (m_timeLbl != null)
                m_timeLbl.text = _curStrengCfg.getIntValue(CFG_TECHNOLOGY.COST_TIME)/60 + "mins";
            if (m_effectLbl != null)
            {
                string strTemp = "";
                if (_curStrengCfg.getIntValue(CFG_TECHNOLOGY.ATTACK_BOUNS) > 0)
                    strTemp += "ATK  +" + _curStrengCfg.getIntValue(CFG_TECHNOLOGY.ATTACK_BOUNS) + "%";
                if (_curStrengCfg.getIntValue(CFG_TECHNOLOGY.DEFENCE_BOUNS) > 0)
                    strTemp += "DEF  +" + _curStrengCfg.getIntValue(CFG_TECHNOLOGY.DEFENCE_BOUNS) + "%";
                if (_curStrengCfg.getIntValue(CFG_TECHNOLOGY.HP_BOUNS) > 0)
                    strTemp += "HP   +" + _curStrengCfg.getIntValue(CFG_TECHNOLOGY.HP_BOUNS);
                m_effectLbl.text = strTemp;
            }
        }

        void onClick(GameObject go)
        {
            if(this._curStrengInfo==null)
                DataManager.getTechData().learn(m_nTechTypeId);
            else
            {
                if(this._curStrengInfo.nState==2)
                {
                    string strText = "提升每分钟消耗人民币0.20元";
                    MessageBoxMgr.ShowMessageBox("SpeedUp", strText, onAcceConfirm, null, false);
                }
                else
                    //升级
                    DataManager.getTechData().update(this._curStrengInfo.nId);
            }
        }

        bool onAcceConfirm(SLG.EventArgs obj)
        {
            DataManager.getTechData().upadteAcce(this._curStrengInfo.nId, 0);
            return true;
        }
	}
}
