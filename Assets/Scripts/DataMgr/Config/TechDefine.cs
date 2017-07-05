using System;
using System.Collections.Generic;

namespace DataMgr
{
	public enum CFG_ARMY
	{
        ARMY_TYPEID,
        ICON_FILE,
        PORTARAIT,
        FBX_FILE,
        ARMY_SERVICE,
        MOVE_SPEED,ATTACK_MODE,
        ATTACK_DISTANCE,ATTACK_SPEED,
        ATTACK_ANIMATORTIME,
        DAMAGE_MODE,
        DAMAGE_RANGE,
        DEFENCE_MODE,
        RESTRAIN_ARMY1_TYPEID,
        RESTRAIN_ARMY1_VALUE,
        RESTRAIN_ARMY2_TYPEID,
        RESTRAIN_ARMY2_VALUE
	}

    public enum CFG_ARMY_LEVEL
    {
        ARMY_TYPEID,
        ARMY_LEVEL,
        NAME_ID,
        PROPERTY_ID,
        DESC_ID,
        LEVEL,HP,
        ATTACK,
        DEFENCE,
        COST_COIN
    }

    public enum CFG_TECHNOLOGY
    {
        TECHNOLOGY_TYPEID,
        LEVEL,
        SERVICE,
        SERVICE_ID,
        DESC_ID,
        BUILDING_LEVEL,
        DEP_TECHNOLOGY_TYPEID,
        DEP_TECHNOLOGY_LEVEL,
        COIN,
        STONE,
        COST_TIME,
        EFFECT_ARMY_TYPEID,
        ATTACK_BOUNS,
        DEFENCE_BOUNS,
        HP_BOUNS,
        RESTRAIN_ARMY1_BOUNS,
        RESTRAIN_ARMY2_BOUNS
    }
}
