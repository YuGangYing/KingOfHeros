using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;
//-
namespace UI
{
    public class StoreUpgradePanel : BuildUpgradePanelBase
    {
        public const PanelID id = PanelID.StoreUpgradePanel;

        public override string GetResPath()
        {
            return "StoreUpgrade.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        UILabel m_goldCoinsLabel;
        UILabel m_magicStoneLabel;
        UILabel goldLabel;
        UILabel magicLabel;

        UIProgressBar m_goldCoinsBar;
        UIProgressBar m_magicStoneBar;

        protected override void Initimp(List<GameObject> prefabs)
        {
            base.Initimp(prefabs);
            m_goldCoinsLabel = PanelTools.FindChild(Root, "goldCoins").GetComponent<UILabel>();
            m_magicStoneLabel = PanelTools.FindChild(Root, "magicStone").GetComponent<UILabel>();
            m_goldCoinsBar = PanelTools.FindChild(Root, "goldCoinsBar").GetComponent<UIProgressBar>();
            m_magicStoneBar = PanelTools.FindChild(Root, "magicStoneBar").GetComponent<UIProgressBar>();

            goldLabel = PanelTools.FindChild(Root, "goldStore").GetComponent<UILabel>();
            string strContent = DataManager.getLanguageMgr().getString("663500022");
            goldLabel.text = strContent;

            magicLabel = PanelTools.FindChild(Root, "magicLabel").GetComponent<UILabel>();
            magicLabel.text = DataManager.getLanguageMgr().getString(magicLabel.text);

            SetVisible(false);
        }

        public override void ShowPanel(Build build)
        {
            base.ShowPanel(build);
            int nLevel = DataManager.getBuildData().GetLordHallLevel();

            int gold = m_config.levels[m_build.m_cbLev].data[0];
            int magicStone = m_config.levels[m_build.m_cbLev].data[1];
            int maxGold = m_config.levels[nLevel].data[0];
            int maxMagicStone = m_config.levels[nLevel].data[1];

            m_goldCoinsLabel.text = gold.ToString() + "/" + maxGold.ToString();
            if (maxGold == 0)
            {
                m_goldCoinsBar.value = 0;
            }
            else
            {
                m_goldCoinsBar.value = gold / (float)maxGold;
            }

            m_magicStoneLabel.text = magicStone.ToString() + "/" + maxMagicStone.ToString();
            if (maxMagicStone == 0)
            {
                m_magicStoneBar.value = 0;
            }
            else
            {
                m_magicStoneBar.value = magicStone / (float)maxMagicStone;
            }
        }
    }
}
