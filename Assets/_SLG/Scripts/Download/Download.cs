using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KOH
{
	public class Download : MonoBehaviour
	{
		
		public Slider slider;
		public Image img_background;

		void Awake(){
			AssetBundle ab = AssetbundleManager.GetInstance.GetAssetbundleFromLocal (ABConstant.SPRITE_BACKGROUND);
			if (ab != null) {
				if (img_background != null) {
					string[] assetNames = ab.GetAllAssetNames ();
					img_background.sprite = ab.LoadAsset <Sprite> (assetNames[Random.Range(0,assetNames.Length)]);
				}
			}
		}

	}
}
