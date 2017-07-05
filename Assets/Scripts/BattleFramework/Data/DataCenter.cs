using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
namespace BattleFramework.Data
{
	public class DataCenter : MonoBehaviour
	{
		static DataCenter instance;
  
		public List<CastleBuildDesign> list_CastleBuildDesign;
		public List<CastleBuildingItems> list_CastleBuildingItems;
		public List<CastleLimits> list_CastleLimits;
		public List<HeroLevelGrowup> list_HeroLevelGrowup;
		public List<Heros> list_Heros;
		public List<PlayerExp> list_PlayerExp;
		public List<Soldier> list_Soldier;
		public List<SoldierProperty> list_SoldierProperty;
		public List<SpellSolution> list_SpellSolution;
		public List<SpellSolutionProperty> list_SpellSolutionProperty;


		//yonghu data
		public List<UserLogin> list_UserLogin;
		public List<GameUser> list_GameUser;
		public List<GameUserData> list_GameUserData;

  
		public static DataCenter SingleTon ()
		{
			if (instance == null) {
				Debug.Log ("new _DataCenter");
				GameObject go = new GameObject ("_DataCenter");
				DataCenter dataCenter = go.AddComponent<DataCenter> ();
				dataCenter.LoadCSV ();
				DontDestroyOnLoad (go);
				instance = dataCenter;
			}
			return instance;
		}
   
   
		public void LoadCSV ()
		{
			list_CastleBuildDesign = CastleBuildDesign.LoadDatas ();
			list_CastleBuildingItems = CastleBuildingItems.LoadDatas ();
			list_CastleLimits = CastleLimits.LoadDatas ();
			list_HeroLevelGrowup = HeroLevelGrowup.LoadDatas ();
			list_Heros = Heros.LoadDatas ();
			list_PlayerExp = PlayerExp.LoadDatas ();
			list_Soldier = Soldier.LoadDatas ();
			list_SoldierProperty = SoldierProperty.LoadDatas ();
			list_SpellSolution = SpellSolution.LoadDatas ();
			list_SpellSolutionProperty = SpellSolutionProperty.LoadDatas ();

			//用户数据   全部加载所有用户
			list_UserLogin = UserLogin.LoadDatas ();
			list_GameUser = GameUser.LoadDatas ();
			list_GameUserData = GameUserData.LoadDatas ();

		}
	}
}
