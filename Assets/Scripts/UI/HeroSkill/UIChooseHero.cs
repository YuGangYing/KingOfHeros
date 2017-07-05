using System;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
namespace UI
{
    public class selHero
    {
        public int nHeroid;
        public int nStarLevel;
        public string strHeroIcon;
        public string strStarIcon;
    }

    public delegate void selFunc(int nCount, selHero[] heroList);

    //选择英雄
	class UIChooseHero
	{
        int m_nCurHeroId = 0;
        int m_nSelectCount = 0;

        Dictionary<int, selHero> m_selList = new Dictionary<int,selHero>();

        UIButton m_btnClose = null;
        UIButton m_btnConfirm = null;
        selFunc m_onSelfunc = null;

        GameObject m_root = null;
        GameObject m_item = null;
        GameObject m_ItemRoot = null;
        GameObject m_parent = null;
        GameObject m_upgradeProgress = null;

        List<UIChooseHeroItem> m_heroList = new List<UIChooseHeroItem>();

        public UIChooseHero()
        {

        }

        public void addSelHero(selHero hero)
        {
            if (hero == null)
                return;
            m_selList.Add(hero.nHeroid,hero);
        }

        public int heroId
        {
            set { m_nCurHeroId = value; }
        }

        public int selectNum
        {
            set { m_nSelectCount = value; }
        }

        public void setRoot(GameObject root)
        {
            m_root = root;
            if (root == null)
                return;
            m_btnClose = UISoldierPanel.setBtnFunc(root.transform, "TitleRoot,close", onClose);
            m_btnConfirm = UISoldierPanel.setBtnFunc(root.transform, "TitleRoot,confirmBtn", onConfirm);
            m_ItemRoot = UISoldierPanel.findChild(root, "DetailRoot,HangArea");
            m_item = UISoldierPanel.findChild(root, "DetailRoot,HangArea,HeroItem");

            m_parent = root.transform.parent.gameObject;
            m_upgradeProgress = UISoldierPanel.findChild(m_parent, "HeroSkill_Upgrade,upgradeProgress");
        }

        public void onClose(GameObject obj)
        {
            m_upgradeProgress.SetActive(true);
            show(false);
            clear();
            m_selList.Clear();
            m_onSelfunc(0,null);
        }

        public void update()
        {
            if (m_btnConfirm == null)
                return;
            int nCount = 0;
            foreach (UIChooseHeroItem item in m_heroList)
            {
                if (item.isSelect)
                    nCount++;
            }
            bool bShow = false;
            if (nCount == 0)
                bShow = false;
            else if (nCount > m_nSelectCount)
                bShow = false;
            else
                bShow = true;

            m_btnConfirm.enabled = bShow;
        }

        public void addItem(selHero item, bool bFlag)
        {
            if (bFlag)
                m_selList[item.nHeroid] = item;
            else
                m_selList.Remove(item.nHeroid);
        }

        public void onConfirm(GameObject obj)
        {
            if (m_selList.Count != m_nSelectCount)
                return;
            
            m_upgradeProgress.SetActive(true);
            show(false);
            clear();

            if (m_onSelfunc != null)
            {
                List<selHero> heroList = new List<selHero>();
                foreach (KeyValuePair<int, selHero> id in m_selList)
                {
                    Debug.Log("confirm:" + id.Value.nHeroid);
                    heroList.Add(id.Value);
                }
                m_onSelfunc(m_selList.Count, heroList.ToArray());
            }
            m_selList.Clear();
        }

        public void show(bool bFlag)
        {
            if (m_root != null)
                m_root.SetActive(bFlag);
        }

        //初始化
        public void init(int nHeroid,selFunc onSelfunc)
        {
            if (m_onSelfunc == null)
                m_onSelfunc = onSelfunc;

            initHeroList();
            update();
        }

        public void clear()
        {
            foreach (UIChooseHeroItem item in m_heroList)
                item.remove();

            if (m_btnConfirm != null)
                m_btnConfirm.enabled = false;
            m_heroList.Clear();
        }

        private void initHeroList()
        {
            if (m_ItemRoot == null)
                return;

            clear();

            if (m_item == null)
                return;
            int nIndex = 0, nx = 190, ny = -30, nOffsetY = 115;
            int nAmount = UICardMgr.singleton.illustratedItemAmount;
            
            for (int i = 0; i < nAmount; i++)
            {
                UICardMgr.CItemData cid = UICardMgr.singleton.getIllustratedItemByIndex(i);
                if(cid == null)
                    continue;

                if (cid.nId == m_nCurHeroId)
                    continue;

                GameObject itemObj = NGUITools.AddChild(m_ItemRoot, m_item);
                UIChooseHeroItem item = new UIChooseHeroItem(itemObj);
                item.updateFunc = this.update;
                item.addItemFunc = this.addItem;
                item.init(cid.nId);
                selHero hero;
                if (m_selList.TryGetValue(item.heroInfo.nHeroid, out hero))
                {
                    item.setSelHero();
                }
                m_heroList.Add(item);

                itemObj.active = true;
                itemObj.transform.localPosition = new UnityEngine.Vector3((float)nx, (float)(ny - nIndex * nOffsetY), 0.0f);
                nIndex++;
            }
        }
	}
}
