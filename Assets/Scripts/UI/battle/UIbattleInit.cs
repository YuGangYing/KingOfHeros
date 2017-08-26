using UnityEngine;
using System.Collections;
using UI;
using DataMgr;
using KOH;

public class UIbattleInit : SingleMonoBehaviour<UIbattleInit>
{
    protected GameObject battleUICamera;
    GameObject UIBattlePrefab;
    GameObject UIResultPrefab;
    BattleUIData data = null; 

	// Use this for initialization
	void Start () {
   
	}

	public void Init(){
		battleUICamera = UISoldierPanel.findChild(gameObject, "Camera");
		GameObject UIBattle = UISoldierPanel.findChild(battleUICamera, "BattleUI");
		GameObject UIResult = UISoldierPanel.findChild(battleUICamera, "BattleResult");
		if (UIBattle != null)
		{
			NGUITools.Destroy(UIBattle);
		}
		if (UIResult != null)
		{
			NGUITools.Destroy(UIResult);
		}
		//        string strPath = "Prefabs/UI/960X640/Interface/BattleUI";
		//        UIBattlePrefab = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPath);
		UIBattlePrefab = ResourcesManager.GetInstance.GetUIInterface("BattleUI");

		//        string strPath2 = "Prefabs/UI/960X640/Interface/BattleResult";
		//        UIResultPrefab = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPath2);
		UIResultPrefab = ResourcesManager.GetInstance.GetUIInterface("BattleResult");
		UIBattle = NGUITools.AddChild(battleUICamera, UIBattlePrefab);
		UIBattle.name = "BattleUI";
		UIResult = NGUITools.AddChild(battleUICamera, UIResultPrefab);
		UIResult.name = "BattleResult";
		data = DataManager.getBattleUIData();
		if (data != null)
		{
			data.battleResult.init(UIResult);
			data.battlePanel.init(UIBattle);
		}
	}

	// Update is called once per frame
	void Update () {
        if (data != null)
        {
            DataManager.getBattleUIData().BattleUIUpdate();
        }
	}
}
