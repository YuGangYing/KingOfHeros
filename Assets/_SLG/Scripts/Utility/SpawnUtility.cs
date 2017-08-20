using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fight;
using DataMgr; 

public class SpawnUtility
{
	public static string hero0 = "Prefabs/Heros/Sparta/Sparta_Red";
	public static string soilder0 = "Prefabs/Soldiers/Magic/Magic";
	public static List<string> heroPrefabStr0;
	public static List<string> heroPrefabStr1;
	public static Dictionary<ARMY_TYPE,string> soldierPrefabStr;

	static string GetPlayerHeroPrefab(int i)
	{
		if(heroPrefabStr0==null)
		{
			heroPrefabStr0 = new List<string>();
			heroPrefabStr0.Add("Prefabs/Heros/Sparta/Sparta_Red");
			heroPrefabStr0.Add("Prefabs/Heros/RobinHood/RobinHood_Red");
			heroPrefabStr0.Add("Prefabs/Heros/Richard/Richard_Red");
			heroPrefabStr0.Add("Prefabs/Heros/KingArthur/KingArthur_Red");
			heroPrefabStr0.Add("Prefabs/Heros/CleopatraVII/CleopatraVII_Red");
		}
		return heroPrefabStr0.Count > i ? heroPrefabStr0[i] : null;
	}

	static string GetEnemyHeroPrefab(int i)
	{
		if(heroPrefabStr1==null)
		{
			Debug.Log("test");
			heroPrefabStr1 = new List<string>();
			heroPrefabStr1.Add("Prefabs/Heros/Sparta/Sparta_Blue");
			heroPrefabStr1.Add("Prefabs/Heros/RobinHood/RobinHood_Blue");
			heroPrefabStr1.Add("Prefabs/Heros/Richard/Richard_Blue");
			heroPrefabStr1.Add("Prefabs/Heros/KingArthur/KingArthur_Blue");
			heroPrefabStr1.Add("Prefabs/Heros/CleopatraVII/CleopatraVII_Blue");
		}
		return heroPrefabStr1.Count > i ? heroPrefabStr1[i] : null;
	}

	public static void InitTmpEnemyHeroList()
	{
        //List<HeroSelect> playerHeroList = new List<HeroSelect>();
        List<EnemyHero> tmp_herolist = new List<EnemyHero>();
        EnemyHero hero = new EnemyHero();
        hero.nHeroType = 10102;
        //hero.nHeroType = 1;
        hero.nLocation = 1;
        hero.nSoldierArmyLevel = 1;
        hero.nSoldierLevel = 1;
        hero.nSoldierNum = 16;
        tmp_herolist.Add(hero);

        hero = new EnemyHero();
        hero.nHeroType = 10103;
        //hero.nHeroType = 1;
        hero.nLocation = 2;
        hero.nSoldierArmyLevel = 1;
        hero.nSoldierLevel = 1;
        hero.nSoldierNum = 16;
        tmp_herolist.Add(hero);

        hero = new EnemyHero();
        hero.nHeroType = 10104;
        //hero.nHeroType = 1;
        hero.nLocation = 3;
        hero.nSoldierArmyLevel = 1;
        hero.nSoldierLevel = 1;
        hero.nSoldierNum = 16;
        tmp_herolist.Add(hero);

        hero = new EnemyHero();
        hero.nHeroType = 10109;
        //hero.nHeroType = 1;
        hero.nLocation = 4;
        hero.nSoldierArmyLevel = 1;
        hero.nSoldierLevel = 1;
        hero.nSoldierNum = 16;
        tmp_herolist.Add(hero);

        hero = new EnemyHero();
        hero.nHeroType = 10111;
        //hero.nHeroType = 1;
        hero.nLocation = 5;
        hero.nSoldierArmyLevel = 1;
        hero.nSoldierLevel = 1;
        hero.nSoldierNum = 16;
        tmp_herolist.Add(hero);

        DataManager.getBattleUIData().EnemyHeroList = tmp_herolist;
	} 

    public static void InitTmpPlayerHeroList()
    {
        List<HeroSelect> tmp_herolist = new List<HeroSelect>();
        Dictionary<uint, BattleUIData.FightHeroInfo> dicFightHero = new Dictionary<uint, BattleUIData.FightHeroInfo>();
        
        BattleUIData.FightHeroInfo heroInfo = new BattleUIData.FightHeroInfo();
        heroInfo.armyType = 1;
        heroInfo.icon = "RobinHood_Portrait";
        heroInfo.idHero = 10102;
        dicFightHero.Add(10102,heroInfo);
        HeroSelect hero = new HeroSelect();
        hero.nLocation = 1;
        hero.nHeroId = 10102;
        hero.nSoldierNum = 16;
        tmp_herolist.Add(hero);

        heroInfo = new BattleUIData.FightHeroInfo();
        heroInfo.armyType = 2;
        heroInfo.icon = "Richard_Portrait";
        heroInfo.idHero = 10103;
        dicFightHero.Add(10103,heroInfo);
        hero = new HeroSelect();
        hero.nLocation = 2;
        hero.nHeroId = 10103;
        hero.nSoldierNum = 16;
        tmp_herolist.Add(hero);

        heroInfo = new BattleUIData.FightHeroInfo();
        heroInfo.armyType = 3;
        heroInfo.icon = "Sparta_Portrait";
        heroInfo.idHero = 10104;
        dicFightHero.Add(10104, heroInfo);
        hero = new HeroSelect();
        hero.nLocation = 3;
        hero.nHeroId = 10104;
        hero.nSoldierNum = 16;
        tmp_herolist.Add(hero);

        heroInfo = new BattleUIData.FightHeroInfo();
        heroInfo.armyType = 4;
        heroInfo.icon = "KingArthur_Portrait";
        heroInfo.idHero = 10109;
        dicFightHero.Add(10109, heroInfo);
        hero = new HeroSelect();
        hero.nLocation = 4;
        hero.nHeroId = 10109;
        hero.nSoldierNum = 16;
        tmp_herolist.Add(hero);

        heroInfo = new BattleUIData.FightHeroInfo();
        heroInfo.armyType = 5;
        heroInfo.icon = "CleopatraVII_Portrait";
        heroInfo.idHero = 10111;
        dicFightHero.Add(10111, heroInfo);
        hero = new HeroSelect();
        hero.nLocation = 5;
        hero.nHeroId = 10111;
        hero.nSoldierNum = 16;
        tmp_herolist.Add(hero);

        DataManager.getBattleUIData().HeroList = tmp_herolist;
        DataManager.getBattleUIData().dicFightHero = dicFightHero;
    }


	public static List<BattleMatrix> InitTestMyMatrixList(SIDE side)
	{
		List<BattleMatrix> battleMatrix = new List<BattleMatrix>();
		for(int i=1;i<=5;i++)
		{
			BattleMatrix tmp_matrix = new BattleMatrix(); 
			tmp_matrix.location = i; 

			Fight.Hero hero = new Fight.Hero();
			hero.side = side;
			if(side == SIDE.LEFT)
			{
				hero.strResName = GetPlayerHeroPrefab(i-1);
			}
			else
			{	
				hero.strResName = GetEnemyHeroPrefab(i-1);
			}
			hero.location = i;
			hero.id = i;
			hero.typeid = i;
			hero.baseHp = 1000;
			hero.baseSpeed = 3.5f;
			hero.atkBaseAttackRange = 2;
			hero.armyType = ARMY_TYPE.CAVALRY;
			hero.soldierType = ARMY_TYPE.CAVALRY;
			tmp_matrix.m_Hero = hero;
			for (int j = 0; j < 16; ++j)
			{
				Character character = new Character();
				character.side = side;
				character.strResName = soilder0;
				character.location = i;
				character.id = i;
				character.baseHp = 100;
				character.baseSpeed = 3.5f;
				character.atkBaseAttackRange = 2;
				character.armyType = ARMY_TYPE.CAVALRY;
				tmp_matrix.m_SoldierList.Add(character);
			}
			battleMatrix.Add(tmp_matrix);
		}
		return battleMatrix;
	}
}


