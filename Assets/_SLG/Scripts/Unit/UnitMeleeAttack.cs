using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMeleeAttack : MonoBehaviour {

	Transform m_Trans;
	Unit m_Unit;
 
    UnitAttribute m_UnitAbt;
	public Unit AttackTarget; 
	public AnimEventCall[] animEvent;  
 
	public _GlobleScanMode GlobleScanMode = _GlobleScanMode.Line;
	public float m_DefaultScaneRadius;

	public AudioClip hitClip;
	public bool Attackable = true;

	public bool IsCharged;
	public float ChargeRadius = 20;

	public GameObject RushFX;

    void Awake()
    {
        m_Trans = transform;
        m_Unit = GetComponent<Unit>();
        m_UnitAbt = GetComponent<UnitAttribute>();
        if (m_UnitAbt == null)
        {
            Logger.LogError("m_UnitAbt is null");
            return;
        }
        m_DefaultScaneRadius = m_UnitAbt.ScanRadius;

        UnitAnim anim = GetComponent<UnitAnim>();
        if (anim == null)
        {
            Logger.LogError("anim is null");
            return;
        } 

        animEvent = GetComponentsInChildren<AnimEventCall>();
        if (animEvent == null)
        {
            Logger.LogError("animEvent is null");
            return;
        }
        for (int i = 0; i < animEvent.Length; ++i)
        {
            animEvent[i].onAttacking += Hit;
			animEvent[i].onAttacking += PlaySound;
        }
		if(RushFX==null)
		{
			if(transform.Find("RushFX")!=null)RushFX = transform.Find("RushFX").gameObject;
		}
        this.enabled = false;

        //m_UnitAbt.BaseDamage = 1;
        //m_UnitAbt.Damage = m_UnitAbt.BaseDamage +m_UnitAbt.SkillDamage;
        m_UnitAbt.AttackRadius = 3;
        m_UnitAbt.ScanRadius = 20;
    } 

	void Start () {
        StartCoroutine(ScanTargetAllAround());
        StartCoroutine(AttackRoutine());
	}

	public void StartAllRoutions()
	{
		StartCoroutine(ScanTargetAllAround());	
		StartCoroutine(AttackRoutine());
	}

	public List<Unit> DiscardList;
	public void DiscardTarget()
	{
        if (DiscardList != null)
        {
            if (!DiscardList.Contains(AttackTarget)) DiscardList.Add(AttackTarget);
            CleanTarget();
        }  
	}

	public void PlaySound()
	{
		if(m_Unit.UnitTroop == ARMY_TYPE.SHIELD)
		{
			AudioManager.SingleTon().PlayPeltastFightAudio();
		}
		else if(m_Unit.UnitTroop == ARMY_TYPE.PIKEMAN)
		{
			AudioManager.SingleTon().PlaySpearFightAudio();
		}
		else if(m_Unit.UnitTroop == ARMY_TYPE.CAVALRY)
		{
			AudioManager.SingleTon().PlayCavalryFightAudio();
		}
	}

	public void ReleaseDiscard()
	{
		DiscardList.Clear();
	}

	public void CleanTarget()
	{ 
//		Debug.Log("CleanTarget");
		AttackTarget = null;
	}

	public void Hit()
	{
		if(AttackTarget!=null)
		{
            AttackTarget.Attribute.OnDamage(m_Unit);  
			//AttackTarget.Attribute.Hit(m_UnitAbt.Damage);
		}
		if(hitClip!=null)
		{
			AudioManager.PlaySound(hitClip);
		}
	}

	public void StopAttack()
	{
//		Debug.Log("StopAttack");
		Attackable = false;
		CleanTarget();
	}

	public void ResumeAttack()
	{
//		Debug.Log("ResumeAttack");
		Attackable = true;
	}

	void ShowRushFX(bool isShow)
	{
		if(RushFX!=null)
		{
			if(RushFX.activeInHierarchy!=isShow)RushFX.SetActive(isShow);
		}
	}

	//Research the attack target;
	public bool Attacking = false;
	public Collider[] cols ;
	public List<Collider> cols1;
	public float CurrentTargetDist;
	public float tmpTime;
	float lastTime;
//	bool IsCharging;
	IEnumerator ScanTargetAllAround()
	{
		Unit currentTarget=null;
		float loopTime = Random.Range(0.5f,1.0f);
		tmpTime = 0;
		while(true)
		{
			if(Attackable && BattleController.SingleTon().IsBegin)
			{
                if (AttackTarget == null )
				{
					cols1 = new List<Collider>();
					if(m_Unit.GlobleScanMode == _GlobleScanMode.Line)
					{

						if(!IsCharged && Physics.OverlapSphere(m_Trans.position, ChargeRadius, 1 << m_Unit.EnemyLayer).Length > 0)
						{
							m_Unit.Move.SpeedUp();
							IsCharged = true;
//							IsCharging = true;
							ShowRushFX(true);
						}

                        cols = Physics.OverlapSphere(m_Trans.position, m_UnitAbt.AttackRadius, 1 << m_Unit.EnemyLayer);
						if(lastTime!=0)tmpTime += Time.time - lastTime;
						lastTime = Time.time;
						if(tmpTime>10)
						{
							if(cols.Length==0)
							{
								cols = Physics.OverlapSphere(m_Trans.position, m_UnitAbt.ScanRadius, 1 << m_Unit.EnemyLayer);
							}
							if(cols.Length==0)
							{
								cols = Physics.OverlapSphere(m_Trans.position, m_UnitAbt.ScanRadius * 2, 1 << m_Unit.EnemyLayer);
							} 
						}
					}
					else
					{
						cols = Physics.OverlapSphere(m_Trans.position, m_UnitAbt.ScanRadius * 4, 1 << m_Unit.EnemyLayer1);
						if(cols.Length==0)
						{
							cols = Physics.OverlapSphere(m_Trans.position, m_UnitAbt.ScanRadius * 4, 1 << m_Unit.EnemyLayer2);
						}
					}

					if(cols.Length>0)
					{
						currentTarget = GetRandomTarget(cols);
						if(currentTarget!=null && currentTarget.Attribute.HP>0 ){
							AttackTarget=currentTarget;
//							Attacking = true;
							if(!m_Unit.UMatrix.IsFighting)m_Unit.UMatrix.IsFighting = true;
						}
					}
//					else
//					{
//						if(Attacking)
//						{
//                            m_UnitAbt.ScanRadius = m_DefaultScaneRadius * 10;
//						}
//					}
				}
			}
			yield return new WaitForSeconds(loopTime);
		}
	}

	Unit GetRandomTarget(Collider[] cols)
	{
		Collider currentCollider=cols[Random.Range(0, cols.Length)];
		return currentCollider.gameObject.GetComponent<Unit>();;
	}

	Unit GetNearestTarget(Collider[] cols)
	{
		float dist=Mathf.Infinity;
		Collider currentCollider=cols[0];
		foreach(Collider col in cols)
		{
			float currentDist=Vector3.Distance(m_Trans.position, col.transform.position);
            if (DiscardList != null)
            {
                if (currentDist < dist && !DiscardList.Contains(col.GetComponent<Unit>()))
                {
                    currentCollider = col;
                    dist = currentDist;
                }
            } 
		}
		return currentCollider.gameObject.GetComponent<Unit>();
	} 

	public float ChargeDist = 20;
	float dist;
//    AttackSpeedData tmp_attackspeedata = null;
    public float m_fattacktime = 0;
    public float m_fattacktimelength = 0;
    private AttackSpeedData m_AttackSpeedData;
    private float m_fAttackRoutlneTime = 0;
	IEnumerator AttackRoutine()
	{
		float attackDelay = Random.Range(0.05f,0.15f);
        //attackDelay = 0;
		while(true)
		{ 
			if(AttackTarget!=null)
			{
				dist = Vector3.Distance(AttackTarget.transform.position,m_Trans.position);

                m_fAttackRoutlneTime +=  1;
                if (m_fAttackRoutlneTime % 5 == 0)
                {
                    m_Trans.LookAt(AttackTarget.transform);
                }
               
                if (AttackTarget.Attribute.HP <= 0 || !AttackTarget.gameObject.activeInHierarchy)
                {
                    CleanTarget();
					if (m_Unit.UnitStatus != _UnitStatus.Run && AttackTarget && AttackTarget.UnitStatus != _UnitStatus.Repel && m_Unit.UnitStatus!=_UnitStatus.Skill) m_Unit.onMove();
                }
				else if (dist > m_UnitAbt.AttackRadius)
                { 
					if (m_Unit.UnitStatus!=_UnitStatus.Run && AttackTarget && AttackTarget.UnitStatus != _UnitStatus.Repel && m_Unit.UnitStatus != _UnitStatus.Skill){
						m_Unit.Move.CurrentTargetPos = AttackTarget.transform.position;
						m_Unit.onMove();
					}else{
						if (AttackTarget)
							m_Unit.Move.CurrentTargetPos = AttackTarget.transform.position;
					} 
				}
				else
				{
					if(m_Unit.UnitStatus!=_UnitStatus.Attack)
					{
                        
						m_Unit.onAttack();  
						m_Unit.Move.SpeedNormalize();
						ShowRushFX(false);
                        m_AttackSpeedData = m_Unit.Anim.GetAttaclSpeedData(m_UnitAbt.m_StaticData.atkBaseSpeed, m_UnitAbt.m_fAttackSpeedAdd);
                        
					}

                    m_Unit.Anim.AttackSpeedLogic(m_AttackSpeedData, m_AttackSpeedData.m_fClipLength, attackDelay);  
				}
			}
			yield return new WaitForSeconds(attackDelay);
		}
	}   
}
