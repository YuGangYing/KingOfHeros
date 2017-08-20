using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using DataMgr;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
// 建筑
public class Pos2D
{
    public float posX; // 创建的默认位置
    public float posY;

    public Pos2D(float x, float y)
    {
        posX = x;
        posY = y;
    }
}
// 放置建筑
public class PutBuild : MonoBehaviour
{
    public static PutBuild me; // Fake singleton

    public GameObject m_buildNamePanelPre;
    public GameObject m_buildingCDPanelPre;
    public GameObject m_buildProTitlePre;
    public GameObject m_buildSelectedPanelPre;
    public GameObject m_buildMovePanelPre;
    public Material buildingMaterial;

    public Camera camera;
    public Build m_build; // 要放置的建筑
    public GameObject m_objSenceRoot;
    public AudioClip m_audioClick;
	public List<Transform> BuildTrans;

    [HideInInspector]

    void Awake()
    {
        me = this;

        if (m_objSenceRoot == null)
        {
            m_objSenceRoot = GameObject.Find("SceneRoot");
        }
    }

    void Start()
    {
        DataManager.getBuildData().CreatBuildingByBuildList();
        camera = Camera.main;
        string strPath = "Prefabs/UI/960X640/Interface3/buildMoveTitle";
        m_buildMovePanelPre = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPath);

        string strPro = "Prefabs/UI/960X640/Interface3/buildProTitle";
        m_buildProTitlePre = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPro);

        string strMaterial = "Prefabs/Buildings/BuildingProcess";
        Material buildingMaterial = DataMgr.ResourceCenter.LoadAsset<Material>(strMaterial);
    }

    void OnDestroy()
    {
        me = null;
    }

    //客户端主动创建
    public void OnPutBuild(int nBuildType)
    {
        if (m_bMoveBuild)
        {
            return;
        }

        if (m_bCreateBuild)
        {
            m_bCreateBuild = false;
            Destroy(m_selectedBuild.root);
        }

		if (!DataManager.getBuildData().canBuildingNewBuild())
        {
            return;
        }

//         Packet.BUILDING_RET ret = PutBuild.me.CheckBuildOperate(nBuildType, 1);
//         if (ret != Packet.BUILDING_RET.BUILDING_RET_SUC)
//         {
// 			DataManager.getBuildData().ShowBuildOperaRetText(ret);
//             return;
//         }

        //Vector3 pos = RPGCamera.Instance.TargetPosition;
        Vector3 pos = new Vector3(0, 0, 0);
        GameObject objBuild = GetNewBuild(nBuildType, 1, new Pos2D(pos.x, pos.z), false);
        if (objBuild)
        {
            Build buildTemp = objBuild.GetComponent<Build>();

            buildTemp.transform.position = new Vector3(buildTemp.transform.position.x, buildTemp.transform.position.y, buildTemp.transform.position.z);
            buildTemp.m_idBuildingType = (uint)nBuildType;
            buildTemp.m_cbLev = 0;

            //CityCameraControl.instance.SetLookAtPos(buildTemp.transform.position);
            m_selectedBuild = buildTemp;
            m_build = buildTemp;
            m_selectedBuild.SetState(Build.BuildState.Normal);

            m_bCreateBuild = true;
            buildTemp.m_startPosition = buildTemp.transform.position;
            NGUITools.PlaySound(m_audioClick);
            m_selectedBuild.ShowSelectedPanel(true);
            m_selectedBuild.ShowMoveTitlePanel();
            m_bMoveBuild = true;
        }
    }

    //服务器下发创建
    public Build OnPutBuild(Packet.BUILDING_INFO info)
    {
        if (info.cbState == 2 && info.cbLev == 0)
        {
            info.cbLev += 1;
        }

        GameObject objBuild = GetNewBuild((int)info.idBuildingType, (int)info.cbLev, new Pos2D(info.fPosX, info.fPosY));
        if (objBuild)
        {
            Build buildTemp = objBuild.GetComponent<Build>();
            buildTemp.m_idBuilding = info.idBuilding;
            buildTemp.m_idBuildingType = info.idBuildingType;
            buildTemp.m_cbAreaWidth = info.cbAreaWidth;
            buildTemp.m_cbAreaHigh = info.cbAreaHigh;
            buildTemp.m_cbLev = info.cbLev;
            buildTemp.m_cbState = info.cbState;
            buildTemp.m_u32LevyTime = info.u32LevyTime;
            buildTemp.CreateBuildPop();
            //buildTemp.CleanRoundTree();

            if (info.fPosX != (float)buildTemp.m_buildFound)
            {
                // 更正服务器位置
                DataManager.getBuildData().SendMoveBuild(buildTemp.m_idBuilding, (float)buildTemp.m_buildFound, 0);
            }

            return buildTemp;
        }

        return null;
    }

    public Build OnChangeBuild(Build build)
    {
        int nIndex = GetFoundIdByPos(build.transform.position.x, build.transform.position.y, build.transform.position.z);

        GameObject objBuild = GetNewBuild((int)build.m_idBuildingType, (int)build.m_cbLev, new Pos2D((float)nIndex, 0));
        if (objBuild)
        {
            Build buildTemp = objBuild.GetComponent<Build>();
            buildTemp.m_idBuilding = build.m_idBuilding;
            buildTemp.m_idBuildingType = build.m_idBuildingType;
            buildTemp.m_cbAreaWidth = build.m_cbAreaWidth;
            buildTemp.m_cbAreaHigh = build.m_cbAreaHigh;
            buildTemp.m_cbLev = build.m_cbLev;
            buildTemp.m_cbState = build.m_cbState;
            buildTemp.m_u32LevyTime = build.m_u32LevyTime;
            buildTemp.m_typeModel = build.m_typeModel;
            buildTemp.CreateBuildPop();
            DestroyImmediate(buildTemp.root);

            return buildTemp;
        }

        return null;
    }

    GameObject GetNewBuild(int nBuildType, int nLev, Pos2D pos, bool isNeedDetection = true)
    {
        DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig(nBuildType);
        if (config == null)
        {
            Logger.LogError("GetNewBuild index:{0} not find!", nBuildType);
            return null;
        }

        string strPrefab = "Prefabs/Buildings/" + config.prefabs;

        List<Pos2D> posList = new List<Pos2D>();

        if (pos != null)
        {
            posList.Add(pos);
        }

        foreach (Pos2D p in posList)
        {
            Vector3 posBuild = new Vector3(0, 0, 0);
            GameObject prefab2 = null;
            GameObject objBuild = null;

            if (nBuildType == (int)BuildType.LORD_HALL)
            {
                GameObject root = new GameObject();
                root.name = nBuildType.ToString();
                root.transform.parent = m_objSenceRoot.transform;
                strPrefab = "Prefabs/Buildings/EmptyObject";
                prefab2 = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPrefab);
                objBuild = (GameObject)Object.Instantiate(prefab2);
                objBuild.name = "LORD_HALL";
                objBuild.transform.parent = root.transform;
                //主城位置
                objBuild.transform.position = new Vector3(0, -20, -80);
                objBuild.layer = LayerMask.NameToLayer("Building");

                if (objBuild)
                {
                    BoxCollider collider = objBuild.AddComponent<BoxCollider>();
                    //主城碰撞盒位置
                    collider.size = new Vector3(40.0f, 40.0f, 30.0f);
                    collider.center = new Vector3(0.0f, 0.0f, 0.0f);
                    
                    Build buildTemp = objBuild.AddComponent<Build>();
                    buildTemp.m_idBuildingType = (uint)nBuildType;
                    buildTemp.m_cbLev = (byte)nLev;
                    buildTemp.root = root;
                    //buildTemp.m_typeModel = nBuildType;
                    //buildTemp.transform.position = posBuild;
                    return objBuild;
                }

            }
            else if (nBuildType == (int)BuildType.MONUMENT)
            {
                GameObject root = new GameObject();
                root.name = nBuildType.ToString();
                root.transform.parent = m_objSenceRoot.transform;
                strPrefab = "Prefabs/Buildings/EmptyObject";
                prefab2 = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPrefab);
                objBuild = (GameObject)Object.Instantiate(prefab2);
                objBuild.name = "MONUMENT";
                objBuild.transform.parent = root.transform;
                //纪念碑位置
                objBuild.transform.position = new Vector3(0, -17, 8);
                objBuild.layer = LayerMask.NameToLayer("Building");

                if (objBuild)
                {
                    BoxCollider collider = objBuild.AddComponent<BoxCollider>();
                    //纪念碑碰撞盒位置
                    collider.size = new Vector3(5.0f, 15.0f, 5.0f);
                    collider.center = new Vector3(0.0f, 0.0f, 0.0f);

                    Build buildTemp = objBuild.AddComponent<Build>();
                    buildTemp.m_idBuildingType = (uint)nBuildType;
                    buildTemp.m_cbLev = (byte)nLev;
                    buildTemp.root = root;
                    //buildTemp.m_typeModel = nBuildType;
                    //buildTemp.transform.position = posBuild;
                    return objBuild;
                }
            }
            else if (nBuildType == (int)BuildType.WALL)
            {
                GameObject root = new GameObject();
                root.name = nBuildType.ToString();
                root.transform.parent = m_objSenceRoot.transform;
                strPrefab = "Prefabs/Buildings/EmptyObject";
                prefab2 = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPrefab);
                objBuild = (GameObject)Object.Instantiate(prefab2);
                objBuild.name = "WALL";
                objBuild.transform.parent = root.transform;
                //城墙位置
                objBuild.transform.position = new Vector3(35, -22, -22);
                objBuild.layer = LayerMask.NameToLayer("Building");

                if (objBuild)
                {
                    BoxCollider collider = objBuild.AddComponent<BoxCollider>();
                    //城墙碰撞盒位置
                    collider.size = new Vector3(7.0f, 15.0f, 60.0f);
                    collider.center = new Vector3(0.0f, 0.0f, 0.0f);

//                     BoxCollider collider2 = objBuild.AddComponent<BoxCollider>();
//                     //城墙碰撞盒位置2
//                     collider2.size = new Vector3(7.0f, 15.0f, 60.0f);
//                     collider2.center = new Vector3(-70.0f, 0.0f, 0.0f);

                    Build buildTemp = objBuild.AddComponent<Build>();
                    buildTemp.m_idBuildingType = (uint)nBuildType;
                    buildTemp.m_cbLev = (byte)nLev;
                    buildTemp.root = root;
                    //buildTemp.m_typeModel = nBuildType;
                    //buildTemp.transform.position = posBuild;
                    return objBuild;
                }
            }
            else
            {
                prefab2 = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPrefab);
                objBuild = (GameObject)Object.Instantiate(prefab2);
            }

            if (objBuild)
            {
                GameObject root = new GameObject();
                if (BuildTrans != null && BuildTrans.Count > 0 && isNeedDetection)
                {
                    posBuild = BuildTrans[(int)pos.posX].position;
                }

                if (!isNeedDetection)
                {
                    posBuild = GetEmptyFoundPos();
                }

                //root.name = DataManager.getLanguageMgr().getString(config.name);
				root.name = nBuildType.ToString();
                root.transform.parent = m_objSenceRoot.transform;
                //root.transform.position = posBuild;
                objBuild.transform.parent = root.transform;
                objBuild.transform.position = posBuild;
                objBuild.layer = LayerMask.NameToLayer("Building");

                Build buildTemp = objBuild.AddComponent<Build>();
                buildTemp.root = root;
                buildTemp.m_idBuildingType = (uint)nBuildType;
                buildTemp.m_cbLev = (byte)nLev;
                buildTemp.m_buildFound = (uint)pos.posX;
                buildTemp.m_startPosition = posBuild;

                return objBuild;
            }
        }

        return null;
    }

//     public Packet.BUILDING_RET CheckBuildOperate(int idBuildingType, int lev)
//     {
//         DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)idBuildingType);
// 
//         if (config == null)
//         {
//             // 找不到则交给服务器验证
//             return Packet.BUILDING_RET.BUILDING_RET_SUC;
//         }
// 
//         long lCoin = DataManager.getUserData().Data.coin;
//         long lStone = DataManager.getUserData().Data.stone;
// 
//         if (lev > config.levels.Length - 1)
//         {
//             return Packet.BUILDING_RET.BUILDING_RET_MAX_LEV;
//         }
//         else if (idBuildingType != (uint)BuildType.LORD_HALL && lev > DataManager.getBuildData().GetBuildLev(DataMgr.BuildType.LORD_HALL))
//         {
//             return Packet.BUILDING_RET.BUILDING_RET_CB_LEV;
//         }
//         else if (lCoin < config.levels[lev].money)
//         {
//             return Packet.BUILDING_RET.BUILDING_RET_MONEY;
//         }
//         else if (lStone < config.levels[lev].magicStone)
//         {
//             return Packet.BUILDING_RET.BUILDING_RET_SMONEY;
//         }
//         else
//         {
// 			uint deadTime = DataManager.getQueueData().GetDueTime(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD);
//             float fDueTime = (float)DataManager.getTimeServer().EstimateServerTime((long)deadTime);
// 
//             if (fDueTime > 0)
//             {
//                 return Packet.BUILDING_RET.BUILDING_RET_NOT_IDLE_QUEUE;
//             }
//         }
// 
//         return Packet.BUILDING_RET.BUILDING_RET_SUC;
//     }

    void OnEnable()
    {
        EasyTouch.On_TouchDown += OnTouchDown;
        EasyTouch.On_TouchUp += OnTouchUp;
        EasyTouch.On_Drag += OnDrag;
    }

    void OnDisible()
    {
        EasyTouch.On_TouchDown -= OnTouchDown;
        EasyTouch.On_TouchUp -= OnTouchUp;
        EasyTouch.On_Drag -= OnDrag;
    }

    public bool m_bCreateBuild = false;
    public bool m_bMoveBuild = false;
    bool m_bDown = false;
    bool m_bDrag = false;
    bool m_bSet = false; // 当前位置是否允许建造
    public Build m_selectedBuild; 

    void OnTouchUp(Gesture gesture)
    {
        if (PanelManage.me.IsHaveShowPanel())
        {
            return;
        }

        if (UICamera.hoveredObject != null)
        {
            return;
        }

        if (m_bDrag == true)
        {
            if (m_selectedBuild != null && m_build != null)
            {
                if (m_bSet == true)
                {
                    m_bMoveBuild = true;
                    m_selectedBuild.ShowMoveTitlePanel();
                    m_selectedBuild.SetState(Build.BuildState.Normal);
                    m_selectedBuild.Reposition(GetCurBuildBasePos());
                }
                else
                {
                    if (m_bCreateBuild && isHaveBuildCollider())
                    {
                        m_selectedBuild.SetState(Build.BuildState.Error);
                    }
                    else
                    {
                        m_selectedBuild.Reposition(m_selectedBuild.m_startPosition);
                        m_selectedBuild.SetState(Build.BuildState.Normal);
                    }
                }
            }
        }
        else
        {
            if (gesture.pickObject != null)
            {
                if (gesture.pickObject.layer == LayerMask.NameToLayer("Building"))
                {
                    Build build;
//                     if (gesture.pickObject.name == "wall")
//                     {
//                         build = gesture.pickObject.transform.parent.parent.GetComponent<Build>();
//                     }
//                     else
//                     {
//                         build = gesture.pickObject.GetComponent<Build>();
//                     }
                    build = gesture.pickObject.GetComponent<Build>();

                    if (build != null)
                    {
                        if (!m_bMoveBuild && !m_bCreateBuild && m_selectedBuild == null)
                        {
                            NGUITools.PlaySound(m_audioClick);
                            SLG.GlobalEventSet.FireEvent(SLG.eEventType.ClickBuild, PanelID.BuildInfoPanel, new SLG.EventArgs(build));
                        }

                        build.SetState(Build.BuildState.Normal);
                    }
                }
                else if (!m_bMoveBuild)
                {
                    PanelBase panelBase = PanelManage.me.getPanel(PanelID.BuildInfoPanel);
                    if (panelBase != null && panelBase.IsVisible())
                    {
                        panelBase.SetVisible(false);
                    }

                    if (m_bCreateBuild)
                    {
                        m_bCreateBuild = false;
                        m_bMoveBuild = false;
                        Destroy(m_selectedBuild.transform.parent.gameObject);
                    }

                    //m_selectedBuild = null;
                }
            }
        }

        m_build = null;
        m_bDrag = false;
        m_bDown = false;
    }

    void OnTouchDown(Gesture gesture)
    {
        if (PanelManage.me.IsHaveShowPanel())
        {
            return;
        }

        if (UICamera.hoveredObject != null)
        {
            return;
        }

        //移动
//         if (this.m_selectedBuild != null && this.m_selectedBuild.m_buildMove!=null && this.m_bMoveBuild)
//         {
//             GameObject cancelBtn = UI.PanelTools.FindChild(this.m_selectedBuild.m_buildMove, "Cancel");
//             if (cancelBtn != null)
//             {
//                 UIEventListener.VoidDelegate func = UIEventListener.Get(cancelBtn).onClick;
//                 if(func!=null)
//                 {
//                     func(cancelBtn);
//                 }
//             }
//         }

        if (this.m_selectedBuild != null && gesture.pickObject.name != m_selectedBuild.name)
        {
            m_selectedBuild.MoveCancel();
        }

        if (gesture.pickObject == null)
        {
            return;
        }

        if (m_build == null && EasyTouch.GetTouchCount() == 1)
        {
            m_bDown = true;

            if (gesture.pickObject.layer == LayerMask.NameToLayer("Building"))
            {
                Build build;
                build = gesture.pickObject.GetComponent<Build>();

                if (build == null)
                {
                    Debug.Log("OnTouchDown::Rouch Error Object");
                    return;
                }

                if (build == m_selectedBuild)
                {
                    m_build = build;

                    if (!m_bMoveBuild)
                    {
                        m_selectedBuild.m_startPosition = m_selectedBuild.transform.position;
                    }

                    if (m_bCreateBuild && !m_selectedBuild.IsCanPutBuild())
                    {
                        m_selectedBuild.SetState(Build.BuildState.Error);
                    }
                    else
                    {
                        m_selectedBuild.SetState(Build.BuildState.Move);
                    }
                }
            }
            else if (gesture.pickObject.layer == LayerMask.NameToLayer("UIView"))
            {
                UICamera.Notify(gesture.pickObject, "OnClick", null);

                return;
            }

            if (m_selectedBuild == null && m_bDrag == false && m_build == null && gesture.pickObject != m_selectedBuild
                && gesture.pickObject.layer == LayerMask.NameToLayer("BuildingBaseLayer"))
            {
                //点击基座处理
                if (camera == null)
                {
                    camera = Camera.main;
                }

                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(gesture.position);

                if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("UI"))
                    || Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("UIView")))
                {
                    return;
                }

                if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("BuildingBaseLayer")))
                {
                    isSelectedEmpryFound = false;
                    bool hasBuild = false;
                    Bounds bounds = hit.collider.bounds;
                    Vector3 center = bounds.center;
                    float radio = 1.0f;
                    Collider[] colliders = Physics.OverlapSphere(center, radio);

                    foreach (Collider c in colliders)
                    {
                        if (c.gameObject.layer == LayerMask.NameToLayer("Building"))
                        {
                            hasBuild = true;
                        }
                    }

                    if (!hasBuild)
                    {
                        isSelectedEmpryFound = true;
                        SelectedFoundPos = center;
                        PanelBase panelBase = PanelManage.me.getPanel(PanelID.BuildMallPanel);
                        if (panelBase != null)
                        {
                            panelBase.ToggleVisible();
                            if (panelBase.IsVisible())
                                DataManager.getBuildData().SetShowToPanel();
                        }
                    }
                }


            }
            
        }
    }

    void OnDrag(Gesture gesture)
    {
        m_bDrag = true;

        if (m_build == null || m_selectedBuild == null)
        {
            return;
        }

        if (m_selectedBuild.m_cbState == 2)
        {
			//DataManager.getBuildData().ShowBuildOperaRetText(Packet.BUILDING_RET.BUILDING_RET_BUILDING_STATUS);
            return;
        }

        DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_selectedBuild.m_idBuildingType);

        if (config == null || !config.isMove)
        {
			//DataManager.getBuildData().ShowBuildOperaRetText(Packet.BUILDING_RET.BUILDING_RET_UNMOVEABLE);
            return;
        }

        if (camera == null)
        {
            camera = Camera.main;
        }

        Ray ray = camera.ScreenPointToRay(gesture.position);
        RaycastHit hit;

        int nTerrain = LayerMask.NameToLayer("Terrain");

        if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Terrain")) == false
            && Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("BuildingBaseLayer")) == false)
        {
            //Debug.LogError("OnDrag::move build error: not find Terrain");
            return;
        }

        Build.BuildState state = Build.BuildState.Move;
        m_bSet = true;

        if (isHaveBuildCollider())
        {
            state = Build.BuildState.Error;
            m_bSet = false;
        }

        if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("BuildingBaseLayer")))
        {
            m_selectedBuild.Reposition(new Vector3(hit.point.x, hit.point.y + 0.07f, hit.point.z));
            
        }
        else if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Terrain")))
        {
            state = Build.BuildState.Error;
            m_bSet = false;
            m_selectedBuild.Reposition(new Vector3(hit.point.x, hit.point.y, hit.point.z));
        }

        GridItem.eValueType type = TerrainLayer.me.GetPosValue((BuildType)m_selectedBuild.m_idBuildingType, m_selectedBuild.transform.position);

        if (type == GridItem.eValueType.Null || type == m_selectedBuild.m_typeModel)
        {
            m_selectedBuild.SetState(state);
            return;
        }

        m_selectedBuild.m_typeModel = type;

        //m_selectedBuild = BuildMgr.me.ChangeBuild(m_selectedBuild);

        m_selectedBuild.SetState(state);
    }

    protected bool isHaveBuildCollider()
    {
        Bounds bounds = m_selectedBuild.m_collider.bounds;
        Vector3 center = bounds.center;
        Vector3 size = bounds.size;
        float radio = Mathf.Max(size.x, size.y);
        Collider[] colliders = Physics.OverlapSphere(center, radio);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Building") && c != m_selectedBuild.GetComponent<Collider>())
            {
                if (bounds.Intersects(c.bounds) && c.bounds.Intersects(bounds))
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected Vector3 GetCurBuildBasePos()
    {
        Bounds bounds = m_selectedBuild.m_collider.bounds;
        Vector3 center = bounds.center;
        float radio = 1.0f;
        Collider[] colliders = Physics.OverlapSphere(center, radio);
        
        Vector3 basePos = new Vector3(0.0f, 0.0f, 0.0f);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("BuildingBaseLayer"))
            {
                return c.bounds.center;
            }
        }

        return basePos;
    }

    public int GetFoundIdByPos(float x, float y, float z)
    {
        int nIndex = 0;
        Vector3 center = new Vector3(x, y, z);
        float radio = 1.0f;
        Collider[] colliders = Physics.OverlapSphere(center, radio);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("BuildingBaseLayer"))
            {
                for (nIndex = 0; nIndex < BuildTrans.Count; ++nIndex)
                {
                    if (c.bounds.center == BuildTrans[nIndex].GetComponent<Collider>().bounds.center)
                    {
                        return nIndex;
                    }
                }
            }
        }

        return nIndex;
    }

    public int GetBuildFoundByPos()
    {
        Vector3 v3 = GetCurBuildBasePos();
        for (int i = 0; i < BuildTrans.Count; ++i)
        {
            if (BuildTrans[i].GetComponent<Collider>().bounds.center == v3)
            {
                return i;
            }
        }
        return 0;
    }

    Vector3 SelectedFoundPos;
    bool isSelectedEmpryFound = false;

    public Vector3 GetEmptyFoundPos()
    {

        if (isSelectedEmpryFound)
        {
            isSelectedEmpryFound = false;
            return SelectedFoundPos;
        }
        
        Vector3 foundPos = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < BuildTrans.Count; ++i)
        {
            bool hasBuild = false;
            Bounds bounds = BuildTrans[i].GetComponent<Collider>().bounds;
            Vector3 center = bounds.center;
            float radio = 1.0f;
            Collider[] colliders = Physics.OverlapSphere(center, radio);

            foreach (Collider c in colliders)
            {
                if (c.gameObject.layer == LayerMask.NameToLayer("Building"))
                {
                    hasBuild = true;
                }
            }

            if (!hasBuild)
            {
                return center;
            }

        }

        return foundPos;
    }

    
}
