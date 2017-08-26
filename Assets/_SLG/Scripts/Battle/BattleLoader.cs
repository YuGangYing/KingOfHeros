using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KOH;

public class BattleLoader : SingleMonoBehaviour<BattleLoader>
{

	protected override void Awake ()
	{
		base.Awake ();
		Init ();
	}

	public GameObject terrainGo;
	public GameObject battleRoot;
	public GameObject battleUIRoot;
	public GameObject battleGlobalSkillEffect;

	void Init ()
	{
		AssetbundleManager.GetInstance.GetAssetbundleFromLocal (ABConstant.SOUND_BGM);
		AssetbundleManager.GetInstance.GetAssetbundleFromLocal (ABConstant.SOUND_SE);
		AssetbundleManager.GetInstance.GetAssetbundleFromLocal (ABConstant.SOUND_BATTLE);
		terrainGo = ResourcesManager.GetInstance.GetBattleTerrain ();
		terrainGo.transform.localEulerAngles = new Vector3 (0, 180, 0);
		terrainGo.transform.localPosition = new Vector3 (0, 0.03f, 0);
		battleGlobalSkillEffect = ResourcesManager.GetInstance.GetBattleGlobalSkillEffect ();
		battleUIRoot = ResourcesManager.GetInstance.GetBattleUIRoot ();
		battleRoot = ResourcesManager.GetInstance.GetBattleRoot ();
	}

}
