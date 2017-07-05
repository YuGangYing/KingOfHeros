using UnityEngine;
using System.Collections;
using BattleFramework;

//the sub action of attackAction;
namespace HutongGames.PlayMaker.Actions{
	public class MultiShootAction : ShootAction {

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat intervalPerShoot;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt totalShoot;

		ShootUtility mShootUtility;
		UnitBase mUnit;
		public override void OnEnter()
		{
			base.OnEnter ();
			attackDur.Value = Mathf.Max (attackDur.Value, intervalPerShoot.Value * totalShoot.Value) * 3;
			if(mUnit==null)
				mUnit = Fsm.GameObject.GetComponent<UnitBase>();
			if(mShootUtility==null)
				mShootUtility = mUnit.AddOrGetComponent<ShootUtility>();
		}

		public override void OnAttack()
		{
			Debug.Log ("OnAttack");
			if (shootObject != null && shootObject.Value != null) 
			{
				mShootUtility.MultiShoot(shootObject.Value,attackPoint.Value,shootTargetPos.Value,shootSpeed.Value,shootTargetLayer.Value,intervalPerShoot.Value,totalShoot.Value);
			}
		}
	}
}