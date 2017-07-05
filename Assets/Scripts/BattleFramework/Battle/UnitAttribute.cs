//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;

namespace BattleFramework{
	public enum UnitType{Plant,Zombie};
	[AddComponentMenu("Framework/Unit/UnitAttribute")]
	public class UnitAttribute : MonoBehaviour {

		public int lineIndex;
		public UnitType type;
		public float damage;
		public float health;

		public float OnDamage(UnitAttribute attacker)
		{
			health = Mathf.Max(0,health - GetDamage (attacker));
			return health;
		}

		//TODO need multiple attribute;
		float GetDamage(UnitAttribute attacker)
		{
			return Mathf.Abs(attacker.damage);
		}
	}
}
