using UnityEngine;
using System.Collections;
using Packet;
using Network;
using SLG;

namespace DataMgr
{
    public class RecruitData
    {
        public int armyType;
        public int count;

        public RecruitData()
        {
            this.armyType = 0;
            count = 0;
        }
    }

    public class SoldierData
    {
        public SoldierData()
        {
        }

        public bool init()
        {
            return true;
        }

        public void release()
        {
            this.reload();
        }

        public void reload()
        {
        }

        public void sendRecruit(RecruitData[] data)
        {
            if (data == null || data.Length == 0)
                return;

            MSG_CLIENT_RECRUIT_ARMY_REQUEST request = new MSG_CLIENT_RECRUIT_ARMY_REQUEST();
            request.lst = new RECRUIT_ARMY_REQ_INFO[data.Length];
            foreach(RecruitData item in data)
            {
                if (item.count > 0)
                {
                    request.lst[request.usCnt].idArmyType = (uint)item.armyType;
                    request.lst[request.usCnt].u8Amount = (byte)item.count;
                    request.usCnt++;
                }
            }
            NetworkMgr.me.getClient().Send(ref request);
        }

        public void sendUnRecruit(RecruitData[] data)
        {
            if (data == null || data.Length==0)
                return;

            MSG_CLIENT_DISMISS_ARMY_REQUEST request = new MSG_CLIENT_DISMISS_ARMY_REQUEST();
            request.lst = new DISMISS_ARMY_REQ_INFO[data.Length];
            foreach (RecruitData item in data)
            {
                if (item.count > 0)
                {
                    request.lst[request.usCnt].idArmyType = (uint)item.armyType;
                    request.lst[request.usCnt].u8Amount = (byte)item.count;
                    request.usCnt++;
                }
            }
            NetworkMgr.me.getClient().Send(ref request);
        }
    }
}