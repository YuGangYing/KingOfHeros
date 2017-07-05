using UnityEngine;
using System.Collections.Generic;
using Packet;
using Network;

namespace DataMgr
{
    public delegate void onUpdateFunc(int nRet);
    public delegate void techUpdateFunc();

    public class TechResult
    {
        public int nTechId;
        public int nTechTypeId;
        public uint nRet;
    }

	public class TechData
	{
        Dictionary<int, TechItem> _lstTech = new Dictionary<int,TechItem>();

        public TechData()
		{
		}
		
		public bool init()
		{
            //学习
            NetworkMgr.me.getClient().RegMsgFunc<MSG_TECHNOLOGY_LEARND_RESPONSE>(onLear);
            //升级
            NetworkMgr.me.getClient().RegMsgFunc<MSG_TECHNOLOGY_UPLEV_RESPONSE>(onUpdateLev);
            //升级加速
            NetworkMgr.me.getClient().RegMsgFunc<MSG_TECHNOLOGY_ACCELERATE_UPLEV_RESPONSE>(onUpdateAcce);
            //获得列表
            NetworkMgr.me.getClient().RegMsgFunc<MSG_TECHNOLOGY_LIST>(onTechList);
            //更新
            NetworkMgr.me.getClient().RegMsgFunc<MSG_TECHNOLOGY_UPDATE>(onTechListUpdate);
            return true;
        }
		
		public void release()
		{
            this._lstTech.Clear();
            this.reload();
		}

        public void reload()
        { 
        }

        public Dictionary<int, TechItem> techList
        {
            get { return this._lstTech; }
        }

        //学习
        public void learn(int nType)
        {
            MSG_TECHNOLOGY_LEARND_REQUEST request = new MSG_TECHNOLOGY_LEARND_REQUEST();
            request.idTechnologyType = (uint)nType;

            NetworkMgr.me.getClient().Send<MSG_TECHNOLOGY_LEARND_REQUEST>(ref request);
        }

        //升级
        public void update(uint nTechId)
        {
            MSG_TECHNOLOGY_UPLEV_REQUEST request = new MSG_TECHNOLOGY_UPLEV_REQUEST();
            request.idTechnology = (uint)nTechId;
            NetworkMgr.me.getClient().Send<MSG_TECHNOLOGY_UPLEV_REQUEST>(ref request);
        }

        //升级加速
        public void upadteAcce(uint nTechId, int nSec)
        {
            MSG_TECHNOLOGY_ACCELERATE_UPLEV_REQUEST request = new MSG_TECHNOLOGY_ACCELERATE_UPLEV_REQUEST();
            request.idTechnology = nTechId;
            request.u32Secs = (uint)nSec;
            NetworkMgr.me.getClient().Send(ref request);
        }

        private void onLear(ushort id, object ar)
        {
            MSG_TECHNOLOGY_LEARND_RESPONSE rsponse = (MSG_TECHNOLOGY_LEARND_RESPONSE)ar;
            TechResult ret = new TechResult();
            ret.nRet = rsponse.unRet;
            ret.nTechId = (int)rsponse.idTechnology;
            ret.nTechTypeId = (int)rsponse.idTechnologyType;
            SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyTechLearn, new SLG.EventArgs(ret));
        }

        private void onUpdateLev(ushort id, object ar)
        {
            MSG_TECHNOLOGY_UPLEV_RESPONSE rsponse = (MSG_TECHNOLOGY_UPLEV_RESPONSE)ar;
            TechResult ret = new TechResult();
            ret.nRet = rsponse.unRet;
            ret.nTechId = (int)rsponse.idTechnology;
            SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyTechUpdate, new SLG.EventArgs(ret));
        }

        private void onUpdateAcce(ushort id, object ar)
        {
            MSG_TECHNOLOGY_ACCELERATE_UPLEV_RESPONSE rsponse = (MSG_TECHNOLOGY_ACCELERATE_UPLEV_RESPONSE)ar;

            TechResult ret = new TechResult();
            ret.nRet = rsponse.unRet;
            ret.nTechId = (int)rsponse.idTechnology;
            SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyTechAcce, new SLG.EventArgs(ret));
        }

        //技能升级
        private void onTechListUpdate(ushort id, object ar)
        {
            MSG_TECHNOLOGY_UPDATE rsponse = (MSG_TECHNOLOGY_UPDATE)ar;
            TechItem item = null;
            if (this._lstTech.TryGetValue((int)rsponse.idTechnology,out item))
            {                
                item.nLevel = (int)rsponse.cbLev;
                item.nArmyLevel = userTechInfo.getArmyLevel(item.nLevel);
                item.nStarLevel = userTechInfo.getArmyStar(item.nLevel);
                item.nState = (int)rsponse.cbState;
            }
            else
            {
                ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_TECHONOLOGY);
                if (config == null)
                    return;

                ConfigRow row = config.getRow(CFG_TECHNOLOGY.TECHNOLOGY_TYPEID, (int)rsponse.idTechnologyType,
                                              CFG_TECHNOLOGY.LEVEL, (int)rsponse.cbLev);

                TechItem newItem = new TechItem();
                newItem.nTechId = (int)rsponse.idTechnologyType;
                newItem.nLevel = (int)rsponse.cbLev;
                newItem.nArmyLevel = userTechInfo.getArmyLevel(newItem.nLevel);
                newItem.nStarLevel = userTechInfo.getArmyStar(newItem.nLevel);
                newItem.nId = rsponse.idTechnology;
                newItem.armyType = userTechInfo.getArmyType(row.getIntValue(CFG_TECHNOLOGY.EFFECT_ARMY_TYPEID));
                newItem.operType = row.getEnumValue<TECH_TYPE>(CFG_TECHNOLOGY.SERVICE, TECH_TYPE.TECH_NONE);
                newItem.nState = (int)rsponse.cbState;
                this._lstTech[newItem.nTechId] = newItem; 
            }
        }

        //技能列表
        public void onTechList(ushort id, object ar)
        {
            MSG_TECHNOLOGY_LIST rsponse = (MSG_TECHNOLOGY_LIST)ar;
            foreach (TECHNOLOGY_INFO item in rsponse.lst)
            {
                TechItem newItem = userTechInfo.getTechItem(item);
                this._lstTech[newItem.nTechId] = newItem;
            }
        }
	}
}