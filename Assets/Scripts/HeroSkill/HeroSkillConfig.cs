using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
namespace HeroSkill
{
    class HeroSkillItem
    {
        public int nSkillId;
        public string strName;
        public string strDesc;
        public int nShootMode;
        public int nClassify;
        public int nTarget;
        public int nTriggerMode;
        public int nColdTime;
        public int nDuration;
        public int nAttackDistance;
        public int nDamageMode;
        public int nDamageRange;
    }

    class HeroSkillConfig
    {
        Dictionary<int,HeroSkillItem> m_skillList = new Dictionary<int,HeroSkillItem>();
        const string m_strConfig = "Assets/Data/Configs/HeroSkill/dict_skill";

        public HeroSkillConfig()
        {
        }

        public bool loadConfig()
        {
            //TextAsset configText = ResourcesMgr.LoadAsset<TextAsset>(m_strConfig.ToLower());
            //CsvReader reader = new CsvReader();
            //reader.Load(configText.text, ',');

            //int line = reader.getYCount();
            //for (int y = 0; y < line; y++)
            //{
            //    int x = 0;
            //    HeroSkillItem item = new HeroSkillItem();
            //    item.nSkillId = reader.getInt(y, x++, 0);
            //    item.strName = reader.getStr(y, x++);
            //    item.strDesc = reader.getStr(y, x++);
            //    item.nShootMode = reader.getInt(y, x++, 0);
            //    item.nClassify = reader.getInt(y, x++, 0);
            //    item.nTarget = reader.getInt(y, x++, 0);
            //    item.nTriggerMode = reader.getInt(y, x++, 0);
            //    item.nColdTime = reader.getInt(y, x++, 0);
            //    item.nDuration = reader.getInt(y, x++, 0);
            //    item.nAttackDistance = reader.getInt(y, x++, 0);
            //    item.nDamageMode = reader.getInt(y, x++, 0);
            //    item.nDamageRange = reader.getInt(y, x++, 0);
            //    m_skillList.Add(item.nSkillId, item);
            //}
            //return true;
            return false;
        }

        public HeroSkillItem getItem(int nId)
        {
            HeroSkillItem outItem;
            if (!m_skillList.TryGetValue(nId, out outItem))
                return null;
            else
                return outItem;
        }
    }
}
