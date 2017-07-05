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


