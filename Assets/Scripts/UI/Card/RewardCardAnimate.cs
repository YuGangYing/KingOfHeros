using UnityEngine;
using System.Collections;
//Line End
public class RewardCardAnimate : MonoBehaviour
{
    public float mfDuration = 0.7f;
    public Vector3 mvDetailPosition = new Vector3(0f, -34f, 0f);
    public Vector3 mvScale = new Vector3(1f, 1f, 1f);

    public TweenPosition mTPosition = null;
    public TweenScale mTScale = null;
    public bool mbDoAnimate = false;

    private const float mfHoldTime = 0.3f;
    private bool mbAuto = false;
    private float mfRecordTime = 0f;
    private float mfCurTime = 0f;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        _OnAnimate();
        _OnEnd();

        _OnAutoTick();
	}

    void _OnAnimate()
    {
        if (mbDoAnimate == false)
            return;

        mbDoAnimate = false;
        mTPosition = TweenPosition.Begin(this.gameObject, mfDuration, mvDetailPosition);
        mTScale = TweenScale.Begin(this.gameObject, mfDuration, mvScale);
    }

    bool _OnEnd()
    {
        if (mTScale == null)
            return false;

        if (mTScale.enabled == true)
            return false;

//         SLG.EventArgs obj = new SLG.EventArgs();
//         obj.m_obj = (object)mIndex;

        SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyOpenRewardCardBg, null);

        mTScale = null;

        _ResetAutoTime(null);

        return true;
    }

    bool _ResetAutoTime(SLG.EventArgs objArg)
    {
        mbAuto = true;

        mfCurTime = Time.deltaTime;
        mfRecordTime = Time.deltaTime;
        return true;
    }

    void _OnAutoTick()
    {
        if (!mbAuto)
        {
            return;
        }

        mfCurTime = mfCurTime + Time.deltaTime - mfRecordTime;
        if (mfCurTime >= mfHoldTime)
        {
            mfCurTime = 0f;
            mbAuto = false;

            SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyAutoOpenRewardItem, null);
        }
    }
}
