using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using UI;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
public class AttendantsPanel : PanelBase
{
    public const PanelID id = PanelID.AttendantsPanel;

    private uint mdwCurSelId = CConstance.DEFAULT_ID;
    public int CurrentSelectId { get { return (int)mdwCurSelId; } }

    private uint mdwCurSetId = CConstance.DEFAULT_ID;
    private int mnCurSetIndex = CConstance.INVALID_ID;
    public int mnUpgradeCount = 1;

    private List<uint> mlistAttendantsId = new List<uint>();

    private GameObject mgoHangHeroRoot = null;
    private GameObject mgoDetailRoot = null;

    private GameObject mgoItemHeroPrefab = null;
    private GameObject mgoItemHangPoint = null;

    private UISprite mSpriteCard = null;
    private UILabel mlabelGold = null;
    private UISprite mSpriteBar = null;

    private List<Transform> mlistSelIcon = new List<Transform>();
    private List<Transform> mlistCueIcon = new List<Transform>();

    private UIButton mbtnHangHeroConfirm = null;
    public UIButton HangHeroConfirmBtn { get { return mbtnHangHeroConfirm; } }


    public AttendantsPanel()
    {
        _RetsetInit();
    }

    void _RetsetInit()
    {
        mdwCurSelId = 0;
        mlistAttendantsId.Clear();
    }

    public override PanelID GetPanelID()
    {
        return id;
    }

    public override string GetResPath()
    {
        return "";
    }

    protected override void Initimp(List<GameObject> prefabs)
    {
        mgoDetailRoot = UICardMgr.findChild(Root.transform, "DetailRoot").gameObject;
        mgoHangHeroRoot = UICardMgr.findChild(Root.transform, "HangHeroRoot").gameObject;

        mgoItemHeroPrefab = prefabs[1];
        mgoItemHangPoint = UICardMgr.findChild(Root.transform, "HangHeroRoot,DetailRoot,HangArea").gameObject;

        mlabelGold = UICardMgr.FindChild<UILabel>(Root.transform, "LeftArea,goldCoin,goldNo");
        mSpriteBar = UICardMgr.FindChild<UISprite>(Root.transform, "LeftArea,goldCoin,goldProcessBar");
        mSpriteCard = UICardMgr.FindChild<UISprite>(Root.transform, "LeftArea,Sprite");

        Transform uitemp = null;
        // cue icon button
        uitemp = UICardMgr.findChild(Root.transform, "DetailRoot,Introduction,Backgroud,CueRoot,iconbtn01");
        mlistCueIcon.Add(uitemp);

        uitemp = UICardMgr.findChild(Root.transform, "DetailRoot,Introduction,Backgroud,CueRoot,iconbtn02");
        mlistCueIcon.Add(uitemp);

        uitemp = UICardMgr.findChild(Root.transform, "DetailRoot,Introduction,Backgroud,CueRoot,iconbtn03");
        mlistCueIcon.Add(uitemp);

        uitemp = UICardMgr.findChild(Root.transform, "DetailRoot,Introduction,Backgroud,CueRoot,iconbtn04");
        mlistCueIcon.Add(uitemp);

        // select icon button
        uitemp = UICardMgr.findChild(Root.transform, "DetailRoot,Introduction,Backgroud,SelRoot,iconbtn01");
        mlistSelIcon.Add(uitemp);

        uitemp = UICardMgr.findChild(Root.transform, "DetailRoot,Introduction,Backgroud,SelRoot,iconbtn02");
        mlistSelIcon.Add(uitemp);

        uitemp = UICardMgr.findChild(Root.transform, "DetailRoot,Introduction,Backgroud,SelRoot,iconbtn03");
        mlistSelIcon.Add(uitemp);

        uitemp = UICardMgr.findChild(Root.transform, "DetailRoot,Introduction,Backgroud,SelRoot,iconbtn04");
        mlistSelIcon.Add(uitemp);

        foreach (var item in mlistSelIcon)
        {
            UIEventListener.Get(item.gameObject).onClick = _OnOpenHeroListClick;
        }

        uitemp = UICardMgr.findChild(Root.transform, "BackgroundRoot,close");
        if (uitemp != null)
        {
            UIEventListener.Get(uitemp.gameObject).onClick = _OnClose;
        }

        uitemp = UICardMgr.findChild(Root.transform, "HangHeroRoot,TitleRoot,close");
        if (uitemp != null)
        {
            UIEventListener.Get(uitemp.gameObject).onClick = _OnCloseHangHeroList;
        }

        uitemp = UICardMgr.findChild(Root.transform, "HangHeroRoot,TitleRoot,confirmBtn");
        if (uitemp != null)
        {
            mbtnHangHeroConfirm = uitemp.GetComponent<UIButton>();
            UIEventListener.Get(uitemp.gameObject).onClick = _OnConfirmSelHangHero;
        }

        GlobalEventSet.SubscribeEvent(eEventType.NodifyUpdateWaiter, this._FreshIcon);

        this.SetVisible(false);

        _FlushTableText();
    }

    void _FlushTableText()
    {
        UICardMgr.setLabelText(Root, "HangHeroRoot,TitleRoot,Sprite,Label", 14020);

        UICardMgr.setLabelText(Root, "HangHeroRoot,TitleRoot,confirmBtn,Label", 14018);
    }

    protected override void ReleaseImp()
    {

    }

    protected override void onShow()
    {
        base.onShow();

        _ResetInitControlData();
    }

    void _ResetInitControlData()
    {
        mdwCurSetId = CConstance.DEFAULT_ID;

        if (mlabelGold != null)
        {
            mlabelGold.text = RewardPanel.getCoins().ToString();
        }

        if (mSpriteBar != null)
        {
            mSpriteBar.fillAmount = 1f;
        }

        _SetRootDetailVisible(true);
    }

    void _OnOpenHeroListClick(GameObject go)
    {
        _SetRootDetailVisible(false);

        int n = CConstance.DEFAULT_ID;
        foreach (var item in mlistSelIcon)
        {
            if (go == item.gameObject)
            {
                mnCurSetIndex = n;
                break;
            }
            n++;
        }
    }

    void _OnClose(GameObject go)
    {
        //this.SetVisible(false);

        CardIllustratedDetailPanel cdp = PanelManage.me.GetPanel<CardIllustratedDetailPanel>(PanelID.CardIllustratedDetailPanel);
        if (cdp != null)
        {
            cdp.SetVisible(true);
        }
    }

    void _OnCloseHangHeroList(GameObject go)
    {
        _SetRootDetailVisible(true);
    }

    void _OnConfirmSelHangHero(GameObject go)
    {
        _SetRootDetailVisible(true);

        _UpdateSelIcon();
    }

    void _SetRootDetailVisible(bool b)
    {
        if (mgoDetailRoot != null)
        {
            mgoDetailRoot.SetActive(b);
        }

        if (mgoHangHeroRoot != null)
        {
            mgoHangHeroRoot.SetActive(!b);
        }
    }

    public void setId(uint dwId)
    {
        mdwCurSelId = dwId;

        _InitList();

        _UpdateDetail();
    }

    void _UpdateDetail()
    {
        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData(mdwCurSelId, ref hi);
        if (!bRet)
            return;

        List<int> lstTypeId = new List<int>();
        bRet = CHerroTalbeAttribute.getHeroGayTypeIdByTypeId((int)hi.idType, ref lstTypeId);
        if(!bRet)
            return;

        // left icon
        _UpdateIconDetailByTypeId((int)hi.idType, mSpriteCard.transform, false);

        // cue icon
        int nIndex = 0;
        int nTypeid = lstTypeId[0]; // cha.getAttributeIntValue(CTableHeroAttendants.enAttributeName.enAN_UseTypeId01);
        Transform tfCue = UICardMgr.findChild(mlistCueIcon[nIndex], "png,Sprite");
        _UpdateIconDetailByTypeId(nTypeid, tfCue);
        _UpdateIconVisible(nTypeid, mlistCueIcon[nIndex++]);

        nTypeid = lstTypeId[1]; // cha.getAttributeIntValue(CTableHeroAttendants.enAttributeName.enAN_UseTypeId02);
        tfCue = UICardMgr.findChild(mlistCueIcon[nIndex], "png,Sprite");
        _UpdateIconDetailByTypeId(nTypeid, tfCue);
        _UpdateIconVisible(nTypeid, mlistCueIcon[nIndex++]);

        nTypeid = lstTypeId[2]; // cha.getAttributeIntValue(CTableHeroAttendants.enAttributeName.enAN_UseTypeId03);
        tfCue = UICardMgr.findChild(mlistCueIcon[nIndex], "png,Sprite");
        _UpdateIconDetailByTypeId(nTypeid, tfCue);
        _UpdateIconVisible(nTypeid, mlistCueIcon[nIndex++]);

        nTypeid = lstTypeId[3]; // cha.getAttributeIntValue(CTableHeroAttendants.enAttributeName.enAN_UseTypeId04);
        tfCue = UICardMgr.findChild(mlistCueIcon[nIndex], "png,Sprite");
        _UpdateIconDetailByTypeId(nTypeid, tfCue);
        _UpdateIconVisible(nTypeid, mlistCueIcon[nIndex++]);

        // select icon
        nIndex = 0;
        HeroData.ItemExternData ied = null;
        bRet = HeroData.getHeroData(mdwCurSelId, out ied);
        if (ied != null)
        {
            nTypeid = (int)ied.mdwWiterAtkId;
            Transform tf = UICardMgr.findChild(mlistSelIcon[nIndex], "png,Sprite");
            _UpdateIconDetailByTypeId(nTypeid, tf);
            _UpdateIconVisible(nTypeid, mlistSelIcon[nIndex++]);

            nTypeid = (int)ied.mdwWiterDef;
            tf = UICardMgr.findChild(mlistSelIcon[nIndex], "png,Sprite");
            _UpdateIconDetailByTypeId(nTypeid, tf);
            _UpdateIconVisible(nTypeid, mlistSelIcon[nIndex++]);

            nTypeid = (int)ied.mdwWiterHp;
            tf = UICardMgr.findChild(mlistSelIcon[nIndex], "png,Sprite");
            _UpdateIconDetailByTypeId(nTypeid, tf);
            _UpdateIconVisible(nTypeid, mlistSelIcon[nIndex++]);

            nTypeid = (int)ied.mdwWiterLeader;
            tf = UICardMgr.findChild(mlistSelIcon[nIndex], "png,Sprite");
            _UpdateIconDetailByTypeId(nTypeid, tf);
            _UpdateIconVisible(nTypeid, mlistSelIcon[nIndex++]);
        }

    }

    void _ResetIconVisible()
    {
        foreach (var item in mlistSelIcon)
        {
            _UpdateIconVisible(CConstance.DEFAULT_ID, item);
        }

        foreach (var item in mlistCueIcon)
        {
            _UpdateIconVisible(CConstance.DEFAULT_ID, item);
        }
    }

    void _UpdateIconVisible(int nTypeId, Transform tf, bool b = true)
    {
        if (nTypeId == CConstance.DEFAULT_ID)
            b = false;

        Transform tfTemp = UICardMgr.findChild(tf, "png,nullIcon");
        if (tfTemp != null)
        {
            tfTemp.gameObject.SetActive(!b);
        }

        tfTemp = UICardMgr.findChild(tf, "png,Sprite");
        if (tfTemp != null)
        {
            tfTemp.gameObject.SetActive(b);
        }

        tfTemp = UICardMgr.findChild(tf, "Label");
        if (tfTemp != null)
        {
            UILabel uil = tfTemp.GetComponent<UILabel>();
            if (uil != null)
            {
                uil.enabled = b;
            }
        }
    }

    bool _UpdateIconDetailByTypeId(int nTypeId, Transform tf, bool bSmall = true)
    {
        if (nTypeId == CConstance.DEFAULT_ID || tf == null)
            return false;

        int nIconIndex = 0;
        bool bRet = CHerroTalbeAttribute.getHeroIconSpriteName(nTypeId, ref nIconIndex);
        if (!bRet)
            return false;

        int n = nIconIndex - 1;

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        UISprite uis = tf.GetComponent<UISprite>();
        if (uis != null)
        {
            if (bSmall)
                uis.spriteName = UICardLogic.mszCardNameSmall[n];
            else
                uis.spriteName = UICardLogic.mszQualityName[n];

            uis = UICardMgr.FindChild<UISprite>(uis.transform, "Star");
            if (uis != null)
            {
                if (bSmall)
                    uis.spriteName = UICardLogic.mszCardNameStarSmall[n];
                else
                    uis.spriteName = UICardLogic.mszQualityNameStar[n];

                bRet = HeroData.getHeroData(mdwCurSelId, ref hi);
                if (!bRet)
                    uis.fillAmount = 0.0f;
                else
                    uis.fillAmount = HeroData.FillValue * hi.u8Star;
            }
        }

        return true;
    }

    void _InitList()
    {
        _RemoveList();
        _ResetIconVisible();

        int nIndex = 0, nx = 190, ny = -30, nOffsetY = 120;
        int nAmount = UICardMgr.singleton.illustratedItemAmount;
        for (int i = 0; i < nAmount; i++)
        {
            UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemByIndex(i);
            if (cid == null)
                continue;

            if (cid.nId == mdwCurSelId)
                continue;

            GameObject goTemp = NGUITools.AddChild(mgoItemHangPoint, mgoItemHeroPrefab);
            goTemp.transform.parent = mgoItemHangPoint.transform;

            UICardQualityItem uicqi = goTemp.AddComponent<UICardQualityItem>();
            if (uicqi != null)
            {
                uicqi.mclsItemData = cid;
                uicqi.menPanelId = GetPanelID();
            }

            goTemp.transform.localPosition = new UnityEngine.Vector3((float)nx, (float)(ny - nIndex * nOffsetY), 0.0f);

            UITable uit = mgoItemHangPoint.GetComponent<UITable>();
            if (uit != null)
            {
                uit.repositionNow = true;
            }

            nIndex++;
        }
    }

    void _RemoveList()
    {
        foreach (Transform item in mgoItemHangPoint.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
    }

    void _UpdateSelIcon()
    {
        int nTypeId = CConstance.DEFAULT_ID;

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData(mdwCurSelId, ref hi);
        if (!bRet)
            return;
        nTypeId = (int)hi.idType;

        // set icon
        HeroData.ItemExternData ied;
        bRet = HeroData.getHeroData(mdwCurSelId, out ied);
        if (!bRet)
            return;

        {
            foreach (Transform item in mgoItemHangPoint.transform)
            {
                UICardQualityItem uicqi = item.GetComponent<UICardQualityItem>();
                if (uicqi != null && uicqi.mbSelect)
                {
                    mdwCurSetId = (uint)uicqi.mclsItemData.nId;
                    break;
                }
            }

            Packet.MSG_WAITER_REQUEST msg = new Packet.MSG_WAITER_REQUEST();

            msg.idMajorHero = mdwCurSelId;

            if (mnCurSetIndex == 0)
                msg.idAtkWaiter = mdwCurSetId;
            else
                msg.idAtkWaiter = ied.mdwWiterAtkId;

            if (mnCurSetIndex == 1)
                msg.idDefenceWaiter = mdwCurSetId;
            else
                msg.idDefenceWaiter = ied.mdwWiterDef;

            if (mnCurSetIndex == 2)
                msg.idHpWaiter = mdwCurSetId;
            else
                msg.idHpWaiter = ied.mdwWiterHp;

            if (mnCurSetIndex == 3)
                msg.idLeaderWaiter = mdwCurSetId;
            else
                msg.idLeaderWaiter = ied.mdwWiterLeader;

            DataMgr.DataManager.MsgHeroCard.questWaiter(ref msg);
        }
    }

    public int getSelectAmount()
    {
        int nCnt = 0;
        foreach (Transform item in mgoItemHangPoint.transform)
        {
            UICardQualityItem uicqi = item.GetComponent<UICardQualityItem>();
            if (uicqi != null && uicqi.mbSelect)
                nCnt++;
        }

        if (mbtnHangHeroConfirm != null)
        {
            if (nCnt >= mnUpgradeCount)
            {
                mbtnHangHeroConfirm.enabled = true;
            }
            else
            {
                mbtnHangHeroConfirm.enabled = false;
            }
        }

        return nCnt;
    }

    bool _FreshIcon(SLG.EventArgs obj)
    {
        int nTypeId = CConstance.DEFAULT_ID;

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData(mdwCurSelId, ref hi);
        if (!bRet)
            return false;
        nTypeId = (int)hi.idType;

        bool b = (bool)obj.m_obj;
        if (!b)
        {
            nTypeId = CConstance.DEFAULT_ID;
            MessageBoxMgr.ShowMessageBox("", "绑定失败", null, null);
        }
        //CardData cd = CsvConfigMgr.me.getHeroDetailByTypeId((int)hi.idType);
        //if (cd == null)
        //    return;

        UISprite uis = UICardMgr.FindChild<UISprite>(mlistSelIcon[mnCurSetIndex], "png,Sprite");
        _UpdateIconDetailByTypeId(nTypeId, uis.transform);

        _UpdateIconVisible(nTypeId, mlistSelIcon[mnCurSetIndex]);

        // reset cursetid
        mdwCurSetId = CConstance.DEFAULT_ID;
        mnCurSetIndex = CConstance.INVALID_ID;

        return true;
    }
}
