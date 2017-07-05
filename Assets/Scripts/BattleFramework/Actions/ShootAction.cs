//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;
using BattleFramework;

//the sub action of attackAction;
namespace HutongGames.PlayMaker.Actions{
	
	[ActionCategory(ActionCategory.GameLogic)]
	public class ShootAction : AttackAction 
	{
		[RequiredField]
		public FsmGameObject shootObject;

		[RequiredField]
		public FsmFloat shootSpeed;

		public FsmGameObject shootTarget;

		public FsmVector3 shootTargetPos;

		[RequiredField]
		public FsmInt shootTargetLayer;	

		public override void OnAttack()
		{
			Debug.Log ("OnAttack");
			if (shootObject != null && shootObject.Value != null) 
			{
				GameObject go = PoolManager.SingleTon().Spawn(shootObject.Value,attackPoint.Value.transform.position,attackPoint.Value.transform.rotation);			
				ShootObject so = go.GetComponent<ShootObject>();
				if(so!=null)
				{
//					so.Shoot(Fsm.GameObject.GetComponent<UnitBase>(),shootTargetPos.Value,shootSpeed.Value,shootTargetLayer.Value);
				}
				else
				{
					PoolManager.SingleTon().UnSpawn(go);
				}
			}
		}
	}
	
}
