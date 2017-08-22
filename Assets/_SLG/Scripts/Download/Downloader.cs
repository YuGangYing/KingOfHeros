using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Downloader : MonoBehaviour {

	public void StartDownload(string assetBundleName,UnityAction onComplete){
		StartCoroutine (_StartDownload(assetBundleName,onComplete));
	}

	private IEnumerator _StartDownload (string assetBundleName,UnityAction onComplete)
	{
		string URL = PathConstant.SERVER_ASSETBUNDLES_PATH + "/" + assetBundleName;
		Debug.Log (URL);
		var www = new WWW (URL);
		yield return www;
		if (www.isDone && string.IsNullOrEmpty (www.error))
		{
			var assetBundle = www.assetBundle;
			FileManager.WriteAllBytes (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + assetBundleName,www.bytes);
		}
		else
		{
			Debug.Log (assetBundleName);
			Debug.Log (www.error);
		}
		www.Dispose();
		www = null;
		if (onComplete != null)
			onComplete ();
	}

}
