using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShootObject : MonoBehaviour {

	public delegate void OnHit();
	public OnHit onHit;

	Transform thisT;
	Transform target;
	Vector3 targetPos;
	public bool level = false;
	public Unit AttackTarget;
    public Unit Attacker;
	public float Damage;
	public GameObject HitAffect;

	// Use this for initialization
	void Awake () {
		thisT = transform;
	}

	void Start()
	{
		onHit += OnDamage;
	}

	public void Shoot(List<Transform> targets,Unit targetUnit,Unit _attacker)
	{
        if (_attacker == null)
        {
            Logger.LogError("_attacker is null");
            return;
        }
        
		if(targets!=null && targets.Count>0)
		{
			AttackTarget = targetUnit;
            Attacker = _attacker;
			Shoot(targets[Random.Range(0, targets.Count)],Attacker);
		}
	}

	public void Shoot(Transform t,Unit _attacker)
	{
		target = t;
		Attacker = _attacker;
		AttackTarget = t.GetComponent<Unit>();
		if(AttackTarget==null && t.parent!=null)
		{
			AttackTarget = t.parent.GetComponent<Unit>();
		}
		if(AttackTarget==null)Debug.Log("AttackTarget is NULL");
		StartCoroutine(_Shoot());
	}

	//for projectile
	public float speed=10;
	public float maxShootRange=10;
	public float maxShootAngle=20;
	[SerializeField]private bool hit;

	IEnumerator _Shoot() {
		if(target!=null) targetPos=target.position;
		else{
			//StartCoroutine(Unspawn());
			yield break;
		}
		hit = false;
		//make sure the shootObject is facing the target and adjust the projectile angle
		thisT.LookAt(targetPos);
		float angle=Mathf.Min(1, Vector3.Distance(thisT.position, targetPos)/maxShootRange)*maxShootAngle;
		//clamp the angle magnitude to be less than 45 or less the dist ratio will be off
		if (!level)
		{
//			Debug.Log("thisT.name................1....................." + thisT.name);
			thisT.rotation=thisT.rotation*Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
		}
		Vector3 startPos=thisT.position;
		float iniRotX=thisT.rotation.eulerAngles.x;
		//if(shootEffect!=null) ObjectPoolManager.Spawn(shootEffect, thisT.position, thisT.rotation);
		//while the shootObject havent hit the target
		while(!hit){
			//if the target is still active, update the target position
			//if not, the position registered from last loop will be used as the target position
			if(target!=null && target.gameObject.activeInHierarchy){
				targetPos=target.position;
			}
			//calculating distance to targetPos
			float currentDist=Vector3.Distance(thisT.position, targetPos);
			//if the target is close enough, trigger a hit
			if(currentDist<0.5f && !hit) Hit();
			//calculate ratio of distance covered to total distance
			float totalDist=Vector3.Distance(startPos, targetPos);
			float invR=1-currentDist/totalDist;
			//use the distance information to set the rotation, 
			//as the projectile approach target, it will aim straight at the target
			Quaternion wantedRotation=Quaternion.LookRotation(targetPos-thisT.position);
			float rotX=Mathf.LerpAngle(iniRotX, wantedRotation.eulerAngles.x, invR);
			//make y-rotation always face target
			if (!level)
			{
//				Debug.Log("thisT.name....................................." + thisT.name);
				thisT.rotation=Quaternion.Euler(rotX, wantedRotation.eulerAngles.y, wantedRotation.eulerAngles.z);
			}
			//Debug.Log(Time.timeScale+"   "+Time.deltaTime);
			//move forward
			thisT.Translate(Vector3.forward*Mathf.Min(speed*Time.deltaTime, currentDist));
			yield return null;
		}
	}

	void Hit()
	{
		hit=true;
		if(onHit!=null)onHit();
		if(HitAffect!=null)
		{
			GameObject go =	Instantiate(HitAffect,transform.position,Quaternion.identity) as GameObject;
			StartCoroutine(InActiveObject(go,3));
		}
		gameObject.SetActive(false);
		SpawnManager.SingleTon().poolManager.UnSpawn(gameObject);
	}

	IEnumerator InActiveObject(GameObject go,float delay)
	{
		yield return new WaitForSeconds(delay);
		go.SetActive(false);
	}


	public bool IsStrickCrucial;
	public bool IsRapidSnipe;
	void OnDamage()
	{
		if (AttackTarget ==null)
		{
			return;
		}
			
		if(AttackTarget.Attribute.IsDeadlyThrowed && IsStrickCrucial)
		{
			Damage += AdditionDamage;
		}
		if(AttackTarget.Attribute.IsRangerMarked && IsRapidSnipe)
		{
			Damage += AdditionDamage;
		}
		if(AttackTarget != null && AttackTarget.Attribute != null)
		{
            if (Attacker == null)
            {
                Logger.LogError("Attacker is null");
                return;
            }

			if (Attacker.IsHero)
				AttackTarget.Attribute.OnDamage(Attacker,true);
			else
            	AttackTarget.Attribute.OnDamage(Attacker);

			//AttackTarget.Attribute.Hit(Damage,ShowDamageText);
		}
	}

	[HideInInspector]public bool ShowDamageText =false;
	[HideInInspector]public float SkillEffectDuration = 0;
	public void OnRangerMarkHit()
	{
		AttackTarget.Attribute.OnRangerMarked(SkillEffectDuration);
	}

	[HideInInspector]public float AdditionDamage = 0;

	[HideInInspector]public float BleedCooldown = 0;
	public void OnDeadlyThrowHit()
	{
		AttackTarget.Attribute.OnBleed(Damage,BleedCooldown);
	}
}
