using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
namespace UI
{
    public class UITechPanel : PanelBase
    {
        const PanelID id = PanelID.TechPanel;

        UITechAdvance m_techMain = new UITechAdvance();
        UITechStreng m_techStreng = new UITechStreng();
        UIButton m_closeBtn = null;

        protected override void Initimp(List<GameObject> prefabs)
        {
            this.SetVisible(false);
            UISoldierPanel.setBtnFunc(Root.transform, "close", onClose);
            m_techMain.setRoot(UISoldierPanel.findChild(Root.transform, "ArmyPage"));
            m_techStreng.setRoot(UISoldierPanel.findChild(Root.transform, "StrengPage"));
            UISoldierPanel.setBtnFunc(Root.transform, "TitleBtn/advanceBtn", onAdvanecePage);
            UISoldierPanel.setBtnFunc(Root.transform, "TitleBtn/strengBtn", onStrengPage);
            onAdvanecePage(null);
        }

        protected override void onShow()
        {
            m_techMain.Show(true);
            m_techStreng.Show(false);
            base.onShow();
        }

        protected override void onHide()
        {
            base.onHide();
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        public override string GetResPath()
        {
            return "Tech.prefab";
        }

        void onAdvanecePage(GameObject obj)
        {
            m_techMain.Show(true);
            //m_techStreng.Show(false);
        }

        void onStrengPage(GameObject obj)
        {
            //m_techMain.Show(false);
            //m_techStreng.Show(true);
        }

        void onClose(GameObject obj)
        {
            this.SetVisible(false);
        }

        static public  string getTimeFormat(int time)
        {
            int nHour = (int)(time / 3600);
            int temp = (int)(time % 3600);
            int nMinute = (int)(temp / 60);
            int nHSec = (int)(temp % 60);
            DateTime date = new DateTime(2010, 1, 1, nHour, nMinute, nHSec);
            return date.ToString("HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
        }
        
        public override void update()
        {
            if (IsVisible())
            {
                m_techMain.update();
                m_techStreng.update();
            }
        }
    }
}
