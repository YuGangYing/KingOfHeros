using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using SLG;
using Packet;
using DataMgr;
using Login;

// 客户端启动入口函数
public class Main : MonoBehaviour
{
    LoginMgr _login = null;
    void Awake()
    {
    }

    void Start()
    {
        _login = new LoginMgr();
        initLanguage();
        AudioCenter.me.play(AudioMgr.AudioName.LOAD_BGM);
    }

    public string ip = "192.168.1.152";
    public int port = 8001;
    public string iggid = "";

    enum eState
    {
        eNull, // 还未登陆
        eIng, // 登陆当中
        eFail, // 登陆失败
        eSuc, // 登陆成功
        eEnd, // 登陆结束
    }

    eState m_state = eState.eNull;
    string m_errorMsg;
    string strLogin = string.Empty;
    string strIP = string.Empty;
    string strReLogin = string.Empty;
    string strIggid = string.Empty;
    string strPort = string.Empty;
    string strLoging = string.Empty;

    void initLanguage()
    {
        strLogin = DataManager.getLanguageMgr().getString("login");
        strIP = DataManager.getLanguageMgr().getString("ip");
        strReLogin = DataManager.getLanguageMgr().getString("relogin");
        strIggid = DataManager.getLanguageMgr().getString("iggid");
        strPort = DataManager.getLanguageMgr().getString("port");
        strLoging = DataManager.getLanguageMgr().getString("loging");
    }

    void OnGUI()
    {
        if (m_state == eState.eEnd)
            return;
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 40;

        GUILayout.BeginArea(new Rect(Screen.width/2-300,Screen.height/2-200,600,400));
        ToolUtil.LineTextAndString(strIP, style, 250f, ref ip, 350f);
        GUILayout.Space(20);
        ToolUtil.LineTextAndInt(strPort, style, 250f, ref port, 350f);
        GUILayout.Space(20);
        ToolUtil.LineTextAndString(strIggid, style, 250f, ref iggid, 350f);
        GUILayout.Space(40);

        switch (m_state)
        {
        case eState.eNull:
            {
                if (GUILayout.Button(strLogin, style, GUILayout.Width(400f)))
                {
                    // 开始登陆
                    m_state = eState.eIng;
                    this._login.Iggid = iggid;
                    this._login.login(ip, port, LoginResult);
                    GUILayout.TextArea("connecting...", style);
                }
            }
            break;
        case eState.eIng:
            {
                GUILayout.TextArea(strLoging, style);
            }
            break;
        case eState.eFail:
            {
                if (m_errorMsg != null)
                {
                    GUILayout.TextArea("Error:" + m_errorMsg, style);
                }

                if (GUILayout.Button(strReLogin, style, GUILayout.Width(400f)))
                {
                    // 开始登陆
                    m_state = eState.eIng;
                    this._login.Iggid = iggid;
                    this._login.login(ip, port, LoginResult);
                }
            }
            break;
        case eState.eSuc:
            {
                // 登陆成功
                GUILayout.TextArea("Login Success!",style);
                m_state = eState.eEnd;
            }
            break;
        }
        GUILayout.EndArea();
    }

    void Update()
    {
    }

    void LoginResult(Login.LOGIN_RET ret, string strErrmsg)
    {
        if (ret == Login.LOGIN_RET.SUCC)
        {
            // 登陆成功了
            m_state = eState.eSuc;
            //获取用户信息
            DataMgr.DataManager.getLoginData().getUserInfo();
            // 可以加载主城场景了
            SLG.GlobalEventSet.FireEvent(SLG.eEventType.ChangeScene, new SLG.EventArgs(MainController.SCENE_MAINCITY));
        }
        else
        {
            m_state = eState.eFail;
            m_errorMsg = strErrmsg;
            // 登陆失败了
            Logger.LogDebug("login fail! {0}", strErrmsg);
        }
    }
}