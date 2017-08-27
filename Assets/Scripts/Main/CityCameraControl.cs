using UnityEngine;
using System.Collections;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108

public class CityCameraControl : MonoBehaviour
{
    public static CityCameraControl instance; // Fake singleton
    public Camera camera; // 相机
    public BoxCollider moveArea;    // the area to constrain camera movement to
    public float fScrollScale = 100;

    public Vector3 ConstrainToMoveArea(Vector3 p)
    {
        if (bounds.size != Vector3.zero)
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            p.x = Mathf.Clamp(p.x, min.x, max.x);
            p.y = Mathf.Clamp(p.y, min.y, max.y);
            p.z = Mathf.Clamp(p.z, min.z, max.z);
        }

        return p;
    }

    void Awake()
    {
        instance = this;
    }

    Bounds bounds;
    // Use this for initialization
    void Start()
    {
        if (moveArea != null)
        {
            bounds = moveArea.bounds;
            Destroy(moveArea);
        }
        else
        {
            bounds.size = Vector3.zero;
        }

        GameObject glo = GameObject.Find("CameraShow_Grp");
        if (glo)
        {
            glo.SetActive(MainController.me.showCameraPath);
            MainController.me.showCameraPath = false;
            glo.SendMessage("SwitchCamera", 1);
        }
    }

    // 得到地形的高度
    public Vector3 TerrainPosition(Vector3 position)
    {
        //         RaycastHit hitInfo;
        //         if (Physics.Raycast(new Vector3(position.x, 100f, position.z), Vector3.down, out hitInfo, 10000f, 1 << LayerMask.NameToLayer("Terrain")))
        //             return hitInfo.point;
        return position;
    }

    void OnEnable()
    {
#if (((UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_BLACKBERRY) && !UNITY_EDITOR))
        EasyTouch.On_Drag += OnDrag;
        EasyTouch.On_PinchIn += OnPinchIn;
        EasyTouch.On_PinchOut += OnPinchOut;
        EasyTouch.On_DragStart+= OnDragStart;
#endif
    }

    void OnDisable()
    {
#if (((UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_BLACKBERRY) && !UNITY_EDITOR))
        EasyTouch.On_Drag -= OnDrag;
        EasyTouch.On_PinchIn -= OnPinchIn;
        EasyTouch.On_PinchOut -= OnPinchOut;
        EasyTouch.On_DragStart -= OnDragStart;
#endif
    }

#if (!((UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_BLACKBERRY) && !UNITY_EDITOR))
    private Vector3 oldMousePosition = Vector3.zero;
    private bool bDraging = false;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            GameObject glo = GameObject.Find("CameraShow_Grp");
            if (glo)
            {
                glo.SetActive(false);
            }
        }
		//ddd
        if (PutBuild.me.m_build != null || UI.PanelManage.me.IsHaveShowPanel())
            return;

        if (Input.GetMouseButton(1))
        {
            CameraShowArea.Instance.isReset = false;
            if (bDraging == false)
            {
                DragStart(Input.mousePosition);
                bDraging = true;
            }
            else
                DragMove(Input.mousePosition);
        }
        else
        {
            bDraging = false;
            oldMousePosition.x = oldMousePosition.y = oldMousePosition.z = -1f;
            CameraShowArea.Instance.isReset = true;
        }

        float fScroll = Input.GetAxis("Mouse ScrollWheel");
        if (fScroll != 0)
        {
            float RealDistance = RPGCamera.Instance.RealDistance;
            RealDistance -= fScroll * PinchScale;
            RPGCamera.Instance.SetDistance(RealDistance);
        }
    }
#endif

    //void UpdateMove(Vector2 deltaPosition)
    //{
    //    if (RPGCamera.Instance.RealDistance >= RPGCamera.Instance.MaxDistance)
    //        return;

    //    deltaPosition = deltaPosition * -DragScale;
    //    Transform lookAtPoint = RPGCamera.Instance.Target;
    //    Vector3 tmpPos = lookAtPoint.position;
    //    lookAtPoint.rotation = RPGCamera.Instance.transform.rotation;
    //    lookAtPoint.Translate(deltaPosition.x, deltaPosition.y, 0f, Space.Self);
    //    tmpPos.x = lookAtPoint.position.x;
    //    tmpPos.z = lookAtPoint.position.z;

    //    float y = tmpPos.y;
    //    tmpPos = ConstrainToMoveArea(tmpPos);
    //    tmpPos.y = y;

    //    lookAtPoint.position = tmpPos;
    //}

    Camera TempCamera;
    Vector3 TargetStartPosition;
    Vector3 StartPostion;
    bool bResetDrag = false;
    void DragStart(Vector3 startPos)
    {
        Camera camera = CameraShowArea.Instance.Camera;
        if (TempCamera == null)
        {
            TempCamera = (new GameObject("TempCamera")).AddComponent<Camera>();
            TempCamera.gameObject.SetActive(false);
            TempCamera.transform.parent = camera.transform.parent;
        }
        TempCamera.CopyFrom(camera);

        Ray ray = camera.ScreenPointToRay(startPos);
        RaycastHit info;
        if (Physics.Raycast(ray, out info, float.MaxValue, 1 << LayerMask.NameToLayer("moveTerrain")) == false)
        {
            return;
        }

        TargetStartPosition = RPGCamera.Instance.Target.position;
        StartPostion = info.point;
        bResetDrag = false;
    }
    public void SetLookAtPos(Vector3 pos)
    {
        Transform lookAtPoint = RPGCamera.Instance.Target;
        pos.y = lookAtPoint.position.y;

        lookAtPoint.position = pos;
    }

    void DragMove(Vector3 newPosition)
    {
        float RealDistance = RPGCamera.Instance.RealDistance;
        if (RealDistance >= RPGCamera.Instance.MaxDistance)
            return;

        Ray ray = TempCamera.ScreenPointToRay(newPosition);
        RaycastHit info;
        if (Physics.Raycast(ray, out info, float.MaxValue, 1 << LayerMask.NameToLayer("moveTerrain")) == false)
            return;
        if (CameraShowArea.Instance.checkArea())
            RPGCamera.Instance.Target.transform.position = TargetStartPosition + StartPostion - info.point;
    }

    void OnDragStart(Gesture gesture)
    {
        DragStart(gesture.position);
    }

    void OnDrag(Gesture gesture)
    {
        if (PutBuild.me.m_build != null)
            return;

        if (gesture.touchCount == 1)
        {
            if (bResetDrag)
            {
                DragStart(gesture.position);
                return;
            }

            DragMove(gesture.position);
        }
        else
        {
            bResetDrag = true;
        }

        //         Vector3 oldPosition = camera.transform.position;
        //         camera.transform.Translate(deltaPosition.x, deltaPosition.y, 0f, Space.Self);
        //         if (camera.transform.position != ConstrainToMoveArea(camera.transform.position))
        //         {
        //             camera.transform.position = oldPosition;
        //         }
    }

    float PinchScale
    {
        get
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo))
            {
                return distance_scale_pinch.Evaluate(hitInfo.distance) * fScrollScale;
            }

            return fScrollScale;
        }
    }

    float DragScale
    {
        get
        {
            //             RaycastHit hitInfo;
            //             if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo))
            //             {
            //                 return distance_scale_drag.Evaluate(hitInfo.distance);
            //             }
            // 
            //             return 1f;
            float dis = RPGCamera.Instance.RealDistance;
            if (dis < 10)
            {
                dis = 10;
            }

            return dis / RPGCamera.Instance.MaxDistance;
        }
    }

    public AnimationCurve distance_scale_pinch;
    public AnimationCurve distance_scale_drag;

    void OnPinchIn(Gesture gesture)
    {
        if (PutBuild.me.m_build != null)
            return;

        ConsoleSelf.me.addText("OnPinchIn(Gesture gesture)");
        float RealDistance = RPGCamera.Instance.RealDistance;
        RealDistance += gesture.deltaPinch;
        RPGCamera.Instance.SetDistance(RealDistance);

        //         Vector3 oldPosition = camera.transform.position;
        //         camera.transform.Translate(0f, 0f, -gesture.deltaPinch * PinchScale, Space.Self);
        //         if (camera.transform.position != ConstrainToMoveArea(camera.transform.position))
        //         {
        //             camera.transform.position = oldPosition;
        //         }
    }

    void OnPinchOut(Gesture gesture)
    {
        if (PutBuild.me.m_build != null)
            return;

        ConsoleSelf.me.addText("OnPinchOut(Gesture gesture)");
        float RealDistance = RPGCamera.Instance.RealDistance;
        RealDistance -= gesture.deltaPinch;
        RPGCamera.Instance.SetDistance(RealDistance);

        //         Vector3 oldPosition = camera.transform.position;
        //         camera.transform.Translate(0f, 0f, gesture.deltaPinch * PinchScale, Space.Self);
        //         if (camera.transform.position != ConstrainToMoveArea(camera.transform.position))
        //         {
        //             camera.transform.position = oldPosition;
        //         }
    }
}
