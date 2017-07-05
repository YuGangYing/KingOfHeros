using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SkillEffectEditor : EditorWindow
{
    static public SkillEffectEditor me { get; private set; }
    static string DefaultFilePath
    {
        get
        {
            return Application.dataPath + "/Data/Configs/";
        }
    }

    static string filePath = "";

    void LoadAs()
    {
        string file = EditorUtility.OpenFilePanel("技能特效配置表", DefaultFilePath, "csv");
        if (string.IsNullOrEmpty(file))
            return;
        filePath = file;
        Load();
    }

    bool m_bfirst = true;
    void Load()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            if (m_bfirst == true)
            {
                string file = DefaultFilePath + "SkEffConfig.csv";
                if (File.Exists(file))
                    filePath = file;
                m_bfirst = false;
            }
            else
            {
                filePath = EditorUtility.OpenFilePanel("技能特效配置表", DefaultFilePath, "csv");
            }
        }

        if (string.IsNullOrEmpty(filePath))
            return;

        allList.Clear();
        s_dirty = false;
        StreamReader filestream = new StreamReader(filePath, System.Text.Encoding.GetEncoding("gb2312"));
        //FileStream filestream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        FileHelper fh = new FileHelper();
        fh.open(filestream);
        filestream.Close();

        allList.Clear();
        int y = fh.rowAmount;
        for (int i = 0; i < y; ++i)
        {
            SkillConfig config = SkillConfig.CreateByString(ref fh, 0, i);
            config.IsEditValid = false; // 编辑器数据部分无效
            allList.Add(config);
        }
    }

    void OnEnable()
    {
        me = this;
        Load();
    }

    static public bool s_dirty = false;

    void OnDisable()
    {
        if (s_dirty)
        {
            if (EditorUtility.DisplayDialog("是否确定保存修改", "文件被修改，是否确定保存?", "保存", "放弃") == true)
            {
                Save();
            }
        }

        me = null;
        SkillEffectConfigEditor.Hide();
    }

    FileStream GetFileStream(string file)
    {
        FileStream fileStream = null;
        FileMode[] fileMode = new FileMode[6] { FileMode.CreateNew, FileMode.Create, FileMode.Open, FileMode.OpenOrCreate, FileMode.Truncate, FileMode.Append };
        FileAccess[] fileAccess = new FileAccess[3] { FileAccess.Read, FileAccess.Write, FileAccess.ReadWrite };
        FileShare[] fileShare = new FileShare[6] { FileShare.None, FileShare.Read, FileShare.Write, FileShare.ReadWrite, FileShare.Delete, FileShare.Inheritable };

        for (int i = 0; i < 6; ++i)
            for (int j = 0; j < 3; ++j)
                for (int k = 0; k < 6; ++k)
                {
                    try
                    {
                        fileStream = File.Open(filePath, fileMode[i], fileAccess[j], fileShare[k]);
                        Debug.Log(string.Format("{0}:{1}:{2}", fileMode[i], fileAccess[j], fileShare[k]));
                        //return fileStream;
                    }
                    catch (System.Exception)
                    {

                    }
                }

        return fileStream;
    }

    void Save()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            SaveAs();
        }
        else
        {
            try
            {
                if(File.Exists(filePath))
                    File.Delete(filePath);

                StreamWriter fileStream = new StreamWriter(filePath, false, System.Text.Encoding.GetEncoding("gb2312"));

                //FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                List<List<string>> dataList = new List<List<string>>();
                foreach (SkillConfig config in allList)
                {
                    config.UpdateEditorToData();
                    dataList.Add(new List<string>(config.ToString()));
                }

                string result = SLG.Util.Write(dataList, ',');
                //byte[] bytes = Csv.Util.ToByte(ref result);
                fileStream.Write(result);
                //fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
                s_dirty = false;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning(ex.Message);
            	EditorUtility.DisplayDialog("技能特效编辑器","另一程序正在使用此文件，进程无法访问。", "确定");
                SaveAs();
            }
        }
    }

    void SaveAs()
    {
        string file = EditorUtility.SaveFilePanel("另存为", string.IsNullOrEmpty(filePath) ? DefaultFilePath : filePath, "SkEffConfig.csv", "csv");
        if (string.IsNullOrEmpty(file))
            return;
        filePath = file;
        Save();
    }

    public static SkillConfig current; // 当前配置

    static List<SkillConfig> allList = new List<SkillConfig>();

    void OnTestClick()
    {
        current.UpdateEditorToData();
        current.UpdateDataToEditor();
    }

    void OnAddConfig()
    {
        current = new SkillConfig(); 
        allList.Add(current);
        s_dirty = true;

        EditorWindow.GetWindow<SkillEffectConfigEditor>(false, "SkillEffectConfig", true);
    }

    Vector2 mScroll = Vector2.zero;
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        ToolUtil.Button("增加", OnAddConfig, GUILayout.Width(100f), GUILayout.Height(30f));
        ToolUtil.Button("加载", delegate() { LoadAs(); }, GUILayout.Width(100f), GUILayout.Height(30f));
        ToolUtil.Button("重加载", delegate() { Load(); }, GUILayout.Width(100f), GUILayout.Height(30f));
        ToolUtil.Button("保存", delegate() { Save(); }, GUILayout.Width(100f), GUILayout.Height(30f));
        ToolUtil.Button("另存为", delegate() { SaveAs(); }, GUILayout.Width(100f), GUILayout.Height(30f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("ID", GUILayout.Height(25f));
        GUILayout.Label("名字", GUILayout.Height(25f));
        //GUILayout.Label("图标", GUILayout.Width(50f), GUILayout.Height(25f));
        GUILayout.EndHorizontal();

        Color color = GUI.backgroundColor;
        Color hightColor = Color.blue;

        mScroll = GUILayout.BeginScrollView(mScroll);
        for (int i = 0; i < allList.Count; )
        {
            if (current == allList[i])
            {
                GUI.backgroundColor = hightColor;
            }

            if (ShowSkillItem(allList[i]))
                allList.RemoveAt(i);
            else
            {
                ++i;
            }

            GUI.backgroundColor = color;
        }
        GUILayout.EndScrollView();
    }

    bool ShowSkillItem(SkillConfig sk)
    {
        GUILayout.Space(-1f);
        GUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(20f));

        // ID 名字 图标
        GUILayout.Label(sk.id.ToString());
        GUILayout.Label(sk.name);
        if (GUILayout.Button("修改", GUILayout.Width(40f)))
        {
//             if (current != null)
//             {
//                 current.UpdateDataToEditor();
//             }

            current = sk;
            if (current.IsEditValid == false)
            {
                current.UpdateDataToEditor();
            }

            current.IsEditValid = true;
            EditorWindow.GetWindow<SkillEffectConfigEditor>(false, "SkillEffectConfig", true);
        }

        bool bDel = GUILayout.Button("删除", GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        return bDel;
    }
}
