using UnityEngine;
using System.Collections;
using SLG;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
public class MainController : SingletonMonoBehaviourNoCreate<MainController>
{
    //全局事件集合
    GlobalEventSet _eventCenter;

    public static int SCENE_LOGIN = 0;
    public static int SCENE_MAINCITY = 1;
    public static int SCENE_BATTLE = 2;

    int _curScene = SCENE_LOGIN;
    AsyncOperation _async = null;
    bool _isLoading = false;
    Texture2D _loadBackPic = null;

    protected override void Init()
    {
        gameObject.AddComponent<Network.NetworkMgr>();
        gameObject.AddComponent<AudioCenter>();
        _eventCenter = GlobalEventSet.me;
        this.showCameraPath = true;
    }

    public bool showCameraPath
    {
        get;
        set;
    }

    public bool showLoading
    {
        get;
        set;
    }

    public int loadCityTimes
    {
        get;
        set;
    }

    void Start()
    {
        this.showLoading = true;
        this.loadCityTimes = 0;
        SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.ChangeScene, this.ChangeScene);
        SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NetDiscon, this.onDiscon);
    }

    void Update()
    {
    }

    bool onDiscon(SLG.EventArgs obj)
    {
        MessageBoxMgr.ShowConfirm("Disconect", "Network Disconect....", onConfirmDiscon);
        return true;
    }

    bool onConfirmDiscon(SLG.EventArgs obj)
    {
        logout();
        return true;
    }

    public bool ChangeScene(SLG.EventArgs obj)
    {
        this._isLoading = true;
        this._curScene = (int)obj.m_obj;
        StartCoroutine(loadSence(this._curScene));
        getRandomPic();
        //音效管理清理
        AudioCenter.me.playLoadAudio();
        //清理事件列表
        SLG.GlobalEventSet.me.removeAllEvents();
        return true;
    }

    IEnumerator loadSence(int nSceneid)
    {
        if (nSceneid == SCENE_MAINCITY )
            this.loadCityTimes++;
        _async = Application.LoadLevelAsync(nSceneid);
        _async.allowSceneActivation = false;
        while (!_async.isDone && _async.progress < 0.9f)
        {
            yield return null;
        }
        this._isLoading = false;
        yield return _async;
    }

    void loadFinish()
    {
        AudioCenter.me.release();
        this._async.allowSceneActivation = true;
        this._async = null;
        SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.ChangeScene, this.ChangeScene);
        SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NetDiscon, this.onDiscon);
    }

    void OnGUI()
    {
        if(!this._isLoading && this._async!=null)
        {
            if (this._curScene == SCENE_MAINCITY && !DataManager.isDone())
            {
            }
            else
            {
                loadFinish();
            }
        }
        if (Application.isLoadingLevel)
        {
            if (this.showLoading)
            {
                GUIStyle style = new GUIStyle(GUI.skin.box);
                style.fontStyle = FontStyle.Bold;
                style.fontSize = 40;
                if (_loadBackPic != null)
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _loadBackPic);
                string strLoading = "loading...";
                GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 180, 300, 60), strLoading, style);
            }
        }
        else
            this.showLoading = true;
    }

    void getRandomPic()
    {
        string strName = "Prefabs/UI/Backgound/load" + UnityEngine.Random.Range(1, 6);
        Texture2D temp = ResourceCenter.LoadAsset<Texture2D>(strName);
        if (temp != null)
            this._loadBackPic = temp;
    }

    public void quit()
    {
        UnityEngine.Application.Quit();
        AudioCenter.me.quit();
    }

    public void logout()
    {
        //清理数据
        DataManager.me.release();
        //返回登录界面
        SLG.GlobalEventSet.FireEvent(SLG.eEventType.ChangeScene, new SLG.EventArgs(MainController.SCENE_LOGIN));
        this.showCameraPath = true;
        this.loadCityTimes = 0;
    }
}
