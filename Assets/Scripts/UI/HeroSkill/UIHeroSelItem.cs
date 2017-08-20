using System;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
namespace UI
{
	class UIHeroSelItem
	{
        selHero m_info = null;
        GameObject m_root = null;
        UISprite m_cardBtn = null;
        UISprite m_starBtn = null;
        UISprite m_bgBtn = null;

        public UIHeroSelItem()
        {

        }

        public selHero hero
        {
            get { return m_info; }
            set { 
                    m_info = value;
                    if (m_info == null)
                    {
                        clear();
                        return;
                    }
                    if (m_cardBtn != null)
                    {
                        m_cardBtn.spriteName = m_info.strHeroIcon;
					m_cardBtn.enabled = true;
                    }
                    if (m_starBtn != null)
                    {
                        m_starBtn.spriteName = m_info.strStarIcon;
					m_starBtn.enabled = true;
                    }
                    if (m_bgBtn != null)
					m_bgBtn.enabled = false;
                }
        }
        public void setRoot(GameObject root)
        {
            m_root = root;
            if (root == null)
                return;
            m_cardBtn = UISoldierPanel.findChild<UISprite>(root.transform, "Ret");
            m_starBtn = UISoldierPanel.findChild<UISprite>(root.transform, "Ret,Star");
            m_bgBtn = UISoldierPanel.findChild<UISprite>(root.transform, "Background");
            clear();
        }

        public void show(bool bFlag)
        {
            if (m_root != null)
                m_root.active = bFlag;
        }

        public void clear()
        {
            m_info = null;
            if (m_cardBtn != null)
				m_cardBtn.enabled = false;
            if (m_starBtn != null)
				m_starBtn.enabled = false;
            if (m_bgBtn != null)
				m_bgBtn.enabled = true;
        }
	}
}
