using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;
//Line End
namespace UI
{
    public class TrainingUpgradePanel : BuildUpgradePanelBase
    {
        public const PanelID id = PanelID.mainCityUpgradePanel;

        public override string GetResPath()
        {
            return "trainingUpgradeUI.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        UILabel m_amountLabel;
        UIProgressBar m_amountBar;
        UILabel barName;

        protected override void Initimp(List<GameObject> prefabs)
        {
            base.Initimp(prefabs);
            barName = PanelTools.FindChild(Root, "barName").GetComponent<UILabel>();
            barName.text = DataManager.getLanguageMgr().getString(barName.text);

            m_amountLabel = PanelTools.FindChild(Root, "barNO").GetComponent<UILabel>();
            m_amountBar = PanelTools.FindChild(Root, "valueBar").GetComponent<UIProgressBar>();

            SetVisible(false);
        }

        public override void ShowPanel(Build build)
        {
            base.ShowPanel(build);

            int amount = m_config.levels[m_build.m_cbLev].data[0];
            int maxAmount = m_config.levels[m_config.levels.Length - 1].data[0];
            m_amountLabel.text = amount.ToString() + "/" + maxAmount.ToString();

            if (maxAmount == 0)
            {
                m_amountBar.value = 0;
            }
            else
            {
                m_amountBar.value = amount / (float)maxAmount;
            }
        }
    }
}
