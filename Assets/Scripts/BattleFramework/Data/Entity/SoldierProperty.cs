using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class SoldierProperty {
        public static string csvFilePath = "Configs/SoldierProperty";
        public static string[] columnNameArray = new string[10];
        public static List<SoldierProperty> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<SoldierProperty> dataList = new List<SoldierProperty>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[10];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                SoldierProperty data = new SoldierProperty();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                data.soldier = csvFile.mapData[i].data[2];
                columnNameArray [2] = "soldier";
                data.needLaboratoryLevel = csvFile.mapData[i].data[3];
                columnNameArray [3] = "needLaboratoryLevel";
                int.TryParse(csvFile.mapData[i].data[4],out data.trainMagicCost);
                columnNameArray [4] = "trainMagicCost";
                data.healthProperty = csvFile.mapData[i].data[5];
                columnNameArray [5] = "healthProperty";
                data.attackValue = csvFile.mapData[i].data[6];
                columnNameArray [6] = "attackValue";
                int.TryParse(csvFile.mapData[i].data[7],out data.magicCost);
                columnNameArray [7] = "magicCost";
                int.TryParse(csvFile.mapData[i].data[8],out data.oilCost);
                columnNameArray [8] = "oilCost";
                int.TryParse(csvFile.mapData[i].data[9],out data.timeCost);
                columnNameArray [9] = "timeCost";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static SoldierProperty GetByID (int id,List<SoldierProperty> data)
        {
            foreach (SoldierProperty item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//士兵编号ID
        public string name;//名字
        public string soldier;//士兵等级
        public string needLaboratoryLevel;//需要实验室等级
        public int trainMagicCost;//训练消耗泉水
        public string healthProperty;//生命值
        public string attackValue;//攻击力
        public int magicCost;//升级需要泉水
        public int oilCost;//升级需要黑油
        public int timeCost;//需要时间
    }
}
