using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;

//Line End
namespace UI
{
    public abstract class BuildUpgradePanelBase : PanelBase
    {
        public const PanelID id = PanelID.Null;

        public override string GetResPath()
        {
            return "";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        protected void OnClose(GameObject go)
        {
            SetVisible(false);
        }

        public Build m_build;
        protected void OnUpGradeClick(GameObject go)
        {
//             Packet.BUILDING_RET ret = PutBuild.me.CheckBuildOperate((int)m_build.m_idBuildingType, m_build.m_cbLev + 1);
//             if (ret == Packet.BUILDING_RET.BUILDING_RET_SUC)
//             {
// 				DataManager.getBuildData().SendBuildUplev(m_build.m_idBuilding);
//             }
//             else
//             {
// 				DataManager.getBuildData().ShowBuildOperaRetText(ret);
//             }

            DataManager.getBuildData().SendBuildUplev(m_build.m_idBuilding);

            SetVisible(false);
        }

        UIButton m_upgradeBtn;
        UILabel m_upgradeLabel;
        UILabel m_nameLabel;
        UILabel m_goldLabel;
        UILabel m_timeLabel;

        UILabel timeName;
        UILabel goldName;
        UILabel effectDec;
        UILabel effectLabel;

        protected override void Initimp(List<GameObject> prefabs)
        {
            GameObject btn = PanelTools.FindChild(Root, "upgrade");
            UIEventListener.Get(btn).onClick = OnUpGradeClick;
            UIEventListener.Get(PanelTools.FindChild(Root, "close")).onClick = OnClose;

            m_upgradeBtn = btn.GetComponentInChildren<UIButton>();
            m_upgradeLabel = btn.GetComponentInChildren<UILabel>();
            m_nameLabel = PanelTools.FindChild(Root, "buildName").GetComponent<UILabel>();
            m_goldLabel = PanelTools.FindChild(Root, "goldNo").GetComponent<UILabel>();
            m_timeLabel = PanelTools.FindChild(Root, "timeNo").GetComponent<UILabel>();

            timeName = PanelTools.FindChild(Root, "timeLabel").GetComponent<UILabel>();
            goldName = PanelTools.FindChild(Root, "goldLabel").GetComponent<UILabel>();

            timeName.text = DataManager.getLanguageMgr().getString(timeName.text);
            goldName.text = DataManager.getLanguageMgr().getString(goldName.text);

            effectDec = PanelTools.FindChild(Root, "effectDescription").GetComponent<UILabel>();
            effectDec.text = DataManager.getLanguageMgr().getString(effectDec.text);

            effectLabel = PanelTools.FindChild(Root, "effectLabel").GetComponent<UILabel>();
            effectLabel.text = DataManager.getLanguageMgr().getString(effectLabel.text);
        }

        public DataMgr.BuildConfig m_config;

        public virtual void ShowPanel(Build build)
        {
            m_build = build;
            //m_config = CsvConfigMgr.me.GetBuildConfig((int)m_build.m_idBuildingType);
            m_config = DataManager.getBuildData().GetBuildConfig((int)m_build.m_idBuildingType);
            m_nameLabel.text = m_config.name.ToString();
            m_nameLabel.text = DataManager.getLanguageMgr().getString(m_nameLabel.text);
            m_nameLabel.text += " LV " + m_build.m_cbLev.ToString();

            if (m_build.m_cbLev < m_config.levels.Length - 1)
            {
                 m_upgradeBtn.isEnabled = true;
                 m_timeLabel.text = m_config.levels[m_build.m_cbLev + 1].time.ToString() + "S";
                 string strPrice = "";
                 if (m_config.levels[m_build.m_cbLev + 1].money > 0)
                     strPrice += m_config.levels[m_build.m_cbLev + 1].money.ToString() + " Coin  ";
                 if (m_config.levels[m_build.m_cbLev + 1].magicStone > 0)
                     strPrice += m_config.levels[m_build.m_cbLev + 1].magicStone.ToString() + " Aureustone ";
                 m_goldLabel.text = strPrice;

                m_upgradeLabel.text = "663500013";
                 m_upgradeLabel.text = DataManager.getLanguageMgr().getString(m_upgradeLabel.text);

            }
            else
            {
                m_upgradeBtn.UpdateColor(false);
                m_upgradeBtn.isEnabled = false;
                m_upgradeLabel.text = "663500010";
                m_upgradeLabel.text = DataManager.getLanguageMgr().getString(m_upgradeLabel.text);
                m_timeLabel.text = "0S";
                m_goldLabel.text = "0G";
            }

            SetVisible(true);
        }
    }
}
