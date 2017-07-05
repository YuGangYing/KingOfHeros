using UnityEngine;
using System.Collections;

namespace UI
{
    public class UITechAdvance
    {
        Transform m_root = null;
        bool m_bShow = false;
        ARMY_TYPE m_curArmyType = ARMY_TYPE.UNKOWN;

        UITechArmyInfo m_ArmyInfo = new UITechArmyInfo();
        UITechAdvanceUpdate m_ArmyUpdateInfo = new UITechAdvanceUpdate();

        public UITechAdvance()
        {
        }

        public void setRoot(Transform root)
        {
            if (root == null)
                return;
            m_root = root;
            UISoldierPanel.setBtnFunc(root, "ArmyBtnList,shield,btnIcon", onShiled);
            UISoldierPanel.setBtnFunc(root, "ArmyBtnList,pike,btnIcon", onPike);
            UISoldierPanel.setBtnFunc(root, "ArmyBtnList,archer,btnIcon", onArcher);
            UISoldierPanel.setBtnFunc(root, "ArmyBtnList,cavalry,btnIcon", onCavalry);
            UISoldierPanel.setBtnFunc(root, "ArmyBtnList,magic,btnIcon", onMagic);
            m_ArmyInfo.setRoot(UISoldierPanel.findChild(root, "ArmyPage,armyData"));
            m_ArmyUpdateInfo.setRoot(UISoldierPanel.findChild(root, "ArmyPage,advanceLayer"));
        }

        ARMY_TYPE curArmyType
        {
            get { return m_curArmyType; }
            set 
            {
                m_curArmyType = value;
                m_ArmyInfo.init(m_curArmyType);
                m_ArmyUpdateInfo.init(m_curArmyType);
            }
        }

        void onShiled(GameObject obj)
        {
            curArmyType = ARMY_TYPE.SHIELD;
        }

        void onPike(GameObject obj)
        {
            curArmyType = ARMY_TYPE.PIKEMAN;
        }

        void onArcher(GameObject obj)
        {
            curArmyType = ARMY_TYPE.ARCHER;
        }
        
        void onCavalry(GameObject obj)
        {
            curArmyType = ARMY_TYPE.CAVALRY;
        }

        void onMagic(GameObject obj)
        {
            curArmyType = ARMY_TYPE.MAGIC;
        }

        public void Show(bool bFlag)
        {
            curArmyType = ARMY_TYPE.PIKEMAN;

            m_bShow = bFlag;
            if (m_root != null)
                m_root.gameObject.SetActive(bFlag);
        }

        public void update()
        {
            if (!m_bShow)
                return;
            m_ArmyInfo.update();
            m_ArmyUpdateInfo.update();
        }
    }
}