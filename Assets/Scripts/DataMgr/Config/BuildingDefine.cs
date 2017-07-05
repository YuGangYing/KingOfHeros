using System;
using System.Collections.Generic;

namespace DataMgr
{

    // 建筑类型
    public enum BuildType
    {
        Null,
        LORD_HALL      = 30000,             // 城堡
        MINT           = 30001,             // 金矿
        STONE          = 30002,             // 魔石矿
        WALL           = 30003,             // 城墙
        BARRACKS       = 30004,             // 训练场
        TREASURY       = 30005,             // 宝库
        COVENANT_TOWER = 30006,             // 盟约塔
        CELLAR         = 30007,             // 地窖
        TOWN_HALL      = 30008,             // 城镇大厅钟楼
        PANTHEON       = 30009,             // 先贤祠
        PUB            = 30010,             // 酒馆
        SMITHY         = 30011,             // 铁匠铺
        MONUMENT       = 30012,             // 纪念碑
        HERO_TOWER     = 30013,             // 奥若奇迹
    }

//     public enum BuildType
//     {
//         Null,
//         Castle = 30001,         // 城堡
//         Wall,                   // 城墙
//         College,                // 科技学院
//         Barracks,               //  军营 民房
//         Training,               // 训练场
//         GoldOre,                // 金矿
//         MagicStone,             // 魔石矿
//         Depot,                  // 仓库
//         Blacksmith,             // 铁匠铺
//         Pub,                    // 酒馆
//         HeroTower,              // 英雄塔
//     }

    // 建筑配置数据
    public class BuildConfig
    {
        public int id;              // 建筑ID
        public int name;         // 建筑名
        public int type;            // 类型  1资源建筑，2防御建筑，3军事建筑，4功能建筑，5装饰建筑（旗帜，雕像，花圃，灯具）
        public int text;         // 描述
        public int maxLevel;        // 最大等级
        public int maxCout;         // 最大可建造数量
        public float sizeX;         // 大小
        public float sizeY;
        public bool isMove;         // 是否可移动
        public string icon;         // 图标
        public string prefabs;      // 预制体
        public Pos2D[] pos = new Pos2D[5];
        public Level[] levels;      // 对应的等级数据
    }

    // 建筑
    // 不同等级对应的数据
    public class Level
    {
        public int level; // 当前等级
//         public string modleResInside; // 模型资源名
//         public string modleResOutside; // 模型资源名
//        public string icon; // 建筑图标资源名

        public int mainLevel; // 升级到下一级所需要的城堡等级
        public int money; // 升级所消耗的金钱
        public int magicStone; // 升级所消耗的魔石
        public float time; // 升级所需要的时间

        public int[] data; // 0.仓库表示当前等级的金币存量,金矿魔石矿表示此级资源产出(H),城墙表示箭塔攻击力,训练场表示可训练人数,军营表示可屯兵数量,铁匠铺兵种强化等级上限
        // 1.仓库表示当前等级的魔石存量,金矿魔石矿表示多长时间显示可征收的泡泡(min)，铁匠铺兵种阶级上限
        // 2.金矿魔石最多可产出的时间(H)
    }
    
    public enum CFG_BUILDING
	{
        BUILDING_TYPEID = 0,
        NAME,
        DESCRIPTION,
        CLASSIFY,
        ICON_FILE,
        FBX_FILE,
        MAX_LEVEL,
        SIZE_X,
        SIZE_Y,
        MOVABLE,
        MAX_COUT,
        POSX1,
        POSY1,
        POSX2,
        POSY2,
        POSX3,
        POSY3,
        POSX4,
        POSY4,
        POSX5,
        POSY5
	}

    public enum CFG_BUILDING_LEVEL
    {
        BUIDING_TYPEID = 0,
        LEVEL,
        CASTLE_LEVEL,
        MONEY,
        MAGICSTONE,
        COST_TIME,
        DATA0,
        DATA1,
        DATA2
    }

}
