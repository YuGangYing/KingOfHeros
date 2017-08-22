﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CSV;

public class DownloadManager : SingleMonoBehaviour<DownloadManager> {

	public int maxDownloadCount = 5;

	protected override void Awake ()
	{
		base.Awake ();
	}

	void Start(){
		DownloadVersionCSV ();
	}

	List<VersionCSVStructure> mVersions;
	int mDownloadingCount = 0;

	void DownloadVersionCSV(){
		StartCoroutine (_StartDownloadCSV());
	}

	private IEnumerator _StartDownloadCSV ()
	{
		Debug.Log ("_StartDownloadCSV".AliceblueColor());
		string URL = PathConstant.SERVER_VERSION_CSV;
		Debug.Log (URL);
		var www = new WWW (URL);
		yield return www;
		if (www.isDone && string.IsNullOrEmpty (www.error))
		{
			var stream = new MemoryStream (www.bytes);
			var reader = new StreamReader (stream);
			CsvContext mCsvContext = new CsvContext ();
			IEnumerable<VersionCSVStructure> list = mCsvContext.Read<VersionCSVStructure> (reader);
			mVersions = new List<VersionCSVStructure> (list);
			StartCoroutine (_DownloadAssets());
		}
		else
		{
			Debug.Log (www.error);
		}
		www.Dispose();
		www = null;
	}


	IEnumerator _DownloadAssets(){
		Debug.Log ("_DownloadAssets".AliceblueColor());
		while(true){
			if(mVersions.Count==0 && mDownloadingCount == 0){
				Debug.Log ("Download Done!".AliceblueColor());
				yield break;
			}
			if (mDownloadingCount < maxDownloadCount && mVersions.Count>0) {
				GameObject go = new GameObject ("Downloader");
				Downloader downloader = go.AddComponent<Downloader> ();
				mDownloadingCount++;
				downloader.StartDownload (mVersions [0].FileName, () => {
					mDownloadingCount--;
				});
				mVersions.RemoveAt (0);
			}
			yield return null;
		}
	}

}
