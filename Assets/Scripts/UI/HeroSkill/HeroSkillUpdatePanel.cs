using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
//-
namespace UI
{
    class HeroSkillUpdatePanel : PanelBase
	{
        public const PanelID id = PanelID.HeroSkillUpdatePanel;

        UISkillUpdate m_updateSkill = new UISkillUpdate();
            
        public HeroSkillUpdatePanel()
        {
        }
        
        protected override void Initimp(List<GameObject> prefabs)
        {
            m_updateSkill.setRoot(Root);
            UISoldierPanel.setBtnFunc(this.Root.transform, "HeroSkill_Upgrade,close", onClose);
            SetVisible(false);
        }

        protected override void onShow()
        {
            base.onShow();
            if (!m_updateSkill.init())
                onClose(null);
        }

        public void updateSkill(int nHeroId,int nSkillId)
        {
            m_updateSkill.nHeroId = nHeroId;
            m_updateSkill.nSkillid = nSkillId;
        }
        
        protected override void onHide()
        {
            base.onHide();
            m_updateSkill.reset();
        }
        
        public override PanelID GetPanelID()
        {
            return id;
        }

        public override string GetResPath()
        {
            return "HeroSkill.prefab";
        }
        
        public void onClose(GameObject obj)
        {
            this.SetVisible(false);
            m_updateSkill.reset();
            CardIllustratedDetailPanel chdp = (CardIllustratedDetailPanel)PanelManage.me.getPanel(PanelID.CardIllustratedDetailPanel);
            chdp.SetVisible(true);
        }
	}
}
