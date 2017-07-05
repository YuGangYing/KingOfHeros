using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//-
[System.Serializable]
public class BaseCardItem
{
	#region variable define
	
	private int mnTypeId = CConstance.INVALID_ID;
	public int typeId{ get{ return mnTypeId; } set{ mnTypeId = value; } }

    DataMgr.enQualityType m_enQuality = DataMgr.enQualityType.enQT_Copper;
    public DataMgr.enQualityType quality { get { return m_enQuality; } set { m_enQuality = value; } }
	
	// base property
	int mnHP = 100;
	public int hp{ get{ return mnHP; } set{ mnHP = value; } }
	
	int mnMP = 100;
	public int mp{ get{ return mnMP; } set{ mnMP = value; } }
	
	int mnLeadPower = 100;
	public int leadPower{ get{ return mnLeadPower; } set{ mnLeadPower = value; } }
	
	int mnAttackPower = 100;
	public int attackPower{ get{ return mnAttackPower; } set{ mnAttackPower = value; } }
	
	int mnDefensePower = 100;
	public int defensePower{ get{ return mnDefensePower; } set{ mnDefensePower = value; } }
	
	int mnViolencePower = 100;
	public int violencePower{ get{ return mnViolencePower; } set{ mnViolencePower = value; } }
	
	List<int> mlistSkill = new List<int>();
	public List<int> skillTable{ get{ return mlistSkill; } }
	
	int mnDamageType = 1;
	public int damageType{ get{ return mnDamageType; } set{ mnDamageType = value; } }
	
	int mnDamageRange = 1;
	public int damageRange{ get{ return mnDamageRange; } set{ mnDamageRange = value; } }
	
	int mnMoveSpeed = 1;
	public int moveSpeed{ get{ return mnMoveSpeed; } set{ mnMoveSpeed = value; } }
	
	#endregion
	
	public BaseCardItem()
	{
	}
}
