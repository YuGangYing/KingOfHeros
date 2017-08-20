using System;
using System.Collections.Generic;
using Packet;
using DataMgr;

namespace Fight
{
	class FightData
	{ 
        //己方获取角色队列中的英雄战斗相关数据
        public static Hero getSelfHero(int nId)
        {
            //从本地数据中心获取用户数据
            //HERO_INFO heroInfo = new HERO_INFO();
            //if(!DataManager.getHeroData().getItemById((uint)nId, ref heroInfo))
            //    return null;

            //Hero hero = getHeroCfg((int)heroInfo.idType, (int)heroInfo.u8Star, (int)heroInfo.usLevel, SIDE.LEFT);
            Hero hero = getHeroCfg(nId,0,0, SIDE.LEFT);
            if (hero == null)
                return null;
            hero.servId = nId;
        
            return hero;
        }

        public static Hero getEnemyHero(EnemyHero heroInfo)
        {
            Hero hero = getHeroCfg(heroInfo.nHeroType, heroInfo.nHeroStar,heroInfo.nHeroLevel, SIDE.RIGHT);
            if (hero == null)
                return null;
            return hero;
        }

        /*
        从配置文件读取英雄信息
        */
        static Hero getHeroCfg(int nType, int nStar, int nLevel, SIDE side)
        {
            DataMgr.ConfigRow cr = null;
            DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)nType, out cr);
            if (cr == null)
                return null;

            //英雄级别配置属性
            Hero hero = new Hero();
            hero.atkBaseAttackRange = cr.getFloatValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.ATTACK_DISTANCE);
            hero.atkBaseSpeed = cr.getFloatValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.ATTACK_SPEED);
            hero.baseSpeed = cr.getFloatValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.MOVE_SPEED);
            hero.baseDamage = cr.getFloatValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_ATTACK);
            hero.baseDef = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_DEF);
            hero.baseHp = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_HP);
            hero.baseLead = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_LEADER);
            hero.Critical = cr.getFloatValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_VIOLENCE);  
            hero.strResName = cr.getStringValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.FBX_FILE/*PREFABE_RED*/); 
            hero.baseSpeed = cr.getFloatValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.MOVE_SPEED);
            hero.AttackRelayTime = cr.getFloatValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.ATTACK_ANIMATORTIME);
            hero.Nature = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.NATURE);
            //带士兵类型
            hero.soldierType = cr.getEnumValue<ARMY_TYPE>(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.ARMY_TYPE, ARMY_TYPE.UNKOWN);

            //英雄ＨＰ，攻防和领导力与英雄星级　级别有关
            DataMgr.ConfigRow crstar = null;
            DataMgr.CHerroTalbeAttribute.getHeroStar(nType, nStar, out crstar);
            if (crstar == null)
                return null;

            int nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_ATTACK);
            hero.baseDamage = UI.CardQualityUpdatePanel.GetValue((float)hero.baseDamage, nStar, nLevel, nFactor);

            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_HP); 
            hero.baseHp = UI.CardQualityUpdatePanel.GetValue((float)hero.baseHp, nStar, nLevel, nFactor);

            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_DEF); 
            hero.baseDef = UI.CardQualityUpdatePanel.GetValue((float)hero.baseDef, nStar, nLevel, nFactor);

            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_LEADER); 
            hero.baseLead = UI.CardQualityUpdatePanel.GetValue((float)hero.baseLead, nStar, nLevel, nFactor);

            hero.typeid = nType;
            hero.star = nStar;
            hero.level = nLevel;
            return hero;
        }

        public static ConfigRow getHeroSkill(int skillid)
        {
            ConfigBase skillcfg = DataManager.getConfig(CONFIG_MODULE.CFG_SKILL);
            if (skillcfg == null)
                return null;
            return skillcfg.getRow(CFG_SKILL.ID, skillid);
        }

        public static ConfigRow getHeroSkillLevel(int skillid, int level)
        {
            ConfigBase skillcfg = DataManager.getConfig(CONFIG_MODULE.CFG_SKILL_LEVEL);
            if (skillcfg == null)
                return null;
            return skillcfg.getRow(CFG_SKILL_LEVEL.ID, skillid,CFG_SKILL_LEVEL.LEVEL,level);
        }

        public static ConfigRow[] getHeroSkillResults(int skillid)
        {
            ConfigBase resultcfg = DataManager.getConfig(CONFIG_MODULE.CFG_SKILL_RESULT);
            if (resultcfg == null)
                return null;
            //技能所带效果列表
            return resultcfg.getRows(CFG_SKILL_RESULT.SKILL_ID, skillid);
        }

        //根据英雄类型获得英雄技能列表
        public static ConfigRow[] getHeroSKill(int heroTypeid)
        {
            ConfigBase skillcfg = DataManager.getConfig(CONFIG_MODULE.CFG_SKILL);
            if (skillcfg == null)
                return null;

            return skillcfg.getRows(CFG_SKILL.HEROTYPEID, heroTypeid);
        }

        public static ConfigRow getHeroSkillResult(int resultid)
        {
            ConfigBase resultcfg = DataManager.getConfig(CONFIG_MODULE.CFG_SKILL_RESULT);
            if (resultcfg == null)
                return null;
            //技能所带效果列表
            return resultcfg.getRow(CFG_SKILL_RESULT.ID, resultid);
        }

        public static ConfigRow getHeroSkillResultEffect(int id)
        {
            //获得配置信息
            ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_SKILL_RESULT_EFFECT);
            if (config == null)
                return null;
            return config.getRow(CFG_SKILL_RESULT_EFFECT.ID, id);
        }

        public static ConfigRow getHeroSkillResultLevel(int id,int level)
        {
            //获得配置信息
            ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_SKILL_RESULT_LEVEL);
            if (config == null)
                return  null;
            ConfigRow row = config.getRow(CFG_SKILL_RESULT_LEVEL.RESULT_ID, id, CFG_SKILL_RESULT_LEVEL.SKILL_LEVEL, level);
            if (row == null)
                return row;
            //找不到对应级别时，查找级配置
            return config.getRow(CFG_SKILL_RESULT_LEVEL.RESULT_ID, id, CFG_SKILL_RESULT_LEVEL.SKILL_LEVEL, 0);
        }

        //兵种克制关系
        public static int getSoldierInfluence(ARMY_TYPE attacker, ARMY_TYPE atkObject)
        {
            ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_SOLDIER_INFLUENCE);
            if(config==null) //默认为不克
                return 100;
            ConfigRow row = config.getRow(CFG_SOLDIER_INFLUENCE.ID, Convert.ToInt32(attacker));
            if (row == null)
                return 100;

            int value = 100;
            if (!row.getIntValue(atkObject, out value))
                return 100;
            return value;
        }

        //获得本方士兵战斗数据
        public static Soldier[] getSelfSoldier(ARMY_TYPE type, int nNum)
        {
            TechItem techInfo = userTechInfo.getUserArmyInfo(type);
            if (techInfo == null)
                return null;
            ConfigRow armyLevelInfo = userTechInfo.getArmyLevelCfg(type, techInfo.nArmyLevel, techInfo.nStarLevel);
            if (armyLevelInfo == null)
                return null;
            ConfigRow armyInfo = userTechInfo.getArmyCfg(type);
            if (armyInfo == null)
                return null;
            List<Soldier> soldierList = new List<Soldier>();
            //获取士兵参数
            for (int n = 0; n < nNum; n++)
                soldierList.Add(getSoldoer(armyInfo, armyLevelInfo, SIDE.LEFT));
            return soldierList.ToArray();
        }

        //获得敌军士兵队列
        public static Soldier[] getEnemySoldier(ARMY_TYPE type,int armyLevel,int nLevel,int nNum)
        {
            ConfigRow armyLevelInfo = userTechInfo.getArmyLevelCfg(type,armyLevel,nLevel);
            if (armyLevelInfo == null)
                return null;
            ConfigRow armyInfo = userTechInfo.getArmyCfg(type);
            if (armyInfo == null)
                return null;
            List<Soldier> soldierList = new List<Soldier>();
            //获取士兵参数
            for (int n = 0; n < nNum; n++)
                soldierList.Add(getSoldoer(armyInfo, armyLevelInfo, SIDE.RIGHT));
            return soldierList.ToArray();
        }

        static Soldier getSoldoer(ConfigRow armyInfo, ConfigRow armyLevelInfo, SIDE side)
        {
            if(armyInfo==null || armyLevelInfo == null)
                return null;
            Soldier soldier = new Soldier();
            soldier.side = side;
            //和等级无关的属性
            soldier.level = armyLevelInfo.getIntValue(CFG_ARMY_LEVEL.LEVEL);
            soldier.name = DataManager.getLanguageMgr().getString(armyLevelInfo.getIntValue(CFG_ARMY_LEVEL.NAME_ID)); 
            soldier.baseSpeed = armyInfo.getFloatValue(CFG_ARMY.MOVE_SPEED,0f);
            soldier.atkBaseAttackRange = armyInfo.getFloatValue(CFG_ARMY.ATTACK_DISTANCE);
            soldier.atkBaseSpeed = armyInfo.getFloatValue(CFG_ARMY.ATTACK_SPEED);
            soldier.AttackRelayTime = armyInfo.getFloatValue(CFG_ARMY.ATTACK_ANIMATORTIME);
            soldier.armyType = (ARMY_TYPE)armyInfo.getIndex(CFG_ARMY.ARMY_SERVICE);
            //和等级有关的属性 
            soldier.baseHp = armyLevelInfo.getIntValue(CFG_ARMY_LEVEL.HP);
            soldier.baseDef = armyLevelInfo.getIntValue(CFG_ARMY_LEVEL.DEFENCE);
            soldier.baseDamage = armyLevelInfo.getIntValue(CFG_ARMY_LEVEL.ATTACK); 
            soldier.strResName = DataMgr.ResourceCenter.soldierPrebPath + armyInfo.getStringValue(CFG_ARMY.FBX_FILE);
            
            //克制兵种
            soldier.restrainiArmy1 = armyInfo.getEnumValue<ARMY_TYPE>(CFG_ARMY.RESTRAIN_ARMY1_TYPEID, ARMY_TYPE.UNKOWN);
            soldier.restrainiArmy2 = armyInfo.getEnumValue<ARMY_TYPE>(CFG_ARMY.RESTRAIN_ARMY2_TYPEID, ARMY_TYPE.UNKOWN);

            return soldier;
        }

        //获得概率结果
        static public bool getProbability(int nValue)
        {
            int nRandom = UnityEngine.Random.Range(1, 99);
            if (nValue < nRandom)
                return true;
            else
                return false;
        }

        static public bool getProbability(float fValue)
        {
            float fRandom = UnityEngine.Random.Range(0f, 1f);
            if (fValue < fRandom)
                return true;
            else
                return false;
        }
	}
}
