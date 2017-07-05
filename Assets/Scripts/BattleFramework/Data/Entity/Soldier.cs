using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Soldier {
        public static string csvFilePath = "Configs/Soldier";
        public static string[] columnNameArray = new string[15];
        public static List<Soldier> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<Soldier> dataList = new List<Soldier>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[15];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Soldier data = new Soldier();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                int.TryParse(csvFile.mapData[i].data[2],out data.accountPopulation);
                columnNameArray [2] = "accountPopulation";
                int.TryParse(csvFile.mapData[i].data[3],out data.needBarrackLevel);
                columnNameArray [3] = "needBarrackLevel";
                int.TryParse(csvFile.mapData[i].data[4],out data.unitType);
                columnNameArray [4] = "unitType";
                int.TryParse(csvFile.mapData[i].data[5],out data.lifeType);
                columnNameArray [5] = "lifeType";
                int.TryParse(csvFile.mapData[i].data[6],out data.attackType);
                columnNameArray [6] = "attackType";
                int.TryParse(csvFile.mapData[i].data[7],out data.damageType);
                columnNameArray [7] = "damageType";
                int.TryParse(csvFile.mapData[i].data[8],out data.armorType);
                columnNameArray [8] = "armorType";
                int.TryParse(csvFile.mapData[i].data[9],out data.moveSpeed);
                columnNameArray [9] = "moveSpeed";
                int.TryParse(csvFile.mapData[i].data[10],out data.TrainingTime);
                columnNameArray [10] = "TrainingTime";
                int.TryParse(csvFile.mapData[i].data[11],out data.attackDistance);
                columnNameArray [11] = "attackDistance";
                float.TryParse(csvFile.mapData[i].data[12],out data.attackSpeed);
                columnNameArray [12] = "attackSpeed";
                int.TryParse(csvFile.mapData[i].data[13],out data.maxLevel);
                columnNameArray [13] = "maxLevel";
                int.TryParse(csvFile.mapData[i].data[14],out data.soldierBeginID);
                columnNameArray [14] = "soldierBeginID";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Soldier GetByID (int id,List<Soldier> data)
        {
            foreach (Soldier item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public string name;//名字
        public int accountPopulation;//所占人口
        public int needBarrackLevel;//所需兵营等级
        public int unitType;//单位类型
        public int lifeType;//分类
        public int attackType;//攻击类型
        public int damageType;//伤害类型
        public int armorType;//护甲类型
        public int moveSpeed;//移动速度
        public int TrainingTime;//训练时间
        public int attackDistance;//攻击距离
        public float attackSpeed;//攻击速度
        public int maxLevel;//最大等级
        public int soldierBeginID;//初始士兵等级ID
    }
}
