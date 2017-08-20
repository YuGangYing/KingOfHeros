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
#pragma warning disable 0649
#pragma warning disable 0472
namespace UI
{
    public delegate void onUpdate();
    public delegate void addItem(selHero item, bool bFlag);

    public class UIChooseHeroItem
    {
        bool mbSelect = false;
        UISprite mIconSprite = null;
        UISprite mIconStar = null;
        GameObject mgoBg = null;
        GameObject m_root = null;
        onUpdate m_updateFunc = null;

        selHero m_info = new selHero();
        addItem m_addItemFunc = null;

        private UIChooseHeroItem()
        {
        }

        public UIChooseHeroItem(GameObject root)
        {
            m_root = root;
        }

        void _OnSelectClick(GameObject go)
        {
            Debug.Log("_OnSelectClick");
            mbSelect = !mbSelect;
            setSel();
            if (m_addItemFunc != null)
                m_addItemFunc(m_info,mbSelect);
        }

        public selHero heroInfo
        {
            get { return m_info; }
        }

        public onUpdate updateFunc
        {
            set { m_updateFunc = value; }
        }

        public addItem addItemFunc
        {
            set { m_addItemFunc = value; }
        }

        public void setSelHero()
        {
            mbSelect = true;
            setSel();
        }

        void setSel()
        {
            if (mgoBg != null)
                mgoBg.SetActive(mbSelect);
            if (m_updateFunc != null)
                m_updateFunc();
        }

        public void remove()
        {
            if (m_root != null)
                GameObject.Destroy(m_root);
        }

        public void reset()
        {
            mgoBg.SetActive(false);
        }

        public bool isSelect
        {
            get { return mbSelect; }
            set { mbSelect = value; }
        }

        public bool init(int nHeroId)
        {
            if (m_root == null)
                return false;

            m_info.nHeroid = nHeroId;
            UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemById(nHeroId);
            ConfigRow cd;
            bool bRet = CHerroTalbeAttribute.getHeroBaseDetail(cid.nTypeId, out cd);
            if (!bRet)
                return false;

            string strName = cd.getStringValue(enCVS_HERO_BASE_ATTRIBUTE.ICON_SPRITE_NAME);
            int nTemp = cd.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.QUALITY);

            //头像
            GameObject tf = UISoldierPanel.findChild(m_root, "HeadRoot/icon");
            if (tf != null)
            {
                mIconSprite = tf.GetComponent<UISprite>();
                if (mIconSprite != null)
                {
                    m_info.strHeroIcon = UICardLogic.getSpriteNameSmall(cid.nTypeId);// mszCardNameSmall[nTemp - 1];
                    mIconSprite.spriteName = m_info.strHeroIcon;                    
                }
                GameObject tfTemp = UISoldierPanel.findChild(tf, "Star");
                if (tfTemp != null)
                {
                    m_info.strStarIcon = UICardLogic.getStarNameSmall(cid.nTypeId);// mszCardNameStarSmall[nTemp - 1];
                    mIconStar = tfTemp.GetComponent<UISprite>();
                    if (mIconStar!=null)
                        mIconStar.spriteName = m_info.strStarIcon;
                }
            }

            //参数
            mgoBg = UISoldierPanel.findChild(m_root, "HeadRoot/iconChooseBg");
            if (mgoBg!= null)
                mgoBg.SetActive(false);

            int nAtc = cd.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.BASE_ATTACK);
            int nHP = cd.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.BASE_HP);
            int nDef = cd.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.BASE_DEF);
            int nLead = cd.getIntValue(enCVS_HERO_BASE_ATTRIBUTE.BASE_LEADER);

            Packet.HERO_INFO hi = new Packet.HERO_INFO();
            DataManager.getHeroData().getItemById((uint)nHeroId,ref hi);
            int lLv = hi.usLevel;
            int lStar = hi.u8Star;

            float fFill = lStar * HeroData.FillValue;

            m_info.nStarLevel = (int)lStar;
            if (mIconStar != null)
                mIconStar.fillAmount = fFill;

            ConfigRow crcr = null;
            CHerroTalbeAttribute.getHeroStar(cid.nTypeId, lLv, out crcr);

            // cur
            int nFactor = crcr.getIntValue(enCVS_HERO_STAR_ATTRIBUTE.FACTOR_ATTACK);
            int nCurAtc = CardQualityUpdatePanel.GetValue((float)nAtc, (int)lStar, (int)lLv, nFactor);

            nFactor = crcr.getIntValue(enCVS_HERO_STAR_ATTRIBUTE.FACTOR_HP);
            int nCurHP = CardQualityUpdatePanel.GetValue((float)nHP, (int)lStar, (int)lLv, nFactor);

            nFactor = crcr.getIntValue(enCVS_HERO_STAR_ATTRIBUTE.FACTOR_DEF);
            int nCurDef = CardQualityUpdatePanel.GetValue((float)nDef, (int)lStar, (int)lLv, nFactor);

            nFactor = crcr.getIntValue(enCVS_HERO_STAR_ATTRIBUTE.FACTOR_LEADER);
            int nCurLead = CardQualityUpdatePanel.GetValue((float)nLead, (int)lStar, (int)lLv, nFactor);

            UISoldierPanel.setLabelText(m_root.transform, "atcNo", nCurAtc.ToString());
            UISoldierPanel.setLabelText(m_root.transform, "defNo", nCurDef.ToString());
            UISoldierPanel.setLabelText(m_root.transform, "hpNo", nCurHP.ToString());
            UISoldierPanel.setLabelText(m_root.transform, "lvNo", lLv.ToString());
            UISoldierPanel.setLabelText(m_root.transform, "labelName", cd.getStringValue(enCVS_HERO_BASE_ATTRIBUTE.NAME_ID));
            UISoldierPanel.setLabelText(m_root.transform, "powerNo", nCurLead.ToString());

            UIEventListener.Get(m_root).onClick = _OnSelectClick;

            return true;
        }
        
    }
}