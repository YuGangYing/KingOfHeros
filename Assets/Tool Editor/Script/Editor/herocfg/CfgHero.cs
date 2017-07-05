using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CfgHero
{
	public enum HERO_TYPE
	{
		DAMAGE,
		DEFENSE,
		ASSIST,
		OTHER
	}
	public enum HERO_PROP_TYPE
	{
		HERO_PROP_TYPE_STR,
		HERO_PROP_TYPE_INT,
		HERO_PROP_TYPE_FLOAT
	}

	public enum HERO_PROP
	{
		HERO_PROP_ID,
		HERO_PROP_NAME,
		HERO_PROP_PREFABS,
		HERO_PROP_PREFABPATH,
		HERO_PROP_TYPE,
		HERO_PROP_HP,
		HERO_PROP_FIRERANGE,
		HERO_PROP_DAMAGERANGE,
		HERO_PROP_MOVESPEED,
		HERO_PROP_DAMAGE,
		HERO_PROP_DEFENSE,
		HERO_PROP_UNKOWN
	}

	public class HERO_COL
	{
		public string showName;
		public float width;
		public HERO_PROP_TYPE type;
	}

	static private Dictionary<HERO_PROP,HERO_COL> s_colList = new Dictionary<HERO_PROP, HERO_COL>();
	private Dictionary<HERO_PROP,System.Object> m_colList = new Dictionary<HERO_PROP, System.Object>();
	
	public CfgHero()
	{
		m_colList.Clear();
	}

	public string getColStr(HERO_PROP prop)
	{
		System.Object obj;
		if(m_colList.TryGetValue(prop,out obj))
		{
			return obj.ToString();
		}
		return null;
	}

	public int getId()
	{
		return getColInt(HERO_PROP.HERO_PROP_ID);
	}

	public int getColInt(HERO_PROP prop)
	{
		HERO_COL col = getCol(prop);
		if(col!=null)
		{
			if(col.type==HERO_PROP_TYPE.HERO_PROP_TYPE_INT)
			{
				System.Object obj;
				if(m_colList.TryGetValue(prop,out obj))
					return (int)obj;
			}
		}
		return 0;
	}

	public HERO_TYPE getColHeroType(HERO_PROP prop)
	{
		HERO_COL col = getCol(prop);
		if(col!=null)
		{
			if(col.type==HERO_PROP_TYPE.HERO_PROP_TYPE_FLOAT)
			{
				System.Object obj;
				if(m_colList.TryGetValue(prop,out obj))
					return (HERO_TYPE)obj;
			}
		}
		return HERO_TYPE.OTHER;
	}
	
	public float getColFloat(HERO_PROP prop)
	{
		HERO_COL col = getCol(prop);
		if(col!=null)
		{
			if(col.type==HERO_PROP_TYPE.HERO_PROP_TYPE_FLOAT)
			{
				System.Object obj;
				if(m_colList.TryGetValue(prop,out obj))
					return (float)obj;
			}
		}
		return 0f;
	}

	public void addCol(HERO_PROP prop,System.Object value)
	{
		m_colList[prop] = value;
	}

	/**/
	public static HERO_COL getCol(HERO_PROP prop)
	{
		init();
		HERO_COL col; 
		if(s_colList.TryGetValue(prop,out col))
			return col;
		return null;
	}
	
	private static void init()
	{
		if(s_colList.Count==0)
		{
			addCol(HERO_PROP.HERO_PROP_ID,"id",80f,HERO_PROP_TYPE.HERO_PROP_TYPE_INT);
			addCol(HERO_PROP.HERO_PROP_NAME,"name",80f,HERO_PROP_TYPE.HERO_PROP_TYPE_STR);
			addCol(HERO_PROP.HERO_PROP_PREFABS,"prefab",140f,HERO_PROP_TYPE.HERO_PROP_TYPE_STR);
			addCol(HERO_PROP.HERO_PROP_PREFABPATH,"prefabpath",140f,HERO_PROP_TYPE.HERO_PROP_TYPE_STR);
			addCol(HERO_PROP.HERO_PROP_HP,"hp",80f,HERO_PROP_TYPE.HERO_PROP_TYPE_INT);
			addCol(HERO_PROP.HERO_PROP_TYPE,"type",80f,HERO_PROP_TYPE.HERO_PROP_TYPE_INT);
			addCol(HERO_PROP.HERO_PROP_FIRERANGE,"fireRange",80f,HERO_PROP_TYPE.HERO_PROP_TYPE_FLOAT);
			addCol(HERO_PROP.HERO_PROP_DAMAGERANGE,"damageRange",80f,HERO_PROP_TYPE.HERO_PROP_TYPE_FLOAT);
			addCol(HERO_PROP.HERO_PROP_MOVESPEED,"movespeed",80f,HERO_PROP_TYPE.HERO_PROP_TYPE_FLOAT);
			addCol(HERO_PROP.HERO_PROP_DAMAGE,"damage",80f,HERO_PROP_TYPE.HERO_PROP_TYPE_INT);
			addCol(HERO_PROP.HERO_PROP_DEFENSE,"defense",80f,HERO_PROP_TYPE.HERO_PROP_TYPE_INT);
		}
	}

	private static void addCol(HERO_PROP prop,string name,float width,HERO_PROP_TYPE type)
	{
		HERO_COL col = new HERO_COL();
		col.width = width;
		col.showName = name;
		col.type = type;
		s_colList.Add(prop,col);
	}
}
