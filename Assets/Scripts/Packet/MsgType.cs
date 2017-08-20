using System;
namespace Packet
{
    public enum HERO_ACQUIRE
    {
        HERO_ACQUIRE_PVE = 1,       // PVE战斗获得
        HERO_ACQUIRE_STONE = 2,     // 魔石抽取
        HERO_ACQUIRE_MONEY = 3,     // 货币抽取
        HERO_ACQUIRE_FREE = 4,      // 免费抽取
    };

    public enum HERO_ATTR
	{
        HERO_ATTR_ID,           // 英雄ID
        HERO_ATTR_TYPE_ID,      // 英雄类型id
        HERO_ATTR_STAR,         // 卡牌星级(最高5星)
        HERO_ATTR_LEVEL,        // 英雄等级
        HERO_ATTR_EXP,          // 英雄经验
    
        // 英雄技能
        HERO_ATTR_SKILL_BEG,
        HERO_ATTR_SKILL1 = HERO_ATTR_SKILL_BEG,       // 技能类型id-1
        HERO_ATTR_SKILL2,       // 技能类型id-2
        HERO_ATTR_SKILL3,       // 技能类型id-3
        HERO_ATTR_SKILL_END,
        HERO_ATTR_SKILL_LVL1,         // 技能等级-1
        HERO_ATTR_SKILL_LVL2,         // 技能等级-2
        HERO_ATTR_SKILL_LVL3,         // 技能等级-3
        HERO_ATTR_SKILL_EXP1,         // 技能经验-1
        HERO_ATTR_SKILL_EXP2,         // 技能经验-2
        HERO_ATTR_SKILL_EXP3,         // 技能经验-3

        // 英雄战斗相关属性
        HERO_ATTR_BATTLE_BEG,
        HERO_ATTR_BATTLE_ATK = HERO_ATTR_BATTLE_BEG,    // 攻
        HERO_ATTR_BATTLE_DEF,   // 防
        HERO_ATTR_BATTLE_HP,    // 血
        HERO_ATTR_BATTLE_VIOLENCE,  // 暴击
        HERO_ATTR_BATTLE_LEADER,    // 领导力
        HERO_ATTR_BATTLE_POS,   // 战斗位置
        HERO_ATTR_BATTLE_END,

        HERO_ATTR_LORD,             // 服侍的主卡英雄
        HERO_ATTR_WAITER_ATK,          // 攻击侍从
        HERO_ATTR_WAITER_DEFENCE,      // 防御侍从
        HERO_ATTR_WAITER_HP,           // 血量侍从
        HERO_ATTR_WAITER_LEADER,       // 领导力侍从

        HERO_ATTR_TRAIN_ATTACK_LEV,             // 培养攻击等级
        HERO_ATTR_TRAIN_DEF_LEV,                // 培养防御等级
        HERO_ATTR_TRAIN_HP_LEV,                 // 培养血量等级
        HERO_ATTR_TRAIN_VIOLENCE_LEV,           // 培养暴击等级
        HERO_ATTR_TRAIN_LEADER_LEV,             // 培养领导力等级
        HERO_ATTR_STATUS,                       // 英雄状态 0-休战，1-出战

        //  服务端属性自用索引
        HERO_ATTR_SERVER_BEGIN = 5000,
    };

    enum USER_ATTR
    {
        // 服务端和客户端属性同步索引
        USER_ATTR_VIP,
        USER_ATTR_COIN,
        USER_ATTR_STONE,
        USER_ATTR_RMB,
        USER_ATTR_RESERVE_SOLDIER,					//	后备兵力数量
        USER_ATTR_RECRUIT_RESERVE_SOLDIER,			//	正在招募中的后备兵力数量
        USER_ATTR_RECRUIT_FINISH_TIME,				//	招募到期时间(时间戳)
        USER_ATTR_DRAW_HERO_CNT,					//  免费抽取英雄次数
        USER_ATTR_DRAW_HERO_TIME,					//  上次免费抽取英雄时间
        USER_ATTR_DRAW_HERO_CNT_STONE,              //  使用魔石抽取英雄次数
        USER_ATTR_DRAW_HERO_TIME_STONE,             //  上次使用魔石抽取英雄时间
        USER_ATTR_ARMY_INFANTRY,
        USER_ATTR_ARMY_SHIELD,
        USER_ATTR_ARMY_CAVALRY,
        USER_ATTR_ARMY_ARCHER,
        USER_ATTR_ARMY_SPELL,
        USER_ATTR_EXP,
        USER_ATTR_IDBATTLE1         = 100,
        USER_ATTR_IDFIELD1          = 101,
        USER_ATTR_IDBATTLE2         = 102,
        USER_ATTR_IDFIELD2          = 103,
        USER_ATTR_IDBATTLE3         = 104,
        USER_ATTR_IDFIELD3          = 105,
    }
}
