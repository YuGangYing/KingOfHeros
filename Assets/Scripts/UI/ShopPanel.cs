using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataMgr;
//Line End
namespace UI
{
    public class ShopPanel : PanelBase
    {
        const PanelID id = PanelID.ShopPanel;

        public override string GetResPath()
        {
            return "MallUI.prefab";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        public class ItemData
        {
            public int id; // 商品类型
            public string name;
            public string icon;
        }

        UILabel shopLabel;
        UILabel buildLabel;


        protected override void Initimp(List<GameObject> prefabs)
        {
            UIEventListener.Get(PanelTools.FindChild(Root, "close")).onClick = OnClose;

            // 暂时只有一个
            UIEventListener.Get(PanelTools.FindChild(Root, "Icon")).onClick = OnItemClick;

            shopLabel = PanelTools.FindChild(Root, "shopLabel").GetComponent<UILabel>();
            buildLabel = PanelTools.FindChild(Root, "buildLabel").GetComponent<UILabel>();

            shopLabel.text = DataManager.getLanguageMgr().getString(shopLabel.text);
            buildLabel.text = DataManager.getLanguageMgr().getString(buildLabel.text);

            SetVisible(false);
        }

        protected void OnClose(GameObject go)
        {
            SetVisible(false);
        }

        void OnItemClick(GameObject go)
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.BuildMallPanel);
            if (panelBase != null)
            {
                panelBase.ToggleVisible();
                if (panelBase.IsVisible())
					DataManager.getBuildData().SetShowToPanel();
            }
        }
    }
}
