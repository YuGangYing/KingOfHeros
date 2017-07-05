using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UI;
using DataMgr;

public class SettingPanel : PanelBase
{
    public const PanelID id = PanelID.SettingPanel;

    List<GameObject> listGoBtn = new List<GameObject>();
    List<UIToggle> listChkToggle = new List<UIToggle>();

    public SettingPanel()
    {

    }

    public override string GetResPath()
    {
        return "SettingPanel.prefab";
    }

    protected override void Initimp(List<GameObject> prefabs)
    {
        UIEventListener.Get(UICardMgr.findChild(Root, "Child,Panel,close")).onClick = _OnClose;
        UIEventListener.Get(UICardMgr.findChild(Root, "Child,Panel,forum")).onClick = _OnForumClick;
        UIEventListener.Get(UICardMgr.findChild(Root, "Child,Panel,help")).onClick = _OnHelpClick;
        UIEventListener.Get(UICardMgr.findChild(Root, "Child,Panel,quest")).onClick = _OnQuestClick;

        GameObject go = UICardMgr.findChild(Root, "Child,Panel,iconbtn00");
        //UIEventListener.Get(go).onClick = _OnMusicClick;
        UIToggle uitg = null;
        if(go != null)
        {
            uitg = go.GetComponent<UIToggle>();
            listGoBtn.Add(go);
            if (uitg != null)
            {
                listChkToggle.Add(uitg);
                EventDelegate.Add(uitg.onChange, _OnMusicChange);
            }
        }

        go = UICardMgr.findChild(Root, "Child,Panel,iconbtn01");
        //UIEventListener.Get(go).onClick = _OnSoundClick;
        if(go != null)
        {
            uitg = go.GetComponent<UIToggle>();
            listGoBtn.Add(go);
            if(uitg != null)
            {
                listChkToggle.Add(uitg);
                EventDelegate.Add(uitg.onChange, _OnSoundChange);
            }
        }


        go = UICardMgr.findChild(Root, "Child,Panel,iconbtn02");
        //UIEventListener.Get(go).onClick = _OnPurchaseClick;
        if(go != null)
        {
            uitg = go.GetComponent<UIToggle>();
            listGoBtn.Add(go);
            if(uitg != null)
            {
                listChkToggle.Add(uitg);
                EventDelegate.Add(uitg.onChange, _OnPurchaseChange);
            }
        }

        go = UICardMgr.findChild(Root, "Child,Panel,iconbtn03");
        //UIEventListener.Get(go).onClick = _OnPushMsgClick;
        if(go != null)
        {
            uitg = go.GetComponent<UIToggle>();
            listGoBtn.Add(go);
            if(uitg != null)
            {
                listChkToggle.Add(uitg);
                EventDelegate.Add(uitg.onChange, _OnPushMsgChange);
            }
        }

        go = UICardMgr.findChild(Root, "Child,Panel,maincityCamera");
        //UIEventListener.Get(go).onClick = _OnPushMsgClick;
        if (go != null)
        {
            uitg = go.GetComponent<UIToggle>();
            listGoBtn.Add(go);
            if (uitg != null)
            {
                listChkToggle.Add(uitg);
                EventDelegate.Add(uitg.onChange, _OnCityCameraChange);
            }
        }

        go = UICardMgr.findChild(Root, "Child,Panel,BattleCamera");
        //UIEventListener.Get(go).onClick = _OnPushMsgClick;
        if (go != null)
        {
            uitg = go.GetComponent<UIToggle>();
            listGoBtn.Add(go);
            if (uitg != null)
            {
                listChkToggle.Add(uitg);
                EventDelegate.Add(uitg.onChange, _OnBattleCameraChange);
            }
        }

        SetVisible(false);

        _FlushTableText();

        _ResetCheckBox();
    }

    void _FlushTableText()
    {
        UICardMgr.setLabelText(Root, "Child,Panel,name", 14026);

        UICardMgr.setLabelText(Root, "Child,Panel,quest,Label", 14033);
        UICardMgr.setLabelText(Root, "Child,Panel,forum,Label", 14034);
        UICardMgr.setLabelText(Root, "Child,Panel,help,Label", 14035);

        UICardMgr.setLabelText(Root, "Child,Panel,iconbtn00,Label", 14027);
        UICardMgr.setLabelText(Root, "Child,Panel,iconbtn01,Label", 14028);
        UICardMgr.setLabelText(Root, "Child,Panel,iconbtn02,Label", 14029);
        UICardMgr.setLabelText(Root, "Child,Panel,iconbtn03,Label", 14030);
        UICardMgr.setLabelText(Root, "Child,Panel,maincityCamera,Label", 14038);
        UICardMgr.setLabelText(Root, "Child,Panel,BattleCamera,Label", 14039);
    }

    public override PanelID GetPanelID()
    {
        return id;
    }

    protected void _OnClose(GameObject go)
    {
        SetVisible(false);
    }

    protected void _OnForumClick(GameObject go)
    {
        MessageBoxMgr.ShowMessageBox("提示", "该功能未开放", null, null);
        //SetVisible(false);
    }

    protected void _OnHelpClick(GameObject go)
    {
        MessageBoxMgr.ShowMessageBox("提示", "该功能未开放", null, null);
        //SetVisible(false);
    }

    protected void _OnQuestClick(GameObject go)
    {
        MessageBoxMgr.ShowMessageBox("提示", "该功能未开放", null, null);
        //SetVisible(false);
    }

    protected override void onShow()
    {
        base.onShow();
    }

    protected void _OnMusicClick(GameObject go)
    {
        _FlushColor(go);
    }

    protected void _OnSoundClick(GameObject go)
    {
        _FlushColor(go);
    }

    protected void _OnPurchaseClick(GameObject go)
    {
        _FlushColor(go);
    }

    protected void _OnPushMsgClick(GameObject go)
    {
        _FlushColor(go);
    }

    protected void _OnCityCameraClick(GameObject go)
    {
        _FlushColor(go);
    }

    protected void _OnBattleCameraClick(GameObject go)
    {
        _FlushColor(go);
    }

    void _FlushColor(GameObject go)
    {
        UISprite uis = null;

        int dwTmp = UnityEngine.Random.Range(1, 100);
        if (dwTmp > 50)
        {
            uis = UICardMgr.FindChild<UISprite>(go, "nullIcon");
            uis.color = new Color((114.0f / 255.0f), (111.0f / 255.0f), (199.0f / 255.0f));
        }
        else
        {
            uis = UICardMgr.FindChild<UISprite>(go, "nullIcon");
            uis.color = new Color((111.0f / 255.0f), (199.0f / 255.0f), (130.0f / 255.0f));
        }
    }

    void _OnMusicChange()
    {
        AudioCenter.me.isBgmEnable = listChkToggle[0].value;
        int n = 0;
        if (listChkToggle[0].value)
            n = 1;
        DataMgr.DataManager.getSyscfg().setValue(SYSTEM_CFG.MUSIC, n);
    }

    void _OnSoundChange()
    {
        AudioCenter.me.isSeEnable = listChkToggle[1].value;
        int n = 0;
        if (listChkToggle[1].value)
            n = 1;
        DataMgr.DataManager.getSyscfg().setValue(SYSTEM_CFG.SOUND, n);
    }

    void _OnPurchaseChange()
    {
        int n = 0;
        if (listChkToggle[2].value)
            n = 1;
        DataMgr.DataManager.getSyscfg().setValue(SYSTEM_CFG.PURCHASE, n);
    }

    void _OnPushMsgChange()
    {
        int n = 0;
        if (listChkToggle[3].value)
            n = 1;
        DataMgr.DataManager.getSyscfg().setValue(SYSTEM_CFG.PUSHMSG, n);
    }

    void _OnCityCameraChange()
    {
        int n = 0;
        if (listChkToggle[4].value)
            n = 1;
        DataMgr.DataManager.getSyscfg().setValue(SYSTEM_CFG.MAINCITYCAMERA, n);
    }

    void _OnBattleCameraChange()
    {
        int n = 0;
        if (listChkToggle[5].value)
            n = 1;
        DataMgr.DataManager.getSyscfg().setValue(SYSTEM_CFG.BATTLECAMERA, n);
    }

    void _ResetCheckBox()
    {
        bool b = true;
        for (SYSTEM_CFG i = SYSTEM_CFG.MUSIC; i < SYSTEM_CFG.MAX; i++)
        {
            b = DataMgr.DataManager.getSyscfg().getValue(i);
            listChkToggle[(int)i].value = b;
        }
    }
}
