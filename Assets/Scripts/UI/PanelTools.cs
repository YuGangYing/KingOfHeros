using System;
using UnityEngine;
using System.Collections.Generic;

namespace UI
{
	class PanelTools
	{                
        // 查找子窗口
        public static GameObject FindChild(GameObject parent, string name)
        {
            GameObject obj;
            Transform tmp;
            for (int i = 0; i < parent.transform.childCount; ++i)
            {
                if ((tmp = parent.transform.GetChild(i)).name == name)
                    return tmp.gameObject;

                if ((obj = FindChild(tmp.gameObject, name)) != null)
                    return obj;
            }

            return null;
        }

        // 查找子窗口,通过分隔符'/'来确定父子窗口
        public static GameObject Find(GameObject parent, string name)
        {
            string[] childs = name.Split('/');
            Transform p = parent.transform;
            foreach (string child in childs)
            {
                p = p.transform.Find(child);
                if (p == null)
                {
                    Logger.LogWarning("name:{0} not find!", name);
                    return null;
                }
            }

            return p.gameObject;
        }

        // 查找子窗口,通过分隔符'/'来确定父子窗口
        public static T Find<T>(GameObject parent, string name) where T : Component
        {
            string[] childs = name.Split('/');
            Transform p = parent.transform;
            foreach (string child in childs)
            {
                p = p.transform.Find(child);
                if (p == null)
                {
                    Logger.LogWarning("{1} name:{0} not find!", name, typeof(T).Name);
                    return null;
                }
            }

            return p.gameObject.GetComponent<T>();
        }

        static Dictionary<string, UIAtlas> UIAtlasList;
        public static Dictionary<string, UIAtlas> GetUIAtlasList()
        {
            if (UIAtlasList != null)
                return UIAtlasList;

            UIAtlasList = new Dictionary<string, UIAtlas>();
            UIAtlas[] atlas = Resources.FindObjectsOfTypeAll<UIAtlas>();
            foreach (UIAtlas atla in atlas)
            {
                if (UIAtlasList.ContainsKey(atla.name) == false)
                {
                    UIAtlasList[atla.name] = atla;
                }
            }

            return UIAtlasList;
        }

        public static UIAtlas GetUIAtlas(string name)
        {
            UIAtlas atlas = null;
            if (GetUIAtlasList().TryGetValue(name, out atlas))
                return atlas;

            return null;
        }

        public static void SetSpriteIcon(UISprite sprite, string icon)
        {
            string[] s = null;
            if (string.IsNullOrEmpty(icon) || ((s = icon.Split(':')).Length != 2))
            {
                sprite.atlas = null;
            }
            else
            {
                sprite.atlas = GetUIAtlas(s[0]);
                sprite.spriteName = s[1];
            }
        }

        /*
        通过分隔符'/'来确定父子窗口设置多语言字符串查找子窗口中的UILabel,
        然后设置标签
        */
        public static void setLabelText(GameObject parent, string name,int id)
        {
            UILabel label = PanelTools.Find<UILabel>(parent,name);
            if(label!=null)
                label.text = DataMgr.DataManager.getLanguageMgr().getString(id);
        }

        public static void setLabelText(GameObject parent, string name,string id)
        {
            UILabel label = PanelTools.Find<UILabel>(parent,name);
            if(label!=null)
                label.text = DataMgr.DataManager.getLanguageMgr().getString(id);
        }

    }
}
