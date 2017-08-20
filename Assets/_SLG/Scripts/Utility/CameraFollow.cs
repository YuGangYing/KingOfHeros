using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public static CameraFollow instance;
	public static CameraFollow SingleTon(){
		return instance;
	}

	
	public Transform CameraPos;
	public Transform SubCameraPos0;
	public Transform SubCameraPos1;
	public Transform MainCameraTrans;
	public Unit CurrentFollowHero;
	public int CurrentSubCameraPos = 0;

	void Awake()
	{
		if(instance==null)
		{
			instance = this;
		}
	}

	void Update()
	{
		if(BattleController.SingleTon().IsBegin)
		{
			Follow();
		}
	}

	public void StartFollow()
	{
		if(GetComponent<Animation>())GetComponent<Animation>().Stop();
		Follow();
		MainCameraTrans.parent = SubCameraPos0;
		MainCameraTrans.localPosition = Vector3.zero;
		MainCameraTrans.localEulerAngles = Vector3.zero;
	}

	public void Follow(Unit unit)
	{
		if(unit!=null)
		{
			CurrentFollowHero = unit;
			CurrentSubCameraPos = 1;
		}
		else
		{
			CurrentSubCameraPos = 0;
		}
		BattleController.SingleTon().ChangeSelectHero(unit);
	}

	public void Follow()
	{
		if(CurrentFollowHero==null || CurrentFollowHero.Attribute.HP <=0 )
		{
			CurrentFollowHero = GetAliveHero();
		}
		else
		{
			if(CameraPos!=null)
			{
				CameraPos.position = CurrentFollowHero.transform.position;
				if(CurrentSubCameraPos==1)
					MainCameraTrans.position = SubCameraPos0.position;
				else
					MainCameraTrans.position = SubCameraPos1.position;
			}
		}
	}
	
	public Unit GetAliveHero()
	{
		foreach(Unit unit in SpawnManager.SingleTon().PlayerHeros)
		{
			if(unit!=null && unit.Attribute.HP >0)
			{
				return unit;
			}
		}
		return null;
	}

}
