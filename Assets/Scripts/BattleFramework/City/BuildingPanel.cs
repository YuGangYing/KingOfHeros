using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BattleFramework.Data;

namespace BattleFramework.City
{
	public class BuildingPanel : BasePanel
	{

		public UIButton BuildingUIreturn;
		public UIGrid buildingGrid;
		public BuildingItem buildingItemPrebfa;
		public List<BuildingItem> buildingItems;//代码添加建筑物属性

		private CastleBuildingItems[] GetBuildingsList ()
		{
			List<CastleBuildDesign> CBDList = DataCenter.SingleTon ().list_CastleBuildDesign;
			List<CastleBuildingItems> CBDITEMList = DataCenter.SingleTon ().list_CastleBuildingItems;
			int i = 0, j = 0;
			int CBDCOUNT = CBDList.Count, CBDITEMCOUNT = CBDITEMList.Count;
			int[] buildingsIDList = new int[CBDCOUNT];
			CastleBuildingItems[] buildingsListItem = new CastleBuildingItems[CBDCOUNT];

			if (CBDList == null) {
				Debug.Log ("fan hui zhi wei null");
			} else {
				Debug.Log ("get CBDLIST" + CBDCOUNT);
				foreach (CastleBuildDesign CBD in CBDList) {
					buildingsIDList [i] = CBD.castleBuildingBeginID;
					i++;
				}
			}

			if (CBDITEMList == null) {
				Debug.Log ("fan hui zhi wei null null null");
			} else {
				Debug.Log ("get CBDLISTITEM" + CBDITEMCOUNT);
				CastleBuildingItems CBDI;
				for (j=0; j<CBDCOUNT; j++) {
					CBDI = CastleBuildingItems.GetByID (buildingsIDList [j], CBDITEMList);
					if (CBDI == null) {
						Debug.Log ("cha xun icon cuo wu ");
						break;
					}
					buildingsListItem [j] = CBDI;
				}
			}
			return buildingsListItem;
		}
		protected override void Init ()
		{
			Debug.Log ("Init buildingsList");
			CastleBuildingItems[] buildingsListItem = GetBuildingsList ();
			//for (int i =0; i<buildingsList.Length; i++) {
			//	Debug.LogError (buildingsList [i] + "0000000000000000");
			//}

			Debug.Log ("Init");
			BuildingUIreturn.onClick.Add (new EventDelegate (CloseBuildingUIreturn)); //传入方法执行点击事件

			BuildingItem item;
			for (int i=0; i<buildingsListItem.Length; i++) {
				GameObject go = Instantiate (buildingItemPrebfa.gameObject) as GameObject;
				go.transform.parent = buildingGrid.transform;
				go.transform.localScale = Vector3.one;
				item = go.GetComponent<BuildingItem> ();
				item.buildingPanel = this;
				item.buildName = buildingsListItem [i].buildingART;
				item.buildImage.spriteName = item.buildName + "_Render";
				item.itemName.text = buildingsListItem [i].name;
				item.goldCost.text = buildingsListItem [i].goldCost.ToString ();
				item.magicCost.text = buildingsListItem [i].magicCost.ToString ();
				item.timeCost.text = buildingsListItem [i].timeCost.ToString ();
				buildingItems.Add (item);
			}
			buildingGrid.Reposition ();
		}

		private void CloseBuildingUIreturn ()
		{
			this.gameObject.SetActive (false);
			mCity.isBuildingPanel = false;
		}

		public  void CreateBuilding (string name)
		{
			//begin building 
			Transform AreaCenter = mCity.clickGround.transform.GetChild (0);
			if (AreaCenter.childCount == 0) {
				Vector3 GroundPosition = AreaCenter.position; //得到ground世界坐标 
				string source = "Prefabs/Buildings/" + name;
				Object ogo = Resources.Load (source, typeof(Object));
				GameObject go = Instantiate (ogo) as GameObject;  //实例化
				
				go.transform.position = GroundPosition;
				go.transform.parent = AreaCenter;

				CloseBuildingUIreturn ();//关闭面板

			} else {
				print ("this is having a child");
			}

		}

	}
}