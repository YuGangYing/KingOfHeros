using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
namespace UI
{

public class CardHeroListPanel : PanelBase
{
    public const PanelID id = PanelID.CardHeroListPanel;

    private GameObject mgoItemPrefab = null;
    private GameObject mgoHangRoot = null;
    private GameObject mgoHeroRoot = null;
    private GameObject mgoItemNullPrefab = null;

    //private string mstrHangRootName = "ItemRoot";
    private string mStrViewCamera = "ViewListCameraArea";
    private string mStrHeroRoot = "HeroRoot";
    
    private Transform mtfCloseBtn = null;

    private Transform mtfTopLeft = null;
    private Transform mtfBottomRight = null;

    // UICamera
    private GameObject mgoUICamera = null;

    private int mCurSelectIndex = CConstance.INVALID_ID;
    private float mfCurOffsetX = 269f;

    // 视图摄像机
    private UnityEngine.Vector3 mvOrigin = new UnityEngine.Vector3(285.3f, 1780.0f, 0.0f);
    private GameObject mgoCameraPrefab = null;
    UIDraggableCamera mDraggableViewCamera = null;
    public UIDraggableCamera dragViewCamera
    {
        get
        {
            if (mDraggableViewCamera == null)
            {
                Transform tfViewHeroRoot = UICardMgr.findChild(Root.transform, "HeroRoot,ViewHeroRoot");
                if (tfViewHeroRoot != null)
                {
                    GameObject goTemp = NGUITools.AddChild(tfViewHeroRoot.gameObject, mgoCameraPrefab);
                    if(goTemp)
                    {
                        goTemp.transform.parent = tfViewHeroRoot;
                        goTemp.transform.localPosition = mvOrigin;
                        mDraggableViewCamera = goTemp.GetComponent<UIDraggableCamera>();
                        if(mDraggableViewCamera != null)
                        {
                            mDraggableViewCamera.rootForBounds = tfViewHeroRoot;
                            mDraggableViewCamera.enabled = true;
                        }

                        UIViewport uivp = goTemp.GetComponent<UIViewport>();
                        if (uivp != null)
                        {
                            if (mgoUICamera == null)
                                mgoUICamera = UICardMgr.getCamera();
                            uivp.sourceCamera = mgoUICamera.GetComponent<Camera>();
                            uivp.topLeft = mtfTopLeft;
                            uivp.bottomRight = mtfBottomRight;
                        }
                    }
                }
            }
            return mDraggableViewCamera;
        }
        private set { }
    }

    // Use this for initialization
    public CardHeroListPanel()
    {
    }

    public override string GetResPath()
    {
        return "HeroListWin.prefab";
    }

    protected override void Initimp(List<GameObject> prefabs)
    {
        //Root.transform.localScale = new UnityEngine.Vector3(4.0f, 4.0f, 4.0f);

        UIAnchor uia = Root.GetComponent<UIAnchor>();
        if (uia != null)
        {
            uia.uiCamera = UICardMgr.getCamera().GetComponent<Camera>();
        }

        mgoItemPrefab = prefabs[1];    //NGUITools.AddChild(m_itemRoot.gameObject, itemPrefab);
        mgoCameraPrefab = prefabs[2];  // 
        mgoItemNullPrefab = prefabs[3];

        mtfCloseBtn = UICardMgr.findChild(Root.transform, "BackgroundRoot,bgRoot,btnRoot,close");
        if (mtfCloseBtn != null)
        {
            UIEventListener.Get(mtfCloseBtn.gameObject).onClick = _OnCloseClick;
        }

        Transform tfHeroRoot = Root.transform.FindChild(mStrHeroRoot);
        if (mgoHeroRoot == null)
        {
            mgoHeroRoot = tfHeroRoot.gameObject;

            mtfTopLeft = UICardMgr.findChild(Root.transform, "BackgroundRoot,ViewPoint,TopLeft");
            mtfBottomRight = UICardMgr.findChild(Root.transform, "BackgroundRoot,ViewPoint,BottomRight");
        }

		mgoHeroRoot.AddComponent<UIHeroRoot>();

        _ResetAnchorCamera();

        UICardMgr.singleton.initial();

        // 注册回调
        //GameInstance.Game.Sgt.mfunNodifyAddCard = revAddCard;

        Transform tf = UICardMgr.findChild(Root.transform, "HeroRoot,ViewHeroRoot,ItemRoot");//Root.transform.FindChild(mstrHangRootName);
        if (null != tf)
        {
            mgoHangRoot = tf.gameObject;
        }
        else
            Logger.LogDebug("CardHeroListPanel::Initimp tf is null");

        SetVisible(false);

        //GlobalEventSet.SubscribeEvent(eEventType.AddIllustratedUI, id, this._OnAddItem);

        GlobalEventSet.SubscribeEvent(eEventType.NodifyReceivRewardIllustrated, this._OnAddRewardItem);
        //testItem();
    }

    public override PanelID GetPanelID()
    {
        return id;
    }

    protected override void ReleaseImp()
    {
//         mgoHeroRoot = null;
//         mgoHangRoot = null;
//         mgoItemPrefab = null;
// 
//         dragViewCamera = null;
//         mtfTopLeft = null;
//         mtfBottomRight = null;
    }

    public override void SetVisible(bool value)
    {
        if (mDraggableViewCamera)
        {
            if (mCurSelectIndex == CConstance.INVALID_ID)
                mDraggableViewCamera.GetComponent<Camera>().transform.localPosition = mvOrigin;
            else
            {
                UnityEngine.Vector3 v = mvOrigin;
                v.x = mCurSelectIndex * mfCurOffsetX;
                mDraggableViewCamera.GetComponent<Camera>().transform.localPosition = v;
            }
        }

        if (!value)
        {
            mCurSelectIndex = CConstance.INVALID_ID;
            UIHeroRoot uihr = mgoHeroRoot.GetComponent<UIHeroRoot>();
            if (uihr != null)
            {
                uihr.setCamera(ref mDraggableViewCamera);
            }
        }

        base.SetVisible(value);
    }

    protected override void onShow()
    {
        base.onShow();

        _ResetDataItem();
    }

    public void revAddCard(uint nId, uint nTypeId)
    {
//         bool bRet = HeroItemMgr.singleton.isHaveCard((int)nId);
//         if (bRet)
//         {
//             Logger.LogDebug("CardHeroListPanel::revAddCard  failed typeid:" + nTypeId.ToString() + "  id:" + nId.ToString());
//             return;
//         }

        CardItem ci = new CardItem();
        ci.mnId = (int)nId;
        ci.mnIndex = UICardMgr.singleton.itemAmount; //HeroItemMgr.singleton.amount;
        ci.mBaseData.typeId = (int)nTypeId;

        Logger.LogDebug("CardHeroListPanel::revAddCard  info typeid:" + nTypeId.ToString() + "  index:" + ci.mnIndex.ToString() + "  id:" + ci.mnId.ToString());

        bool bRet = false;
        if(nId >= UICardMgr.mNullId)
            bRet = UICardMgr.singleton.addItem(ci, mgoHangRoot, mgoItemNullPrefab);
        else
            bRet = UICardMgr.singleton.addItem(ci, mgoHangRoot, mgoItemPrefab);

        if (bRet)
        {
            Logger.LogDebug("CardHeroListPanel::revAddCard  success");

            UICardMgr.CItemData cid = UICardMgr.singleton.getItemByIndex(ci.mnIndex);
            UIEventListener.Get(cid.goIcon).onClick = _OnItemClick;

            int nOffset = 280;
            int xIndex = ci.mnIndex;
            cid.goIcon.transform.localPosition = new UnityEngine.Vector3(xIndex * nOffset, 0.0f, 0.0f);

            //SLG.GlobalEventSet.FireEvent(SLG.eEventType.AddIllustratedItem, PanelID.CardIllustratedListPanel, new SLG.EventArgs((object)cid));
        }
    }

    void _ResetAnchorCamera()
    {
        Transform bgRoot = UICardMgr.findChild(Root.transform, "BackgroundRoot");
        if (bgRoot != null)
        {
            UIAnchor uia = bgRoot.GetComponent<UIAnchor>();
            if (uia != null)
            {
                if (mgoUICamera == null)
                    mgoUICamera = UICardMgr.getCamera();
                uia.uiCamera = mgoUICamera.GetComponent<Camera>();
            }

            Transform tfVhRoot = UICardMgr.findChild(Root.transform, "HeroRoot,ViewHeroRoot");
            if (null != tfVhRoot)
            {
                uia = tfVhRoot.GetComponent<UIAnchor>();
                if (uia != null)
                {
                    if (mgoUICamera == null)
                        mgoUICamera = UICardMgr.getCamera();
                    uia.uiCamera = mgoUICamera.GetComponent<Camera>();
                }
            }

        }
    }

    void _OnItemClick(GameObject go)
    {
        //this.SetVisible(false);
        UICardLogic uic = go.GetComponent<UICardLogic>();
        if (uic.mnId >= UICardMgr.mNullId)
            return;

        CardHeroDetailPanel chdp = (CardHeroDetailPanel)PanelManage.me.getPanel(PanelID.CardHeroDetailPanel);
        chdp.SetVisible(true);
        chdp.setData(uic);

        mCurSelectIndex = uic.mnIndex;
    }

    void _ResetDataItem()
    {
        UICardMgr.singleton.cleanHeroItem();
        // add null item on begin position
        revAddCard(UICardMgr.mNullId, UICardMgr.mDefauldTypeId);
        for (int i = 0; i < DataMgr.UserData.HeroPhotoAmount; i++)
        {
            uint dwTypeId = DataMgr.UserData.getHeroPhotoTypeId(i);
            //uint typeId = DataCenterMgr.getHeroPhotoItmeIdByIndex(i);
            revAddCard(dwTypeId, (uint)dwTypeId);
        }

        // add null item on end position
        uint nTemp = UICardMgr.mNullId + 1;
        revAddCard(nTemp, UICardMgr.mDefauldTypeId);
    }

    void _OnCloseClick(GameObject go)
    {
        Debug.Log("_OnCloseClick");
//        this.SetVisible(false);
//        UICardLogic.resetCurSelect(UICardLogic.enItemSource.enIS_HeroList);
    }

    bool _OnAddRewardItem(SLG.EventArgs obj)
    {
        Packet.MSG_ACQUIRE_HERO_RESPONSE res = (Packet.MSG_ACQUIRE_HERO_RESPONSE)obj.m_obj;

        DataMgr.UserData ud = DataManager.getUserData();
        if (ud == null)
            return false;

        bool bRet = DataMgr.UserData.getHeroPhotoData(res.idHeroType);
        //bool bRet = DataCenterMgr.isHaveHeroPhotoItem(res.idHeroType);
        if (!bRet)
        {
            uint nTempId = UICardMgr.mNullId + 1;
            UICardMgr.singleton.RemoveItemById((int)nTempId);

            revAddCard(res.idHeroType, res.idHeroType);

            revAddCard(nTempId, UICardMgr.mDefauldTypeId);
        }

        return true;
    }

    void testItem()
    {
        int nx = 160, ny = -220;
        //if (en == UICardLogic.enItemSource.enIS_HeroList)
        {
            nx = 290;
        }

        for (int i = 0; i < 11; i++)
        {
            CardItem ci = new CardItem();
            ci.mnId = i + 1;
            ci.mnIndex = i;
            ci.mBaseData.typeId = UnityEngine.Random.Range(0, 10 /*CsvConfigMgr.me.HeroAmount*/) + 1;

            revAddCard((uint)ci.mnId, (uint)ci.mBaseData.typeId);

//             {
//                 int xIndex = 0;
//                 int yIndex = 0;
// 
//                 xIndex = i;
// 
//                 GameObject go = UICardMgr.singleton.getItemByIndex(i).goIcon;
//                 go.transform.localPosition = new Vector3(xIndex * nx, yIndex * ny, 0.0f);
// 
//             }
        }
    }

}

}

