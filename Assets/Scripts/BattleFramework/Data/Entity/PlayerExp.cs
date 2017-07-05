using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class PlayerExp {
        public static string csvFilePath = "Configs/PlayerExp";
        public static string[] columnNameArray = new string[2];
        public static List<PlayerExp> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<PlayerExp> dataList = new List<PlayerExp>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[2];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                PlayerExp data = new PlayerExp();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                int.TryParse(csvFile.mapData[i].data[1],out data.playerExp);
                columnNameArray [1] = "playerExp";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static PlayerExp GetByID (int id,List<PlayerExp> data)
        {
            foreach (PlayerExp item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//玩家等级
        public int playerExp;//升级所需城堡经验
    }
}
