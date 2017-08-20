using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Line End
public class NpcAI : MonoBehaviour {

    enum NPC_STATE
    {
        None = 0,
        Walk,
        Run,
        SpWait,
    }

    List<Vector3> m_posMoveTargets;
    Vector3 m_posTarget;
    int m_TargetIndex;

    public float m_fSpeedWalk = 2.5f;
    public float m_fSpeedRun = 5;

    float m_fSpeed;

    Animator m_Animator;
    float m_fTimeInterval;
    NPC_STATE m_state;
    PathFollowing m_following;

	// Use this for initialization
	void Awake ()
    {
        m_Animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_state == NPC_STATE.None)
        {
            return;
        }

        float deltaTime = Time.deltaTime;
        m_fTimeInterval += deltaTime;

        if (m_fTimeInterval > 2.5f)
        {
            ChangeState();
            m_fTimeInterval = 0;
            return;
        }

        if (m_state != NPC_STATE.SpWait)
        {
            float dis = Vector3.Distance(transform.position, m_posTarget);
            if (dis <= 2)
            {
                ChangeState();
                m_fTimeInterval = 0;
                return;
            }
        }
	}

    public void Init(List<Vector3> targets, int nFirst)
    {
        if (targets.Count == 0)
        {
            return;
        }

       // gameObject.AddComponent<AiPathFinding>();
        m_following = gameObject.AddComponent<PathFollowing>();
        m_following.waypointActivationDistance = 1;

        m_posMoveTargets = new List<Vector3>();
        m_posMoveTargets.AddRange(targets);
        m_TargetIndex = nFirst;
        m_posTarget = m_posMoveTargets[nFirst];
        transform.position = m_posTarget;
        m_state = NPC_STATE.Walk;

        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }

        m_Animator.SetBool("walk", false);
    }

    void ChangeState()
    {
        if (m_state == NPC_STATE.Run)
        {
            m_Animator.SetBool("run", false);
        }
        else if (m_state == NPC_STATE.Walk)
        {
            m_Animator.SetBool("walk", false);
        }
        else if (m_state == NPC_STATE.SpWait)
        {
            m_Animator.SetBool("spwait", false);
        }

        int state = UnityEngine.Random.Range(1, 4);
        m_state = (NPC_STATE)state;

        if (m_state == NPC_STATE.Run)
        {
            ChangeTargetPos();

            m_fSpeed = m_fSpeedRun;
            m_Animator.SetBool("run", true);
        }
        else if (m_state == NPC_STATE.Walk)
        {
            ChangeTargetPos();

            m_fSpeed = m_fSpeedWalk;
            m_Animator.SetBool("walk", true);
        }
        else if (m_state == NPC_STATE.SpWait)
        {
            m_fSpeed = 0;
            m_following.pathFindingScript.enabled = false;
            m_Animator.SetBool("spwait", true);
        }

        m_following.movementSpeed = m_fSpeed;
    }

    void ChangeTargetPos()
    {
        int move = UnityEngine.Random.Range(0, 1);
        m_TargetIndex = move == 0 ? m_TargetIndex - 1 : m_TargetIndex + 1;

        if (m_TargetIndex >= m_posMoveTargets.Count)
        {
            m_TargetIndex = 0;
        }
        else if (m_TargetIndex < 0)
        {
            m_TargetIndex = m_posMoveTargets.Count - 1;
        }

        m_posTarget = m_posMoveTargets[m_TargetIndex];

        m_following.pathFindingScript.enabled = true;
        m_following.pathFindingScript.SetTargetPositon(m_posTarget);
    }
}
