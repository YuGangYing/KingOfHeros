using UnityEngine;
using System.Collections;

public class AttackSpeedData
{
   public float m_fAttackInterval;

   public float m_fAnimSpeed;

   public float m_fClipLength;
}

public class FormulaTool : Singleton<FormulaTool> 
{
    public float NormalDamageFormula(float _myattack, float _targetdef, float _restrictionfactor, float _critadd)
    {
        if ( (_myattack + _targetdef) == 0 )
        {
            Debug.LogError("argument can not be 0"); 
            return 0;
        }

        if (_critadd < 0 &&_critadd > 1)
        {
            Debug.LogError("argument Error");
            return 0;
        }

        float tmp_rangefactor = Random.Range(0.9f, 1.2f);

        float tmp_damage =  _myattack * _myattack / (_myattack + _targetdef) * (_restrictionfactor/100) * tmp_rangefactor * (1 + _critadd);

        tmp_damage = Mathf.Abs(tmp_damage);

        tmp_damage = Mathf.Max(1, tmp_damage); 

        return tmp_damage;
    }

    public float SkillDamageFormula(float _myattack, float _targetdef, float _skilldamagefactor, float _skillbasedamage, float _critadd)
    {
        if ((_myattack + _targetdef) == 0)
        {
            Logger.LogError("argument Error");
            
            return 0;
        }

        float tmp_rangefactor = Random.Range(0.9f, 1.2f);

        float tmp_damage = (_myattack * _myattack / (_myattack + _targetdef) * _skilldamagefactor + _skillbasedamage) * (1 + _critadd) * tmp_rangefactor;

        tmp_damage = Mathf.Abs(tmp_damage);

        tmp_damage = Mathf.Max(1, tmp_damage); 

        return tmp_damage;
    }

    public float ViolenceRate(float _vlovalue, float _level, float _targetvlovlaue, float _targetlevel)
    {
        float tmp_vlodiff = (_vlovalue - _targetvlovlaue);

        float tmp_levlediff = (_level + _targetlevel);

        if ((tmp_levlediff / 2) == 0)
        {
            Logger.LogError("argument Error");
            return 0;
        }

        float tmp_value = Mathf.Max(5 + ( (tmp_vlodiff / (tmp_levlediff / 2) ) * 80), 0);

        return tmp_value;
    }
     
    public float LeadershipAdd(float _leadervalue, int _level)
    {
        if ((_leadervalue + 7) == 0)
        {
            Logger.LogError("argument Error");

            return 0;
        } 
       
        float tmp_leaderadd = Mathf.Min(_leadervalue / (_leadervalue + 7) * 80, 2);

        return tmp_leaderadd;
    }
     
    public AttackSpeedData CalculateAttackAnimSpeed(float _attackspeed, float _cliplength, float _attackspeedadd)
    {
        if ( 1 + (_attackspeedadd / 100) == 0 )
        {
            Logger.LogError("value can not is 0");

            return null;
        }

        float tmp_interval = (_attackspeed / (1 + (_attackspeedadd / 100))) - _cliplength;

        AttackSpeedData tmp = new AttackSpeedData();

        if (tmp_interval > 0)
        {
            tmp.m_fAnimSpeed = 1;

            tmp.m_fAttackInterval = tmp_interval;

            return tmp; 
        }

        _attackspeed = (_attackspeed / (1 + (_attackspeedadd / 100)));

        if (_attackspeed == 0)
        {
            Logger.LogError("value can not is 0");

            return null;
        }

        tmp.m_fAnimSpeed = _cliplength /_attackspeed ;

        tmp.m_fAttackInterval = 0;

        return tmp;
    } 
}
