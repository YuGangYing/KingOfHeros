using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SkillShow
{
	//Line End
    class stHeroSkillCfg
    {
        public string strHero;
        public string strSkill;
        public stHeroSkillCfg(string strH,string strS)
        {
            strHero = strH;
            strSkill = strS;
        }
    }

	class ConfigMgr
	{
        static public int MAX_RANG = 10;
        static public int RANG_ROUND = 1;
        static public int RANG_RECT = 2;
        static public int RANG_FAN = 3;
        static public int SIMPLE_TAP = 4;
        static public int DOUBLE_TAP = 5;
        static public int LONG_TAP = 6;

        public Dictionary<stHeroSkillCfg, SkillShow.HeroSkill> mHeroSkillList = new Dictionary<stHeroSkillCfg, SkillShow.HeroSkill>();
        public string m_strPath;
        string m_strTitle;

        public ConfigMgr()
        {
#if (UNITY_ANDROID || UNITY_IPHONE) || FORCE_RESOURCESMGR
            m_strPath = "Assets/Data/Configs/dict_hero_skill";
#else
            m_strPath = Application.dataPath + "/Data/Configs/dict_hero_skill";
#endif
        }

        public bool init(TextAsset skillFile)
        {
            if (skillFile != null)
            {
                CsvReader SkillReader = new CsvReader();
                SkillReader.Load(skillFile.text, ',');

                m_strTitle = SkillReader.mTitle;
                int line = SkillReader.getYCount();
                for (int y = 1; y < line; y++)
                {
                    int x = 0;
                    SkillShow.HeroSkill skill = new SkillShow.HeroSkill();
                    skill.strHero = SkillReader.getStr(y, x++);
                    skill.strSkill = SkillReader.getStr(y, x++);
                    skill.strBoolParam = SkillReader.getStr(y, x++);
                    skill.fColdTime = SkillReader.getFloat(y, x++, 0f);
                    skill.fExecTime = SkillReader.getFloat(y, x++, 0f);
                    skill.rangType = SkillReader.getInt(y, x++, 0);
                    skill.fRang1 = SkillReader.getFloat(y, x++, 0f);
                    skill.fRang2 = SkillReader.getFloat(y, x++, 0f);
                    skill.strEffect = SkillReader.getStr(y, x++);
                    skill.strGesture = SkillReader.getStr(y, x++);
                    mHeroSkillList.Add(new stHeroSkillCfg(skill.strHero, skill.strSkill), skill);
                }
            }
            getContent();
            return true;
        }

        public SkillShow.HeroSkill getSkill(string strHero, string strSKill)
        {
            foreach (KeyValuePair<stHeroSkillCfg, SkillShow.HeroSkill> item in this.mHeroSkillList)
            {
                if (strHero == item.Key.strHero && strSKill == item.Key.strSkill)
                    return item.Value;
            }
            SkillShow.HeroSkill skill = new SkillShow.HeroSkill();
            skill.strSkill = strSKill;
            skill.strHero = strHero;
            skill.strBoolParam = strSKill;
            mHeroSkillList.Add(new stHeroSkillCfg(strHero, skill.strSkill), skill);
            return skill;
        }

        public SkillShow.HeroSkill getSkillName(string strHero, string strSKill)
        {
            foreach (KeyValuePair<stHeroSkillCfg, SkillShow.HeroSkill> item in this.mHeroSkillList)
            {
                if (strHero == item.Key.strHero && strSKill == item.Key.strSkill)
                    return item.Value;
            }

            return null;
        }

        public SkillShow.HeroSkill getSkillByGesture(string strHero, string strGesture)
        {
            foreach (KeyValuePair<stHeroSkillCfg, SkillShow.HeroSkill> item in this.mHeroSkillList)
            {
                if (strHero == item.Key.strHero && strGesture == item.Value.strGesture)
                    return item.Value;
            }

            return null;
        }

        string getContent()
        {
            string strContent = m_strTitle;

            foreach (KeyValuePair<stHeroSkillCfg, SkillShow.HeroSkill> item in this.mHeroSkillList)
            {
                string strNewLine = "\r\n";
                strNewLine += item.Value.strHero + ",";
                strNewLine += item.Value.strSkill + ",";
                strNewLine += item.Value.strBoolParam + ",";
                strNewLine += item.Value.fColdTime.ToString() + ",";
                strNewLine += item.Value.fExecTime.ToString() + ",";
                strNewLine += item.Value.rangType.ToString() + ",";
                strNewLine += item.Value.fRang1.ToString() + ",";
                strNewLine += item.Value.fRang2.ToString() + ",";
                strNewLine += item.Value.strEffect + ",";
                strNewLine += item.Value.strGesture;
                strContent += strNewLine;
            }
            return strContent;
        }

        public void save()
        {
            try
            {
                //先删除
                File.Delete(m_strPath);
                StreamWriter sw = null;
                FileInfo file = new FileInfo(m_strPath);
                if (!file.Exists)
                    sw = file.CreateText();
                else
                    sw = file.AppendText();
                sw.WriteLine(getContent());
                sw.Close();
                sw.Dispose();
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
	}
}
