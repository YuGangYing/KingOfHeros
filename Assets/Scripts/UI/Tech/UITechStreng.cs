using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
	public class UITechStreng
	{
        Transform m_root = null;
        bool m_bShow = false;
        UITechArmyInfo m_armyInfo = new UITechArmyInfo();

        public void setRoot(Transform root)
        {
            m_root = root;
        }

        public void Show(bool bFlag)
        {
            m_bShow = bFlag;
            if (m_root != null)
                m_root.gameObject.SetActive(bFlag);
        }

        public void update()
        {
            if (!m_bShow)
                return;
        }
	}
}
