using System;
using System.Collections.Generic;

namespace DataMgr
{
    public enum enQualityType : int
    {
        enQT_Invalid = -1,
        enQT_Iron = 0,     // 铁
        enQT_Copper,       // 铜
        enQT_Silver,       // 银
        enQT_Gold,         // 金
        enQT_Amount,
    }

    public enum enCVS_HERO_BASE_ATTRIBUTE : int
    {
        HERO_TYPEID = 0,
        NAME_ID,
        DESC_ID, //DESCRIPTION,

        ICON_FILE,          // UI图集 文件名
        ICON_SPRITE_NAME,   // UI ICON
        FBX_FILE,           // PREFABE_RED,        // 模型 文件名    FBX_FILE,
        //PREFABE_BLUE,       // 模型 文件名    FBX_FILE,

        ARMY_TYPE,          // 士兵类型
        PORTARAIT,          // 头像 ICON
        QUALITY,            // 卡牌星级
        CLASSIFY,           // 卡牌品质

        ATTACK_DISTANCE,
        ATTACK_SPEED,
        ATTACK_ANIMATORTIME,
        ATTACK_INTERVALj,

        DAMAGE_MODE,
        DAMAGE_RANGE,

        MOVE_SPEED,
        BASE_ATTACK,
        BASE_DEF,
        BASE_HP,
        BASE_VIOLENCE,
        BASE_LEADER,

        BASE_SKILL1_TYPEID, // 技能01
        BASE_SKILL2_TYPEID, // 技能02
		BASE_SKILL3_TYPEID, // 技能03

        LIBRARY,        // 图鉴

        // 基友数据
        GAY_ATK,
        GAY_DEF,
        GAY_HP,
        GAY_LEADER,
        //攻击模式
        NATURE
    }


    public enum enCVS_HERO_EXP_ATTRIBUTE : int
    {
        LEVEL = 0,
        EXP,
    }

    public enum enCVS_HERO_STAR_ATTRIBUTE : int
    {
        HERO_TYPEID = 0,
        STAR_LEVEL,
        FACTOR_HP,
        FACTOR_ATTACK,
        FACTOR_DEF,
        FACTOR_VIOLENCE,
        FACTOR_LEADER,
        UNLOCK_SKILL_TYPEID
    }

    //public enum enCVS_HERO_LEVEL_ATTRIBUTE : int
    //{
    //    SKILL_TYPEID = 0,
    //    LEVEL,
    //    DAMAGE,
    //    RATIO,
    //    EFFECT_VALUE,
    //    EXP,

    //    STATUS1_TYPEID,
    //    STATUS1_TIME,
    //    STATUS2_TYPEID,
    //    STATUS2_TIME
    //}

    //public enum enCVS_HERO_SKILL_EFFECT_ATTRIBUTE : int
    //{
    //    id = 0,
    //    herotype,
    //    name,
    //    desc,
    //    icon,
    //    mode,
    //    getsure,
    //    animator,
    //    weapon,
    //    effect,
    //    audio,
    //    cost,
    //    duration,
    //    cdtime,
    //}


    public enum enCVS_HERO_GAY_ATTRIBUTE : int
    {
        HERO_TYPEID	= 0,
        GAY_TYPEID_1,
        GAY_TYPEID_2,
        GAY_TYPEID_3,
        GAY_TYPEID_4,
    }

    // 
    public class CHerroTalbeAttribute
    {
        // 获取图鉴
        public static bool getHeroDetailByIllustratedId(int nId, ref int nTypeId)
        {
            ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CVS_HERO_BASE);
            if (cb.RowList.Count == 0)
                return false;

            for (int i = 0; i < cb.RowList.Count; i++)
            {
                ConfigRow cr = cb.RowList[i];
                int id = cr.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.LIBRARY);
                if (id == nId)
                {
                    nTypeId = cr.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.HERO_TYPEID);
                    return true;
                }
            }

            return false;
        }

        // 获取HERO基础数据
        public static bool getHeroBaseDetail(int nTypdId, out ConfigRow cr)
        {
            cr = null;

            ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CVS_HERO_BASE);
            if (cb.RowList.Count == 0)
                return false;

            for (int i = 0; i < cb.RowList.Count; i++)
            {
                ConfigRow tmp = cb.RowList[i];
                int id = tmp.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.HERO_TYPEID);
                if (id == nTypdId)
                {
                    cr = tmp;
                    return true;
                }
            }

            return false;
        }

        public static bool getHeroIconPrefabName(int nId, ref string strPrefab)
        {
            ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CVS_HERO_BASE);
            if (cb.RowList.Count == 0)
                return false;

            for (int i = 0; i < cb.RowList.Count; i++)
            {
                ConfigRow cr = cb.RowList[i];
                int id = cr.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.HERO_TYPEID);
                if (id == nId)
                {
                    strPrefab = cr.getStringValue(enCVS_HERO_BASE_ATTRIBUTE.ICON_FILE);
                    return true;
                }
            }

            return false;
        }

        public static bool getHeroIconSpriteName(int nId, ref int nIconSpriteName)
        {
            ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CVS_HERO_BASE);
            if (cb.RowList.Count == 0)
                return false;

            for (int i = 0; i < cb.RowList.Count; i++)
            {
                ConfigRow cr = cb.RowList[i];
                int id = cr.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.HERO_TYPEID);
                if (id == nId)
                {
                    nIconSpriteName = cr.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.ICON_SPRITE_NAME);
                    return true;
                }
            }

            return false;
        }

        // 获取基友数据
        public static bool getHeroGayTypeIdByTypeId(int nTypeId, ref List<int> refListTypeId)
        {
            if (refListTypeId == null)
                return false;
            bool bHave = false;

            ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CVS_HERO_BASE);
            if (cb == null)
                return false;
            ConfigRow cr = cb.getRow(enCVS_HERO_BASE_ATTRIBUTE.HERO_TYPEID, nTypeId);
            if (cr == null)
                return false;

            int nId = cr.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.GAY_ATK);
            if (nId != 0)
            {
                bHave = true;
                refListTypeId.Add(nId);
            }

            nId = cr.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.GAY_DEF);
            if (nId != 0)
            {
                bHave = true;
                refListTypeId.Add(nId);
            }

            nId = cr.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.GAY_HP);
            if (nId != 0)
            {
                bHave = true;
                refListTypeId.Add(nId);
            }

            nId = cr.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.GAY_LEADER);
            if (nId != 0)
            {
                bHave = true;
                refListTypeId.Add(nId);
            }

            //ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CVS_HERO_GAY);
            //if (cb == null)
            //    return false;

            //ConfigRow cr = cb.getRow(enCVS_HERO_GAY_ATTRIBUTE.HERO_TYPEID, nTypeId);
            //if (cr == null)
            //    return false;

            //int nId = cr.getIntValue(enCVS_HERO_GAY_ATTRIBUTE.GAY_TYPEID_1);
            //if (nId != 0)
            //{
            //    bHave = true;
            //    refListTypeId.Add(nId);
            //}

            //nId = cr.getIntValue(enCVS_HERO_GAY_ATTRIBUTE.GAY_TYPEID_2);
            //if (nId != 0)
            //{
            //    bHave = true;
            //    refListTypeId.Add(nId);
            //}

            //nId = cr.getIntValue(enCVS_HERO_GAY_ATTRIBUTE.GAY_TYPEID_3);
            //if (nId != 0)
            //{
            //    bHave = true;
            //    refListTypeId.Add(nId);
            //}

            //nId = cr.getIntValue(enCVS_HERO_GAY_ATTRIBUTE.GAY_TYPEID_4);
            //if (nId != 0)
            //{
            //    bHave = true;
            //    refListTypeId.Add(nId);
            //}

            return bHave;
        }

        // 获取星级相关
        public static bool getHeroStar(int nTypeId, int nlv, out ConfigRow cr)
        {
            cr = null;

            ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CVS_HERO_STAR);
            if (cb.RowList.Count == 0)
                return false;

            for (int i = 0; i < cb.RowList.Count; i++)
            {
                ConfigRow tmp = cb.RowList[i];
                int id = tmp.getIntValue(enCVS_HERO_STAR_ATTRIBUTE.HERO_TYPEID);
                int lv = tmp.getIntValue(enCVS_HERO_STAR_ATTRIBUTE.STAR_LEVEL);
                if (id == nTypeId && nlv == lv)
                {
                    cr = tmp;
                    return true;
                }
            }

            return false;
        }

        public static bool getHeroSkill(int nTypeId, out ConfigRow cr)
        {
            cr = null;

            ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_SKILL);
            if (cb.RowList.Count == 0)
                return false;

            for (int i = 0; i < cb.RowList.Count; i++)
            {
                ConfigRow tmp = cb.RowList[i];
                int ntmpTypeId = tmp.getIntValue(CFG_SKILL.ID);
                if (nTypeId == ntmpTypeId)
                {
                    cr = tmp;
                    return true;
                }
            }
            return false;
        }

    }


    public enum enCFG_CSV_CHECKIN_ITEM
    {
        WEEKID = 0,
        MONEY,
        STONE,
        DIAMOND,
        ITEM01_TYPEID,
        ITEM01_AMOUNT,
        ITEM02_TYPEID,
        ITEM02_AMOUNT
    }

    public class CCheckInItemAttribute
    {
        // 星期(1-6) 星期天(0)
        public static bool getItemByWeekDay(int nWeek, ref ConfigRow refCR)
        {
            ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CSV_CHECKIN_ITEM);
            if (cb.RowList.Count == 0)
                return false;

            for (int i = 0; i < cb.RowList.Count; i++)
            {
                ConfigRow cr = cb.RowList[i];
                int id = cr.getIntValue(enCFG_CSV_CHECKIN_ITEM.WEEKID);
                if (id == nWeek)
                {
                    refCR = cr;
                    return true;
                }
            }

            return false;
        }       
    }
}


