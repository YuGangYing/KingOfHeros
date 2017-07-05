using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions{

	[ActionCategory(ActionCategory.GameLogic)]
	public class MoveAction : FsmStateAction 
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 moveTarget;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat moveSpeed;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat stopDistance;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmEvent reachEvent;

		public override void OnUpdate()
		{
			Fsm.GameObject.transform.LookAt (moveTarget.Value);
			Fsm.GameObject.transform.Translate(Vector3.forward * moveSpeed.Value);
			if(reachEvent!=null && Vector3.Distance(Fsm.GameObject.transform.position,moveTarget.Value) <= stopDistance.Value)
			{
				Fsm.Event(reachEvent);
			}
		}


	}
}
