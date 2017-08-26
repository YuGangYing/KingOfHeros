using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KOH
{
	public class ResourcesManager : SingleMonoBehaviour<ResourcesManager>
	{


		protected override void Awake ()
		{
			base.Awake ();
		}

		#region Units(hero and solider)

		public GameObject GetHeroObject (string resPath)
		{
			GameObject prefab = GetHeroPrefab (resPath);
			#if UNITY_EDITOR
			Renderer[] rrs = prefab.GetComponentsInChildren<Renderer> (true);
			for (int i = 0; i < rrs.Length; i++) {
				Renderer rr = rrs [i];
				rr.sharedMaterial.shader = Shader.Find (rr.sharedMaterial.shader.name);
			}
			#endif
			GameObject go = Instantiate (prefab) as GameObject;
			return go;
		}

		public GameObject GetHeroPrefab (string resPath)
		{
			string subPath = resPath.Substring (0, resPath.LastIndexOf ('/'));
			string prefabName = resPath.Substring (resPath.LastIndexOf ('/') + 1);
			string abName = subPath.Substring (subPath.LastIndexOf ('/') + 1);
			abName = PathConstant.HERO_AB_FRONT + abName;
			return AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (abName, prefabName);
		}

		public GameObject GetSoliderObject (string resPath)
		{
			GameObject prefab = GetSoliderPrefab (resPath);
			#if UNITY_EDITOR
			Renderer[] rrs = prefab.GetComponentsInChildren<Renderer> (true);
			for (int i = 0; i < rrs.Length; i++) {
				Renderer rr = rrs [i];
				rr.sharedMaterial.shader = Shader.Find (rr.sharedMaterial.shader.name);
			}
			#endif
			GameObject go = Instantiate (prefab) as GameObject;
			return go;
		}

		public GameObject GetSoliderPrefab (string resPath)
		{
			string subPath = resPath.Substring (0, resPath.LastIndexOf ('/'));
			string prefabName = resPath.Substring (resPath.LastIndexOf ('/') + 1);
			string abName = subPath.Substring (subPath.LastIndexOf ('/') + 1);
			abName = PathConstant.SOLDIER_AB_FRONT + abName;
			return AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (abName, prefabName);
		}

		#endregion

		#region Buildings

		public GameObject GetCityTerrain ()
		{
			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.TERRAIN_CITY, ABConstant.TERRAIN_CITY);
			GameObject go = Instantiate (prefab) as GameObject;
			#if UNITY_EDITOR
			Renderer[] rrs = go.GetComponentsInChildren<Renderer> (true);
			for (int i = 0; i < rrs.Length; i++) {
				Renderer rr = rrs [i];
				rr.sharedMaterial.shader = Shader.Find (rr.sharedMaterial.shader.name);
			}
			#endif
			return go;
		}

		public GameObject GetBattleTerrain ()
		{
			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.TERRAIN_GOBI, ABConstant.TERRAIN_GOBI);
			GameObject go = Instantiate (prefab) as GameObject;
			#if UNITY_EDITOR
			Renderer[] rrs = go.GetComponentsInChildren<Renderer> (true);
			for (int i = 0; i < rrs.Length; i++) {
				Renderer rr = rrs [i];
				rr.sharedMaterial.shader = Shader.Find (rr.sharedMaterial.shader.name);
			}
			#endif
			return go;
		}


		public GameObject GetBuildingObejct (string buildingName)
		{
			string abName = PathConstant.BUILDING_AB_FRONT + buildingName;
			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (abName, buildingName);
			GameObject go = Instantiate (prefab) as GameObject;
			return go;
		}

		#endregion

		#region Audios

		public AudioClip GetAudioClipBGM (string bgm)
		{
			AudioClip clip = AssetbundleManager.GetInstance.GetAssetFromLocal<AudioClip> (ABConstant.SOUND_BGM, bgm);
			return clip;
		}

		public AudioClip GetAudioClipSE (string se)
		{
			AudioClip clip = AssetbundleManager.GetInstance.GetAssetFromLocal<AudioClip> (ABConstant.SOUND_BGM, se);
			return clip;
		}

		#endregion

		#region UI

		public GameObject GetUIInterface (string prefabName)
		{
			GameObject go = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.PREFAB_INTERFACE, prefabName);
			return go;
		}

		#endregion

		#region Battles

		public GameObject GetBattleRoot ()
		{
			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.BATTLE, "BattleRoot");
			GameObject go = Instantiate (prefab) as GameObject;
			return go;
		}

		public GameObject GetBattleUIRoot ()
		{
			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.BATTLE, "BattleUIRoot");
			GameObject go = Instantiate (prefab) as GameObject;
			return go;
		}

		public GameObject GetBattleGlobalSkillEffect ()
		{
			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.BATTLE, "GlobalSkillEffect");
			GameObject go = Instantiate (prefab) as GameObject;
			return go;
		}

		#endregion

		public Sprite GetSprite (string path)
		{
			return null;
		}

		//get character head icon by character id
		public Sprite GetCharacterIconById (int charaId)
		{
			return null;
		}

		public GameObject GetCharacterPrefab (int charaId, int sortLayer = 1)
		{
			return null;
		}

		public byte[] GetCSV (string csvName)
		{
			return null;
		}
	}
}

public enum SoundSE
{
	SE01,
	SE02,
	SE03,
	SE04,
	SE05,
	SE06,
	SE07,
	SE08,
	SE09,
	SE10,
	SE11,
	SE12,
	SE13,
	SE14,
	SE15
}

public enum SoundBGM
{
	BGM01,
	BGM02,
	BGM03
}