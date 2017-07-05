using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class ToolUtil
{
    static public void LineTextAndString(string text, float twidth, ref string v, float vwidth, float height = -1f)
    {
        GUILayout.BeginHorizontal();
        if (height==-1f)
            GUILayout.Label(text + ":", GUILayout.Width(twidth));
        else
            GUILayout.Label(text + ":", GUILayout.Width(twidth), GUILayout.Height(height));

        v = GUILayout.TextArea(v, GUILayout.Width(vwidth));
        GUILayout.EndHorizontal();
    }

    static public void LineTextAndString(string text,GUIStyle style, float twidth, ref string v, float vwidth, float height = -1f)
    {
        GUILayout.BeginHorizontal();
        if (height == -1f)
            GUILayout.Label(text + ":", style, GUILayout.Width(twidth));
        else
            GUILayout.Label(text + ":", style, GUILayout.Width(twidth), GUILayout.Height(height));

        v = GUILayout.TextArea(v, style, GUILayout.Width(vwidth));
        GUILayout.EndHorizontal();
    }

    static public void LineTextAndFloat(string text, float twidth, ref float v, float vwidth, float height = -1f)
    {
        GUILayout.BeginHorizontal();
        if (height == -1f)
            GUILayout.Label(text + ":", GUILayout.Width(twidth));
        else
            GUILayout.Label(text + ":", GUILayout.Width(twidth), GUILayout.Height(height));

        v = ToFloat(GUILayout.TextArea(v.ToString(), GUILayout.Width(vwidth)), v);
        GUILayout.EndHorizontal();
    }

    static public void LineTextAndInt(string text, float twidth, ref int v, float vwidth, float height = -1f)
    {
        GUILayout.BeginHorizontal();
        if (height == -1f)
            GUILayout.Label(text + ":", GUILayout.Width(twidth));
        else
            GUILayout.Label(text + ":", GUILayout.Width(twidth), GUILayout.Height(height));
        v = ToInt(GUILayout.TextArea(v.ToString(), GUILayout.Width(vwidth)), v);
        GUILayout.EndHorizontal();
    }

    static public void LineTextAndInt(string text, GUIStyle style, float twidth, ref int v, float vwidth, float height = -1f)
    {
        GUILayout.BeginHorizontal();
        if (height == -1f)
            GUILayout.Label(text + ":",style, GUILayout.Width(twidth));
        else
            GUILayout.Label(text + ":", style, GUILayout.Width(twidth), GUILayout.Height(height));
        v = ToInt(GUILayout.TextArea(v.ToString(), style, GUILayout.Width(vwidth)), v);
        GUILayout.EndHorizontal();
    }

    static float ToFloat(string v, float def)
    {
        float.TryParse(v, out def);
        return def;
    }

    static int ToInt(string v, int def)
    {
        int.TryParse(v, out def);
        return def;
    }

    public delegate void OnClick();
    static public bool Button(string text, OnClick call, params GUILayoutOption[] options)
    {
        if (GUILayout.Button(text, options))
        {
            if (call != null)
                call();

            return true;
        }

        return false;
    }

    static public bool Toggle(ref bool v, string text, params GUILayoutOption[] options)
    {
        v = GUILayout.Toggle(v, text, options);
        return v;
    }

#if UNITY_EDITOR
    public static string GetAssetPath(Object o)
    {
        if (o == null)
            return "";

        return UnityEditor.AssetDatabase.GetAssetPath(o);
    }

    public static T LoadAssetPath<T>(string path) where T : Object
    {
        return (T)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T));
    }

    public static T LoadAssetPathGetComponent<T>(string path) where T : Component
    {
        GameObject obj = LoadAssetPath<GameObject>(path);
        if (obj == null)
            return null;

        return obj.GetComponent<T>();
    }
    public static List<T> CollectAll<T>(string path) where T : Object
    {
        List<T> l = new List<T>();
        string[] files = Directory.GetFiles(path);

        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;
            T asset = (T)UnityEditor.AssetDatabase.LoadAssetAtPath(file, typeof(T));
            if (asset == null)
                continue;
            l.Add(asset);
        }
        return l;
    }

    // 递归调用
    public static void CollectAllRecursion<T>(string path, List<T> l) where T : Object
    {
        l.AddRange(CollectAll<T>(path));

        string[] pathfiles = Directory.GetDirectories(path);
        foreach (string pathfile in pathfiles)
        {
            if (pathfile == path)
                continue;

            CollectAllRecursion<T>(pathfile, l);
        }
    }
#endif

    static string TookPrefsKey = "Tool:";

    // 保存的Key修饰
    static public string PrefsKey(string k)
    {
        return TookPrefsKey + k;
    }

    // 保存的Key去掉修饰
    static public string ResPrefsKey(string k)
    {
        if (k.Substring(0, TookPrefsKey.Length) != TookPrefsKey)
            return k.Substring(TookPrefsKey.Length, k.Length);

        return k;
    }

    static public string GetFileDirectory(string file)
    {
        return file.Substring(0, file.LastIndexOf('/') + 1);
    }
}