using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class SpellSolutionProperty {
        public static string csvFilePath = "Configs/SpellSolutionProperty";
        public static string[] columnNameArray = new string[7];
        public static List<SpellSolutionProperty> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<SpellSolutionProperty> dataList = new List<SpellSolutionProperty>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[7];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                SpellSolutionProperty data = new SpellSolutionProperty();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                int.TryParse(csvFile.mapData[i].data[2],out data.trainGoldCost);
                columnNameArray [2] = "trainGoldCost";
                float.TryParse(csvFile.mapData[i].data[3],out data.totalValue);
                columnNameArray [3] = "totalValue";
                int.TryParse(csvFile.mapData[i].data[4],out data.singleValue);
                columnNameArray [4] = "singleValue";
                int.TryParse(csvFile.mapData[i].data[5],out data.magicCost);
                columnNameArray [5] = "magicCost";
                int.TryParse(csvFile.mapData[i].data[6],out data.timeCost);
                columnNameArray [6] = "timeCost";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static SpellSolutionProperty GetByID (int id,List<SpellSolutionProperty> data)
        {
            foreach (SpellSolutionProperty item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//法术编号ID
        public string name;//名称
        public int trainGoldCost;//训练消耗金币
        public float totalValue;//总值（伤害/治疗/冰冻时间）
        public int singleValue;//单次值（伤害/治疗）
        public int magicCost;//升级需要泉水
        public int timeCost;//需要时间
    }
}
