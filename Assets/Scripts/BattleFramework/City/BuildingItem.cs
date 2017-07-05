using UnityEngine;
using System.Collections;

namespace BattleFramework.City
{
	public class BuildingItem : MonoBehaviour
	{

		public UIEventTrigger buildTrigger;
		public string buildName;
		[HideInInspector]public BuildingPanel buildingPanel;
		public UISprite buildImage;
		public UILabel  itemName;
		public UILabel  goldCost;
		public UILabel  magicCost;
		public UILabel  timeCost;

		void Awake ()
		{
			buildTrigger.onClick.Add (new EventDelegate (Click));
		}

		void Click ()
		{
			buildingPanel.CreateBuilding (buildName);
		}

	}
}
