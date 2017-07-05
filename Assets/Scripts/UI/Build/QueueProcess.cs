using UnityEngine;
using System.Collections;
using DataMgr;

public delegate void UpdateBuildInfo(float fTime);
//Line End
public class QueueProcess : MonoBehaviour {

    UpdateBuildInfo m_callBack = null;
	DataMgr.QueueData.QUEUE_TYPE m_queueType;
    public float m_fDueTime;
    uint m_deadTime;

    bool bUpdate = false;

	// Update is called once per frame
    void Update()
    {
        if (!bUpdate) 
        {
            return;
        }

        m_fDueTime = (float)DataManager.getTimeServer().EstimateServerTime((long)m_deadTime);

        if (m_fDueTime < 0)
        {
            bUpdate = false;
            m_deadTime = 0;
            m_fDueTime = 0;
        }

        if (m_callBack != null)
        {
            m_callBack(m_fDueTime);
        }
    }

	public void SetQueueInfo(DataMgr.QueueData.QUEUE_TYPE queueType, UpdateBuildInfo callBack)
    {
        m_queueType = queueType;
        m_callBack = callBack;

		m_deadTime = DataManager.getQueueData().GetDueTime(queueType);

        if (m_deadTime > 0)
        {
            bUpdate = true;
        }
    }

    public void UpdateQueueTime()
    {
		m_deadTime = DataManager.getQueueData().GetDueTime(m_queueType);

        if (m_deadTime > 0)
        {
            bUpdate = true;
        }
    }
}
