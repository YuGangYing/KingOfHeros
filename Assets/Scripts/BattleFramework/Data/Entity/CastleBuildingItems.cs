using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class CastleBuildingItems {
        public static string csvFilePath = "Configs/CastleBuildingItems";
        public static string[] columnNameArray = new string[9];
        public static List<CastleBuildingItems> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<CastleBuildingItems> dataList = new List<CastleBuildingItems>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[9];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                CastleBuildingItems data = new CastleBuildingItems();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                int.TryParse(csvFile.mapData[i].data[2],out data.level);
                columnNameArray [2] = "level";
                int.TryParse(csvFile.mapData[i].data[3],out data.goldCost);
                columnNameArray [3] = "goldCost";
                int.TryParse(csvFile.mapData[i].data[4],out data.magicCost);
                columnNameArray [4] = "magicCost";
                int.TryParse(csvFile.mapData[i].data[5],out data.timeCost);
                columnNameArray [5] = "timeCost";
                int.TryParse(csvFile.mapData[i].data[6],out data.numericalAttributes);
                columnNameArray [6] = "numericalAttributes";
                int.TryParse(csvFile.mapData[i].data[7],out data.getPlayeExp);
                columnNameArray [7] = "getPlayeExp";
                data.buildingART = csvFile.mapData[i].data[8];
                columnNameArray [8] = "buildingART";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static CastleBuildingItems GetByID (int id,List<CastleBuildingItems> data)
        {
            foreach (CastleBuildingItems item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//城堡建筑物id
        public string name;//建筑物名字
        public int level;//建筑物等级
        public int goldCost;//需要金币
        public int magicCost;//需要泉水
        public int timeCost;//需要时间
        public int numericalAttributes;//数值属性
        public int getPlayeExp;//获得玩家经验表
        public string buildingART;//美术资源
    }
}
