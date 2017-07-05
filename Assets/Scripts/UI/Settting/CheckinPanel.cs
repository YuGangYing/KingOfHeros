using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UI;
using DataMgr;
using Packet;

public class CheckinPanel : PanelBase
{
    public const PanelID id = PanelID.CheckInPanel;
    List<GameObject> listIcon = new List<GameObject>();


    public CheckinPanel()
    {

    }

    public override string GetResPath()
    {
        return "SettingPanel.prefab";
    }



    protected override void Initimp(List<GameObject> prefabs)
    {
        UIEventListener.Get(UICardMgr.findChild(Root, "Child,Panel,close")).onClick = _OnCloseBtn;
        UIEventListener.Get(UICardMgr.findChild(Root, "Child,Panel,confirm")).onClick = _OnRewardBtn;

        //m_achievenmentListGrid = PanelTools.FindChild(Root, "AchievenmentListGrid").AddComponent<UIGrid>();
        //m_achievenmentListGrid.arrangement = UIGrid.Arrangement.Vertical;
        //Bounds itemBounds = NGUIMath.CalculateRelativeWidgetBounds(m_achievenmentListGridItemPrefab.transform, m_achievenmentListGridItemPrefab.transform, true);
        //m_achievenmentListGrid.cellHeight = itemBounds.size.y;
        //m_achievenmentListGrid.cellWidth = itemBounds.size.x;

        string str = DataMgr.DataManager.getLanguageMgr().getString(14206);
        string str1 = DataMgr.DataManager.getLanguageMgr().getString(14207);
        string str2 = DataMgr.DataManager.getLanguageMgr().getString(14208);

        // 一周七天
        string strTreeName = "";
        string strLabel = "";
        for (int i = 0; i < 7; i++)
        {
            string strbtn = "iconbtn0" + i.ToString();
            strTreeName = "Child,Panel," + strbtn;

            DataMgr.ConfigRow cr = null;
            GameObject go = UICardMgr.findChild(Root, strTreeName);
            if (go != null)
            {
                listIcon.Add(go);
            }

            strLabel = strTreeName + ",Label";
            UILabel lb = UICardMgr.FindChild<UILabel>(Root, strLabel);
            if (lb != null)
            {
                int nWeek = i + 1;
                if (i == 6)
                    nWeek = 0;

                if (CCheckInItemAttribute.getItemByWeekDay(nWeek, ref cr))
                {
                    int nCoin = cr.getIntValue(enCFG_CSV_CHECKIN_ITEM.MONEY);
                    int nStone = cr.getIntValue(enCFG_CSV_CHECKIN_ITEM.STONE);
                    int nDiamond = cr.getIntValue(enCFG_CSV_CHECKIN_ITEM.DIAMOND);
                    int nId = cr.getIntValue(enCFG_CSV_CHECKIN_ITEM.ITEM01_TYPEID);
                    int nIdCnt = cr.getIntValue(enCFG_CSV_CHECKIN_ITEM.ITEM01_AMOUNT);
                    int nIdEx = cr.getIntValue(enCFG_CSV_CHECKIN_ITEM.ITEM02_TYPEID);
                    int nIdExCnt = cr.getIntValue(enCFG_CSV_CHECKIN_ITEM.ITEM02_AMOUNT);

                    if(nId != 0 && nIdEx != 0)
                        lb.text = string.Format(str2, nCoin, nStone, nDiamond, nId, nIdCnt, nIdEx, nIdExCnt);
                    else if(nId != 0)
                        lb.text = string.Format(str1, nCoin, nStone, nDiamond, nId, nIdCnt);
                    else
                        lb.text = string.Format(str, nCoin, nStone, nDiamond);
                }
            }
        }
        
        SetVisible(false);

        _FlushTableText();
    }

    void _FlushTableText()
    {
        UICardMgr.setLabelText(Root, "Child,Panel,name", 14025);
        UICardMgr.setLabelText(Root, "Child,Panel,confirm,Label", 14024);
    }

    public override PanelID GetPanelID()
    {
        return id;
    }

    void _OnCloseBtn(GameObject go)
    {
        SetVisible(false);
    }

    void _OnRewardBtn(GameObject go)
    {
        _OnCloseBtn(null);
    }

    protected override void onShow()
    {
        base.onShow();
        //DataManager.getAchievementData().SendQueryAchievenment();
    }

    protected override void onHide()
    {
        base.onHide();
    }

    protected override void ReleaseImp()
    {

    }


    void _ReFlushItem()
    {
        UISprite uis = null;

        DataMgr.HeroData.CCheckInData clscd = DataMgr.DataManager.getHeroData().CheckInInfo;
        clscd._isHave = false;
        for (int i = 0; i < 7; i++)
        {
            uint dwBit = clscd._stData.unDayLogined >> i;
            uint dwTmp = dwBit & 0x01;

            if (dwTmp == 1)
            {
                uis = UICardMgr.FindChild<UISprite>(listIcon[i], "nullIcon");
                uis.color = new Color((114.0f / 255.0f), (111.0f / 255.0f), (199.0f / 255.0f));              
            }

            if (i == clscd._stData.unToday)
            {
                uis = UICardMgr.FindChild<UISprite>(listIcon[i], "nullIcon");
                uis.color = new Color((111.0f / 255.0f), (199.0f / 255.0f), (130.0f / 255.0f));   
            }
        }
    }

    public void run()
    {
        if (DataMgr.DataManager.getHeroData() == null)
            return;
        if (!DataMgr.DataManager.getHeroData().CheckInInfo._isHave)
            return;

        _ReFlushItem();
        SetVisible(true);
    }

    public override void update()
    {
        this.run();
    }
}
