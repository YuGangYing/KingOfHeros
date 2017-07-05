using System;
using System.Collections.Generic;
using Packet;
using Network;
using System.IO;
using System.Runtime.InteropServices;

namespace DataMgr
{
    //登陆后返回
	public class LoginData
	{
        uint _totalSize = 0;
        uint _curSize = 0;
        byte[] _data = null;

        public void init()
        {
            this.isDone = false;
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_QUERY_USER_INFO_AFTER_LOGIN_REQUEST>(this.onRecv);
        }

        public void getUserInfo()
        {
            MSG_CLIENT_QUERY_USER_INFO_AFTER_LOGIN_REQUEST msg = new MSG_CLIENT_QUERY_USER_INFO_AFTER_LOGIN_REQUEST();
            Network.NetworkMgr.me.getClient().Send(ref msg);
        }

        public void onRecv(ushort wMsgId, object ar)
        {
            if (!MSG.Sgt.CheckMessageId<MSG_CLIENT_QUERY_USER_INFO_AFTER_LOGIN_RESPONSE>(wMsgId) || ar == null)
                return;
            MSG_CLIENT_QUERY_USER_INFO_AFTER_LOGIN_RESPONSE msg = (MSG_CLIENT_QUERY_USER_INFO_AFTER_LOGIN_RESPONSE)ar;
            if (_totalSize == 0)
            {
                _totalSize = msg.totolsize;
                this._data = new byte[this._totalSize];
            }
            Array.Copy(msg.data, 0, this._data, this._curSize, msg.cursize);
            this._curSize += msg.cursize;
            if (this._curSize == this._totalSize)
                this.unpack();
        }

        public void unpack()
        {
            DataMgr.Tools.unzip(ref _data);

            MemoryStream ms = new MemoryStream(this._data);
            BinaryReader br = new BinaryReader(ms);

            while (ms.Position < ms.Length)
            {
                long pos = ms.Position;
                ushort wMsgSize = br.ReadUInt16();
                ushort wMsgType = br.ReadUInt16();
                ms.Position += wMsgSize - 4;

                byte[] bt = new byte[wMsgSize];
                Array.Copy(this._data, pos, bt, 0, wMsgSize);
                switch (wMsgType)
                {
                    case 10005://userdata
                        {
                            MSG_CLIENT_USER_INFO_EVENT evt = new MSG_CLIENT_USER_INFO_EVENT();
                            evt.unpack(ref bt);
                            DataManager.getUserData().onRecv(wMsgType, evt);
                        }
                        break;
                    case 10006://heroList
                        {
                            MSG_CLIENT_HERO_LST_EVENT evt = new MSG_CLIENT_HERO_LST_EVENT();
                            evt.unpack(ref bt);
                            DataManager.getHeroData().OnMsgList(wMsgType, evt);
                        }
                        break;
                    case 10015://buildData
                        {
                            MSG_BUILDING_LIST_EVENT evt = new MSG_BUILDING_LIST_EVENT();
                            evt.unpack(ref bt);
                            DataManager.getBuildData().OnRecClientBuildList(wMsgType, evt);
                        }
                        break;
                    case 10019://QueueData
                        {
                            MSG_QUEUE_LIST evt = new MSG_QUEUE_LIST();
                            evt.unpack(ref bt);
                            DataManager.getQueueData().OnRecQueueList(wMsgType, evt);
                        }
                        break;
                    case 10053://mail
                        {
                            MSG_CLIENT_MAIL_INFO evt = new MSG_CLIENT_MAIL_INFO();
                            evt.unpack(ref bt);
                            DataManager.getMailData().onMailInfo(wMsgType, evt);
                        }
                        break;
                    case 10032:
                        {
                            MSG_TECHNOLOGY_LIST evt = new MSG_TECHNOLOGY_LIST();
                            evt.unpack(ref bt);
                            DataManager.getTechData().onTechList(wMsgType, evt);
                        }
                        break;
                }
            }
            this.isDone = true;
        }

        public void reload()
        {
        }

        public void release()
        {
            this.isDone = false;
        }

        public bool isDone
        {
            get;
            set;
        }
    }
}
