using System.Collections.Generic;
using UnityEngine;

namespace DataMgr
{
	class ResourceCenter
	{
        static public string heroPrebPath = "";
        static public string soldierPrebPath = "";
        static public string configPath = "Configs/";
        static public string audioPath = "Audios/";
        static public string panelPrebPath = "Prefabs/UI/960X640/Interface/";

        public static T LoadAsset<T>(string strPath) where T : Object
        {
            object obj = Resources.Load(strPath,typeof(T));

            if(obj==null)
            {
				Debug.LogError(string.Format("load resource {0} failed", strPath));
                return null;
            }
            return obj as T;
        }
    }
}
