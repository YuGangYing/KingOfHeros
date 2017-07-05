//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;

public static class CommonUtility
{

	public static T AddOrGetComponent<T> (this MonoBehaviour mono) where T : MonoBehaviour
	{
		T t = mono.GetComponent<T> ();
		if (t == null) {
			t = mono.gameObject.AddComponent<T> ();
		}
		return t;
	}

	//TODO
	public static bool CheckTwoLineIntersect (Vector2 L00, Vector2 L01, Vector2 L10, Vector2 L11)
	{

		float d0 = Vector2.Dot (L00 - L10, L11 - L10);
		float d1 = Vector2.Dot (L01 - L10, L11 - L10);

		float d2 = Vector2.Dot (L00 - L11, L10 - L11);
		float d3 = Vector2.Dot (L01 - L11, L10 - L11);

		float d4 = Vector2.Dot (L11 - L00, L01 - L00);
		float d5 = Vector2.Dot (L10 - L00, L01 - L00);

		float d6 = Vector2.Dot (L11 - L01, L00 - L01);
		float d7 = Vector2.Dot (L01 - L01, L00 - L01);
		if (d0 >= 0 && d1 >= 0 && d2 >= 0 && d3 >= 0 && d4 >= 0 && d5 >= 0 && d6 >= 0 && d7 >= 0) {
			return true;
		}
		return false;
	}

	public static bool CheckPointInPolygon (Vector2 point, Vector2[] polygonPoint)
	{
		if (polygonPoint == null || polygonPoint.Length < 3) {
			return false;
		}
		float maxX = 0;
		foreach (Vector2 pos in polygonPoint) {
			maxX = Mathf.Max (pos.x, maxX);
		}

		Vector2 outPos = new Vector2 (maxX + 100, 0);

		Debug.Log ("outPos:" + outPos);
		int intersectCount = 0;
		Vector2 startPos = polygonPoint [0];
		for (int i = 1; i < polygonPoint.Length; i ++) {
			if (CheckTwoLineIntersect (outPos, point, startPos, polygonPoint [i])) {
				intersectCount ++;
			}
			if (i == polygonPoint.Length - 1) {
				if (CheckTwoLineIntersect (outPos, point, polygonPoint [0], polygonPoint [i])) {
					intersectCount ++;
				}
			}
			startPos = polygonPoint [i];
		}
		if (intersectCount % 2 == 1) {
			return true;
		} else {
			return false;
		}
	}

	public static bool PointInPolygon (Vector2 p, Vector2[] poly)
	{
		Vector2 p1, p2;
		bool inside = false;
		if (poly.Length < 3) {
			return inside;
		}
		Vector2 oldPoint = new Vector2 (poly [poly.Length - 1].x, poly [poly.Length - 1].y);
		for (int i = 0; i < poly.Length; i++) {
			Vector2 newPoint = new Vector2 (poly [i].x, poly [i].y);
			if (newPoint.x > oldPoint.x) {
				p1 = oldPoint;
				p2 = newPoint;
			} else {
				p1 = newPoint;
				p2 = oldPoint;
			}
			if ((newPoint.x < p.x) == (p.x <= oldPoint.x)
				&& ((long)p.y - (long)p1.y) * (long)(p2.x - p1.x)
				< ((long)p2.y - (long)p1.y) * (long)(p.x - p1.x)) {
				inside = !inside;
			}
			oldPoint = newPoint;
		}
		return inside;
	}

	/***
	 *  1.Vector3.Dot(P - P0,normal) = 0;
	 *  2.dL + L0 = P;
	 *  Vector3.Dot(P - P0,normal) = 0; --> Vector3.Dot(dL+L0-P0,normal) --> d * Vector3.Dot(L+L0.normal) = Vector3.Dot(P0,normal);
	 *  d = Vector3.Dot(P0,normal)/Vector3.Dot(L+L0.normal);
	 *  pass d to next equation
	 *  d * L + L0 = P;
	 *  @See http://en.wikipedia.org/wiki/Line%E2%80%93plane_intersection
	 ***/
	public static Vector3 GetIntersectWithLineAndPlane(Vector3 point,Vector3 direct,Vector3 planeNormal,Vector3 planePoint){
		float d = Vector3.Dot (planePoint - point, planeNormal) / Vector3.Dot (direct, planeNormal);
		return d * direct.normalized + point;
	}


}

