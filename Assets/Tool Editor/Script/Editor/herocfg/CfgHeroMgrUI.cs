using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
/*
public class WizardCreateLight : ScriptableWizard {	

	public GameObject ObjectToCopy = null;
	public int numberOfCopies = 2;
	public stHero hh ;
	public Font font;
	//public List<stHero> heroList = null ;//new Dictionary<int, stHero>();

	static public void CreateWindow() {
		// Creates the wizard for display
		ScriptableWizard.DisplayWizard("Copy an object.", 
		                               typeof(WizardCreateLight), 
		                               "Copy!");
	}
	void OnWizardUpdate() {
		helpString = "Clones an object a number of times";
		if(!ObjectToCopy) {
			errorString = "Please assign an object";
			isValid = false;
		} else {
			errorString = "";
			isValid = true;	
		}
	}
	void OnWizardCreate () {
		for(int i = 0; i < numberOfCopies; i++)
			Instantiate(ObjectToCopy, Vector3.zero, Quaternion.identity);
	}
	void OnGUI()
	{
	//	GUILayout.Button("打开通知",GUILayout.Width(200));
	}
}*/

public class CfgHeroMgrUI : EditorWindow
{
	static CfgHeroMgrUI s_Instance = null;
	[MenuItem("Tool/Hero config")]
	static public void init ()
	{
		if(s_Instance!=null)
			return;
		s_Instance = (CfgHeroMgrUI)EditorWindow.GetWindow(typeof(CfgHeroMgrUI),false,"英雄配置");  
		s_Instance.Show();
	}

	static public CfgHeroMgrUI getInstance()
	{
		return s_Instance;
	}

	public void release()
	{
		if(CfgHeroMgr.getInstance()!=null)
			CfgHeroMgr.getInstance().release();
		s_Instance = null;
	}

	private bool needSave = false;

	void OnDestroy()
	{
		if(needSave)
		{
			if (EditorUtility.DisplayDialog("是否确定保存修改", "文件被修改，是否确定保存?", "保存", "放弃") == true)
			{
				CfgSoldierMgr.getInstance().save();
			}
		}
		s_Instance = null;
	}

	public void refresh()
	{
		Repaint();
	}

	private int nSelectIndex = 0;
	void OnGUI()
	{
		GUILayout.Space(5);
		GUILayout.BeginVertical();
		showSystem();
		DrawSeparator();
		showEdit();
		DrawSeparator();
		showHeroList();
		DrawSeparator();
		GUILayout.BeginVertical();
	}

	void showSystem()
	{
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("导入",GUILayout.Width(80)))
			CfgHeroMgr.getInstance().load();
		if(GUILayout.Button("保存",GUILayout.Width(80)))
		{
			if(CfgHeroMgr.getInstance().save())
				needSave = false;
		}
		if(GUILayout.Button("另存为",GUILayout.Width(80)))
		{ 
			if(CfgHeroMgr.getInstance().saveAs())
				needSave = false;
		}
		if(GUILayout.Button("退出",GUILayout.Width(80)))
			exit();
		GUILayout.EndHorizontal();
	}

	void showEdit()
	{
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("添加",GUILayout.Width(80)))
			addHero();
		if(GUILayout.Button("删除",GUILayout.Width(80)))
			delSelHero();
		if(GUILayout.Button("编辑",GUILayout.Width(80)))
			editHero();
		GUILayout.EndHorizontal();
	}

	void showErrMsg(string strMsg)
	{
		this.ShowNotification(new GUIContent(strMsg));
	}

	void showHeroList()
	{
		showHeader();
		foreach(KeyValuePair<int,CfgHero> it in CfgHeroMgr.getInstance().heroList)
		{
			showHero(it.Value);
		}
	}

	void showHeader()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("", GUILayout.Width(20f));

		for(CfgHero.HERO_PROP i = CfgHero.HERO_PROP.HERO_PROP_ID;i<CfgHero.HERO_PROP.HERO_PROP_UNKOWN;i++)
		{
			CfgHero.HERO_COL col = CfgHero.getCol(i);
			if(col==null)
				continue;
			GUILayout.Label(col.showName, GUILayout.Width(col.width));
		}

		GUILayout.EndHorizontal();
		DrawSeparator();
	}

	void showHero(CfgHero hero)
	{
		if(hero.getId() == nSelectIndex)
		{
			GUI.backgroundColor = Color.blue;
		}
		GUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(20f));

		bool bSel = GUILayout.Button("",EditorStyles.label,GUILayout.MinWidth(20f));
		for(CfgHero.HERO_PROP i = CfgHero.HERO_PROP.HERO_PROP_ID;i<CfgHero.HERO_PROP.HERO_PROP_UNKOWN;i++)
		{
			CfgHero.HERO_COL col = CfgHero.getCol(i);
			string temp = hero.getColStr(i);
			if(temp==null)
				temp = "";
			bSel = bSel || GUILayout.Button(temp,EditorStyles.label,GUILayout.MinWidth(col.width),GUILayout.MaxWidth(col.width));
		}
		if(bSel)
		{
			nSelectIndex = hero.getId();
			Repaint();
		}
		GUI.backgroundColor = Color.white;
		GUILayout.EndHorizontal();
	}

	void delSelHero()
	{
		if(nSelectIndex > 0)
		{
			CfgHeroMgr.getInstance().delHero(nSelectIndex);
			nSelectIndex = 0;
		}
	}

	void addHero()
	{
		CfgHeroUI.createWindow();
		needSave = true;
	}

	void editHero()
	{
		if(nSelectIndex <=0)
			return;
		CfgHero hero = CfgHeroMgr.getInstance().getHero(nSelectIndex);
		if(hero==null)
			return;
		CfgHeroUI.createWindow(hero);
		needSave = true;
	}

	void exit()
	{
		release();
		Close();
	}

	void DrawSeparator ()
	{
		GUILayout.Space(12f);		
		if (Event.current.type == EventType.Repaint)
		{
			Texture2D tex = EditorGUIUtility.whiteTexture;
			Rect rect = GUILayoutUtility.GetLastRect();
			GUI.color = new Color(0f, 0f, 0f, 0.25f);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
			GUI.color = Color.white;
		}
	}
}