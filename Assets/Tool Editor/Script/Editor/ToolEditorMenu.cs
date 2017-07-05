using UnityEngine;
using UnityEditor;
using System.Collections;

static public class ToolEditorMenu
{
    [MenuItem("Tool/SkillEffect")]
    static public void OpenSkillEffectEditor()
    {
        EditorWindow.GetWindow<SkillEffectEditor>(false, "SkillEffect", true);
    }

    [MenuItem("Tool/AnimatonType")]
    static public void OpenAnimatonTypeEditor()
    {
        EditorWindow.GetWindow<AnimatonTypeEditor>(false, "新增动画类型", true);
    }

    [MenuItem("Assets/UI Pack", true, 30002)]
    public static bool UIPackValid()
    {
        for (var i = 0; i < Selection.objects.Length; ++i)
        {
            GameObject obj = Selection.objects[i] as GameObject;
            if (obj == null)
                return false;

            string filePath = AssetDatabase.GetAssetPath(obj);
            if (!filePath.Contains("Data/ArtResources/UI"))
                return false;

            if (UnityEditor.PrefabUtility.GetPrefabType(obj) != PrefabType.Prefab)
                return false;
        }

        return Selection.objects.Length == 0 ? false : true;
    }

    [MenuItem("Assets/UI Pack", false, 30002)]
    public static void UIPack()
    {
        // 资源打包
        string path;
        for (int i = 0; i < Selection.objects.Length; ++i)
        {
            path = AssetDatabase.GetAssetPath(Selection.objects[i]);
            Debug.Log("Path:"+path);
            path = path.Replace(".prefab", ".assets");
            Debug.Log("Path:" + path);
            BuildPipeline.BuildAssetBundle(Selection.objects[i], null, path, BuildAssetBundleOptions.CollectDependencies);
        }
    }
}