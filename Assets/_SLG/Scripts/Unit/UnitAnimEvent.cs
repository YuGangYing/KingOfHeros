using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EventInfo
{
    public string m_ClipName; 

    public string CallBackFuncName;

    public AnimEventCall m_AnimEventCall;

    public float m_fTime;

    public float m_flengthtime;

    public float m_fBiasTime;
} 

public class UnitAnimEvent : MonoBehaviour 
{ 
    private UnitAnim m_Anim; 

    public float m_fEventTime = 0;

    public bool m_bLoop = false;

    //note:The Upadate function speed will be change, When AnimationClip speed was changed
    public float m_fEventSpeed ; 
    
    public List<EventInfo> EventInfomation = new List<EventInfo>();  

    private List<EventInfo> m_EventList = new List<EventInfo>();

    private float m_fCurClipLength;

    void Awake()
    { 
        if (EventInfomation != null)
        {
            for (int i = 0; i < EventInfomation.Count; ++i )
            {
                if (EventInfomation[i] != null)
                {
                    EventInfomation[i].m_AnimEventCall = Common.AddObjComponent<AnimEventCall>(gameObject);
                }
            } 
        } 
    } 
    
    void Start()
    {
        m_Anim = GetComponent<UnitAnim>();

        if (m_Anim == null)
        { 
            return;
        }

        m_EventList.Clear(); 
    } 
  
    void LateUpdate()
    {
        if (m_Anim == null)
        {
            return;
        }
       
        if (m_EventList.Count > 0)
        {
            m_fEventTime += Time.deltaTime * m_fEventSpeed;

            if (m_EventList[0] != null)
            {
                if (m_fEventTime >= m_EventList[0].m_fTime)
                {
                    //Debug.Log(gameObject.name + "Attack!!!");   

                    if (m_EventList[0].m_AnimEventCall != null)
                    {
                        m_EventList[0].m_AnimEventCall.OnAnimAttack();
                    }
                           
                    m_EventList.RemoveAt(0);
                }
            } 
        }

        if (m_bLoop)
        {
            if (m_fEventTime >= m_fCurClipLength)
            {
                m_fEventTime = 0;

                SetAnimEvent(m_Anim.m_CurClipName, true, m_fEventSpeed);

                m_Anim.SetAnimationTime(m_Anim.m_CurClipName, 0);
            }
        } 
    }

    private void GetSameEventInfo(string _name,ref List<EventInfo> _list)
    { 
        if (EventInfomation == null)
        {
            Logger.LogError("EventInfomation is null");
        }

        for (int i = 0; i < EventInfomation.Count; ++i)
        {
            if (EventInfomation[i].m_ClipName == _name)
            {
                _list.Add(EventInfomation[i]);
            }
        } 
    }

    List<EventInfo> tmp_list = new List<EventInfo>();
    public void SetAnimEvent(string _name, bool _loop, float _eventspeed = 1.0f)
    { 
        //if (m_EventList == null)
        //{
        //    Logger.LogError("m_EventList is null");

        //    return;
        //}

        m_EventList.Clear();

        m_bLoop = _loop;

        if (_eventspeed <=0)
        {
            Logger.LogError("动作事件更新速度异常");

            _eventspeed = 1;
        }

        m_fEventSpeed = _eventspeed;

        m_fEventTime = 0;

        tmp_list.Clear();

        GetSameEventInfo(_name, ref tmp_list);

        //if (tmp_list == null)
        //{
        //    Logger.LogError("tmp_list is null");

        //    return;
        //}

        for (int i = 0; i < tmp_list.Count; ++i)
        {
            m_EventList.Add(tmp_list[i]);
        } 

        if (m_EventList.Count > 0)
        {
           m_fCurClipLength = m_EventList[0].m_flengthtime;
        }
    }
}
