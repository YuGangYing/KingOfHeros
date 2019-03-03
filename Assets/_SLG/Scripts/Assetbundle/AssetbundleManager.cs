﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KOH
{
	public class AssetbundleManager : SingleMonoBehaviour<AssetbundleManager>
	{

		Dictionary<string,AssetBundle> mCachedAssetbundles;
		AssetBundleManifest mManifest;
		AssetBundle mManifestAB;

		protected override void Awake ()
		{
			base.Awake ();
			mCachedAssetbundles = new Dictionary<string, AssetBundle> ();
		}

		void LoadManifestAssetbundle(){
			string path = PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + SystemConstant.GetPlatformName();
			if (FileManager.Exists (path)) {
				mManifestAB = AssetBundle.LoadFromFile (path);
				if (mManifestAB != null) {
					mManifest = mManifestAB.LoadAsset<AssetBundleManifest> ("assetbundlemanifest");
				}
			}
		}

		public AssetBundle GetAssetbundleFromLocal (string abName)
		{
			abName = (abName + "." + PathConstant.AB_VARIANT).ToLower ();
			if(mCachedAssetbundles.ContainsKey(abName))
				return mCachedAssetbundles[abName];

			if (mManifestAB == null)
				LoadManifestAssetbundle ();
			if (mManifest != null) {
				string[] dependABs = mManifest.GetAllDependencies (abName);
				for (int i = 0; i < dependABs.Length; i++) {
					if (!mCachedAssetbundles.ContainsKey (dependABs [i])) {
						AssetBundle ab = AssetBundle.LoadFromFile (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + dependABs [i]);
						mCachedAssetbundles.Add (dependABs [i], ab);
					}
				}
			}
			Debug.Log (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + abName);
			AssetBundle ab0 = AssetBundle.LoadFromFile (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + abName);
			mCachedAssetbundles.Add (abName,ab0);
			return ab0;
		}

		public T GetAssetFromLocal<T>(string abName,string assetName) where T : Object{
			AssetBundle ab = GetAssetbundleFromLocal (abName);
			if (ab == null)
				return null;
			T t = ab.LoadAsset<T> (assetName);
			return t;
		}

	}
}