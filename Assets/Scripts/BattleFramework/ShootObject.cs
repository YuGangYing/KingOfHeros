//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;

namespace BattleFramework{
    public class ShootObject : MonoBehaviour {

    	[SerializeField]
    	private Vector3 mTargetPos;
    	[SerializeField]
    	private float mSpeed;
    	public float startAngle;

    	public GameObject hitEffect;

    	private UnitBase mAttacker;
    	private float mPhysicsCheckRadius;
    	private int mTargetLayer;


    	void Update()
    	{
    		if(Input.GetKeyDown(KeyCode.H))
    		{
    			Projection(transform.position + new Vector3(20,0,0),mSpeed);
    		}
    	}

    	public void Shoot(UnitBase attacker,Vector3 targetPos,float speed,int layer)
    	{
    		Shoot (attacker,null,targetPos,speed,0.1f,layer);
    	}

    	/***
    	 *  note: 
    	 *  if targetT is not null,means the shootobject will follow the targetT. else it will move to the targetPos.
    	 *  the checkRadius and the layer is used to physcis checking.
    	 */
    	public virtual void Shoot(UnitBase attacker,Transform targetT,Vector3 targetPos,float speed,float checkRadius,int layer)
    	{
    		mPhysicsCheckRadius = checkRadius;
    		mTargetLayer = layer;
    		mSpeed = speed;
    		mTargetPos = targetPos;
    		mAttacker = attacker;
    		StartCoroutine(_ShootLinear());
    	}

    	public bool hit;
    	IEnumerator _ShootLinear()
    	{
    		 hit = false;yield return null;
    		while(!hit)
    		{
    			Collider[] colls = Physics.OverlapSphere (transform.position,mPhysicsCheckRadius,1<<mTargetLayer);
    			if(colls!=null && colls.Length > 0)
    			{
    				hit = true;
    				colls[0].GetComponent<UnitController>().OnDamage(mAttacker);

    			}
    			else if(Vector3.Distance(mTargetPos,transform.position) <= mSpeed * Time.deltaTime)
    			{
    				hit = true;;
    			}

    			if(hit)
    			{
    				//TODO there maybe multi way to show hiteffect;
    				if(hitEffect!=null)
    				{
    					PoolManager.SingleTon().Spawn(hitEffect,transform.position,Quaternion.identity);
    				}
    				PoolManager.SingleTon().UnSpawn(gameObject);
    			}
    			else
    			{
    				transform.Translate(Vector3.forward * mSpeed * Time.deltaTime);
    			}
    			yield return null;
    		}
    	}

    	public void Projection(Vector3 targetPos,float speed)
    	{
    		mTargetPos = targetPos;
    		mSpeed = speed;
    		Projection ();
    	}

    	public void Projection(Transform target,float speed){
    		mSpeed = speed;
    		Projection ();
    	}

    	private void Projection()
    	{
    		StopCoroutine ("_Projection");
    		StartCoroutine (_Projection());
    	}

    	IEnumerator _Projection()
    	{
    		Vector3 startPos = transform.position;
    		bool hit = false;
    		bool moveable = true;
    		transform.LookAt (mTargetPos);
    		transform.eulerAngles = new Vector3 (0,startAngle,0);
    		float startDistance = Vector3.Distance (startPos,mTargetPos);
    		float distance;
    		while(!hit && moveable)
    		{
    			Quaternion from = transform.rotation;
    			Quaternion to = Quaternion.LookRotation(mTargetPos-transform.position);
    			distance = Vector3.Distance(transform.position,mTargetPos);
    			transform.rotation = Quaternion.Slerp(from,to,1 - distance/startDistance);
    			transform.Translate(Vector3.forward * mSpeed);
    			if(Vector3.Distance(transform.position,mTargetPos) <= 1)
    			{
    				hit = true;
    			}
    			yield return null;
    		}
    	}

    	void OnDrawGizmos()
    	{
    		Gizmos.color = Color.blue;
    		Gizmos.DrawWireSphere (transform.position,mPhysicsCheckRadius);
    	}
    }
}
