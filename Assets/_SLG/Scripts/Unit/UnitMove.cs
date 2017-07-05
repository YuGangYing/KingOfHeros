using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum _MoveMode{Array,Free}
//[RequireComponent(typeof(NavMeshAgent))]
public class UnitMove : MonoBehaviour {

    private Unit m_Unit;

    private UnitAttribute m_UnitAbt;

//	public delegate void _OnMove();
//	
//    public _OnMove onMove;

	public _MoveMode MoveMode = _MoveMode.Array; 

    public float ArrayForwardDist = 40;
	
    public Vector3 DefaultMoveTarget; 
	Transform m_Trans;

	void Awake () {
        m_Unit = GetComponent<Unit>();
        m_Trans = transform;
        if (m_Unit == null)
        {
            return;
        }
        m_UnitAbt = GetComponent<UnitAttribute>();
        if (m_UnitAbt == null)
        {
            return;
        }
        m_UnitAbt.RealAdjustSpeed = Random.Range(0.0f, m_UnitAbt.AdjustMoveSpeed); 
	} 
 
	void Start()
	{
//		onMove += m_Unit.Anim.TransitionRun;
		InitDefaultTarget();
	}

	public void QuickMove()
	{
		if(!IsMoving && Movable)
		{
			StartCoroutine(_QuickMove());
		}
	}

	public bool Movable = true;
	public bool IsMoving;
	public float StopDistance = 2;
	public int MoveType=0;//0:move to position,1:move to transform
	public float speed  ;
	public float CurrentDist;
	public float CurrentDist1;
	IEnumerator _QuickMove()
	{
		IsMoving = true;
		float dist = 0;
		while(IsMoving)
		{
			m_Trans.LookAt(CurrentTargetPos);
			dist = Vector3.Distance(m_Trans.position,CurrentTargetPos);
			CurrentDist =dist;
			if(Movable)m_Trans.Translate(new Vector3(0,0,Mathf.Min(dist,speed*Time.deltaTime)));
			CurrentDist1 = Mathf.Min(dist,speed*Time.deltaTime);
			if(dist <= StopDistance)
			{
				IsMoving=false;
			}
			if(m_Unit.Attacker!=null && m_Unit.Attacker.AttackTarget!=null && Vector3.Distance(m_Trans.position,m_Unit.Attacker.AttackTarget.transform.position)<m_Unit.Attribute.AttackRadius)
			{
				IsMoving=false;
			}
			yield return null;
		}
		if(m_Unit.MeleeAttacker!=null)m_Unit.MeleeAttacker.ResumeAttack();
		if(m_Unit.Attacker!=null)m_Unit.Attacker.ResumeAttack();
		m_Unit.onIdle();
	}

	public void SpeedUp()
	{ 
        if (speed != m_UnitAbt.BaseMoveSpeed + m_UnitAbt.RealAdjustSpeed)
        {
            speed = m_UnitAbt.BaseMoveSpeed + m_UnitAbt.RealAdjustSpeed;

            float tmp_animspeed = Mathf.Min(speed, 1.5f);
            
            m_Unit.Anim.SetAnimation(AnimationTyp.RUN, 2, WrapMode.Loop, tmp_animspeed);
        }
	}

	public void SpeedNormalize()
	{ 
        if (speed != m_UnitAbt.BaseMoveSpeed)
        {
            speed = m_UnitAbt.BaseMoveSpeed; 

            m_Unit.Anim.SetAnimation(AnimationTyp.RUN, 1, WrapMode.Loop, speed);
        }
	}

	public float Velocity;
	public float BlockTimeDur;
	public float RemainDist;
	void InitDefaultTarget()
	{
		Vector3 targetPos = new Vector3(transform.position.x + ArrayForwardDist,transform.position.y,transform.position.z);
		DefaultMoveTarget = targetPos;
		CurrentTargetPos = targetPos;
	}

	public void SetToDefaultTargetPos()
	{
		CurrentTargetPos = DefaultMoveTarget;
	}

	public Vector3 CurrentTargetPos;
	public void Move()
	{
		m_Unit.Anim.TransitionRun();
		QuickMove();
//		if(onMove!=null)onMove();
	}

	public void MoveToDefaultTarget()
	{
		CurrentTargetPos = DefaultMoveTarget;
		Move();
	}

	public void Move(Transform target)
	{
		CurrentTargetPos = target.position;
		QuickMove();
//		if(onMove!=null)onMove();
	}

	public void Move(Vector3 pos)
	{
		CurrentTargetPos = pos;
		Move ();
	}

	public void Stop()
	{ 
		IsMoving=false;
	}

}
