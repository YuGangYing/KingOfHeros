using UnityEngine;
using UnityEngine.UI;  			//UI命名空间
using UnityEngine.EventSystems;	//事件系统命名空间
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;


namespace BattleFramework.Data
{
	public class ReadWriteDataClass:MonoBehaviour
	{
		private string appDBPath = @"Assets/Resources/GameDB/slgGame.db";
		private DbAccess db;

		public static UserLogin backUserLogin;
		public static GameUser  backGameUser;
		public static GameUserData[] backGameUserDataList;

		//登陆
		public bool  CheckDBAndTables (string name, string pwd)
		{
			DataCenter dc = DataCenter.SingleTon ();
			bool backFunctionValue = true;
			if (!File.Exists (appDBPath)) {
				db = new DbAccess (@"Data Source=" + appDBPath);  //连接数据库  不存在则创建
				Debug.Log ("create DB complete");
				CreateTables ();	//输入哪个数据库创建
			} 
			//测试登录数据
			db = new DbAccess (@"Data Source=" + appDBPath);  //连接数据库  不存在则创建
			backUserLogin = ReadUserLogin (name, pwd);
			if (backUserLogin == null) {
				Debug.Log ("用户不存在或者密码错误，请联系客服小妹哈！！ 或者注册新用户");
				backFunctionValue = false;
			} else {
				backGameUser = ReadGameUser (backUserLogin.id);
				if (backGameUser == null) { //查找不存在  需要初始化用户
					Debug.Log ("登陆成功，我们开始初始化玩家数据~~~~");
					InitGameUser (backUserLogin.id);
					Debug.Log ("始化玩家数据完成~~~~");

					backGameUser = ReadGameUser (backUserLogin.id);//获取初始列表


					Debug.Log ("开始初始化玩家的建筑物数据~~~~");
					InitGameUserData (backGameUser.id);
					Debug.Log ("初始化玩家的建筑物数据完成~~~~");

					backGameUserDataList = ReadGameUserData (backGameUser.id);
				} else {
					//读取之前要检查数据是否需要扩充gameUserData    如建筑物数量的增加、等级的增加  等等



					//读取用户数据
					Debug.Log ("玩家登陆成功，开始读取游戏数据~~~~");
					backGameUser = ReadGameUser (backUserLogin.id);//获取初始列表
					backGameUserDataList = ReadGameUserData (backGameUser.id);
					Debug.Log ("读取游戏数据完成~~~~");

					//更新用户数据情况
					backUserLogin.createTime = (int)GetTimestamp.GetNowTimestamp ();
					UpdateUserLogin (backUserLogin);
				}
			}

			return backFunctionValue;
		}

		//注册
		public bool  CheckDBAndTables (int type, string name, string pwd)
		{
			DataCenter dc = DataCenter.SingleTon ();
			bool backFunctionValue = true;

			if (!File.Exists (appDBPath)) {
				db = new DbAccess (@"Data Source=" + appDBPath);  //连接数据库  不存在则创建
				Debug.Log ("create DB complete");
				CreateTables ();	//输入哪个数据库创建
			} 
			//测试登录数据
			db = new DbAccess (@"Data Source=" + appDBPath);  //连接数据库  不存在则创建

			InitUserLogin (type, name, pwd); //初始化注册用户
			backUserLogin = ReadUserLogin (name, pwd);
			if (backUserLogin == null) {
				Debug.Log ("注册失败，请联系工作人员");
			} else {
				Debug.Log ("登陆成功，我们开始初始化玩家数据~~~~");
				InitGameUser (backUserLogin.id);
				Debug.Log ("始化玩家数据完成~~~~");
				backGameUser = ReadGameUser (backUserLogin.id);//获取初始列表
				Debug.Log ("开始初始化玩家的建筑物数据~~~~");
				InitGameUserData (backGameUser.id);
				Debug.Log ("初始化玩家的建筑物数据完成~~~~");
				backGameUserDataList = ReadGameUserData (backGameUser.id);
			}	

			return backFunctionValue;
		}

		//更新游戏数据
		public bool UpdateData (GameUser gameUser, GameUserData[] gameUserDataArray)
		{
			bool value = true;

			UpdateGameUser (gameUser);
			UpdateGameUserDataArray (gameUserDataArray);
			return value;
		}



		//创建数据表结构
		void CreateTables ()
		{
			CreateTableFunction ("userLogin", UserLogin.columnNameArray);
			CreateTableFunction ("gameUser", GameUser.columnNameArray);
			CreateTableFunction ("gameUserData", GameUserData.columnNameArray);
			Debug.Log ("table struct completed");

		}
		//创建玩家的数据库以及玩家数据表
		void CreateTableFunction (string tableName, string[] columnNameArray)
		{
			string[] tableFieldName = columnNameArray;//列名
			int len = columnNameArray.Length;
			string[] tableFieldType = new string[len];//列数据类型
			for (int i =0; i<len; i++) {
				tableFieldType [i] = "TEXT";
			}
			db.CreateTable (tableName, tableFieldName, tableFieldType);
		}

		//-------------------------------------------------------------------------------------------------------
		//读取登陆函数表   目的是为了得到uid
		UserLogin ReadUserLogin (string name, string pwd)
		{
			UserLogin myuserLogin = new UserLogin ();
			string tableName = "userLogin";
			string[] columnName = UserLogin.columnNameArray;
			string[] chazhao = {columnName [1],columnName [2]};
			string[] guanxi = {"=","="};
			string[] value = {name,pwd};

			string[] getValue = new string[columnName.Length];
			bool havingGetData = false;
			using (SqliteDataReader sqReader = db.SelectWhere (tableName, columnName, chazhao, guanxi, value)) {
				Debug.Log ("read_begin_yaozhen__");
				while (sqReader.Read()) { 
					havingGetData = true;
					//Debug.Log ("yaozhen__" + sqReader.GetString (sqReader.GetOrdinal ("id")));
					/*
					Debug.Log ("yaozhen__" + sqReader.GetString (1));	
					Debug.Log ("yaozhen__" + sqReader.GetString (2));		
					Debug.Log ("yaozhen__" + sqReader.GetString (0));	
					Debug.Log ("yaozhen__" + sqReader.GetString (3));	
					Debug.Log ("yaozhen__" + sqReader.GetString (4));	
					*/
					for (int i = 0; i<columnName.Length; i++) {
						Debug.Log ("yaozhen__" + columnName [i] + "__" + sqReader.GetString (i));	
						getValue [i] = sqReader.GetString (i);
					}
				}
			}
			if (havingGetData == false) {//没有查找到需要的数据
				myuserLogin = null;
			} else {
				int indexSecond = 0;
				myuserLogin.id = int.Parse (getValue [indexSecond++]);
				myuserLogin.userName = getValue [indexSecond++];
				myuserLogin.passWord = getValue [indexSecond++];
				myuserLogin.status = DataFunctionClass.GetBoolValue (getValue [indexSecond++]);
				myuserLogin.createTime = int.Parse (getValue [indexSecond++]);
			}
			return myuserLogin;
		}



		//读取用户数据函数  gameUser
		GameUser ReadGameUser (int loginID)
		{
			GameUser mygameuser = new GameUser ();
			string loginid = loginID.ToString ();
			string tableName = "gameUser";
			string[] columnName = GameUser.columnNameArray;
			string[] chazhao = {columnName [1]};
			string[] guanxi = {"="};
			string[] value = {loginid};
			
			string[] getValue = new string[columnName.Length];
			bool havingGetData = false;
			using (SqliteDataReader sqReader = db.SelectWhere (tableName, columnName, chazhao, guanxi, value)) {
				Debug.Log ("read_begin_yaozhen__");
				while (sqReader.Read()) { 
					havingGetData = true;
					for (int i = 0; i<columnName.Length; i++) {
						Debug.Log ("yaozhen__" + columnName [i] + "__" + sqReader.GetString (i));	
						getValue [i] = sqReader.GetString (i);
					}
					
				}
			}

			if (havingGetData == false) {
				mygameuser = null;
			} else {
				int indexSecond = 0;
				mygameuser.id = int.Parse (getValue [indexSecond++]);
				mygameuser.loginID = int.Parse (getValue [indexSecond++]);
				mygameuser.name = getValue [indexSecond++];
				mygameuser.level = int.Parse (getValue [indexSecond++]);
				mygameuser.currentExp = int.Parse (getValue [indexSecond++]);
				mygameuser.diamond = int.Parse (getValue [indexSecond++]);
				mygameuser.gold = int.Parse (getValue [indexSecond++]);
				mygameuser.magic = int.Parse (getValue [indexSecond++]);
				mygameuser.regTime = int.Parse (getValue [indexSecond++]);
				mygameuser.lastLoginTime = int.Parse (getValue [indexSecond++]);
			}
			return mygameuser;
		}

		//读取用户信息   gameUserData
		GameUserData[] ReadGameUserData (int userid)
		{
			List<GameUserData> mygameuserlist = new List<GameUserData> ();
			string uid = userid.ToString ();
			string tableName = "gameUserData";
			string[] columnName = GameUserData.columnNameArray;
			string[] chazhao = {columnName [2]};
			string[] guanxi = {"="};
			string[] value = {uid};
			int index = 0;
	
			string[] getValue;
			GameUserData mygameuserdata;
			using (SqliteDataReader sqReader = db.SelectWhere (tableName, columnName, chazhao, guanxi, value)) {
				while (sqReader.Read()) { 
					getValue = new string[columnName.Length];
					mygameuserdata = new GameUserData ();
					for (int i = 0; i<columnName.Length; i++) {
						getValue [i] = sqReader.GetString (i);
					}
					int indexSecond = 0;
					//初始化返回的第一条数据
					mygameuserdata.id = int.Parse (getValue [indexSecond++]);
					mygameuserdata.isHaving = DataFunctionClass.GetBoolValue (getValue [indexSecond++]);
					mygameuserdata.userID = int.Parse (getValue [indexSecond++]);
					mygameuserdata.buildingTypeID = int.Parse (getValue [indexSecond++]);
					mygameuserdata.buildingName = getValue [indexSecond++];
					mygameuserdata.buildingTypeNUM = int.Parse (getValue [indexSecond++]);
					mygameuserdata.buildingItemID = int.Parse (getValue [indexSecond++]);
					mygameuserdata.isBuilding = DataFunctionClass.GetBoolValue (getValue [indexSecond++]);
					mygameuserdata.beginBuildingTime = int.Parse (getValue [indexSecond++]);
					mygameuserdata.predictEndingTime = int.Parse (getValue [indexSecond++]);
					mygameuserdata.buildingPosition = DataFunctionClass.GetVector3Value (getValue [indexSecond++]);
					mygameuserdata.resourceState = DataFunctionClass.GetListListIntValue (getValue [indexSecond++]);
					mygameuserdata.isOutput = DataFunctionClass.GetBoolValue (getValue [indexSecond++]);
					mygameuserdata.beginProduceTime = int.Parse (getValue [indexSecond++]);
					mygameuserdata.productList = DataFunctionClass.GetListListIntValue (getValue [indexSecond++]);
					//加入到list中
					mygameuserlist.Add (mygameuserdata);
				}
			}

			GameUserData[] backGameUserData = new GameUserData[mygameuserlist.Count];

			foreach (GameUserData ga in mygameuserlist) {
				backGameUserData [index] = ga;
				index++;
			}

			return backGameUserData;

		}
		//-----------------------------------初始化GameUser----------------------------------------------------
		void InitUserLogin (int type, string name, string pwd)
		{
			string tableName = "userLogin";
			string[] columnName = UserLogin.columnNameArray;
			string[] columnValue = new string[columnName.Length];
			int index = 0;
			columnValue [index++] = Random.Range (1, 9999).ToString ();//id
			columnValue [index++] = name.ToString ();//userName
			columnValue [index++] = pwd.ToString ();//passWord
			columnValue [index++] = "1";//status
			columnValue [index++] = GetTimestamp.GetNowTimestamp ().ToString ();//createTime
			
			//db.InsertInto ("momo", new string[]{ "'宣雨松'","'289187120'","'xuanyusong@gmail.com'","'www.xuanyusong.com'"   });
			using (SqliteDataReader sqReader = db.InsertIntoSpecific (tableName, columnName, columnValue)) {
				
			}
		}


		//-----------------------------------初始化GameUser----------------------------------------------------
		void InitGameUser (int id)
		{
			string tableName = "gameUser";
			string[] columnName = GameUser.columnNameArray;
			string[] columnValue = new string[columnName.Length];

			int index = 0;
			columnValue [index++] = (101000000 + id * 100).ToString ();//id
			columnValue [index++] = id.ToString ();//loginID
			columnValue [index++] = "我是姚大爷";//+ id.ToString ();//name
			columnValue [index++] = "1";//level
			columnValue [index++] = "0";//currentExp
			columnValue [index++] = "10000";//ownDiamond
			columnValue [index++] = "1000";//ownGold
			columnValue [index++] = "1000";//magic
			columnValue [index++] = GetTimestamp.GetNowTimestamp ().ToString ();//regTime
			columnValue [index++] = GetTimestamp.GetNowTimestamp ().ToString ();//lastLoginTime

			//db.InsertInto ("momo", new string[]{ "'宣雨松'","'289187120'","'xuanyusong@gmail.com'","'www.xuanyusong.com'"   });
			using (SqliteDataReader sqReader = db.InsertIntoSpecific (tableName, columnName, columnValue)) {
		
			}
		}

		//-----------------------------------初始化GameUserData-----------------------------------------
		void InitGameUserData (int id)
		{
			string tableName = "gameUserData";
			string[] columnName = GameUserData.columnNameArray;
			string[] columnValue;
			//获取所有玩家的建筑序列表
			List<CastleBuildDesign> buildingList = DataCenter.SingleTon ().list_CastleBuildDesign;

			foreach (CastleBuildDesign CBD in buildingList) {
				string CBDidText = CBD.id.ToString ();
				string type = CBDidText.Substring (4, 2);
				int CBDbuingNUM = CBD.maxBuildingNUM;

				for (int i =0; i<CBDbuingNUM; i++) {
					columnValue = new string[columnName.Length];
					string listNum = (1000 + i + 1).ToString ().Substring (1, 3);
					//初始化的行数
					int index = 0;
					columnValue [index++] = (10000 + (id % 1000000) / 100).ToString () + type + listNum;//id  01建筑物类型   001 第一个
					columnValue [index++] = "0";//isHaving
					columnValue [index++] = (id).ToString ();//userID
					columnValue [index++] = CBDidText; //buildingTypeID
					columnValue [index++] = CBD.cName + (i + 1).ToString () + "号";//buildingTypeName
					columnValue [index++] = (i + 1).ToString ();//buildingTypeNUM
					columnValue [index++] = (CBD.castleBuildingBeginID).ToString ();//buildingItemID
					columnValue [index++] = "0";//isBuilding
					columnValue [index++] = "0";//beginBuildingTime
					columnValue [index++] = "0";//predictEndingTime
					columnValue [index++] = "0,0,0";//buildingPosition
					columnValue [index++] = "0";//resoureState
					columnValue [index++] = "0";//isOutput
					columnValue [index++] = "0";//beginProduceTime
					columnValue [index++] = "0";//productList

					db.InsertIntoSpecific (tableName, columnName, columnValue);
				}


			}
	
		}


		//------------------------------更新userLogin-------------------------------

		void UpdateUserLogin (UserLogin userLogin)
		{
			string tableName = "userLogin";
			string[] columnName = UserLogin.columnNameArray;
			string[] columnValue = new string[columnName.Length];
			string selectKey = columnName [0];//列名
			string selectValue = userLogin.id.ToString ();//值名
			int index = 0;
			columnValue [index++] = selectValue;//id
			columnValue [index++] = userLogin.userName;//loginID
			columnValue [index++] = userLogin.passWord;//+ passWord
			columnValue [index++] = DataFunctionClass.GetBoolText (userLogin.status);//status
			columnValue [index++] = userLogin.createTime.ToString ();//createTime

			db.UpdateInto (tableName, columnName, columnValue, selectKey, selectValue);
		}

		//------------------------------更新gameUser-------------------------------

		void UpdateGameUser (GameUser gameUser)
		{
			string tableName = "gameUser";
			string[] columnName = GameUser.columnNameArray;
			string[] columnValue = new string[columnName.Length];
			string selectKey = columnName [1];//loginID
			string selectValue = gameUser.loginID.ToString ();//值名
			int index = 0;
			columnValue [index++] = gameUser.id.ToString ();//id
			columnValue [index++] = selectValue;//loginID
			columnValue [index++] = gameUser.name;// name
			columnValue [index++] = gameUser.level.ToString ();//level
			columnValue [index++] = gameUser.currentExp.ToString ();//currentExp
			columnValue [index++] = gameUser.diamond.ToString ();//diamond
			columnValue [index++] = gameUser.gold.ToString ();//gold
			columnValue [index++] = gameUser.magic.ToString ();//magic
			columnValue [index++] = gameUser.regTime.ToString ();//regTime
			columnValue [index++] = gameUser.lastLoginTime.ToString ();//lastLoginTime

			db.UpdateInto (tableName, columnName, columnValue, selectKey, selectValue);
		}


		//------------------------------更新gameUserData-------------------------------
		
		void UpdateGameUserData (GameUserData gameUserData)
		{
			string tableName = "gameUserData";
			string[] columnName = GameUserData.columnNameArray;
			string[] columnValue = new string[columnName.Length];
			string selectKey = columnName [0];//id
			string selectValue = gameUserData.id.ToString ();//值
			int index = 0;
			columnValue [index++] = gameUserData.id.ToString ();//id
			columnValue [index++] = DataFunctionClass.GetBoolText (gameUserData.isHaving);//isHaving
			columnValue [index++] = gameUserData.userID.ToString ();// userID
			columnValue [index++] = gameUserData.buildingTypeID.ToString ();//buildingTypeID
			columnValue [index++] = gameUserData.buildingName;//buildingName
			columnValue [index++] = gameUserData.buildingTypeNUM.ToString ();//buildingTypeNUM
			columnValue [index++] = gameUserData.buildingItemID.ToString ();//buildingItemID
			columnValue [index++] = DataFunctionClass.GetBoolText (gameUserData.isBuilding);//isBuilding
			columnValue [index++] = gameUserData.beginBuildingTime.ToString ();//beginBuildingTime
			columnValue [index++] = gameUserData.predictEndingTime.ToString ();//predictEndingTime
			columnValue [index++] = DataFunctionClass.GetVector3Text (gameUserData.buildingPosition);//buildingPosition
			columnValue [index++] = DataFunctionClass.GetListListIntText (gameUserData.resourceState);//resourceState
			columnValue [index++] = DataFunctionClass.GetBoolText (gameUserData.isOutput);//isOutput
			columnValue [index++] = gameUserData.beginProduceTime.ToString ();//beginProduceTime
			columnValue [index++] = DataFunctionClass.GetListListIntText (gameUserData.productList);//productList
			
			db.UpdateInto (tableName, columnName, columnValue, selectKey, selectValue);
		}

		void UpdateGameUserDataArray (GameUserData[] gameUserDataArray)
		{
			for (int i = 0; i<gameUserDataArray.Length; i++) {

				UpdateGameUserData (gameUserDataArray [i]);
			}
		}




	}
	
}