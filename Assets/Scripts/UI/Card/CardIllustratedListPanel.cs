using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using SLG;
using Packet;
using DataMgr;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
namespace UI
{

public class CardIllustratedListPanel : PanelBase
{
    public const PanelID id = PanelID.CardIllustratedListPanel;
    private GameObject mgoItemPrefab = null;
    private GameObject mgoHangRoot = null;

    private List<GameObject> mgoListBtn = new List<GameObject>();
    private List<UILabel> mlistLabel = new List<UILabel>();

    int mnCurSelId = CConstance.INVALID_ID;

    // Use this for initialization
    public CardIllustratedListPanel()
    {
    }

    public override string GetResPath()
    {
        return "IllustratedListWin.prefab";
    }

    protected override void Initimp(List<GameObject> prefabs)
    {
        //Root.transform.localScale = new UnityEngine.Vector3(4.0f, 4.0f, 4.0f);

        UIAnchor uia = Root.GetComponent<UIAnchor>();
        if (uia != null)
        {
            uia.uiCamera = UICardMgr.getCamera().GetComponent<Camera>();
        }

        UICardMgr.singleton.initial();

        mgoItemPrefab = prefabs[1];

        if (mgoHangRoot == null)
        {
            Transform tfTemp = UICardMgr.findChild(Root.transform, "ItemRootPanel,ItemRootArea");
            mgoHangRoot = tfTemp.gameObject;
        }

        Transform tfChild = UICardMgr.findChild(Root.transform, "BackgroundRoot,btnRoot,close");
        if (tfChild != null)
        {
            mgoListBtn.Add(tfChild.gameObject);
            UIEventListener.Get(tfChild.gameObject).onClick = _OnCloseClick;
        }

        tfChild = UICardMgr.findChild(Root.transform, "BackgroundRoot,btnRoot,dimond");
        if (tfChild != null)
        {
            mgoListBtn.Add(tfChild.gameObject);
        }

        tfChild = UICardMgr.findChild(Root.transform, "BackgroundRoot,btnRoot,stone");
        if (tfChild != null)
        {
            mgoListBtn.Add(tfChild.gameObject);
        }

        tfChild = UICardMgr.findChild(Root.transform, "BackgroundRoot,btnRoot,dimond,dimNo");
        if (tfChild != null)
        {
            UILabel uil = tfChild.GetComponent<UILabel>();
            mlistLabel.Add(uil);
        }

        tfChild = UICardMgr.findChild(Root.transform, "BackgroundRoot,btnRoot,stone,stoneNo");
        if (tfChild != null)
        {
            UILabel uil = tfChild.GetComponent<UILabel>();
            mlistLabel.Add(uil);
        }

        SetVisible(false);

        //testItem();
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

        mlistLabel[0].text = RewardPanel.getDimonds().ToString();

        mlistLabel[1].text = RewardPanel.getMagicStones().ToString();

        _ResetDataItem();
    }

    protected override void onHide()
    {
        base.onHide();

        PanelBase pdDetail = PanelManage.me.getPanel(PanelID.CardIllustratedDetailPanel);
        PanelBase pdQuilty = PanelManage.me.getPanel(PanelID.CardQualityUpdatePanel);
        PanelBase pdLevel = PanelManage.me.getPanel(PanelID.CardStarUpdatePanel);

    }

    void revAddCard(uint nId, uint nTypeId)
    {
        CardItem ci = new CardItem();
        ci.mnId = (int)nId;
		ci.mnIndex = UICardMgr.singleton.illustratedItemAmount;
        ci.mBaseData.typeId = (int)nTypeId;

        Logger.LogDebug("CardIllustratedListPanel::revAddCard  info typeid:" + nTypeId.ToString() + "  index:" + ci.mnIndex.ToString() + "  id:" + ci.mnId.ToString());

        bool bRet = UICardMgr.singleton.addItem(ci, mgoHangRoot, mgoItemPrefab, UICardLogic.enItemSource.enIS_HeroIllustrated);

        if (bRet)
        {
            Logger.LogDebug("CardIllustratedListPanel::revAddCard  success");

            UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemByIndex(ci.mnIndex);
            UIEventListener.Get(cid.goIcon).onClick = _OnItemClick;

            int max = 4;
            //int nx = 185, ny = -260; // 800x480
            int nx = 230, ny = -260;   // 960x640
            int xIndex = ci.mnIndex % max;
            int yIndex = ci.mnIndex / max;

            cid.goIcon.transform.localPosition = new UnityEngine.Vector3(xIndex * nx, yIndex * ny, 0.0f);

            updateStar(cid);

            UITable uit = mgoHangRoot.GetComponent<UITable>();
            uit.repositionNow = true;
        }
    }

    void _OnItemClick(GameObject go)
    {
        //this.SetVisible(false);

        CardIllustratedDetailPanel chdp = (CardIllustratedDetailPanel)PanelManage.me.getPanel(PanelID.CardIllustratedDetailPanel);
        chdp.SetVisible(true);

        //CardHeroDetailPanel.CData cdt = new CardHeroDetailPanel.CData();
        UICardLogic uic = go.GetComponent<UICardLogic>();
        chdp.updateDetial(uic);

        mnCurSelId = uic.mnId;
    }

    bool _testAddItem(SLG.EventArgs obj)
    {
        UICardMgr.CItemData cid = (UICardMgr.CItemData)obj.m_obj;
        if (cid == null)
        {
            Logger.LogDebug("_testAddItem is null");
            return false;
        }

        revAddCard((uint)cid.nId, (uint)cid.nTypeId);

        return true;
    }

    void _OnCloseClick(GameObject go)
    {
        this.SetVisible(false);

        mnCurSelId = CConstance.INVALID_ID;
        //UICardLogic.resetCurSelect(UICardLogic.enItemSource.enIS_HeroIllustrated);
    }

    void updateStar(UICardMgr.CItemData cid)
    {
        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)cid.nId, ref hi);
        if (!bRet)
            return ;

        float fbase = HeroData.FillValue * hi.u8Star;

        Transform tf = UICardMgr.findChild(cid.goIcon.transform, "Sprite,Star");
        if(tf != null)
        {
            UISprite uis = tf.GetComponent<UISprite>();
            uis.fillAmount = fbase;
        }

        UILabel uil = UICardMgr.FindChild<UILabel>(cid.goIcon, "Sprite,Info,ATK");
        if (uil != null)
        {
            uil.text = CardIllustratedDetailPanel.getAtk((uint)cid.nTypeId, cid.nId);
        }
        uil = UICardMgr.FindChild<UILabel>(cid.goIcon, "Sprite,Info,DEF");
        if (uil != null)
        {
            uil.text = CardIllustratedDetailPanel.getDef((uint)cid.nTypeId, cid.nId);
        }
        uil = UICardMgr.FindChild<UILabel>(cid.goIcon, "Sprite,Info,HP");
        if (uil != null)
        {
            uil.text = CardIllustratedDetailPanel.getHP((uint)cid.nTypeId, cid.nId);
        }
        uil = UICardMgr.FindChild<UILabel>(cid.goIcon, "Sprite,Info,FG");
        if (uil != null)
        {
            uil.text = CardIllustratedDetailPanel.getFight(hi.idType, (int)hi.idHero);
        }
        uil = UICardMgr.FindChild<UILabel>(cid.goIcon, "Sprite,Info,LV");
        if (uil != null)
        {
            uil.text = hi.usLevel.ToString();
        }
    }

    void _ResetDataItem()
    {
        UICardMgr.singleton.cleanIlIllustratedItem();

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        for (int i = 0; i < DataManager.getHeroData().Amount; i++)
        {
            bool bRet = HeroData.getHeroData(i, ref hi);
            if (!bRet)
                continue;
            
            revAddCard(hi.idHero, hi.idType);
        }

        UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemById(mnCurSelId);
        if (cid != null)
        {
            UICardLogic cl = cid.goIcon.GetComponent<UICardLogic>();
            if (cl != null)
            {
                cl.DoSelect = true;
            }
        }
    }

    public void resetDataItem(int nId)
    {
        this.mnCurSelId = nId;

        _ResetDataItem();

        CardIllustratedDetailPanel chdp = (CardIllustratedDetailPanel)PanelManage.me.getPanel(PanelID.CardIllustratedDetailPanel);
        UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemById(mnCurSelId);
        if (cid != null)
        {
            UICardLogic uic = cid.goIcon.GetComponent<UICardLogic>();
            chdp.updateDetial(uic);
        }
    }


    //bool _OnAddCardItem(SLG.EventArgs obj)
    //{
    //    Packet.HERO_INFO info = (Packet.HERO_INFO)obj.m_obj;

    //    bool bRet = HeroItemMgr.singleton.isHaveCard((int)info.idHero);
    //    if (bRet)
    //        return false;

    //    CardItem ci = new CardItem();
    //    ci.mnId = (int)info.idHero;
    //    ci.mBaseData.typeId = (int)info.idType;
    //    ci.mnLevel = (int)info.usLevel;
    //    ci.mnExp = info.unExp;
    //    ci.mnStar = (int)info.u8Star;
    //    ci.mnIndex = HeroItemMgr.singleton.amount;
    //    ci.mnStatus = (int)info.u8Status;
    //    ci.mnWeapon = (int)info.u8Weapon;
        
    //    ci.mBaseData.skillTable.Add((int)info.unSkill1);
    //    ci.mBaseData.skillTable.Add((int)info.unSkill2);
    //    ci.mBaseData.skillTable.Add((int)info.unSkill3);

    //    ci.mlistSkillLv.Add((int)info.usSkillLvl1);
    //    ci.mlistSkillLv.Add((int)info.usSkillLvl2);
    //    ci.mlistSkillLv.Add((int)info.usSkillLvl3);

    //    ci.mlistSkillExp.Add(info.unSkillExp1);
    //    ci.mlistSkillExp.Add(info.usSkillLvl2);
    //    ci.mlistSkillExp.Add(info.usSkillLvl3);

    //    bRet = HeroItemMgr.singleton.addCardItem(ci);
    //    if (!bRet)
    //        return false;

    //    revAddCard(info.idHero, info.idType);

    //    return true;
    //}


    void testItem()
    {
        int max = 5;
//         if (en == UICardLogic.enItemSource.enIS_HeroList)
//         {
//             nx = 240;
//         }

        for (int i = 0; i < 11; i++)
        {
            CardItem ci = new CardItem();
            ci.mnId = i + 1;
            ci.mnIndex = i;
            ci.mBaseData.typeId = UnityEngine.Random.Range(0, 10/*CsvConfigMgr.me.HeroAmount*/) + 1;

            revAddCard((uint)ci.mnId, (uint)ci.mBaseData.typeId);
            //if (bRet)
//             {
//                 int xIndex = 0;
//                 int yIndex = 0;
// 
// //                if (en == UICardLogic.enItemSource.enIS_HeroIllustrated)
//                 {
//                     xIndex = i % max;
//                     yIndex = i / max;
//                 }
// //                 else
//                 {
//                     xIndex = i;
//                 }
// 
//                 GameObject go = UICardMgr.singleton.getIllustratedItemByIndex(i).goIcon;// mlistItem[i].goIcon;
//                 go.transform.localPosition = new Vector3(xIndex * nx, yIndex * ny, 0.0f);
//             }
        }
    }

}


}

