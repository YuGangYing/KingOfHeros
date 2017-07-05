using UnityEngine;
using System.Collections;
using Packet;
using Network;
using System;
//-
namespace Login
{
    public class LoginAuth
	{ 
		LoginResult m_callBack = null;
		ulong m_idIGG;
		string m_strCookie;
		string m_strServIp;
		int m_nPort;

        public LoginAuth()
        {
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_AUTH_RESPONSE>(onRecv);
        }

		public ulong getIggid()
		{
			return m_idIGG;
		}

		public string getCookie()
		{
			return m_strCookie;
		}

		public string getServIp()
		{
			return m_strServIp;
		}

		public int getServPort()
		{
			return m_nPort;
		}
		
		public bool login(string strIgg,string strToken,LoginResult ret)
		{
			m_callBack = ret;

            m_idIGG = Convert.ToUInt64(strIgg);

            MSG_CLIENT_AUTH_REQUEST msg = new MSG_CLIENT_AUTH_REQUEST();
            msg.idIGG = m_idIGG;
            msg.token = strToken;
            NetworkMgr.me.getClient().Send(ref msg);

			return true;
		}

        public void onRecv(ushort id, object ar)
		{
            MSG_CLIENT_AUTH_RESPONSE response = (MSG_CLIENT_AUTH_RESPONSE)ar;
			LOGIN_RET ret;
            string strMsg = "Auth Succ";

			if(response.idIGG == this.m_idIGG)
            {
				this.m_nPort = response.port;
				this.m_strCookie = response.cookie;
				this.m_strServIp = response.ip;
				ret = LOGIN_RET.SUCC;
			}
			else
			{
				ret = LOGIN_RET.FAILED;
				strMsg = "Auth Failed";
			}
			if(this.m_callBack!=null)
                this.m_callBack(ret, strMsg);
		}

		public void disConnAuth()
		{
            MSG_CLIENT_DISCONNECT_REQUEST msg = new MSG_CLIENT_DISCONNECT_REQUEST();
            NetworkMgr.me.getClient().Send(ref msg);
		}
	}
}

