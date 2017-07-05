using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
//-
public class CSolider : MonoBehaviour {
	
	public	enum HERO_STATUS
	{
		ST_IDLE,
		ST_WALK,
		ST_RUN,
		ST_ATTACK1,
		ST_ATTACK2,
		ST_DEATH,
		ST_JUMP,
		ST_JUMPUP,
		ST_JUMPDOWN,
		ST_JUMPSTAY
	}

	//移动目标点
	protected Vector3 destPt = Vector3.zero;
	//动作系统	
	protected Animator animator;
    //状态
	protected HERO_STATUS nStatus;

    List<string> m_animatorList = new List<string>();
    string m_strStatus = "";
    bool bUpdate = false;

	protected void Awake()
	{
        nStatus = HERO_STATUS.ST_IDLE;
		animator = gameObject.GetComponent<Animator>();
	}
	
	protected void Start()
	{
        getParamList();
	}

    public List<string> animationList
    {
        get { return m_animatorList; }
    }

    //获得参数列表
    void getParamList()
    {
        if(animator==null)
            return;
        /*m_animatorList.Clear();
        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
        int nCount = controller.parameterCount;
        for (int i = 0; i < nCount; i++)
        {
            AnimatorControllerParameter ps = controller.GetParameter(i);
            m_animatorList.Add(ps.name);
        }*/
    }
	
	private void LateUpdate()
	{
        if (!bUpdate)
            return;
        bUpdate = false;
		animator.SetBool("Idle",false);
        if (m_strStatus != null && m_strStatus.Length > 0)
        {
            Debug.Log(m_strStatus);
            animator.SetBool(m_strStatus, true);
        }
		/*
         switch(nStatus)
		{
		default:
		case HERO_STATUS.ST_IDLE:
			animator.SetBool("isIdle",true);
			break;
		case HERO_STATUS.ST_RUN:
			animator.SetBool("isRun",true);
			break;
		case HERO_STATUS.ST_DEATH:
			animator.SetBool("isDeath",true);
			break;			
		case HERO_STATUS.ST_WALK:
			animator.SetBool("isWalk",true);
			break;
		case HERO_STATUS.ST_ATTACK2:
			animator.SetBool("isAttack2",true);
			break;
		case HERO_STATUS.ST_ATTACK1:
			animator.SetBool("isAttack1",true);
			break;
		case HERO_STATUS.ST_JUMPUP:
		case HERO_STATUS.ST_JUMPDOWN:
		case HERO_STATUS.ST_JUMPSTAY:
			animator.SetBool("isJump",true); 
			break;
		}*/
	}

    public void execParam(string strParam)
    {
        foreach (string item in m_animatorList)
            animator.SetBool(item, false);
        //animator.SetBool(strParam, true);
        animator.SetBool("Idle", true);
        m_strStatus = strParam;
        bUpdate = true;
    }
	
	public virtual void setStatus(HERO_STATUS nNewStatus)
	{
		if(nNewStatus==nStatus || animator==null)
			return;
		//复位
		animator.SetBool("isIdle",true);
		animator.SetBool("isWalk",false);
		animator.SetBool("isRun",false);
		animator.SetBool("isJump",false);
		animator.SetBool("isAttack1",false);
		animator.SetBool("isAttack2",false);
		animator.SetBool("isDeath",false);
		nStatus = nNewStatus;
	}
	
	public HERO_STATUS getStatus()
	{
		return nStatus;
	}
	
	public Vector3 getDest()
	{
		return destPt;
	}
	
	public void setDest(Vector3 move)
	{
		destPt =  move;
	}
}
