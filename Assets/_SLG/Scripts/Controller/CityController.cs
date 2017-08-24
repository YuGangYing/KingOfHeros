using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KOH;

public class CityController : MonoBehaviour {

	public static CityController instance;
	public static CityController SingleTon()
	{
		return instance;
	}

    public List<GameObject> cityPrefabs;

	public Camera CityCamera;
	public float CameraSpeed = 100;
	private int m_BuildingLayer = 11;
	private int m_BuildingBaseLayer = 16;
	private int m_TerrainLayer = 9;

	public Color AvailableColor;
	public Color UnAvailableColor;
	
	public Transform SelectedBuild;
	public Vector3 PreviousPos;
	public bool AreaAvaliable;
    public AudioClip backMusic;
    public AudioSource cityAudio;
	public Transform cityPoint;
	GameObject mCityObject;

	void Awake () {
		if(instance==null)
		{
			instance = this;
            PlayCityMisic();
		}
	}

    void Start()
    {
		mCityObject = ResourcesManager.GetInstance.GetCity ();
		mCityObject.transform.position = cityPoint.position;
    }

    void Update()
    {
        if(DoubleClick())
        {
            Debug.Log("DoubleClick");
        }
    }

    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;
    bool DoubleClick(){
        if (Input.GetMouseButtonDown (0)) {
            clicked++;
            if (clicked == 1) clicktime = Time.time;
        }
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            return true;
        } 
        else if (clicked > 2 || Time.time - clicktime > 1)
        {
            clicked = 0;   
        }
        return false;
    }


    void PlayCityMisic()
    {
        if(backMusic==null)
            backMusic = Resources.Load<AudioClip>("Audio/dashidai");
        if(cityAudio==null)
        {
            if(GetComponent<AudioSource>() == null)
            {
                cityAudio = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                cityAudio = GetComponent<AudioSource>();
            }
        }
        cityAudio.clip = backMusic;
        cityAudio.Play();
        cityAudio.loop = true;
    }

//	void Update () {
//		if(!SelectedBuild)
//		{
//			if(DoubleClick())
//			{
//				Debug.Log("DoubleClick");
//				SelectedBuild = SelectBuilding();
//				if(SelectedBuild!=null)
//				{
//					PreviousPos = SelectedBuild.position;
//					ToggleColor(AvailableColor,SelectedBuild.gameObject);
//					AreaAvaliable = true;
//				}
//			}
//		}
//		else
//		{
//			MouseFollow();
//			if(Input.GetMouseButtonDown(0))
//			{
//				ToggleColor(Color.white,SelectedBuild.gameObject);
//				if(!AreaAvaliable)
//				{
//					SelectedBuild.position = PreviousPos;
//				}
//				SelectedBuild = null;
//			}
//		}
////		float fScroll = Input.GetAxis("Mouse ScrollWheel");
////		if (fScroll != 0)
////		{
////			CityCamera.transform.Translate(Vector3.forward * Time.deltaTime * fScroll * CameraSpeed);
////		}
//	}
//
//	void MouseFollow()
//	{
//		if(!SelectBuildingBase())
//		{
//			Vector3 pos = GetMovePos();
//			if(pos!=Vector3.zero)
//			{
//				SelectedBuild.position = pos;
//				ToggleColor(UnAvailableColor,SelectedBuild.gameObject);
//				AreaAvaliable = false;
//			}
//		}else{
//			SelectedBuild.position = SelectBuildingBase().FindChild("AreaCenter").position;
//			ToggleColor(AvailableColor,SelectedBuild.gameObject);
//			AreaAvaliable = true;
//		}
//	}
//
//	void ToggleColor(Color color,GameObject go)
//	{
//		Renderer[] rends = go.GetComponentsInChildren<Renderer>();
//		foreach(Renderer rend in rends)
//		{
//			rend.material.SetColor("_Color",color);
//		}
//	}
//
//	Vector3 GetMovePos()
//	{
//		Ray ray = CityCamera.ScreenPointToRay(Input.mousePosition);
//		RaycastHit hit;
//		if(Physics.Raycast(ray,out hit,Mathf.Infinity,1<<m_TerrainLayer))
//		{
//			return hit.point;
//		}
//		return Vector3.zero;
//	}
//
//	Transform SelectBuilding()
//	{
//		Ray ray = CityCamera.ScreenPointToRay(Input.mousePosition);
//		RaycastHit hit;
//		if(Physics.Raycast(ray,out hit,Mathf.Infinity,1<<m_BuildingLayer))
//		{
//			return hit.transform;
//		}
//		return null;
//	}
//
//	Transform SelectBuildingBase()
//	{
//		Ray ray = CityCamera.ScreenPointToRay(Input.mousePosition);
//		RaycastHit hit;
//		if(Physics.Raycast(ray,out hit,Mathf.Infinity,1<<m_BuildingBaseLayer))
//		{
//			return hit.transform;
//		}
//		return null;
//	}
//


}
