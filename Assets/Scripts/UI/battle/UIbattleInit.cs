using UnityEngine;
using System.Collections;
using UI;
using DataMgr;

public class UIbattleInit : MonoBehaviour
{
    protected GameObject battleUICamera;
    public GameObject UIBattlePrefab;
    public GameObject UIResultPrefab;
    BattleUIData data = null; 

	// Use this for initialization
	void Start () {
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

        string strPath = "Prefabs/UI/960X640/Interface3/BattleUI";
        UIBattlePrefab = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPath);

        string strPath2 = "Prefabs/UI/960X640/Interface3/BattleResult";
        UIResultPrefab = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPath2);

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
