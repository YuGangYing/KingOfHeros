using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UI
{
    public class CardStarUpdatePanel : PanelBase
    {
        public const PanelID id = PanelID.CardStarUpdatePanel;

        public CardStarUpdatePanel()
        {

        }

        public override string GetResPath()
        {
            return "StarUpdateWin.prefab";
        }

        protected override void Initimp(List<GameObject> prefabs)
        {
            //Root.transform.localScale = new UnityEngine.Vector3(4.0f, 4.0f, 4.0f);

            UIAnchor uia = Root.GetComponent<UIAnchor>();
            if (uia != null)
            {
                uia.uiCamera = UICardMgr.getCamera().GetComponent<Camera>();
            }

            m_icon = PanelTools.Find<UISprite>(Root, "StarlRoot/IntroductionRoot/icon");
            m_text = PanelTools.Find<UILabel>(Root, "StarlRoot/IntroductionRoot/DescribeRoot/LabelContext");
            m_title = PanelTools.Find<UILabel>(Root, "StarlRoot/IntroductionRoot/DescribeRoot/LabelTitle");

            UIEventListener.Get(PanelTools.Find(Root, "StarlRoot/btnRoot/backBtn")).onClick = OnBackClick;
            UIEventListener.Get(PanelTools.Find(Root, "StarlRoot/btnRoot/updateBtn")).onClick = OnUpdateClick;

            SetVisible(false);
        }

        public class Data
        {
            public int id; // 英雄的ID
            public int index;   // 列表中的索引
            public string icon; // 英雄图标
            public string text; // 英雄文本
            public string title; // 英雄标题
            public UIAtlas uiatIcon; // 英雄图标
        }

        Data m_data;
        public void SetData(Data data)
        {
            m_data = data;
            UpdateData();
        }

        void UpdateData()
        {
            if (m_data == null)
            {
                m_icon.atlas = null;
                m_icon.spriteName = "";
            }
            else
            {
                //SetSpriteIcon(m_icon, m_data.icon);
                m_icon.atlas = m_data.uiatIcon;
                m_icon.spriteName = m_data.icon;
                m_text.text = m_data.text;
                m_title.text = m_data.title;
            }
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        protected override void ReleaseImp()
        {
            
        }

        void OnBackClick(GameObject go)
        {
            this.SetVisible(false);

            CardIllustratedDetailPanel pb = (CardIllustratedDetailPanel)PanelManage.me.getPanel(PanelID.CardIllustratedDetailPanel);
            if (pb != null)
            {
                pb.SetVisible(true);
            }
        }

        void OnUpdateClick(GameObject go)
        {
            //this.SetVisible(false);
        }

        private UISprite m_icon; // 图标
        private UILabel m_text; // 文本
        private UILabel m_title; // 标题
    }
}


