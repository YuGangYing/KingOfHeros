using UnityEngine;
using System;
using System.Collections.Generic;

using Packet;
using Network;



namespace DataMgr
{

public class HeroData
{
	#region ------ hero extern define variable ------

    public class ItemExternData
    {
        public uint mdwId = CConstance.DEFAULT_ID;  // 本身ID

        public uint mdwBattleAtk = 0;               // HERO_ATTR_BATTLE_ATK = HERO_ATTR_BATTLE_BEG,    // 攻
        public uint mdwBattleDef = 0;               // HERO_ATTR_BATTLE_DEF,   // 防
        public uint mdwBattleHp = 0;                // HERO_ATTR_BATTLE_HP,    // 血
        public uint mdwBattleViolence = 0;          // HERO_ATTR_BATTLE_VIOLENCE,  // 暴击
        public uint mdwBattleLeader = 0;            // HERO_ATTR_BATTLE_LEADER,    // 领导力
        public uint mdwBattlePos = 0;               // HERO_ATTR_BATTLE_POS,   // 战斗位置

        public uint mdwLordId = CConstance.DEFAULT_ID;      // HERO_ATTR_LORD,                // 服侍的主卡英雄
        public uint mdwWiterAtkId = CConstance.DEFAULT_ID;  //  HERO_ATTR_WAITER_ATK,          // 攻击侍从
        public uint mdwWiterDef = CConstance.DEFAULT_ID;    //HERO_ATTR_WAITER_DEFENCE,      // 防御侍从
        public uint mdwWiterHp = CConstance.DEFAULT_ID;     //HERO_ATTR_WAITER_HP,           // 血量侍从
        public uint mdwWiterLeader = CConstance.DEFAULT_ID; //HERO_ATTR_WAITER_LEADER,       // 领导力侍从
    }

//	[SerializeField]
    private Dictionary<uint, HERO_INFO> mmapData = null;         // 拥有卡牌数据
	public int Amount { get{ return mmapData.Count; } }

    private Dictionary<uint, ItemExternData> mmapExtData = null; // 扩展数据
    public Dictionary<uint, ItemExternData> ItemExData
    {
        get { return mmapExtData; }
    }

    uint mErrorId = 0xFFFF;
    public uint InvalidId { get { return mErrorId; } }
    private HERO_INFO mErrorItem = new HERO_INFO();

    // star progress base value
    static float mfFillValue = 0.2f;
    public static float FillValue { get { return mfFillValue; } }

	#endregion

    #region ------ CCheckInData ------
    public class CCheckInData
    {
        public MSG_CLIENT_DAY_LOGIN_AWARD _stData = new MSG_CLIENT_DAY_LOGIN_AWARD();
        public bool _isHave = false;
    }
    CCheckInData mclsCheckData = new CCheckInData();
    public CCheckInData CheckInInfo { get { return mclsCheckData; } }

    #endregion

    public HeroData()
	{
        mmapData = new Dictionary<uint, HERO_INFO>();
        mmapExtData = new Dictionary<uint, ItemExternData>();

        mErrorItem.idHero = this.InvalidId;
        mErrorItem.idType = this.InvalidId;
	}
		
	public bool init()
	{
        RegisterMsg();
		return true;
	}
		
	public void release()
	{
        CleanItem();
	}

    
    public HERO_INFO this[uint dwId]
    {
        get
        {
            if (!isHaveItem(dwId))
            {
					Debug.LogError(string.Format("HeroItemData::this[{0}]", dwId));
                return mErrorItem;
            }

            return mmapData[dwId];
        }
    }

    #region function


    void RegisterMsg()
    {
        NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_HERO_LST_EVENT>(this.OnMsgList);
        NetworkMgr.me.getClient().RegMsgFunc<MSG_HERO_ATTR_EVENT>(this._OnMsgAttr);
        NetworkMgr.me.getClient().RegMsgFunc<MSG_HERO_CHANGE_EVENT>(this._OnChange);

        NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_DAY_LOGIN_AWARD>(this._MsgCheckIn);
    }

    public void OnMsgList(ushort wMsgId, object ar)
    {
        bool bRet = MSG.Sgt.CheckMessageId<MSG_CLIENT_HERO_LST_EVENT>(wMsgId);
        if (!bRet)
            return;

        MSG_CLIENT_HERO_LST_EVENT e = (MSG_CLIENT_HERO_LST_EVENT)ar;

        for (int i = 0; i < Math.Min(e.usCnt, e.lst.Length); i++)
        {
            HERO_INFO data = new HERO_INFO();
            data = e.lst[i];

            mmapData.Add(data.idHero, data);

            ItemExternData ied = new ItemExternData();
            mmapExtData.Add(data.idHero, ied);
        }
    }

    private void _OnMsgAttr(ushort wMsgId, object ar)
    {
        bool bRet = MSG.Sgt.CheckMessageId<MSG_HERO_ATTR_EVENT>(wMsgId);
        if (!bRet)
            return;

        MSG_HERO_ATTR_EVENT e = (MSG_HERO_ATTR_EVENT)ar;

        if (!isHaveItem(e.idHero))
            return;

        for (int i = 0; i < Math.Min(e.usCnt, e.lst.Length); i++)
        {
            HERO_ATTR n = (HERO_ATTR)e.lst[i].attr;
            _SetAttributeValue(e.idHero, n, e.lst[i].unVal);
        }

        //SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyRefreshQualityUpgrade, null);
    }

    bool _SetAttributeValue(uint dwId, HERO_ATTR en, uint uVal)
    {
        if (!isHaveItem(dwId))
            return false;

        switch (en)
        {
            case HERO_ATTR.HERO_ATTR_ID: break;
            case HERO_ATTR.HERO_ATTR_TYPE_ID: break;
            case HERO_ATTR.HERO_ATTR_STAR:
                {
                    HERO_INFO hi = mmapData[dwId];
                    hi.u8Star = (byte)uVal;
                    mmapData[dwId] = hi;

                    SLG.EventArgs obj = new SLG.EventArgs();
                    obj.m_obj = dwId;
                    SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyRefreshQualityUpgrade, obj);
                }
                break;
            case HERO_ATTR.HERO_ATTR_LEVEL:
                {
                    HERO_INFO hi = mmapData[dwId];
                    hi.usLevel = (byte)uVal;
                    mmapData[dwId] = hi;
                }
                break;
            case HERO_ATTR.HERO_ATTR_EXP:
                {
                    HERO_INFO hi = mmapData[dwId];
                    hi.unExp = (byte)uVal;
                    mmapData[dwId] = hi;
                }
                break;
            case HERO_ATTR.HERO_ATTR_STATUS:
                {
                    HERO_INFO hi = mmapData[dwId];
                    hi.u8Status = (byte)uVal;
                    mmapData[dwId] = hi;
                }
                break;
            case HERO_ATTR.HERO_ATTR_SKILL1: break;
            case HERO_ATTR.HERO_ATTR_SKILL2: break;
            case HERO_ATTR.HERO_ATTR_SKILL3: break;
            case HERO_ATTR.HERO_ATTR_SKILL_LVL1:
                {
                    HERO_INFO hi = mmapData[dwId];
                    hi.usSkillLvl1 = (ushort)uVal;
                    mmapData[dwId] = hi;
                }
                break;
            case HERO_ATTR.HERO_ATTR_SKILL_LVL2:
                {
                    HERO_INFO hi = mmapData[dwId];
                    hi.usSkillLvl2 = (ushort)uVal;
                    mmapData[dwId] = hi;
                }
                break;
            case HERO_ATTR.HERO_ATTR_SKILL_LVL3:
                {
                    HERO_INFO hi = mmapData[dwId];
                    hi.usSkillLvl3 = (ushort)uVal;
                    mmapData[dwId] = hi;
                }
                break;
            case HERO_ATTR.HERO_ATTR_SKILL_EXP1:
                {
                    HERO_INFO hi = mmapData[dwId];
                    hi.unSkillExp1 = uVal;
                    mmapData[dwId] = hi;
                }
                break;
            case HERO_ATTR.HERO_ATTR_SKILL_EXP2:
                {
                    HERO_INFO hi = mmapData[dwId];
                    hi.unSkillExp2 = uVal;
                    mmapData[dwId] = hi;
                }
                break;
            case HERO_ATTR.HERO_ATTR_SKILL_EXP3:
                {
                    HERO_INFO hi = mmapData[dwId];
                    hi.unSkillExp3 = uVal;
                    mmapData[dwId] = hi;
                }
                break;
            case HERO_ATTR.HERO_ATTR_BATTLE_DEF:
                {
                    if(mmapExtData.ContainsKey(dwId))
                        mmapExtData[dwId].mdwBattleDef = uVal;
                }
                break;
            case HERO_ATTR.HERO_ATTR_BATTLE_HP:
                {
                    if (mmapExtData.ContainsKey(dwId))
                        mmapExtData[dwId].mdwBattleHp = uVal;
                }
                break;
            case HERO_ATTR.HERO_ATTR_BATTLE_VIOLENCE:
                {
                    if (mmapExtData.ContainsKey(dwId))
                        mmapExtData[dwId].mdwBattleViolence = uVal;
                }
                break;
            case HERO_ATTR.HERO_ATTR_BATTLE_LEADER:
                {
                    if (mmapExtData.ContainsKey(dwId))
                        mmapExtData[dwId].mdwBattleLeader = uVal;
                }
                break;
            case HERO_ATTR.HERO_ATTR_BATTLE_POS:
                {
                    if (mmapExtData.ContainsKey(dwId))
                        mmapExtData[dwId].mdwBattlePos = uVal;
                }
                break;
 
            case HERO_ATTR.HERO_ATTR_LORD:                // 服侍的主卡英雄
                {
                    if (mmapExtData.ContainsKey(dwId))
                        mmapExtData[dwId].mdwLordId = uVal;
                }
                break;
            case HERO_ATTR.HERO_ATTR_WAITER_ATK:          // 攻击侍从
                {
                    if (mmapExtData.ContainsKey(dwId))
                        mmapExtData[dwId].mdwWiterAtkId = uVal;
                }
                break;
            case HERO_ATTR.HERO_ATTR_WAITER_DEFENCE:      // 防御侍从
                {
                    if (mmapExtData.ContainsKey(dwId))
                        mmapExtData[dwId].mdwWiterDef = uVal;
                }
                break;
            case HERO_ATTR.HERO_ATTR_WAITER_HP:           // 血量侍从
                {
                    if (mmapExtData.ContainsKey(dwId))
                        mmapExtData[dwId].mdwWiterHp = uVal;
                }
                break;
            case HERO_ATTR.HERO_ATTR_WAITER_LEADER:       // 领导力侍从
                {
                    if (mmapExtData.ContainsKey(dwId))
                        mmapExtData[dwId].mdwWiterLeader = uVal;
                }
                break;
            default: return false;
        }

        return true;
    }

    public void _OnChange(ushort wMsgId, object ar)
    {
        bool bRet = MSG.Sgt.CheckMessageId<MSG_HERO_CHANGE_EVENT>(wMsgId);
        if (!bRet)
            return;

        MSG_HERO_CHANGE_EVENT e = (MSG_HERO_CHANGE_EVENT)ar;
        foreach (var i in e.lst)
        {
            if (i.idHero > 0)
            {
                if (e.tag > 0) // del item
                {
                    int removeid = (int)i.idHero;
                    UICardMgr.singleton.RemoveIllustratedItemById((int)removeid);
                    DelItemById(i.idHero);
                }
                else  // add item
                {
                    AddItemByIdAndTypeId(i.idHero, i.idHeroType);


                    MSG_ACQUIRE_HERO_RESPONSE rsponse = new MSG_ACQUIRE_HERO_RESPONSE();
                    rsponse.idHero = i.idHero;
                    rsponse.idHeroType = i.idHeroType;

                    SLG.EventArgs obj = new SLG.EventArgs((object)rsponse);
                    SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyReceivRewardCard, obj);
                }
            }
        }

    }

    public bool isHaveItem(uint nId)
	{
		if (this.mmapData == null )
		{
			return false;
		}

        bool bHave = this.mmapData.ContainsKey(nId);

		return bHave;
	}

    bool AddItemByIdAndTypeId(uint dwId, uint dwTypeId)
    {
        if (isHaveItem(dwId))
        {
            return false;
        }

        ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CVS_HERO_BASE);
        if (cb == null)
            return false;

        ConfigRow cfRow = cb.getRow(enCVS_HERO_BASE_ATTRIBUTE.HERO_TYPEID, (int)dwTypeId);
        if (cfRow == null)
            return false;


        int nSkillTypeId = cfRow.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL1_TYPEID);// cd.getAttributeIntValue(CardData.enAttributeName.enAN_Skill01);
        int nSkillTypeId2 = cfRow.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL1_TYPEID);
        int nSkillTypeId3 = 3;

        HERO_INFO data = new HERO_INFO();
        data.idHero = dwId;
        data.idType = dwTypeId;

        data.u8Star = CConstance.STAR_ID;
        data.usLevel = CConstance.LEVEL_ID;
        data.unExp = 0;

        data.u8Status = 0;
//        data.u8Weapon = 0;
        
        data.unSkill1 = (uint)nSkillTypeId;
        data.usSkillLvl1 = CConstance.LEVEL_ID;
        data.unSkillExp1 = 0;

        data.unSkill2 = (uint)nSkillTypeId2;
        data.usSkillLvl2 = CConstance.LEVEL_ID;
        data.unSkillExp2 = 0;

        data.unSkill3 = (uint)nSkillTypeId3;
        data.usSkillLvl3 = CConstance.LEVEL_ID;
        data.unSkillExp3 = 0;

        mmapData.Add(dwId, data);

        ItemExternData ied = new ItemExternData();
        mmapExtData.Add(dwId, ied);

        return true;
    }

	bool DelItemById(uint nId)
	{
        if (!isHaveItem(nId))
			return false;

        this.mmapData.Remove(nId);
        this.mmapExtData.Remove(nId);

		return false;
	}

	void CleanItem()
	{
        if (this.mmapData != null)
            this.mmapData.Clear();

        if (mmapExtData != null)
            mmapExtData.Clear();
	}

    public bool getItemById(uint nId, ref HERO_INFO hi)
	{
		if(!isHaveItem(nId))
            return false;

        hi = mmapData[nId];

        return true;
	}

    public bool getItemByIndex(int nIndex, ref HERO_INFO hi)
    {
        int nOffset = 0;
        foreach (KeyValuePair<uint, HERO_INFO> item in mmapData)
        {
            if (nOffset == nIndex)
            {
                hi = item.Value;
                return true;
            }

            nOffset++;
        }

        //Nullable<HERO_INFO> empty = null;
        return false;
    }

    public bool getItemExById(uint dwId, out ItemExternData ied)
    {
        ied = null;
        if (!mmapExtData.ContainsKey(dwId))
            return false;

        ied = mmapExtData[dwId];
        return true;
    }

    public bool getItemExByIndex(int nIndex, out ItemExternData ied)
    {
        ied = null;
        int nOffset = 0;
        foreach (KeyValuePair<uint, ItemExternData> item in mmapExtData)
        {
            if (nOffset == nIndex)
            {
                ied = item.Value;
                return true;
            }
            nOffset++;
        }

        return false;
    }

	public void clean()
	{
//		CancelInvoke("Process");
    }

    void _MsgCheckIn(ushort wMsgId, object ar)
    {
        bool bRet = MSG.Sgt.CheckMessageId<MSG_CLIENT_DAY_LOGIN_AWARD>(wMsgId);
        if (!bRet)
            return;

        mclsCheckData._stData = (MSG_CLIENT_DAY_LOGIN_AWARD)ar;
        mclsCheckData._isHave = true;
        //MSG_CLIENT_DAY_LOGIN_AWARD msg = 

        //CheckinPanel cp = UI.PanelManage.me.GetPanel<CheckinPanel>(UI.PanelID.CheckInPanel);
        ////if (!cp.IsVisible())
        ////    cp.SetVisible(true);

        //cp.flush(ar);
    }

    #endregion


    #region ------ public static function ------

    static public bool getHeroData(uint dwId, ref Packet.HERO_INFO hi)
    {
        if (DataManager.getHeroData() == null)
            return false;

        HeroData hd = DataManager.getHeroData();
        bool bRet = hd.getItemById(dwId, ref hi);

        return bRet;
    }

    static public bool getHeroData(uint dwId, out HeroData.ItemExternData ied)
    {
        ied = null;
        if (DataManager.getHeroData() == null)
            return false;

        HeroData hd = DataManager.getHeroData();
        bool bRet = hd.getItemExById(dwId, out ied);

        return bRet;
    }

    static public bool getHeroData(int nIndex, ref Packet.HERO_INFO hi)
    {
        if (DataManager.getHeroData() == null)
            return false;

        HeroData hd = DataManager.getHeroData();
        bool bRet = hd.getItemByIndex(nIndex, ref hi);

        return bRet;
    }

    static public bool getHeroData(int nIndex, out HeroData.ItemExternData ied)
    {
        ied = null;
        if (DataManager.getHeroData() == null)
            return false;

        HeroData hd = DataManager.getHeroData();
        bool bRet = hd.getItemExByIndex(nIndex, out ied);

        return bRet;
    }

    #endregion



}

}