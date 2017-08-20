using UnityEngine;
using System;
using System.Collections;
using UI;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
public class UICardLogic : MonoBehaviour
{
	public enum enItemSource
	{
		enIS_Invalid = -1,
		enIS_HeroList = 0,
		enIS_HeroIllustrated,
		enIS_Amount,
	}

	public enItemSource menType = enItemSource.enIS_Invalid;
	public int mnId;
	public int mnIndex;
    public int mnTypeId;

    private string mStrSelectName = "SelectSprite";
    private UISprite mclsSelectSprite = null;
    private Transform mtfSelectBg = null;
    public bool mbSelect = false;
    private bool mbDoSelect = false;
    public bool DoSelect { set { mbDoSelect = value; } }

	private string[] mszStrAction = {"ShowDetail", "ShowLevel", "ShowQuality"};
	private string[] mszUpateBtnName = { "btnRoot", "levelbtn", "qualitybtn"};

	//private string[] mszQualityName = { "copper card", "gold card", "iron card", "silver card"};

    public static string[] mszQualityName = { "Sparta", "RobinHood", "KingArthur", "CleopatraVII", "Richard", };
    public static string[] mszQualityNameStar = { "copperCardStar", "goldCardStar", "ironCardStar", "silverCardStar", };
    public static string[] mszCardNameSmall = { "Sparta", "RobinHood", "KingArthur", "CleopatraVII", "Richard", };
    public static string[] mszCardNameStarSmall = { "copperCardStar_s", "goldCardStar_s", "ironCardStar_s", "silverCardStar_s", };

	// Use this for initialization
	void Start()
	{
		StartCoroutine(AddEvtListen(0.05f));
		//AddEvtListen();
	}

	IEnumerator AddEvtListen(float fWaitTime)
	{
//		Logger.LogDebug("AddEvtListen  0000");

        UIEventListener uel = UIEventListener.Get(this.gameObject);
		if(uel == null)
			yield return new WaitForSeconds(1);

//		Logger.LogDebug("AddEvtListen  1111");

		uel.onClick += OnItemDetailClick;

//		Logger.LogDebug("AddEvtListen  2222");
		//uid.draggableCamera = UICardMgr.singleton.dragViewCamera;

        switch (menType)
        {
            case enItemSource.enIS_Invalid:
		        {
			        Logger.LogDebug("UICardLogic::AddEvtListen error");
			        yield return new WaitForSeconds(1);
		        }
                break;
            case enItemSource.enIS_HeroList:
                {
                    UIDragCamera UIDC = this.gameObject.AddComponent<UIDragCamera>();
                    if (UIDC == null)
                        yield return new WaitForSeconds(1);

                    CardHeroListPanel chlp = (CardHeroListPanel)PanelManage.me.getPanel(PanelID.CardHeroListPanel);
                    if(chlp == null)
                        yield return new WaitForSeconds(1);

                    if(chlp.dragViewCamera == null)
                        yield return new WaitForSeconds(1);

                    UIDC.draggableCamera = chlp.dragViewCamera;
                    //UIDC.draggableCamera = UICardMgr.singleton.dragViewCamera;
                }
                break;
            case enItemSource.enIS_HeroIllustrated:
                {
                    UIDragScrollView UIDPC = this.gameObject.AddComponent<UIDragScrollView>();
                     if (UIDPC == null)
                         yield return new WaitForSeconds(1);
                }
                break;
            default:
                break;
        }

		Transform tfRoot = this.transform.FindChild(mszUpateBtnName[0]);
		if(tfRoot != null)
		{
			Transform tfLvl = tfRoot.FindChild(mszUpateBtnName[1]);
			Transform tfQua = tfRoot.FindChild(mszUpateBtnName[2]);
			if(tfLvl != null)
			{
				uel = tfLvl.gameObject.GetComponent<UIEventListener>();
				uel.onClick += ClickLevel;
			}

			if(tfQua != null)
			{
				uel = tfQua.gameObject.GetComponent<UIEventListener>();
				uel.onClick += ClickQuality;
			}
		}

        UISprite uis = null;
        Transform tf = null;
		Transform tfIcon = this.transform.FindChild("Sprite");
		if(tfIcon != null)
		{
			uis = tfIcon.GetComponent<UISprite>();
			if(uis != null)
			{
                if(menType == enItemSource.enIS_HeroIllustrated)
                    uis.spriteName = getSpriteNameSmall(mnTypeId);            //mszCardNameSmall[nIndex];
                else
                    uis.spriteName = getSpriteNameBig(mnTypeId);              //mszQualityName[nIndex];
			}

            tf = UICardMgr.findChild(this.transform, "Sprite,Star");
            if (tf != null)
            {
                bool bHave = false;
                int lStar = 5;
                if (menType == enItemSource.enIS_HeroIllustrated)
                {
                    tf.GetComponent<UISprite>().spriteName = getStarNameSmall(mnTypeId);//mszCardNameStarSmall[nIndex];

                    Packet.HERO_INFO hi = new Packet.HERO_INFO();
                    bool bRet = HeroData.getHeroData((uint)this.mnId, ref hi);
                    if (bRet)
                        lStar = hi.u8Star;
                }
                else if (menType == enItemSource.enIS_HeroList)
                {
                    tf.GetComponent<UISprite>().spriteName = getStarNameBig(mnTypeId);

                    bHave = DataMgr.UserData.getHeroPhotoData((uint)this.mnId);
                    //bHave = DataCenterMgr.isHaveHeroPhotoItem((uint)this.mnId);
                }
                
                if(bHave)
                {
                    float fFill = lStar * HeroData.FillValue;
                    tf.GetComponent<UISprite>().fillAmount = fFill;
                }
            }

            tf = UICardMgr.findChild(this.transform, "Sprite,Select");
            if (tf != null)
            {
                mtfSelectBg = tf;
            }

            mclsSelectSprite = uis;
            resetBgColor();
		}
	}

	// Update is called once per frame
	void Update()
	{
        _OnSelect();
	}

	void Destroy()
	{
		UIEventListener uel = this.gameObject.GetComponent<UIEventListener>();
		if(uel != null)
			uel.onClick -= OnItemDetailClick;

		Transform tfRoot = this.transform.FindChild(mszUpateBtnName[0]);
		if(tfRoot != null)
		{
			Transform tfLvl = tfRoot.FindChild(mszUpateBtnName[1]);
			Transform tfQua = tfRoot.FindChild(mszUpateBtnName[2]);
			if(tfLvl != null)
			{
				uel = tfLvl.gameObject.GetComponent<UIEventListener>();
				uel.onClick -= ClickLevel;
			}
			
			if(tfQua != null)
			{
				uel = tfQua.gameObject.GetComponent<UIEventListener>();
				uel.onClick -= ClickQuality;
			}
		}
	}

	void OnItemDetailClick(GameObject go)
	{
		Logger.LogDebug("OnItemDetailClick");
		// 到时候修正

        resetCurSelect(menType);

        setSelectColor();

        string strParameters = mnId.ToString() + "," + mnIndex.ToString();

        switch (menType)
        {
            case enItemSource.enIS_Invalid:
                break;
            case enItemSource.enIS_HeroList:
                {
                    UI.CardHeroDetailPanel chp = UI.PanelManage.me.GetPanel<UI.CardHeroDetailPanel>(UI.PanelID.CardHeroDetailPanel);
                    chp.mclsPM.mbUpdate = true;
                }
                break;
            case enItemSource.enIS_HeroIllustrated:
                {
                    //UICardMgr.singleton.gameObject.SendMessage(mszStrAction[0], this/*.gameObject*/);
                }
                break;
            default:
                break;
        }
	}

    void _OnSelect()
    {
        if (mbDoSelect == false)
            return;
        DoSelect = false;

        setSelectColor();

    }

    void setSelectColor()
    {
        mbSelect = true;
        //mclsSelectSprite.color = new Color(1.0f, 0.0f, 0.0f);
        if (mtfSelectBg != null)
            mtfSelectBg.gameObject.SetActive(true);
    }

    public void resetBgColor()
    {
        mbSelect = false;
        //mclsSelectSprite.color = new Color(1.0f, 1.0f, 1.0f);
        if (mtfSelectBg != null)
            mtfSelectBg.gameObject.SetActive(false);
    }

    public static void resetCurSelect(enItemSource en)
    {
        switch (en)
        {
            case enItemSource.enIS_Invalid:
                break;
            case enItemSource.enIS_HeroList:
                {
                    for (int i = 0; i < UICardMgr.singleton.itemAmount; i++)
                    {
                        UICardMgr.CItemData cd = UICardMgr.singleton.getItemByIndex(i);
                        UICardLogic uicl = cd.goIcon.GetComponent<UICardLogic>();
                        uicl.mbSelect = false;
                        uicl.resetBgColor();
                    }
                }
                break;
            case enItemSource.enIS_HeroIllustrated:
                {
                    for (int i = 0; i < UICardMgr.singleton.illustratedItemAmount; i++)
                    {
                        UICardMgr.CItemData cd = UICardMgr.singleton.getIllustratedItemByIndex(i);
                        UICardLogic uicl = cd.goIcon.GetComponent<UICardLogic>();
                        uicl.mbSelect = false;
                        uicl.resetBgColor();
                    }
                }
                break;
            default:
                break;
        }
    }

	void ClickLevel(GameObject go)
	{
		//UICardMgr.singleton.gameObject.SendMessage(mszStrAction[1]);
	}

	void ClickQuality(GameObject go)
	{
		//UICardMgr.singleton.gameObject.SendMessage(mszStrAction[2]);
	}

    public static int getNameIndexBySmallName(string str)
    {
        int i = 0;
        for ( i = 0; i < mszCardNameSmall.Length; i++)
        {
            if(str == mszCardNameSmall[i])
                break;;
        }
        return i;
    }

    public static int getNameIndexByQualityName(string str)
    {
        int i = 0;
        for (i = 0; i < mszQualityName.Length; i++)
        {
            if (str == mszQualityName[i])
                break; ;
        }
        return i;
    }

    public void upadteStar()
    {
         Transform tf = UICardMgr.findChild(this.transform, "Sprite,Star");
        if (tf != null)
        {
            Packet.HERO_INFO hi = new Packet.HERO_INFO();
            bool bRet = HeroData.getHeroData((uint)this.mnId, ref hi);
            if (bRet)
            {
                UISprite uis = tf.GetComponent<UISprite>();
                uis.fillAmount = hi.u8Star * HeroData.FillValue;
            }
        }
    }

    static string _GetSpriteName(int nHeroTypeId)
    {
        string str = "";

        ConfigRow cr = null;
        CHerroTalbeAttribute.getHeroBaseDetail(nHeroTypeId, out cr);

        if (cr == null)
            return CConstance.ERROR_STR;

        str = cr.getStringValue(enCVS_HERO_BASE_ATTRIBUTE.ICON_SPRITE_NAME);

        return str;
    }

    public static string getSpriteNameSmall(int nHeroTypeId)
    {
        string str = _GetSpriteName(nHeroTypeId);

        if (str != CConstance.ERROR_STR)
            str = str + "_s";

        return str;
    }

    public static string getSpriteNameBig(int nHeroTypeId)
    {
        string str = _GetSpriteName(nHeroTypeId);

        return str;
    }

    static int _GetStarName(int nHeroTypeId)
    {
        ConfigRow cr = null;
        CHerroTalbeAttribute.getHeroBaseDetail(nHeroTypeId, out cr);

        if (cr == null)
            return 0;

        int nTemp = cr.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.QUALITY);
        int nIndex = nTemp - 1;

        return nIndex;
    }

    public static string getStarNameSmall(int nHeroTypeId)
    {
        int nQuality = _GetStarName(nHeroTypeId);
        if (nQuality < 0 || nQuality >= 4)
            nQuality = 0;

        return mszCardNameStarSmall[1/*nQuality*/];
    }

    public static string getStarNameBig(int nHeroTypeId)
    {
        int nQuality = _GetStarName(nHeroTypeId);
        if(nQuality < 0 || nQuality >= 4)
            nQuality = 0;

        return mszQualityNameStar[1/*nQuality*/];
    }
}
