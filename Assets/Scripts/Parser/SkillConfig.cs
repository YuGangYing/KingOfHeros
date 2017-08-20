using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
#pragma warning disable 0114

// 技能数据配置
[System.Serializable]
public class SkillConfig
{
    //     // 技能的类型
    //     public enum TYPE
    //     {
    //         Null, // 空类型
    //         Intonate, // 吟唱
    //         Blink, // 瞬发技能
    //     }

    public int id = -1; // 技能ID
    public string name = ""; // 技能名
    public float cdTime = 0f; // 技能CD时间
    public string icon = ""; // 技能图标

//if UNITY_EDITOR
    public bool IsEditValid = false; // 此部分数据是否有效
    public UIAtlas iconAtlas; // 技能图标
    public UISpriteData iconSprite;
    public string IconSpriteName
    {
        get { return (iconAtlas == null || iconSprite == null) ? "" : iconSprite.name; }
    }

    //     public RuntimeAnimatorController releaseRactrl; // 动画控制器
    //     public RuntimeAnimatorController targetRactrl; // 动画控制器
    public Xft.XffectComponent startEffect; // 起始特效
    //public AnimationClip startAnim; // 起始动作

    public Xft.XffectComponent flyEffect; // 飞行特效

    public Xft.XffectComponent blowEffect; // 受击特效
    //public AnimationClip blowAnim; // 受击动作

#if UNITY_EDITOR
    // 更新编辑器界面数据
    public void UpdateEditorToData()
    {
        icon = (IconSpriteName == "" ? "" : string.Format("{0}:{1}", ToolUtil.GetAssetPath(iconAtlas), IconSpriteName));

        // 特效
        startEffName = ToolUtil.GetAssetPath(startEffect); // 起始特效
        flyEffName = ToolUtil.GetAssetPath(flyEffect); // 飞行特效
        blowEffName = ToolUtil.GetAssetPath(blowEffect); // 受击特效

        // 动作
        //         startAnimName = (startAnim != null ? startAnim.name : "");
        //         blowAnimName = (blowAnim != null ? blowAnim.name : "");
    }
    
    public void UpdateDataToEditor()
    {
        string[] t = icon.Split(':');
        if (t.Length == 2)
        {
            iconAtlas = ToolUtil.LoadAssetPathGetComponent<UIAtlas>(t[0]);
            if (iconAtlas != null)
                iconSprite = iconAtlas.GetSprite(t[1]);
        }

        startEffect = ToolUtil.LoadAssetPathGetComponent<Xft.XffectComponent>(startEffName);
        flyEffect = ToolUtil.LoadAssetPathGetComponent<Xft.XffectComponent>(flyEffName);
        blowEffect = ToolUtil.LoadAssetPathGetComponent<Xft.XffectComponent>(blowEffName);
    }

#endif

    public string[] ToString()
    {
        List<string> strList = new List<string>();
        strList.Add(id.ToString());
        strList.Add(name);
        strList.Add(icon);
        strList.Add(cdTime.ToString());

        strList.Add(startAnimName);
        strList.Add(startEffName);
        strList.Add(flyEffName);
        strList.Add(blowAnimName);
        strList.Add(blowEffName);

        strList.Add(areaType.ToString());
        strList.Add(areaParam.ToString());
        strList.Add(fireGraphType.ToString());
        return strList.ToArray();
    }

//#endif

    public static SkillConfig CreateByString(ref FileHelper reader, int x, int y)
    {
        int startindex = x;
        SkillConfig config = new SkillConfig();
        config.id = reader.getInt(y, startindex++, -1);
//         if (config.id == -1)
//             return null;

        config.name = reader.getStr(y, startindex++);
        config.icon = reader.getStr(y, startindex++);
        config.cdTime = reader.getFloat(y, startindex++, 0.0f);

        config.startAnimName = reader.getStr(y, startindex++);
        config.startEffName = reader.getStr(y, startindex++);
        config.flyEffName = reader.getStr(y, startindex++);
        config.blowAnimName = reader.getStr(y, startindex++);
        config.blowEffName = reader.getStr(y, startindex++);

        config.areaType = SLG.Util.StringToEnum<SkillConfig.Area>(reader.getStr(y, startindex++), Area.Null);
        config.areaParam = reader.getFloat(y, startindex++, 0f);
        config.fireGraphType = SLG.Util.StringToEnum<SkillConfig.FireGraph>(reader.getStr(y, startindex++), FireGraph.Null);

        return config;
    }

    public string startAnimName; // 起始动作名
    public string startEffName; // 起始特效名

    public string flyEffName; // 飞行特效名
    public float flySpeed; // 飞行速度

    public string blowAnimName; // 受击动作名
    public string blowEffName; // 受击特效名

    // 技能的攻击范围
    public enum Area
    {
        Null, // 空
        Single, // 单体
        Curved, // 弧形,以施法者朝向为弧形的中心
    }

    public Area areaType = Area.Curved; // 攻击范围类型
    public float areaParam = 3.0f; // 范围参数

#if UNITY_EDITOR
    public void OnDrawAttackArea(Transform transform)
    {
        switch (areaType)
        {
        case Area.Null:
        case Area.Single:
            break;
        case Area.Curved:
            ToolDrawGizmos.me.Begin();
            ToolDrawGizmos.me.DrawSphere(transform.position, areaParam);
            ToolDrawGizmos.me.End();
            break;
        }
    }
#endif

    // 技能的释放图形
    public enum FireGraph
    {
        Null,
        FireGraph1,
        FireGraph2,
        FireGraph3,
        FireGraph4,
    }

    public FireGraph fireGraphType = FireGraph.Null; // 释放类型
}
