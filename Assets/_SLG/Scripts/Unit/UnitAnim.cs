using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
public enum AnimationTyp { ATTACK, SKILL, HIT, IDLE, RUN, DEATH, REPEL, REPELSTANDUP, MAXTYPE}

public class UnitAnim : MonoBehaviour
{  
    private string[] m_AnimationTypeNames = { "Attack0", "Skill0", "Hit0", "StandBy0", "Run0", "Death0","Repel0","RepelStandUp0" };

    //骑兵攻击动画顺序 
    private int[] m_CavalryPlaySequence = {2,1};

    //盾兵攻击动画顺序
    private int[] m_PeltastPlaySSequence = {1,2};

    private Unit m_Unit = null;

    private UnitAnimEvent m_AnimEvent = null;

    private UnitAttribute m_UnitAtb = null; 
   
    public List<Animation> m_AnimationList = new List<Animation>();

    public Dictionary<string, int> m_AnimNumsDic = new Dictionary<string, int>();
    
    public int AnimMode = 0;  
    
    public string m_CurClipName; 

    public const int m_iAnimMaxIndex = 3;

    public const int m_iAnimMinIndex = 1;

    private float m_fAnimtionTime = 0;

    public float m_fAnimSpeed = 0;

    private int m_iPlayIndex = 0;

    private float m_fattacktime;

    void Awake()
    { 
        if (m_AnimationList == null)
        {
            return;
        }

        Animation[] tmp_anims = GetComponentsInChildren<Animation>(true);

        if (tmp_anims == null)
        {
            Logger.LogError("tmp_anims is null");

            return;
        }

        for (int i = 0; i < tmp_anims.Length; ++i)
        {
            if (tmp_anims[i] != null && tmp_anims[i].gameObject.activeSelf)
            {
                m_AnimationList.Add(tmp_anims[i]);
            }
        }

        m_Unit = GetComponent<Unit>();

        if (m_Unit == null)
        {
            Logger.LogError("m_Unit is null");

            return;
        }

        m_UnitAtb = GetComponent<UnitAttribute>();

        if (m_UnitAtb == null)
        {
            Logger.LogError("m_UnitAtb is null");

            return;
        }

        m_AnimEvent = GetComponent<UnitAnimEvent>();

        if (m_AnimEvent == null)
        {
            Logger.LogError("m_AnimEvent is null");

            return;
        }

        InitAnimationData(); 
    }

    void LateUpdate()
    {
        m_fAnimtionTime += Time.deltaTime * m_fAnimSpeed; 
    }

    private void InitAnimationData()
    {
        if (m_AnimationList == null)
        {
            return;
        }

        CollectSameAnimaClipNums(); 

        SetAnimation(AnimationTyp.IDLE,0,WrapMode.Loop);
    }

    private void CollectSameAnimaClipNums()
    {
        if (m_AnimationList == null)
        {
            return;
        }

        if (m_AnimationList.Count > 0)
        {
            foreach (AnimationState state in m_AnimationList[0])
            {
                for (int j = 0; j < m_AnimationTypeNames.Length; ++j)
                {
                    if (state.name.Contains(m_AnimationTypeNames[j]))
                    {
                        if (!m_AnimNumsDic.ContainsKey(m_AnimationTypeNames[j]))
                        {
                            m_AnimNumsDic.Add(m_AnimationTypeNames[j], 1);
                        }
                        else
                        {
                            m_AnimNumsDic[m_AnimationTypeNames[j]]++;
                        }
                    }
                }
            }
        }
    }

    private float GetLegalSpeed(float _speed)
    { 
        float tmp_speed = 0.0f;
        if (_speed <= 0)
        {
            tmp_speed = 1;
        }
        else
        {
            tmp_speed = Mathf.Min(_speed, 1.5f);
        }

        return tmp_speed;
    }

    public float GetClipLength(string _name)
    {
        if (m_AnimationList == null)
        {
            Logger.LogError("m_Animationlist is null");

            return 0;
        } 

        if (!CheckAnimation(_name))
        {
            Logger.LogError("not have this clip");

            return 0 ;
        }

        for (int i = 0; i < m_AnimationList.Count; ++i)
        {
            if (m_AnimationList[i] != null)
            {
				if (CheckAnimation(_name))
                	return m_AnimationList[i][_name].clip.length;
				else 
					return 0;
            }
        }

        return 0;
    }

    public float GetClipLength(AnimationTyp _type, int _index)
    {
        if (m_AnimationList == null)
        {
            Logger.LogError("m_AnimationList is null");

            return 0 ;
        }

        if (_index == 0)
        {
            _index = 1;
        } 

        int tmp_type = (int)_type; 
      
        string tmp_clipname = m_AnimationTypeNames[tmp_type];

        if (!m_AnimNumsDic.ContainsKey(m_AnimationTypeNames[tmp_type]))
        {
            Logger.LogError("动作组件不包含" + m_AnimationTypeNames[tmp_type] + "动画");

            return 0;
        }

        _index = Mathf.Min(m_AnimNumsDic[m_AnimationTypeNames[tmp_type]], _index);

        tmp_clipname += _index;

        if (!CheckAnimation(tmp_clipname))
        {
            Logger.LogError("not have this clip");

            return 0;
        }

        for (int i=0; i < m_AnimationList.Count; ++i)
        {
            if (m_AnimationList[i] != null)
            {
                return  m_AnimationList[i][tmp_clipname].clip.length;
            }
        }

        return 0;
    }

    public void SetAnimationSpeed(string _name, float _speed)
    {
        if (m_AnimationList == null)
        {
            Logger.LogError("m_Animationlist is null");

            return;
        }
 
        if (_name.Length <= 0)
        {
            Logger.LogError("_name.length is 0");
            
            return;
        }

        if (!CheckAnimation(_name))
        {
            Logger.LogError("not have this clip");

            return;
        }

        for (int i = 0; i < m_AnimationList.Count; ++i)
        {
            if (m_AnimationList[i] != null)
            { 
                float speed = GetLegalSpeed(_speed);

                m_AnimationList[i][m_CurClipName].speed = _speed;
            }
        }
    }

    public void SetAnimationTime(string _name, float _time)
    {
        if (m_AnimationList == null)
        {
            Logger.LogError("m_Animationlist is null");

            return;
        }

        if (_time < 0)
        {
            Logger.LogError("SetAnimationTime Value can not is -0");

            _time = 0;
        }

        if (!CheckAnimation(_name))
        {
            Logger.LogError("not have this clip");

            return ;
        }

        for (int i = 0; i < m_AnimationList.Count; ++i)
        {
            if (m_AnimationList[i] != null)
            {
                m_AnimationList[i][_name].normalizedTime = _time;
            }
        }
    }  

    private void PlayeAnimation(string _name, WrapMode _mode, float _speed = 1.0f, float _fadelength = 0.1f)
    {   
        m_CurClipName = _name;

        for (int i = 0; i < m_AnimationList.Count; ++i)
        { 
            m_AnimationList[i].CrossFade(_name, _fadelength); 
          
            m_AnimationList[i][_name].speed = _speed;

            m_AnimationList[i][_name].wrapMode = _mode;

            m_fAnimtionTime = 0; 
        }  
    }

    private void PlaySequenceAnimation(int[] _arrayindex)
    {
        if (_arrayindex == null)
        {
            Logger.LogError("_arrayindex is null");

            return;
        }

        if (_arrayindex.Length > 0)
        {
            //根据不同类型的兵种攻击动作调用的顺序会不一样 
            if (m_Unit != null && m_Unit.Attribute != null && m_Unit.Attribute.m_StaticData != null)
            {
                string tmp_animname = "";

                if (_arrayindex == null)
                {
                    return;
                }

                tmp_animname = SetAnimation(AnimationTyp.ATTACK, _arrayindex[m_iPlayIndex],
                                            WrapMode.ClampForever, m_UnitAtb.BaseAttackSpeed);

                m_iPlayIndex++;

                if (m_iPlayIndex > _arrayindex.Length - 1)
                {
                    m_iPlayIndex = 0;
                }  
            }
        }
    }

    public bool IsPlayComplete(float _speed = 1.0f)
    {
        if (m_AnimationList == null)
        {
            return false;
        }

        m_fAnimSpeed = _speed;

        for (int i = 0; i < m_AnimationList.Count; ++i)
        {
            if (m_AnimationList[i] != null)
            {
                if (m_fAnimtionTime >= m_AnimationList[i][m_CurClipName].clip.length)
                {
                    m_fAnimtionTime = 0;

                    return true;
                }
            }
        }

        return false;
    }

    public string RandomSetAnimation(AnimationTyp _type, int _max, int _min, 
                                    WrapMode _mode, float _speed = 1.0f, float _fadelength = 0.5f)
    {
        string tmp_clipname = "";

        int tmp_type = (int)_type;
        
        string tmp_animname = m_AnimationTypeNames[tmp_type];

        if (!m_AnimNumsDic.ContainsKey(tmp_animname))
        {
            Logger.LogError(tmp_animname + "_name do not contain key in m_AnimNumsDic");
            return "";
        }

		if (_mode == WrapMode.Clamp)
			_mode = WrapMode.Once;

        int tmp_index = Random.Range(_min, _max);

        tmp_index = Mathf.Min(m_AnimNumsDic[tmp_animname], tmp_index);

        tmp_clipname = tmp_animname + tmp_index;   
      
        if (m_CurClipName != tmp_clipname)
        {
            PlayeAnimation(tmp_clipname, _mode, _speed, _fadelength);  
        }
         
        return tmp_clipname;
   } 

    public string SetAnimation(AnimationTyp _type, int _index, WrapMode _mode, float _speed = 1.0f, float _fadelength = 0.5f)
    { 
        if (_index == 0)
        {
            _index = 1;
        }

        int tmp_type = (int)_type;

        string tmp_clipname = m_AnimationTypeNames[tmp_type];

		if (!m_AnimNumsDic.ContainsKey(m_AnimationTypeNames[tmp_type]))
		{
			return tmp_clipname;
		}

        _index =  Mathf.Min(m_AnimNumsDic[m_AnimationTypeNames[tmp_type]], _index);

        tmp_clipname += _index;

        if (!CheckAnimation(tmp_clipname))
        {
            Logger.LogError("Error");
            return "";
        }

		if (_mode == WrapMode.Clamp)
			_mode = WrapMode.Once;

		if (tmp_clipname == "Skill02")
		{
			Debug.Log("m_CurClipName............................" + m_CurClipName);
			Debug.Log("tmp_clipname............................" + tmp_clipname);
		}

        if (m_CurClipName != tmp_clipname)
        {
            PlayeAnimation(tmp_clipname, _mode, _speed, _fadelength);    
        } 

        return tmp_clipname;
    }
    
    public bool CheckAnimation(string _name)
    {
        if (m_AnimationList == null)
        {
            Logger.LogError("m_AnimationList is null");

            return false; 
        } 
        
        for (int i=0 ; i<m_AnimationList.Count; ++i)
        {
            foreach (AnimationState state in m_AnimationList[i])
            {
                if (_name == state.name)
                { 
                    return true;
                }
            }
        }  
        
        return false;
    }

    public AttackSpeedData GetAttaclSpeedData(float _attackspeed, float _attackspeedadd)
    {
        float temp_cliplength = GetClipLength(m_CurClipName);

        AttackSpeedData tmp_attackspeedata = FormulaTool.me.CalculateAttackAnimSpeed(_attackspeed, temp_cliplength, _attackspeedadd);

        tmp_attackspeedata.m_fClipLength = temp_cliplength;

        m_UnitAtb.BaseAttackSpeed = tmp_attackspeedata.m_fAnimSpeed;

        SetAnimationSpeed(m_Unit.Anim.m_CurClipName, m_UnitAtb.BaseAttackSpeed);

        return tmp_attackspeedata;
    }

    public void AttackSpeedLogic(AttackSpeedData _data, float _cliplength, float _time)
    {  
        if (_data.m_fAttackInterval != 0)
        {
            m_fattacktime += (Time.deltaTime + _time);

            float m_fattacktimelength = (float)(_data.m_fAttackInterval + _cliplength);

            if (IsPlayComplete(_data.m_fAnimSpeed))
            {
                SetAnimation(AnimationTyp.IDLE, 1, WrapMode.Loop,1);
            }

            if (m_fattacktime >= m_fattacktimelength)
            {
                TransitionAttack();
                
                SetAnimationTime(m_Unit.Anim.m_CurClipName, 0);
                
                m_fattacktime = 0;
            }   
        }
        else
        {
            if (IsPlayComplete(_data.m_fAnimSpeed))
            {
                TransitionAttack();
                 
                SetAnimationTime(m_Unit.Anim.m_CurClipName, 0);
            }
        } 
    }

    public void TransitionSkill()
	{
		Debug.Log("TransitionSkill................................." + Time.realtimeSinceStartup);
		float clipLength = 3f;
		m_CurClipName = "";
		if (m_Unit.SkillAnimIndex != -1)
		{
			SetAnimation(AnimationTyp.SKILL,m_Unit.SkillAnimIndex,WrapMode.ClampForever);
			clipLength = GetClipLength(AnimationTyp.SKILL,m_Unit.SkillAnimIndex);
			StartCoroutine(_SkillDone(clipLength));
			Debug.Log("TransitionSkill........................clipLength........." + clipLength);

		}
		else
		{
			RandomSetAnimation(AnimationTyp.SKILL,  m_iAnimMaxIndex, m_iAnimMinIndex, WrapMode.ClampForever);   
			clipLength = GetClipLength(AnimationTyp.SKILL,m_iAnimMinIndex);
			StartCoroutine(_SkillDone(clipLength));
		}	
		//SetAnimationTime(m_Unit.Anim.m_CurClipName, 0);
	}

	public void TransitionSkill(int skillAnimIndex)
	{
		float clipLength = 3f;
		Animation anim = m_Unit.Anim.m_AnimationList[0];
		int tmp_type = (int)AnimationTyp.SKILL;		
		string tmp_clipname = m_AnimationTypeNames[tmp_type];
		tmp_clipname = tmp_clipname + skillAnimIndex;
		anim.Play(tmp_clipname);
		//SetAnimation(AnimationTyp.SKILL,skillAnimIndex,WrapMode.Once);
		clipLength = GetClipLength(AnimationTyp.SKILL,m_Unit.SkillAnimIndex);
		StartCoroutine(_SkillDone(clipLength));
	}


	IEnumerator _SkillDone(float duration)
	{
		yield return new WaitForSeconds(duration + 0.5f);
		m_Unit.GetComponent<UnitSkill>().SetSkillDone();
		//Debug.Log("Skill Done true................................" + m_Unit.GetComponent<UnitSkill>().currSkill.SkillId);
	}


	public void TransitionSkill2()
	{
        RandomSetAnimation(AnimationTyp.SKILL, m_iAnimMaxIndex, m_iAnimMinIndex, WrapMode.Once);  
	} 
	  
	public void TransitionHit()
	{
        RandomSetAnimation(AnimationTyp.HIT, m_iAnimMaxIndex, m_iAnimMinIndex, WrapMode.Once);  
	}

	public void TransitionAttack()
    { 
        //根据不同类型的兵种攻击动作调用的顺序会不一样  
        switch (m_Unit.Attribute.m_StaticData.armyType)
        {
            case ARMY_TYPE.CAVALRY:
                PlaySequenceAnimation(m_CavalryPlaySequence);
                break;
            case ARMY_TYPE.SHIELD:
                PlaySequenceAnimation(m_PeltastPlaySSequence);
                break;
            case ARMY_TYPE.MAGIC:
                RandomSetAnimation(AnimationTyp.ATTACK, m_iAnimMaxIndex, m_iAnimMinIndex, WrapMode.ClampForever, m_UnitAtb.BaseAttackSpeed);
                break;
            case ARMY_TYPE.PIKEMAN:
                RandomSetAnimation(AnimationTyp.ATTACK, m_iAnimMaxIndex, m_iAnimMinIndex, WrapMode.ClampForever, m_UnitAtb.BaseAttackSpeed);
                break;
            case ARMY_TYPE.HERO:
                RandomSetAnimation(AnimationTyp.ATTACK, m_iAnimMaxIndex, m_iAnimMinIndex, WrapMode.ClampForever, m_UnitAtb.BaseAttackSpeed);
                break;
            default:
                RandomSetAnimation(AnimationTyp.ATTACK, m_iAnimMaxIndex, m_iAnimMinIndex, WrapMode.ClampForever, m_UnitAtb.BaseAttackSpeed);
                break;
        }

        m_AnimEvent.SetAnimEvent(m_CurClipName, false, m_UnitAtb.BaseAttackSpeed); 
	} 

	public void TransitionIdle()
	{
		RandomSetAnimation(AnimationTyp.IDLE, m_iAnimMaxIndex, m_iAnimMinIndex, WrapMode.Loop,1,0.1f); 
	}

	public void TransitionRun()
	{
        float speed = m_Unit.Move.speed / UnitAttribute.DEFAULTSMOVESPEED ;

        float tmp_speed = GetLegalSpeed(speed);

        SetAnimation(AnimationTyp.RUN, 1, WrapMode.Loop, tmp_speed); 
	} 
	  
	public void TransitionDie()
	{
		if(m_Unit.Attribute.LastAttacker!=null && m_Unit.Attribute.LastAttacker.IsHero && !m_Unit.IsHero)
		{
			TransitionFly();
		}else{
			RandomSetAnimation(AnimationTyp.DEATH, m_iAnimMaxIndex, m_iAnimMinIndex, WrapMode.Once);   
		}
	}  

	public void TransitionFly()
	{
		foreach(Animation anim in m_AnimationList)
		{
			anim.Play("Repel01");
		}
	}
}
 