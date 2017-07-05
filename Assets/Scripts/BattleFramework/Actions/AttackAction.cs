//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;
using BattleFramework;

namespace HutongGames.PlayMaker.Actions{
	
	[ActionCategory(ActionCategory.GameLogic)]
	public class AttackAction : FsmStateAction{ 

		[RequiredField]
		public FsmGameObject attackPoint;
        //if base on animation, the variable attackDur,minAttackInterval,maxAttackInterval has not effect.
        public FsmBool baseOnAnimation;
        [CheckForComponent(typeof(Animation))]
        public FsmOwnerDefault gameObject;
        protected Animation anim;

        [RequiredField]
        public FsmFloat effectTime;//real damage time ,0.0~1.0.during in attackDur;

        public FsmFloat attackDur;//duration per attack,not include interval,usually based on attack animation;
		public FsmFloat minAttackInterval;
		public FsmFloat maxAttackInterval;
		public FsmEvent attackDoneEvent;
		public FsmBool loop;//Loop attack or one attack

		protected float mCurrentAttackDur;
		protected bool mAttacked;
		protected float mLastTime;

		public override void Awake()
		{
			if(Application.isPlaying)
			{
                if(baseOnAnimation.Value)
                {
                    anim = gameObject.GameObject.Value.GetComponent<Animation>();
                }
                else
                {
                    minAttackInterval.Value = Mathf.Min(minAttackInterval.Value,maxAttackInterval.Value);
                    maxAttackInterval.Value = Mathf.Max(minAttackInterval.Value,maxAttackInterval.Value);
                    mCurrentInterval = Random.Range(minAttackInterval.Value,maxAttackInterval.Value);
                }
                effectTime.Value = Mathf.Clamp(effectTime.Value,0,1);
                mCurrentAttackDur = 0;
                mAttacked = false;
            }
        }

		public override void OnEnter()
		{
            if(baseOnAnimation.Value)
            {
                mAttacked = false;
            }
            //TODO
            else if (mCurrentAttackDur + Time.time - mLastTime > attackDur.Value + mCurrentInterval) {
				mAttacked = false;
				mLastTime = Time.time;
                mCurrentAttackDur = 0;
			}
		}

		float mCurrentInterval;
		public override void OnUpdate()
		{
			mLastTime = Time.time;
			mCurrentAttackDur += Time.deltaTime;
			if(mCurrentAttackDur >= attackDur.Value + mCurrentInterval)
			{
				if(loop.Value)
				{
					mCurrentAttackDur =0;
					mCurrentInterval = Random.Range(minAttackInterval.Value,maxAttackInterval.Value);
					mAttacked = false;
				}
				else
				{
					if(attackDoneEvent!=null)
						Fsm.Event(attackDoneEvent);
				}
			}
			if(!mAttacked && mCurrentAttackDur>=effectTime.Value*attackDur.Value)
			{
				mAttacked = true;
				OnAttack();
			}
		}

		public virtual void OnAttack()
		{
			Debug.Log ("OnAttack");
		}
	}
}
