using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CfgSoldier
{
	public enum SOLDIER_TYPE
	{
		TYPE1,
		TYPE2,
		TYPE3,
		TYPE4,
		TYPE5,
		OTHER
	}
	public enum SOLDIER_PROP_TYPE
	{
		SOLDIER_PROP_TYPE_STR,
		SOLDIER_PROP_TYPE_INT,
		SOLDIER_PROP_TYPE_FLOAT
	}

	public enum SOLDIER_PROP
	{
		SOLDIER_PROP_ID,
		SOLDIER_PROP_NAME,
		SOLDIER_PROP_PREFABS,
		SOLDIER_PROP_PREFABPATH,
		SOLDIER_PROP_TYPE,
		SOLDIER_PROP_HP,
		SOLDIER_PROP_FIRERANGE,
		SOLDIER_PROP_DAMAGERANGE,
		SOLDIER_PROP_MOVESPEED,
		SOLDIER_PROP_DAMAGE,
		SOLDIER_PROP_DEFENSE,
		SOLDIER_PROP_UNKOWN
	}

	public class SOLDIER_COL
	{
		public string showName;
		public float width;
		public SOLDIER_PROP_TYPE type;
	}

	static private Dictionary<SOLDIER_PROP,SOLDIER_COL> s_colList = new Dictionary<SOLDIER_PROP, SOLDIER_COL>();
	private Dictionary<SOLDIER_PROP,System.Object> m_colList = new Dictionary<SOLDIER_PROP, System.Object>();
	
	public CfgSoldier()
	{
		m_colList.Clear();
	}

	public string getColStr(SOLDIER_PROP prop)
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
		return getColInt(SOLDIER_PROP.SOLDIER_PROP_ID);
	}

	public int getColInt(SOLDIER_PROP prop)
	{
		SOLDIER_COL col = getCol(prop);
		if(col!=null)
		{
			if(col.type==SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_INT)
			{
				System.Object obj;
				if(m_colList.TryGetValue(prop,out obj))
					return (int)obj;
			}
		}
		return 0;
	}

	public SOLDIER_TYPE getColSoldierType(SOLDIER_PROP prop)
	{
		SOLDIER_COL col = getCol(prop);
		if(col!=null)
		{
			if(col.type==SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_FLOAT)
			{
				System.Object obj;
				if(m_colList.TryGetValue(prop,out obj))
					return (SOLDIER_TYPE)obj;
			}
		}
		return SOLDIER_TYPE.OTHER;
	}
	
	public float getColFloat(SOLDIER_PROP prop)
	{
		SOLDIER_COL col = getCol(prop);
		if(col!=null)
		{
			if(col.type==SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_FLOAT)
			{
				System.Object obj;
				if(m_colList.TryGetValue(prop,out obj))
					return (float)obj;
			}
		}
		return 0f;
	}

	public void addCol(SOLDIER_PROP prop,System.Object value)
	{
		m_colList[prop] = value;
	}

	/**/
	public static SOLDIER_COL getCol(SOLDIER_PROP prop)
	{
		init();
		SOLDIER_COL col; 
		if(s_colList.TryGetValue(prop,out col))
			return col;
		return null;
	}
	
	private static void init()
	{
		if(s_colList.Count==0)
		{
			addCol(SOLDIER_PROP.SOLDIER_PROP_ID,"id",80f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_INT);
			addCol(SOLDIER_PROP.SOLDIER_PROP_NAME,"name",80f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_STR);
			addCol(SOLDIER_PROP.SOLDIER_PROP_PREFABS,"prefab",140f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_STR);
			addCol(SOLDIER_PROP.SOLDIER_PROP_PREFABPATH,"prefabpath",140f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_STR);
			addCol(SOLDIER_PROP.SOLDIER_PROP_HP,"hp",80f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_INT);
			addCol(SOLDIER_PROP.SOLDIER_PROP_TYPE,"type",80f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_INT);
			addCol(SOLDIER_PROP.SOLDIER_PROP_FIRERANGE,"fireRange",80f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_FLOAT);
			addCol(SOLDIER_PROP.SOLDIER_PROP_DAMAGERANGE,"damageRange",80f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_FLOAT);
			addCol(SOLDIER_PROP.SOLDIER_PROP_MOVESPEED,"movespeed",80f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_FLOAT);
			addCol(SOLDIER_PROP.SOLDIER_PROP_DAMAGE,"damage",80f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_INT);
			addCol(SOLDIER_PROP.SOLDIER_PROP_DEFENSE,"defense",80f,SOLDIER_PROP_TYPE.SOLDIER_PROP_TYPE_INT);
		}
	}

	private static void addCol(SOLDIER_PROP prop,string name,float width,SOLDIER_PROP_TYPE type)
	{
		SOLDIER_COL col = new SOLDIER_COL();
		col.width = width;
		col.showName = name;
		col.type = type;
		s_colList.Add(prop,col);
	}
}
