using UnityEngine;
using System.Collections;
using Packet;
using Network;
using System;

namespace Login
{
	//Line End
	public class SlgLogin
	{
		LoginResult m_callBack = null;

		public SlgLogin()
		{
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_LOGIN_RESPONSE>(onRecv);
		}

		public bool login(ulong uIgg,string strCookie,LoginResult ret)
		{
			m_callBack = ret;
            
            MSG_CLIENT_LOGIN_REQUEST msg = new MSG_CLIENT_LOGIN_REQUEST();
            msg.idIGG = uIgg;
            msg.szCookie = strCookie;

            NetworkMgr.me.getClient().Send(ref msg);
			return true;
		}

		public void onRecv(ushort id, object ar)
		{
			LOGIN_RET ret = LOGIN_RET.SUCC;
			string strErr = "SUCC";

            MSG.Sgt.CheckMessageId<MSG_CLIENT_LOGIN_RESPONSE>(id);

            MSG_CLIENT_LOGIN_RESPONSE response = (MSG_CLIENT_LOGIN_RESPONSE)ar;
            if (response.res == 0)
            {
                ret = LOGIN_RET.FAILED;
                strErr = "loginFailed";
            }
            else
            {
                ret = LOGIN_RET.SUCC;
            }

			if(this.m_callBack!=null)
				this.m_callBack(ret,strErr);
		}
	}
}