using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class GameUser {
        public static string csvFilePath = "Configs/GameUser";
        public static string[] columnNameArray = new string[10];
        public static List<GameUser> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<GameUser> dataList = new List<GameUser>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[10];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                GameUser data = new GameUser();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                int.TryParse(csvFile.mapData[i].data[1],out data.loginID);
                columnNameArray [1] = "loginID";
                data.name = csvFile.mapData[i].data[2];
                columnNameArray [2] = "name";
                int.TryParse(csvFile.mapData[i].data[3],out data.level);
                columnNameArray [3] = "level";
                int.TryParse(csvFile.mapData[i].data[4],out data.currentExp);
                columnNameArray [4] = "currentExp";
                int.TryParse(csvFile.mapData[i].data[5],out data.diamond);
                columnNameArray [5] = "diamond";
                int.TryParse(csvFile.mapData[i].data[6],out data.gold);
                columnNameArray [6] = "gold";
                int.TryParse(csvFile.mapData[i].data[7],out data.magic);
                columnNameArray [7] = "magic";
                int.TryParse(csvFile.mapData[i].data[8],out data.regTime);
                columnNameArray [8] = "regTime";
                int.TryParse(csvFile.mapData[i].data[9],out data.lastLoginTime);
                columnNameArray [9] = "lastLoginTime";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static GameUser GetByID (int id,List<GameUser> data)
        {
            foreach (GameUser item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public int loginID;//登陆账号ID
        public string name;//用户名字
        public int level;//等级
        public int currentExp;//当前经验
        public int diamond;//拥有钻石
        public int gold;//拥有金币
        public int magic;//拥有魔法
        public int regTime;//注册时间
        public int lastLoginTime;//上次登录时间
    }
}
