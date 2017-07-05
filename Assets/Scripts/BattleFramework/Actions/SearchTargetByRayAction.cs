//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions{

	//Base search target action, default is search by raycast;
	[ActionCategory(ActionCategory.GameLogic)]
	public class SearchTargetByRayAction : FsmStateAction {

		[UIHint(UIHint.Variable)]
		public FsmEvent searchedEvent;

		[UIHint(UIHint.Variable)]
		public FsmEvent notSearchedEvent;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat interval;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt targetLayer;

		float mCurrentInterval;

		public override void OnEnter()
		{
			mCurrentInterval = 0;
		}

		public override void OnUpdate()
		{
			if(mCurrentInterval > interval.Value)
			{
				mCurrentInterval = 0;
				//Search
				if(searchedEvent!=null && SearchTarget(targetLayer.Value))
				{
					Fsm.Event(searchedEvent);
				}
				if(notSearchedEvent!=null && !SearchTarget(targetLayer.Value))
				{
					Fsm.Event(notSearchedEvent);
				}

			}
			mCurrentInterval += Time.deltaTime;
		}

		public virtual bool SearchTarget(int layer)
		{
			return Physics.Raycast (Fsm.GameObject.transform.position, Fsm.GameObject.transform.forward, Mathf.Infinity, 1 << layer);
		}
	}

}