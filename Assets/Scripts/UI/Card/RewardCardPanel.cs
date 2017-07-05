using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
namespace UI
{

public class RewardCardPanel : PanelBase
{
    public const PanelID id = PanelID.RewardCardPanel;

    public Transform mtfChooseDetialRoot = null;
    public Transform mtfChooseResultRoot = null;
    public Transform mtfChooseRoot = null;

    Transform mtfClose = null;
    UILabel mlabelDimond = null;
    UILabel mlabelMagicStone = null;

    List<Transform> mlistOriginal = new List<Transform>();
    List<Transform> mlistReward = new List<Transform>();
    Transform mtfCurSelect = null;
    Transform mtfCurSelectBg = null;
    Transform mtfCurClick = null;

    List<Transform> mlistBtn = new List<Transform>();
    Transform mtfLabel = null;

    bool mbAnimate = true;

    public float mfScale = 0.7f;
    public Vector3 mvDetailPosition = new Vector3(0f, -34f, 0f);

    public int mnRewardStyle = CConstance.INVALID_ID;
    Packet.MSG_ACQUIRE_HERO_RESPONSE mclsRestMsg;

    public RewardCardPanel()
    {
    }

    public override string GetResPath()
    {
        return "RewardWin.prefab";
    }

    protected override void Initimp(List<GameObject> prefabs)
    {
        mtfClose = UICardMgr.findChild(Root.transform, "chooseAcard,close");
        if (mtfClose != null)
        {
            UIEventListener.Get(mtfClose.gameObject).onClick = _OnCloseClick;
        }

        // dimond
        Transform tf = UICardMgr.findChild(Root.transform, "chooseAcard,dimond,dimNo");
        if (tf != null)
        {
            mlabelDimond = tf.GetComponent<UILabel>();
        }

        // magic stone
        tf = UICardMgr.findChild(Root.transform, "chooseAcard,magic stone,stoneNo");
        if (tf != null)
        {
            mlabelMagicStone = tf.GetComponent<UILabel>();
        }

        mtfChooseRoot = UICardMgr.findChild(Root.transform, "chooseAcard,ChooseRoot");
        if (mtfChooseRoot != null)
        {
            int nIndex = 1;
            string strBase = "cardBack";

            for (int i = 0; i < mtfChooseRoot.childCount; i++ )
            {
                string str = strBase + nIndex.ToString();
                tf = UICardMgr.findChild(mtfChooseRoot, str);
                if (tf != null)
                {
                    RewardCardLogic rcl = tf.gameObject.AddComponent<RewardCardLogic>();
                    if (rcl != null)
                    {
                        rcl.mIndex = nIndex;
                        Logger.LogDebug("RewardCardPanel::InitImp  add rewardCardLogic succ");
                    }

                    UIEventListener.Get(tf.gameObject).onClick = _OnItemClick;

                    nIndex++;
                    mlistOriginal.Add(tf);
                }
            }
        }

        mtfChooseResultRoot = UICardMgr.findChild(Root.transform, "chooseAcard,ChooseResultRoot");
        if (mtfChooseResultRoot != null)
        {
            mtfCurSelect = UICardMgr.findChild(mtfChooseResultRoot, "Icon");
            mtfCurSelect.gameObject.AddComponent<RewardCardAnimate>();

            UIEventListener.Get(mtfCurSelect.gameObject).onClick = _OnDetailItemClick;

            int nIndex = 1;
            string strBase = "ItemCard0";

            for (int i = 0; i < mtfChooseResultRoot.childCount; i++)
            {
                string str = strBase + nIndex.ToString();
                tf = UICardMgr.findChild(mtfChooseResultRoot, str);
                if (tf != null)
                {
                    UIEventListener.Get(tf.gameObject).onClick = _OnItemResultClick;

                    nIndex++;
                    mlistReward.Add(tf);
                }
            }
        }

        mtfChooseDetialRoot = UICardMgr.findChild(Root.transform, "chooseAcard,ChooseDetialRoot");
        if (mtfChooseResultRoot != null)
        {
            mtfCurSelectBg = UICardMgr.findChild(mtfChooseDetialRoot, "bg");
            if (null != mtfCurSelectBg)
            {
                UIEventListener.Get(mtfCurSelectBg.gameObject).onClick = _OnDetailItemClick;
            }
        }

        tf = UICardMgr.findChild(Root.transform, "chooseAcard,BtnRoot,Other");
        if (tf != null)
        {
            mlistBtn.Add(tf);
            UIEventListener.Get(tf.gameObject).onClick = _OnOtherBtnClick;
        }

        tf = UICardMgr.findChild(Root.transform, "chooseAcard,BtnRoot,Try");
        if (tf != null)
        {
            mlistBtn.Add(tf);
            UIEventListener.Get(tf.gameObject).onClick = _OnTryBtnClick;
        }

        mtfLabel = UICardMgr.findChild(Root.transform, "chooseAcard,BtnRoot,ClickToSelectAcard");

        SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyOpenRewardCard, this._OnListItem);
        SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyOpenRewardCardBg, this._OnOpenDetailBg);
        SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyReceivRewardCard, this._OnRewardCard);

        SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyRefreshMoney, this._RefreshMoney);
        SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyAutoOpenRewardItem, this._AutoOpenRewardItem);

        SetVisible(false);

        ResetListItemVisible();

        _FlushTableText();
    }

    void _FlushTableText()
    {
        UICardMgr.setLabelText(Root, "chooseAcard,tavernTit", 14000);
        UICardMgr.setLabelText(Root, "chooseAcard,BtnRoot,ClickToSelectAcard", 14006);
        UICardMgr.setLabelText(Root, "chooseAcard,BtnRoot,Other,Sprite,Label", 14007);
        UICardMgr.setLabelText(Root, "chooseAcard,BtnRoot,Try,Sprite,Label", 14008);
    }

    public override PanelID GetPanelID()
    {
        return id;
    }

    protected override void onShow()
    {
        base.onShow();

        _RefreshMoney(null);
    }

    bool _RefreshMoney(SLG.EventArgs arg)
    {
        mlabelDimond.text = RewardPanel.getDimonds().ToString();

        mlabelMagicStone.text = RewardPanel.getMagicStones().ToString();

        return true;
    }

    protected override void ReleaseImp()
    {

    }

    void _OnCloseClick(GameObject go)
    {
        mnRewardStyle = CConstance.INVALID_ID;

        this.SetVisible(false);
        ResetListItemVisible();

        for (int i = 0; i < mlistOriginal.Count; i++)
        {
            RewardCardLogic rcl = mlistOriginal[i].GetComponent<RewardCardLogic>();
            if (rcl.mTweenRotate == null)
                continue;
            rcl.mTweenRotate.enabled = false;
            rcl.mTweenRotate = null;
            mlistOriginal[i].rotation = Quaternion.identity;
        }

        RewardCardAnimate rca = mtfCurSelect.GetComponent<RewardCardAnimate>();
        if (rca.mTPosition != null)
            rca.mTPosition.enabled = false;
        if (rca.mTScale != null)
            rca.mTScale.enabled = false;
    }

    void _OnItemClick(GameObject go)
    {
        if (mtfCurClick != null)
            return;

        bool bCan = false;
        Packet.HERO_ACQUIRE en = (Packet.HERO_ACQUIRE)mnRewardStyle;
        switch (en)
        {
            case Packet.HERO_ACQUIRE.HERO_ACQUIRE_PVE:
                break;
            case Packet.HERO_ACQUIRE.HERO_ACQUIRE_STONE:
                if (RewardPanel.getMagicStones() >= RewardPanel.RewardOnceCostValue)
                    bCan = true;
                else
                    MessageBoxMgr.ShowMessageBox("", "enough stones", null, null);
                break;
            case Packet.HERO_ACQUIRE.HERO_ACQUIRE_MONEY:
                if (RewardPanel.getDimonds() >= RewardPanel.RewardOnceCostValue)
                    bCan = true;
                else
                    MessageBoxMgr.ShowMessageBox("", "enough money", null, null);
                break;
            case Packet.HERO_ACQUIRE.HERO_ACQUIRE_FREE:
                if (IsCanFreeReward())
                {
                    bCan = true;
                }
                else
                {
                    MessageBoxMgr.ShowMessageBox("", "no free count", null, null);
                }
                break;
            default:
                break;
        }

        if (bCan)
        {
            DataMgr.DataManager.MsgHeroCard.questEveryDayCard(en);
            mtfCurClick = go.transform;
        }
    }

    bool _OnRewardCard(SLG.EventArgs obj)
    {
        mclsRestMsg = (Packet.MSG_ACQUIRE_HERO_RESPONSE)obj.m_obj;

        RewardCardLogic rcl = mtfCurClick.GetComponent<RewardCardLogic>();
        if (rcl != null)
        {
            rcl.mbSelect = true;

            mbAnimate = true;
        }

        return true;
    }

    void _OnItemResultClick(GameObject go)
    {
//         mtfChooseResultRoot.gameObject.SetActive(false);
//         mtfChooseRoot.gameObject.SetActive(true);
    }

    void _OnDetailItemClick(GameObject go)
    {
        ResetDetialRootVisible();

        for (int i = 0; i < mlistOriginal.Count; i++)
        {
            GameObject goTemp = mlistOriginal[i].gameObject;

            if(mtfCurClick == goTemp.transform)
                continue;

            RewardCardLogic rcl = goTemp.GetComponent<RewardCardLogic>();
            if (rcl != null)
            {
                rcl.mbSelect = true;
                rcl.mbRandom = true;
            }
        }

        mbAnimate = false;
        //mtfCurClick = null;
    }

    bool _OnListItem(SLG.EventArgs obj)
    {
        int nIndex = (int)obj.m_obj;
        nIndex--;

        for (int i = 0; i < mlistOriginal.Count; i++)
        {
            if (nIndex == i)
            {
                mlistOriginal[i].gameObject.SetActive(false);
                break;
            }
        }

        Transform tf = null;
        for (int i = 0; i < mlistReward.Count; i++)
        {
            if (nIndex == i)
            {
                tf = mlistReward[i];
                tf.gameObject.SetActive(true);
                if (mbAnimate)
                {
                    //SetBtnVisible(true);
                    SetLabelVisible(false);
                    SetCurSelectVisible(false);

                    SetRewardCardByIndex(nIndex);

                    UISprite uis = UICardMgr.FindChild<UISprite>(mlistReward[i], "Sprite,StarSelect");//
                    if (uis != null)
                    {
                        uis.gameObject.SetActive(true);
                    }
                }
                else
                {
                    _OnRandomSprite(i);

                    UISprite uis = UICardMgr.FindChild<UISprite>(mlistReward[i], "Sprite,StarSelect");//
                    if (uis != null)
                    {
                        uis.gameObject.SetActive(false);
                    }
                }
                break;
            }
        }

//         if (mbAnimate)
//         {
//             SetRewardCardByIndex(nIndex);
//         }

        return true;
    }

    void SetRewardCardByIndex(int nIndex)
    {
        if (nIndex >= mlistReward.Count || nIndex <= CConstance.INVALID_ID)
            return;

        UISprite uisDest = mlistReward[nIndex].GetChild(0).GetComponent<UISprite>();
        if (uisDest != null)
        {
            //UICardMgr.CItemData cd = UICardMgr.singleton.getIllustratedItemById((int)mclsRestMsg.idHero);
            //CardData cd = CsvConfigMgr.me.getHeroDetailByTypeId((int)mclsRestMsg.idHeroType);

            DataMgr.ConfigRow cr = null;
            DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)mclsRestMsg.idHeroType, out cr);

            int nTemp = 0; // Convert.ToInt32(cd.getAttributeStringValue(CardData.enAttributeName.enAN_SpriteName));
            int n = 0;     ////UnityEngine.Random.Range(0, mszCardNameSmall.Length);
            if (cr != null)
            {
                nTemp = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.ICON_SPRITE_NAME);
                n = nTemp - 1;//UnityEngine.Random.Range(0, mszCardNameSmall.Length);
            }


            uisDest.spriteName = UICardLogic.getSpriteNameSmall((int)mclsRestMsg.idHeroType);// mszQualityName[n];
            UISprite uisDestStar = UICardMgr.FindChild<UISprite>(uisDest.transform, "Star");
            uisDestStar.spriteName = UICardLogic.getStarNameSmall((int)mclsRestMsg.idHeroType);// mszQualityNameStar[n];
            uisDestStar.fillAmount = 0.0f;

            UISprite retUIS = mtfCurSelect.GetComponent<UISprite>();
            retUIS.spriteName = UICardLogic.getSpriteNameBig((int)mclsRestMsg.idHeroType);

            UISprite uisStar = mtfCurSelect.GetChild(0).GetComponent<UISprite>();
            uisStar.spriteName = UICardLogic.getStarNameBig((int)mclsRestMsg.idHeroType);
            uisStar.fillAmount = 0.0f;

            mtfCurSelect.localScale = new UnityEngine.Vector3(mfScale, mfScale, mfScale);
            mtfCurSelect.localPosition = mlistReward[nIndex].localPosition;

            RewardCardAnimate rca = mtfCurSelect.GetComponent<RewardCardAnimate>();
            if (rca != null)
                rca.mbDoAnimate = true;
        }

        UpdateRewardCount();
    }

    void _OnRandomSprite(int nIndex)
    {
        if (nIndex >= mlistReward.Count || nIndex <= CConstance.INVALID_ID)
            return;

        UISprite uisDest = mlistReward[nIndex].GetChild(0).GetComponent<UISprite>();
        if (uisDest != null)
        {
            int nRandomIndex = UnityEngine.Random.Range(0, UICardLogic.mszQualityName.Length);
            int n = UnityEngine.Random.Range(0, UICardLogic.mszQualityNameStar.Length);


            uisDest.spriteName = UICardLogic.mszCardNameSmall[nRandomIndex];
            UISprite uisDestStar = UICardMgr.FindChild<UISprite>(uisDest.transform, "Star");
            uisDestStar.spriteName = UICardLogic.mszCardNameStarSmall[1];
            uisDestStar.fillAmount = DataMgr.HeroData.FillValue * n;
        }

        if (CheckRotateFinish())
        {
            SetBtnVisible(true);
        }
    }

    void UpdateRewardCount()
    {
        Packet.HERO_ACQUIRE en = (Packet.HERO_ACQUIRE)mnRewardStyle;
        if (en == Packet.HERO_ACQUIRE.HERO_ACQUIRE_FREE)
        {
            RewardPanel rp = PanelManage.me.GetPanel<RewardPanel>(PanelID.RewardPanel);
            if (rp.mnFreeRewardCnt > 0)
            {
                rp.mnFreeRewardCnt--;
            }

            rp.setFreeReward();
        }
    }

    bool IsCanFreeReward()
    {
        RewardPanel rp = PanelManage.me.GetPanel<RewardPanel>(PanelID.RewardPanel);
        if (rp.mnFreeRewardCnt > 0)
            return true;

        return false;
    }

    void ResetListItemVisible()
    {
        for (int i = 0; i < mlistOriginal.Count; i++)
        {
            mlistOriginal[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < mlistReward.Count; i++)
        {
            mlistReward[i].gameObject.SetActive(false);
            UISprite uis = UICardMgr.FindChild<UISprite>(mlistReward[i], "Sprite,StarSelect");//
            if (uis != null)
            {
                uis.gameObject.SetActive(false);
            }
        }

        ResetBtnRootDisable();
        ResetDetialRootVisible();
        SetBtnVisible(false);
        SetLabelVisible(true);

        mtfCurClick = null;

        mbAnimate = false;
    }

    void ResetDetialRootVisible()
    {
        mtfChooseDetialRoot.localPosition = mvDetailPosition;

        mtfCurSelectBg.gameObject.SetActive(false);
        mtfCurSelect.gameObject.SetActive(false);
    }

    void SetCurSelectVisible(bool b)
    {
        mtfCurSelectBg.gameObject.SetActive(b);
        mtfCurSelect.gameObject.SetActive(!b);
    }

    bool _OnOpenDetailBg(SLG.EventArgs obj)
    {
        mtfCurSelectBg.gameObject.SetActive(true);
        return true;
    }

    void _OnOtherBtnClick(GameObject go)
    {
        _OnCloseClick(go);

        RewardPanel rp = PanelManage.me.GetPanel<RewardPanel>(PanelID.RewardPanel);
        rp.SetVisible(true);
    }

    void _OnTryBtnClick(GameObject go)
    {
        ResetListItemVisible();

        SetBtnVisible(false);
        SetLabelVisible(true);
    }

    void SetBtnVisible(bool b)
    {
        foreach (var item in mlistBtn)
        {
            item.gameObject.SetActive(b);
        }
    }

    void ResetBtnRootDisable()
    {
        foreach (var item in mlistBtn)
        {
            item.gameObject.SetActive(false);
        }

        mtfLabel.gameObject.SetActive(false);
    }

    void SetLabelVisible(bool b)
    {
        mtfLabel.gameObject.SetActive(b);
    }

    bool CheckRotateFinish()
    {
        int nCount = 0;
        for (int i = 0; i < mlistOriginal.Count; i++)
        {
            GameObject goTemp = mlistOriginal[i].gameObject;

//             if (mtfCurClick == goTemp.transform)
//                 continue;

            RewardCardLogic rcl = goTemp.GetComponent<RewardCardLogic>();
            if(rcl.mbSelect == false)
            {
                nCount++;
            }
        }

        if (nCount == mlistOriginal.Count)
        {
            return true;
        }

        return false;
    }

    bool _AutoOpenRewardItem(SLG.EventArgs obj)
    {
        _OnDetailItemClick(null);

        return true;
    }
}


}

