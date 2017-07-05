using UnityEngine;
using Object = UnityEngine.Object;
using System.Collections;
using System.Collections.Generic;

public class ResourceUnit
{
//	#region static variable
//	
//	private static ResourceUnit msInstance = null;
//	public static ResourceUnit singleton
//	{
//		get
//		{
//			if(msInstance == null)
//				msInstance = new ResourceUnit();
//			return msInstance;
//		}
//		private set{}
//	}
//	
//	#endregion
//
//	#region attribute variable define
//	#endregion

	public UISprite loadPrefab(string strPrefabPath, string strCurSpriteName = "")
	{
		UIAtlas tu = Resources.Load(strPrefabPath, typeof(UIAtlas)) as UIAtlas;
		//GameObject tu = Resources.Load(strPrefabPath, typeof(GameObject)) as GameObject;
		if(tu == null)
		{
			Debug.Log("load prefab failure" + "  path[" + strPrefabPath + "]");
			return null;
		}

		UISprite icon = new UISprite();

		icon.atlas = tu;
		icon.spriteName = strCurSpriteName;
		//sprite.MakePixelPerfect();    //这里记得要make一下，不然_Player的大小是不会变化的，看你个人需要

		return icon;
	}

	public static T loadResouce<T>(string strPrefabPath) where T : Object
	{
		return (T)Resources.Load(strPrefabPath, typeof(T)) as T;
	}



}
