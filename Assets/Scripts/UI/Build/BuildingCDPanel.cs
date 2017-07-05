using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
using DataMgr;

namespace UI
{
    public class BuildingCDPanel : MonoBehaviour
    {
        public UISprite m_timeBar;
        public UILabel m_timeLabel;

        float m_fTotalTime;
        float m_fDueTime;
        uint m_deadTime;

        bool bUpdate = false;

        // Use this for initialization
        void Start()
        {
            if (m_timeBar == null)
            {
                m_timeBar = this.GetComponentInChildren<UISprite>();
            }

            if (m_timeLabel == null)
            {
                m_timeLabel = this.GetComponentInChildren<UILabel>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!bUpdate)
            {
                return;
            }

            m_timeBar.gameObject.SetActive(true);
            m_timeLabel.gameObject.SetActive(true);
			m_deadTime = DataManager.getQueueData().GetDueTime(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD);
            //m_fDueTime = (float)GameInstance.Game.Sgt.EstimateServerTime((long)m_deadTime);
			m_fDueTime = (float)DataManager.getTimeServer().EstimateServerTime((long)m_deadTime);

            if (m_fDueTime < 0)
            {
                bUpdate = false;
                m_deadTime = 0;
                m_fDueTime = 0;
                this.gameObject.SetActive(false);
            }

            if (m_fDueTime > 59)
            {
                float m = m_fDueTime / 60;
                float s = m_fDueTime % 60;
                m_timeLabel.text = ((uint)m).ToString() + "m" + ((uint)s + 1).ToString() + "s";
            }
            else
            {
                m_timeLabel.text = ((uint)m_fDueTime + 1).ToString() + "s";
            }

            m_timeBar.fillAmount = 1 - m_fDueTime / m_fTotalTime;
        }

        public void SetInfo(float fTotalTime)
        {
            m_fTotalTime = fTotalTime;
			m_deadTime = DataManager.getQueueData().GetDueTime(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD);

            if (m_deadTime > 0)
            {
                bUpdate = true;
                this.gameObject.SetActive(true);
            }
        }
    }
}
