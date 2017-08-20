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
#pragma warning disable 0472
[ExecuteInEditMode]
public class HreoObjMgr : MonoBehaviour {

    private Dictionary<string, Object> mHeroList = new Dictionary<string, Object>();
    private Dictionary<string, Object> mEffectList = new Dictionary<string, Object>();

    [System.Serializable]
    public class KeyValue
    {
        public KeyValue(string k, Object o)
        {
            key = k;
            obj = o;
        }
        public string key;
        public Object obj;
    }
    public List<KeyValue> heroPrepList = new List<KeyValue>();
    public List<KeyValue> effectPrepList = new List<KeyValue>();

	void Start () 
    {
        foreach (KeyValue item in heroPrepList)
        {
            mHeroList.Add(item.key, item.obj);
        }
        foreach (KeyValue item in effectPrepList)
        {
            mEffectList.Add(item.key, item.obj);
        }
    }

    public GameObject getHreoObj(string strHero)
    {
        Object HeroObj;
        if (!mHeroList.TryGetValue(strHero, out HeroObj))
            return null;
        return GameObject.Instantiate(HeroObj) as GameObject;
    }

    public GameObject getEffectObj(string strHeroEffect)
    {
        if (strHeroEffect == null)
            return null;
        Object EffectObj;
        if (!mEffectList.TryGetValue(strHeroEffect, out EffectObj))
            return null;
        return GameObject.Instantiate(EffectObj) as GameObject;
    }

#if UNITY_EDITOR
    public string heropath = "Data/Prefabs/Hero"; // 资源路径
    public string effectpath = "Data/ArtResources/Effects/Prefabs/Effect_Hero"; // 资源路径
    public bool PackAllRes; // 打包资源

    void Update()
    {
        if (PackAllRes == true)
        {
            PackAllRes = false;

            loadPreb(heropath, ref heroPrepList);
            loadPreb(effectpath, ref effectPrepList);
        }
    }

    void loadPreb(string path,ref  List<KeyValue> prebList)
    {
        string key;
        string fullPath = "Assets/" + path;
        prebList.Clear();

        // 加载所有资源
        List<Object> objs = new List<Object>();
        ToolUtil.CollectAllRecursion<Object>(fullPath, objs);

        foreach (Object obj in objs)
        {
            if (UnityEditor.PrefabUtility.GetPrefabType(obj) == UnityEditor.PrefabType.Prefab)
            {
                prebList.Add(new KeyValue(obj.name, obj));
            }
        }
    }
#endif
}
