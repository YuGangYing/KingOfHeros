using UnityEngine;
using UnityEditor;
using System.Collections;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162

public class CfgHeroUI :ScriptableWizard
{
	public int id = 0;
	public string name = "";
	public int hp = 0;
	public GameObject prefab = null;
	public string prefabName = "";
	public string prefabPath = "";
	public CfgHero.HERO_TYPE type = CfgHero.HERO_TYPE.OTHER;
	public float fireRange = 2f;
	public float damageRange = 2f;
	public float moveSpeed;
	public int damage;
	public int defense;

	private GameObject gameObj = null;
	private CfgHero srcHero = null;
	private static CfgHeroUI s_Instance = null;

	static public CfgHeroUI createWindow(CfgHero hero = null)
	{
		if(s_Instance!=null)
			return s_Instance;
		if(hero==null)
		{
			s_Instance = ScriptableWizard.DisplayWizard<CfgHeroUI>("新建英雄","创建","测试动作");
		}
		else
		{
			s_Instance = ScriptableWizard.DisplayWizard<CfgHeroUI>("编辑英雄","修改","测试动作");
			s_Instance.init(hero);
		}
		return s_Instance;
	}

	void init(CfgHero hero)
	{
		if(hero==null)
			return;
		srcHero = hero;
		id = hero.getColInt(CfgHero.HERO_PROP.HERO_PROP_ID);
		name = hero.getColStr(CfgHero.HERO_PROP.HERO_PROP_NAME);
		prefabName = hero.getColStr(CfgHero.HERO_PROP.HERO_PROP_PREFABS);
		prefabPath = hero.getColStr(CfgHero.HERO_PROP.HERO_PROP_PREFABPATH);
		hp = hero.getColInt(CfgHero.HERO_PROP.HERO_PROP_HP);
		type = hero.getColHeroType(CfgHero.HERO_PROP.HERO_PROP_TYPE);
		fireRange = hero.getColFloat(CfgHero.HERO_PROP.HERO_PROP_FIRERANGE);
		damageRange = hero.getColFloat(CfgHero.HERO_PROP.HERO_PROP_DAMAGERANGE);
		moveSpeed = hero.getColFloat(CfgHero.HERO_PROP.HERO_PROP_MOVESPEED);
		damage = hero.getColInt(CfgHero.HERO_PROP.HERO_PROP_DAMAGE);
		defense = hero.getColInt(CfgHero.HERO_PROP.HERO_PROP_DEFENSE);

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
		if(srcHero==null ||
		   (srcHero!=null && srcHero.getId()!=id))
			idOK = CfgHeroMgr.getInstance().checkId(id);
		if(!idOK)
			errorString = "ID已经存在或异常,请重新设置!";
		else
			errorString = "";
	}

	void OnDestroy()
	{
		s_Instance = null;
	}

	void OnWizardUpdate ()
	{
		helpString = "设置英雄参数";
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
			ToolDrawGizmos.me.DrawSphere(gameObj.transform.position,damageRange);
			ToolDrawGizmos.me.End();
		}
	}
	
	void OnWizardCreate ()
	{	
		CfgHero hero = getHero();
		if(srcHero==null)//新建
			CfgHeroMgr.getInstance().addHero(hero);
		else
		{
			if(srcHero.getId()==hero.getId())
				CfgHeroMgr.getInstance().updateHero(hero);
			else
			{
				CfgHeroMgr.getInstance().delHero(srcHero.getId());
				CfgHeroMgr.getInstance().addHero(hero);
			}
		}
		CfgHeroMgrUI.getInstance().refresh();
	}
	
	CfgHero getHero()
	{
		CfgHero hero = new CfgHero();
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_ID,id);
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_NAME,name);
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_PREFABS,prefabName);
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_PREFABPATH,prefabPath);
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_HP,hp);
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_TYPE,type);
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_FIRERANGE,fireRange);
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_DAMAGERANGE,damageRange);
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_MOVESPEED,moveSpeed);
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_DAMAGE,damage);
		hero.addCol(CfgHero.HERO_PROP.HERO_PROP_DEFENSE,defense);
		return hero;
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
