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
namespace UI
{

public class CardIllustratedDetailPanel : PanelBase
{
    public const PanelID id = PanelID.CardIllustratedDetailPanel;

    private GameObject mgoDetailRoot = null;
    private string mstrDetailRoot = "DetailRoot";
    private List<GameObject> mlistGoPrefab = new List<GameObject>();

    [HideInInspector]
    public int mnId = CConstance.INVALID_ID;
    [HideInInspector]
    public int mnIndex = CConstance.INVALID_ID;

    private UILabel mLabelContext = null;
    private UILabel mLabelTitle = null;
    private UILabel mLabelDescribe = null;

    private List<UILabel> mLabelName = new List<UILabel>();
    private List<UILabel> mLabelNameValue = new List<UILabel>();

    private UISprite mIconSprite = null;
    private List<UILabel> mlistSkill = new List<UILabel>();
    private List<Transform> mtfLevelBtn = new List<Transform>();
    private List<UISprite> mlistSkillBar = new List<UISprite>();
    //private List<GameObject> mlistGOSkill = new List<GameObject>();
    private List<int> mlistSkillId = new List<int>();

    private Transform mtftCloseBtn = null;
    private Transform mtfBackBtn = null;
    private Transform mtfQualityBtn = null;
    private UISprite mtfStarBar = null;
    private UISprite mtfExpBar = null;
    private Transform mtfExpRoot = null;

    private Transform mtfDataRoot = null;

    private string mstrSmallName = "";


    // Use this for initialization
    public CardIllustratedDetailPanel()
    {
    }

    public override string GetResPath()
    {
        return "DetailWin.prefab";
    }

    protected override void Initimp(List<GameObject> prefabs)
    {
        for (int i = 1; i < prefabs.Count; i++ )
            mlistGoPrefab.Add(prefabs[i]); // skillroot

		//Root.transform.localScale = new UnityEngine.Vector3(4.0f, 4.0f, 4.0f);

        UIAnchor uia = Root.GetComponent<UIAnchor>();
        if (uia != null)
        {
            uia.uiCamera = UICardMgr.getCamera().GetComponent<Camera>();
        }

        Transform tfDR = Root.transform.FindChild(mstrDetailRoot);
        if (tfDR != null)
        {
            //tfDR.gameObject.AddComponent<UICardDetailLogic>();

            mtfBackBtn = UICardMgr.findChild(Root.transform, "btnRoot,backBtn");
            if (mtfBackBtn != null)
            {
                UIEventListener.Get(mtfBackBtn.gameObject).onClick = _OnBackBtnClick;
            }

            mtftCloseBtn = UICardMgr.findChild(Root.transform, "btnRoot,close");
            if (mtfBackBtn != null)
            {
                UIEventListener.Get(mtftCloseBtn.gameObject).onClick = _OnBackBtnClick;
            }

            mtfQualityBtn = UICardMgr.findChild(Root.transform, "DetailRoot,introductionRoot,context,normalbtn");
            if (mtfQualityBtn != null)
            {
                UIEventListener.Get(mtfQualityBtn.gameObject).onClick = _OnQuiltyBtnClick;
            }

            Transform tfArea = UICardMgr.findChild(Root.transform, "DetailRoot,introductionRoot,dataRoot");
            if(tfArea != null)
                mtfDataRoot = tfArea;

            tfArea = UICardMgr.findChild(Root.transform, "LeftArea,icon");
            if (tfArea != null)
                mIconSprite = tfArea.GetComponent<UISprite>();
            tfArea = UICardMgr.findChild(Root.transform, "LeftArea,icon,star");
            if (tfArea != null)
                mtfStarBar = tfArea.GetComponent<UISprite>();

            UILabel uil = UICardMgr.FindChild<UILabel>(Root.transform, "DetailRoot,introductionRoot,LabelTitle");
            if (uil != null)
                mLabelTitle = uil;

            uil = UICardMgr.FindChild<UILabel>(Root.transform, "DetailRoot,introductionRoot,context,auttribute,ATK");
            if (tfArea != null)
            {
                mLabelName.Add(uil);
            }
            uil = UICardMgr.FindChild<UILabel>(Root.transform, "DetailRoot,introductionRoot,context,auttribute,ATKValue");
            if (tfArea != null)
            {
                mLabelNameValue.Add(uil);
            }

            // def
            uil = UICardMgr.FindChild<UILabel>(Root.transform, "DetailRoot,introductionRoot,context,auttribute,DEF");
            if (tfArea != null)
            {
                mLabelName.Add(uil);
            }
            uil = UICardMgr.FindChild<UILabel>(Root.transform, "DetailRoot,introductionRoot,context,auttribute,DEFValue");
            if (tfArea != null)
            {
                mLabelNameValue.Add(uil);
            }

            // HP
            uil = UICardMgr.FindChild<UILabel>(Root.transform, "DetailRoot,introductionRoot,context,auttribute,HP");
            if (tfArea != null)
            {
                mLabelName.Add(uil);
            }
            uil = UICardMgr.FindChild<UILabel>(Root.transform, "DetailRoot,introductionRoot,context,auttribute,HPValue");
            if (tfArea != null)
            {
                mLabelNameValue.Add(uil);
            }

            // LED
            uil = UICardMgr.FindChild<UILabel>(Root.transform, "DetailRoot,introductionRoot,context,auttribute,LED");
            if (tfArea != null)
            {
                mLabelName.Add(uil);
            }
            uil = UICardMgr.FindChild<UILabel>(Root.transform, "DetailRoot,introductionRoot,context,auttribute,LEDValue");
            if (tfArea != null)
            {
                mLabelNameValue.Add(uil);
            }

            tfArea = UICardMgr.findChild(Root.transform, "DetailRoot,introductionRoot,context,expProcessBar");
            if (tfArea != null)
                mtfExpRoot = tfArea;
            tfArea = UICardMgr.findChild(mtfExpRoot, "expProcessBar");
            if (tfArea != null)
                mtfExpBar = tfArea.GetComponent<UISprite>();

            tfArea = UICardMgr.findChild(Root.transform, "DetailRoot,introductionRoot,context,AttendantslBtn");
            if (tfArea != null)
            {
                UIEventListener.Get(tfArea.gameObject).onClick = _OnOpenAttendants;
            }

        }

        SetVisible(false);

        GlobalEventSet.SubscribeEvent(eEventType.HeroUpgradeEvent, id, this._UpdateList);

        _FlushTableText();
    }

    void _FlushTableText()
    {
        UICardMgr.setLabelText(Root, "DetailRoot,introductionRoot,LabelTitle", 14009);
        UICardMgr.setLabelText(Root, "DetailRoot,introductionRoot,context,normalbtn,Label", 14015);
        UICardMgr.setLabelText(Root, "DetailRoot,introductionRoot,context,AttendantslBtn,Label", 14016);
        UICardMgr.setLabelText(Root, "DetailRoot,introductionRoot,context,auttribute,ATK", 14010);
        UICardMgr.setLabelText(Root, "DetailRoot,introductionRoot,context,auttribute,DEF", 14011);
        UICardMgr.setLabelText(Root, "DetailRoot,introductionRoot,context,auttribute,HP", 14012);
        UICardMgr.setLabelText(Root, "DetailRoot,introductionRoot,context,auttribute,LED", 14014);
        UICardMgr.setLabelText(Root, "DetailRoot,introductionRoot,context,expProcessBar,EXPLabel", 14013);
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

    }

    public override void SetVisible(bool value)
    {
        base.SetVisible(value);

        //if (value == false)
        {
            //setDataRootPosition(-112);
            setExpPosition(-50f);
        }
    }

    private void setExpPosition(float fy)
    {
        mtfExpRoot.transform.localPosition = new UnityEngine.Vector3(mtfExpRoot.transform.localPosition.x, fy,
                                                mtfExpRoot.transform.localPosition.z);
    }

    void setDataRootPosition(float fy)
    {
        UIPanel uipl = mtfDataRoot.GetComponent<UIPanel>();
        uipl.clipRange = new Vector4(0, 0, uipl.clipRange.z, uipl.clipRange.w);

        mtfDataRoot.localPosition = new UnityEngine.Vector3(
            mtfDataRoot.localPosition.x, fy, mtfDataRoot.localPosition.z);
    }

    void _OnBackBtnClick(GameObject go)
    {
        this.SetVisible(false);

        PanelBase pb = PanelManage.me.getPanel(PanelID.CardIllustratedListPanel);
        if (pb != null)
        {
            pb.SetVisible(true);
        }
    }

    void _OnQuiltyBtnClick(GameObject go)
    {
        this.SetVisible(false);

        PanelBase pb = PanelManage.me.getPanel(PanelID.CardQualityUpdatePanel);
        if (pb != null)
        {
            CardQualityUpdatePanel cqup = (CardQualityUpdatePanel)pb;

            cqup.CurSelectId = mnId;

            pb.SetVisible(true);
        }
    }

    void _OnLevelBtnClick(GameObject go)
    {
        this.SetVisible(false);

        int nIndex = CConstance.INVALID_ID;
        for (int i = 0; i < mtfLevelBtn.Count; i++)
        {
            if (go == mtfLevelBtn[i].gameObject)
            {
                nIndex = i;
                break;
            }
        }
        
        if (nIndex != CConstance.INVALID_ID)
        {
            int skillId = mlistSkillId[nIndex];
            HeroSkill.HeroSkillMgr.me.updateSkill(this.mnId, skillId);
        }
 
    }

    public void updateDetial(UICardLogic refUIcl)
    {
        UICardLogic uil = refUIcl;
        if (uil == null)
            return;

        GameObject go = uil.gameObject;
        if (go == null)
            return;
        Transform tfRef = go.transform.FindChild("Sprite");
        if (null == tfRef)
            return;
        UISprite refUIS = tfRef.GetComponent<UISprite>();
        if (null == refUIS)
            return;

        this.mnId = uil.mnId;
        this.mnIndex = uil.mnIndex;

        UICardMgr.CItemData ci = UICardMgr.singleton.getIllustratedItemById(this.mnId);
        if (ci == null)
            return;

        setExpPosition(-50f);

        mIconSprite.atlas = refUIS.atlas;
        //mstrSmallName = refUIS.spriteName;

        //int nIndex = getIndexByName(mstrSmallName);

        mIconSprite.spriteName = UICardLogic.getSpriteNameBig(ci.nTypeId);// mszQualityName[nIndex];
        mtfStarBar.spriteName = UICardLogic.getStarNameBig(ci.nTypeId);   // mszQualityNameStar[nIndex];

        string s = getAtk((uint)ci.nTypeId, this.mnId); 
        string s1 = getHP((uint)ci.nTypeId, this.mnId);
        string s2 = getDef((uint)ci.nTypeId, this.mnId);
        string s3 = getLead((uint)ci.nTypeId, this.mnId);

        mLabelNameValue[0].text = s;
        mLabelNameValue[1].text = s2;
        mLabelNameValue[2].text = s1;
        mLabelNameValue[3].text = s3;

        DataMgr.ConfigRow cr = null;
        DataMgr.CHerroTalbeAttribute.getHeroBaseDetail(ci.nTypeId, out cr);
        if (cr != null)
        {
            string str = DataMgr.DataManager.getLanguageMgr().getString(14009);
            int nNameId = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.NAME_ID);
            string str1 = DataMgr.DataManager.getLanguageMgr().getString(nNameId);

            mLabelTitle.text = "[" + str1 + "]" + str;
        }

        updateStar();
        updateSkillBar();
        updateExpBar();

    }

    public static int getIndexByName(string strName)
    {
        for (int i = 0; i < UICardLogic.mszCardNameSmall.Length; i++)
        {
            if (strName == UICardLogic.mszCardNameSmall[i])
            {
                return i;
            }
        }

        return 0;
    }

    public void updateStar()
    {
        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)mnId, ref hi);
        if (!bRet)
            return;

        float fbase = HeroData.FillValue * hi.u8Star;
        mtfStarBar.fillAmount = fbase;

        UILabel uil = UICardMgr.FindChild<UILabel>(Root, "LeftArea,icon,Info,ATK");
        if (uil != null)
        {
            uil.text = CardIllustratedDetailPanel.getAtk(hi.idType, (int)hi.idHero);
        }
        uil = UICardMgr.FindChild<UILabel>(Root, "LeftArea,icon,Info,DEF");
        if (uil != null)
        {
            uil.text = CardIllustratedDetailPanel.getDef(hi.idType, (int)hi.idHero);
        }
        uil = UICardMgr.FindChild<UILabel>(Root, "LeftArea,icon,Info,HP");
        if (uil != null)
        {
            uil.text = CardIllustratedDetailPanel.getLead(hi.idType, (int)hi.idHero);
        }
        uil = UICardMgr.FindChild<UILabel>(Root, "LeftArea,icon,Info,FG");
        if (uil != null)
        {
            uil.text = CardIllustratedDetailPanel.getFight(hi.idType, (int)hi.idHero);
        }
        uil = UICardMgr.FindChild<UILabel>(Root, "LeftArea,icon,Info,LV");
        if (uil != null)
        {
            uil.text = hi.usLevel.ToString();
        }
    }

    public void updateSkillBar()
    {
        mlistSkillId.Clear();

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)mnId, ref hi);
        if (!bRet)
            return;

        mlistSkillId.Add((int)hi.unSkill1);
        mlistSkillId.Add((int)hi.unSkill2);
        mlistSkillId.Add((int)hi.unSkill3);

        _ResetSkillList();
        _UpdateDescribute();
    }

    public void updateExpBar()
    {
        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)mnId, ref hi);
        if (!bRet)
            return;

        uint dwNext = 1;// hi.unExp;
        ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CVS_HERO_EXP);
        ConfigRow cr = cb.getRow(enCVS_HERO_EXP_ATTRIBUTE.LEVEL, hi.usLevel + 1);
        if (cr != null)
        {
            dwNext = (uint)cr.getIntValue(enCVS_HERO_EXP_ATTRIBUTE.EXP);
        }
        float fbase = hi.unExp / dwNext;//HeroData.FillValue * hi.unExp;
        mtfExpBar.fillAmount = fbase;
    }

    bool _UpdateList(SLG.EventArgs obj)
    {
        HeroUpgradePanel.UpgradeEvent ue = (HeroUpgradePanel.UpgradeEvent)(obj.m_obj);

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)ue.id, ref hi);
        if (!bRet)
            return false;

        for ( int i = 0; i < ue.eatids.Count; i++)
        {
            int nid = ue.eatids[i];
            UICardMgr.singleton.RemoveIllustratedItemById(nid);
        }

        return true;
    }

    void _ResetSkillList()
    {
        Transform tfArea = UICardMgr.findChild(Root.transform, "DetailRoot,introductionRoot,dataRoot,Area");
        if(tfArea == null)
            return;

        mtfLevelBtn.Clear();
        mlistSkill.Clear();
        mlistSkillBar.Clear();

        foreach (Transform item in tfArea)
        {
            GameObject.Destroy(item.gameObject);
        }

        UITable uit = tfArea.GetComponent<UITable>();

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)mnId, ref hi);
        if (!bRet)
            return;

        List<uint> skillExp = new List<uint>();
        List<uint> skillLv = new List<uint>();

        skillExp.Add(hi.unSkillExp1);
        skillExp.Add(hi.unSkillExp2);
        skillExp.Add(hi.unSkillExp3);

        skillLv.Add(hi.usSkillLvl1);
        skillLv.Add(hi.usSkillLvl2);
        skillLv.Add(hi.usSkillLvl3);

        SkillData sd = DataManager.getSkillData();
        int nIndex = 0;
        float fx = 180f, fy = -50f, fOffsety = 100f;
        foreach (int nTypeId in mlistSkillId)
        {
            if (nTypeId == CConstance.DEFAULT_ID)
                continue;

            GameObject go = NGUITools.AddChild(tfArea.gameObject, mlistGoPrefab[0]);
            go.transform.parent = tfArea;
            go.transform.localPosition = new UnityEngine.Vector3(fx, fy - nIndex * fOffsety, 0f);

            Transform tfItem = go.transform;
            Transform tfTempBtn = UICardMgr.findChild(tfItem, "normalbtn");
            if (tfTempBtn != null)
            {
                mtfLevelBtn.Add(tfTempBtn);
                UIEventListener.Get(mtfLevelBtn[nIndex].gameObject).onClick = _OnLevelBtnClick;
            }

            tfTempBtn = UICardMgr.findChild(tfItem, "iconbtn,Label");
            if (tfTempBtn != null)
            {
                UILabel uil = tfTempBtn.GetComponent<UILabel>();
                mlistSkill.Add(uil);
                if (sd != null)
                {
                    DataMgr.Skill sl = sd.getSkillBySkillID(nTypeId);
                    if (sl != null)
                    {
                        string str = DataMgr.DataManager.getLanguageMgr().getString(sl.NameId);
                        string str1 = DataMgr.DataManager.getLanguageMgr().getString(sl.DescId);
                        uil.text = str + "\n" + str1;
                    }
                }
            }

            tfTempBtn = UICardMgr.findChild(tfItem, "ProcessBar,fg,skillProcessBar");
            if (tfTempBtn != null)
            {
                UISprite uil = tfTempBtn.GetComponent<UISprite>();
                mlistSkillBar.Add(uil);

                int lv = (int)skillLv[nIndex];
                uint next = 1;

                DataMgr.ConfigRow crstar = null;
                DataMgr.CHerroTalbeAttribute.getHeroStar(nTypeId, lv, out crstar);
                if (crstar != null)
                {
                    next = (uint)crstar.getIntValue(DataMgr.enCVS_HERO_EXP_ATTRIBUTE.EXP);
                    float f = (float)skillExp[nIndex] / (float)(next + 1);
                    mlistSkillBar[nIndex].fillAmount = f;
                }
            }

            //uit.repositionNow = true;
            nIndex++;
        }

        GameObject goDesribute = NGUITools.AddChild(tfArea.gameObject, mlistGoPrefab[1]);
        goDesribute.transform.parent = tfArea;
        goDesribute.transform.localPosition = new UnityEngine.Vector3(fx, fy - nIndex * fOffsety, 0f);
        mLabelDescribe = goDesribute.GetComponent<UILabel>();

        uit.repositionNow = true;
    }

    void _UpdateDescribute()
    {
        UICardMgr.CItemData ci = UICardMgr.singleton.getIllustratedItemById(this.mnId);
        if (ci == null)
            return;

        DataMgr.ConfigRow cr = null;
        DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)ci.nTypeId, out cr);
        if (cr == null)
            return;

        int skillTypeId = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL1_TYPEID);  //Convert.ToInt32(cd.getAttributeStringValue(CardData.enAttributeName.enAN_Skill01));

        DataMgr.ConfigRow crSkill = null;
        DataMgr.CHerroTalbeAttribute.getHeroSkill(skillTypeId, out crSkill);
        if (crSkill != null && mlistSkill.Count >=1 )
        {
            mlistSkill[0].text = crSkill.getStringValue(DataMgr.CFG_SKILL.NAME);
        }

        skillTypeId = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL2_TYPEID);
        crSkill = null;
        if (crSkill != null && mlistSkill.Count >= 2)
        {
            mlistSkill[1].text = crSkill.getStringValue(DataMgr.CFG_SKILL.NAME);
        }

        int nDesId = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.DESC_ID);
        //nDesId = 10101 + UnityEngine.Random.Range(1, 10);
        string str = DataMgr.DataManager.getLanguageMgr().getString(nDesId);
        mLabelDescribe.text = str;// cr.getStringValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.DESCRIPTION);
    }

    void _OnOpenAttendants(GameObject go)
    {
        AttendantsPanel ap = PanelManage.me.GetPanel<AttendantsPanel>(PanelID.AttendantsPanel);
        if (ap != null)
        {
            ap.SetVisible(true);
            ap.setId((uint)mnId);
        }
    }

    public static string getAtk(uint dwTypeId, int nHeroId, bool bNext = false)
    {
        string str = "";

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)nHeroId, ref hi);
        if (!bRet)
            return "error";

        DataMgr.ConfigRow cr = null;
        DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)dwTypeId, out cr);
        if (cr == null)
            return "error";

        int nAtc = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_ATTACK);

        int lLv = hi.usLevel;
        int lStar = hi.u8Star;
        if (bNext)
            lStar = hi.u8Star + 1;

        DataMgr.ConfigRow crstar = null;
        DataMgr.CHerroTalbeAttribute.getHeroStar((int)dwTypeId, lStar, out crstar);
        if (crstar == null)
            return "error";

        int nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_ATTACK);
        int nCurAtc = UI.CardQualityUpdatePanel.GetValue((float)nAtc, lStar, lLv, nFactor);

        str = nCurAtc.ToString();

        return str;
    }

    public static string getDef(uint dwTypeId, int nHeroId, bool bNext = false)
    {
        string str = "";

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)nHeroId, ref hi);
        if (!bRet)
            return "error";

        DataMgr.ConfigRow cr = null;
        DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)dwTypeId, out cr);
        if (cr == null)
            return "error";

        int nDef = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_DEF);

        int lLv = hi.usLevel;
        int lStar = hi.u8Star;
        if (bNext)
            lStar = hi.u8Star + 1;

        DataMgr.ConfigRow crstar = null;
        DataMgr.CHerroTalbeAttribute.getHeroStar((int)dwTypeId, lStar, out crstar);
        if (crstar == null)
            return "error";

        int nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_DEF);
        int nCurDef = UI.CardQualityUpdatePanel.GetValue((float)nDef, lStar, lLv, nFactor);

        str = nCurDef.ToString();

        return str;
    }

    public static string getHP(uint dwTypeId, int nHeroId, bool bNext = false)
    {
        string str = "";

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)nHeroId, ref hi);
        if (!bRet)
            return "error";

        DataMgr.ConfigRow cr = null;
        DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)dwTypeId, out cr);
        if (cr == null)
            return "error";

        int nHP = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_HP);

        int lLv = hi.usLevel;
        int lStar = hi.u8Star;
        if (bNext)
            lStar = hi.u8Star + 1;

        DataMgr.ConfigRow crstar = null;
        DataMgr.CHerroTalbeAttribute.getHeroStar((int)dwTypeId, lStar, out crstar);
        if (crstar == null)
            return "error";

        int nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_HP);
        int nCurHP = UI.CardQualityUpdatePanel.GetValue((float)nHP, lStar, lLv, nFactor);

        str = nCurHP.ToString();

        return str;
    }

    public static string getLead(uint dwTypeId, int nHeroId, bool bNext = false)
    {
        string str = "";

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)nHeroId, ref hi);
        if (!bRet)
            return "error";

        DataMgr.ConfigRow cr = null;
        DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)dwTypeId, out cr);
        if (cr == null)
            return "error";

        int nLead = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_LEADER);

        int lLv = hi.usLevel;
        int lStar = hi.u8Star;
        if (bNext)
            lStar = hi.u8Star + 1;

        DataMgr.ConfigRow crstar = null;
        DataMgr.CHerroTalbeAttribute.getHeroStar((int)dwTypeId, lStar, out crstar);
        if (crstar == null)
            return "error";

        int nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_LEADER);
        int nCurLead = UI.CardQualityUpdatePanel.GetValue((float)nLead, lStar, lLv, nFactor);

        str = nCurLead.ToString();

        return str;
    }

    public static string getFight(uint dwTypeId, int nHeroId, bool bNext = false)
    {
        string str = "";

        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)nHeroId, ref hi);
        if (!bRet)
            return "error";

        int n = getSkillCount(nHeroId);

        string sAtc = getAtk(dwTypeId, nHeroId, bNext);
        string sDef = getDef(dwTypeId, nHeroId, bNext);
        string sHp = getHP(dwTypeId, nHeroId, bNext);
        string sLead = getLead(dwTypeId, nHeroId, bNext);

        uint nCurHP = 0, nCurAtc = 0, nCurDef = 0, nCurLead = 0;
        System.UInt32.TryParse(sAtc, out nCurAtc);
        System.UInt32.TryParse(sDef, out nCurDef);
        System.UInt32.TryParse(sHp, out nCurHP);
        System.UInt32.TryParse(sLead, out nCurLead);

        float dwPower = (float)(nCurHP + 40 * nCurAtc + 20 * nCurDef + 35 * nCurLead);
        float dwP2 = (float)(1 + 0.04f);
        float dwP3 = (float)(1 + 0.05 * n + 0.01*hi.usSkillLvl1 + 0.01 * hi.usSkillLvl2 + 0.01 * hi.usSkillLvl3);

        uint dwTotal = (uint)(dwPower + dwP2 + dwP3);
        str = dwTotal.ToString();
        return str;
    }

    public static int getSkillCount(int nHeroId)
    {
        int n = 0;
        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bool bRet = HeroData.getHeroData((uint)nHeroId, ref hi);
        if (!bRet)
            return 0;

        if (hi.unSkill1 != CConstance.DEFAULT_ID)
            n++;
        if (hi.unSkill2 != CConstance.DEFAULT_ID)
            n++;
        if (hi.unSkill3 != CConstance.DEFAULT_ID)
            n++;

        return n;
    }
}


}

