using UnityEngine;
using System.Collections;
using System;
using SLG;
using Packet;
using DataMgr;
using Login;
using UI;
using WWWNetwork;

public class LoginController : SingleMonoBehaviour<LoginController>
{

	public string ip = "192.168.1.152";
	public int port = 8100;
	public string iggid = "3100";
	public GameObject loginMain = null;

	LoginMgr _login = null;
	public UIInput m_wndIp = null;
	public UIInput m_wndIgg = null;
	public UIButton m_wndConfirm = null;
	public UILabel m_wndMsg = null;
	public UILabel m_wndInfo = null;

	public Animation _loginAnimation = null;
	string strKey = "loginKey";

	public SigninAPI signin;

	protected override void Awake ()
	{
		if (_loginAnimation != null)
			_loginAnimation.playAutomatically = false;
		signin = gameObject.GetOrAddComponent<SigninAPI> ();
		signin.Send ((WWW www) => {
			
		});
	}

	// Use this for initialization
	void Start ()
	{
		_login = new LoginMgr ();
//		AudioCenter.me.play (AudioMgr.AudioName.LOAD_BGM);
		SoundManager.GetInstance.PlayBGM (SoundConstant.BGM_LOGIN);
		string strTemp = PlayerPrefs.GetString (strKey);
		if (!strTemp.Equals (string.Empty))
			iggid = strTemp;
		if (loginMain != null) {
			m_wndIgg = UISoldierPanel.findChild<UIInput> (loginMain.transform, "InputIgg");
			m_wndIp = UISoldierPanel.findChild<UIInput> (loginMain.transform, "InputIP");

			if (m_wndIgg != null)
				m_wndIgg.value = iggid;
			if (m_wndIp != null)
				m_wndIp.value = ip;

			m_wndConfirm = UISoldierPanel.findChild<UIButton> (loginMain.transform, "confirmBtn");
			if (m_wndConfirm != null) {
//              UIEventListener.Get(m_wndConfirm.gameObject).onClick = login;
				m_wndConfirm.gameObject.AddMissingComponent<UIEventTrigger> ().onClick.Add (new EventDelegate (login));
			}


			m_wndMsg = UISoldierPanel.findChild<UILabel> (loginMain.transform, "msgLabel");
			m_wndInfo = UISoldierPanel.findChild<UILabel> (loginMain.transform, "InfoLabel");
			if (m_wndInfo != null)
				m_wndInfo.gameObject.SetActive (false);
			showMsg ("");
		}
	}

	void login ()
	{
		ip = m_wndIp.value;
		iggid = m_wndIgg.value;
		PlayerPrefs.SetString (strKey, iggid);
		this._login.Iggid = iggid;
//        this._login.login(ip, port, LoginResult);
		StartCoroutine ("ChangeScene");
		if (m_wndInfo != null) {
			m_wndInfo.gameObject.SetActive (true);
			m_wndInfo.text = "Logining...";
		}
		if (m_wndConfirm != null)
			m_wndConfirm.gameObject.SetActive (false);

		if (_loginAnimation != null)
			_loginAnimation.Play ();
	}

	IEnumerator ChangeScene ()
	{
		yield return new WaitForSeconds (3);
		Application.LoadLevelAsync (1);
	}

	void showMsg (string strMsg)
	{
		if (m_wndMsg != null)
			m_wndMsg.text = strMsg;
	}

	void LoginResult (Login.LOGIN_RET ret, string strErrmsg)
	{
		if (ret == Login.LOGIN_RET.SUCC) {
			m_wndInfo.text = "Login SUCC";
			//获取用户信息
			DataMgr.DataManager.getLoginData ().getUserInfo ();
			// 可以加载主城场景了
			MainController.me.showLoading = false;
			SLG.GlobalEventSet.FireEvent (SLG.eEventType.ChangeScene, new SLG.EventArgs (MainController.SCENE_MAINCITY));
		} else {
			// 登陆失败了
			showMsg (strErrmsg);
			Logger.LogDebug ("login fail! {0}", strErrmsg);
			if (m_wndInfo != null) {
				m_wndInfo.gameObject.SetActive (false);
				m_wndInfo.text = "Logining...";
			}
			if (m_wndConfirm != null)
				m_wndConfirm.gameObject.SetActive (true);
		}
	}
}
