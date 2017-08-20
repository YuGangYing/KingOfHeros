using System;
using System.Collections.Generic;

namespace DataMgr
{
    public enum CFG_AUDIO : int
    {
        AUDIO_ID,
        NAME,
        DESCRIPTION,
        SEQUENCE,
        AUTOLOOP,
        INTERVAL,
        FADETIME,
        TYPE,
        SUB_TYPE,
        DIMENSIONS
    }

    public enum CFG_AUDIO_FILE : int
    {
        AUDIO_ID,
        INDEX,
        FILE,
        DURATION,
        DESC
    }

    public enum AUDIO_TYPE:int
    {
        UNKNOWN = 0,
        BGM, //背景音乐
        BGS,//环境声效
        UI,//UI操作
        STATE,//状态表现
        FIGHT,//战斗声效
        SKILL//技能施放
    }

    public enum AUDIO_FIGHT_TYPE:int
    {
        UNKNOWN = 0,
        STEP,//脚步
        SPECIAL,//特殊
        HIT,//打击
    }

    public enum AUDIO_SKILL_TYPE:int
    {
        UNKNOWN = 0,
        EXEC, //技能施放
        HIT,//技能打击
    }

    public enum AUDIO_SEQUENCE
    {
        UNKNOWN = 0,
        RANDOM,
        SERIAL,//串行
    }

    public enum AUDIO_DIMENSIONS
    {
        UNKNOWN = 0,
        DIMEN_2D,
        DIMEN_3D,
    }
}
