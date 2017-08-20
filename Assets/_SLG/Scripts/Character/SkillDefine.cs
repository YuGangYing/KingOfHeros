using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Fight
{
    //技能执行模式
    public enum SKILL_EXECMODE
    {
        ACTIVE = 1,//主动
        PASSIVE,
        UNKNOW
    }

    //武器类型
    public enum SKILL_WEAPON
    {
        ARROW = 1,//弓箭
        PIKE,//长枪
        POWDER//炸药桶
    }

    //攻击范围类型
    public enum SKILL_RANG
    {
        UNKNOWN =0,
        ALL = 1,//全场
        LINE,
        FAN,
        CIRLE_ME, //
        CIRCLE_OTHER
    }

    public enum SKILL_SIDE
    {
        UNKNOWN = 0,
        SELF,//本方
        ENEMY,//敌方
        ALL  //所有
    }

    public enum SKILL_OBJTYPE
    {
        UNKNOWN = 0,
        SELF,       //英雄自己
        HERO,       //英雄（不包括自己？）
        HERO_MATRIX, //方阵英雄
        HERO_ROW,   //行英雄
        SOLDIER,    //士兵
        SOLDIER_MATRIX,//方阵中的士兵
        SOLDIER_ROW, //行中所有的士兵
        ALL, //所有角色
        ALL_MATRIX, 
        ALL_ROW
    }

    //属性操作
    public enum SKILL_ATTR_OPER
    {
        ADD, //值增加
        SUBTRACT,//值减少
        UNKNOWN
    }
    
    //技能状态
    public enum SKILL_STATUS
    {
        UNKNOWN = 0,
        //空闲
        IDLE = 1,
        //执行中
        EXECING,
        //冷却中
        COLDING,
        //完成状态
        FINISH
        //
    }

    //技能手势
    public enum SKILL_GESTURE
    {
        UNKNOWN = 0,
        LINE = 1,
        CIRCLE,
        FAN
    }

    //值类型
    public enum SKILL_RESULT_VALUE_TYPE
    {
        UNKNOWN = 0,
        PERCENT = 1,//百分比
        ABS //绝对值
    }

    //技能效果等级表值影响效果类型
    public enum SKILL_RESULT_LEVEL_TYPE : int
    {
        UNKNOWN = 0,
        DAMAGE = 1, //伤害
        RESTOREHP,  //恢复生命
        TIME,       //时间
    }

    //效果类型
    public enum SKILL_RESULT_TYPE
    {
        UNKNOWN = 0,
        DAMAGE,//直接伤害型有防御
        DAMAGENODEF,//直接伤害型无视防御
        BUFF,  //属性加成
        DEBUFF,//属性降低
        REVIVE,//复活
        CURE,//治疗
        SETEFFECT,//进入特效状态
        MOVE,//移动
        CHGSKILL,//修改技能
        NORMALATK,
        DAMAGEBOUNCE,//反弹伤害
    }

    public enum SKILL_REUSLT_REPEAT_TYPE
    {
        UNKNOWN = 0,
        INSKILL,//
        INRESULT
    }

    //影响效果对象类型
    public enum SKILL_RESULT_OBJECSORT
    {
        HP, //生命值
        UNKNOWN
    }

    //升序降序方式
    public enum SKILL_RESULT_SORTMODE
    {
        ASCEND = 1,//升序
        DESCEND,   //降序
    }

    public enum SKILL_RESULT_ATRRTYPE
    {
        UNKNOWN =0,
        MAXHP =1,       //最大生命值
        CURLOSTHP,       //当前生命值 
        MOVESPEED,   //移动速度
        LEADPOWER,   //领导力
        ATTACKRANG,  //攻击范围
        ATTACKSPEED, //攻击速度
        ATTACKDAMAGE,//攻击伤害值
    }

    //效果触发条件
    public enum SKILL_RESULT_CONDITION
    {
        NONE =0, //无条件直接执行
        HPLEVEL ,//HP值
        DEATH,//死亡
        EFFECT,//效果
        RATIO//按概率触发
    }
}
