using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
namespace DataMgr
{
    public enum SYSTEM_CFG
    {
        MUSIC=0,
        SOUND,
        PURCHASE,
        PUSHMSG,
        MAINCITYCAMERA,
        BATTLECAMERA,
        MAX
    }

    public class SystemCfg
    {
        string _cfgArea = "Setting";
        string _strFolder = "setting/";
        string _cfgIni = "gameSetting";
        static string[] _szKey = { "Music", "Sound", "Purchase", "PushMsg","maincityCamera","battleCamera"};

        class CData
        {
            public int isOn;        // 0 is off !0 is on
            public string strKey;

            public CData()
            {
            }

            public CData(int nValue, string str)
            {
                isOn = nValue;
                strKey = str;
            }
        }

        Dictionary<string, CData> mmapData = null;

        public SystemCfg()
        {
            mmapData = new Dictionary<string, CData>();
        }


        public void init()
        {
            _DefaultInit();

            if (!_ReadIni(_cfgIni))
            {
                return;
            }

        }

        public void release()
        {
            _SaveIni();
        }

        void _DefaultInit()
        {
            if (mmapData == null)
                return;

            for (int n = 0; n < _szKey.Length; n++)
            {
                CData cd = new CData(100, _szKey[n]);
                mmapData.Add(cd.strKey, cd);
            }
        }


        bool _ReadIni(string strFileName)
        {
            int nResult = 1;
            string strValue = "1";
            foreach (var item in _szKey)
	        {
		        strValue = PlayerPrefs.GetString(item, "1");

                System.Int32.TryParse(strValue, out nResult);

                if(mmapData.ContainsKey(item))
                    mmapData[item].isOn = nResult;
	        }

            return true;
        }

        bool _SaveIni()
        {
            foreach (var item in mmapData)
            {
                PlayerPrefs.SetString(item.Key, item.Value.isOn.ToString());
            }

            return true;
        }

        public bool getValue(SYSTEM_CFG item)
        {
            string strKey = _szKey[(int)item];
            int nValue = 0;
            if(mmapData.ContainsKey(strKey))
                nValue = mmapData[strKey].isOn;
            if (nValue != 0)
                return true;

            return false;
        }

        public void setValue(SYSTEM_CFG item, int nValue)
        {
            string strKey = _szKey[(int)item];
            if (mmapData.ContainsKey(strKey))
                mmapData[strKey].isOn = nValue;
            _SaveIni();
        }
    }
}