using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataMgr
{
    //技能配置
    public enum CFG_SKILL : int
    {
        //技能ID
        ID=0,
        //所属英雄
        HEROTYPEID,
        //技能名
        NAME,
        //描述
        DESC,
        //图标
        ICON,
        //触发模式(主、被动)
        MODE,
        //触发手势
        GETSURE,
        //执行动作名
        ANIMATOR,
        //武器类型
        WAEPON,
        //特效文件名
        EFFECT,
        //音效文件
        AUDIO,
        //消耗(怒气值)
        COST,
        //执行时间
        DURATION,
        //冷却时间
        CDTIME
    }

    public enum CFG_SKILL_LEVEL : int
    {
        ID =0,
        LEVEL,//等级
        BASEDAMAGE,//基本伤害
        DAMAGERATIO,//伤害系数
        EFFECT_VALUE,
        EXP,
        STATUS1_TYPEID,
        STATUS1_TIME,
        STATUS2_TYPEID,
        STATUS2_TIME
    }


    //技能效果配置
    public enum CFG_SKILL_RESULT : int
    {
        //效果ID
        ID =0,
        //技能ID
        SKILL_ID,
        //效果类型
        RESULTTYPE,
        //子类型
        SUBTYPE,
        //分边
        SIDE,
        //目标类型
        OBJECTYPE,
        //最大攻击数量
        MAXNUMBER,
        //排序规则
        SORTTYPE,
        //攻击范围
        RANGTYPE,
        //攻击范围
        RANGVALUE1,
        //攻击范围
        RANGVALUE2,
        //持续时长
        DURATION,
        //延迟时间
        RELAYTIME,
        //触发条件对象所属
        CONDITIONSIDE,
        //触发条件对象类型
        CONDITIONOBJTYPE,
        //触发条件
        CONDITION,
        //触发条件值
        CONDITIONVALUE,
        //重复类型
        REPEATTYPE,
        //重复执行时间间隔
        REPEATINTERVAL
    }

    //伤害效果等级表
    public enum CFG_SKILL_RESULT_LEVEL : int
    {
        RESULT_ID = 0,//效果ID
        SKILL_LEVEL,//技能等级
        TYPE, //值类型
        VALUE_TYPE, //值类型
        VALUE //数值
    }

    //技能效果特效
    public enum CFG_SKILL_RESULT_EFFECT : int
    {
        ID = 0,     //描述
        NAME,   //状态类型
        DESC,   //描述
        TYPE,   //值类型
        RESOURCE,
        MOVEFLAG,
        ATKFLAG,
        SKILLFLAG,
        DAMAGEFLAG,
        BUFFFLAG,
        ABSORB, //吸收
        RESIST, //抵抗
        CHGPOINT,
    }

    public enum CFG_SOLDIER_INFLUENCE : int
    {
        ID = 0,
        SHIELD = 1,//盾兵
        PIKEMAN = 2,//枪兵
        ARCHER = 3,//弓兵
        CAVALRY = 4,//骑兵
        MAGIC = 5,//法兵
        HERO = 6,  //英雄
    }

	public enum RANGETYPE: int
	{
		NONE = 0,
		LINE = 1,
		ARC = 2,
		CIRCLE = 3,
	}

	public struct RANGEVALUE
	{
		public float value1;
		public float value2;
	}

	public enum CFG_SKILLCONDITION: int
	{
		NONE = 0,
		DEAD = 1,
		BEFOREDEAD = 2,
	}

	//手势类型
	public enum CFG_FINGERTYPE: int
	{
		NONE = 0, //无
		LINE = 1, //直线
		ARC = 2,  //半圆
		CIRCLE = 3, //圆形
		LONGTAP = 4, //长按
		DOUBLETAP = 5,  //双击
	}

	public enum CFG_PLAYEFFECT: int
	{
		NONE = 0, //不播全屏动画，播慢动作
		ANIM = 1, //播全屏动画，不播慢动作
		ALL = 2,  //播全屏动画，播慢动作
	}
	//技能配置表
	public enum CFG_SKILLCFG: int
	{	
		ID,
		SKILLID,
		NAMEID,
		DESCID,
		NAME,
		ICON,
		MAXCOOLDOWN,
		EFFECTDURATION,
		ANIMDURATION,
		COST,
		DAMAGE,
		ADDITIONDAMAGE,
		CONDITION,
		CONDITIONVALUE,
		ATTACKRADIUS,
		//RANGETYPE,
		//RANGEVALUE1,
		//RANGEVALUE2,
		MAXHITCOUNT,
		ISACTIVE,
		SKILLPREFAB,
		SHOOTEFFECTCPREFAB,
		SHOOTSTANDBYEFFECTCPREFAB,
		HITEFFECTCPREFAB,
		CASTAUDIOCLIP,
		SHOOTAUDIOCLIP,
		HITAUDIOCLIP,
		TYPE,
		SUBSKILL,
		FINGERTYPE,
		OPERCOUNT,
		ISGLOBAL,
		SKILLANIMINDEX,
	}


	public enum SkillType{Circle,CircleFly,Whirlwind}
	[System.Serializable]
	public class Skill
	{
		public delegate void OnSkill(Skill skill);
		public OnSkill onSkill;

		public delegate void OnGlobalEffect(Skill skill);
		public OnSkill onGlobalEffect;

		public int Id;
		public bool IsSkilled;
		public int SkillId;
		public int NameId;
		public int DescId;
		public string Name;
		public string Icon; 
		public float MaxCooldown = 10;
		public float CurCooldown = 0;
		public float SkillDuration = 1.333f;
		public float SkillEffectDuration = 3;
		public float SkillAnimDuration = 2;
		public float Cost = 50;
		public float Damage;
		public float AdditionDamage;
		public float Condition;
		public float ConditionValue;
		//	public float SkillScaneRadius = 20;
		public float SkillAttackRadius = 5;
		//public int RangeType = 1;
		//public float RangeValue1 = 0;
		//public float RangeValue2 = 0;
		public int MaxHitCount = 6;  
		public bool isDieTrigger = false;
		//public bool isDesireWarSkill = false;
		public bool IsActive = true;
		public int FingerType = 0;
		public GameObject SkillPrefab;
		public GameObject ShootEffectPrefab;
		public GameObject ShootStandByEffectPrefab;
		public GameObject HitEffectPrefab;
		public GameObject GlobalSkillEffect;
		public AudioClip CastAudioClip;
		public AudioClip ShootAudioClip;
		public AudioClip HitAudioClip;
		public SkillType Type;
		public int OperCount;
		public Skill[] SubSkills;
		public Vector3 CircleCenter = new Vector3(0,0,0);
		public int IsGlobal;
		public int SkillAnimIndex;
	}
}
