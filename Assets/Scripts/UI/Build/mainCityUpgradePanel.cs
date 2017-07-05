using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;

namespace UI
{
	//Line End
    public class mainCityUpgradePanel : BuildUpgradePanelBase
    {
        public const PanelID id = PanelID.mainCityUpgradePanel;

        public override string GetResPath()
        {
            return "mainCityUpgrade.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        UILabel m_limitLabel;
        UIProgressBar m_pb;
        UILabel barName;

        protected override void Initimp(List<GameObject> prefabs)
        {
            base.Initimp(prefabs);
            m_limitLabel = PanelTools.FindChild(Root, "barNO").GetComponent<UILabel>();
            m_pb = PanelTools.FindChild(Root, "valueBar").GetComponent<UIProgressBar>();

            barName = PanelTools.FindChild(Root, "barName").GetComponent<UILabel>();
            barName.text = DataManager.getLanguageMgr().getString(barName.text);

            SetVisible(false);
        }

        public override void ShowPanel(Build build)
        {
            base.ShowPanel(build);

            m_limitLabel.text = m_build.m_cbLev.ToString() + "/" + m_config.maxLevel;
            m_pb.value = m_build.m_cbLev / (float)m_config.maxLevel;
        }
    }
}
