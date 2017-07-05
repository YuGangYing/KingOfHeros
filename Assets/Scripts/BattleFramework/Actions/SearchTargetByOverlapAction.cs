//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions{
	
	//Base search target action, default is search by raycast;
	[ActionCategory(ActionCategory.GameLogic)]
	public class SearchTargetByOverlapAction : SearchTargetByRayAction {
		
		public override bool SearchTarget(int layer)
		{
			return Physics.Raycast (Fsm.GameObject.transform.position, Fsm.GameObject.transform.forward, Mathf.Infinity, 1 << layer);
		}
	}
	
}