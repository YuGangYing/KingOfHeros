using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;
//Line End
namespace UI
{
    public class MagicStoneUpgradePanel : BuildUpgradePanelBase
    {
        public const PanelID id = PanelID.MagicStoneUpgradePanel;

        public override string GetResPath()
        {
            return "MagicStoneUpgradeUI.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        UILabel m_producLabel;
        UIProgressBar m_pb;
        UILabel m_limitLabel;
        UIProgressBar m_pbLimit;
        UILabel barName;
        UILabel limitName;

        protected override void Initimp(List<GameObject> prefabs)
        {
            base.Initimp(prefabs);
            m_producLabel = PanelTools.FindChild(Root, "barNO").GetComponent<UILabel>();
            m_pb = PanelTools.FindChild(Root, "valueBar").GetComponent<UIProgressBar>();

            m_limitLabel = PanelTools.FindChild(Root, "limitNO").GetComponent<UILabel>();
            m_pbLimit = PanelTools.FindChild(Root, "limitBar").GetComponent<UIProgressBar>();

            barName = PanelTools.FindChild(Root, "barName").GetComponent<UILabel>();
            barName.text = DataManager.getLanguageMgr().getString(barName.text);

            limitName = PanelTools.FindChild(Root, "limitName").GetComponent<UILabel>();
            limitName.text = DataManager.getLanguageMgr().getString(limitName.text);

            SetVisible(false);
        }

        public override void ShowPanel(Build build)
        {
            base.ShowPanel(build);

            int produc = m_config.levels[m_build.m_cbLev].data[0];
            int maxProduc = m_config.levels[m_config.levels.Length - 1].data[0];
            m_producLabel.text = produc.ToString() + "/H";
            m_pb.value = produc / (float)maxProduc;

            int time = -(int)DataMgr.DataManager.getTimeServer().EstimateServerTime((long)m_build.m_u32LevyTime) / 3600;
            int maxProducTime = m_config.levels[m_config.levels.Length - 1].data[2];

            if (time > maxProducTime)
            {
                time = maxProducTime;
            }

            m_limitLabel.text = (produc * time).ToString() + "/" + (produc * maxProducTime);

            if (maxProducTime == 0)
            {
                m_pbLimit.value = 0;
            }
            else
            {
                m_pbLimit.value = time / maxProducTime;
            }
        }
    }
}
