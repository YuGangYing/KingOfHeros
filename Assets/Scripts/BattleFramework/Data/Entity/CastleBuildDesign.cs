using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class CastleBuildDesign {
        public static string csvFilePath = "Configs/CastleBuildDesign";
        public static string[] columnNameArray = new string[6];
        public static List<CastleBuildDesign> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<CastleBuildDesign> dataList = new List<CastleBuildDesign>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[6];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                CastleBuildDesign data = new CastleBuildDesign();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.cName = csvFile.mapData[i].data[1];
                columnNameArray [1] = "cName";
                data.eName = csvFile.mapData[i].data[2];
                columnNameArray [2] = "eName";
                int.TryParse(csvFile.mapData[i].data[3],out data.maxBuildingNUM);
                columnNameArray [3] = "maxBuildingNUM";
                int.TryParse(csvFile.mapData[i].data[4],out data.maxLevel);
                columnNameArray [4] = "maxLevel";
                int.TryParse(csvFile.mapData[i].data[5],out data.castleBuildingBeginID);
                columnNameArray [5] = "castleBuildingBeginID";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static CastleBuildDesign GetByID (int id,List<CastleBuildDesign> data)
        {
            foreach (CastleBuildDesign item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//建筑物编号
        public string cName;//建筑物名字
        public string eName;//建筑物名字
        public int maxBuildingNUM;//最大建造数量
        public int maxLevel;//建筑物最大等级
        public int castleBuildingBeginID;//初始建筑物等级id
    }
}
