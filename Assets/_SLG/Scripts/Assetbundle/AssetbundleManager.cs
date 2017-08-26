using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KOH
{
	public class AssetbundleManager : SingleMonoBehaviour<AssetbundleManager>
	{

		Dictionary<string,AssetBundle> mCachedAssetbundles;

		protected override void Awake ()
		{
			base.Awake ();
			mCachedAssetbundles = new Dictionary<string, AssetBundle> ();
		}

		public AssetBundle GetAssetbundleFromLocal (string abName)
		{
			abName = abName.ToLower ();
			if(mCachedAssetbundles.ContainsKey(abName))
				return mCachedAssetbundles[abName];
			AssetBundle ab = AssetBundle.LoadFromFile (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + abName + "." + PathConstant.AB_VARIANT);
			mCachedAssetbundles.Add (abName,ab);
			return ab;
		}

		public T GetAssetFromLocal<T>(string abName,string assetName) where T : Object{
			AssetBundle ab = GetAssetbundleFromLocal (abName);
			T t = ab.LoadAsset<T> (assetName);
			return t;
		}

	}
}