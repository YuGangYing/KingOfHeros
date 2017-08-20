using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Packet;
using DataMgr;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
namespace UI
{
    public class CardQualityUpdatePanel : PanelBase
    {
        public const PanelID id = PanelID.CardQualityUpdatePanel;

        private GameObject mgoItemQualityPrefab = null;
        private GameObject mgoItemHeroPrefab = null;
        private GameObject mgoIconBtnPrefab = null;

        private GameObject mgoQualityRoot = null;
        private GameObject mgoHangHeroRoot = null;
        private GameObject mgoItemHangPoint = null;

        private GameObject mgoBackBtn = null;
        private GameObject mgoUpgradeBtn = null;

        private Transform mtfCloseBtn = null;

        // upgrade info
        private int mnIconAmount = 0;
        private List<GameObject> mgoIcon = new List<GameObject>();
        private List<UILabel> mlistLabelCur = new List<UILabel>();
        private List<UILabel> mlistLabelNext = new List<UILabel>();

        private UILabel mlabelCost = null;

        private Transform mtfUpgrade = null;

        private List<UILabel> mlistLabelName = new List<UILabel>();

        //  left area info
        private UISprite mCurSprite = null;
        private UISprite mCurStar = null;
        private UILabel mlabelMoney = null;
        private UISprite mSpriteProcessBar = null;

        // hang area info
        private List<GameObject> mlist = new List<GameObject>();

        public Transform mConfirmBtn = null;

        // cur cur select hero id
        private int mnCurSelectHeroId = CConstance.INVALID_ID;
        public int CurSelectId
        {
            get { return mnCurSelectHeroId; }
            set { mnCurSelectHeroId = value; UpdateData(); }
        }

        public int mnUpgradeCount = 2;


        // Use this for initialization
        public CardQualityUpdatePanel()
        {
        }

        public override string GetResPath()
        {
            return "QualityUpdateWin.prefab";
        }

        protected override void Initimp(List<GameObject> prefabs)
        {
            UIAnchor uia = Root.GetComponent<UIAnchor>();
            if (uia != null)
            {
                uia.uiCamera = UICardMgr.getCamera().GetComponent<Camera>();
            }

            mgoItemHeroPrefab = prefabs[1];
            mgoItemQualityPrefab = prefabs[2];
            mgoIconBtnPrefab = prefabs[3];

            Transform tf = UICardMgr.findChild(Root.transform, "BackgroundRoot,close");
            if(tf != null)
                UIEventListener.Get(tf.gameObject).onClick = OnBackClick;

            tf = UICardMgr.findChild(Root.transform, "QualityRoot");
            if (tf != null)
                mgoQualityRoot = tf.gameObject;

            tf = UICardMgr.findChild(Root.transform, "HangHeroRoot");
            if (tf != null)
                mgoHangHeroRoot = tf.gameObject;

            tf = UICardMgr.findChild(Root.transform, "HangHeroRoot,DetailRoot,HangArea");
            if (tf != null)
                mgoItemHangPoint = tf.gameObject;

            tf = UICardMgr.findChild(Root.transform, "QualityRoot,btnRoot,updateBtn");
            if (tf != null)
            {
                mtfUpgrade = tf;
                UIEventListener.Get(tf.gameObject).onClick = OnUpdateClick;
            }

            tf = UICardMgr.findChild(Root.transform, "QualityRoot,btnRoot,Sprite,Label");
            if (tf != null)
            {
                UILabel ui = tf.GetComponent<UILabel>();
                mlistLabelName.Add(ui);
            }

            Transform tfTemp = UICardMgr.findChild(Root.transform, "QualityRoot,Introduction,describeRoot,CostNo");
            if (tfTemp != null)
            {
                mlabelCost = tfTemp.GetComponent<UILabel>();
                mlabelCost.text = "10";
            }

            tf = UICardMgr.findChild(Root.transform, "QualityRoot,Introduction,describeRoot,LabelRoot,curDetail");
            if (tf != null)
            {
                UILabel uil = null;
                tfTemp = UICardMgr.findChild(tf, "atcNo");
                if (tfTemp != null)
                {
                    uil = tfTemp.GetComponent<UILabel>();
                    mlistLabelCur.Add(uil);
                }

                tfTemp = UICardMgr.findChild(tf, "hpNo");
                if (tfTemp != null)
                {
                    uil = tfTemp.GetComponent<UILabel>();
                    mlistLabelCur.Add(uil);
                }

                tfTemp = UICardMgr.findChild(tf, "defNo");
                if (tfTemp != null)
                {
                    uil = tfTemp.GetComponent<UILabel>();
                    mlistLabelCur.Add(uil);
                }

                tfTemp = UICardMgr.findChild(tf, "ledNo");
                if (tfTemp != null)
                {
                    uil = tfTemp.GetComponent<UILabel>();
                    mlistLabelCur.Add(uil);
                }
            }

            tf = UICardMgr.findChild(Root.transform, "QualityRoot,Introduction,describeRoot,LabelRoot,nextdetails");
            if (tf != null)
            {
                UILabel uil = null;
                tfTemp = UICardMgr.findChild(tf, "atcNo");
                if (tfTemp != null)
                {
                    uil = tfTemp.GetComponent<UILabel>();
                    mlistLabelNext.Add(uil);
                }

                tfTemp = UICardMgr.findChild(tf, "hpNo");
                if (tfTemp != null)
                {
                    uil = tfTemp.GetComponent<UILabel>();
                    mlistLabelNext.Add(uil);
                }

                tfTemp = UICardMgr.findChild(tf, "defNo");
                if (tfTemp != null)
                {
                    uil = tfTemp.GetComponent<UILabel>();
                    mlistLabelNext.Add(uil);
                }

                tfTemp = UICardMgr.findChild(tf, "ledNo");
                if (tfTemp != null)
                {
                    uil = tfTemp.GetComponent<UILabel>();
                    mlistLabelNext.Add(uil);
                }
            }

            tf = UICardMgr.findChild(Root.transform, "LeftArea,Sprite");
            if (tf != null)
            {
                mCurSprite = tf.GetComponent<UISprite>();

                tfTemp = UICardMgr.findChild(tf, "Star");
                if (tfTemp != null)
                {
                    mCurStar = tfTemp.GetComponent<UISprite>();
                }
            }

            tf = UICardMgr.findChild(Root.transform, "LeftArea,goldCoin,goldProcessBar");
            if (tf != null)
            {
                mSpriteProcessBar = tf.GetComponent<UISprite>();
            }

            tf = UICardMgr.findChild(Root.transform, "LeftArea,goldCoin,goldNo");
            if (tf != null)
            {
                mlabelMoney = tf.GetComponent<UILabel>();
            }

            tf = UICardMgr.findChild(Root.transform, "LeftArea,ChangeCard");
            if (tf != null)
                UIEventListener.Get(tf.gameObject).onClick = _OnChangeCard;

            tfTemp = UICardMgr.findChild(Root.transform, "HangHeroRoot,TitleRoot,confirmBtn");
            if (tfTemp != null)
            {
                mConfirmBtn = tfTemp;
                UIEventListener.Get(mConfirmBtn.gameObject).onClick = _OnSelectConfirmBtnClick;
            }

            tfTemp = UICardMgr.findChild(Root.transform, "HangHeroRoot,TitleRoot,close");
            if (tfTemp != null)
            {
                UIEventListener.Get(tfTemp.gameObject).onClick = _OnCloseHangRoot;
            }

            tf = UICardMgr.findChild(Root.transform, "HangHeroRoot,TitleRoot,Sprite,Label");
            if (tf != null)
            {
                UILabel ui = tf.GetComponent<UILabel>();
                mlistLabelName.Add(ui);
            }

            setPanelAlpha();

            SetVisible(false);

            SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyRefreshQualityUpgrade, this._OnRefreshPosition);

            _FlushTableText();
        }

        void _FlushTableText()
        {
            UICardMgr.setLabelText(Root, "QualityRoot,btnRoot,Sprite,Label", 14015);
            UICardMgr.setLabelText(Root, "LeftArea,ChangeCard,Background,Label", 14019);
            UICardMgr.setLabelText(Root, "QualityRoot,Introduction,describeRoot,CostNo,Label", 14021);
            UICardMgr.setLabelText(Root, "QualityRoot,Introduction,describeRoot,LabelRoot,InfoTitleRoot,atcNo", 14010);
            UICardMgr.setLabelText(Root, "QualityRoot,Introduction,describeRoot,LabelRoot,InfoTitleRoot,defNo", 14011);
            UICardMgr.setLabelText(Root, "QualityRoot,Introduction,describeRoot,LabelRoot,InfoTitleRoot,hpNo", 14012);
            UICardMgr.setLabelText(Root, "QualityRoot,Introduction,describeRoot,LabelRoot,InfoTitleRoot,ledNo", 14014);
            UICardMgr.setLabelText(Root, "QualityRoot,Introduction,describeRoot,TipLabel", 14022);

            UICardMgr.setLabelText(Root, "HangHeroRoot,TitleRoot,confirmBtn,Label", 14018);
        }

        void setPanelAlpha()
        {
            _OnCloseHangRoot(null);

            ResetIcon();
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        protected override void ReleaseImp()
        {

        }

        public override void SetVisible(bool value)
        {
            base.SetVisible(value);

            if (value)
            {
                //Root.StartCoroutine(this._InitList);
                _InitList();
            }
            else
            {
                _Remove();
            }
        }

        protected override void onHide()
        {
            base.onHide();

            setPanelAlpha();
        }

        int getTypeId()
        {
            if (CurSelectId == CConstance.INVALID_ID)
                return CConstance.INVALID_ID;

            UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemById(CurSelectId);

            return cid.nTypeId;
        }

        void UpdateData()
        {
            if (CurSelectId == CConstance.INVALID_ID)
            {
                return;
            }

            UICardMgr.CItemData cid =UICardMgr.singleton.getIllustratedItemById(CurSelectId);

            mCurSprite.spriteName = UICardLogic.getSpriteNameBig(cid.nTypeId);// mszQualityName[nTemp - 1];
            mCurStar.spriteName = UICardLogic.getStarNameBig(cid.nTypeId);// mszQualityNameStar[nTemp - 1];

            Packet.HERO_INFO hi = new Packet.HERO_INFO();
            bool bRet = HeroData.getHeroData((uint)this.CurSelectId, ref hi);
            if (!bRet)
                return;

            int lLv = hi.usLevel;
            int lStar = hi.u8Star;

            float fFill = lStar * HeroData.FillValue;
            mCurStar.fillAmount = fFill;

            ConfigRow cr;
            bRet = CHerroTalbeAttribute.getHeroBaseDetail(cid.nTypeId, out cr);

            // set name
            if (bRet)
            {
                foreach (UILabel ui in mlistLabelName)
                {
                    int nNameId = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.NAME_ID);
                    string str1 = DataMgr.DataManager.getLanguageMgr().getString(nNameId);
                    ui.text = str1; // cr.getStringValue(enCVS_HERO_BASE_ATTRIBUTE.NAME); // cd.getAttributeStringValue(CardData.enAttributeName.enAN_Name);
                }
            }

            // cur
            mlistLabelCur[0].text = CardIllustratedDetailPanel.getAtk(hi.idType, (int)hi.idHero);//nCurAtc.ToString();
            mlistLabelCur[1].text = CardIllustratedDetailPanel.getHP(hi.idType, (int)hi.idHero);//nCurHP.ToString();
            mlistLabelCur[2].text = CardIllustratedDetailPanel.getDef(hi.idType, (int)hi.idHero);//nCurDef.ToString();
            mlistLabelCur[3].text = CardIllustratedDetailPanel.getLead(hi.idType, (int)hi.idHero);//nCurLead.ToString();

            // next
            mlistLabelNext[0].text = CardIllustratedDetailPanel.getAtk(hi.idType, (int)hi.idHero, true);// nCurAtc.ToString();
            mlistLabelNext[1].text = CardIllustratedDetailPanel.getHP(hi.idType, (int)hi.idHero, true);//nCurHP.ToString();
            mlistLabelNext[2].text = CardIllustratedDetailPanel.getDef(hi.idType, (int)hi.idHero, true);//nCurDef.ToString();
            mlistLabelNext[3].text = CardIllustratedDetailPanel.getLead(hi.idType, (int)hi.idHero, true);//nCurLead.ToString();

            mlabelMoney.text = RewardPanel.getMagicStones().ToString();

            UILabel uil = UICardMgr.FindChild<UILabel>(Root, "LeftArea,Sprite,Info,ATK");
            if (uil != null)
            {
                uil.text = CardIllustratedDetailPanel.getAtk(hi.idType, (int)hi.idHero);
            }
            uil = UICardMgr.FindChild<UILabel>(Root, "LeftArea,Sprite,Info,DEF");
            if (uil != null)
            {
                uil.text = CardIllustratedDetailPanel.getDef(hi.idType, (int)hi.idHero);
            }
            uil = UICardMgr.FindChild<UILabel>(Root, "LeftArea,Sprite,Info,HP");
            if (uil != null)
            {
                uil.text = CardIllustratedDetailPanel.getHP(hi.idType, (int)hi.idHero);
            }
            uil = UICardMgr.FindChild<UILabel>(Root, "LeftArea,Sprite,Info,FG");
            if (uil != null)
            {
                uil.text = CardIllustratedDetailPanel.getFight(hi.idType, (int)hi.idHero);
            }
            uil = UICardMgr.FindChild<UILabel>(Root, "LeftArea,Sprite,Info,LV");
            if (uil != null)
            {
                uil.text = hi.usLevel.ToString();
            }

            ResetIcon();
        }

        public static int GetValue(float lBase, int lStar, int lLv, int nFactor)
        {
            float fRet = (float)(lBase * (1 + 0.2 * lStar) + ( lLv + 7) * nFactor);
            Int64 n64 = (Int64)(fRet * 100);
            int nRet = (int)(n64 / 100);
            return nRet;
        }

        void OnBackClick(GameObject go)
        {
            this.SetVisible(false);

            CardIllustratedDetailPanel pb = (CardIllustratedDetailPanel)PanelManage.me.getPanel(PanelID.CardIllustratedDetailPanel);
            if (pb != null)
            {
                pb.SetVisible(true);

                ResetIcon();
            }
        }

        void OnUpdateClick(GameObject go)
        {
            //this.SetVisible(false);
            if (this.getSelectAmount() < mnUpgradeCount)
                return;

            uint nId = (uint)CurSelectId;
            List<uint> yy = new List<uint>();

            uint nIndex = 0;
            foreach (Transform item in mgoItemHangPoint.transform)
            {
                UICardQualityItem uicqi = item.GetComponent<UICardQualityItem>();
                if (uicqi != null && uicqi.mbSelect)
                {
                    nIndex = (uint)uicqi.mclsItemData.nId;
                    yy.Add(nIndex);
                }
            }

            DataMgr.DataManager.MsgHeroCard.requestCardQuality(nId, yy.ToArray());
        }

        void _OnIconBtnClick(GameObject go)
        {
            mgoQualityRoot.SetActive(false);
            mgoHangHeroRoot.SetActive(true);

        }

        void _OnSelectConfirmBtnClick(GameObject go)
        {
            if (this.getSelectAmount() < mnUpgradeCount)
                return;

            _OnCloseHangRoot(go);

            int nIndex = 0;
            foreach (Transform item in mgoItemHangPoint.transform)
            {
                UICardQualityItem uicqi = item.GetComponent<UICardQualityItem>();
                if (uicqi != null && uicqi.mbSelect)
                {
                    if (nIndex < mgoIcon.Count)
                    {
                        Transform tf = UICardMgr.findChild(mgoIcon[nIndex].transform, "png,nullIcon");
                        if (tf != null)
                        {
                            tf.gameObject.SetActive(false);
                        }

                        tf = UICardMgr.findChild(mgoIcon[nIndex].transform, "png,Sprite");
                        if (tf == null)
                            continue;
                        tf.gameObject.SetActive(true);
                        
                        UISprite uis = tf.GetComponent<UISprite>();
                        //uis.atlas = uicqi.mIconSprite.atlas;
                        uis.spriteName = uicqi.mIconSprite.spriteName;

                        tf = UICardMgr.findChild(tf, "Star");
                        if (tf == null)
                            continue;
                        uis = tf.GetComponent<UISprite>();
                        uis.spriteName = uicqi.mIconStar.spriteName;

                        Packet.HERO_INFO hi = new Packet.HERO_INFO();
                        bool bRet = HeroData.getHeroData((uint)uicqi.mclsItemData.nId, ref hi);
                        if (!bRet)
                            return;
                        
                        float fFill = hi.u8Star * HeroData.FillValue;
                        uis.fillAmount = fFill;
                        //uicqi.reset();
                    }

                    nIndex++;
                }
            }
        }

        void _InitList()
        {
            int nIndex = 0, nx = 190, ny = -30, nOffsetY = 120;
            int nAmount = UICardMgr.singleton.illustratedItemAmount;
            for (int i = 0; i < nAmount; i++)
            {
                UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemByIndex(i);
                if(cid == null)
                    continue;

                if(cid.nId == CurSelectId)
                    continue;

                if (cid.nTypeId != getTypeId())
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

            ResetIcon();
        }

        void _Remove()
        {
            foreach (Transform item in mgoItemHangPoint.transform)
            {
                GameObject.Destroy(item.gameObject);
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

            if (nCnt >= mnUpgradeCount)
            {
                UIButton uib = mtfUpgrade.GetComponent<UIButton>();
                uib.enabled = true;

                uib = mConfirmBtn.GetComponent<UIButton>();
                uib.enabled = true;
            }
            else
            {
                UIButton uib = mtfUpgrade.GetComponent<UIButton>();
                uib.enabled = false;

                uib = mConfirmBtn.GetComponent<UIButton>();
                uib.enabled = false;
            }

            return nCnt;
        }

        void ResetIcon()
        {
            foreach (GameObject item in mgoIcon)
            {
                GameObject.Destroy(item);
            }
            mgoIcon.Clear();

            //int nOffset = 50, nx = 90, ny = -45; // 800x480
            int nOffset = 50, nx = 114, ny = -45;  // 900x640
            Transform goTemp = UICardMgr.findChild(Root.transform, "QualityRoot,Introduction,dataRoot,Area");
            UITable uit = goTemp.GetComponent<UITable>();
            //uit.repositionNow = true;

            UIButton uib = mConfirmBtn.GetComponent<UIButton>();
            uib.enabled = false;

            uib = mtfUpgrade.GetComponent<UIButton>();
            uib.enabled = false;

            if (mnCurSelectHeroId == CConstance.INVALID_ID)
            {
                return;
            }

            Packet.HERO_INFO hi = new Packet.HERO_INFO();
            bool bRet = HeroData.getHeroData((uint)this.mnCurSelectHeroId, ref hi);
            if (!bRet)
                return;

            mnUpgradeCount = (hi.u8Star + 1) * 2;
            for (int i = 0; i < mnUpgradeCount; i++)
            {
                GameObject go = NGUITools.AddChild(goTemp.gameObject, mgoIconBtnPrefab);

                mgoIcon.Add(go);
                UIEventListener.Get(go).onClick = _OnIconBtnClick;
                go.AddComponent<UIDragScrollView>();

                go.transform.localPosition = new UnityEngine.Vector3(i * nx + nOffset, ny, 0.0f);

                Transform tf = UICardMgr.findChild(go.transform, "png,nullIcon");
                if (tf != null)
                {
                    tf.gameObject.SetActive(true);
                }
                tf = UICardMgr.findChild(go.transform, "png,Sprite");
                if (tf != null)
                {
                    tf.gameObject.SetActive(false);
                }

                uit.repositionNow = true;
            }
        }

        void _OnChangeCard(GameObject go)
        {
            _ResetChangeChard(true);
        }

        void _ResetChangeChard(bool bIncIndex)
        {
            _Remove();

            UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemById(this.CurSelectId);

            int nIndex = cid.nIndex;
            if (bIncIndex)
            {
                int nAmount = UICardMgr.singleton.illustratedItemAmount - 1;
                if (nAmount < 0)
                    nAmount = 0;
                if (nIndex < nAmount)
                {
                    nIndex++;
                }
                else
                    nIndex = 0;
            }

            UICardMgr.CItemData cidTemp = UICardMgr.singleton.getIllustratedItemByIndex(nIndex);
            this.CurSelectId = cidTemp.nId;

            _InitList();
        }

        void _OnCloseHangRoot(GameObject go)
        {
            mgoQualityRoot.SetActive(true);
            mgoHangHeroRoot.SetActive(false);
        }

        bool _OnRefreshPosition(SLG.EventArgs objArg)
        {
            RefreshList();

            CurSelectId = mnCurSelectHeroId;

            return true;
        }

        void RefreshList()
        {
            CardIllustratedListPanel cilp = (CardIllustratedListPanel)PanelManage.me.getPanel(PanelID.CardIllustratedListPanel);
            cilp.resetDataItem(mnCurSelectHeroId);
        }
    }

}


