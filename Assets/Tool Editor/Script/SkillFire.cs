using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
//using UnityEditor;
#endif
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
public class SkillFire
{
    public Animator release; // 释放者
    public Animator target; // 目标
    public SkillConfig config; // 技能配置

    // 特效
    public Xft.XffectComponent startEffect; // 起始特效
    public Xft.XffectComponent flyEffect; // 飞行特效
    public Xft.XffectComponent BlowEffect; // 击中特效

    public enum eState
    {
        Null, // 还未开始
        Start, // 起始阶段
        Starting,
        Fly, // 飞行阶段
        Flying,
        Hit, // 击中阶段
        Hiting,

        End, // 结束
    }

    public void begin()
    {
        m_state = eState.Start;
    }

    public void Release()
    {
        m_state = eState.Null;
        if (startEffect != null)
        {
            Object.DestroyImmediate(startEffect.gameObject);
            startEffect = null;
        }

        if (flyEffect != null)
        {
            Object.DestroyImmediate(flyEffect.gameObject);
            flyEffect = null;
        }

        if (BlowEffect != null)
        {
            Object.DestroyImmediate(BlowEffect.gameObject);
            BlowEffect = null;
        }

        if (release != null)
        {
            foreach (string name in aniNameList_release)
                release.SetBool(name, false);
        }

        if (target != null)
        {
            foreach (string name in aniNameList_target)
                target.SetBool(name, false);
        }
        aniNameList_release.Clear();
        aniNameList_target.Clear();
    }

    List<string> aniNameList_release = new List<string>();
    List<string> aniNameList_target = new List<string>();

    public eState currState
    {
        get { return m_state; }
    }

    eState m_state = eState.Null;
    public void Update(float time)
    {
        UpdateByState();
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (release)
                release.Update(time);
            if (release)
                target.Update(time);
        }
#endif
    }

    static AnimatorOverrideController s_aoc = null;
    static float GetAnimatonLenght(Animator a, string name)
    {
        if (a.runtimeAnimatorController == null)
            return 0f;
        else if (a.runtimeAnimatorController as AnimatorOverrideController)
            return (((AnimatorOverrideController)a.runtimeAnimatorController)[name]).length;
        else
        {
            if (s_aoc == null)
                s_aoc = new AnimatorOverrideController();
            s_aoc.runtimeAnimatorController = a.runtimeAnimatorController;
            AnimationClipPair[] pairs = s_aoc.clips;
            foreach (AnimationClipPair pair in pairs)
            {
                if (pair.overrideClip != null && pair.overrideClip.name == name)
                    return pair.overrideClip.length;
                else if (pair.originalClip != null && pair.originalClip.name == name)
                    return pair.originalClip.length;
            }
        }

        return 0f;
    }

    double TimeSinceStartup
    {
        get
        {
#if UNITY_EDITOR
            return UnityEditor.EditorApplication.timeSinceStartup;
#endif
            return Time.realtimeSinceStartup;
        }
    }

    double m_lastTime = -1;
    private void UpdateByState()
    {
        if (config == null)
            return;

        switch (m_state)
        {
        case eState.Start:
            {
                // 特效
                if (release != null)
                {
                    release.SetBool(config.startAnimName, true);
                    aniNameList_release.Add(config.startAnimName);
                }

                if (config.startEffect != null)
                {
                    startEffect = ((GameObject)Object.Instantiate(config.startEffect.gameObject)).GetComponent<Xft.XffectComponent>();
                    startEffect.EditView = true;
                    startEffect.gameObject.transform.parent = release.transform;
                    startEffect.transform.localPosition = Vector3.zero;
                    startEffect.transform.rotation = release.transform.rotation;
                }

                m_state = eState.Starting;
                m_lastTime = TimeSinceStartup;
            }
            break;
        case eState.Starting:
            {
                float detal = (float)(TimeSinceStartup - m_lastTime);
                //Debug.Log("detal:" + detal);
                if (detal >= GetAnimatonLenght(release, config.startAnimName))
                {
                    //Debug.Log("eState.Starting");
                    if (release != null)
                    {
                        release.SetBool(config.startAnimName, false);
                        aniNameList_release.Remove(config.startAnimName);
                    }

                    if (startEffect != null)
                    {
                        Object.DestroyImmediate(startEffect.gameObject);
                        startEffect = null;
                    }

                    m_state = eState.Fly;
                }
            }
            break;
        case eState.Fly:
            {
                m_state = eState.Flying;
                if (config.flyEffect != null)
                {
                    flyEffect = ((GameObject)Object.Instantiate(config.flyEffect.gameObject)).GetComponent<Xft.XffectComponent>();
                    flyEffect.EditView = true;
                    //flyEffect.gameObject.transform.parent = release.transform;
                    flyEffect.transform.position = release.transform.position;
                    flyEffect.transform.rotation = release.transform.rotation;
                    flyEffect.gameObject.SetActive(true);

//                     flyEffect.SetCollisionGoalPos(target.transform);
//                     flyEffect.SetArractionAffectorGoal(target.transform);
//                     flyEffect.SetCollisionCallback(OnCollision);
//                     flyEffect.ResetEditScene();
//                     flyEffect.EnableEditView();
                    m_lastTime = -1;
                }
                else
                {
                    OnCollision(null);
                }
            }
            break;
        case eState.Flying:
            {
                if (m_lastTime == -1)
                {
                    m_lastTime = TimeSinceStartup;
                    if (flyEffect != null)
                    {
                        //flyEffect.SetCollisionGoalPos(target.transform);
#if UNITY_EDITOR
                        flyEffect.ResetEditScene();
#endif
                        flyEffect.SetArractionAffectorGoal(target.transform);
                    }
                    return;
                }

                if ((flyEffect != null && flyEffect.IsPlaying == false) || (TimeSinceStartup - m_lastTime) >= 10.0f)
                {
                    OnCollision(null);
                }
            }
            break;
        case eState.Hit:
            {
                if (target)
                {
                    target.SetBool(config.blowAnimName, true);
                    aniNameList_target.Add(config.blowAnimName);
                }

                // 开始播放攻击特效
                if (config.blowEffect != null)
                {
                    BlowEffect = ((GameObject)Object.Instantiate(config.blowEffect.gameObject)).GetComponent<Xft.XffectComponent>();
                    BlowEffect.EditView = true;
                    BlowEffect.transform.position = target.transform.position;
                    BlowEffect.transform.rotation = target.transform.rotation;
                }

                m_state = eState.Hiting;
                m_lastTime = TimeSinceStartup;
            }
            break;
        case eState.Hiting:
            {
                float endTime = string.IsNullOrEmpty(config.blowAnimName) ? 3.0f : GetAnimatonLenght(target, config.blowAnimName);
                if ((TimeSinceStartup - m_lastTime) >= endTime)
                {
                    if (target)
                    {
                        target.SetBool(config.blowAnimName, false);
                        aniNameList_target.Remove(config.blowAnimName);
                    }
                    m_state = eState.End;
                }       
            }
            break;

        case eState.End:
            {
                if (startEffect != null)
                {
                    Object.DestroyImmediate(startEffect.gameObject);
                    startEffect = null;
                }

                if (flyEffect != null)
                {
                    Object.DestroyImmediate(flyEffect.gameObject);
                    flyEffect = null;
                }

                if (BlowEffect != null)
                {
                    Object.DestroyImmediate(BlowEffect.gameObject);
                    BlowEffect = null;
                }
            }
            break;
        }
    }

    void OnCollision(Xft.EffectLayer eff)
    {
        if (flyEffect != null)
        {
            Object.DestroyImmediate(flyEffect.gameObject);
            flyEffect = null;
        }

        m_state = eState.Hit;
    }
}