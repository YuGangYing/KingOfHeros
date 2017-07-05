using UnityEngine;
using System.Collections;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162

[ExecuteInEditMode]
[RequireComponent(typeof(Unit))]
public class UnitGizmos : MonoBehaviour {


	UnitAttack m_Attacker;
	UnitMeleeAttack m_MeleeAttacker;
	Unit m_Unit;
	UnitMove m_Move;
    UnitAttribute m_UnitAbt;

	public float LineGizmosCubeSize = 0.5f;

	void Awake()
	{ 
        m_Attacker = GetComponent<UnitAttack>();
        m_MeleeAttacker = GetComponent<UnitMeleeAttack>();
        m_Unit = GetComponent<Unit>();
        m_Move = GetComponent<UnitMove>();
        m_UnitAbt = GetComponent<UnitAttribute>();
	} 
    
	void OnDrawGizmosSelected()
	{
		//Show m_Attacker Info
		if(m_Attacker!=null)
		{
			Gizmos.color = Color.red;
			//Gizmos.DrawWireSphere(transform.position,m_Attacker.ScanRadius);
            Gizmos.DrawWireSphere(transform.position, m_UnitAbt.ScanRadius);
            if(m_Attacker.AttackTarget != null)
			{
				Gizmos.DrawLine(transform.position,m_Attacker.AttackTarget.transform.position);
			}
		}

		//Show m_MeleeAttacker Info
		if(m_MeleeAttacker!=null)
		{
			Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_UnitAbt.ScanRadius);
			Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, m_UnitAbt.AttackRadius);
			if(m_MeleeAttacker.AttackTarget != null)
			{
				Gizmos.DrawLine(transform.position,m_MeleeAttacker.AttackTarget.transform.position);
			}
		}
		//Show Line Info
		if(m_Unit!=null)
		{
			switch(m_Unit.Line)
			{
			case 0: Gizmos.color = Color.red;break;
			case 1: Gizmos.color = Color.green;break;
			case 2: Gizmos.color = Color.blue;break;
			default: break;
			}
			Gizmos.DrawCube(transform.position,Vector3.one * LineGizmosCubeSize);
		}
//		if(m_Move != null )
//		{
//			if(m_Move.m_Nav!=null)
//			{
//				Gizmos.color = Color.yellow;
//				Gizmos.DrawLine(transform.position,m_Move.m_Nav.destination);
//			}
//		}
	}

}
