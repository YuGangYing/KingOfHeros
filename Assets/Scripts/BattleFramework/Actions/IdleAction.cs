//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BattleFramework;

namespace HutongGames.PlayMaker.Actions{
	
	[ActionCategory(ActionCategory.GameLogic)]
	public class IdleAction : FsmStateAction 
	{
		UnitBase mUnit;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmEvent exitEvent;

		public override void Awake()
		{
			if(Application.isPlaying)
			{
				mUnit = Fsm.GameObject.GetComponent<UnitBase>();
			}
		}

		public override void OnUpdate()
		{
			if(mUnit.SearchTarget())
			{
				Fsm.Event(exitEvent);
			}
		}

	}
}
