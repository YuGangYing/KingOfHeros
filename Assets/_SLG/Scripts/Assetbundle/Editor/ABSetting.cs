using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;

public class ABSetting : EditorWindow
{

	[MenuItem ("Tools/AB Name Setting")] 
	static void InitAnimation ()
	{
		SetABNames (PathConstant.HERO_AB_PATH,PathConstant.HERO_AB_FRONT);
		SetABNames (PathConstant.SOLDIER_AB_PATH,PathConstant.SOLDIER_AB_FRONT);
		SetABNames (PathConstant.BUILDING_AB_PATH,PathConstant.BUILDING_AB_FRONT);
		Debug.Log ("Animation AB Reset Done!");
	}

	static void SetABNames (string path,string front)
	{
		string[] dirs = FileManager.GetDirectories (Application.dataPath + path, "*", SearchOption.TopDirectoryOnly);
		for (int i = 0; i < dirs.Length; i++) {
			string subDir = dirs[i].Substring (dirs[i].LastIndexOf ("/Assets/") + 1);
			string dirName = dirs[i].Substring(dirs[i].LastIndexOf ("/") + 1); 
			AssetImporter assetImporter = AssetImporter.GetAtPath (subDir);
			assetImporter.SetAssetBundleNameAndVariant (string.Format("{0}{1}",front,dirName),PathConstant.AB_VARIANT);
		}
	}

}
