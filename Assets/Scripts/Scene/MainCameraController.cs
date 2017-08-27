using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MainCameraController : MonoBehaviour 
{
    public Camera _mainCamera = null;
    public List<GameObject> AreaPolyPoints = new List<GameObject>();
    public GameObject lookAtObj;
    
    string _strTerrain = "moveTerrain";
    
    bool _isMove = false;
    Vector3 _startMovePt = Vector3.zero;
    Vector3 _startCameraPt = Vector3.zero;
    Camera _TempCamera = null;
    
    int haveCount = 0;
    List<Vector3> _AreaPolyVec3 = new List<Vector3>();
    Vector3 _prePoint = Vector3.zero;

    void Start () 
    {
        initList();
	}

    void initList()
    {
        _AreaPolyVec3.Clear();
        foreach (GameObject item in this.AreaPolyPoints)
        {
            if (item != null)
                _AreaPolyVec3.Add(item.transform.position);
        }
    }

    public void OnDrawGizmos()
    {
        GameObject begin =null;
        GameObject prePt = null;//前一个点
        GameObject end = null;

        if(lookAtObj==null)
            return;

        foreach (GameObject item in this.AreaPolyPoints)
        {
            if (item == null)
                continue;
            item.transform.position = new Vector3(item.transform.position.x,lookAtObj.transform.position.y,item.transform.position.z);
            end = item;
            if (begin == null)
            {
                begin = item;
            }
            else if (prePt!=null)
            {
                Gizmos.DrawLine(prePt.transform.position, item.transform.position);
            }
            prePt = item;
            haveCount++;
        }
		//ddd
        if(haveCount >= 2)
            Gizmos.DrawLine(begin.transform.position, end.transform.position);
		
    }


#if (((UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_BLACKBERRY) && !UNITY_EDITOR))
    void OnEnable()
    {
         EasyTouch.On_Drag += onDrag;
         EasyTouch.On_DragStart += onDragStart;
         EasyTouch.On_DragEnd += onDragEnd;
         EasyTouch.On_PinchIn += OnPinchIn;
         EasyTouch.On_PinchOut += OnPinchOut;
    }

    void OnDisable()
    {
         EasyTouch.On_Drag -= onDrag;
         EasyTouch.On_DragStart -= onDragStart;
         EasyTouch.On_DragEnd -= onDragEnd;
         EasyTouch.On_PinchIn -= OnPinchIn;
         EasyTouch.On_PinchOut -= OnPinchOut;
    }
#else
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (!this._isMove)
                this.onDragStart(Input.mousePosition);
            else
                this.onDrag(Input.mousePosition);
        }
        else
        {
            if (!this.isMoveValid() && this._prePoint != Vector3.zero)
                RPGCamera.Instance.Target.transform.position = this._prePoint;
            this.onDragEnd();
        }
        float fScroll = Input.GetAxis("Mouse ScrollWheel");
        if (fScroll != 0)
            onScroll(-fScroll * 20);
    }
#endif

    Vector3 getLookAtPoint()
    {
        return Vector3.zero;
    }

    //拖拉
    void onDragStart(Gesture gesture)
    {
        onDragStart(gesture.position);
    }

    void onDragStart(Vector3 point)
    {
        if (!isCanOper())
            return;
        if (_TempCamera == null)
        {
            _TempCamera = (new GameObject("TempCamera")).AddComponent<Camera>();
            _TempCamera.gameObject.SetActive(false);
            _TempCamera.transform.parent = this._mainCamera.transform.parent;
        }

        if (!isMoveValid())
        {
            this.onDragEnd();
            return;
        }

        this._TempCamera.CopyFrom(this._mainCamera);
        this._isMove = true;
        this._startCameraPt = RPGCamera.Instance.Target.transform.position;
        this._startMovePt = this.getTerrainPt(this._TempCamera,point);
    }

    void onDrag(Gesture gesture)
    {
        onDrag(gesture.position);
    }

    void onDrag(Vector3 point)
    {
        if (!isCanOper())
        {
            this.onDragEnd();
            return;
        }
        if (this._isMove)
        {
            if (this.isMoveValid())
            {
                this._prePoint = RPGCamera.Instance.Target.transform.position;
                Vector3 movePt = getTerrainPt(this._TempCamera, point);
                onMove(movePt);
            }
            else
                this.onDragEnd();
        }
    }

    //移动点是否有效
    bool isMoveValid()
    {
        if (this.lookAtObj == null)
            return false;
        if (this.ContainsPoint(this._AreaPolyVec3.ToArray(), this.lookAtObj.transform.position))
            return true;
        return false;
    }

    void onDragEnd(Gesture gesture)
    {
        if (!this.isMoveValid() && this._prePoint != Vector3.zero)
            RPGCamera.Instance.Target.transform.position = this._prePoint;
        onDragEnd();
    }

    void onDragEnd()
    {
        this._isMove = false;
        this._startMovePt = Vector3.zero;
        this._startCameraPt = Vector3.zero;
    }

    void OnPinchIn(Gesture gesture)
    {
        onScroll(gesture.deltaPinch * 0.2f);
    }

    void OnPinchOut(Gesture gesture)
    {
        onScroll(-gesture.deltaPinch * 0.2f);
    }

    //判断是否可以操作
    bool isCanOper()
    {
        if (PutBuild.me.m_build != null)
            return false;
        if (UI.PanelManage.me.IsHaveShowPanel())
            return false;
        if (UICamera.hoveredObject != null)
            return false;
        return true;
    }

    //缩放
    void onScroll(float value)
    {
        if (!isCanOper())
            return;
        float RealDistance = RPGCamera.Instance.RealDistance;
        RealDistance += value;
        RPGCamera.Instance.SetDistance(RealDistance);
    }
    
    //拖拉
    void onMove(Vector3 value)
    {
        //_mainCamera.transform.position = this._startCameraPt + this._startMovePt - value;
        RPGCamera.Instance.Target.transform.position = this._startCameraPt + this._startMovePt - value;
    }

    Vector3 getTerrainPt(Camera camera,Vector3 srcPt)
    {
        if (camera == null)
            return Vector3.zero;
        Ray ray = camera.ScreenPointToRay(srcPt);
        RaycastHit info;
        if (Physics.Raycast(ray, out info, float.MaxValue, 1 << LayerMask.NameToLayer(_strTerrain)) == false)
            return Vector3.zero;
        return info.point;
    }

    //判断点是否在区域内
    bool ContainsPoint(Vector3[] polyPoints, Vector3 p)
    {
        int j = polyPoints.Length - 1;
        bool inside = false;

        for (int i = 0; i < polyPoints.Length; j = i++)
        {
            if (((polyPoints[i].z <= p.z && p.z < polyPoints[j].z) || (polyPoints[j].z <= p.z && p.z < polyPoints[i].z)) &&
               (p.x < (polyPoints[j].x - polyPoints[i].x) * (p.z - polyPoints[i].z) / (polyPoints[j].z - polyPoints[i].z) + polyPoints[i].x))
                inside = !inside;
        }
        return inside;
    }
}
