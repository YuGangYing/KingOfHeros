using UnityEngine;
using UnityEditor;
using System.Collections;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162

public class CfgSoldierUI :ScriptableWizard
{
	public int id = 0;
	public string name = "";
	public int hp = 0;
	public GameObject prefab = null;
	public string prefabName = "";
	public string prefabPath = "";
	public CfgSoldier.SOLDIER_TYPE type = CfgSoldier.SOLDIER_TYPE.OTHER;
	public float fireRange = 2f;
	public float damageRange = 2f;
	public float moveSpeed;
	public int damage;
	public int defense;

	private GameObject gameObj = null;
	private CfgSoldier srcSoldier = null;
	private static CfgSoldierUI s_Instance = null;

	static public CfgSoldierUI createWindow(CfgSoldier soldier = null)
	{
		if(s_Instance!=null)
			return s_Instance;
		if(soldier==null)
		{
			s_Instance = ScriptableWizard.DisplayWizard<CfgSoldierUI>("新建士兵","创建","测试动作");
		}
		else
		{
			s_Instance = ScriptableWizard.DisplayWizard<CfgSoldierUI>("编辑士兵","修改","测试动作");
			s_Instance.init(soldier);
		}
		return s_Instance;
	}

	void init(CfgSoldier soldier)
	{
		if(soldier==null)
			return;
		srcSoldier = soldier;
		id = soldier.getColInt(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_ID);
		name = soldier.getColStr(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_NAME);
		prefabName = soldier.getColStr(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_PREFABS);
		prefabPath = soldier.getColStr(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_PREFABPATH);
		hp = soldier.getColInt(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_HP);
		type = soldier.getColSoldierType(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_TYPE);
		fireRange = soldier.getColFloat(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_FIRERANGE);
		damageRange = soldier.getColFloat(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_DAMAGERANGE);
		moveSpeed = soldier.getColFloat(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_MOVESPEED);
		damage = soldier.getColInt(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_DAMAGE);
		defense = soldier.getColInt(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_DEFENSE);

		if(prefabPath!=null)
		{
			prefab = ToolUtil.LoadAssetPath<GameObject>(prefabPath);
			if(prefab!=null)
				gameObj = Instantiate(prefab) as GameObject;
		}
		showWarn();
	}

	void showWarn()
	{
		bool idOK = true;
		if(srcSoldier==null ||
		   (srcSoldier!=null && srcSoldier.getId()!=id))
			idOK = CfgSoldierMgr.getInstance().checkId(id);
		if(!idOK)
			errorString = "ID已经存在或异常,请重新设置!";
		else
			errorString = "";
	}

	void OnWizardUpdate ()
	{
		helpString = "设置士兵参数";
		if(prefab!=null)
		{
			prefabName = prefab.name;
			prefabPath = ToolUtil.GetAssetPath(prefab);
			if(gameObj!=null)
				GameObject.DestroyImmediate(gameObj);
			gameObj = Instantiate(prefab) as GameObject;
		}
		showWarn();
	}

	void Update()
	{
		if(gameObj!=null)
		{
			ToolDrawGizmos.me.Begin();
			ToolDrawGizmos.me.DrawCircle(gameObj.transform.position,damageRange);
			ToolDrawGizmos.me.End();
		}
	}

	void OnDestroy()
	{
		s_Instance = null;
	}

	void OnWizardCreate ()
	{	
		CfgSoldier soldier = getSoldier();
		if(srcSoldier==null)//新建
			CfgSoldierMgr.getInstance().addSoldier(soldier);
		else
		{
			if(srcSoldier.getId()==soldier.getId())
				CfgSoldierMgr.getInstance().updateSoldier(soldier);
			else
			{
				CfgSoldierMgr.getInstance().delSoldier(srcSoldier.getId());
				CfgSoldierMgr.getInstance().addSoldier(soldier);
			}
		}
		CfgSoldierMgrUI.getInstance().refresh();
	}
	
	CfgSoldier getSoldier()
	{
		CfgSoldier soldier = new CfgSoldier();
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_ID,id);
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_NAME,name);
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_PREFABS,prefabName);
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_PREFABPATH,prefabPath);
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_HP,hp);
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_TYPE,type);
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_FIRERANGE,fireRange);
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_DAMAGERANGE,damageRange);
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_MOVESPEED,moveSpeed);
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_DAMAGE,damage);
		soldier.addCol(CfgSoldier.SOLDIER_PROP.SOLDIER_PROP_DEFENSE,defense);
		return soldier;
	}

	void OnWizardOtherButton ()
	{
		if(gameObj==null)
		{
			if(prefab!=null)
				gameObj = Instantiate(prefab) as GameObject;
		}
		if(gameObj!=null)
			AnimatorTest.create(gameObj);
	}
}
