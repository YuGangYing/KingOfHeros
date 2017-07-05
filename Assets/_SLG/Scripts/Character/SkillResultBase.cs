using System;
using System.Collections.Generic;
using DataMgr;
using UnityEngine;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
namespace Fight
{
    //技能效果基类
	public class SkillResultBase
    {
        #region define variable

        //状态时间
        protected ConfigRow _config = null;

        //效果施放时间
        protected float _startTime;
        protected bool _finish = false;
        protected int _doneTimes = 0;

        protected SKILL_REUSLT_REPEAT_TYPE _repeatType;

        //技能ID，和等级

        #endregion
        #region getset

        //技能效果
        public SKILL_RESULT_TYPE resultType
        {
            get;
            set;
        }

        public object param
        {
            get;
            set;
        }

        public int skillId
        {
            get;
            set;
        }

        public int resultId
        {
            get;
            set;
        }

        public int level
        {
            get;
            set;
        }

        public float duration
        {
            get;
            set;
        }

        public ConfigRow config
        {
            get;
            set;
        }

        //技能发起者
        public Character attacker
        {
            get;
            set;
        }
        //被施加对象角色
        public Character atkObject
        {
            get;
            set;
        }

        public bool isFinish
        {
            get { return this._finish; }
        }

        public float levelValue
        {
            get;
            set;
        }

        public SKILL_RESULT_VALUE_TYPE levelValueType
        {
            get;
            set;
        }

        public SKILL_RESULT_LEVEL_TYPE levelType
        {
            get;
            set;
        }

        //重复执行间隔
        public float repeatInterval
        {
            get;
            set;
        }

        public float relayTime
        {
            get;
            set;
        }

        //子类型
        public int subType
        {
            get;
            set;
        }

        public CBattleFightGestureData paramData
        {
            get;
            set;
        }

        #endregion
        #region public function

        public SkillResultBase()
        {
            _startTime = Time.time;
        }

        public virtual bool init(ConfigRow resultCfg)
        {
            if(resultType==null)
                return false;
            this._config = resultCfg;

            this.subType = _config.getIntValue(CFG_SKILL_RESULT.SUBTYPE, 0);
            this.duration = _config.getFloatValue(CFG_SKILL_RESULT.DURATION, 0f);
            this._repeatType = _config.getEnumValue(CFG_SKILL_RESULT.REPEATTYPE, SKILL_REUSLT_REPEAT_TYPE.UNKNOWN);
            this.repeatInterval = _config.getFloatValue(CFG_SKILL_RESULT.REPEATINTERVAL, 0f);

            ConfigRow resultLevel = FightData.getHeroSkillResultLevel(this.resultId, this.level);
            if (resultLevel == null)
                return false;
            //本效果对应等级值
            this.levelValue = resultLevel.getFloatValue(CFG_SKILL_RESULT_LEVEL.VALUE, 0f);
            this.levelType = resultLevel.getEnumValue<SKILL_RESULT_LEVEL_TYPE>(CFG_SKILL_RESULT_LEVEL.TYPE, SKILL_RESULT_LEVEL_TYPE.UNKNOWN);
            this.levelValueType = resultLevel.getEnumValue<SKILL_RESULT_VALUE_TYPE>(CFG_SKILL_RESULT_LEVEL.VALUE_TYPE, SKILL_RESULT_VALUE_TYPE.UNKNOWN);
            return true;
        }

        //消除技能伤害效果。
        public virtual void release()
        {
            //if (this.atkObject != null && this.atkObject.resultMgr != null)
            //    this.atkObject.resultMgr.removeResult(this);
        }

        public virtual void onResult()
        {
        }

        public virtual void update()
        {
            if (_finish || atkObject == null)
                return;
            if (this.atkObject.isDeath())//死亡时不再处理
                return;
            //判断超时
            if (Time.time - (this._startTime + this.duration) >= 0f)
            {
                //第一次无条件执行
                if (this._doneTimes != 0)
                {
                    this._finish = true;
                    this.release();
                    return;
                }
            }
            //多次执行
            if (Time.time - this._startTime - this.repeatInterval * this._doneTimes >= this.relayTime)//真的效果开始了
            {
                if(this._doneTimes ==0 || this._repeatType == SKILL_REUSLT_REPEAT_TYPE.INRESULT)
                {
                    this.onResult();
                    this._doneTimes++;
                }
             }
        }
        #endregion


        #region private function
        #endregion
    }
}
