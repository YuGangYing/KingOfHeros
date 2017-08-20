using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirctShoot : MonoBehaviour {

	public Unit Attacker;
	private Transform m_Trans;
	public GameObject hitEffect;
	public DataMgr.Skill CurrSkill;

	private float speed = 150f;
	void Update()
	{
		//rigidbody.velocity = new Vector3(20,0,0);

		//Debug.Log("transform.position.................................." + transform.position);

		transform.Translate(Vector3.forward*Mathf.Min(speed*Time.deltaTime, 50));


		RaycastHit hit;
		 if (Physics.SphereCast(transform.position,5,transform.forward,out hit))
		{

			if (hit.transform.tag == "Soldier" && hit.transform.GetComponent<Unit>() && hit.transform.GetComponent<Unit>().Alignment != Attacker.Alignment)
			{
				//Debug.Log("hit............................." + hit.transform.gameObject.name);
				hit.transform.GetComponent<Unit>().Attribute.OnDamage(Attacker,true);

				Instantiate(hitEffect,hit.transform.position,hit.transform.rotation) ;

				if(CurrSkill.HitAudioClip!=null)
					AudioSource.PlayClipAtPoint(CurrSkill.HitAudioClip,Camera.main.transform.position);

				//go.transform.parent = hit.transform;		
				//StartCoroutine(_Destory(go,3));

				//Debug.Log("hitEffect....................." + hitEffect);

			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.name);
		if (other.tag == "Soldier")
		{
			_UnitAlignment align = other.gameObject.GetComponent<Unit>().Alignment;
			Debug.Log("other......................" + other.name);
			//other.transform.Translate(Vector3.left * 5);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
			//StartCoroutine(HitRepel(other.transform));
			//setFly(other.transform);
			//other.gameObject.GetComponent<UnitAttribute>().HP = -1;
			other.GetComponent<Unit>().Attribute.OnDamage(Attacker,true);
		}

	}

	IEnumerator HitRepel(Transform other)
	{
		bool fly = true;
		float speed = 1;
		float dis = 2;
		while (fly)
		{
			//yield return new WaitForSeconds(0.1f);
			speed = speed + speed * Time.deltaTime;
			other.Translate(-Vector3.left * speed);
			if (speed >= dis)
				fly = false;
		}
		yield return null;
	}

	void setFly(Transform other)
	{
		m_Trans = other.transform;
		Vector3 flyPos = m_Trans.position + (m_Trans.position - Attacker.transform.position).normalized * Random.Range(5,BattleController.SingleTon().FlyDistance);
		StartCoroutine(_Fly(flyPos));
	}


	bool hit;
	//	float speed = 10;
	IEnumerator _Fly(Vector3 explosionPos)
	{
		hit = false;
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

	
	IEnumerator _Destory(GameObject obj,float delay)
	{
		yield return new WaitForSeconds(delay);
		if (obj) 
		{
			obj.SetActive (false);
		}
		
	}
}