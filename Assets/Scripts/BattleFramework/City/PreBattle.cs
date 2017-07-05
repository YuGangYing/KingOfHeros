using UnityEngine;
using UnityEngine.UI;  			//UI命名空间
using UnityEngine.EventSystems;	//事件系统命名空间
using System.Collections;
using System.Collections.Generic;
using BattleFramework.Data;

namespace BattleFramework.City
{
	public class PreBattle : BasePanel
	{
		public UIButton preBattleUIreturn;
		public UIGrid heroInfoGrid;
		public HeroInfo heroInfoPrefab;
		public List<HeroInfo> heroInfoList;//代码添加建筑物属性
	
		// Update is called once per frame
		private  List<Heros> GetHerosList ()
		{
			List<Heros> heroList = DataCenter.SingleTon ().list_Heros;
			return heroList;
		}
		//关闭面板
		private void PreBattleClose ()
		{
			this.gameObject.SetActive (false);
			mCity.isBuildingPanel = false;
		}

		protected override void Init ()
		{
			preBattleUIreturn.onClick.Add (new EventDelegate (PreBattleClose)); //传入方法执行点击事件
			getuser ();
			Debug.Log ("Init heroList");
			List<Heros> heroList = GetHerosList ();

			HeroInfo heroinfo;
			foreach (Heros hero in heroList) {
				GameObject go = Instantiate (heroInfoPrefab.gameObject) as GameObject;
				go.transform.parent = heroInfoGrid.transform;
				Vector3 v3 = new Vector3 (0.8f, 0.8f, 1.0f);
				go.transform.localScale = v3;
				heroinfo = go.GetComponent<HeroInfo> ();
				heroinfo.heroImage.spriteName = hero.itemIcon;
				heroinfo.heroName.text = hero.showName;
				heroinfo.heroLevel.text = Random.Range (10, 20).ToString ();
				heroInfoList.Add (heroinfo);
			}
			heroInfoGrid.Reposition ();
			
		}

		private void getuser ()
		{
			GameUser gu = GameUser.GetByID (1001, DataCenter.SingleTon ().list_GameUser);
			GameUserData gua = GameUserData.GetByID (1000103001, DataCenter.SingleTon ().list_GameUserData);
			foreach (List<int> a in gua.resourceState) {
				foreach (int b in a) {
					Debug.Log (a + "+______________+" + b + "test");
				}

			}

			//得到玩家的建造列表  根据id 获得一条数据  根据选择条件获得n条
			//查询用户数据  



		}





	}
}