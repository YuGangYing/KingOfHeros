using UnityEngine;
using System;
using System.Collections;
using UI;
using DataMgr;


public class UICardQualityItem : MonoBehaviour
{
    public bool mbSelect = false;
    public UISprite mIconSprite = null;
    public UISprite mIconStar = null;

    public GameObject mgoBg = null;

    public UICardMgr.CItemData mclsItemData =  new UICardMgr.CItemData();

    public PanelID menPanelId = PanelID.Null;

    public string mstrIconBtnName = "";

	// Use this for initialization
	void Start() 
    {
        StartCoroutine("_Init");
	}
	
	// Update is called once per frame
	void Update()
    {
	
	}

    IEnumerator _Init()
    {
        UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemByIndex(this.mclsItemData.nIndex);
        if (cid == null)
            yield return new WaitForSeconds(0.1f);

        SetHangData(this.gameObject, this.mclsItemData.nId);
    }

    void _OnSelectClick(GameObject go)
    {
        if (_IsSameItem())
        {
            Logger.LogDebug("UICardQualityItem::_OnSelectClick is click same item");
            return;
        }

        if (_IsEnable() && mbSelect == false)
            return;

        mbSelect = !mbSelect;
        mgoBg.SetActive(mbSelect);
        
        if (_IsEnable())
        {
            _SetButtonState(true);
        }
        else
        {
            _SetButtonState(false);
        }
    }

    public void reset()
    {
        mgoBg.SetActive(false);
        //mBgSprite = null;
        //mIconSprite = null;
    }

    bool _IsSameItem()
    {
        int nId = CConstance.DEFAULT_ID;
        switch (menPanelId)
        {
            case PanelID.CardQualityUpdatePanel:
                {
                    CardQualityUpdatePanel cup = PanelManage.me.GetPanel<CardQualityUpdatePanel>(menPanelId);
                    nId = cup.CurSelectId;
                }
                break;
            case PanelID.AttendantsPanel:
                {
                    AttendantsPanel ap = PanelManage.me.GetPanel<AttendantsPanel>(menPanelId);
                    nId = ap.CurrentSelectId;
                }
                break;
            default:
                return false;
        }
        if (mclsItemData.nId == nId)
            return true;

        return false;
    }

    int _GetSelectAmount()
    {
        int nAmount = 0;
        switch (menPanelId)
        {
            case PanelID.CardQualityUpdatePanel:
                {
                    CardQualityUpdatePanel cup = PanelManage.me.GetPanel<CardQualityUpdatePanel>(menPanelId);
                    nAmount = cup.getSelectAmount();
                }
                break;
            case PanelID.AttendantsPanel:
                {
                    AttendantsPanel ap = PanelManage.me.GetPanel<AttendantsPanel>(menPanelId);
                    nAmount = ap.getSelectAmount();
                }
                break;
            default:
                break;
        }

        return nAmount;
    }

    bool _IsEnable()
    {
        switch (menPanelId)
        {
            case PanelID.CardQualityUpdatePanel:
                {
                    CardQualityUpdatePanel cup = PanelManage.me.GetPanel<CardQualityUpdatePanel>(menPanelId);
                    if (cup.getSelectAmount() >= cup.mnUpgradeCount)
                        return true;
                }
                break;
            case PanelID.AttendantsPanel:
                {
                    AttendantsPanel ap = PanelManage.me.GetPanel<AttendantsPanel>(menPanelId);
                    if (ap.getSelectAmount() >= ap.mnUpgradeCount)
                    {
                        return true;
                    }
                }
                break;
            default:
                return false;
        }

        return false;
    }

    void _SetButtonState(bool b)
    {
        switch (menPanelId)
        {
            case PanelID.CardQualityUpdatePanel:
                {
                    CardQualityUpdatePanel cup = PanelManage.me.GetPanel<CardQualityUpdatePanel>(menPanelId);
                    UIButton uib = cup.mConfirmBtn.GetComponent<UIButton>();
                    if (uib != null)
                        uib.enabled = b;
                }
                break;
            case PanelID.AttendantsPanel:
                {
                    AttendantsPanel ap = PanelManage.me.GetPanel<AttendantsPanel>(menPanelId);
                    if (ap != null && ap.HangHeroConfirmBtn != null)
                    {
                        ap.HangHeroConfirmBtn.enabled = b;
                    }
                }
                break;
            default:
                break;
        }
    }

    bool SetHangData(GameObject go, int nHeroId)
    {
        if (go == null)
            return false;

        UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemById(nHeroId);
        ConfigRow crdata;
        bool bRet = CHerroTalbeAttribute.getHeroBaseDetail(cid.nTypeId, out crdata);
        if (!bRet)
            return false;

        Transform tf = UICardMgr.findChild(transform, "HeadRoot,icon");
        if (tf != null)
        {
            mIconSprite = tf.GetComponent<UISprite>();
            mIconSprite.spriteName = UICardLogic.getSpriteNameSmall(cid.nTypeId);
            Transform tfTemp = UICardMgr.findChild(tf, "Star");
            if (tfTemp != null)
            {
                mIconStar = tfTemp.GetComponent<UISprite>();
                mIconStar.spriteName = UICardLogic.getStarNameSmall(cid.nTypeId);// mszCardNameStarSmall[nTemp - 1];
            }
        }

        tf = UICardMgr.findChild(transform, "HeadRoot,iconChooseBg");
        if (tf != null)
        {
            mgoBg = tf.gameObject;
            mgoBg.SetActive(false);
        }

        int nAtc = crdata.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.BASE_ATTACK); // cd.getAttributeIntValue(CardData.enAttributeName.enAN_AttackPower);
        int nHP = crdata.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.BASE_HP); //cd.getAttributeIntValue(CardData.enAttributeName.enAN_HP);
        int nDef = crdata.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.BASE_DEF); //cd.getAttributeIntValue(CardData.enAttributeName.enAN_DefensePower);
        int nLead = crdata.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.BASE_LEADER); //cd.getAttributeIntValue(CardData.enAttributeName.enAN_LeadPower);

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bRet = HeroData.getHeroData((uint)nHeroId, ref hi);
        int lLv = hi.usLevel;
        int lStar = hi.u8Star;

        float fFill = lStar * HeroData.FillValue;
        mIconStar.fillAmount = fFill;

        ConfigRow cr;
        bRet = CHerroTalbeAttribute.getHeroStar(cid.nTypeId, lLv, out cr);
        if (!bRet)
        {
            throw new Exception("error");
        }

        // cur
        int nFactor = cr.getIntValue(enCVS_HERO_STAR_ATTRIBUTE.FACTOR_ATTACK);// cths.getAttributeIntValue(CTableHerolStar.enAttributeName.enAN_Attack);
        int nCurAtc = CardQualityUpdatePanel.GetValue((float)nAtc, (int)lStar, (int)lLv, nFactor);

        nFactor = cr.getIntValue(enCVS_HERO_STAR_ATTRIBUTE.FACTOR_HP);//cths.getAttributeIntValue(CTableHerolStar.enAttributeName.enAN_HP);
        int nCurHP = CardQualityUpdatePanel.GetValue((float)nHP, (int)lStar, (int)lLv, nFactor);

        nFactor = cr.getIntValue(enCVS_HERO_STAR_ATTRIBUTE.FACTOR_DEF);//cths.getAttributeIntValue(CTableHerolStar.enAttributeName.enAN_Defence);
        int nCurDef = CardQualityUpdatePanel.GetValue((float)nDef, (int)lStar, (int)lLv, nFactor);

        nFactor = cr.getIntValue(enCVS_HERO_STAR_ATTRIBUTE.FACTOR_LEADER);//cths.getAttributeIntValue(CTableHerolStar.enAttributeName.enAN_Leader);
        int nCurLead = CardQualityUpdatePanel.GetValue((float)nLead, (int)lStar, (int)lLv, nFactor);


        tf = UICardMgr.findChild(go.transform, "atcNo");
        if (tf == null)
            return false;
        UILabel atc = tf.GetComponent<UILabel>();
        atc.text = nCurAtc.ToString();

        // defence
        tf = UICardMgr.findChild(go.transform, "defNo");
        if (tf == null)
            return false;
        atc = tf.GetComponent<UILabel>();
        atc.text = nCurDef.ToString();

        // hp
        tf = UICardMgr.findChild(go.transform, "hpNo");
        if (tf == null)
            return false;
        atc = tf.GetComponent<UILabel>();
        atc.text = nCurHP.ToString();

        // lv
        tf = UICardMgr.findChild(go.transform, "lvNo");
        if (tf == null)
            return false;
        atc = tf.GetComponent<UILabel>();
        atc.text = lLv.ToString();

        // name
        tf = UICardMgr.findChild(go.transform, "labelName");
        if (tf == null)
            return false;
        atc = tf.GetComponent<UILabel>();
        atc.text = crdata.getStringValue(enCVS_HERO_BASE_ATTRIBUTE.NAME_ID); //cd.getAttributeStringValue(CardData.enAttributeName.enAN_Name);

        // power
        tf = UICardMgr.findChild(go.transform, "powerNo");
        if (tf == null)
            return false;
        atc = tf.GetComponent<UILabel>();
        atc.text = nCurLead.ToString();

        UIEventListener.Get(this.gameObject).onClick = _OnSelectClick;

        return true;
    }
        
}
