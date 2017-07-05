using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
namespace UI
{
    public class CollegeUpgradePanel : BuildUpgradePanelBase
    {
        public const PanelID id = PanelID.mainCityUpgradePanel;

        public override string GetResPath()
        {
            return "CollegeUpgradeUI_800x480.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        UILabel m_levelLabel;
        UISprite m_levelBar;

        protected override void Initimp(List<GameObject> prefabs)
        {
            base.Initimp(prefabs);
            m_levelLabel = PanelTools.FindChild(Root, "level").GetComponent<UILabel>();
            m_levelBar = PanelTools.FindChild(Root, "levelBar").GetComponent<UISprite>();

            SetVisible(false);
        }

        public override void ShowPanel(Build build)
        {
            base.ShowPanel(build);
            m_levelLabel.text = m_build.m_cbLev.ToString() + "/" + m_config.maxLevel;
            m_levelBar.fillAmount = m_build.m_cbLev / (float)m_config.maxLevel;
        }
    }
}
