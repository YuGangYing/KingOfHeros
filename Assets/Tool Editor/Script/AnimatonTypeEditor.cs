#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AnimatonTypeEditor : EditorWindow
{
    static public AnimatonTypeEditor me { get; private set; }
    void OnEnable()
    {
        me = this;
    }

    public void Hide()
    {
        if (me)
            me.Close();
    }

    static public List<string> AnimatonList
    {
        get
        {
            if (animList.Count == 0)
            {
                Load();
            }

            return animList;
        }
    }

    public static int GetAnimatonIndex(string n)
    {
        List<string> tmp = AnimatonList;
        for (int i = 0; i < tmp.Count; ++i)
        {
            if (tmp[i] == n)
                return i;
        }

        return -1;
    }

    // 动画类型链表
    static private List<string> animList = new List<string>();
    static private string DefaultFile = Application.dataPath + "/Tool Editor/Data/AnimatorType.csv";
    static bool s_dirty = false;

    static void Load()
    {
        string filepath = DefaultFile;
        if (File.Exists(filepath) == false)
        {
            Directory.CreateDirectory(ToolUtil.GetFileDirectory(filepath));
            File.Create(filepath);
        }

        animList.Clear();
        FileStream fileStream = File.OpenRead(filepath);
        StreamReader sr = new StreamReader(fileStream);

        FileHelper fh = new FileHelper();
        bool bRet = fh.open(sr);
        if (!bRet)
        {
            Debug.Log("加载文件:" + filepath + "失败!");
            return;
        }
        sr.Close();

        string type;
        for (int i = 0; i < fh.rowAmount; ++i)
        {
            if (!string.IsNullOrEmpty(type = fh.getStr(i, 0)))
                animList.Add(type);
        }
        s_dirty = false;
        Debug.Log("加载文件:"+filepath+"成功!");
    }

    static void Save()
    {
        string filepath = DefaultFile;
        if (File.Exists(filepath) == false)
        {
            Directory.CreateDirectory(ToolUtil.GetFileDirectory(filepath));
            File.Create(filepath);
        }

        FileStream fileStream = File.OpenWrite(filepath);
        fileStream.SetLength(0);
        string text = "";
        for (int i = 0; i < animList.Count; ++i)
        {
            text += animList[i];
            text += "\n";
        }

        byte[] bytes = SLG.Util.ToByte(ref text);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
        s_dirty = false;

        Debug.Log("保存动画文件成功!");
    }

    void OnDisable()
    {
        if (s_dirty == true)
        {
            if (EditorUtility.DisplayDialog("是否确定保存修改", "文件被修改，是否确定保存?", "保存", "放弃") == true)
            {
                Save();
            }
            else
            {
                Load();
            }
        }
        me = null;
    }

    string newType="";
    bool bSet = false;
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        ToolUtil.Button("重加载", delegate() { Load(); }, GUILayout.Width(100f), GUILayout.Height(30f));
        ToolUtil.Button("保存", delegate() { Save(); }, GUILayout.Width(100f), GUILayout.Height(30f));
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        if (bSet == false)
        {
            string type = GUILayout.TextField("新动画类型", GUILayout.Width(100f));
            if (type != "新动画类型")
                newType = type;

            bSet = true;
        }
        else
        {
            newType = GUILayout.TextField(newType, GUILayout.Width(100f));
        }

        if (GUILayout.Button("添加", GUILayout.Width(100f)))
        {
            if (!string.IsNullOrEmpty(newType))
            {
                if (!animList.Contains(newType))
                {
                    animList.Add(newType);
                    s_dirty = true;
                    newType = "";
                }
            }
        }

        GUILayout.EndHorizontal();

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < animList.Count; )
        {
            string s = animList[i];
            GUILayout.BeginHorizontal();
            GUILayout.Label(s, GUILayout.Width(80f));
            if (GUILayout.Button("删除", GUILayout.Width(80f)))
            {
                animList.RemoveAt(i);
                s_dirty = true;
            }
            else
                ++i;

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

    Vector2 scrollPosition;
}
#endif
