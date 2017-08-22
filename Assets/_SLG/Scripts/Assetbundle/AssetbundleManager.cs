using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KOH
{
	public class AssetbundleManager : SingleMonoBehaviour<AssetbundleManager>
	{

		protected override void Awake ()
		{
			base.Awake ();
		}

		public AssetBundle GetAssetbundleFromLocal ()
		{
			return null;
		}

		public void GetAssetFromLocal<T>(string abName){
			
		}
	}
}