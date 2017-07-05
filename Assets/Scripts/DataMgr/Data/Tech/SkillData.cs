using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DataMgr
{
	public class SkillData
	{

		private ConfigBase skillcfg;

		public SkillData()
		{

		}

		public bool Init()
		{
			LoadSkillCfg();
			return true;
		}

		public void reload()
		{

		}

		public void release()
		{
			m_SkillConfigList.Clear();
		}


		private Dictionary<int,Skill>  m_SkillConfigList = new Dictionary<int,Skill>();

		private bool LoadSkillCfg()
		{
					
			if (m_SkillConfigList.Count > 0)
				return true;

			skillcfg = DataMgr.DataManager.getConfig(CONFIG_MODULE.CFG_SKILL_CFG);
			if (skillcfg == null)
				return false;

			foreach (ConfigRow row in skillcfg.rows) 
			{
				/*Skill skill = new Skill();
				skill.SkillId =  row.getIntValue(CFG_SKILLCFG.SKILLID);
				skill.name = row.getStringValue(CFG_SKILLCFG.NAME);
				skill.Icon = row.getStringValue(CFG_SKILLCFG.ICON);
				skill.MaxCooldown = row.getFloatValue(CFG_SKILLCFG.MAXCOOLDOWN);
				skill.CurCooldown = skill.MaxCooldown;
				skill.SkillEffectDuration = row.getFloatValue(CFG_SKILLCFG.EFFECTDURATION);
				skill.SkillAnimDuration = row.getFloatValue(CFG_SKILLCFG.ANIMDURATION);
				skill.Cost = row.getFloatValue(CFG_SKILLCFG.COST);
				skill.Damage = row.getFloatValue(CFG_SKILLCFG.DAMAGE); 
				skill.AdditionDamage = row.getFloatValue(CFG_SKILLCFG.ADDITIONDAMAGE); 
				skill.Condition = row.getFloatValue(CFG_SKILLCFG.CONDITION); 
				skill.ConditionValue = row.getFloatValue(CFG_SKILLCFG.CONDITIONVALUE);
				skill.SkillAttackRadius = row.getFloatValue(CFG_SKILLCFG.ATTACKRADIUS); 
				skill.MaxHitCount = row.getIntValue(CFG_SKILLCFG.MAXHITCOUNT); 
				//skill.SkillPrefab = Common.CreateGameObject(row.getStringValue(CFG_SKILLCFG.SKILLPREFAB)); 

				skill.SkillPrefab = Resources.Load<GameObject>(row.getStringValue(CFG_SKILLCFG.SKILLPREFAB)); 
				string path  = row.getStringValue(CFG_SKILLCFG.HITEFFECTCPREFAB);
				Debug.Log("path............................" + path);
				skill.HitEffectPrefab = Resources.Load<GameObject>(row.getStringValue(CFG_SKILLCFG.HITEFFECTCPREFAB));
				skill.CastAudioClip = Resources.Load<AudioClip>(row.getStringValue(CFG_SKILLCFG.CASTAUDIOCLIP)); 
				skill.HitAudioClip = Resources.Load<AudioClip>(row.getStringValue(CFG_SKILLCFG.HITAUDIOCLIP)); 
				skill.Type = (SkillType)row.getIntValue(CFG_SKILLCFG.TYPE);

				//skill.SubSkills = row.getIntValue(CFG_SKILLCFG.SUBSKILL);

				if (row.getIntValue(CFG_SKILLCFG.ISACTIVE) == 1)
					skill.IsActive = true; 
				else
					skill.IsActive = false;

				skill.FingerType = row.getIntValue(CFG_SKILLCFG.FINGERTYPE);
				skill.OperCount = row.getIntValue(CFG_SKILLCFG.OPERCOUNT);*/

				Skill skill;
				getSkill(row,out skill);
				m_SkillConfigList.Add(skill.SkillId,skill);

			}

			return true;
		}

		public void getSkill(ConfigRow row,out Skill skill)
		{
			skill = new Skill();
			skill.Id =  row.getIntValue(CFG_SKILLCFG.ID);
			skill.SkillId = row.getIntValue(CFG_SKILLCFG.SKILLID);
			skill.NameId = row.getIntValue(CFG_SKILLCFG.NAMEID);
			skill.DescId = row.getIntValue(CFG_SKILLCFG.DESCID);
			skill.Name = row.getStringValue(CFG_SKILLCFG.NAME);
			skill.Icon = row.getStringValue(CFG_SKILLCFG.ICON);
			skill.MaxCooldown = row.getFloatValue(CFG_SKILLCFG.MAXCOOLDOWN) / 10;
			skill.CurCooldown = skill.MaxCooldown;
			skill.SkillEffectDuration = row.getFloatValue(CFG_SKILLCFG.EFFECTDURATION);
			skill.SkillAnimDuration = row.getFloatValue(CFG_SKILLCFG.ANIMDURATION);
			skill.Cost = row.getFloatValue(CFG_SKILLCFG.COST) / 10;
			skill.Damage = row.getFloatValue(CFG_SKILLCFG.DAMAGE); 
			skill.AdditionDamage = row.getFloatValue(CFG_SKILLCFG.ADDITIONDAMAGE); 
			skill.Condition = row.getFloatValue(CFG_SKILLCFG.CONDITION); 
			skill.ConditionValue = row.getFloatValue(CFG_SKILLCFG.CONDITIONVALUE);
			skill.SkillAttackRadius = row.getFloatValue(CFG_SKILLCFG.ATTACKRADIUS); 
			//skill.RangeType = row.getIntValue(CFG_SKILLCFG.RANGETYPE);
			//skill.RangeValue1 = row.getFloatValue(CFG_SKILLCFG.RANGEVALUE1);
			//skill.RangeValue2 = row.getFloatValue(CFG_SKILLCFG.RANGEVALUE2);
			skill.MaxHitCount = row.getIntValue(CFG_SKILLCFG.MAXHITCOUNT); 
			//skill.SkillPrefab = Common.CreateGameObject(row.getStringValue(CFG_SKILLCFG.SKILLPREFAB)); 
			
			skill.SkillPrefab = Resources.Load<GameObject>(row.getStringValue(CFG_SKILLCFG.SKILLPREFAB)); 
			//string path  = row.getStringValue(CFG_SKILLCFG.HITEFFECTCPREFAB);
			//Debug.Log("path............................" + path);
			skill.ShootEffectPrefab = Resources.Load<GameObject>(row.getStringValue(CFG_SKILLCFG.SHOOTEFFECTCPREFAB));
			skill.ShootStandByEffectPrefab = Resources.Load<GameObject>(row.getStringValue(CFG_SKILLCFG.SHOOTSTANDBYEFFECTCPREFAB));
			skill.HitEffectPrefab = Resources.Load<GameObject>(row.getStringValue(CFG_SKILLCFG.HITEFFECTCPREFAB));
			skill.CastAudioClip = Resources.Load<AudioClip>(row.getStringValue(CFG_SKILLCFG.CASTAUDIOCLIP)); 
			skill.ShootAudioClip = Resources.Load<AudioClip>(row.getStringValue(CFG_SKILLCFG.SHOOTAUDIOCLIP));
			skill.HitAudioClip = Resources.Load<AudioClip>(row.getStringValue(CFG_SKILLCFG.HITAUDIOCLIP)); 
			skill.Type = (SkillType)row.getIntValue(CFG_SKILLCFG.TYPE);
			
			//skill.SubSkills = row.getIntValue(CFG_SKILLCFG.SUBSKILL);
			
			if (row.getIntValue(CFG_SKILLCFG.ISACTIVE) == 1)
				skill.IsActive = true; 
			else
				skill.IsActive = false;
			
			skill.FingerType = row.getIntValue(CFG_SKILLCFG.FINGERTYPE);
			skill.OperCount = row.getIntValue(CFG_SKILLCFG.OPERCOUNT);
			skill.IsGlobal = row.getIntValue(CFG_SKILLCFG.ISGLOBAL);
			skill.SkillAnimIndex =  row.getIntValue(CFG_SKILLCFG.SKILLANIMINDEX);

		}


		public Skill getSkillBySkillID(int skillid)
		{
            LoadSkillCfg();
			if (skillcfg.rows.Length == 0)
            {
            
                if (skillcfg.rows.Length == 0)
                return null;
            }
            Skill skill;
			foreach (ConfigRow row in skillcfg.rows) 
			{
				if (row.getIntValue(CFG_SKILLCFG.SKILLID) == skillid)
				{
					getSkill(row,out skill);
					return skill;
				}
			}
			return null;
		}

		/*public Skill getSkillBySkillID(int skillid)
		{
			//Skill skill = null;
			Skill skill = new Skill();
			if (m_SkillConfigList.Count == 0)
				LoadSkillCfg();
			if (m_SkillConfigList.TryGetValue (skillid, out skill) == false)
				return null;

			return skill;
		}*/

		/*public DataMgr.RANGEVALUE getRangeValue(Skill skill)
		{
			DataMgr.RANGEVALUE rangeValue = new DataMgr.RANGEVALUE();

			if ((RANGETYPE)skill.RangeType == RANGETYPE.NONE)
			{
				rangeValue.value1 = 0;
				rangeValue.value2 = 0;
			}
			else
			if ((RANGETYPE)skill.RangeType == RANGETYPE.LINE)
			{
				rangeValue.value1 = skill.RangeValue1;
				rangeValue.value2 = 0;
			}
			else
			if ((RANGETYPE)skill.RangeType == RANGETYPE.ARC)
			{
				rangeValue.value1 = skill.RangeValue1;
				rangeValue.value2 = skill.RangeValue2;
			}
			else
			if ((RANGETYPE)skill.RangeType == RANGETYPE.CIRCLE)
			{
				rangeValue.value1 = skill.RangeValue1;
				rangeValue.value2 = 0;
			}
			return rangeValue;
		}*/

		public void setSkillEvent(UnitSkill unitSkill,Skill skill)
		{
			if (skill.Id == 1) 
			{
				skill.onSkill += unitSkill.RangerMark;
			}
			else
				if (skill.Id == 2) 
			{
				skill.onSkill += unitSkill.RapidSnipe;
			}
			else
				if (skill.Id == 3) 
			{
				skill.onSkill += unitSkill.RangerCharm;
			}
			else
				if (skill.Id == 4) 
			{
				skill.onSkill += unitSkill.Lammasu;
			}
			else
				if (skill.Id == 5) 
			{
				skill.onSkill += unitSkill.LightWingGift;
			}
			else
				if (skill.Id == 6) 
			{
				skill.onSkill += unitSkill.LionHeartImmotral;
				skill.onSkill(skill);
			}
			else
				if (skill.Id == 7) 
			{
				skill.onSkill = unitSkill.DeadlyThrow;
			}
			else
				if (skill.Id == 8) 
			{
				skill.onSkill = unitSkill.StrickCrucial;
			}
			else
				if (skill.Id == 9) 
			{
				skill.onSkill = unitSkill.DesireWarSkill;
				skill.onSkill(skill);
			}
			else
				if (skill.Id == 10) 
			{
				skill.onSkill = unitSkill.UnityIsStrengthSkill;
			}
			else
				if (skill.Id == 11) 
			{
				skill.onSkill = unitSkill.KingSacrificeSkill;
			}
			else
				if (skill.Id == 12) 
			{
				skill.onSkill = unitSkill.HeroMournfulSongSkill;
				skill.onSkill(skill);
			}
			else
				if (skill.Id == 13) 
			{
				skill.onSkill = unitSkill.SurviveSongSkill;
			}
			else
				if (skill.Id == 14) 
			{
				skill.onSkill = unitSkill.PetrifactionSkill;
			}
			else
				if (skill.Id == 15) 
			{
				skill.onSkill = unitSkill.ToxTouchSkill;
			}
		}
	}
}