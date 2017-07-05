using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class SpellSolution {
        public static string csvFilePath = "Configs/SpellSolution";
        public static string[] columnNameArray = new string[10];
        public static List<SpellSolution> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<SpellSolution> dataList = new List<SpellSolution>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[10];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                SpellSolution data = new SpellSolution();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                int.TryParse(csvFile.mapData[i].data[2],out data.needSpellFactoryLevel);
                columnNameArray [2] = "needSpellFactoryLevel";
                float.TryParse(csvFile.mapData[i].data[3],out data.radius);
                columnNameArray [3] = "radius";
                float.TryParse(csvFile.mapData[i].data[4],out data.randomRadius);
                columnNameArray [4] = "randomRadius";
                int.TryParse(csvFile.mapData[i].data[5],out data.NUM);
                columnNameArray [5] = "NUM";
                float.TryParse(csvFile.mapData[i].data[6],out data.interval);
                columnNameArray [6] = "interval";
                int.TryParse(csvFile.mapData[i].data[7],out data.TrainingTime);
                columnNameArray [7] = "TrainingTime";
                int.TryParse(csvFile.mapData[i].data[8],out data.maxLevel);
                columnNameArray [8] = "maxLevel";
                int.TryParse(csvFile.mapData[i].data[9],out data.spellSolutionBeginID);
                columnNameArray [9] = "spellSolutionBeginID";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static SpellSolution GetByID (int id,List<SpellSolution> data)
        {
            foreach (SpellSolution item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//法术药水编号
        public string name;//名称
        public int needSpellFactoryLevel;//需要法术工厂等级
        public float radius;//半径
        public float randomRadius;//随机半径
        public int NUM;//数量
        public float interval;//释放间隔时间
        public int TrainingTime;//训练时间
        public int maxLevel;//最大等级
        public int spellSolutionBeginID;//初始法术等级ID
    }
}
