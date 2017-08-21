using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataMgr;

public enum _UnitStatus{Idle,Attack,Run,Over,Arrive,Dead,Skill,Repel,RepelStandUp}
public enum _UnitAlignment{Player,Enemy}

//[RequireComponent(typeof(Collider))]
public class Unit : MonoBehaviour {

	GameObject m_ThisObj;
	Transform m_Trans;

	public delegate void OnArrive();
	public OnArrive onArrive;

	public delegate void OnBeforeDead();
	public OnBeforeDead onBeforeDead;

	public delegate void OnAfterDead(Unit unit);
	public OnAfterDead onAfterDead;

	public delegate void OnDead();
	public OnDead onDead;

	public delegate void OnFly();
	public OnFly onFly;

	public delegate void OnSkill();
	public OnSkill onSkill;
	
	public delegate void OnOver();
	public OnOver onOver;

	public delegate void OnChangeUnitStatus();
	public OnChangeUnitStatus onChangeUnitStatus;

	public _UnitAlignment Alignment = _UnitAlignment.Player;
	public ARMY_TYPE UnitTroop;
	public _GlobleScanMode GlobleScanMode = _GlobleScanMode.Line;

	public UnitAttack Attacker;
	public UnitMeleeAttack MeleeAttacker;
	public UnitAttribute Attribute;
	public UnitAnim Anim;
	public UnitMove Move; 
    public UnitSkill Skill;
	public UnitMatrix UMatrix;
	public UnitMaterial UnitMat;
	public UnitRepelEffect repel;

	public int Line = 0;
	public int Matrix = 0;

	public int EnemyLayer;
	public int EnemyLayer1;
	public int EnemyLayer2;

	public List<Unit> Enemys;
	public List<List<Unit>> EnemyUnitByLines;

	public bool IsReGrouped = false; 

	public _UnitStatus UnitStatus = _UnitStatus.Idle;

	public bool IsHero;
	public int HeroId; 
	public int HeroTypeId;
	public int SkillAnimIndex = -1;
	
	public List<Vector3> WayPoints;

	void Awake () 
	{  
		Init ();
		StartCoroutine(_Disperse());
	}  

	public void Init(){
		m_Trans = transform;
		Attacker = GetComponent<UnitAttack>();
		Attribute = GetComponent<UnitAttribute>();
		Anim = GetComponent<UnitAnim>();
		Move = GetComponent<UnitMove>();
		Skill = GetComponent<UnitSkill>();
		MeleeAttacker = GetComponent<UnitMeleeAttack>();
		UnitMat = GetComponent<UnitMaterial>();

		InitOverEvent();
		InitArriveEvent();
		InitDeadEvent();
		InitSkillEvent();
	}
 	
    public UnitAnim CptUnitAnim
    {
        get
        {
            if (Anim == null)
            {
                return null;
            }
            return Anim;
        }
        set
        {
            Anim = value;
        }  
    } 
	 
    void InitDeadEvent()
    {
        if (Anim != null) onDead += Anim.TransitionDie;
		onDead += RemoveHeroList;
		onDead += RemoveItSelfFromController;
        onDead += DisableScripts;
        onDead += DeadStatus;
        onDead += HideOrDestroy;
		onDead += DeadEffect;
    }

	void DeadEffect()
	{
		if(Attribute.LastAttacker != null && Attribute.LastAttacker.IsHero && !IsHero)
		{
//			Attribute.LastAttacker.m_Trans.position += Attribute.LastAttacker.m_Trans.position.normalized * 10;

			ToggleColor();

		}
	}
	//0:Fire and lamster 1:Magic 2:Posion 3:Fhysics and fly
	void ToggleColor()
	{
//		int index = Random.Range(0,4);
		switch(Attribute.LastAttacker.Attribute.Nature)
		{
			case 0 : 
				_ToggleColor(BattleController.SingleTon().DeathMatFire);
				StartCoroutine(_Lamster());
				break;
			case 1 : _ToggleColor(BattleController.SingleTon().DeathMatMagic);break;
			case 2 : _ToggleColor(BattleController.SingleTon().DeathMatPosion);break;
			case 3 : 
				Vector3 flyPos = m_Trans.position + (m_Trans.position - Attribute.LastAttacker.m_Trans.position).normalized * Random.Range(5,BattleController.SingleTon().FlyDistance);
				StartCoroutine(_Fly(flyPos));
				break;
			default : break;
		}
//		Vector3 flyPos = m_Trans.position + (m_Trans.position - Attribute.LastAttacker.m_Trans.position).normalized * Random.Range(5,BattleController.SingleTon().FlyDistance);
//		StartCoroutine(_Fly(flyPos));
//		StartCoroutine(_Lamster());
	}

	void _ToggleColor(Material mat)
	{
		Renderer[] rends = GetComponentsInChildren<Renderer>();
		foreach(Renderer rend in rends)
		{
			if(rend.gameObject.name == "Effect_Character_Shadow" )
				continue;
			if(rend.gameObject.name == "RushFX" )
				continue;
			rend.material = mat;
		}
	}

	
	public List<Vector3> LamsterPosList;
	public float LamsterSpeed = 5;
	IEnumerator _Lamster()
	{
		LamsterPosList = new List<Vector3>();
		Vector3 direction = (m_Trans.position - Attribute.LastAttacker.m_Trans.position).normalized;
		Vector3 pos0 = m_Trans.position + direction * 8;
		if(pos0.z > 16.6f || pos0.z < -19)
		{
			pos0 = new Vector3(pos0.x,pos0.y,Mathf.Clamp(pos0.z,-19,16.6f));
		}
		Vector3 pos1 = pos0 + new Vector3(Random.Range(-10,10),0,Random.Range(-10,10)).normalized;
		if(pos1.z > 16.6f || pos1.z < -19)
		{
			pos1 = new Vector3(pos1.x,pos1.y,Mathf.Clamp(pos1.z,-19,16.6f));
//			pos1 = pos0 + new Vector3(m_Trans.position.x + Random.Range(-10,10),m_Trans.position.y,m_Trans.position.z).normalized * 3;
		}
		Vector3 pos2 = pos0 + new Vector3(Random.Range(-10,10),0,Random.Range(-10,10)).normalized;
		if(pos2.z > 16.6f || pos2.z < -19)
		{
			pos2 = new Vector3(pos2.x,pos2.y,Mathf.Clamp(pos2.z,-19,16.6f));
//			pos2 = pos0 + new Vector3(m_Trans.position.x + Random.Range(-10,10),m_Trans.position.y,m_Trans.position.z).normalized * 3;
		}
		Vector3 pos3 = pos0 + new Vector3(Random.Range(-10,10),0,Random.Range(-10,10)).normalized;
		if(pos3.z > 16.6f || pos3.z < -19)
		{
			pos3 = new Vector3(pos3.x,pos3.y,Mathf.Clamp(pos3.z,-19,16.6f));
//			pos3 = pos0 + new Vector3(m_Trans.position.x + Random.Range(-10,10),m_Trans.position.y,m_Trans.position.z).normalized * 3;
		}
		Vector3 pos4 = pos0 + new Vector3(Random.Range(-10,10),0,Random.Range(-10,10)).normalized;
		if(pos4.z > 16.6f || pos4.z < -19)
		{
			pos4 = new Vector3(pos4.x,pos4.y,Mathf.Clamp(pos4.z,-19,16.6f));
//			pos4 = pos0 + new Vector3(m_Trans.position.x + Random.Range(-10,10),m_Trans.position.y,m_Trans.position.z).normalized * 3;
		}
		Vector3 pos5 = pos0 + new Vector3(Random.Range(-10,10),0,Random.Range(-10,10)).normalized;
		if(pos5.z > 16.6f || pos5.z < -19)
		{
			pos5 = new Vector3(pos5.x,pos5.y,Mathf.Clamp(pos5.z,-19,16.6f));
//			pos5 = pos0 + new Vector3(m_Trans.position.x + Random.Range(-10,10),m_Trans.position.y,m_Trans.position.z).normalized * 3;
		}
		LamsterPosList.Add(pos0);
		LamsterPosList.Add(pos1);
		LamsterPosList.Add(pos2);
		LamsterPosList.Add(pos3);
		LamsterPosList.Add(pos4);
		LamsterPosList.Add(pos5);
		Animation anim = Anim.m_AnimationList[1];
		anim["Lamster01"].wrapMode = WrapMode.Loop;
		anim.Play("Lamster01");
		Vector3 currentPos;
		while(true)
		{
			if(LamsterPosList.Count == 0)
			{
				anim.Play("LamsterDeath01");
				break;
			}
			if(Vector3.Distance(transform.position,LamsterPosList[0]) < 0.25f)
			{
				LamsterPosList.RemoveAt(0);
				if(LamsterPosList.Count == 0)
				{
					anim.Play("LamsterDeath01");
					break;
				}
			}
			currentPos = LamsterPosList[0];
			transform.LookAt(currentPos);
			transform.Translate(Vector3.forward * LamsterSpeed * Time.deltaTime);
			yield return null;
		}
	}



	//public float explosionForce = 300;
//	public float maxFlyRange=10;
//	public float maxFlyAngle=40;
//	bool hit;
//	float speed = 10;
	IEnumerator _Fly(Vector3 explosionPos)
	{
//		hit = false;
		//make sure the shootObject is facing the target and adjust the projectile angle
		m_Trans.LookAt(explosionPos);
		float angle=Mathf.Min(1, Vector3.Distance(m_Trans.position, explosionPos)/BattleController.SingleTon().FlyDistance)*BattleController.SingleTon().FlyAngle;
		//clamp the angle magnitude to be less than 45 or less the dist ratio will be off
		m_Trans.rotation=m_Trans.rotation*Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
		Vector3 startPos=m_Trans.position;
		float iniRotX=m_Trans.rotation.eulerAngles.x;
		//if(shootEffect!=null) ObjectPoolManager.Spawn(shootEffect, thisT.position, thisT.rotation);
		//while the shootObject havent hit the target
		while(true){
			//if the target is still active, update the target position
			//if not, the position registered from last loop will be used as the target position
//			if(target!=null && target.gameObject.activeInHierarchy){
//				targetPos=target.position;
//			}
			//calculating distance to targetPos
			float currentDist=Vector3.Distance(m_Trans.position, explosionPos);
			//if the target is close enough, trigger a hit
			if(currentDist<0.25f )
			{
//				m_Trans.LookAt(explosionPos);
				m_Trans.eulerAngles = Vector3.zero;
				break;
			}
			//calculate ratio of distance covered to total distance
			float totalDist=Vector3.Distance(startPos, explosionPos);
			float invR=1-currentDist/totalDist;
			//use the distance information to set the rotation, 
			//as the projectile approach target, it will aim straight at the target
			Quaternion wantedRotation=Quaternion.LookRotation(explosionPos-m_Trans.position);
			float rotX=Mathf.LerpAngle(iniRotX, wantedRotation.eulerAngles.x, invR);
			//make y-rotation always face target
			m_Trans.rotation=Quaternion.Euler(rotX, wantedRotation.eulerAngles.y, wantedRotation.eulerAngles.z);
			//Debug.Log(Time.timeScale+"   "+Time.deltaTime);
			//move forward
			m_Trans.Translate(Vector3.forward*Mathf.Min(BattleController.SingleTon().FlySpeed*Time.deltaTime, currentDist));
			yield return null;
		}
	}


	void RemoveHeroList()
	{
		if (this.IsHero)
		{
			SpawnManager.SingleTon().RemoveHeroFromList(this);
		}
	}

	void DeadStatus()
	{
		ChangeUnitStatus(_UnitStatus.Dead);
	} 

	public float dist = 0;
	public bool WayPointMove = false;
	public Vector3 tempPos;
	public bool WayPointMoveDone = false;
	public float IdleDuring = 0;
	void Update()
	{ 
		if(BattleController.SingleTon().IsBegin)
		{ 
//			if(IsHero)
//			{
//				if(UnitStatus == _UnitStatus.Run && !Anim.m_AnimationList[0].IsPlaying("Run01") && !Anim.m_AnimationList[0].IsPlaying("Run02"))
//				{
//					Anim.m_AnimationList[0].Play("Run01");
//				} 
//			}	


			if(UnitStatus == _UnitStatus.Idle || UnitStatus ==_UnitStatus.Arrive)
			{
				IdleDuring += Time.deltaTime;
				if(IdleDuring >= 5)
				{
					BattleController.SingleTon().MyUnitByLines[Line].Remove(this);
					GlobleScanMode = _GlobleScanMode.Free;
					IdleDuring = 0;
				}
			}else{
				IdleDuring = 0;
			}
			if(!WayPointMoveDone)
			{
				if(WayPointMove && WayPoints!=null && WayPoints.Count == 0)
				{
					WayPointMove = false;
					WayPointMoveDone = true;
				}
				if(!WayPointMove && MeleeAttacker!=null && UMatrix.FrontMatrix!=null && UMatrix.FrontMatrix.IsFighting && WayPoints!=null && WayPoints.Count > 0)
				{
					WayPointMove = true;
				}
				if(WayPointMove)
				{
					if(Attribute.HP < Attribute.MaxHP)
					{
						WayPoints.Clear();
						WayPointMove =false;
						if(MeleeAttacker!=null && !MeleeAttacker.Attackable)MeleeAttacker.ResumeAttack();
						if(Attacker!=null && !Attacker.Attackable)Attacker.ResumeAttack();
					}else{
						if(MeleeAttacker.Attackable)MeleeAttacker.StopAttack();
						dist = Vector3.Distance(m_Trans.position,WayPoints[0]);
						if(dist<=2.1f)
						{
							WayPoints.RemoveAt(0);
						}
						if(WayPoints.Count > 0)
						{
							Move.CurrentTargetPos = WayPoints[0];
							if(UnitStatus!=_UnitStatus.Run && UnitStatus!=_UnitStatus.Skill)onMove();
						}else{
							WayPointMoveDone = true;
						}
					}
				}else{
					if(MeleeAttacker!=null && !MeleeAttacker.Attackable && UnitStatus!=_UnitStatus.Skill)MeleeAttacker.ResumeAttack();
					if(Attacker!=null && !Attacker.Attackable && UnitStatus!=_UnitStatus.Skill)Attacker.ResumeAttack();
				}

//				if(IsHero && Alignment == _UnitAlignment.Player)
//				{
//					Collider[] cols = Physics.OverlapSphere(m_Trans.position,3);
//					foreach(Collider col in cols)
//					{
//						Unit unit = col.GetComponent<Unit>();
//						if(unit != null)
//						{
//							Vector3 direction = (unit.m_Trans.position - m_Trans.position).normalized;
//							unit.m_Trans.Translate(direction * Time.deltaTime);
//						}
//					}
//					
//				}
			}

			//if(Skill != null && Skill.IsSkillDone && Skill.enabled == true && UnitStatus!=_UnitStatus.Skill)
			if(Skill != null && Skill.IsSkillDone && Skill.enabled == true)
			{
				onMove();
				Anim.m_AnimationList[0].Play("Run01");
				Debug.Log("Skill Done");
				ResumeAttack();
				Skill.IsSkillDone = false;
			}
			else if(Move != null && Move.Movable == true && !WayPointMove)
			{
				//Check if Arrive default pos,通过调整DefaultTargetPos,可以在任意地方重组队伍  
				if(IsReGrouped && UnitStatus == _UnitStatus.Idle)
				{
					onArrive();
				}
			}
		} 
	}

	float maxDisperseFrequency = 2;
	public float disperseFrequency = 2;
	Vector3 DispersePos;
	IEnumerator _Disperse()
	{
		while(true)
		{
			if(UnitStatus!=_UnitStatus.Run && BattleController.SingleTon().IsBegin && Move.Movable)
			{
				if(Attacker!=null && Attacker.AttackTarget != null)
				{
					if(Physics.OverlapSphere(m_Trans.position,0.5f).Length > 4)
					{
						if(UnitStatus!=_UnitStatus.Run && UnitStatus!=_UnitStatus.Skill)
						{
							Attacker.StopAttack();
							DispersePos = m_Trans.position + m_Trans.TransformDirection(Vector3.forward) * 5 + new Vector3(Random.Range(-5,5),0,0);
							//the battle field width
							if(DispersePos.z > 16.6f || DispersePos.z < -19)
							{
								DispersePos = new Vector3(m_Trans.position.x + Random.Range(-5,5),m_Trans.position.y,m_Trans.position.z);
							}
							Move.CurrentTargetPos = m_Trans.position + m_Trans.TransformDirection(Vector3.forward) * 5 + new Vector3(Random.Range(-5,5),0,0);
							onMove();
							disperseFrequency = Mathf.Max(0,disperseFrequency - 1);
						}
					}
					else
					{
						disperseFrequency = Mathf.Min(maxDisperseFrequency,disperseFrequency + 1);
					}
				}
				if(MeleeAttacker!=null && MeleeAttacker.AttackTarget != null)
				{
					if(Physics.OverlapSphere(m_Trans.position,0.5f).Length > 5)
					{
						if(UnitStatus!=_UnitStatus.Run && UnitStatus!=_UnitStatus.Skill)
						{
							MeleeAttacker.DiscardTarget();
							MeleeAttacker.StopAttack();
							Move.CurrentTargetPos = m_Trans.position + new Vector3(Random.Range(-8,8),0,Random.Range(-8,8));
							onMove();
							disperseFrequency = Mathf.Max(0,disperseFrequency - 1);
						}
					}
					else
					{
						disperseFrequency = Mathf.Min(maxDisperseFrequency,disperseFrequency + 1);
					}
				}
			

			}
			yield return new WaitForSeconds(disperseFrequency);
		}
	}

	void InitArriveEvent()
	{
		onArrive += ArriveStatus;
		if(Anim!=null)onArrive += Anim.TransitionIdle;
		if(Move != null)
		{
			onArrive += Move.Stop;
		}
	}

	void ArriveStatus()
	{
		ChangeUnitStatus(_UnitStatus.Arrive);
	} 

	void InitOverEvent()
	{
		onOver += OverStatus;
		if(Attacker!=null)onOver += Attacker.StopAttack;
		if(MeleeAttacker!=null)onOver += MeleeAttacker.StopAttack;
		if(Anim!=null)onOver += Anim.TransitionIdle;
		if(Move != null)onOver += Move.Stop;
	}

	void OverStatus()
	{
		ChangeUnitStatus(_UnitStatus.Over);
	} 

	public void onMove()
	{
		if (UnitStatus == _UnitStatus.Skill)
		{
			Debug.Log("Skill.................................And..........................Move");
		}
		RunStatus();
		Move.Move();
	}

	public void onAttack()
	{
		AttackStatus();
		if(Anim!=null)Anim.TransitionAttack();
		if(Move != null)Move.Stop();
	}

	public void onIdle()
	{
		IdleStatus();
		if(Anim!=null)Anim.TransitionIdle();
		if(Move != null)Move.Stop();
	}

	public void onRepel()
	{
		RepelStatus();
		//StopMoveAndAttack();


		repel = gameObject.GetComponent<UnitRepelEffect>();
		if (!repel)
		{
			repel = gameObject.AddComponent<UnitRepelEffect>();
		}
		repel.RepelEffect();
	}


	void RunStatus()
	{
		//UnitStatus = _UnitStatus.Run;
		ChangeUnitStatus(_UnitStatus.Run);
	} 

	void IdleStatus()
	{
		//UnitStatus = _UnitStatus.Idle;
		ChangeUnitStatus(_UnitStatus.Idle);
	} 

	void AttackStatus()
	{
		//UnitStatus = _UnitStatus.Attack;
		ChangeUnitStatus(_UnitStatus.Attack);
	}

	void RepelStandUpStatus()
	{
		ChangeUnitStatus(_UnitStatus.RepelStandUp);
	}

	void RepelStatus()
	{
		ChangeUnitStatus(_UnitStatus.Repel);
	}

	void ChangeUnitStatus(_UnitStatus status)
	{
		//Debug.Log("UnitStatus........................................" + UnitStatus);
		UnitStatus = status;	
		//onChangeUnitStatus();

	}

    void InitSkillEvent()
    {
        if (Move != null) onSkill += Move.Stop;
        onSkill += SkillStatus;
        //if (Attacker != null) onSkill += Attacker.StopAttack;
        //if (MeleeAttacker != null) onSkill += MeleeAttacker.StopAttack;
		if (Attacker != null) 
			Attacker.Attackable = false;
		if (MeleeAttacker != null) 
			MeleeAttacker.Attackable = false;
		if (Anim != null) onSkill += Anim.TransitionSkill;
    }

    void SkillStatus()
    {
        //UnitStatus = _UnitStatus.Skill;
		BattleController.SingleTon().gSkilling = true;
		Skill.IsSkillDone = false;
		ChangeUnitStatus(_UnitStatus.Skill);
    }      

	void DisableScripts()
	{
		if(Attacker!=null)
		{
			Attacker.enabled = false;
			Attacker.StopAllCoroutines();
		}
		if(Move!=null){
			Move.enabled = false;
			Move.StopAllCoroutines();
		}
		if(MeleeAttacker!=null)
		{
			if(MeleeAttacker.RushFX!=null && MeleeAttacker.RushFX.activeInHierarchy)
			{
				MeleeAttacker.RushFX.SetActive(false);
			}
			MeleeAttacker.enabled = false;
			MeleeAttacker.StopAllCoroutines();
		}
		if(Skill!=null)
		{
			Skill.enabled = false;
			Skill.StopAllCoroutines();
		}
		GetComponent<Collider>().enabled = false;
		this.enabled = false;
	}  

	void RemoveItSelfFromController()
	{ 
		if(Alignment == _UnitAlignment.Player)
		{
			switch(UnitTroop)
			{
				case ARMY_TYPE.ARCHER:BattleController.SingleTon().PlayerDeadArcherCount++; break;
				case ARMY_TYPE.CAVALRY:BattleController.SingleTon().PlayerDeadCavalryCount++; break;
				case ARMY_TYPE.MAGIC:BattleController.SingleTon().PlayerDeadMagicCount++; break;
				case ARMY_TYPE.SHIELD:BattleController.SingleTon().PlayerDeadPeltastCount++; break;
				case ARMY_TYPE.PIKEMAN:BattleController.SingleTon().PlayerDeadSpearmenCount++; break;
				default:break;
			}
			if(BattleController.SingleTon().MyUnits!=null)BattleController.SingleTon().MyUnits.Remove(this);
			BattleController.SingleTon().MyUnitByLines[Line].Remove(this);
			BattleController.SingleTon().CurrentPlayerFighting = Mathf.Max(0,BattleController.SingleTon().CurrentPlayerFighting - Attribute.MaxHP);
            if (IsHero == false)
            { 
				if(DataManager.me!=null)DataManager.getBattleUIData().HeroArmyDie((uint)HeroId);
            } 
		}
		else if(Alignment == _UnitAlignment.Enemy)
		{
			if(BattleController.SingleTon().EnemyUnits!=null)BattleController.SingleTon().EnemyUnits.Remove(this);
			BattleController.SingleTon().EnemyUnitByLines[Line].Remove(this);
			BattleController.SingleTon().CurrentEnemyFighting = Mathf.Max(0,BattleController.SingleTon().CurrentEnemyFighting - Attribute.MaxHP);
		}
	} 

	public bool DestroyAble = true;
	public bool Hide = true;
	float corpseDuration = 5;
	void HideOrDestroy()
	{
		//TODO
		DestroyAble=true;
		if(DestroyAble)StartCoroutine(_HideOrDestroy());
	}

	IEnumerator _HideOrDestroy()
	{
		yield return new WaitForSeconds(corpseDuration);
		if(Hide)
		{
			if(Attribute.OverlayBar!=null)Attribute.OverlayBar.gameObject.SetActive(false);
			gameObject.SetActive(false);
		}
		else
		{
			if(Attribute.OverlayBar!=null)Destroy(Attribute.OverlayBar.gameObject);
			Destroy(gameObject);
		} 
	} 

    void ResumeAttack()
	{
		if(Attacker!=null)Attacker.ResumeAttack();
		if(MeleeAttacker!=null) MeleeAttacker.ResumeAttack();
	}

	public void StopMoveAndAttack()
	{
		SkillStatus();
		if (Move)
		{
			Move.Movable = false;
			Move.Stop();
		}
		//if(Attacker!=null)Attacker.StopAttack();
		if(Attacker!=null)Attacker.Attackable = false;
		//if(MeleeAttacker!=null) MeleeAttacker.StopAttack();
		if(MeleeAttacker!=null) MeleeAttacker.Attackable = false;
	}

	public void ResumeMoveAndAttack()
	{
		if (Move)
		{
			Move.Move();
		}
		if(Attacker!=null)Attacker.ResumeAttack();
		if(MeleeAttacker!=null) MeleeAttacker.ResumeAttack();
	}
	

} 
   
    
 
 



	 
 
