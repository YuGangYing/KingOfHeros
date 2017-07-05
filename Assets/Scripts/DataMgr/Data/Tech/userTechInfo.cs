using System;
using System.Collections.Generic;
using Packet;

namespace DataMgr
{
    public enum TECH_TYPE
    {
        TECH_NONE = 0,
        TECH_ARMY = 1,
        TECH_STRENG = 2
    };

    public class TechItem
    {
        public ARMY_TYPE armyType;
        public TECH_TYPE operType;
        public uint nId;
        public int nTechId;
        public int nArmyLevel;
        public int nStarLevel;
        public int nLevel;
        public int nState;

        public TechItem Clone()
        {
            TechItem newObj = new TechItem();
            newObj.armyType = this.armyType;
            newObj.operType = this.operType;
            newObj.nId = this.nId;
            newObj.nTechId = this.nTechId;
            newObj.nArmyLevel = this.nArmyLevel;
            newObj.nStarLevel = this.nStarLevel;
            newObj.nLevel = this.nLevel;
            newObj.nState = this.nState;

            return newObj;
        }
    };

	class userTechInfo
	{
        static public TechItem getTechItem(TECHNOLOGY_INFO item)
        {
            ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_TECHONOLOGY);
            if (config == null)
                return null;

            ConfigRow row = config.getRow(CFG_TECHNOLOGY.TECHNOLOGY_TYPEID,(int) item.idTechnologyType,
                                          CFG_TECHNOLOGY.LEVEL,(int) item.cbLev);
            if (row == null)
                return null;
            TechItem newItem = new TechItem();
            newItem.nTechId = (int)item.idTechnologyType;
            newItem.nLevel = (int)item.cbLev;
            newItem.nArmyLevel = getArmyLevel(newItem.nLevel);
            newItem.nStarLevel = getArmyStar(newItem.nLevel);
            newItem.nId = item.idTechnology;
			newItem.armyType = getArmyType(row.getIntValue(CFG_TECHNOLOGY.EFFECT_ARMY_TYPEID));
            newItem.operType = row.getEnumValue<TECH_TYPE>(CFG_TECHNOLOGY.SERVICE,TECH_TYPE.TECH_NONE);
            newItem.nState = (int)item.cbState;
            return newItem;
        }

		static public ARMY_TYPE getArmyType(int armyTypeId)
		{
			ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_ARMY);
			if(config==null)
				return ARMY_TYPE.UNKOWN;
			ConfigRow row = config.getRow(CFG_ARMY.ARMY_TYPEID,armyTypeId);
			if(row==null)
				return ARMY_TYPE.UNKOWN;
			return row.getEnumValue<ARMY_TYPE>(CFG_ARMY.ARMY_SERVICE,ARMY_TYPE.UNKOWN);
		}

		static public int getArmyTypeId(ARMY_TYPE type)
		{
			ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_ARMY);
			if(config==null)
				return 0;
			ConfigRow row = config.getRow(CFG_ARMY.ARMY_SERVICE,(int)type);
			if(row==null)
				return 0;
			return row.getIntValue(CFG_ARMY.ARMY_TYPEID,0);
		}

        static public TechItem getUserArmyInfo(ARMY_TYPE type)
        {
            foreach (KeyValuePair<int,TechItem> item in DataManager.getTechData().techList)
            {
                if (item.Value != null)
                {
                    if (item.Value.armyType == type && item.Value.operType == TECH_TYPE.TECH_ARMY)
                        return item.Value;
                }
            }
            return null;
        }

		static public TechItem getUserStrengInfo(int nTypeId)
		{
            foreach (KeyValuePair<int, TechItem> item in DataManager.getTechData().techList)
            {
                if (item.Value != null)
				{
                    if (item.Value.nTechId == nTypeId && item.Value.operType == TECH_TYPE.TECH_STRENG)
                        return item.Value;
				}
			}
			return null;
		}

        static public int getArmyStar(int level)
        {
            if (level <= 5)
                return level;
            else if (level <= 10)
                return level - 5;
            else if (level <= 15)
                return level - 10;
            return 0;
        }

        static public int getArmyLevel(int level)
        {
            if (level <= 5)
                return 1;
            else if (level <= 10)
                return 2;
            else if (level <= 15)
                return 3;
            return 0;
        }
		
		public static int[] getStrengTechTypeListCfg(ARMY_TYPE armyType)
		{
			ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_TECHONOLOGY);
			if (config == null)
				return null;
			ConfigRow [] rows = config.getRows(CFG_TECHNOLOGY.EFFECT_ARMY_TYPEID,getArmyTypeId(armyType),
			                      CFG_TECHNOLOGY.SERVICE,2,
                                  CFG_TECHNOLOGY.LEVEL,1);
			if(rows==null)
				return null;
			List<int> _list = new List<int>();
			foreach(ConfigRow row in rows)
				_list.Add(row.getIntValue(CFG_TECHNOLOGY.TECHNOLOGY_TYPEID));
			return _list.ToArray();
		}

        public static ConfigRow getStrengTechCfg(int nTypeId,int nLevel)
        {
            ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_TECHONOLOGY);
            if (config == null)
                return null;
            return config.getRow(CFG_TECHNOLOGY.TECHNOLOGY_TYPEID, nTypeId,
			                     CFG_TECHNOLOGY.LEVEL,nLevel);
        }

		public static ConfigRow getArmyTechLevelCfg(ARMY_TYPE armyType,int level)
		{
			ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_TECHONOLOGY);
			if (config == null)
				return null;
			return config.getRow(CFG_TECHNOLOGY.EFFECT_ARMY_TYPEID,getArmyTypeId(armyType),
			                     CFG_TECHNOLOGY.SERVICE,1,
			                     CFG_TECHNOLOGY.LEVEL,level);
		}

		public static ConfigRow getArmyCfg(ARMY_TYPE armyType)
		{
			ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_ARMY);
			if(config!=null)
				return config.getRow(CFG_ARMY.ARMY_SERVICE,(int)armyType);
			return null;
		}

        public static ConfigRow getArmyLevelCfg(ARMY_TYPE armyType, int nArmyLevel,int nStarLevel)
        {
			ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_ARMY_LEVEL);
			if (config == null)
				return null;
			return  config.getRow(CFG_ARMY_LEVEL.ARMY_TYPEID,getArmyTypeId(armyType),
			                      CFG_ARMY_LEVEL.ARMY_LEVEL, nArmyLevel,
			                      CFG_ARMY_LEVEL.LEVEL,nStarLevel);            
        }

		public static ConfigRow getArmyLevelCfg(ARMY_TYPE armyType,int nLevel)
		{
			int nArmyLevel = getArmyLevel(nLevel);
			int nStar = getArmyStar(nLevel);
            return getArmyLevelCfg(armyType,nArmyLevel,nStar);
		}
	}
}
