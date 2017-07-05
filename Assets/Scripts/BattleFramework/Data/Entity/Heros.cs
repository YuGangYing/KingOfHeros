using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Heros {
        public static string csvFilePath = "Configs/Heros";
        public static string[] columnNameArray = new string[18];
        public static List<Heros> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<Heros> dataList = new List<Heros>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[18];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Heros data = new Heros();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                bool.TryParse(csvFile.mapData[i].data[1],out data.isOpen);
                columnNameArray [1] = "isOpen";
                data.showName = csvFile.mapData[i].data[2];
                columnNameArray [2] = "showName";
                int.TryParse(csvFile.mapData[i].data[3],out data.unitType);
                columnNameArray [3] = "unitType";
                int.TryParse(csvFile.mapData[i].data[4],out data.lifeType);
                columnNameArray [4] = "lifeType";
                int.TryParse(csvFile.mapData[i].data[5],out data.attackType);
                columnNameArray [5] = "attackType";
                int.TryParse(csvFile.mapData[i].data[6],out data.damageType);
                columnNameArray [6] = "damageType";
                int.TryParse(csvFile.mapData[i].data[7],out data.armorType);
                columnNameArray [7] = "armorType";
                int.TryParse(csvFile.mapData[i].data[8],out data.moveSpeed);
                columnNameArray [8] = "moveSpeed";
                int.TryParse(csvFile.mapData[i].data[9],out data.attackDistance);
                columnNameArray [9] = "attackDistance";
                float.TryParse(csvFile.mapData[i].data[10],out data.attackSpeed);
                columnNameArray [10] = "attackSpeed";
                int.TryParse(csvFile.mapData[i].data[11],out data.maxLevel);
                columnNameArray [11] = "maxLevel";
                int.TryParse(csvFile.mapData[i].data[12],out data.growupID);
                columnNameArray [12] = "growupID";
                data.itemIcon = csvFile.mapData[i].data[13];
                columnNameArray [13] = "itemIcon";
                data.descrption = csvFile.mapData[i].data[14];
                columnNameArray [14] = "descrption";
                data.prefab = csvFile.mapData[i].data[15];
                columnNameArray [15] = "prefab";
                int.TryParse(csvFile.mapData[i].data[16],out data.beginSkill);
                columnNameArray [16] = "beginSkill";
                int.TryParse(csvFile.mapData[i].data[17],out data.passivitySkill);
                columnNameArray [17] = "passivitySkill";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Heros GetByID (int id,List<Heros> data)
        {
            foreach (Heros item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//序号
        public bool isOpen;//是否投放
        public string showName;//屏幕显示名称
        public int unitType;//单位类型
        public int lifeType;//类型
        public int attackType;//攻击方式对空对地
        public int damageType;//伤害类型
        public int armorType;//护甲类型
        public int moveSpeed;//移动速度
        public int attackDistance;//攻击距离
        public float attackSpeed;//攻击速度
        public int maxLevel;//最大等级
        public int growupID;//成长
        public string itemIcon;//头像
        public string descrption;//描述
        public string prefab;//prefab资源
        public int beginSkill;//主动技能
        public int passivitySkill;//被动技能
    }
}
