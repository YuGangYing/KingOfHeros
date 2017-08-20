using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
namespace DataMgr
{
    class IniItem
    {
        public string strKey = string.Empty;
        public string strValue = string.Empty;

        public IniItem(string key,string value)
        {
            strKey = key;
            strValue = value;
        }
    }

    class IniSection
    {
        public Dictionary<string, string> mDict = new Dictionary<string, string>();

        public IniSection()
        {
        }

        public void addItem(string strKey, string strValue)
        {
            //空行
            strKey = strKey.TrimStart();
            strKey = strKey.TrimEnd();
            strValue = strValue.TrimStart();
            strValue = strValue.TrimEnd();
            if (mDict.ContainsKey(strKey))
                Logger.LogError("language key {0} already exists!", strKey);
            mDict[strKey] = strValue;
        }

        public string getString(string strKey)
        {
            string strOut;
            if (mDict.TryGetValue(strKey, out strOut))
                return strOut;
            return string.Empty;
        }
    }

	class iniReader
	{
        public Dictionary<string, List<string>> mDict = new Dictionary<string, List<string>>();
        public Dictionary<string, IniSection> mIniDict = new Dictionary<string,IniSection>();
        
        string strDefault = "default";
        string strCur = string.Empty;

        public iniReader()
        {

        }

        public void release()
        {
            mDict.Clear();
            mIniDict.Clear();
        }

        public bool loadFile(string strPath)
        {
            mDict.Clear();

            strCur = strDefault;

            TextAsset fileText = DataMgr.ResourceCenter.LoadAsset<TextAsset>(strPath);
            if (fileText == null)
                return false;

            string[] strLines = fileText.text.Split('\n');
            for (int i = 0; i < strLines.Length; i++)
            {
                string strLine = strLines[i];
                if (strLine.StartsWith("\r"))
                    strLine = strLine.Substring(1, strLine.Length - 1);
                else if (strLine.EndsWith("\r"))
                    strLine = strLine.Substring(0, strLine.Length - 1);
                if (strLine != string.Empty)
                    parseLine(strLine);
            }
            return true;
        }

        public bool parseIni()
        {
            foreach (KeyValuePair<string, List<string>> item in mDict)
            {
                foreach (string strLine in item.Value)
                {
                    IniItem line = parseIniLine(strLine);
                    if (line != null)
                    {
                        IniSection sec = null;
                        if (!mIniDict.TryGetValue(item.Key, out sec))
                        {
                            sec = new IniSection();
                            mIniDict.Add(item.Key,sec);
                        }
                        if(sec!=null)
                            sec.addItem(line.strKey, line.strValue);
                    }
                }
            }
            return true;
        }

        bool parseLine(string strLine)
        {
            if (strLine == null || strLine == string.Empty)
                return false;
            //空行
            strLine = strLine.TrimStart();
            strLine = strLine.TrimEnd();
            if (strLine==string.Empty)
                return false;
            //开始新段
            if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                strCur = strLine.Substring(1, strLine.Length - 2);
            else
            {
                List<string> module;
                if (!mDict.TryGetValue(strCur, out module))
                {
                    module = new List<string>();
                    mDict.Add(strCur, module);
                }
                if (module != null)
                    module.Add(strLine);
            }
            return true;
        }

        IniItem parseIniLine(string strLine)
        {
            if (strLine.StartsWith("="))
                return null;
            if (strLine.StartsWith("//"))
                return null;

            string[] strSplit = strLine.Split('=');
            if (strSplit.Length < 2)
                return null;
            string strKey = strSplit[0];
            int nIndex = strKey.Length;
            strKey = strKey.TrimStart();
            strKey = strKey.TrimEnd();

            return new IniItem(strKey, strSplit[1]);
        }
	}
}
