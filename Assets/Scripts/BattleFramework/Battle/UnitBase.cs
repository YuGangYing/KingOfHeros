//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;

namespace BattleFramework{
	//the base behaviour for a unit
	[AddComponentMenu("Framework/Unit/UnitBase")]
	public class UnitBase : MonoBehaviour {

		public virtual void OnDamage()
		{

		}

		public virtual bool SearchTarget()
		{
			return false;
		}

	}
}
