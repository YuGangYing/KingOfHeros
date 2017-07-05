using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum _ScanPriority{Default,Nearest}//Default mode means to fetch the random one.
public enum _AttackMode{Melee,Archer}
public enum _GlobleScanMode{Line,Free}
[RequireComponent(typeof(Unit))]
public class UnitAttack : MonoBehaviour {

	Transform m_Trans;
	private Unit m_Unit;
    private UnitAttribute m_UnitAtb;

	public _ScanPriority ScanPriority = _ScanPriority.Nearest;
	public _GlobleScanMode GlobleScanMode = _GlobleScanMode.Line;

	public bool ScanRequire = true;
	public Unit AttackTarget;
	public float CurrentTargetDist; 
	public bool Attackable = true;
 
    public AnimEventCall[] animEvent; 
	 
	public string shootObjectResourcePath;
	public int shootOjectNum = 1;//the shoot object shoot frequency;
	public GameObject shootObjectPrefab;

	//skill addition damage
	public float SkillDamage = 30f; 

	void Awake()
	{
        m_Trans = transform;
        m_Unit = GetComponent<Unit>();

        m_UnitAtb = GetComponent<UnitAttribute>();

        if (m_UnitAtb == null)
        {
            Logger.LogError("m_UnitAtb is null");
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
            animEvent[i].onAttacking += _Attack;
			animEvent[i].onAttacking += PlaySound;
        } 

		shootObjectPrefab = SpawnManager.SingleTon ().LoadShootObject (shootObjectResourcePath,shootOjectNum);
		shootOjectNum = Mathf.Max(shootOjectNum,1);

        this.enabled = false;
	} 

	void Start () 
	{
		ShootPoint = m_Unit.Attribute.ShootPoint;
		StartCoroutine(ScanTargetAllAround());	
		StartCoroutine(AttackRoutine());
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void PlaySound()
	{
		if(m_Unit.UnitTroop == ARMY_TYPE.ARCHER)
		{
			AudioManager.SingleTon().PlayArcherFightAudio();
		}
		else if(m_Unit.UnitTroop == ARMY_TYPE.MAGIC)
		{
			AudioManager.SingleTon().PlayMagicFightAudio();
		}
	}

	public void StopAttack()
	{
		Attackable = false;
		CleanTarget();
	}
	
	public void ResumeAttack()
	{
		Attackable = true;
	}

	public void StartAllRoutions()
	{
		StartCoroutine(ScanTargetAllAround());	
		StartCoroutine(AttackRoutine());
	}

	public void CleanTarget()
	{ 
		//m_Unit.Anim.UnTransitionAttack();
		AttackTarget = null;
	}

	//public float AttackFrequncy = 1;
	public GameObject ShootObject;
	public Transform ShootPoint;
	public float ShootDelay = 0;
	public float Cooldown = 0.5f; 
	public void _Attack()
	{
        //Debug.Log("Shoot~~~~~");
		if(ShootPoint==null)
		{
			ShootPoint = transform;
		}
		if(AttackTarget!=null)
		{

			GameObject go = SpawnManager.SingleTon().poolManager.Spawn(shootObjectPrefab,ShootPoint.position,ShootPoint.rotation);
			ShootObject shootObj = go.GetComponent<ShootObject>();
            shootObj.Damage = m_UnitAtb.BaseDamage;
            
			if (AttackTarget.Attribute.HitTargets.Count > 0)
            {
                shootObj.Shoot(AttackTarget.Attribute.HitTargets, AttackTarget, m_Unit);
            }
            else
            {
                Logger.LogError(gameObject.name + "没有被攻击点HitPoint");
            } 
		}
	}

	// Scan the attack target around by a radius
	public bool Attacking = false; 
	public Collider[] cols ;
	public List<Collider> cols1;

	IEnumerator ScanTargetAllAround()
	{
		while(true)
		{
			if(Attackable)
			{
				if(AttackTarget == null && ScanRequire)
				{
					cols1 = new List<Collider>();
					if(m_Unit.GlobleScanMode == _GlobleScanMode.Line)
					{
						cols = Physics.OverlapSphere(m_Trans.position, m_UnitAtb.ScanRadius,1<<m_Unit.EnemyLayer);
						if(BattleController.SingleTon().DuringSinceBattleBegin >= 30 && cols.Length == 0)
						{
							cols = Physics.OverlapSphere(m_Trans.position , m_UnitAtb.ScanRadius * 10 , 1<<m_Unit.EnemyLayer);
						}
						if(cols.Length>0){
							Unit currentTarget=null;
							if(ScanPriority==_ScanPriority.Nearest)
							{
								float dist=Mathf.Infinity;
								Collider currentCollider=cols[0];
								foreach(Collider col in cols)
								{
									if (col != null)
									{
										float currentDist = Vector3.Distance(m_Trans.position, col.transform.position);
										if (currentDist < dist)
										{
											currentCollider = col;
											dist = currentDist;
										}
									} 
								}
								currentTarget=currentCollider.gameObject.GetComponent<Unit>();
								if(currentTarget!=null && currentTarget.Attribute.HP>0 ) 
								{
									AttackTarget=currentTarget;
									if(!m_Unit.UMatrix.IsFighting)m_Unit.UMatrix.IsFighting = true;
								}
							}
							else if(ScanPriority==_ScanPriority.Default)
							{
								Collider currentCollider=cols[Random.Range(0, cols.Length)];
								currentTarget=currentCollider.gameObject.GetComponent<Unit>();
								if(currentTarget!=null && currentTarget.Attribute.HP>0 )
								{
									Attacking = true;
									if(!m_Unit.UMatrix.IsFighting)m_Unit.UMatrix.IsFighting = true;
									ScanPriority=_ScanPriority.Nearest;
									AttackTarget=currentTarget;
								}
							}
						}
					}
					else
					{
						//Global Scan
						cols = Physics.OverlapSphere(m_Trans.position, m_UnitAtb.ScanRadius * 2, 1 << m_Unit.EnemyLayer1);
						if(cols.Length==0)
						{
							cols = Physics.OverlapSphere(m_Trans.position, m_UnitAtb.ScanRadius * 2, 1 << m_Unit.EnemyLayer2);
						}
					}
					if(cols.Length>0){
						Unit currentTarget=null;
						if(ScanPriority==_ScanPriority.Nearest)
						{
							float dist=Mathf.Infinity;
							Collider currentCollider=cols[0];
							foreach(Collider col in cols)
							{
	                            if (col != null)
	                            {
	                                float currentDist = Vector3.Distance(m_Trans.position, col.transform.position);
	                                if (currentDist < dist)
	                                {
										if(!col.GetComponent<Unit>().IsHero)
										{
											currentCollider = col;
											dist = currentDist;
										}
	                                }
	                            } 
							}
							currentTarget=currentCollider.gameObject.GetComponent<Unit>();
							if(currentTarget!=null && currentTarget.Attribute.HP>0 ) 
							{
								AttackTarget=currentTarget;
								if(!m_Unit.UMatrix.IsFighting)m_Unit.UMatrix.IsFighting = true;
							}
						}else if(ScanPriority==_ScanPriority.Default)
						{
							Collider currentCollider=cols[Random.Range(0, cols.Length)];
							currentTarget=currentCollider.gameObject.GetComponent<Unit>();
							if(currentTarget!=null && currentTarget.Attribute.HP>0 )
							{
								Attacking = true;
								if(!m_Unit.UMatrix.IsFighting)m_Unit.UMatrix.IsFighting = true;
								ScanPriority=_ScanPriority.Nearest;
								AttackTarget=currentTarget;
							}
						}
					}
				}
				else
				{
					CurrentTargetDist=Vector3.Distance(m_Trans.position, AttackTarget.transform.position);
					m_Trans.LookAt(AttackTarget.transform);
	                if (CurrentTargetDist > m_UnitAtb.ScanRadius + 5 || AttackTarget.Attribute.HP <= 0 || !AttackTarget.gameObject.activeInHierarchy)
	                {
						CleanTarget();
					}
					if(m_Unit.UnitStatus!=_UnitStatus.Run && m_Unit.UnitStatus!=_UnitStatus.Skill && BattleController.SingleTon().IsBegin && AttackTarget==null)m_Unit.onMove();
				}
			}
			yield return null;
		}
	}
	
	public bool IsAttackByAnimEvent = true;
    private AttackSpeedData m_AttackSpeedData;
	IEnumerator AttackRoutine()
	{
		while(true)
		{
			if(Attackable)
			{
				if(AttackTarget!=null)
				{
					if(Vector3.Distance(m_Trans.position,AttackTarget.transform.position) > m_UnitAtb.AttackRadius && AttackTarget 
					   && AttackTarget.UnitStatus != _UnitStatus.Repel && m_Unit.UnitStatus != _UnitStatus.Skill)
					{
                        //yield return new WaitForSeconds(0.2f); 

						m_Unit.Move.CurrentTargetPos = AttackTarget.transform.position;

						m_Unit.onMove();
					}
					else
					{
                        //yield return new WaitForSeconds(0.2f);

						if (m_Unit.UnitStatus != _UnitStatus.Attack && m_Unit.UnitStatus != _UnitStatus.Skill && AttackTarget.UnitStatus != _UnitStatus.Repel) 
                        {
                           
                            m_Unit.onAttack();

                            m_AttackSpeedData = m_Unit.Anim.GetAttaclSpeedData(m_UnitAtb.m_StaticData.atkBaseSpeed, m_UnitAtb.m_fAttackSpeedAdd);
                        }

						if (m_Unit.UnitStatus != _UnitStatus.Skill)
                        	m_Unit.Anim.AttackSpeedLogic(m_AttackSpeedData, m_AttackSpeedData.m_fClipLength,0);  
	                    //if (!IsAttackByAnimEvent)
	                    //{
	                    //    m_Unit.Anim.m_AnimationList[0].CrossFade("Attack01");
	                    //    yield return new WaitForSeconds(ShootDelay);
	                    //    m_Unit.Anim.m_AnimationList[0].CrossFade("Goblin_Attack");
	                    //    _Attack();
	                    //    yield return new WaitForSeconds(Mathf.Max(0.05f, Cooldown - ShootDelay));
	                    //    m_Unit.Anim.m_AnimationList[0].CrossFade("Attack01");
	                    //}
					}
				}
			}
			yield return null;
		}
	}
	
	public bool isPromoteDamage = false;
	public float priorDamage = 0f;
	public float PromoteDuration = 0f;

	public void OnPromoteDamage(float rate,float duration)	
	{
        if (!isPromoteDamage)
        {
            PromoteDuration = duration;
            priorDamage = m_UnitAtb.BaseDamage;
            m_UnitAtb.BaseDamage = m_UnitAtb.BaseDamage + m_UnitAtb.BaseDamage * rate;
            StartCoroutine(_onPromoteDamageCoroutine());
        }
	}	
			
	IEnumerator _onPromoteDamageCoroutine()
	{
		isPromoteDamage = true;
		while (PromoteDuration > 0)
		{
			PromoteDuration = Mathf.Max(0,PromoteDuration - Time.deltaTime);
			yield return null;
		}
		isPromoteDamage = false;
        m_UnitAtb.BaseDamage = priorDamage;
	}
}
