using System;
using System.Collections.Generic;
using UnityEngine;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
namespace Fight
{
    [System.Serializable]
	public class SkillBase
    {
        #region define variable
        //技能对应配置信息
        ConfigRow _config;
        //实时执行数据
        //状态
        SKILL_STATUS _status;
        //状态时间
        protected float _stateTime;
        //技能参数
        object _paramObj = null;

        SKILL_EXECMODE _execMode = SKILL_EXECMODE.UNKNOW;
        float _cdTime;

        int _cost = 20;

        List<SkillReusltData> _resultDataList = new List<SkillReusltData>();
        //被其它技能改变的效果
        //Dictionary<> _chgResult =
 
        #endregion
        #region getset
        public int id
        {
            get { return this._config.getIntValue(CFG_SKILL.ID, 0); }
        }

        public SKILL_GESTURE getsure
        {
            get{ return this._config.getEnumValue<SKILL_GESTURE>(CFG_SKILL.GETSURE, SKILL_GESTURE.UNKNOWN);}
        }

        public SKILL_STATUS status
        {
            get { return _status; }
            set
            {
                _status = value;
                _stateTime = Time.time;
                //if (this.attacker != null && this.attacker.BhvBase != null)
                //{
                //    if (_status == SKILL_STATUS.EXECING)
                //        this.attacker.BhvBase.ActiveAi(false);
                //    else
                //        this.attacker.BhvBase.ActiveAi(true);
                //}
            }
        }

        public float statusTime
        {
            get { return Time.time - this._stateTime; }
        }

        public float coldTime
        {
            get { return this._cdTime; }
        }

        //执行时间
        public float duration
        {
            get;
            set;
        }
        
        //攻击模式（主、被动）
        public SKILL_EXECMODE execMode
        {
            get { return this._execMode; }
        }

        //技能发动者
        public Character attacker
        {
            get;
            set;
        }
        //技能等级
        public int level
        {
            get;set;
        }

        //技能伤害系数（和等级有关？）
        public float damageRatio
        {
            get;
            set;
        }

        //暴击概率
        public float critChance
        {
            get;
            set;
        }

        #endregion
        #region public function

        public SkillBase()
        {
            this.status = SKILL_STATUS.IDLE;
        }

        public void release()
        {
            this._paramObj = null;
        }

        public bool init(int nId,int nLevel)
        {
            this._config = FightData.getHeroSkill(nId);
            if (this._config == null)
                return false;
            this.level = nLevel;
            //技能持续时长
            this.duration = this._config.getFloatValue(CFG_SKILL.DURATION, 0f);
            //消耗
            _cost = this._config.getIntValue(CFG_SKILL.COST);
            this._paramObj = null;

            //初始化效果队列
            ConfigRow[] resultList = FightData.getHeroSkillResults(this.id);
            if (resultList == null || resultList.Length == 0)
                return false;

            _cdTime = this._config.getFloatValue(CFG_SKILL.CDTIME, 0f); 
            //执行模式
            this._execMode = this._config.getEnumValue<SKILL_EXECMODE>(CFG_SKILL.MODE, SKILL_EXECMODE.UNKNOW);
            if (this._execMode == SKILL_EXECMODE.UNKNOW)
                return false;
            _resultDataList.Clear();
//            foreach (ConfigRow item in resultList)
//            {
                //SkillReusltData data = new SkillReusltData();
                //data.attacker = this.attacker;
                //data.config = item;
                //data.skillId = nId;
                //data.skillLevel = nLevel;
                //data.init();
                //this._resultDataList.Add(data);
//            }

            if (this._execMode == SKILL_EXECMODE.PASSIVE)//被动技能直接开始执行
            {
                this.status = SKILL_STATUS.EXECING;
            }
            return true;
        }

        public void update()
        {
            if (this.attacker != null)
            {
                if (this.attacker.isDeath())//死亡时不再执行动作
                {
                    this.status = SKILL_STATUS.FINISH;
                }
            }
            if (this._execMode == SKILL_EXECMODE.PASSIVE) //被动技能
            {
                foreach (SkillReusltData item in this._resultDataList)
                {
                    if (Time.time - item.stateTime > item.repeatInterval)
                    {
                        //符合执行条件
                        if (item.checkCondition(this.attacker))
                        {
                            item.onExec(null);
                        }
                    }
                }
            }
            else if(this._execMode == SKILL_EXECMODE.ACTIVE) //主动技能
            {
                switch (status)
                {
                    case SKILL_STATUS.EXECING:
                        if (this.statusTime - this.duration >= 0f)//总时长到时
                        {
                            this._paramObj = null;
                            this.status = SKILL_STATUS.COLDING;
                            foreach (SkillReusltData item in this._resultDataList)
                                item.clear();
                            return;
                        }

                        foreach (SkillReusltData item in this._resultDataList)
                        {
                            if (item.doneTimes == 0 || item.repeatType == SKILL_REUSLT_REPEAT_TYPE.INSKILL)
                            {
                                if (Time.time - item.stateTime > item.repeatInterval)
                                {
                                    item.onExec(this._paramObj);
                                    Debug.Log("repeat:" + item.doneTimes);
                                }
                            }
                        }
                        break;
                    case SKILL_STATUS.COLDING:
                        //需要多次执行寻找对象
                        //动作结束，冷却阶段
                        this.attacker.setAnimatorBool(this._config.getStringValue(CFG_SKILL.ANIMATOR), false);
                        if (this.statusTime - this.coldTime >= 0f)//到时
                            this.status = SKILL_STATUS.IDLE;
                        break;
                    case SKILL_STATUS.IDLE:
                        break;
                    default:
                        break;
                }
            }
        }

        public bool enable()
        {
            if (this.attacker != null)
            {
                //死亡后
                if (this.attacker.isDeath())
                    return false;
                //判断当是否能执行技能
                //if (this.attacker.effectMgr.cantSkill > 0)
                //    return false;
            }
            //判断能量不具备执行条件
            //if (this._cost > CharacterMgr.me.getCurAnger(attacker.side))
            //    return false;
            if (this.status == SKILL_STATUS.FINISH)
                return false;
            return true;
        }

        /*
        执行技能,参数待定
        */
        public bool exec(object param)
        {
            //if (this.execMode != SKILL_EXECMODE.ACTIVE)
            //    return false;
            //if (!enable() || status != SKILL_STATUS.IDLE)
            //    return false;
            //this._paramObj = param as CBattleFightGestureData;
            //this.status = SKILL_STATUS.EXECING;
            ////消耗怒气
            ////CharacterMgr.me.costAnger(attacker.side,(uint)this._cost);
            ////播放技能动画
            //this.attacker.setAnimatorBool(this._config.getStringValue(CFG_SKILL.ANIMATOR), true);
            //this.attacker.showEffect(null,this._config.getStringValue(CFG_SKILL.EFFECT),0);            
            return true;
        }

        #endregion
    }
}
