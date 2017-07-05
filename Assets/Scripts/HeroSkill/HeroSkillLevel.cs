using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
namespace HeroSkill
{
    class SkillLevelItem
    {
        public int nTypeId;
        public int nLevel;
        public int nEffect_value;
        public int nExp;
        public int nStatus1Id;
        public int nStatus1Time;
        public int nStatus2Id;
        public int nStatus2Time;
    }

	class HeroSkillLevel
	{
        List<SkillLevelItem> m_LevelList = new List<SkillLevelItem>();
        const string m_strConfig = "Assets/Data/Configs/HeroSkill/dict_skill_level";

        public HeroSkillLevel()
        {

        }

        public bool loadConfig()
        {
            //m_LevelList.Clear();

            //TextAsset configText = ResourcesMgr.LoadAsset<TextAsset>(m_strConfig.ToLower());
            //CsvReader reader = new CsvReader();
            //reader.Load(configText.text, ',');

            //int line = reader.getYCount();
            //for (int y = 0; y < line; y++)
            //{
            //    int x = 0;
            //    SkillLevelItem item = new SkillLevelItem();
            //    item.nTypeId = reader.getInt(y, x++, 0);
            //    item.nLevel = reader.getInt(y, x++, 0);
            //    item.nEffect_value = reader.getInt(y, x++, 0);
            //    item.nExp = reader.getInt(y, x++, 0);
            //    item.nStatus1Id = reader.getInt(y, x++, 0);
            //    item.nStatus1Time = reader.getInt(y, x++, 0);
            //    item.nStatus2Id = reader.getInt(y, x++, 0);
            //    item.nStatus2Time = reader.getInt(y, x++, 0);
            //    m_LevelList.Add(item);
            //}
            //return true;
            return false;
        }

        public SkillLevelItem getItem(int nTypeId, int nLevel)
        {
            ConfigBase cb = DataMgr.DataManager.getConfig(CONFIG_MODULE.CFG_SKILL_LEVEL);
            ConfigRow cr = cb.getRow(CFG_SKILL_LEVEL.ID, nTypeId, CFG_SKILL_LEVEL.LEVEL, nLevel);
            if (cr == null)
            {
                return null;
            }

            //if (csl != null)
            {
                SkillLevelItem item = new SkillLevelItem();
                item.nTypeId = nTypeId;
                item.nLevel = nLevel;
                item.nEffect_value = cr.getIntValue(CFG_SKILL_LEVEL.EFFECT_VALUE); // csl.getAttributeIntValue(CTableSkillLevel.enAttributeName.enAN_EffectValue);
                item.nExp = cr.getIntValue(CFG_SKILL_LEVEL.EXP);                   // csl.getAttributeIntValue(CTableSkillLevel.enAttributeName.enAN_Exp);
                item.nStatus1Id = cr.getIntValue(CFG_SKILL_LEVEL.STATUS1_TYPEID);  // csl.getAttributeIntValue(CTableSkillLevel.enAttributeName.enAN_Status1_TypeID);
                item.nStatus1Time = cr.getIntValue(CFG_SKILL_LEVEL.STATUS1_TIME);  // csl.getAttributeIntValue(CTableSkillLevel.enAttributeName.enAN_Status1_Time);
                item.nStatus2Id = cr.getIntValue(CFG_SKILL_LEVEL.STATUS2_TYPEID);  // csl.getAttributeIntValue(CTableSkillLevel.enAttributeName.enAN_Status2_TypeID);
                item.nStatus2Time = cr.getIntValue(CFG_SKILL_LEVEL.STATUS2_TIME);  // csl.getAttributeIntValue(CTableSkillLevel.enAttributeName.enAN_Status2_Time);

                return item;
            }


//             foreach (SkillLevelItem item in m_LevelList)
//             {
//                 if (item.nLevel == nLevel && item.nTypeId == nTypeId)
//                     return item;
//             }
//            return null;
        }
	}
}
