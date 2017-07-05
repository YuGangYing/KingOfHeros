using System;
using System.Collections.Generic;

namespace DataMgr
{
	public enum CFG_BATTLE
	{
        BATTLE_ID = 0,
        BATTLE_NAME,
        BATTLE_DESC
	}

    public enum CFG_BATTLE_FIELD
    {
        BATTLE_ID = 0,
        BATTLE_NAME,
        BATTLE_DESC,
        BATTLE_FIELD_ID,
        NEXT_FIELD_ID,
        BATTLE_FIELD_NAME,
        BATTLE_FIELD_STAR,
        BATTLE_FIELD_DESC,
        BATTLE_STYLE,
        ENEMY_NAME,
        AWARD_EXP,
        AWARD_GOLD,
        AWARD_STONE,
        AWARD_DIAMOND,
        ITEM1_TYPEID,
        ITEM1_COUNT,
        ITEM2_TYPEID,
        ITEM2_COUNT,
        FIELD_HAS_BOSS,
        FREE_TIMES,
        CHARGE,
    }

    public enum CFG_BATTLE_ENEMY
    {
        BATTLE_ID = 0,
        BATTLE_FIELD_ID,
        HERO_TYPE,
        HERO_STAR,
        HERO_LEVEL,
        LOCATION,
        HERO_SKILL1,
        HERO_SKILL1_LEVEL,
        HERO_SKILL2,
        HERO_SKILL2_LEVEL,
        HERO_SKILL3,
        HERO_SKILL3_LEVEL,
        SOLDIER_NUM,
        SOLDIER_TYPE,
        SOLDIER_LEVEL,
        SOLDIER_ARMY_LEVEL,
    }

}
