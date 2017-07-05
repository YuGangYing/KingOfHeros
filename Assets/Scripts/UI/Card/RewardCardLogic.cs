using UnityEngine;
using System.Collections;
//Line End
public class RewardCardLogic : MonoBehaviour
{
    public int mIndex = CConstance.INVALID_ID;
    public bool mbSelect = false;
    public bool mbRandom = false;

    public TweenRotation mTweenRotate = null;
    public float mfRotateY = 0f;

	// Use this for initialization
	void Start ()
    {
        //mTweenRotate = this.gameObject.AddComponent<TweenRotation>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        _OnRotate();

        isEnd();

        _OnRandomSprite();
	}

    void _OnRotate()
    {
        if (!mbSelect)
        {
            return;
        }

        //mTweenRotate.from = new Vector3(0f, 0f, 0f);
        //mTweenRotate.to = new Vector3(0f, 360f,0f);

        this.transform.localRotation = new UnityEngine.Quaternion(
            this.transform.localRotation.x, 0f,
            this.transform.localRotation.z, this.transform.localRotation.w);

        mTweenRotate = TweenRotation.Begin(this.gameObject, 1f, this.transform.localRotation * Quaternion.Euler(new Vector3(0f, 180f, 0f)));
        mTweenRotate.style = UITweener.Style.Once;

        mbSelect = false;

        //if (Time.deltaTime >= .0001f)
        //    mfRotateY = mfRotateY + 10f;

        //if (mfRotateY >= 360f)
        //{
        //    mfRotateY = 0f;
        //    mbSelect = false;
        //}

        //this.transform.localRotation = new UnityEngine.Quaternion(
        //    this.transform.localRotation.x, mfRotateY / 360f,
        //    this.transform.localRotation.z, this.transform.localRotation.w);
    }

    bool isEnd()
    {
        if (mTweenRotate == null)
            return false;

        if (mTweenRotate.enabled == true)
            return false;

        this.gameObject.SetActive(false);

        FirEvent();

        mTweenRotate = null;

        return true;
    }

    void FirEvent()
    {
        SLG.EventArgs obj = new SLG.EventArgs();
        obj.m_obj = (object)mIndex;

        SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyOpenRewardCard, obj);        
    }

    void _OnRandomSprite()
    {
        if (!mbRandom)
        {
            return;
        }


        mbRandom = false;
    }
}
