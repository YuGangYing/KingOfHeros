using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Line End
namespace UI
{
    public class MessageBoxPanel : PanelBase
    {
        const PanelID id = PanelID.MessageBoxPannel;

        public override string GetResPath()
        {
            return "speedUpWindowUI_800x480";
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        private int m_btnCount = 0;
        private bool m_showCancel = true;
        private bool m_showConfirm = true;

        private UILabel m_labelTitleText;
        private UILabel m_labelContentText;
        private UIButton m_btnConfirm;
        private UIButton m_btnCancel;

        GameObject m_btn1;
        GameObject m_btn2;
        GameObject m_btn3;

        private UILabel m_labelCancelText;
        private UILabel m_labelConfirmText;

        protected override void Initimp(List<GameObject> prefabs)
        {
            m_labelContentText = PanelTools.FindChild(Root, "contentText").GetComponent<UILabel>();
            m_labelTitleText = PanelTools.FindChild(Root, "TitleText").GetComponent<UILabel>();
            m_btnConfirm = PanelTools.FindChild(Root, "confirm").GetComponent<UIButton>();
            m_btnCancel = PanelTools.FindChild(Root, "cancel").GetComponent<UIButton>();

            m_btn1 = PanelTools.FindChild(Root, "btn1");
            m_btn2 = PanelTools.FindChild(Root, "btn2");
            m_btn3 = PanelTools.FindChild(Root, "btn3");

            UIEventListener.Get(m_btnConfirm.gameObject).onClick = OnConfirm;
            UIEventListener.Get(m_btnCancel.gameObject).onClick = OnCancel;

            SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.ShowMessageBox, id, this.OnShowMessageBox);

            SetVisible(false);
        }

        protected override void onShow()
        {
            if (null != m_showMessageBoxEvent && m_showMessageBoxEvent.bHidePrevious)
            {
                base.onShow();
            }

            return;
        }

        protected override void onHide()
        {
            if (null != m_showMessageBoxEvent && m_showMessageBoxEvent.bHidePrevious)
            {
                base.onHide();
            }
            return;
        }

        protected void OnCancel(GameObject go)
        {
            if (null != m_showMessageBoxEvent && null != m_showMessageBoxEvent.eventCancelCallBack)
            {
                m_showMessageBoxEvent.eventCancelCallBack(m_showMessageBoxEvent.eventCancelArgs);
            }

            AudioCenter.me.play(AudioMgr.AudioName.UI_CANCEL);
            SetVisible(false);
        }

        private MessageBoxMgr.ShowMessageBoxEvent m_showMessageBoxEvent;
        protected void OnConfirm(GameObject go)
        {
            if (null != m_showMessageBoxEvent && null != m_showMessageBoxEvent.eventConfirmCallBack)
            {
                m_showMessageBoxEvent.eventConfirmCallBack(m_showMessageBoxEvent.eventConfirmArgs);
            }

            AudioCenter.me.play(AudioMgr.AudioName.UI_OK);
            SetVisible(false);
        }

        protected bool OnShowMessageBox(SLG.EventArgs obj)
        {
            m_showMessageBoxEvent = null;
            m_showMessageBoxEvent = (MessageBoxMgr.ShowMessageBoxEvent)obj.m_obj;

            if (null != m_showMessageBoxEvent)
            {
                m_labelTitleText.text = m_showMessageBoxEvent.strTitleText;
                m_labelContentText.text = m_showMessageBoxEvent.strContentText;
                m_labelContentText.color = m_showMessageBoxEvent.ContentTextClolor;

                if (m_labelCancelText!=null)
                    m_labelCancelText.text = m_showMessageBoxEvent.strCancelText;
                if (m_labelConfirmText != null)
                    m_labelConfirmText.text = m_showMessageBoxEvent.strConfirmText;

                this.m_btnCount = 0;

                m_showCancel = checkButton(m_btnCancel, MESSBOX_FLAG.MB_CANCEL);
                m_showConfirm = checkButton(m_btnConfirm, MESSBOX_FLAG.MB_CONFIRM);
                showBtn();
                SetVisible(true);
            }

            return true;
        }

        bool checkButton(UIButton button,MESSBOX_FLAG flag)
        {
            if (button == null)
                return false;
            if ((m_showMessageBoxEvent.btnFlag & flag) == flag)
            {
                button.gameObject.SetActive(true);
                m_btnCount++;
                return true;
            }
            else
            {
                button.gameObject.SetActive(false);
                return false;
            }
        }

        void showBtn()
        {
            if (m_btnCount == 2)
            {
                this.m_btnCancel.gameObject.transform.position = m_btn1.transform.position;
                this.m_btnConfirm.gameObject.transform.position = m_btn2.transform.position;
            }
            else if (m_btnCount == 1)
            {
                if(m_showCancel)
                    this.m_btnCancel.gameObject.transform.position = m_btn3.transform.position;
                else if(m_showConfirm)
                    this.m_btnConfirm.gameObject.transform.position = m_btn3.transform.position;
            }
        }
    }
}
