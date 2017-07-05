//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace BattleFramework{

	[AddComponentMenu("Framework/Unit/UnitBehaviour")]
	public class UnitBehaviour : MonoBehaviour {

		public string shootPrefabPath;//base on resoures

		//these variables use to transform the value to actions;
#if PublicDebug
		public FsmFloat mSearchInterval;
		public FsmInt mTargetLayer;
		public FsmFloat mMinAttackInterval;
		public FsmFloat mMaxAttackInterval;
		public FsmGameObject mShootObject;
		public FsmFloat mShootSpeed;
		public FsmVector3 mShootTargetPos;
		public FsmFloat mAttackDuration;
		public FsmFloat mEffectTime;
		public FsmFloat mIntervalPerShoot;
		public FsmInt mShootCount;

		public FsmVector3 mMoveTargetPos;
		public FsmFloat mMoveSpeed;

#else
		FsmFloat mSearchInterval;
		FsmInt mTargetLayer;
		FsmFloat mMinAttackInterval;
		FsmFloat mMaxAttackInterval;
		FsmGameObject mShootObject;
		FsmFloat mShootSpeed;
		FsmVector3 mShootTargetPos;
		FsmFloat mAttackDuration;
		FsmFloat mEffectTime;
		FsmFloat mIntervalPerShoot;
		FsmInt mShootCount;
#endif
		public UnitController unit;

		//TODO
		public void Init(Fsm fsm)
		{
			mSearchInterval = fsm.GetFsmFloat ("searchInterval");
			if (mSearchInterval != null)
				mSearchInterval.Value = 2;

			mTargetLayer = fsm.GetFsmInt ("enemyLayer");
			if (mTargetLayer != null)
				mTargetLayer.Value = unit.attr.type == UnitType.Plant ? Configs.zombieLayer : Configs.plantLayer;

			mMinAttackInterval = fsm.GetFsmFloat ("minAttackInterval");
			if (mMinAttackInterval != null)
				mMinAttackInterval.Value = 0.1f;

			mMaxAttackInterval = fsm.GetFsmFloat ("maxAttackInterval");
			if (mMaxAttackInterval != null)
				mMaxAttackInterval.Value = 0.2f;

			mShootObject = fsm.GetFsmGameObject ("shootObject");
			if (mShootObject != null)
				mShootObject.Value = Resources.Load(shootPrefabPath) as GameObject;

			mShootSpeed = fsm.GetFsmFloat ("shootSpeed");
			if (mShootSpeed != null)
				mShootSpeed.Value = 9;

			mShootTargetPos = fsm.GetFsmVector3 ("shootTargetPos");
			if (mShootTargetPos != null)
				mShootTargetPos.Value = transform.position + new Vector3(0,0,20);

			mAttackDuration = fsm.GetFsmFloat ("attackDuration");
			if (mAttackDuration != null)
				mAttackDuration.Value = 0.6f;

			mEffectTime = fsm.GetFsmFloat ("effectTime");
			if (mEffectTime != null)
				mEffectTime.Value = 0.6f;

			mIntervalPerShoot = fsm.GetFsmFloat ("intervalPerShoot");
			if (mIntervalPerShoot != null)
				mIntervalPerShoot.Value = 0.06f;

			mShootCount = fsm.GetFsmInt ("shootCount");
			if (mShootCount != null)
				mShootCount.Value = 20;

		}
	}
}