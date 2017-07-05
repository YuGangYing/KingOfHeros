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

public class CfgSoldierMgrUI : EditorWindow
{
	static CfgSoldierMgrUI s_Instance = null;
	[MenuItem("Tool/Soldier config")]
	static public void init ()
	{
		if(s_Instance!=null)
			return;
		s_Instance = (CfgSoldierMgrUI)EditorWindow.GetWindow(typeof(CfgSoldierMgrUI),false,"士兵配置");  
		s_Instance.Show();
	}

	static public CfgSoldierMgrUI getInstance()
	{
		return s_Instance;
	}

	public void release()
	{
		if(CfgSoldierMgr.getInstance()!=null)
			CfgSoldierMgr.getInstance().release();
		s_Instance = null;
	}

	public void refresh()
	{
		Repaint();
	}

	private int nSelectIndex = 0;
	private bool needSave = false;

	void OnGUI()
	{
		GUILayout.Space(5);
		GUILayout.BeginVertical();
		showSystem();
		DrawSeparator();
		showEdit();
		DrawSeparator();
		showSoldierList();
		DrawSeparator();
		GUILayout.BeginVertical();
	}

	void showSystem()
	{
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("导入",GUILayout.Width(80)))
			CfgSoldierMgr.getInstance().load();
		if(GUILayout.Button("保存",GUILayout.Width(80)))
		{
			if(CfgSoldierMgr.getInstance().save())
				needSave = false;
		}
		if(GUILayout.Button("另存为",GUILayout.Width(80)))
		{
			if(CfgSoldierMgr.getInstance().saveAs())
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
			addSoldier();
		if(GUILayout.Button("删除",GUILayout.Width(80)))
			delSelSoldier();
		if(GUILayout.Button("编辑",GUILayout.Width(80)))
			editSoldier();
		GUILayout.EndHorizontal();
	}

	void showErrMsg(string strMsg)
	{
		this.ShowNotification(new GUIContent(strMsg));
	}

	void showSoldierList()
	{
		showHeader();
		foreach(KeyValuePair<int,CfgSoldier> it in CfgSoldierMgr.getInstance().soldierList)
		{
			showSoldier(it.Value);
		}
	}

	void showHeader()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("", GUILayout.Width(20f));

		for(CfgSoldier.SOLDIER_PROP i = CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_ID;i<CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_UNKOWN;i++)
		{
			CfgSoldier.SOLDIER_COL col = CfgSoldier.getCol(i);
			if(col==null)
				continue;
			GUILayout.Label(col.showName, GUILayout.Width(col.width));
		}

		GUILayout.EndHorizontal();
		DrawSeparator();
	}

	void showSoldier(CfgSoldier soldier)
	{
		if(soldier.getId() == nSelectIndex)
		{
			GUI.backgroundColor = Color.blue;
		}
		GUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(20f));

		bool bSel = GUILayout.Button("",EditorStyles.label,GUILayout.MinWidth(20f));
		for(CfgSoldier.SOLDIER_PROP i = CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_ID;i<CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_UNKOWN;i++)
		{
			CfgSoldier.SOLDIER_COL col = CfgSoldier.getCol(i);
			string temp = soldier.getColStr(i);
			if(temp==null)
				temp = "";
			bSel = bSel || GUILayout.Button(temp,EditorStyles.label,GUILayout.MinWidth(col.width),GUILayout.MaxWidth(col.width));
		}
		if(bSel)
		{
			nSelectIndex = soldier.getId();
			Repaint();
		}
		GUI.backgroundColor = Color.white;
		GUILayout.EndHorizontal();
	}

	void delSelSoldier()
	{
		if(nSelectIndex > 0)
		{
			CfgSoldierMgr.getInstance().delSoldier(nSelectIndex);
			nSelectIndex = 0;
		}
	}

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

	void addSoldier()
	{
		CfgSoldierUI.createWindow();
		needSave = true;
	}

	void editSoldier()
	{
		if(nSelectIndex <=0)
			return;
		CfgSoldier soldier = CfgSoldierMgr.getInstance().getSoldier(nSelectIndex);
		if(soldier==null)
			return;
		CfgSoldierUI.createWindow(soldier);
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