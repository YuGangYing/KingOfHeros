//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// EditorGUILayout.ObjectField doesn't support custom components, so a custom wizard saves the day.
/// Unfortunately this tool only shows components that are being used by the scene, so it's a "recently used" selection tool.
/// </summary>

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162

public class ToolEditor : ScriptableWizard
{
    public delegate void OnSelectionCallback(Object obj);
    public delegate bool OnFilterCallback(Object obj);

    System.Type mType;
    OnSelectionCallback mCallback;
    OnFilterCallback mFilterCallback;
    Object[] mObjects;

    /// <summary>
    /// Draw a button + object selection combo filtering specified types.
    /// </summary>

    static public void Draw<T>(string buttonName, T obj, OnSelectionCallback cb, OnFilterCallback fc = null, params GUILayoutOption[] options) where T : Object
    {
        GUILayout.BeginHorizontal();
        bool show = GUILayout.Button(buttonName, "DropDownButton", GUILayout.Width(76f), GUILayout.MinHeight(30f));
        GUILayout.BeginVertical();
        GUILayout.Space(5f);

        T o = EditorGUILayout.ObjectField(obj, typeof(T), false, options) as T;
        GUILayout.EndVertical();

        if (o != null && Selection.activeObject != o && GUILayout.Button("Edit", GUILayout.Width(40f)))
        {
            Selection.activeObject = o;
        }
        GUILayout.EndHorizontal();
        if (show) Show<T>(cb);
        else if (o != obj) cb(o);
    }

    /// <summary>
    /// Draw a button + object selection combo filtering specified types.
    /// </summary>

    static public void Draw<T>(T obj, OnSelectionCallback cb, OnFilterCallback fc = null, params GUILayoutOption[] options) where T : Object
    {
        Draw<T>(typeof(T).ToString(), obj, cb, fc, options);
    }

    /// <summary>
    /// Show the selection wizard.
    /// </summary>

    static void Show<T>(OnSelectionCallback cb, OnFilterCallback fc = null) where T : Object
    {
        System.Type type = typeof(T);
        ToolEditor comp = ScriptableWizard.DisplayWizard<ToolEditor>("Select " + type.ToString());
        comp.mType = type;
        comp.mCallback = cb;

        //comp.mObjects = Resources.FindObjectsOfTypeAll(type) as Object[];
        if (fc != null)
        {
            List<Object> allList = new List<Object>();
            comp.mObjects = FindObjectsOfTypeAllEx<T>() as Object[];
            foreach (Object o in comp.mObjects)
            {
                if (fc(o) == true)
                    allList.Add(o);
            }

            comp.mObjects = allList.ToArray();
        }
        else
        {
            comp.mObjects = FindObjectsOfTypeAllEx<T>() as Object[];
        }
    }

    static T[] FindObjectsOfTypeAllEx<T>() where T : Object
    {
        System.Type tType = typeof(T);
        if ((new List<System.Type>(tType.GetInterfaces())).Contains(typeof(Behaviour)))
        {
            List<T> resultlist = new List<T>();
            List<GameObject> objs = new List<GameObject>();
            ToolUtil.CollectAllRecursion<GameObject>("Assets", objs);
            Component t = null;
            for (int i = 0; i < objs.Count; )
            {
                if ((t = (objs[i].GetComponent(tType))) != null)
                {
                    resultlist.Add(t as T);
                }
            }

            return resultlist.ToArray();
        }
        else
        {
            List<T> objs = new List<T>();
            ToolUtil.CollectAllRecursion<T>("Assets", objs);
            return objs.ToArray();
        }
    }

    /// <summary>
    /// Draw the custom wizard.
    /// </summary>

    void OnGUI()
    {
        EditorGUIUtility.LookLikeControls(80f);
        GUILayout.Label("Recently used components", "LODLevelNotifyText");
        NGUIEditorTools.DrawSeparator();

        if (mObjects.Length == 0)
        {
            EditorGUILayout.HelpBox("No recently used " + mType.ToString() + " components found.\nTry drag & dropping one instead, or creating a new one.", MessageType.Info);

            bool isDone = false;

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (mType == typeof(UIFont))
            {
                if (GUILayout.Button("Open the Font Maker", GUILayout.Width(150f)))
                {
                    EditorWindow.GetWindow<UIFontMaker>(false, "Font Maker", true);
                    isDone = true;
                }
            }
            else if (mType == typeof(UIAtlas))
            {
                if (GUILayout.Button("Open the Atlas Maker", GUILayout.Width(150f)))
                {
                    EditorWindow.GetWindow<UIAtlasMaker>(false, "Atlas Maker", true);
                    isDone = true;
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (isDone) Close();
        }
        else
        {
            Object sel = null;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            foreach (Object o in mObjects)
            {
                if (DrawObject(o))
                {
                    sel = o;
                }
            }
            GUILayout.EndScrollView();

            if (sel != null)
            {
                mCallback(sel);
                Close();
            }
        }
    }

    /// <summary>
    /// Draw details about the specified monobehavior in column format.
    /// </summary>

    Vector2 scrollPosition = Vector2.zero;
    bool DrawObject(Object mb)
    {
        bool retVal = false;

        float itemHight = 20f;
        GUILayout.Label("", GUILayout.Height(5f));

        GUILayout.BeginHorizontal();
        {
            if (EditorUtility.IsPersistent(mb))
            {
                GUILayout.Label("Prefab", "AS TextArea", GUILayout.Width(80f), GUILayout.Height(itemHight));
            }
            else
            {
                GUI.color = Color.grey;
                GUILayout.Label("Object", "AS TextArea", GUILayout.Width(80f), GUILayout.Height(itemHight));
            }

//             if (mb as Behaviour)
//             {
//                 //GUILayout.Label(NGUITools.GetHierarchy(((Behaviour)mb).gameObject), "AS TextArea", GUILayout.Width(200f), GUILayout.Height(20f));
//             }

            //GUILayout.Label(AssetDatabase.GetAssetPath(mb), GUILayout.Height(20f));

            UnityEditor.EditorGUILayout.ObjectField(mb, mb.GetType(), false, GUILayout.Width(200f), GUILayout.Height(itemHight));
            
            if (GUILayout.Button("Edit", GUILayout.Width(40f), GUILayout.Height(itemHight)))
                Selection.activeObject= mb;

            GUI.color = Color.white;
            if (mb as UIAtlas)
            {
                UIAtlas atlas = (UIAtlas)mb;
                Texture texture = atlas.texture;
                retVal = GUILayout.Button(new GUIContent(AssetDatabase.GetAssetPath(mb), texture), "ButtonLeft", GUILayout.Height(itemHight));
            }
            else
            {
                retVal = GUILayout.Button(AssetDatabase.GetAssetPath(mb), "ButtonLeft", GUILayout.Height(itemHight));
            }
        }

        GUILayout.EndHorizontal();

//         if (mb as UIAtlas)
//         {
//             UIAtlas atlas = (UIAtlas)mb;
//             Texture texture = atlas.texture;
//             Rect area = UnityEditor.EditorGUILayout.GetControlRect();
//             GUILayout.Label(texture);
//         }

        return retVal;
    }
}
