using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SkillEffectTest : MonoBehaviour
{
    public List<SkillConfig> m_skillList = new List<SkillConfig>();
#if UNITY_EDITOR
    public TextAsset textAsset;
#endif
    public Dictionary<string, Object> m_loadAssetList = new Dictionary<string, Object>(); // 资源
    public List<Object> m_assetList = new List<Object>();
    public SkillFire m_skillFire;
    public Animator release; // 释放者
    public Animator target; // 目标

	// Use this for initialization
	void Start ()
    {
        m_skillFire = new SkillFire();
        //Load();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_skillFire != null)
            m_skillFire.Update(Time.deltaTime);
	}

#if UNITY_EDITOR
    void Load()
    {
        if (textAsset == null)
            return;

        string filePath = ToolUtil.GetAssetPath(textAsset);
        System.IO.StreamReader streamReader = new System.IO.StreamReader(filePath, System.Text.Encoding.GetEncoding("gb2312"));
        FileHelper fh = new FileHelper();
        fh.open(streamReader);

        int y = fh.rowAmount;
        m_skillList.Clear();
        for (int i = 0; i < y; ++i)
        {
            SkillConfig config = SkillConfig.CreateByString(ref fh, 0, i);

            config.IsEditValid = false; // 编辑器数据部分无效
            m_skillList.Add(config);
        }
    }

    void PackageAsset()
    {
        Load();
        m_assetList.Clear();
        m_loadAssetList.Clear();
        foreach (SkillConfig sc in m_skillList)
        {
            // 打包相关的资源
            sc.UpdateDataToEditor();
            m_assetList.Add(sc.startEffect);
            m_assetList.Add(sc.flyEffect);
            m_assetList.Add(sc.blowEffect);

            m_loadAssetList[sc.startEffName] = sc.startEffect;
            m_loadAssetList[sc.flyEffName] = sc.flyEffect;
            m_loadAssetList[sc.blowEffName] = sc.blowEffect;
        }
    }
#endif

    Vector2 scrollPosition;
    SkillConfig current;
    public bool IsLoadAssetNew = false;
    void OnGUI()
    {
#if UNITY_EDITOR
        if (IsLoadAssetNew)
        {
            Debug.Log("资源打包完成!");
            PackageAsset();
            IsLoadAssetNew = false;
        }
#endif

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (SkillConfig sc in m_skillList)
        {
            if (GUILayout.Button(sc.name, GUILayout.Width(120f), GUILayout.Height(60f)))
            {
                current = sc;
                m_skillFire.Release();
                m_skillFire.release = release;
                m_skillFire.target = target;
                m_skillFire.config = current;
                m_skillFire.begin();
            }
        }

        GUILayout.EndScrollView();
    }

    //public List<PointCloudGestureTemplate> Templates;
    //public SkillConfig[] GetSkillFireByFireGraph(SkillConfig.FireGraph f)
    //{
    //    List<SkillConfig> list = new List<SkillConfig>();
    //    foreach(SkillConfig c in m_skillList)
    //    {
    //        if (c.fireGraphType == f)
    //            list.Add(c);
    //    }

    //    return list.ToArray();
    //}

    //void OnCustomGesture(PointCloudGesture gesture)
    //{
    //    for (int i = 0; i < Templates.Count; ++i)
    //    {
    //        PointCloudGestureTemplate template = Templates[i];
    //        if (template == gesture.RecognizedTemplate)
    //        {
    //            SkillConfig[] scs = GetSkillFireByFireGraph((SkillConfig.FireGraph)i+1);
    //            if (scs.Length == 0)
    //                return;

    //            SkillConfig sc = scs[UnityEngine.Random.Range(0, scs.Length - 1)];
    //            current = sc;
    //            m_skillFire.Release();
    //            m_skillFire.release = release;
    //            m_skillFire.target = target;
    //            m_skillFire.config = current;
    //            m_skillFire.begin();
    //            return;
    //        }
    //    }
    //}
}
