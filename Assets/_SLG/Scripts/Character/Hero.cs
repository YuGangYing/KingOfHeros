using System;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
namespace Fight
{
    public class Hero : Character
	{
        //领导力
        int _nBaseLead;
        int _nCurLead;

        //带士兵数量
        int _soldierNum;
        //在服务端ID
        int _servId;
        //SkillMgr _skillMgr = new SkillMgr(); 

        //
        int _quality = 0;
        //英雄怒气值
        float _angerTime = 0f;
        //
        ARMY_TYPE _soldierType = ARMY_TYPE.UNKOWN;

        //暴击
        private float m_Critical;

        private int m_iNature;

        public Hero()
        { 
            //this._skillMgr.hero = this;
            this.armyType = ARMY_TYPE.HERO;
        }

        //更新怒气
        void updateAnger()
        {
            //if (isDeath() || !CharacterMgr.me.isBegin ||CharacterMgr.me.isEnd)
            //    return;
            if (this._angerTime == 0f)
            {
                this._angerTime = Time.time;
                return;
            }
            if (Time.time - this._angerTime < 5.0f)//5秒更新一次
                return;
            this._angerTime = Time.time;
            int nRatio = 0;
            switch (this._quality)
            {
                case 0: //白卡
                    nRatio = 0;
                    break;
                case 1://铜卡
                    nRatio = 1;
                    break;
                case 2://银卡
                    nRatio = 2;
                    break;
                case 3://金卡
                    nRatio = 3;
                    break;
            }
            //能量成长（1次/5秒） = 10+3*英雄品质系数+1*英雄星级
//            int anger = 10 + 3 * nRatio + this.star;
           // CharacterMgr.me.addAnger(this.side,(uint)anger);
        }

        protected override void initRoot()
        { 
            this._animator = _root.GetComponent<Animator>();
            if (this._animator == null)
                this._animator = _root.GetComponentInChildren<Animator>();
        }

        public override void update()
        {
            base.update();

            //_skillMgr.update(); 
            //更新怒气值
            updateAnger();
        }

        public int Nature
        {
            get { return m_iNature; }
            set { m_iNature = value; }
        }

        public float Critical
        {
            get { return m_Critical; }
            set { m_Critical = value;}
        }
        public ARMY_TYPE soldierType
        {
            get { return _soldierType; }
            set { _soldierType = value; }
        }

        public int soldierNum
        {
            get { return _soldierNum; }
            set { _soldierNum = value; }
        }

        public int servId
        {
            get { return this._servId; }
            set { this._servId = value; }
        }

        public int baseLead
        {
            get { return this._nBaseLead; }
            set { this._nBaseLead = value; }
        }

        public int curLead
        {
            get { return this._nCurLead; }
            set { this._nCurLead = value; }
        }

        public int star
        {
            get;
            set;
        }

        public int typeid
        {
            get;
            set;
        } 
       
        public override void release()
        {
            //this._skillMgr.release();
        }
	}
}
