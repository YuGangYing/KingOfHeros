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

		public GameObject GetHeroObject(string resPath){
			GameObject prefab = GetHeroPrefab (resPath);
			#if UNITY_EDITOR
			Renderer[] rrs = prefab.GetComponentsInChildren<Renderer>(true);
			for(int i=0;i<rrs.Length;i++){
				Renderer rr = rrs[i];
				rr.sharedMaterial.shader = Shader.Find(rr.sharedMaterial.shader.name);
			}
			#endif
			GameObject go = Instantiate (prefab) as GameObject;
			return go;
		}

		public GameObject GetHeroPrefab(string resPath){
			string subPath = resPath.Substring (0,resPath.LastIndexOf('/'));
			string prefabName = resPath.Substring (resPath.LastIndexOf('/') + 1);
			string abName = subPath.Substring (subPath.LastIndexOf('/') + 1);
			abName = PathConstant.HERO_AB_FRONT + abName;
			return GetHeroPrefab (abName,prefabName);
		}

		GameObject GetHeroPrefab(string abName,string heroName){
			return AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (abName,heroName);
		}

		public GameObject GetSoliderObject(string resPath){
			GameObject prefab = GetSoliderPrefab (resPath);
			#if UNITY_EDITOR
			Renderer[] rrs = prefab.GetComponentsInChildren<Renderer>(true);
			for(int i=0;i<rrs.Length;i++){
				Renderer rr = rrs[i];
				rr.sharedMaterial.shader = Shader.Find(rr.sharedMaterial.shader.name);
			}
			#endif
			GameObject go = Instantiate (prefab) as GameObject;
			return go;
		}

		public GameObject GetSoliderPrefab(string resPath){
			string subPath = resPath.Substring (0,resPath.LastIndexOf('/'));
			string prefabName = resPath.Substring (resPath.LastIndexOf('/') + 1);
			string abName = subPath.Substring (subPath.LastIndexOf('/') + 1);
			abName = PathConstant.SOLDIER_AB_FRONT + abName;
			return GetHeroPrefab (abName,prefabName);
		}

		GameObject GetSoliderPrefab(string abName,string prefabName){
			return AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (abName,prefabName);
		}

		#endregion

		#region Audios

		public AudioClip GetAudioClipBGM(string bgm){
			AudioClip clip = AssetbundleManager.GetInstance.GetAssetFromLocal<AudioClip> (ABConstant.BGM,bgm);
			return clip;
		}

		public AudioClip GetAudioClipSE(string se){
			AudioClip clip = AssetbundleManager.GetInstance.GetAssetFromLocal<AudioClip> (ABConstant.BGM,se);
			return clip;
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