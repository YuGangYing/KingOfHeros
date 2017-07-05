//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace BattleFramework{
	//this class used to store the variable about unit model,
	//e.g gameObjects renders animations.
	[AddComponentMenu("Framework/Unit/UnitModel")]
	public class UnitModel : MonoBehaviour {

		public GameObject attackPoint;
		FsmGameObject mAttackPoint;

		public void Init(Fsm fsm)
		{
			mAttackPoint = fsm.GetFsmGameObject ("attackPoint");
			if (mAttackPoint != null)
				mAttackPoint.Value = attackPoint;
		}

	}
}
