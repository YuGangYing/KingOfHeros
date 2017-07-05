using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MathDemo : MonoBehaviour {

	public Vector2 L00;
	public Vector2 L01;
	public Vector2 L10;
	public Vector2 L11;

	public Vector2 point;
	public Transform t;
	public Vector2[] polygonPoints;

	


	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
//			Debug.Log( CommonUtility.CheckTwoLineIntersect(L00,L01,L10,L11));
			Debug.Log(CommonUtility.GetIntersectWithLineAndPlane(t.position,t.forward,new Vector3(0,0,1),polygonPoints[0]));
			RaycastHit hit;
			if(Physics.Raycast(t.position,t.forward,out hit,Mathf.Infinity))
			{
				Debug.Log(hit.point);
			}

		}
	}

	void OnDrawGizmos()
	{
//		Gizmos.DrawLine (L00,L01);
//		Gizmos.DrawLine (L10,L11);
//		Debug.DrawRay (t.position,t.forward);
		Gizmos.DrawLine(t.position,t.position + t.forward * 100);
		if(polygonPoints!=null && polygonPoints.Length > 1)
		{

			Gizmos.DrawWireSphere(point,2);

			Vector2 prePoint = polygonPoints[0];
			for(int i = 1; i < polygonPoints.Length ; i ++)
			{
				Gizmos.DrawLine (prePoint,polygonPoints[i]);
				prePoint = polygonPoints[i];
				if(i == polygonPoints.Length - 1)
				{
					Gizmos.DrawLine (polygonPoints[0],polygonPoints[i]);
				}
			}
		}



	}






}
