using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class CastleLimits {
        public static string csvFilePath = "Configs/CastleLimits";
        public static string[] columnNameArray = new string[32];
        public static List<CastleLimits> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<CastleLimits> dataList = new List<CastleLimits>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[32];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                CastleLimits data = new CastleLimits();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                bool.TryParse(csvFile.mapData[i].data[1],out data.isOpen);
                columnNameArray [1] = "isOpen";
                int.TryParse(csvFile.mapData[i].data[2],out data.barrackID);
                columnNameArray [2] = "barrackID";
                int.TryParse(csvFile.mapData[i].data[3],out data.barrackNUM);
                columnNameArray [3] = "barrackNUM";
                int.TryParse(csvFile.mapData[i].data[4],out data.barrackMaxLEvel);
                columnNameArray [4] = "barrackMaxLEvel";
                int.TryParse(csvFile.mapData[i].data[5],out data.campsiteID);
                columnNameArray [5] = "campsiteID";
                int.TryParse(csvFile.mapData[i].data[6],out data.campsiteNUM);
                columnNameArray [6] = "campsiteNUM";
                int.TryParse(csvFile.mapData[i].data[7],out data.campsiteMaxLEvel);
                columnNameArray [7] = "campsiteMaxLEvel";
                int.TryParse(csvFile.mapData[i].data[8],out data.goldmineID);
                columnNameArray [8] = "goldmineID";
                int.TryParse(csvFile.mapData[i].data[9],out data.goldmineNUM);
                columnNameArray [9] = "goldmineNUM";
                int.TryParse(csvFile.mapData[i].data[10],out data.goldmineMaxLEvel);
                columnNameArray [10] = "goldmineMaxLEvel";
                int.TryParse(csvFile.mapData[i].data[11],out data.magicSpringID);
                columnNameArray [11] = "magicSpringID";
                int.TryParse(csvFile.mapData[i].data[12],out data.magicSpringNUM);
                columnNameArray [12] = "magicSpringNUM";
                int.TryParse(csvFile.mapData[i].data[13],out data.magicSpringMaxLEvel);
                columnNameArray [13] = "magicSpringMaxLEvel";
                int.TryParse(csvFile.mapData[i].data[14],out data.savingsPotID);
                columnNameArray [14] = "savingsPotID";
                int.TryParse(csvFile.mapData[i].data[15],out data.savingsPotNUM);
                columnNameArray [15] = "savingsPotNUM";
                int.TryParse(csvFile.mapData[i].data[16],out data.savingsPotMaxLEvel);
                columnNameArray [16] = "savingsPotMaxLEvel";
                int.TryParse(csvFile.mapData[i].data[17],out data.magicBottleID);
                columnNameArray [17] = "magicBottleID";
                int.TryParse(csvFile.mapData[i].data[18],out data.magicBottleNUM);
                columnNameArray [18] = "magicBottleNUM";
                int.TryParse(csvFile.mapData[i].data[19],out data.magicBottleMaxLEvel);
                columnNameArray [19] = "magicBottleMaxLEvel";
                int.TryParse(csvFile.mapData[i].data[20],out data.heroCampsiteID);
                columnNameArray [20] = "heroCampsiteID";
                int.TryParse(csvFile.mapData[i].data[21],out data.heroCampsiteNUM);
                columnNameArray [21] = "heroCampsiteNUM";
                int.TryParse(csvFile.mapData[i].data[22],out data.heroCampsiteMaxLEvel);
                columnNameArray [22] = "heroCampsiteMaxLEvel";
                int.TryParse(csvFile.mapData[i].data[23],out data.laboratoryID);
                columnNameArray [23] = "laboratoryID";
                int.TryParse(csvFile.mapData[i].data[24],out data.laboratoryNUM);
                columnNameArray [24] = "laboratoryNUM";
                int.TryParse(csvFile.mapData[i].data[25],out data.laboratoryMaxLEvel);
                columnNameArray [25] = "laboratoryMaxLEvel";
                int.TryParse(csvFile.mapData[i].data[26],out data.spellFactoryID);
                columnNameArray [26] = "spellFactoryID";
                int.TryParse(csvFile.mapData[i].data[27],out data.spellFactoryNUM);
                columnNameArray [27] = "spellFactoryNUM";
                int.TryParse(csvFile.mapData[i].data[28],out data.spellFactoryMaxLEvel);
                columnNameArray [28] = "spellFactoryMaxLEvel";
                int.TryParse(csvFile.mapData[i].data[29],out data.workerHouseID);
                columnNameArray [29] = "workerHouseID";
                int.TryParse(csvFile.mapData[i].data[30],out data.workerHouseNUM);
                columnNameArray [30] = "workerHouseNUM";
                int.TryParse(csvFile.mapData[i].data[31],out data.workerHouseMaxLEvel);
                columnNameArray [31] = "workerHouseMaxLEvel";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static CastleLimits GetByID (int id,List<CastleLimits> data)
        {
            foreach (CastleLimits item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//城堡等级
        public bool isOpen;//对应等级是否开放
        public int barrackID;//兵营编号
        public int barrackNUM;//兵营限制数量
        public int barrackMaxLEvel;//兵营限制等级
        public int campsiteID;//营地编号
        public int campsiteNUM;//营地限制数量
        public int campsiteMaxLEvel;//营地限制等级
        public int goldmineID;//金矿编号
        public int goldmineNUM;//金矿限制数量
        public int goldmineMaxLEvel;//金矿限制等级
        public int magicSpringID;//魔法泉编号
        public int magicSpringNUM;//魔法泉限制数量
        public int magicSpringMaxLEvel;//魔法泉限制等级
        public int savingsPotID;//储金罐编号
        public int savingsPotNUM;//储金罐限制数量
        public int savingsPotMaxLEvel;//储金罐限制等级
        public int magicBottleID;//魔法瓶编号
        public int magicBottleNUM;//魔法瓶限制数量
        public int magicBottleMaxLEvel;//魔法瓶限制等级
        public int heroCampsiteID;//英雄营地编号
        public int heroCampsiteNUM;//英雄营地限制数量
        public int heroCampsiteMaxLEvel;//英雄营地限制等级
        public int laboratoryID;//实验室编号
        public int laboratoryNUM;//实验室限制数量
        public int laboratoryMaxLEvel;//实验室限制等级
        public int spellFactoryID;//法术工厂编号
        public int spellFactoryNUM;//法术工厂限制数量
        public int spellFactoryMaxLEvel;//法术工厂限制等级
        public int workerHouseID;//工人木屋编号
        public int workerHouseNUM;//工人木屋限制数量
        public int workerHouseMaxLEvel;//工人木屋限制等级
    }
}
