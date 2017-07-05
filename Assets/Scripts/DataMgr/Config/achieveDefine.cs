using System;
using System.Collections.Generic; 

namespace DataMgr
{
    //成就类型字典表
    public enum CFG_ACHIEVENMENT : int
    {
        ACHIEVENMENT_TYPEID,//成就类型ID
        CLASSIFY,//成就分类
        MAX_STEP,//最大阶段
        NAME_ID, //NAME,//成就名称

        DESC_ID, //DESCRIPTION,//成就描述
        STEP,    //当前成就阶段
        NEED_ACCUMULATE_VALUE,//当前成绩累积值
        REWARD_MONEY,     //奖励金币
        REWARD_MAGICSTONE,//奖励魔石
        REWARD_RMB,//奖励RMB
        REWARD_ITEM_TYPEID,//奖励物品类型ID
        REWARD_ITEM_AMOUNT,//奖励物品数量
    }
}
