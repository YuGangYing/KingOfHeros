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
    public class HouseUpgradePanel : BuildUpgradePanelBase
    {
        public const PanelID id = PanelID.HouseUpgradePanel;

        public override string GetResPath()
        {
            return "HouseUpgradeUI_800x480.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        UILabel m_strengthLabel;
        UISprite m_strengthBar;

        protected override void Initimp(List<GameObject> prefabs)
        {
            base.Initimp(prefabs);
            m_strengthLabel = PanelTools.FindChild(Root, "strength").GetComponent<UILabel>();
            m_strengthBar = PanelTools.FindChild(Root, "strengthBar").GetComponent<UISprite>();

            SetVisible(false);
        }

        public override void ShowPanel(Build build)
        {
            base.ShowPanel(build);

            int amount = m_config.levels[m_build.m_cbLev].data[0];
            int maxAmount = m_config.levels[m_config.levels.Length - 1].data[0];
            m_strengthLabel.text = amount.ToString() + "/" + maxAmount.ToString();
            m_strengthBar.fillAmount = amount / (float)maxAmount;
        }
    }
}
