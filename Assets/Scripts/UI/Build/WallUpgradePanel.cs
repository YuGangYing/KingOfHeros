using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
#pragma warning disable 0114
#pragma warning disable 3021
namespace UI
{
    public class WallUpgradePanel : BuildUpgradePanelBase
    {
        public const PanelID id = PanelID.WallUpgradePanel;

        public override string GetResPath()
        {
            return "wallUpgradeUI.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        UILabel m_amountLabel;
        UILabel m_attackLabel;
        UIProgressBar m_amountBar;
        UIProgressBar m_attackBar;

        UILabel barName;
        UILabel atkName;

        protected override void Initimp(List<GameObject> prefabs)
        {
            base.Initimp(prefabs);
            m_amountLabel = PanelTools.FindChild(Root, "barNO").GetComponent<UILabel>();
            m_attackLabel = PanelTools.FindChild(Root, "atkNO").GetComponent<UILabel>();
            m_amountBar = PanelTools.FindChild(Root, "amountBar").GetComponent<UIProgressBar>();
            m_attackBar = PanelTools.FindChild(Root, "attackBar").GetComponent<UIProgressBar>();

            barName = PanelTools.FindChild(Root, "barName").GetComponent<UILabel>();
            barName.text = DataManager.getLanguageMgr().getString(barName.text);

            atkName = PanelTools.FindChild(Root, "atkName").GetComponent<UILabel>();
            atkName.text = DataManager.getLanguageMgr().getString(atkName.text);

            SetVisible(false);
        }

        public override void ShowPanel(Build build)
        {
            base.ShowPanel(build);

			int count = DataManager.getBuildData().GetBuildCountByType((DataMgr.BuildType)m_build.m_idBuildingType);
            int maxCount = m_config.maxCout;
            m_amountLabel.text = count.ToString() + "/" + maxCount.ToString();

            if (maxCount == 0)
            {
                m_amountBar.value = 0;
            }
            else
            {
                m_amountBar.value = count / (float)maxCount;
            }

            int attack = m_config.levels[m_build.m_cbLev].data[0];
            int maxAttack = m_config.levels[m_config.levels.Length - 1].data[0];
            m_attackLabel.text = attack.ToString() + "/" + maxAttack.ToString();

            if (maxAttack == 0)
            {
                m_attackBar.value = 0;
            }
            else
            {
                m_attackBar.value = attack / (float)maxAttack;
            }
        }
    }
}
