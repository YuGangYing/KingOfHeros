using UnityEngine;
using System.Collections;

public class SimpleWayPoint : MonoBehaviour {

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
	}

}
