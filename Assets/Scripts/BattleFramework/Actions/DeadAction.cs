//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BattleFramework;

namespace HutongGames.PlayMaker.Actions{
	
	[ActionCategory(ActionCategory.GameLogic)]
	public class DeadAction : FsmStateAction 
	{

		public FsmFloat hideDelay;

		public FsmString animStateName;

		public FsmGameObject deadEffect;

		Animation mAnimation;

		float mCurrentDelay;

		public override void Awake()
		{
			if(Application.isPlaying)
			{
				mAnimation = Fsm.GameObject.GetComponent<Animation>();
				if(hideDelay.Value==0 && mAnimation!=null && mAnimation[animStateName.Value]!=null)
				{
					hideDelay.Value = mAnimation[animStateName.Value].length;
				}
			}
		}

		public override void OnEnter()
		{
			if (mAnimation != null)
				mAnimation.Play (animStateName.Value);
			mCurrentDelay = 0;
			if(deadEffect!=null && deadEffect.Value!=null)
			{
				GameObject.Instantiate(deadEffect.Value,Fsm.GameObject.transform.position,Quaternion.identity);
			}
		}

		public override void OnUpdate()
		{
			mCurrentDelay += Time.deltaTime;
			if(mCurrentDelay>=hideDelay.Value)
			{
				Fsm.GameObject.SetActive(false);
			}
		}

	}
}
