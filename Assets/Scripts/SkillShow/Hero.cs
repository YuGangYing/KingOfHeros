using UnityEngine;
using System.Collections.Generic;

using SkillShow;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
public class Hero : MonoBehaviour {

    Animator mAnimator = null;
    Drawer drawer = null;
    SkillShow.HeroSkill mCurSkill = null;
    bool bExec = false;
    GameObject mEffect = null;
    public Color CurColor { get; set; }
    public int CurWidth { get; set; }

    Camera mCam = null;

    List<int> mIndex = new List<int>();//CConstance.INVALID_ID;

	// Use this for initialization
    void Awake()
    {
        mAnimator = gameObject.GetComponent<Animator>();
        drawer = gameObject.AddComponent<Drawer>();
        drawer.setLine(Color.blue, 1.0f);
    }

	void Start () 
    {
        this.CurColor = Color.red;
        this.CurWidth = 2;
        GameObject go = GameObject.Find("Main Camera");
        if (go != null)
        {
            mCam = go.GetComponent<Camera>();
        }
	}
	
    void Update () 
    {
        if (mCurSkill == null)
            return;
        if (mCurSkill.state == SKillState.EXECING)//执行时
        {
            if (Time.time - mCurSkill.fStateTime >= mCurSkill.fExecTime)//执行完成，开始冷却
            {
                stopSkill();
                mCurSkill.state = SKillState.COLDING;
            }
        }
        else if (mCurSkill.state == SKillState.COLDING)//冷却时
        {
            if (Time.time - mCurSkill.fStateTime >= mCurSkill.fColdTime)//冷却完成
            {
                mCurSkill.state = SKillState.WAITING;
                if (mIndex.Count != 0)//CConstance.INVALID_ID)
                {
                    for (int i = 0; i < mIndex.Count; i++)
                    {
                        int nIndex = mIndex[i];
                        drawer.cleanByIndex(nIndex);
                    }

                    mIndex.Clear();

                    SimpleTouch.me.V3ScreenPoint = Vector3.zero;

                    if (mgoAttackPtEffect != null)
                    {
                        GameObject.Destroy(mgoAttackPtEffect);
                        mgoAttackPtEffect = null;
                    }
                }
            }
        }
	}
    
    public GameObject effect
    {
        get {return mEffect;}
        set
        {
            if (mEffect != null)
            {
                GameObject.Destroy(mEffect);
                mEffect = null;
            }
            mEffect = value;
        }
    }

    GameObject mgoAttackPtEffect = null;
    public GameObject AttackPointEffect
    {
        get { return mgoAttackPtEffect; }
        set
        {
            if (mgoAttackPtEffect != null)
            {
                GameObject.Destroy(mgoAttackPtEffect);
                mgoAttackPtEffect = null;
            }

            mgoAttackPtEffect = value;
            if (mgoAttackPtEffect != null)
            {
                mgoAttackPtEffect.transform.position = _GetPos(SimpleTouch.me.V3ScreenPoint);
                SimpleTouch.me.V3ScreenPoint = Vector3.zero;
            }
        }
    }

    public SkillShow.HeroSkill skill
    {
        get { return mCurSkill; }
        set { mCurSkill = value; }
    }

    //显示技能
    public void showSkill()
    {
        //停止动作
        if (mCurSkill != null)
        {
            if (mCurSkill.state == SKillState.EXECING)//执行时停止播放技能
                stopSkill();
            mCurSkill.state = SKillState.WAITING;
        }
        if (skill == null)
            return;

        mCurSkill = skill;
        ExecSkill(mCurSkill);
        mCurSkill.state = SKillState.EXECING;
    }

    public void drawCircle(float fRange)
    {
        drawer.clear();

        drawer.setLine(this.CurColor, this.CurWidth);

        Vector3 v = _GetPos(SimpleTouch.me.V3ScreenPoint); //transform.position;
        drawer.setLine(Color.red, 2f);

        v.y = transform.position.y;
        drawer.drawCircle(v, fRange);

        //if (SimpleTouch.me.Enable)
        //{
        //    v = _GetPos(SimpleTouch.me.V3ScreenPoint);
        //    drawer.setLine(Color.green, 2f);
        //    if (v != transform.position)
        //    {
        //        int nIndex = drawer.drawCircle(v, fRange);
        //        mIndex.Add(nIndex);
        //    }
        //}
    }

    public void drawRect(float fRange1, float fRange2)
    {
        drawer.clear();

        Vector3 v = transform.position;
        Vector3 vex = _GetPos(SimpleTouch.me.V3ScreenPoint);
        drawer.setLine(Color.red, 2f);

        Vector3 topleft = new Vector3(v.x, v.y, v.z - fRange2/2);
        Vector3 bottomRight = new Vector3(vex.x, v.y, vex.z - fRange2 / 2); //Vector3(v.x + fRange1, v.y, v.z + fRange2 / 2);
        drawer.drawRect(topleft, bottomRight);

        //if (SimpleTouch.me.Enable)
        //{
        //    v = _GetPos(SimpleTouch.me.V3ScreenPoint);
        //    drawer.setLine(Color.green, 2f);

        //    if (v != transform.position)
        //    {
        //        topleft = new Vector3(v.x, v.y, v.z - fRange2 / 2);
        //        bottomRight = new Vector3(v.x + fRange1, v.y, v.z + fRange2 / 2);

        //        int nIndex = drawer.drawRect(topleft, bottomRight);
        //        mIndex.Add(nIndex);
        //    }
        //}
    }

    public void drawFan(float fAngle,float fRadius)
    {
        drawer.clear();

        float f = fAngle / 2 * Mathf.Deg2Rad;
        float fX = fRadius * Mathf.Sin(f);
        float fZ = fRadius * Mathf.Cos(f);

        Vector3 v = transform.position;
        Vector3 vex = _GetPos(SimpleTouch.me.V3ScreenPoint);

        vex.y = v.y;
        float freal = Vector3.Distance(vex, v);
        float frealx = freal * Mathf.Sin(f);
        float frealz = freal * Mathf.Cos(f);

        //float frealtt = freal * Mathf.Tan(f);
        //float frealt = freal / Mathf.Cos(f);

        //float x = 0, y = 0;
        //float tt = frealtt * frealtt;

        //float ffff = frealtt * frealtt - frealt * frealt;

        //tt = (v.x * v.x - 2 * x * v.x + x * x) + (v.z * v.z - 2 * y * v.z + y * y);
        ////tt = (vex.x * vex.x - 2 * x * vex.x + x * x) + (vex.z * vex.z - 2 * y * vex.z + y * y);

        //ffff = (v.x - vex.x) * (v.x - vex.x) - 2 * x * (v.x - vex.x) + (v.z - vex.z) * (v.z - vex.z) - 2 * y * (v.z - vex.z);
        //x = ((v.x - vex.x) * (v.x - vex.x) - ffff + (v.z - vex.z) * (v.z - vex.z) - 2 * y * (v.z - vex.z)) / (2 * (v.x - vex.x));




        drawer.setLine(Color.red, 2f);



        Vector3 begin = new Vector3(vex.x - frealx, vex.y, vex.z + frealz); //Vector3(v.x - fX, v.y, v.z + fZ);
        Vector3 end = new Vector3(vex.x + frealx, vex.y, vex.z + frealz);  //Vector3(v.x + fX, v.y, v.z + fZ);

        drawer.drawLine(v, begin);//new Vector3(v.x - fX, v.y, v.z + fZ));
        drawer.drawLine(v, end);//new Vector3(v.x + fX, v.y, v.z + fZ));
        //drawer.drawCircle(v, fRadius);
        //drawer.drawFan(v, fAngle, fRadius);

        //if (SimpleTouch.me.Enable)
        //{
        //    v = _GetPos(SimpleTouch.me.V3ScreenPoint);
        //    drawer.setLine(Color.green, 2f);
        //    if (v != transform.position)
        //    {
        //        begin = new Vector3(v.x - fX, v.y, v.z + fZ);
        //        end = new Vector3(v.x + fX, v.y, v.z + fZ);

        //        int nIndex = 0;
        //        nIndex = drawer.drawLine(v, new Vector3(v.x - fX, v.y, v.z + fZ));
        //        mIndex.Add(nIndex);
        //        nIndex = drawer.drawLine(v, new Vector3(v.x + fX, v.y, v.z + fZ));
        //        mIndex.Add(nIndex);
        //        nIndex = drawer.drawCircle(v, fRadius);
        //        mIndex.Add(nIndex);
        //    }
        //}
    }

    void ExecSkill(SkillShow.HeroSkill skill)
    {
        bExec = true;
        mAnimator.SetBool(skill.strBoolParam, true);
    }

    void LateUpdate()
    {
        if(bExec)
        {
            AnimatorStateInfo state = mAnimator.GetCurrentAnimatorStateInfo(1);
            skill.fExecTime = state.length;
            bExec = false;
        }
    }

    public void stopSkill()
    {
        mAnimator.SetBool(mCurSkill.strBoolParam, false);
        bExec = false;
    }

    public void release()
    {
        drawer.clear();
    } 

    public float execPercent()
    {
        if (mEffect != null)
        {
            GameObject.Destroy(mEffect);
            mEffect = null;
        }
        return 1f;
    }

    Vector3 _GetPos(Vector3 vPos)
    {
         Vector3 v = Vector3.zero;
        if (vPos == Vector3.zero)
            v = transform.position;
        else
        {
            vPos.z = vPos.z - mCam.transform.position.z;
            Plane nearPlane = new Plane(mCam.transform.forward, mCam.transform.position);
            float dist = nearPlane.GetDistanceToPoint(transform.position);

            Vector3 vTemp = new Vector3(vPos.x, vPos.y, dist);
            v = mCam.ScreenToWorldPoint(vTemp);
        }

        return v;
    }
}
