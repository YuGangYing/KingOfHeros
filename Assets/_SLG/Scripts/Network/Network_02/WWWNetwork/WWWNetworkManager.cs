using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Network;


namespace WWWNetwork
{
	public class WWWNetworkManager : SingleMonoBehaviour<WWWNetworkManager>
	{
		public Model model;

		public  Dictionary<string,string> cookies;

		public UnityAction<WWW> failCallBack;


		protected override void Awake ()
		{
			base.Awake ();
			model = new Model ();
		}

		public void CommonCallBack (WWW www)
		{
			Debug.Log ("Error: " + www.error);
			Debug.Log ("Text: " + www.text);
		}

		IEnumerator WaitWWW (WWW www, UnityAction<WWW> callBack)
		{
			yield return www;
			if (www.error != null) {
				Debug.LogError (www.error);
				if (failCallBack != null)
					failCallBack (www);
			} else {
				Debug.Log (www.text);
				if (cookies == null)
					cookies = www.ParseCookies ();//获取服务端Session
				JsonUtility.FromJsonOverwrite(www.text,model);
				if (callBack != null)
					callBack (www);
			}
		}

		public void Send (string apiPath, byte[] data, UnityAction<WWW> complete)
		{
			WWW www;
			if(cookies==null)
				www = new WWW (PathConstant.SERVER_PATH + apiPath, data,new Dictionary<string,string>{ { "DeviceID",SystemInfo.deviceUniqueIdentifier} });
			else
				www = new WWW (PathConstant.SERVER_PATH + apiPath, data, UnityCookies.GetCookieRequestHeader(cookies));
			StartCoroutine (WaitWWW (www, complete));
		}
	}
}