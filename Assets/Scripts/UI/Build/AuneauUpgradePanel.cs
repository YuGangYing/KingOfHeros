using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;
//-
namespace UI
{
    public class AuneauUpgradePanel : BuildUpgradePanelBase
    {
        public const PanelID id = PanelID.AuneauUpgradePanel;

        public override string GetResPath()
        {
            return "AuneauUpUI.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        UILabel m_producLabel;
        UIProgressBar m_pb;
        UILabel barName;

        protected override void Initimp(List<GameObject> prefabs)
        {
            base.Initimp(prefabs);
            m_producLabel = PanelTools.FindChild(Root, "barNO").GetComponent<UILabel>();
            m_pb = PanelTools.FindChild(Root, "valueBar").GetComponent<UIProgressBar>();

            barName = PanelTools.FindChild(Root, "barName").GetComponent<UILabel>();
            barName.text = DataManager.getLanguageMgr().getString(barName.text);

            SetVisible(false);
        }

        public override void ShowPanel(Build build)
        {
            base.ShowPanel(build);

            int produc = m_config.levels[m_build.m_cbLev].data[0];
            int maxProduc = m_config.levels[m_config.levels.Length - 1].data[0];
            m_producLabel.text = produc.ToString() + " / " + maxProduc.ToString();
            m_pb.value = produc / (float)maxProduc;

            int time = -(int)DataMgr.DataManager.getTimeServer().EstimateServerTime((long)m_build.m_u32LevyTime) / 3600;
            int maxProducTime = m_config.levels[m_config.levels.Length - 1].data[2];

            if (time > maxProducTime)
            {
                time = maxProducTime;
            }

        }
    }
}
