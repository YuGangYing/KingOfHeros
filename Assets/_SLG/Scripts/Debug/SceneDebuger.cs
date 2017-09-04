using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SceneDebuger : MonoBehaviour {

	public GameObject all;
	public bool IsDebug;
	public static SceneDebuger instance;
	public static SceneDebuger SingleTon()
	{
		return instance;
	}

	void Awake()
	{
		if(instance==null){instance =this;}
	}

	bool isShowHealthBar = false;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if(Input.GetKeyDown(KeyCode.H))
		{
			foreach(Unit unit in BattleController.SingleTon().MyUnits)
			{
				unit.Attribute.OverlayBar.HealthBar.gameObject.SetActive(isShowHealthBar);
				foreach(UISprite sp in unit.Attribute.OverlayBar.SkillBars)
				{
					sp.gameObject.SetActive(isShowHealthBar);
				}
			}
			foreach(Unit unit in BattleController.SingleTon().EnemyUnits)
			{
				unit.Attribute.OverlayBar.HealthBar.gameObject.SetActive(isShowHealthBar);
				foreach(UISprite sp in unit.Attribute.OverlayBar.SkillBars)
				{
					sp.gameObject.SetActive(isShowHealthBar);
				}
			}
			isShowHealthBar = isShowHealthBar == true ? false : true;
		}
		if(Input.GetKeyDown(KeyCode.J))
		{
			foreach(Unit unit in SpawnManager.SingleTon().PlayerHeros)
			{
				unit.Attribute.OverlayBar.PlayTweener("+10",Color.green);
			}
		}
	}

	void ShopAnimator()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Soldier");
		foreach(GameObject obj in objs)
		{
			Animator[] anims = obj.GetComponentsInChildren<Animator>();
			foreach(Animator anim in anims)
			{
				anim.enabled = false;
			}
		}
	}

	void StartAnimator()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Soldier");
		foreach(GameObject obj in objs)
		{
			Animator[] anims = obj.GetComponentsInChildren<Animator>();
			foreach(Animator anim in anims)
			{
				anim.enabled = true;
			}
		}
	}

	void HideAll()
	{
		all.SetActive(false);
	}

	void ReStart()
	{
//		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	public bool IsReady;
	void OnGUI()
	{
		if(BattleController.SingleTon().IsCountDown)
		{
			GUIStyle myStyle = new GUIStyle();
			myStyle.fontSize = 60;
			GUI.Label(new Rect(Screen.width/2,(Screen.height-100)/2.0f,100,90), BattleController.SingleTon().TimeStr,myStyle);
		}
	}
}
