//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;
using BattleFramework;

public class ShootUtility : MonoBehaviour {

	public void MultiShoot(GameObject shootObject,GameObject spawnPoint,Vector3 shootTargetPos,float shootSpeed,int shootTargetLayer,float intervalPerShoot,int totalShoot)
	{
		StartCoroutine (_MultiShoot(shootObject,spawnPoint,shootTargetPos,shootSpeed,shootTargetLayer,intervalPerShoot,totalShoot));
	}

	IEnumerator _MultiShoot(GameObject shootObject,GameObject spawnPoint,Vector3 shootTargetPos,float shootSpeed,int shootTargetLayer,float intervalPerShoot,int totalShoot)
	{
		int currentShoot = 0;
		while(currentShoot<totalShoot)
		{
			GameObject go = PoolManager.SingleTon().Spawn(shootObject,spawnPoint.transform.position,spawnPoint.transform.rotation);			
			ShootObject so = go.GetComponent<ShootObject>();
			if(so!=null)
			{
//				so.Shoot(gameObject.GetComponent<UnitBase>(),shootTargetPos,shootSpeed,shootTargetLayer);
			}
			else
			{
				PoolManager.SingleTon().UnSpawn(go);
			}
			currentShoot++;
			yield return new WaitForSeconds(intervalPerShoot);
		}
#if UNITY_EDITOR
		Debug.Log("_MultiShoot Done");
#endif
	}
}
