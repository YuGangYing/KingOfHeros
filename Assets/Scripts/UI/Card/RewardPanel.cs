using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using SLG;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
public class RewardPanel : PanelBase
{
    public const PanelID id = PanelID.RewardPanel;

    private List<Transform> mlistBtn = new List<Transform>();
    private List<UILabel> mlistLabel = new List<UILabel>();
    private UILabel mlabelDimond = null;
    private UILabel mlabelMagicStone = null;
    private UISprite mSpriteProcessBar = null;

    private bool mbFirstOpen = true;
    private const int mnAmount = 5;
    public int mnFreeRewardCnt = 0;

    public RewardPanel()
    {
    }

    public override string GetResPath()
    {
        return "RewardWin.prefab";
    }

    protected override void Initimp(List<GameObject> prefabs)
    {
        Transform tf = UICardMgr.findChild(Root.transform, "tavern,close");
        if (tf != null)
        {
            mlistBtn.Add(tf);
            UIEventListener.Get(tf.gameObject).onClick = _OnCloseClick;
        }

        tf = UICardMgr.findChild(Root.transform, "tavern,tavernHeroList,tavernHeroList");
        if (tf != null)
        {
            mlistBtn.Add(tf);
            UIEventListener.Get(tf.gameObject).onClick = _OnOpenHeroListClick;
        }

        tf = UICardMgr.findChild(Root.transform, "tavern,dimond");
        if (tf != null)
        {
            mlistBtn.Add(tf);
            UIEventListener.Get(tf.gameObject).onClick = _OnCloseClick;
        }

        tf = UICardMgr.findChild(Root.transform, "tavern,dimond,dimNo");
        if (tf != null)
        {
            mlabelDimond = tf.GetComponent<UILabel>();
        }

        // magic stone
        tf = UICardMgr.findChild(Root.transform, "tavern,magic stone,stoneNo");
        if (tf != null)
        {
            mlabelMagicStone = tf.GetComponent<UILabel>();
        }

        tf = UICardMgr.findChild(Root.transform, "tavern,magic stone,stoneProcessBar");
        if (tf != null)
        {
            mSpriteProcessBar = tf.GetComponent<UISprite>();
        }

        tf = UICardMgr.findChild(Root.transform, "tavern,dimondDrawGroup");
        if (tf != null)
        {
            mlistBtn.Add(tf);
            UIEventListener.Get(tf.gameObject).onClick = _OnDimondClick;
        }
        tf = UICardMgr.findChild(Root.transform, "tavern,dimondDrawGroup,dimondDrawTime");
        if (tf != null)
        {
            tf.GetComponent<UILabel>().text = "";
            mlistLabel.Add(tf.GetComponent<UILabel>());
        }

        tf = UICardMgr.findChild(Root.transform, "tavern,freeDrawGroup");
        if (tf != null)
        {
            mlistBtn.Add(tf);
            UIEventListener.Get(tf.gameObject).onClick = _OnFreeCardClick;
        }
        tf = UICardMgr.findChild(Root.transform, "tavern,freeDrawGroup,freeDrawTime");
        if (tf != null)
        {
            tf.GetComponent<UILabel>().text = "";
            mlistLabel.Add(tf.GetComponent<UILabel>());
        }

        tf = UICardMgr.findChild(Root.transform, "tavern,magicDrawGroup");
        if (tf != null)
        {
            mlistBtn.Add(tf);
            UIEventListener.Get(tf.gameObject).onClick = _OnMagicStoneClick;
        }
        tf = UICardMgr.findChild(Root.transform, "tavern,magicDrawGroup,magicDrawTime");
        if (tf != null)
        {
            tf.GetComponent<UILabel>().text = "";
            mlistLabel.Add(tf.GetComponent<UILabel>());
        }

        SetVisible(false);

        GlobalEventSet.SubscribeEvent(eEventType.FreshRewardMoenyUI, this.updateDetial);
        //GlobalEventSet.SubscribeEvent(eEventType.NodifyReceivRewardCard, this._AddRewardCard);
        GlobalEventSet.SubscribeEvent(eEventType.NodifyUpdateFreeRewardCount, this._ResetFreeCount);

        _FlushTableText();
    }

    void _FlushTableText()
    {
        UICardMgr.setLabelText(Root, "tavern,tavernTit", 14000);
        UICardMgr.setLabelText(Root, "tavern,freeDrawGroup,freeDraw", 14002);
        UICardMgr.setLabelText(Root, "tavern,magicDrawGroup,magicDraw", 14003);
        UICardMgr.setLabelText(Root, "tavern,dimondDrawGroup,freeDraw", 14004);
        UICardMgr.setLabelText(Root, "tavern,tavernHeroList,tavernHeroList,Label", 14005);
    }

    public override PanelID GetPanelID()
    {
        return id;
    }

    protected override void ReleaseImp()
    {

    }

    protected override void onShow()
    {
        base.onShow();

        updateDetial(null);
    }

    public bool updateDetial(SLG.EventArgs obj)
    {
        mlabelDimond.text = getDimonds().ToString();

        mlabelMagicStone.text = getMagicStones().ToString();

        mSpriteProcessBar.fillAmount = 1;

        return false;
    }

    void _OnCloseClick(GameObject go)
    {
        this.SetVisible(false);
    }

    void _OnOpenHeroListClick(GameObject go)
    {
        AudioCenter.me.play(AudioMgr.AudioName.UI_OK);
        _OnCloseClick(go);

        CardIllustratedListPanel pn = PanelManage.me.GetPanel<CardIllustratedListPanel>(PanelID.CardIllustratedListPanel);
        pn.SetVisible(true);
    }

    void _OnDimondClick(GameObject go)
    {
        this.SetVisible(false);

        RewardCardPanel rcp = PanelManage.me.GetPanel<RewardCardPanel>(PanelID.RewardCardPanel);
        rcp.mnRewardStyle = (int)Packet.HERO_ACQUIRE.HERO_ACQUIRE_MONEY;
        rcp.SetVisible(true);
    }

    void _OnMagicStoneClick(GameObject go)
    {
        this.SetVisible(false);

        RewardCardPanel rcp = PanelManage.me.GetPanel<RewardCardPanel>(PanelID.RewardCardPanel);
        rcp.mnRewardStyle = (int)Packet.HERO_ACQUIRE.HERO_ACQUIRE_STONE;
        rcp.SetVisible(true);
    }

    void _OnFreeCardClick(GameObject go)
    {
        if (mnFreeRewardCnt == 0)
        {
            MessageBoxMgr.ShowMessageBox("Error", "You have zero FreeCount", null, null);
            return;
        }

        this.SetVisible(false);
        //MsgCardRecruit.me.questEveryDayCard();
        RewardCardPanel rcp = PanelManage.me.GetPanel<RewardCardPanel>(PanelID.RewardCardPanel);
        rcp.mnRewardStyle = (int)Packet.HERO_ACQUIRE.HERO_ACQUIRE_FREE;
        rcp.SetVisible(true);
    }

    //bool _AddRewardCard(SLG.EventArgs obj)//(uint nId, uint nTypeId)
    //{
    //    Packet.MSG_ACQUIRE_HERO_RESPONSE msg = (Packet.MSG_ACQUIRE_HERO_RESPONSE)obj.m_obj;

    //    CardIllustratedListPanel panel = PanelManage.me.GetPanel<CardIllustratedListPanel>(PanelID.CardIllustratedListPanel);
    //    panel.addCardItem(msg.idHero, msg.idHeroType);

    //    return true;
    //}

    bool _ResetFreeCount(SLG.EventArgs obj)
    {
        this.mnFreeRewardCnt = mnAmount;

        return true;
    }

    public override void SetVisible(bool value)
    {
        base.SetVisible(value);
//         if (value && mbFirstOpen)
//         {
//             mbFirstOpen = false;
//             UpdateRewardCount();
//         }
        UpdateRewardCount();
    }

    void UpdateRewardCount()
    {
        DataMgr.UserData ud = DataMgr.DataManager.getUserData();

        long licon = (long)ud.Data.u8DrawCnt;// DataCenterMgr.UserInfo.u8DrawCnt;

        mnFreeRewardCnt = (int)licon;

        setFreeReward();
    }

    public void setFreeReward()
    {
        string str = DataMgr.DataManager.getLanguageMgr().getString(14001);
        mlistLabel[1].text = string.Format(str, mnFreeRewardCnt);
    }

    public static long getDimonds()
    {
        DataMgr.UserData ud = DataMgr.DataManager.getUserData();

        long licon = (long)ud.Data.rmb; //DataCenterMgr.UserInfo.rmb;

        return licon;
    }

    public static long getMagicStones()
    {
        DataMgr.UserData ud = DataMgr.DataManager.getUserData();
        long licon = (long)ud.Data.stone; //DataCenterMgr.UserInfo.stone;

        return licon;
    }

    public static long getCoins()
    {
        DataMgr.UserData ud = DataMgr.DataManager.getUserData();
        long licon = (long)ud.Data.coin; //DataCenterMgr.UserInfo.coin;

        return licon;
    }

    public static string UserName
    {
        get
        {
            DataMgr.UserData ud = DataMgr.DataManager.getUserData();
            string licon = ud.Data.szName; //DataCenterMgr.UserInfo.coin;

            return licon;
        }

    }

    public static long RewardOnceCostValue { get { return 10; } }
}
