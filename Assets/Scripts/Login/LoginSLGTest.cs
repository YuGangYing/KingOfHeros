using UnityEngine;
using UnityEngine.UI;  			//UI命名空间
using UnityEngine.EventSystems;	//事件系统命名空间
using System.Collections;
using System.Collections.Generic;
using BattleFramework.Data;

public class LoginSLGTest : MonoBehaviour
{
	public UILabel regName;
	public UILabel regPasswordOne;
	public UILabel regPasswordtwo;

	public UILabel loginName;
	public UILabel loginPassword;

	public UIPanel regPanel;
	public UIPanel loginPanel;


	public UIButton loginButtion;
	public UIButton regButtion;

	public UIButton bakLoginButtion;
	public UIButton backRegButtion;


	public void SetRegDisplay ()
	{
		regPanel.gameObject.SetActive (false);
		loginPanel.gameObject.SetActive (true);

	}

	public void SetLogDisplay ()
	{
		loginPanel.gameObject.SetActive (false);
		regPanel.gameObject.SetActive (true);
		
	}


	public void LoginGame ()
	{
		string name = loginName.text;
		string pwd1 = loginPassword.text;

		if (name == "") {
			Debug.Log ("用户名不为空");
			return;
		}
		if (pwd1 == "") {
			Debug.Log ("密码不为空");
			return;
		}
		ReadWriteDataClass rwd = new ReadWriteDataClass ();
		bool result = rwd.CheckDBAndTables (name, pwd1);

		if (result) {
			Application.LoadLevel (1);
		} else {
			Debug.Log ("登陆失败");
		}
	}


	public void RegGame ()
	{
		int type = 1;
		string name = regName.text;
		string pwd1 = regPasswordOne.text;
		string pwd2 = regPasswordtwo.text;

		if (name == "") {
			Debug.Log ("用户名不为空");
			return;
		}
		if (pwd1 == "") {
			Debug.Log ("密码不为空");
			return;
		}

		if (pwd2 == "") {
			Debug.Log ("密码不为空");
			return;
		}

		if (pwd1 != pwd2) {
			Debug.Log ("密码两次输入不相同");
			return;
		}
		ReadWriteDataClass rwd = new ReadWriteDataClass ();
		bool result = rwd.CheckDBAndTables (type, name, pwd1);

		if (result) {
			Application.LoadLevel (1);
		} else {
			Debug.Log ("注册失败");
		}

	}



	
}
