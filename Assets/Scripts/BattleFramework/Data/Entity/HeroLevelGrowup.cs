using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class HeroLevelGrowup {
        public static string csvFilePath = "Configs/HeroLevelGrowup";
        public static string[] columnNameArray = new string[11];
        public static List<HeroLevelGrowup> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<HeroLevelGrowup> dataList = new List<HeroLevelGrowup>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[11];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                HeroLevelGrowup data = new HeroLevelGrowup();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                int.TryParse(csvFile.mapData[i].data[1],out data.life);
                columnNameArray [1] = "life";
                int.TryParse(csvFile.mapData[i].data[2],out data.attack);
                columnNameArray [2] = "attack";
                int.TryParse(csvFile.mapData[i].data[3],out data.defence);
                columnNameArray [3] = "defence";
                float.TryParse(csvFile.mapData[i].data[4],out data.lifeGrowup);
                columnNameArray [4] = "lifeGrowup";
                float.TryParse(csvFile.mapData[i].data[5],out data.attackGrowup);
                columnNameArray [5] = "attackGrowup";
                float.TryParse(csvFile.mapData[i].data[6],out data.defenceGrowup);
                columnNameArray [6] = "defenceGrowup";
                float.TryParse(csvFile.mapData[i].data[7],out data.reduceDamage);
                columnNameArray [7] = "reduceDamage";
                float.TryParse(csvFile.mapData[i].data[8],out data.moveSpeedAdd);
                columnNameArray [8] = "moveSpeedAdd";
                float.TryParse(csvFile.mapData[i].data[9],out data.attackSpeedAdd);
                columnNameArray [9] = "attackSpeedAdd";
                float.TryParse(csvFile.mapData[i].data[10],out data.reduceSkillCD);
                columnNameArray [10] = "reduceSkillCD";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static HeroLevelGrowup GetByID (int id,List<HeroLevelGrowup> data)
        {
            foreach (HeroLevelGrowup item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//序号
        public int life;//初始生命
        public int attack;//初始攻击
        public int defence;//初始防御
        public float lifeGrowup;//生命成长
        public float attackGrowup;//攻击成长
        public float defenceGrowup;//防御成长
        public float reduceDamage;//伤害免伤
        public float moveSpeedAdd;//移动速度加成（百化）
        public float attackSpeedAdd;//攻击速度加成（百化）
        public float reduceSkillCD;//技能冷却时间缩减（百化）
    }
}
