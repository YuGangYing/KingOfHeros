using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
namespace UI
{

public class CardHeroDetailPanel : PanelBase
{
    public const PanelID id = PanelID.CardHeroDetailPanel;
//     private string[] mstrTreeRoot = { "DetailRoot", "introduction", "modelArea", "suitBtn",
//                                         "bgRect", "itemModelRoot" };

//     private GameObject mgoIntroductionDataRoot = null;
//     private GameObject mgoIconBtn = null;
//     private GameObject mgoNormalBtn = null;

    //private int mIconBtnAmount = 3;

    private UILabel mLabelContext = null;
    private UILabel mLabelTitle = null;
    private GameObject[] mgoSuitBtn = null;
    private GameObject mgoHangModelRoot = null;
    private GameObject mgoModel = null;
    private Transform mtfRealHangModelPoint = null;

    public PanelMouse mclsPM = null;

    private Transform mtfCloseBtn = null;

    public class CData
    {
        public int id = CConstance.INVALID_ID;
        public int index = CConstance.INVALID_ID;
        public string strContext = string.Empty;
        public string strTitle = string.Empty;
    }
    private UICardLogic mclsData = new UICardLogic();


    public CardHeroDetailPanel()
    {
    }

    public override string GetResPath()
    {
        return "HeroDetailWin.prefab";
    }

    protected override void Initimp(List<GameObject> prefabs)
    {
        //Root.transform.localScale = new UnityEngine.Vector3(4.0f, 4.0f, 4.0f);

        UIAnchor uia = Root.GetComponent<UIAnchor>();
        if (uia != null)
        {
            uia.uiCamera = UICardMgr.getCamera().GetComponent<Camera>();
        }

        Transform tfDR = UICardMgr.findChild(Root.transform, "DetailRoot,introduction,dataRoot,context,LabelContext");
        if (tfDR != null)
        {
            mLabelContext = tfDR.GetComponent<UILabel>();
        }

        mtfCloseBtn = UICardMgr.findChild(Root.transform, "BackgroundRoot,btnRoot,close");
        if (mtfCloseBtn != null)
        {
            UIEventListener.Get(mtfCloseBtn.gameObject).onClick = _OnCloseClick;
        }

        tfDR = UICardMgr.findChild(Root.transform, "DetailRoot,introduction,dataRoot,LabelTitle");
        if(tfDR != null)
        {
            mLabelTitle = tfDR.GetComponent<UILabel>();
        }

        tfDR = UICardMgr.findChild(Root.transform, "DetailRoot,modelArea,itemModelRoot,hangPoint");
        if(tfDR != null)
        {
            mgoHangModelRoot = tfDR.gameObject;
        }

        tfDR = UICardMgr.findChild(mgoHangModelRoot.transform, "SBD");
        if (tfDR != null)
        {
            mtfRealHangModelPoint = tfDR;
        }
       
        tfDR = UICardMgr.findChild(mgoHangModelRoot.transform, "PMouse");
        if (tfDR != null)
        {
            mclsPM = tfDR.GetComponent<PanelMouse>();
            if (mclsPM)
                mclsPM.initCallBack = initList;
        }

        tfDR = UICardMgr.findChild(Root.transform, "DetailRoot,modelArea,bgRect");
        if (tfDR != null)
        {
            UIBgNullMsgLogic ubnml = tfDR.GetComponent<UIBgNullMsgLogic>();
            ubnml.mfunClick = _OnClickNull;
        }

        tfDR = UICardMgr.findChild(Root.transform, "DetailRoot,suitBtn");
        if(tfDR != null)
        {
            int nCnt = tfDR.childCount;
            int nIndex = 0;
            mgoSuitBtn = new GameObject[nCnt];

            foreach (Transform tfBtn in tfDR)
            {
                UIEventListener.Get(tfBtn.gameObject).onClick = _OnIconButtonClick;
                mgoSuitBtn[nIndex] = tfBtn.gameObject;
                tfBtn.gameObject.SetActive(false);
                nIndex++;
            }
        }

        SetVisible(false);
    }

    public override void SetVisible(bool value)
    {
        base.SetVisible(value);

        if (value == false)
        {
            mtfRealHangModelPoint.localRotation = new UnityEngine.Quaternion(mtfRealHangModelPoint.localRotation.x,
                180f, mtfRealHangModelPoint.localRotation.z, mtfRealHangModelPoint.localRotation.w);
        }
    }

    public void initList()
    {
        int objCount = mgoSuitBtn.Length;
        
        CSolider soldier = mclsPM.ca.mSolider;
        if(soldier==null)
            return;

        int anmCOunt = soldier.animationList.Count;
        int minCount = (objCount > anmCOunt) ? anmCOunt : objCount;
        float y = 200f;
        float x = -340;
        float yPer = -40f;
        for (int nIndex = 0; nIndex < minCount; nIndex++)
        {
            mgoSuitBtn[nIndex].SetActive(true);
            mgoSuitBtn[nIndex].transform.localPosition = new Vector3(x, y + yPer * nIndex,0);
            Transform lable = mgoSuitBtn[nIndex].transform.FindChild("Label");
            if (lable != null)
            {
                UILabel textLbl = lable.GetComponent<UILabel>();
                if (textLbl != null)
                    textLbl.text = soldier.animationList[nIndex];
            }
        }
    }

    public override PanelID GetPanelID()
    {
        return id;
    }

    protected override void ReleaseImp()
    {

    }

    void _OnIconButtonClick(GameObject go)
    {
        Transform lable = go.transform.FindChild("Label");
        if (lable != null)
        {
            UILabel textLbl = lable.GetComponent<UILabel>();
            if (textLbl != null)
            {
                CSolider soldier = mclsPM.ca.mSolider;
                if (soldier != null)
                    soldier.execParam(textLbl.text);
            }
        }
    }

    void _OnClickNull()
    {
        this.SetVisible(false);

        CardHeroListPanel chlp = (CardHeroListPanel)PanelManage.me.getPanel(PanelID.CardHeroListPanel);
        if (chlp != null)
        {
            chlp.SetVisible(true);
        }
    }

    public void setData(UICardLogic clsCl)
    {
        mclsData = clsCl;

        _UpdateDetail();
    }

    void _UpdateDetail()
    {
        UICardMgr.CItemData ci = UICardMgr.singleton.getItemById(mclsData.mnId);//HeroItemMgr.singleton.getCardItemById(mclsData.mnId);
        //CardData cd = CsvConfigMgr.me.getHeroDetailByTypeId(ci.nTypeId);
        DataMgr.ConfigRow cr = null;
        DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)ci.nTypeId, out cr);
        if (cr == null)
            return ;

        string s = cr.getStringValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_ATTACK);//cd.getAttributeStringValue(CardData.enAttributeName.enAN_AttackPower);
        string s1 = cr.getStringValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_HP);// cd.getAttributeStringValue(CardData.enAttributeName.enAN_HP);
        string s2 = cr.getStringValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_DEF);// cd.getAttributeStringValue(CardData.enAttributeName.enAN_AttackPower);
        string s3 = cr.getStringValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_LEADER);// cd.getAttributeStringValue(CardData.enAttributeName.enAN_LeadPower);
        string s4 =  "100";

        mLabelContext.text = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n", s, s1, s2, s3, s4);
        mLabelTitle.text = cr.getStringValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.NAME_ID);// cd.getAttributeStringValue(CardData.enAttributeName.enAN_Name);

        string str = "Assets/Data/TestModels/Heros/Sparta_Higher/Sparta_Higher.prefab";
        Object obj = DataMgr.ResourceCenter.LoadAsset<Object>(str);
        if (obj == null)
            Logger.LogDebug("CardHeroDetailPanel::_UpdateDetail  is null");
        else
            mgoModel = (GameObject)GameObject.Instantiate(obj);

    }

    void _OnCloseClick(GameObject go)
    {
        _OnClickNull();
    }

    void setObjLayer(GameObject obj, int nLayer)
    {
        if (obj == null)
            return;
        obj.layer = nLayer;
        int nCount = obj.transform.childCount;
        for (int n = 0; n < nCount; n++)
        {
            Transform child = obj.transform.GetChild(n);
            setObjLayer(child.gameObject, nLayer);
        }
    }

}

}

