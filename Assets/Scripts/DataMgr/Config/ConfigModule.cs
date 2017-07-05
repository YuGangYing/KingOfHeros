using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace DataMgr
{
    public enum CONFIG_MODULE
    {
        [Description("dict_army")]
        CFG_ARMY,
        [Description("dict_army_level")]
        CFG_ARMY_LEVEL,
        [Description("dict_technology")]
        CFG_TECHONOLOGY,
        [Description("dict_skill")]
        CFG_SKILL,
        [Description("dict_skill_level")]
        CFG_SKILL_LEVEL,
        [Description("dict_skill_result")]
        CFG_SKILL_RESULT,
        [Description("dict_skill_result_level")]
        CFG_SKILL_RESULT_LEVEL,
        [Description("dict_skill_result_effect")]
        CFG_SKILL_RESULT_EFFECT,
        [Description("dict_soldier_influence")]
        CFG_SOLDIER_INFLUENCE,
        [Description("dict_achievenment")]
        CFG_ACHIEVENMENT,	
        // 英雄相关静态表
        [Description("dict_hero")]
        CFG_CVS_HERO_BASE,
        [Description("dict_hero_star")]
        CFG_CVS_HERO_STAR,
        [Description("dict_hero_level")]
        CFG_CVS_HERO_EXP,
        [Description("dict_hero_gay")]
        CFG_CVS_HERO_GAY,

        // 副本相关静态表
//         [Description("dict_battle")]
//         CFG_BATTLE_BASE,
        [Description("dict_battle")]
        CFG_BATTLE_FIELD,
        [Description("dict_battle_enemy")]
        CFG_BATTLE_ENEMY,

        // 建筑相关静态表
        [Description("Build/dict_building")]
        CFG_BUILDING_BASE,
        [Description("Build/dict_building_level")]
        CFG_BUILDING_LEVEL,

        //音效
        [Description("dict_audio")]
        CFG_AUDIO,
        //音效文件
        [Description("dict_audio_file")]
        CFG_AUDIO_FILE,

        // 每日签到
        [Description("dict_CheckInItem")]
        CFG_CSV_CHECKIN_ITEM,
		//技能配置文件
		[Description("dict_skillcfg")]
		CFG_SKILL_CFG,
    }
}
