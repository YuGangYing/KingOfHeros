using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SkillEffectConfigEditor : EditorWindow
{
    static public SkillEffectConfigEditor me { get; private set; }

    public static void Hide()
    {
        if (me)
            me.Close();
    }

    public SkillConfig skillConfig
    {
        get { return SkillEffectEditor.current;}
    }

    SkillFire m_skillFire = null;

    static Animator ReleaseObjPrefab; // 施法者
    static Animator BlowObjPrefab; // 目标

    static Animator ReleaseObj; // 施法者
    static Animator BlowObj; // 目标

    bool IsUpateInEditor = false;

    public void OnEnable()
    {
        me = this;
        m_skillFire = new SkillFire();

        if (ReleaseObjPrefab == null)
        {
            string filePath = PlayerPrefs.GetString(ToolUtil.PrefsKey("SE-ReleaseObjPrefab"), "");
            if (filePath != null)
            {
                ReleaseObjPrefab = ToolUtil.LoadAssetPathGetComponent<Animator>(filePath);
            }
        }

        if (BlowObjPrefab == null)
        {
            string filePath = PlayerPrefs.GetString(ToolUtil.PrefsKey("SE-BlowObjPrefab"), "");
            if (filePath != null)
            {
                BlowObjPrefab = ToolUtil.LoadAssetPathGetComponent<Animator>(filePath);
            }
        }
    }

    void OnDisable()
    {
        me = null;
    }

    static double LastTime = -1;
    public void Update()
    {
        if (skillConfig == null)
            return;

        if (ReleaseObj != null)
            skillConfig.OnDrawAttackArea(ReleaseObj.transform);

        if (IsUpateInEditor == false)
        {
            if (m_skillFire != null)
                m_skillFire.Release();
            return;
        }

        if (m_skillFire == null)
        {
            m_skillFire = new SkillFire();
        }

        if (m_skillFire.currState == SkillFire.eState.Null)
        {
            m_skillFire.release = ReleaseObj;
            m_skillFire.target = BlowObj;
            m_skillFire.config = skillConfig;
            m_skillFire.begin();
        }
        else if (m_skillFire.currState == SkillFire.eState.End)
        {
            IsUpateInEditor = false; // 播放完成
            LastTime = -1;
            Repaint();
            Debug.Log("已播放完成!");
        }

        if (LastTime == -1)
            LastTime = EditorApplication.timeSinceStartup;

        m_skillFire.Update((float)(EditorApplication.timeSinceStartup - LastTime));
        LastTime = EditorApplication.timeSinceStartup;
    }

    void SaveToPlayerPrefs(string key, Object obj)
    {
        if (obj == null)
        {
            PlayerPrefs.DeleteKey(ToolUtil.PrefsKey(key));
        }
        else
        {
            PlayerPrefs.SetString(ToolUtil.PrefsKey(key), AssetDatabase.GetAssetPath(obj));
        }
    }

    Vector2 scrollPosition;
    public void OnGUI()
    {
        EditorGUILayout.LabelField("设置预览模型:");
        ToolEditor.Draw<Animator>("施法者", ReleaseObjPrefab, delegate(Object o) { ReleaseObjPrefab = (Animator)o; OnAlter(); SaveToPlayerPrefs("SE-ReleaseObjPrefab", ReleaseObjPrefab); });
        ToolEditor.Draw<Animator>("目标", BlowObjPrefab, delegate(Object o) { BlowObjPrefab = (Animator)o; OnAlter(); SaveToPlayerPrefs("SE-BlowObjPrefab", BlowObjPrefab); });

        if (IsUpateInEditor == true)
        {
            if (ToolUtil.Button("暂停", null, GUILayout.MinHeight(50f)))
            {
                IsUpateInEditor = false;
            }
        }
        else
        {
            if (ToolUtil.Button("播放", null, GUILayout.MinHeight(50f)))
            {
                IsUpateInEditor = true;

                if (!ReleaseObj || !BlowObj)
                {
                    OnPreviewClick();
                    return;
                }
            }
        }

        EditorGUILayout.LabelField("", GUILayout.Height(20f));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        ShowSkillConfig(skillConfig, 30f);
        GUILayout.EndScrollView();
    }

    void OnAlter()
    {
        SkillEffectEditor.s_dirty = true;
    }

    void OnPreviewClick()
    {
        if (skillConfig == null)
            return;

        EditorApplication.SaveCurrentSceneIfUserWantsTo();
        EditorApplication.NewScene();

        // 可以开始创建模型了
        ReleaseObj = ((GameObject)GameObject.Instantiate(ReleaseObjPrefab.gameObject)).GetComponent<Animator>();
        ReleaseObj.gameObject.transform.position = new Vector3(0f, 0f, 5f);
        BlowObj = ((GameObject)GameObject.Instantiate(BlowObjPrefab.gameObject)).GetComponent<Animator>();
        BlowObj.gameObject.transform.position = new Vector3(0f, 0f, -5f);

        ReleaseObj.gameObject.transform.LookAt(BlowObj.transform.position);
        BlowObj.gameObject.transform.LookAt(ReleaseObj.transform.position);

        if (m_skillFire != null)
            m_skillFire.Release();
    }

    void ShowAnimaton(ref string aniName, string text)
    {
        // 受击动作
        List<string> animList = AnimatonTypeEditor.AnimatonList;
        int src = AnimatonTypeEditor.GetAnimatonIndex(aniName);
        int index = UnityEditor.EditorGUILayout.Popup(text, src, animList.ToArray());
        if (index != src)
            aniName = animList[index];
    }

    void ShowSkillConfig(SkillConfig current, float height)
    {
        if (current == null)
        {
            GUILayout.Label("请添加或选择技能!");
            return;
        }

        // 技能ID
        ToolUtil.LineTextAndInt("ID", 60.0f, ref current.id, 300.0f, height);

        // 技能名字
        ToolUtil.LineTextAndString("Name", 60.0f, ref current.name, 300.0f, height);

        // 技能CD时间
        ToolUtil.LineTextAndFloat("CDTime", 60.0f, ref current.cdTime, 300.0f, height);

        // 技能图标
        ToolEditor.Draw<UIAtlas>("IconAtlas", current.iconAtlas, delegate(Object atlas) { current.iconAtlas = (UIAtlas)atlas; Repaint(); }, null);
        if (current.iconAtlas != null)
        {
            NGUIEditorTools.DrawAdvancedSpriteField(current.iconAtlas, current.IconSpriteName, delegate(string spriteName) { Repaint(); if (current.iconAtlas != null) { current.iconSprite = current.iconAtlas.GetSprite(spriteName); current.UpdateDataToEditor(); } }, false);
        }
        ShowAnimaton(ref current.startAnimName, "起始动作");

        // 特效
        ToolEditor.Draw<Xft.XffectComponent>("起始特效", current.startEffect, delegate(Object b) { Repaint(); if (current != null) { current.startEffect = (Xft.XffectComponent)b; current.UpdateEditorToData(); } });

        // 飞行特效
        ToolEditor.Draw<Xft.XffectComponent>("飞行特效", current.flyEffect, delegate(Object b) { Repaint(); if (current != null) { current.flyEffect = (Xft.XffectComponent)b; current.UpdateEditorToData(); } });

        // 受击动作
        ShowAnimaton(ref current.blowAnimName, "受击动作");

        //ToolEditor.Draw<AnimationClip>("受击动作", current.blowAnim, delegate(Object b) { Repaint(); if (current != null) { current.blowAnim = (AnimationClip)b; current.UpdateEditorToData(); } });

        // 受击特效
        ToolEditor.Draw<Xft.XffectComponent>("受击特效", current.blowEffect, delegate(Object b) { Repaint(); if (current != null) { current.blowEffect = (Xft.XffectComponent)b; current.UpdateEditorToData(); } });

        // 技能攻击范围
        current.areaType = (SkillConfig.Area)EditorGUILayout.EnumPopup("攻击范围", current.areaType, GUILayout.Width(300f));
        if (current.areaType == SkillConfig.Area.Curved)
        {
            current.areaParam = EditorGUILayout.Slider("半径:", current.areaParam, 0f, 10f);
        }

        // 
        current.fireGraphType = (SkillConfig.FireGraph)EditorGUILayout.EnumPopup("释放图形", current.fireGraphType, GUILayout.Width(300f));
        if (GUI.changed)
        {
            OnAlter();
        }
    }
}