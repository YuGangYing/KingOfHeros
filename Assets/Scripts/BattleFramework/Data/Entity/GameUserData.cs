using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class GameUserData {
        public static string csvFilePath = "Configs/GameUserData";
        public static string[] columnNameArray = new string[15];
        public static List<GameUserData> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<GameUserData> dataList = new List<GameUserData>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[15];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                GameUserData data = new GameUserData();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                bool.TryParse(csvFile.mapData[i].data[1],out data.isHaving);
                columnNameArray [1] = "isHaving";
                int.TryParse(csvFile.mapData[i].data[2],out data.userID);
                columnNameArray [2] = "userID";
                int.TryParse(csvFile.mapData[i].data[3],out data.buildingTypeID);
                columnNameArray [3] = "buildingTypeID";
                data.buildingName = csvFile.mapData[i].data[4];
                columnNameArray [4] = "buildingName";
                int.TryParse(csvFile.mapData[i].data[5],out data.buildingTypeNUM);
                columnNameArray [5] = "buildingTypeNUM";
                int.TryParse(csvFile.mapData[i].data[6],out data.buildingItemID);
                columnNameArray [6] = "buildingItemID";
                bool.TryParse(csvFile.mapData[i].data[7],out data.isBuilding);
                columnNameArray [7] = "isBuilding";
                int.TryParse(csvFile.mapData[i].data[8],out data.beginBuildingTime);
                columnNameArray [8] = "beginBuildingTime";
                int.TryParse(csvFile.mapData[i].data[9],out data.predictEndingTime);
                columnNameArray [9] = "predictEndingTime";
                data.buildingPosition= new Vector3();
                strs = csvFile.mapData[i].data[10].Split(new char[1]{','});
                    data.buildingPosition.x = (float.Parse(strs[0]));
                    data.buildingPosition.y = (float.Parse(strs[1]));
                    data.buildingPosition.z = (float.Parse(strs[2]));
                columnNameArray [10] = "buildingPosition";
                data.resourceState= new List<List<int>>();
                strs = csvFile.mapData[i].data[11].Split(new char[1]{';'});
                for(int j=0;j<strs.Length;j++){
                      listChild = new List<int>();
                      strsTwo = strs[j].Split(new char[1]{','});
                      for(int m=0;m<strsTwo.Length;m++){
                            listChild.Add(int.Parse(strsTwo[m]));
                      }
                    data.resourceState.Add(listChild);
                }
                columnNameArray [11] = "resourceState";
                bool.TryParse(csvFile.mapData[i].data[12],out data.isOutput);
                columnNameArray [12] = "isOutput";
                int.TryParse(csvFile.mapData[i].data[13],out data.beginProduceTime);
                columnNameArray [13] = "beginProduceTime";
                data.productList= new List<List<int>>();
                strs = csvFile.mapData[i].data[14].Split(new char[1]{';'});
                for(int j=0;j<strs.Length;j++){
                      listChild = new List<int>();
                      strsTwo = strs[j].Split(new char[1]{','});
                      for(int m=0;m<strsTwo.Length;m++){
                            listChild.Add(int.Parse(strsTwo[m]));
                      }
                    data.productList.Add(listChild);
                }
                columnNameArray [14] = "productList";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static GameUserData GetByID (int id,List<GameUserData> data)
        {
            foreach (GameUserData item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号主键
        public bool isHaving;//是否拥有1拥有 0 没有
        public int userID;//用户ID
        public int buildingTypeID;//建筑物编号
        public string buildingName;//建筑物编号名字
        public int buildingTypeNUM;//建筑物编号编号序号
        public int buildingItemID;//建筑物具体ID
        public bool isBuilding;//是否在建造状态
        public int beginBuildingTime;//开始建造时间
        public int predictEndingTime;//预计结束时间
        public Vector3 buildingPosition;//建筑物位置
        public List<List<int>> resourceState;//库存状态(士兵id,num）或者（法术id,num）
        public bool isOutput;//是否在生产状态
        public int beginProduceTime;//生产开始时间
        public List<List<int>> productList;//生产列表
    }
}
