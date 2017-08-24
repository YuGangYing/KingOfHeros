using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimplePathMove : MonoBehaviour {

	public List<Transform> WayPoints;
	public Transform CurrentPoint;
	public int PointIndex = 0;
	public Animation Anim;
	public float Speed = 2;
	public string moveState = "Walk01";
	public string idleState = "StandBy01";
	void Start () {
		//TODO
//		Anim[moveState].wrapMode = WrapMode.Loop;
//		Anim.Play(moveState);
//		Anim[idleState].wrapMode = WrapMode.Loop;
	}
	public bool IsIdle;
	void Update () {
		if(!IsIdle)
		{
			CurrentPoint = WayPoints[PointIndex];
			transform.LookAt(CurrentPoint);
			transform.Translate(Vector3.forward*Time.deltaTime*Speed);
			transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,transform.eulerAngles.z);
			if(Vector3.Distance(transform.position,CurrentPoint.position)<= 0.1f)
			{
				PointIndex ++;
				if(PointIndex==WayPoints.Count)
				{
					PointIndex = 0;
					IsIdle = true;
					StartCoroutine(_Idle(3));
				}
			}
		}
	}


	IEnumerator _Idle(float idleDuring)
	{
//		Anim.Play(idleState);
		yield return new WaitForSeconds(idleDuring);
//		Anim.Play(moveState);
		IsIdle = false;
	}


}
