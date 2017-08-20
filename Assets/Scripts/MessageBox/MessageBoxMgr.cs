using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataMgr;
//Line End
public enum MESSBOX_FLAG
{
    MB_CANCEL = 0x01,
    MB_CONFIRM = 0x10
}

public class MessageBoxMgr
{
    public class ShowMessageBoxEvent
    {
        public string strTitleText;
        public string strContentText;
        public string strCancelText;
        public string strConfirmText;

        public SLG.Subscriber eventConfirmCallBack;
        public SLG.Subscriber eventCancelCallBack;

        public SLG.EventArgs eventCancelArgs;
        public SLG.EventArgs eventConfirmArgs;

        public Color ContentTextClolor;

        public MESSBOX_FLAG btnFlag;
        public bool bHidePrevious;

        public ShowMessageBoxEvent()
        {
            btnFlag = MESSBOX_FLAG.MB_CONFIRM | MESSBOX_FLAG.MB_CANCEL;
            strCancelText = DataManager.getLanguageMgr().getString(17597);
            strConfirmText = DataManager.getLanguageMgr().getString(17598);
            bHidePrevious = true;
            ContentTextClolor = Color.white;
        }
    }

    static public void ShowConfirm(string strTitleText, string strContentText, SLG.Subscriber eventCallBack,SLG.EventArgs eventArgs=null)
    {
        ShowMessageBoxEvent showMessageBoxEvent = new ShowMessageBoxEvent();
        showMessageBoxEvent.strTitleText = strTitleText;
        showMessageBoxEvent.strContentText = strContentText;
        showMessageBoxEvent.bHidePrevious = false;
        showMessageBoxEvent.btnFlag = MESSBOX_FLAG.MB_CONFIRM;
        showMessageBoxEvent.ContentTextClolor = Color.red;
        showMessageBoxEvent.eventConfirmCallBack = eventCallBack;
        showMessageBoxEvent.eventConfirmArgs = eventArgs;
        ShowMessageBox(showMessageBoxEvent);
    }
    //只显示确认框
    static public void ShowConfirm(string strTitleText, string strContentText)
    {
        ShowMessageBoxEvent showMessageBoxEvent = new ShowMessageBoxEvent();
        showMessageBoxEvent.strTitleText = strTitleText;
        showMessageBoxEvent.strContentText = strContentText;
        showMessageBoxEvent.bHidePrevious = false;
        showMessageBoxEvent.btnFlag = MESSBOX_FLAG.MB_CONFIRM;
        showMessageBoxEvent.ContentTextClolor = Color.red;
        ShowMessageBox(showMessageBoxEvent);
    }

    static public void ShowMessageBox(string strTitleText, string strContentText, SLG.Subscriber eventCallBack, SLG.EventArgs eventArgs, bool bHidePrevious = true)
    {
        ShowMessageBoxEvent showMessageBoxEvent = new ShowMessageBoxEvent();
        showMessageBoxEvent.strTitleText = strTitleText;
        showMessageBoxEvent.strContentText = strContentText;

        showMessageBoxEvent.eventConfirmCallBack = eventCallBack;
        showMessageBoxEvent.eventConfirmArgs = eventArgs;
        
        showMessageBoxEvent.bHidePrevious = bHidePrevious;
        showMessageBoxEvent.btnFlag = MESSBOX_FLAG.MB_CONFIRM | MESSBOX_FLAG.MB_CANCEL;
        ShowMessageBox(showMessageBoxEvent);
    }

    static public void ShowMessageBox(ShowMessageBoxEvent showMessageBoxEvent)
    {
        SLG.GlobalEventSet.FireEvent(SLG.eEventType.ShowMessageBox, UI.PanelID.MessageBoxPannel, new SLG.EventArgs(showMessageBoxEvent));
    }
}
