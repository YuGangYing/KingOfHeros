using UnityEngine;
using System.Collections;
using Network;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
namespace Login
{
    public enum NETMSG_CONNECT_RESULT
    {
        NETMSG_CONNSUCC,
        NETMSG_CONNFAILED,
    }

	public enum LOGIN_RET:int
	{
		SUCC,
		FAILED,
		UNKNOW
	}
	public delegate void LoginResult(LOGIN_RET ret,string strErrmsg);

	enum LOGIN_STEP:int
	{
		NOLOGIN,
		IGG_LOGIN,
		SLG_AUTH,
		SLG_LOGIN,
		SLG_SUCC
	}
	
	public class LoginMgr
	{
		string strAuthServ = "";
		int nAuthPort = 8001;

		IggLogin m_iggLogin = null;
		SlgLogin m_slgLogin = null;
		LoginAuth m_loginAuth = null;

		string m_strErrMsg = "";
		LOGIN_STEP m_step = LOGIN_STEP.IGG_LOGIN;
		LoginResult m_callback = null;

		bool m_isLogin = false;

        string m_strIggId = "12345";

        public string Iggid
        {
            set { m_strIggId = value; }
        }

        public LoginMgr()
        {
            SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NetConntSucc, this.connSucc);
            SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NetConntFailed, this.connFailed);
        }

        bool connSucc(SLG.EventArgs obj)
        {
            string strMsg = "";
            if(obj!=null && obj.m_obj!=null)
                strMsg = obj.m_obj as string;
            ConnectResult(Login.NETMSG_CONNECT_RESULT.NETMSG_CONNSUCC, m_strErrMsg);
            return true;
        }

        bool connFailed(SLG.EventArgs obj)
        {
            string strMsg = "";
            if (obj != null && obj.m_obj != null)
                strMsg = obj.m_obj as string;
            ConnectResult(Login.NETMSG_CONNECT_RESULT.NETMSG_CONNFAILED, m_strErrMsg);
            return true;
        }

		public bool login(string strIp,int nPort,LoginResult ret)
		{
			strAuthServ = strIp;
			nAuthPort = nPort;
			m_callback = ret;
			return login();
		}

        public bool isLogin()
        {
            return this.m_isLogin;
        }

		void release()
		{
			m_iggLogin = null;
			m_slgLogin = null;
			m_loginAuth = null;
			//NetworkMgr.getInstance().getClient().close();
            m_isLogin = true;
		}

		bool login()
		{
			release();
			m_isLogin = false;

            Logger.LogDebug("Login Begin");
			m_step = LOGIN_STEP.IGG_LOGIN;
			m_iggLogin = new IggLogin();
			if(!m_iggLogin.login(this.onLogin))
			{
				m_strErrMsg = m_iggLogin.getErrMsg();
				m_iggLogin = null;
				return false;
			}
			return true;
		}

		void onLogin(LOGIN_RET ret,string strErrmsg)
		{
			if(ret==LOGIN_RET.SUCC)
			{
				if(m_step== LOGIN_STEP.IGG_LOGIN)
				{
                    Logger.LogDebug("IGG Server Login SUUC");
					NetworkMgr.getInstance().getClient().connect(strAuthServ,nAuthPort);
					m_step = LOGIN_STEP.SLG_AUTH;
				}
				else if(m_step== LOGIN_STEP.SLG_AUTH)
				{
                    //m_loginAuth.disConnAuth();
                    Logger.LogDebug("Auth SUUC");
					NetworkMgr.getInstance().getClient().connect(m_loginAuth.getServIp(),m_loginAuth.getServPort());
					m_step = LOGIN_STEP.SLG_LOGIN;
				}
				else if(m_step== LOGIN_STEP.SLG_LOGIN)
				{
                    Logger.LogDebug("Login SUUC");
                    m_strErrMsg = "Login SUUC";
					m_isLogin = true;
					m_step = LOGIN_STEP.SLG_SUCC;
                    m_callback(LOGIN_RET.SUCC,m_strErrMsg);
				}
			}
			else
			{
                Logger.LogDebug("Login Failed");
				m_strErrMsg = strErrmsg;
				if(m_callback!=null)
					m_callback(ret,m_strErrMsg);
			}
		}

        public void ConnectResult(NETMSG_CONNECT_RESULT reID, string strRe)
		{
            m_strErrMsg = strRe;

            if (reID == NETMSG_CONNECT_RESULT.NETMSG_CONNSUCC)
			{
				if(m_step== LOGIN_STEP.SLG_AUTH)
				{
					if(m_loginAuth==null)
						m_loginAuth = new LoginAuth();
                    if (m_strIggId.Length < 1)
    					this.m_loginAuth.login(m_iggLogin.getIggId(),m_iggLogin.getToken(),this.onLogin);
                    else
                        this.m_loginAuth.login(m_strIggId, m_iggLogin.getToken(), this.onLogin);
                }
				else if(m_step== LOGIN_STEP.SLG_LOGIN)
				{
					if(m_slgLogin==null)
						m_slgLogin = new SlgLogin();
					this.m_slgLogin.login(m_loginAuth.getIggid(),m_loginAuth.getCookie(),this.onLogin);
				}
			}
            else if (reID == NETMSG_CONNECT_RESULT.NETMSG_CONNFAILED)
			{
				if(m_callback!=null)
					m_callback(LOGIN_RET.FAILED,m_strErrMsg);
			}
		}

        public void OnSlgLoginRecv(ushort id, object ar)
        {
            if (m_slgLogin != null)
            {
                m_slgLogin.onRecv(id, ar);
            }
        }
	}
}