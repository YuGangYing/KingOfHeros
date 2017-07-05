using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class UserLogin {
        public static string csvFilePath = "Configs/UserLogin";
        public static string[] columnNameArray = new string[5];
        public static List<UserLogin> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<UserLogin> dataList = new List<UserLogin>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[5];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                UserLogin data = new UserLogin();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.userName = csvFile.mapData[i].data[1];
                columnNameArray [1] = "userName";
                data.passWord = csvFile.mapData[i].data[2];
                columnNameArray [2] = "passWord";
                bool.TryParse(csvFile.mapData[i].data[3],out data.status);
                columnNameArray [3] = "status";
                int.TryParse(csvFile.mapData[i].data[4],out data.createTime);
                columnNameArray [4] = "createTime";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static UserLogin GetByID (int id,List<UserLogin> data)
        {
            foreach (UserLogin item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号(自增长）
        public string userName;//用户名字
        public string passWord;//用户密码
        public bool status;//状态(1在线 0不在线)
        public int createTime;//创建时间
    }
}
