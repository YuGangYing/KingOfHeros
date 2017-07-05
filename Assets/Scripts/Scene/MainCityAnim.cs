using UnityEngine;
using System.Collections;
using DataMgr;
using UI;

public class MainCityAnim : MonoBehaviour
{
    public Transform parent;
    bool _bInitCamera = false;
    GameObject _camera = null;
    CameraPathBezierAnimator _player;
    bool _bInit = false;
    float _lenthTime = 0f;
    float _startTime = 0f;

    void Awake()
    {
        this._bInitCamera = DataManager.getSyscfg().getValue(SYSTEM_CFG.MAINCITYCAMERA);
    }

	void Start () 
    {
        init();
        //init1();
    }

    void init()
    {
        if (this._bInit)
            return;
        //签到奖励时不显示相机巡游。
        //         if (DataMgr.DataManager.getHeroData().CheckInInfo._isHave)
        //             return;
        //         PanelBase panel = UI.PanelManage.me.getPanel(PanelID.CheckInPanel);
        //         if(panel==null || panel.IsVisible())
        //             return;
        if (!this._bInit)
        {
            if (this._bInitCamera && MainController.me.showCameraPath)
            {
                GameObject prefab = ResourceCenter.LoadAsset<GameObject>("Prefabs/CameraAnima/CameraAnim");
                if (prefab == null)
                    return;
                _camera = GameObject.Instantiate(prefab) as GameObject;
                if (parent != null)
                    _camera.transform.parent = parent;
            }
//            MainController.me.showCameraPath = false;
            this._bInit = true;
        }
    }

    void init1()
    {
        if (this._bInit)
            return;
        //签到奖励时不显示相机巡游。
//         if (DataMgr.DataManager.getHeroData().CheckInInfo._isHave)
//             return;
//         PanelBase panel = UI.PanelManage.me.getPanel(PanelID.CheckInPanel);
//         if(panel==null || panel.IsVisible())
//             return;
        if (!this._bInit)
        {
            if (this._bInitCamera && MainController.me.showCameraPath)
            {
                GameObject prefab = ResourceCenter.LoadAsset<GameObject>("Prefabs/Main City Camera Anima");
                if (prefab == null)
                    return;
                _camera = GameObject.Instantiate(prefab) as GameObject;
                if (parent != null)
                    _camera.transform.parent = parent;
                _player = _camera.GetComponentInChildren<CameraPathBezierAnimator>();
                if (_player != null)
                {
                    _player.AnimationFinished += onFinish;
                    _player.AnimationStarted += onStart;
                }
            }
            MainController.me.showCameraPath = false;
            this._bInit = true;
        }
    }
	
	void Update () 
    {
        init();

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            onFinish();
        }

        if (this._lenthTime <= 0f && _camera!=null)
        {
            Animator animator = _camera.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
                if (state.nameHash == Animator.StringToHash("Base Layer.MaincityCam_Animation"))
                {
                    this._lenthTime = state.length;
                    this._startTime = Time.time;
                }
            }
            showMain(false);
        }
        else if (Time.time - this._startTime > this._lenthTime)
            onFinish();
	}

    void onStart()
    {
        this._player.AnimationStarted -= onStart;
        showMain(false);
    }

    void showMain(bool bFlag)
    {
        MainPanel main = PanelManage.me.GetPanel<MainPanel>(PanelID.MainPanel);
        if (main != null)
        {
            main.SetVisible(bFlag);
            main.ShowCover(false);
        }
    }

    void onFinish()
    {
        showMain(true);
        if (_camera != null)
            _camera.SetActive(false);
        Destroy(this._camera);
        Destroy(this);
    }
}
