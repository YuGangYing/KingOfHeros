using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Packet;
using UI;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
#pragma warning disable 0114
#pragma warning disable 3021
public class Build : MonoBehaviour
{
    public enum BuildState
    {
        Normal,
        Move,
        Error,
    }

    public GameObject root;

    public uint m_idBuilding;
    public uint m_idBuildingType;
    public byte m_cbLev;
    public byte m_cbAreaWidth;
    public byte m_cbAreaHigh;
    public byte m_cbState; // 1为正常状态，2为建筑中
    public uint m_u32LevyTime;
    public uint m_buildFound;       //基座编号

    public Vector3 m_startPosition; // 用于移动失败 返回位置用

    public GridItem.eValueType m_typeModel;
    public Collider m_collider;
    public GameObject ConstructPre = null;
    public GameObject ConstructBuild = null;
    BuildState m_state;

    private Dictionary<GameObject, List<Material>> m_dicMaterials = new Dictionary<GameObject, List<Material>>();
    void Awake()
    {
        m_state = BuildState.Normal;
        m_collider = GetComponent<Collider>();
    }

    float m_fTitlePopTime;

    void Update()
    {
        Reposition(transform.position);
        float deltaTime = Time.deltaTime;

        if (m_objPop != null)
        {
            if (!m_objPop.activeSelf)
            {
                m_fTitlePopTime += deltaTime;

                if (m_fTitlePopTime > 60)
                {
                    //BuildConfig config = CsvConfigMgr.me.GetBuildConfig((int)m_idBuildingType);
                    //DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_idBuildingType);
                    float time = -(float)DataManager.getTimeServer().EstimateServerTime((long)m_u32LevyTime);
                    //int showTime = config.levels[config.levels.Length - 1].data[1];

                    //5分钟提示可以采
                    if (time > 5 * 60)
                    {
                        m_fTitlePopTime = 0;
                        m_objPop.SetActive(true);
                    }
                }
            }
        }
    }

    GameObject m_buildName = null;
    public void CreateNamePanel(string name)
    {
        if (m_buildName != null)
        {
            BuildNamePanel namePanple = m_buildName.GetComponent<BuildNamePanel>();

            namePanple.SetBuildName(name);
            return;
        }

        if (PutBuild.me.m_buildNamePanelPre != null)
        {
            m_buildName = (GameObject)Object.Instantiate(PutBuild.me.m_buildNamePanelPre);
            m_buildName.transform.parent = root.transform;
            Bounds bounds = m_collider.bounds;
            float offset = bounds.size.y + bounds.center.y;
            m_buildName.transform.localPosition = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
            BuildNamePanel namePanple = m_buildName.GetComponent<BuildNamePanel>();
            if (namePanple != null)
            {
                namePanple.SetBuildName(name);
            }
        }
    }

    GameObject m_buildingCD = null;
    public void ShowBuildingCDPanel()
    {
		uint id = DataManager.getQueueData().GetProgressID(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD);

//         if (m_idBuildingType == (uint)BuildType.WALL || id != m_idBuilding)
//         {
//             return;
//         }
        if (id != m_idBuilding)
        {
            return;
        }

        if (m_buildingCD != null)
        {
            BuildingCDPanel panel = m_buildingCD.GetComponent<BuildingCDPanel>();
            DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_idBuildingType);
            panel.SetInfo(config.levels[m_cbLev + 1].time);
            return;
        }

        if (PutBuild.me.m_buildingCDPanelPre != null)
        {
            Bounds bounds = m_collider.bounds;
            float offset = bounds.size.y + bounds.center.y;
            m_buildingCD = (GameObject)Object.Instantiate(PutBuild.me.m_buildingCDPanelPre);
            m_buildingCD.transform.parent = PanelManage.me.getRoot().transform;
            m_buildingCD.transform.localScale = Vector3.one;

            BuildingCDPanel panel = m_buildingCD.GetComponent<BuildingCDPanel>();
            DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_idBuildingType);
            panel.SetInfo(config.levels[m_cbLev + 1].time);
        }
    }

    GameObject m_objPop = null;
    public void CreateBuildPop()
    {
        if (m_idBuildingType != (uint)BuildType.STONE && m_idBuildingType != (uint)BuildType.MINT && PutBuild.me.m_buildProTitlePre != null)
        {
            return;
        }

        Bounds bounds = m_collider.bounds;
        float offset = bounds.size.y + bounds.center.y;
        m_objPop = (GameObject)Object.Instantiate(PutBuild.me.m_buildProTitlePre);
        m_objPop.transform.parent = root.transform;
        m_objPop.transform.localPosition = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
        m_objPop.transform.localRotation = Quaternion.identity;
        m_objPop.AddComponent<LookAtCamera>();

        UIEventListener.Get(m_objPop).onClick = HidePopObj;
        UISprite icon = m_objPop.GetComponentInChildren<UISprite>();

        if (m_idBuildingType == (uint)BuildType.MINT)
        {
            icon.spriteName = "goldMineFull";
        }
        else if (m_idBuildingType == (uint)BuildType.STONE)
        {
            icon.spriteName = "magicStoneFull";
        }

		DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_idBuildingType);
        //float time = -(float)GameInstance.Game.Sgt.EstimateServerTime((long)m_u32LevyTime);
		float time = -(float)DataManager.getTimeServer().EstimateServerTime((long)m_u32LevyTime);
        //int showTime = config.levels[config.levels.Length - 1].data[1];

        if (time > 5 * 60)
        {
            m_objPop.SetActive(true);
        }
        else
        {
            m_objPop.SetActive(false);
        }
    }

    void OnDestroy()
    {
        Debug.Log("build OnDestroy");
    }

    public bool isShowPop()
    {
        if (m_objPop != null)
        {
            return m_objPop.activeSelf;
        }

        return false;
    }

    public void HidePopObj(GameObject go)
    {
        m_fTitlePopTime = 0;
        m_objPop.SetActive(false);
		DataManager.getBuildData().SendBuildLevy(m_idBuilding);
    }

    public void AddNpc()
    {
        List<Vector3> targets = new List<Vector3>();
        DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_idBuildingType);
        float x = config.sizeX;
        float y = config.sizeY;

        for (int i = 0; i < 4; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + x, 2, transform.position.z + y);
            x = -x;

            if (i == 1)
            {
                y = -y;
            }

            targets.Add(pos);
        }

//         string strGirl = "Assets/Data/ArtResources/Models/Prefabs/NPC/" + "girl" + ".prefab";
//         string strBoy = "Assets/Data/ArtResources/Models/Prefabs/NPC/" + "boy" + ".prefab";
// 
//         GameObject girl = GameObject.Instantiate(ResourcesMgr.LoadAsset<GameObject>(strGirl)) as GameObject;
//         NpcAI girlAI = girl.AddComponent<NpcAI>();
//         girlAI.Init(targets, 0);
//         girl.transform.parent = root.transform;
// 
//         GameObject boy = GameObject.Instantiate(ResourcesMgr.LoadAsset<GameObject>(strBoy)) as GameObject;
//         NpcAI boyAI = boy.AddComponent<NpcAI>();
//         boyAI.Init(targets, 2);
//         boy.transform.parent = root.transform;
    }

    GameObject m_Selected = null;
    public void ShowSelectedPanel(bool bShow)
    {
        DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_idBuildingType);
        if (!config.isMove)
        {
            return;
        }

        if (m_Selected != null)
        {
            m_Selected.SetActive(bShow);
            return;
        }

        if (PutBuild.me.m_buildSelectedPanelPre != null)
        {
            m_Selected = (GameObject)Object.Instantiate(PutBuild.me.m_buildSelectedPanelPre);

            m_Selected.transform.parent = root.transform;
            m_Selected.transform.position = transform.position;
            float x = config.sizeX + 1;
            float y = config.sizeY + 1;

            if (m_Selected.transform.childCount < 4)
            {
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                Vector3 pos;
                if (i <= 1)
                {
                    pos = new Vector3(-x, 0, 0);
                    x = -x;
                }
                else
                {
                    pos = new Vector3(0, -y, 0);
                    y = -y;
                }

                Transform icon = m_Selected.transform.GetChild(i);

                if (icon)
                {
                    icon.localPosition = pos;
                }
            }

            m_Selected.transform.rotation = Quaternion.Euler(90, 0, 0);
            m_Selected.SetActive(bShow);
        }
    }

    public GameObject m_buildMove = null;
    public void ShowMoveTitlePanel()
    {
        DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_idBuildingType);

        if (config == null || !config.isMove || m_cbState == 2)
        {
            PutBuild.me.m_selectedBuild = null;
            return;
        }

        if (m_buildMove != null)
        {
            m_buildMove.SetActive(true);
            PutBuild.me.m_selectedBuild = this;
            PutBuild.me.m_bMoveBuild = true;
            return;
        }

        if (PutBuild.me.m_buildMovePanelPre != null)
        {
            m_buildMove = (GameObject)Object.Instantiate(PutBuild.me.m_buildMovePanelPre);
            m_buildMove.transform.parent = PanelManage.me.getRoot().transform;
            m_buildMove.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            m_buildMove.transform.localRotation = Quaternion.identity;

            UIEventListener.Get(PanelTools.FindChild(m_buildMove, "OK")).onClick = MoveOKClick;
            UIEventListener.Get(PanelTools.FindChild(m_buildMove, "Cancel")).onClick = MoveCancelClick;

            m_startPosition = transform.position;
            PutBuild.me.m_selectedBuild = this;
            PutBuild.me.m_bMoveBuild = true;
        }
    }

    void MoveOKClick(GameObject go)
    {
        if (m_state == BuildState.Normal)
        {
            if (PutBuild.me.m_bCreateBuild)
            {
                // 发送创建建筑信息
                Reposition(new Vector3(transform.position.x, transform.position.y, transform.position.z));
                m_buildFound = (uint)PutBuild.me.GetBuildFoundByPos();

				DataManager.getBuildData().SendBuildingBuild(this);
                PutBuild.me.m_bCreateBuild = false;
                ShowSelectedPanel(false);
                PutBuild.me.m_selectedBuild = null;
            }
            else
            {
                // 发送移动信息
                Reposition(new Vector3(transform.position.x, transform.position.y, transform.position.z));
                m_buildFound = (uint)PutBuild.me.GetBuildFoundByPos();

				DataManager.getBuildData().SendMoveBuild(m_idBuilding, m_buildFound, 0);
                m_startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                PutBuild.me.m_selectedBuild = null;
            }

            m_buildMove.SetActive(false);
            PutBuild.me.m_bMoveBuild = false;
        }
    }

    void MoveCancelClick(GameObject go)
    {
        if (PutBuild.me.m_bCreateBuild)
        {
            PutBuild.me.m_bCreateBuild = false;
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Reposition(m_startPosition);
        }

        m_buildMove.SetActive(false);
        PutBuild.me.m_bMoveBuild = false;
        PutBuild.me.m_selectedBuild = null;
    }

    public void MoveCancel()
    {
        if (PutBuild.me.m_bCreateBuild)
        {
            PutBuild.me.m_bCreateBuild = false;
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Reposition(m_startPosition);
        }

        m_buildMove.SetActive(false);
        PutBuild.me.m_bMoveBuild = false;
        PutBuild.me.m_selectedBuild = null;
    }

    public void Reposition(Vector3 pos)
    {
        if (m_collider == null)
        {
            return;
        }
        
        Bounds bounds = m_collider.bounds;
        //float offsetY = bounds.size.y * 0.75f;
        float offsetY = 10.0f;
        float offsetZ = 0.0f;
        transform.position = pos;

        if (m_idBuildingType == (uint)BuildType.LORD_HALL)
        {
            offsetZ = bounds.center.z;
        }

        Vector3 posOffset = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
        Vector3 posTemp = Camera.main.WorldToViewportPoint(posOffset);
        posTemp = UICamera.currentCamera.ViewportToWorldPoint(posTemp);
        posTemp.z = 0;

        if (m_Selected)
        {
            m_Selected.transform.position = transform.position;
        }

        if (m_objPop)
        {
            m_objPop.transform.localPosition = posOffset;
        }

        if (m_buildingCD)
        {
            m_buildingCD.transform.position = posTemp;
        }

        if (m_buildName)
        {
            m_buildName.transform.localPosition = posOffset;
        }

        if (m_buildMove)
        {
            m_buildMove.transform.position = posTemp;
        }

        BuildInfoPanel bip = PanelManage.me.GetPanel<BuildInfoPanel>(PanelID.BuildInfoPanel);
        if (bip.IsVisible())
        {
            bip.Reposition();
        }
        
    }

    public void SetState(BuildState state)
    {
        if (m_state == state)
        {
            return;
        }

        m_state = state;

        if (m_state == BuildState.Normal)
        {
            Restore();
        }
        else if (m_state == BuildState.Move)
        {
            SetColor(new Color(0.65f, 0.86f, 0.11f));
        }
        else if (state == BuildState.Error)
        {
            SetColor(new Color(0.65f, 0.08f, 0.08f));
        }
    }

    void SetColor(Color color)
    {
        MeshRenderer[] meshs = transform.GetComponentsInChildren<MeshRenderer>();

        List<Material> mats = new List<Material>();

        foreach (MeshRenderer mesh in meshs)
        {
            if (mesh.sharedMaterials != null)
            {
                foreach (Material m in mesh.sharedMaterials)
                {
                    if (m != null)
                    {
                        mats.Add(new Material(m));
                    }
                }

                mesh.materials = mats.ToArray();
            }

            foreach (Material m in mats)
            {
               m.SetColor("_Emission", color);
               m.SetColor("_Color", color);
            }

            mats.Clear();
        }
    }

    void Restore()
    {
        MeshRenderer[] meshs = transform.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in meshs)
        {
            if (m_dicMaterials.ContainsKey(mesh.gameObject))
            {
                mesh.sharedMaterials = m_dicMaterials[mesh.gameObject].ToArray();
            }
        }
    }

    // 得到此范围下的阻挡
    public List<GameObject> GetRoundObj()
    {
        List<GameObject> objs = new List<GameObject>();
        if (m_collider == null)
            return objs;

        Bounds bounds = m_collider.bounds;
        Vector3 center = bounds.center;
        Vector3 size = bounds.size;
        float radio = Mathf.Max(size.x, size.y, size.z);
        Collider[] colliders = Physics.OverlapSphere(center, radio);
        if (colliders.Length <= 1)
        {
            objs.Clear();
            return objs;
        }

        foreach (Collider c in colliders)
        {
            if (c == GetComponent<Collider>() || c.gameObject.layer == LayerMask.NameToLayer("Terrain") || c.gameObject.layer == LayerMask.NameToLayer("UI") || c.gameObject.layer == LayerMask.NameToLayer("UIView") || (bounds.Intersects(c.bounds) == false && c.bounds.Intersects(bounds) == false))
                continue;

            objs.Add(c.gameObject);
        }

        return objs;
    }

    public void CleanRoundTree()
    {
        List<GameObject> objs = new List<GameObject>();

        if (m_collider == null)
            return;

        Bounds bounds = m_collider.bounds;
        Vector3 center = bounds.center;
        Vector3 size = bounds.size;
        float radio = Mathf.Max(size.x, size.y, size.z);
        Collider[] colliders = Physics.OverlapSphere(center, radio);

        if (colliders.Length <= 1)
        {
            objs.Clear();
            return;
        }

        foreach (Collider c in colliders)
        {
            if (c == GetComponent<Collider>() || (bounds.Intersects(c.bounds) == false && c.bounds.Intersects(bounds) == false))
                continue;

            if (c.gameObject.layer == LayerMask.NameToLayer("Tree"))
            {
                DestroyImmediate(c.gameObject);
            }
        }
    }

    public void RestorePos()
    {
        if (Vector3.zero != m_startPosition)
        {
            transform.position = m_startPosition;
            m_startPosition = PutBuild.me.GetEmptyFoundPos();
        }
    }

    public bool IsCanPutBuild()
    {
        List<GameObject> hoveList = GetRoundObj();

        foreach (GameObject obj in hoveList)
        {
            if (obj.layer == 0)
                continue;

            if (obj.layer != LayerMask.NameToLayer("Tree"))
            {
                return false;
            }
        }

        if (!TerrainLayer.me.IsCanBuild((BuildType)m_idBuildingType, GetComponent<Collider>()))
        {
            return false;
        }

        return true;
    }

    public void ShowBuildConstruct()
    {
        DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_idBuildingType);

        if (config == null || !config.isMove)
        {
            return;
        }
        
        if (ConstructPre != null)
        {
            foreach(Transform t in transform)
            {
                if (t != transform)
                {
                    t.gameObject.SetActive(false);
                }
            }

            ConstructPre.SetActive(true);

            return;
        }

        string strPrefab = "Prefabs/Buildings/Construct";
        GameObject go = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPrefab);
        ConstructPre = (GameObject)Object.Instantiate(go);
        ConstructPre.transform.position = transform.position;
        ConstructPre.transform.parent = transform;

        foreach (Transform t in transform)
        {
            if (t != transform)
            {
                t.gameObject.SetActive(false);
            }
        }

        ConstructPre.SetActive(true);
    }



    public void ShowBuildConstructShader()
    {
        DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_idBuildingType);

        if (config == null || !config.isMove)
        {
            return;
        }

        if (ConstructBuild == null)
        {
            CreatConstructBuild();
        }

        foreach (Transform t in transform)
        {
            if (t != transform)
            {
                t.gameObject.SetActive(false);
            }
        }

        ConstructBuild.SetActive(true);

    }

    public void HideBuildConstruct()
    {

        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }

        if (ConstructPre != null)
        {
            ConstructPre.SetActive(false);
        }
       
    }

    public void RestoreBuild()
    {
        DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)m_idBuildingType);

        if (config == null || !config.isMove)
        {
            return;
        }

        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }

        if (ConstructBuild != null)
        {
            ConstructBuild.SetActive(false);
        }
    }

    protected void CreatConstructBuild()
    {
        //创建虚化建筑
        ConstructBuild = (GameObject)Object.Instantiate(gameObject);
        ConstructBuild.transform.position = transform.position;
        ConstructBuild.transform.parent = transform;

        foreach (Transform t in ConstructBuild.transform)
        {
            Renderer renderer = t.gameObject.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material m = PutBuild.me.buildingMaterial;

                Material[] materials = new Material[t.gameObject.GetComponent<Renderer>().materials.Length];
                for (int i = 0; i < t.gameObject.GetComponent<Renderer>().materials.Length; ++i)
                {
                    materials[i] = m;
                }

                t.gameObject.GetComponent<Renderer>().materials = materials;
            }
        }
        
        ConstructBuild.SetActive(false);
    }
}
