using System;
using UnityEngine;

namespace SkillShow
{
    public enum SkillRangType
    {
        RECT,
        CIRCLE,
        LINE
    }

    public enum SKillState
    {
        //冷却中
        COLDING,
        //等待中
        WAITING,
        //执行中
        EXECING
    }

    public enum enSkillGesture
    {
        enSG_Invalid = 0,
        enSG_DoubleTap,
        enSG_LongTap,
        enSG_Line,
        enSG_Arc,
        enSG_Circle,
        enSG_Amount,
    }

    //英雄技能
    public class HeroSkill
    {
        public int rangType;
        SKillState    skillState;
        //矩形攻击范围
        public float fRang1 = 1.0f;
        public float fRang2 = 1.0f;
        
        //特效
        public string strEffect;
        //技能执行时长
        public float fExecTime = 5f;
        //冷却时间
        public float fColdTime = 1f;
        //技能名
        public string strSkill;
        //英雄
        public string strHero;
        //技能对应变量
        public string strBoolParam;
        //技能对应float变量
        public string strFloatParam;
        //当前状态开始时间
        public float fStateTime;

        public string strGesture;
 
        public HeroSkill()
        {
            state = SKillState.WAITING;
        }

        public SKillState state
        {
            get 
            {
                return skillState; 
            }
            set
            {
                skillState = value;
                fStateTime = Time.time;
            }
        }
    }
}
