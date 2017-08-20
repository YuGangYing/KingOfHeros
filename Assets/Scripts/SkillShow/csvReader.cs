using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace SkillShow
{
    public class CsvReader
    {
        protected List<List<string>> m_dataList = new List<List<string>>(); // 数据
        public string mTitle;

        public void Load(StreamReader reader, char compart, bool bAdd = false)
        {
            if (!bAdd)
                m_dataList.Clear();

            string szLine;
            while ((szLine = reader.ReadLine()) != null)
            {
                if (szLine.Length >= 2 && szLine[0] == '/' && szLine[1] == '/')
                    continue;

                List<string> lineList = new List<string>();
                m_dataList.Add(lineList);

                SLG.Util.Split(ref szLine, lineList, compart);
            }
        }

        public void Load(string reader, char compart, bool bAdd = false)
        {
            if (!bAdd)
                m_dataList.Clear();

            List<string> listlines = new List<string>();
            string line = "";
            bool bCom = false;
            for (int i = 0; i < reader.Length; ++i)
            {
                if (reader[i] == '\n' || reader[i] == '\r')
                {
                    if (bCom == true)
                        line += reader[i];
                    else
                    {
                        if (line.Length > 0)
                        {
                            listlines.Add(line);
                            line = "";
                        }
                    }
                }
                else if (reader[i] == '"')
                {
                    bCom = !bCom;
                }
                else
                {
                    line += reader[i];
                }
            }

            string[] lines = listlines.ToArray();
            if (lines.Length == 0)
                return;
            mTitle = lines[0];
            string szLine = "";
            for (int i = 0; i < lines.Length; ++i)
            {
                szLine = lines[i];
                if (szLine.Length >= 2 && szLine[0] == '/' && szLine[1] == '/')
                    continue;

                List<string> lineList = new List<string>();
                m_dataList.Add(lineList);

                SLG.Util.Split(ref szLine, lineList, compart);
            }
        }

        // "得到第y行的列数"
        public int getXCount(int y)
        {
            if (m_dataList.Count <= y)
                return 0;

            return m_dataList[y].Count;
        }

        // "得到行数"
        public int getYCount()
        {
            return m_dataList.Count;
        }

        // "得到字符串"
        public string getStr(int y, int x)
        {
            if (getYCount() > y && getXCount(y) > x)
                return m_dataList[y][x];

            return "";
        }

        public int getInt(int y, int x, int defValue)
        {
            string text = getStr(y, x);
            if (text.Length == 0)
                return defValue;

            int result = 0;
            if (System.Int32.TryParse(text, out result) == true)
                return result;

            return defValue;
        }

        public ushort getUShort(int y, int x, ushort defValue)
        {
            string text = getStr(y, x);
            if (text.Length == 0)
                return defValue;

            ushort result = 0;
            if (System.UInt16.TryParse(text, out result) == true)
                return result;

            return defValue;
        }

        public float getFloat(int y, int x, float defValue)
        {
            string text = getStr(y, x);
            if (text.Length == 0)
                return defValue;

            float reslut = 0;
            if (System.Single.TryParse(text, out reslut) == true)
                return reslut;

            return defValue;
        }

        public bool getBool(int y, int x, bool defValue)
        {
            string text = getStr(y, x);
            if (text.Length == 0)
                return defValue;

            if (text[0] == '0')
                return false;

            return true;
        }
    }
}
