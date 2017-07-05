using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class ResourceConversionValue {
        public static string csvFilePath = "Configs/ResourceConversionValue";
        public static string[] columnNameArray = new string[7];
        public static List<ResourceConversionValue> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            List<ResourceConversionValue> dataList = new List<ResourceConversionValue>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[7];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                ResourceConversionValue data = new ResourceConversionValue();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                int.TryParse(csvFile.mapData[i].data[2],out data.resourceType);
                columnNameArray [2] = "resourceType";
                int.TryParse(csvFile.mapData[i].data[3],out data.resourceRangeMin);
                columnNameArray [3] = "resourceRangeMin";
                int.TryParse(csvFile.mapData[i].data[4],out data.resourceRangeMax);
                columnNameArray [4] = "resourceRangeMax";
                int.TryParse(csvFile.mapData[i].data[5],out data.FormulaParameter_1);
                columnNameArray [5] = "FormulaParameter_1";
                int.TryParse(csvFile.mapData[i].data[6],out data.FormulaParameter_2);
                columnNameArray [6] = "FormulaParameter_2";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static ResourceConversionValue GetByID (int id,List<ResourceConversionValue> data)
        {
            foreach (ResourceConversionValue item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public string name;//名字
        public int resourceType;//资源类型 1金币、2魔法水、3时间（Time单位秒）
        public int resourceRangeMin;//所需资源范围最小值（大于）
        public int resourceRangeMax;//所需资源范围最大值（小于等于）
        public int FormulaParameter_1;//公式参数1 min < 所需资源 < = max  钻石 = （所需资源-min）/参数1 + 参数3
        public int FormulaParameter_2;//公式参数2
    }
}
