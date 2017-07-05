//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

namespace BattleFramework{

	[ExecuteInEditMode]
	[RequireComponent(typeof(UnitAttribute))]
	[RequireComponent(typeof(UnitBehaviour))]
	[RequireComponent(typeof(UnitModel))]
	[AddComponentMenu("Framework/Unit/UnitController")]
	public class UnitController : UnitBase {

		public UnitAttribute attr;
		public UnitBehaviour behaviour;
		public UnitModel model;
		public PlayMakerFSM pm;

		// Use this for initialization
		void Awake () {
			pm = GetComponent<PlayMakerFSM> ();
			attr = this.AddOrGetComponent<UnitAttribute>();
			behaviour = this.AddOrGetComponent<UnitBehaviour>();
			model = this.AddOrGetComponent<UnitModel>();
			behaviour.unit = this;
			behaviour.Init (pm.Fsm);
			model.Init (pm.Fsm);
		}

        void Update()
        {
            if(Application.isPlaying)
            {
#if UNITY_EDITOR
                if(Input.GetKeyDown(KeyCode.A))
                {
                    pm.SendEvent("OnAttack");
                }
#endif
            }
        }


		public void OnSkill()
		{
			pm.Fsm.Event ("OnSkill");	
			Debug.Log ("OnSkill");
		}


		public override bool SearchTarget(){
			return SearchTargetByRayCast ();
		}

		bool SearchTargetByRayCast()
		{
			Vector3 targetRayPos = BattleController.SingleTon().GetPlantShootTargetPosByLine (attr.lineIndex).transform.position;
			targetRayPos.y = transform.position.y;
			int layer = attr.type == UnitType.Plant ?  Configs.zombieLayer : Configs.plantLayer;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, targetRayPos - transform.position,out hit, Mathf.Infinity, 1 << layer))
			{
				return true;
			}
#if UNITY_EDITOR
			Debug.DrawRay (transform.position, targetRayPos - transform.position,Color.red);
#endif
			return false;
		}

		//hited by attacker;
		public void OnDamage(UnitBase attacker)
		{
			Debug.Log ("OnDamage");
			float health = attr.OnDamage (((UnitController)attacker).attr);
			if(health <= 0 && pm.Fsm.ActiveStateName!="Dead")
			{
				pm.Fsm.Event("OnDead");
			}
		}
	}
}
