using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using Packet;
using Network;
using SLG;

namespace HeroSkill
{
    class HeroSkillMgr : Singleton<HeroSkillMgr>
	{
        HeroSkillConfig m_skillCfg = new HeroSkillConfig();
        HeroSkillLevel  m_skillLevelCfg = new HeroSkillLevel();
        HeroSkillStatus m_skillStatus = new HeroSkillStatus();

        onUpdateCall m_callFunc = null;

        public delegate void onUpdateCall(int nRet);

        public HeroSkillConfig skillCfg
        {
            get { return m_skillCfg; }
        }

        public HeroSkillLevel levelCfg
        {
            get { return m_skillLevelCfg; }
        }

        public HeroSkillStatus statusCfg
        {
            get { return m_skillStatus; }
        }

        public HeroSkillMgr()
        {
            init();

            NetworkMgr.me.getClient().RegMsgFunc<MSG_ACQUIRE_SKILL_EXP_RESPONSE>(onUpdate);
        }
        
        private bool init()
        {
            if (!m_skillCfg.loadConfig())
                return false;
            if (!m_skillLevelCfg.loadConfig())
                return false;
            if (!m_skillStatus.loadConfig())
                return false;
            return true;
        }

        public void updateSkill(int HeroId, int nSkillId)
        {
            HeroSkillUpdatePanel skillUpdate = UI.PanelManage.me.GetPanel<HeroSkillUpdatePanel>(PanelID.HeroSkillUpdatePanel);
            if (skillUpdate != null)
            {
                skillUpdate.updateSkill(HeroId, nSkillId);
                skillUpdate.SetVisible(true);
            }
        }

        public void sendUpdate(int nHeroId, int nSkillId, int nViceHeroId,onUpdateCall callFunc)
        {
            if (callFunc != null)
                m_callFunc = callFunc;
            MSG_ACQUIRE_SKILL_EXP_REQUEST request = new MSG_ACQUIRE_SKILL_EXP_REQUEST();
            request.idMajorHero = (uint)nHeroId;
            request.idSkill = (uint)nSkillId;
            request.idViceHero = (uint)nViceHeroId;
            NetworkMgr.me.getClient().Send(ref request);
        }

        private void onUpdate(ushort id, object ar)
        {
            MSG_ACQUIRE_SKILL_EXP_RESPONSE rsponse = (MSG_ACQUIRE_SKILL_EXP_RESPONSE)ar;
            if (m_callFunc != null)
                m_callFunc(rsponse.u8Err);
        }
	}
}
