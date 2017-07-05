using UnityEngine;
using System.Collections;

public class CityCamera : MonoBehaviour {

	public Camera cityCamera;

	public bool cameraMovable = true;
	public bool isCameraMoving = false;

	public float moveSpeed = 10;

	public Vector3[] movableArea;
	public Vector2[] movableArea2D;
	Vector3 movablePlaneNormal;

	void Awake()
	{
		cityCamera = Camera.main;
		if (movableArea.Length >= 3) {
			movablePlaneNormal = Vector3.Cross((movableArea[1]-movableArea[0]).normalized,(movableArea[2]-movableArea[0]).normalized);
		}
		movableArea2D = new Vector2[movableArea.Length];
		for(int i=0;i < movableArea.Length;i ++)
		{
			movableArea2D[i] = new Vector2(movableArea[i].x,movableArea[i].z);
		}
	}

	Vector2 preMousePos;
	Vector2 curMousePos;

	Vector3 startCameraPos;
	public Vector3 caculateCameraPos;
	Vector3 targetCameraPos;


	Vector3 pointAtMovablePlane;


	// Update is called once per frame
	void Update () {

#region move by keybouard
		float y = Input.GetAxis("Vertical");
		float x = Input.GetAxis("Horizontal");
		cityCamera.transform.position += new Vector3(x,y,0) * moveSpeed;
#endregion

#region move by mouse or touch
		if(Input.GetMouseButtonDown(1))
		{
			preMousePos = Input.mousePosition;
			startCameraPos = cityCamera.transform.position;
			targetCameraPos = cityCamera.transform.position;
		}
		if(Input.GetMouseButton(1))
		{
			curMousePos = Input.mousePosition;
			Vector2 deltaPos = curMousePos - preMousePos;

			targetCameraPos +=  new Vector3(deltaPos.x,0,deltaPos.y ) * moveSpeed;
			preMousePos = curMousePos; 
			caculateCameraPos = Vector3.Slerp(cityCamera.transform.position,targetCameraPos,0.8f);
			cityCamera.transform.position = caculateCameraPos;
//			pointAtMovablePlane = CommonUtility.GetIntersectWithLineAndPlane(caculateCameraPos,cityCamera.transform.forward,movablePlaneNormal,movableArea[0]);
//
//			if(CommonUtility.PointInPolygon(new Vector2(pointAtMovablePlane.x,pointAtMovablePlane.z) ,movableArea2D))
//			{
//				cityCamera.transform.position = caculateCameraPos;
//			}
		}
		else
		{
			pointAtMovablePlane = CommonUtility.GetIntersectWithLineAndPlane(cityCamera.transform.position,cityCamera.transform.forward,movablePlaneNormal,movableArea[0]);
			float x0 = 0;
			if(pointAtMovablePlane.x > 30 )
			{
				x0 = 30 - pointAtMovablePlane.x;
			}
			else if(pointAtMovablePlane.x < -30)
			{
				x0 = -30 - pointAtMovablePlane.x;
			}
			float y0 = 0;
			if(pointAtMovablePlane.z > 30 )
			{
				y0 = 30 - pointAtMovablePlane.z;
			}
			else if(pointAtMovablePlane.z < -70)
			{
				y0 = - 70 -pointAtMovablePlane.z;
			}

			cityCamera.transform.position = Vector3.Slerp(cityCamera.transform.position,cityCamera.transform.position+new Vector3(x0,0,y0),0.3f);
			cityCamera.transform.position = new Vector3(cityCamera.transform.position.x,70,cityCamera.transform.position.z);
		}
#endregion

#region scale by mouse or touch


#endregion
//		RotateCamera ();
	}


	
	Vector2 preScalePos0;
	Vector2 preScalePos1;
	
	Vector2 curScalePos0;
	Vector2 curScalePos1;

	Vector3 targetLocalAngle;
	void RotateCamera()
	{
		preScalePos0 = new Vector2 (Screen.width/2,Screen.height/2);
		curScalePos0 = preScalePos0;
		if(Input.GetMouseButtonDown(0))
		{
			preScalePos1 = Input.mousePosition;
			targetLocalAngle = transform.localEulerAngles;
		}

		if(Input.GetMouseButton(0))
		{
			curScalePos1 = Input.mousePosition;
			Vector2 preDeltaPos = (preScalePos0 - preScalePos1).normalized;
			Vector2 curDeltaPos = (curScalePos0 - curScalePos1).normalized;
			
			float cos = Vector3.Dot(new Vector3(preDeltaPos.x,preDeltaPos.y,0),new Vector3(curDeltaPos.x,curDeltaPos.y,0));
			float angle = Mathf.Acos(cos);
			float z = Vector3.Cross(new Vector3(preDeltaPos.x,preDeltaPos.y,0),new Vector3(curDeltaPos.x,curDeltaPos.y,0)).z;
			Debug.Log("cos:" + cos + ";angle:" + angle + ";z" + z);

			if(z > 0)
			{
				z = 1;
			}
			else
			{
				z = -1;
			}
			if(angle>0 || angle<0)
			{
				targetLocalAngle += new Vector3(0,20 * angle * z,0);
				Quaternion to = Quaternion.Euler(targetLocalAngle);
				cityCamera.transform.rotation = Quaternion.Slerp(cityCamera.transform.rotation,to,0.3f);
				pointAtMovablePlane = CommonUtility.GetIntersectWithLineAndPlane(cityCamera.transform.position,cityCamera.transform.forward,movablePlaneNormal,movableArea[0]);
				cityCamera.transform.RotateAround(pointAtMovablePlane,Vector3.up,angle * z * 20);
			}

			preScalePos1 = curScalePos1;
		}
	}


	void OnDrawGizmos()
	{
		if(movableArea!=null && movableArea.Length>=3)
		{
			for(int i = 1,j = 0;i < movableArea.Length;i ++,j ++)
			{
				Gizmos.DrawLine(movableArea[i],movableArea[j]);
				if(i == movableArea.Length - 1)
				{
					Gizmos.DrawLine(movableArea[i],movableArea[0]);
				}
			}
			Gizmos.DrawSphere(pointAtMovablePlane,0.5f);
		}
	}


}
