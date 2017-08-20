using UnityEngine;
using System.Collections;

public class UnitRepelEffect : MonoBehaviour {
	
	bool isExplosion = false;
	// Use this for initialization
	
	private float m_fAnimtionTime = 0;	
//	private float m_fAnimSpeed = 1;
	private bool isOver = false;
	
	Animation ani;
	CapsuleCollider Capsule;
	Rigidbody rigid;
	UnitAnim unitAnim;
	Quaternion priorQuaternion;
//	string m_PriorClipName; 

	void Start() {
		
//		m_fAnimSpeed = 1;
		
		Capsule = gameObject.GetComponent<CapsuleCollider>();	
		unitAnim = GetComponent<UnitAnim>();
	}
	
	
	// Update is called once per frame
	void Update () {		

		//float length = unitAnim.GetClipLength(AnimationTyp.REPELSTANDUP,1);
		//Debug.Log("lengh..................................................." + length);

		//if (isExplosion && m_fAnimtionTime > unitAnim.GetClipLength("RepelStandUp1") && isOver)


		if (isExplosion && m_fAnimtionTime > unitAnim.GetClipLength(AnimationTyp.REPELSTANDUP,1) && isOver)
		{
			Debug.Log("AnimationTyp.REPELSTANDUP........................................................." + unitAnim.GetClipLength(AnimationTyp.REPELSTANDUP,1));
			//unitAnim.SetAnimation(AnimationTyp.RUN, 1, WrapMode.Loop); 
			/*Rigidbody rigid = this.gameObject.GetComponent<Rigidbody>();
			if (rigid)
				Destroy(rigid);*/

			//GetComponent<Unit>().onMove();
			this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,0,this.gameObject.transform.position.z);
			//this.gameObject.transform.rotation = Quaternion.identity;
			this.gameObject.transform.rotation = priorQuaternion;
			isExplosion = false;
			isOver = false;
		}
		
		/*if (isExplosion && isOver)
		//if (isOver)
		{
			Vector3 bwd = transform.TransformDirection(-Vector3.forward);
			Vector3 fwd = transform.TransformDirection(Vector3.forward);
			if (Physics.Raycast(transform.position,fwd,0.2f) || Physics.Raycast(transform.position,bwd,0.2f))
			{
				Rigidbody rigid = this.gameObject.GetComponent<Rigidbody>();
				Destroy(rigid);
				//StartCoroutine(setExplosion());		
			}
		}*/

	}


	public void RepelEffect()
	{
		isExplosion = true;	
		isOver = false;		
		priorQuaternion = gameObject.transform.rotation;

		if (!Capsule)
			Capsule = gameObject.GetComponent<CapsuleCollider>();	

		//Capsule.isTrigger = true;
		rigid = this.gameObject.GetComponent<Rigidbody>();
		if (rigid == null)
			rigid = this.gameObject.AddComponent<Rigidbody>();

		if (!unitAnim)
			unitAnim = GetComponent<UnitAnim>();


//		m_PriorClipName = unitAnim.m_CurClipName;
		unitAnim.SetAnimation(AnimationTyp.REPEL, 1, WrapMode.Once); 
		Debug.Log("REPEL..........................................................");

		Vector3 force = new Vector3(0,1.5f,-0.5f);
		transform.Translate(force);
		StartCoroutine(AddDownForce());
		rigid.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		rigid.AddForce(0,300,-100);
		//StartCoroutine(setExplosion());	
	}
	
	IEnumerator setExplosion()
	{
		yield return new WaitForSeconds(0.5f);
		Rigidbody rigid = this.gameObject.GetComponent<Rigidbody>();
		if (rigid)
			Destroy(rigid);
		unitAnim.SetAnimation(AnimationTyp.REPELSTANDUP, 1, WrapMode.Once); 
		Debug.Log("REPELSTANDUP..........................................................");
		//yield return new WaitForSeconds(0.5f);
		m_fAnimtionTime = 0;
		isOver = true;
		//this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,0,this.gameObject.transform.position.z);
		//this.gameObject.transform.rotation = Quaternion.identity;
		//this.gameObject.transform.rotation = priorQuaternion;
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Plane" || other.tag == "Soldier")
		{
			Capsule.isTrigger = false;
			StartCoroutine(setExplosion());		
		}
	}
	
	void LateUpdate()
	{
		//m_fAnimtionTime += Time.deltaTime * m_fAnimSpeed;
		m_fAnimtionTime += Time.deltaTime;
		Debug.Log("m_fAnimtionTime............................." + m_fAnimtionTime);
	}
	
	IEnumerator AddDownForce()
	{
		yield return new WaitForSeconds(0.1f);
	 	if (rigid)
			rigid.AddForce(0,-300,0);

		//yield return new WaitForSeconds(0.1f);
		Capsule.isTrigger = true;
	}
}

