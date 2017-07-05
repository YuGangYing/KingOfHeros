using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;
//-
namespace UI
{
    public class PubUpgradePanel : BuildUpgradePanelBase
    {
        public const PanelID id = PanelID.PubUpgradePanel;

        public override string GetResPath()
        {
            return "GoldUpgrade.prefab";
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
            int nLevel = DataManager.getBuildData().GetLordHallLevel();

            int produc = m_config.levels[m_build.m_cbLev].data[0];
            int maxProduc = m_config.levels[nLevel].data[0];
            m_producLabel.text = produc.ToString() + " / " + maxProduc.ToString();

            if (maxProduc == 0)
            {
                m_pb.value = 0;
            }
            else
            {
                m_pb.value = produc / (float)maxProduc;
            }

            int time = m_config.levels[m_build.m_cbLev].data[1];
            int maxProducTime = m_config.levels[nLevel].data[1];

            m_limitLabel.text = time.ToString() + " / " + maxProducTime.ToString();

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
