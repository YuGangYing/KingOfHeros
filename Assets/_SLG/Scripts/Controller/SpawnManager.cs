using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fight;
using DataMgr;

 
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
//方阵数据结构 
public class BattleMatrix
{
	//方阵位置 
	public int location;

	//英雄  
	public Fight.Hero m_Hero;

	//士兵列表 
	public List<Character> m_SoldierList = new List<Character>();
}

[RequireComponent(typeof(BattleController))]
public class SpawnManager : MonoBehaviour 
{ 
	public static SpawnManager instance;
	public static SpawnManager SingleTon()
	{
		return instance;
	} 
	 
	public bool IsSingleMode = false;
	public float RowPadding = 2; 
	public float ColumnPadding = 2;    

	public Color PlayerColor;
	public Color EnemyColor;

	public List<Transform> PlayerWayPointLine0;
	public List<Transform> PlayerWayPointLine1;
	public List<Transform> PlayerWayPointLine2;

	public Transform[] PlayerSpawnPoints;
	public Transform[] EnemySpawnPoints;

	public Camera MainCamera;
	public Camera NGUICamera;
	public GameObject OverlayPrefab;

	private List<BattleMatrix> m_MyMatrixList = new List<BattleMatrix>(); 
	public UnitMatrix[,] PlayerMatrixs = new UnitMatrix[3,3];
	public UnitMatrix[,] EnemyMatrixs = new UnitMatrix[3,3];
	private List<BattleMatrix> m_EnemyMatrixList = new List<BattleMatrix>(); 
	public Dictionary<int, Transform> m_MyDicLocation = new Dictionary<int, Transform>();  
	public Dictionary<int, Transform> m_EnemyDicLocation = new Dictionary<int, Transform>();
	public Dictionary<int, int> m_DicMatrixId = new Dictionary<int, int>();
  
	public List<Unit> PlayerHeros = new List<Unit>();
	public List<Unit> EnemyHeros = new List<Unit>();

	public List<Fight.Hero> HeroList; 
	public AudioClip BattleStartClip;

	#region skill resources
	public delegate void OnKingSacrifice(Unit unit);
	public OnKingSacrifice onKingSacrifice;

	public delegate void OnRemoveHero(Unit unit);
	public OnRemoveHero onRemoveHero;

	public int Skill1;
	public int Skill2;
	public int Skill3;

	public GameObject BaseCastSkillEffect;

	public GameObject ShootObject;
	public GameObject ShootObject1;
	public GameObject ShootObject2;
	public GameObject ShootObject3;
	public Pool ArrowPool;
	public Pool MagicBallPool;
	public Pool HeroSOPool0;
	public Pool HeroSOPool1;

	#endregion

	void Awake()
	{
		if(instance==null)instance=this;
		//InstantiateMatirxs();
		//if(!IsSingleMode)
        //   GetHeroInfo();//TODO
		//LoadResource();
		InitPools();
	}

	void InitPools()
	{
		if(ShootObject==null)ShootObject = Resources.Load<GameObject>("ShootObject/Effect_Bow_Trail");
		if(ShootObject1==null)ShootObject1 = Resources.Load<GameObject>("ShootObject/Magic_ball");
		if(ShootObject2==null)ShootObject2 = Resources.Load<GameObject>("ShootObject/Effect_RobinHood_Track");
		if(ShootObject3==null)ShootObject3 = Resources.Load<GameObject>("ShootObject/Debuff_Poison");
		ArrowPool = new Pool(ShootObject,32,0);
		MagicBallPool = new Pool(ShootObject1,32,1);
		HeroSOPool0 = new Pool(ShootObject2,4,2);
		HeroSOPool1 = new Pool(ShootObject3,4,3);
	}

	public void InitBattleData()
	{ 
		//if(!IsSingleMode)
            InitMyMatrixList(); //TODO
		//if(!IsSingleMode)
            InitEnemyMatrixList(); //TODO
		InitMyMatrixLocation(); 
		InitEnemyMatrixLocation();
		InitMatrixId();
		InitHeroSkills();
	}

	//TODO
	public GameObject GlobalSkillEffect;
	void InitHeroSkills()
	{ 
		setSkillsInfo(PlayerHeros);
		//setSkillsInfo(EnemyHeros);
		/*for(int i = 0 ; i < PlayerHeros.Count ; i ++)
		{
			DataMgr.ConfigRow cr = null;
			if(DataManager.me!=null)DataMgr.CHerroTalbeAttribute.getHeroBaseDetail(PlayerHeros[i].HeroTypeId, out cr);
			if (cr != null)
			{
				Skill1 = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL1_TYPEID);
				Skill2 = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL2_TYPEID);
				Skill3 = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL3_TYPEID);

				DataMgr.Skill skillObj1 = DataManager.getSkillData().getSkillBySkillID(Skill1);
				if(!skillObj1.IsActive)
				{
					skillObj1.GlobalSkillEffect = GlobalSkillEffect;//TODO
					skillObj1.onSkill += PlayerHeros[i].Skill.PlayGlobalSkillEffect;
				}
				PlayerHeros[i].Skill.HeroSkills.Add(skillObj1);
				DataManager.getSkillData().setSkillEvent(PlayerHeros[i].Skill,skillObj1);


				DataMgr.Skill skillObj2 = DataManager.getSkillData().getSkillBySkillID(Skill2);
				if(!skillObj2.IsActive)
				{
					skillObj2.GlobalSkillEffect = GlobalSkillEffect;//TODO
					skillObj2.onSkill += PlayerHeros[i].Skill.PlayGlobalSkillEffect;
				}
				PlayerHeros[i].Skill.HeroSkills.Add(skillObj2);
				DataManager.getSkillData().setSkillEvent(PlayerHeros[i].Skill,skillObj2);

				DataMgr.Skill skillObj3 = DataManager.getSkillData().getSkillBySkillID(Skill3);
				if(!skillObj3.IsActive)
				{
					skillObj3.GlobalSkillEffect = GlobalSkillEffect;//TODO
					skillObj3.onSkill += PlayerHeros[i].Skill.PlayGlobalSkillEffect;
				}
				PlayerHeros[i].Skill.HeroSkills.Add(skillObj3);
				DataManager.getSkillData().setSkillEvent(PlayerHeros[i].Skill,skillObj3);
			}				
		}*/	

	}

	void setSkillsInfo(List<Unit> herolist)
	{
					
		for(int i = 0 ; i < herolist.Count ; i ++)
		{
			//Debug.Log("herolist[i].HeroTypeId.................................." + herolist[i].HeroTypeId);
			DataMgr.ConfigRow cr = null;
			if(DataManager.me!=null)DataMgr.CHerroTalbeAttribute.getHeroBaseDetail(herolist[i].HeroTypeId, out cr);
			if (cr != null)
			{
				Skill1 = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL1_TYPEID);
				Skill2 = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL2_TYPEID);
				Skill3 = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_SKILL3_TYPEID);
				DataMgr.Skill skillObj1 = DataManager.getSkillData().getSkillBySkillID(Skill1);
				herolist [i].Init ();
				herolist[i].Skill.HeroSkills.Add(skillObj1);
				DataManager.getSkillData().setSkillEvent(herolist[i].Skill,skillObj1);
				if(!skillObj1.IsActive)
				{
					skillObj1.GlobalSkillEffect = GlobalSkillEffect;//TODO
					//skillObj1.onSkill += herolist[i].Skill.PlayGlobalSkillEffect;
					skillObj1.onGlobalEffect += herolist[i].Skill.PlayGlobalSkillEffect;
					//skillObj1.IsGlobal = false;
				}
				DataMgr.Skill skillObj2 = DataManager.getSkillData().getSkillBySkillID(Skill2);
				herolist[i].Skill.HeroSkills.Add(skillObj2);
				DataManager.getSkillData().setSkillEvent(herolist[i].Skill,skillObj2);
				if(!skillObj2.IsActive)
				{
					skillObj2.GlobalSkillEffect = GlobalSkillEffect;//TODO
					//skillObj2.onSkill += herolist[i].Skill.PlayGlobalSkillEffect;
					skillObj2.onGlobalEffect += herolist[i].Skill.PlayGlobalSkillEffect;
					//skillObj2.IsGlobal = true;
				}
				
				DataMgr.Skill skillObj3 = DataManager.getSkillData().getSkillBySkillID(Skill3);
				herolist[i].Skill.HeroSkills.Add(skillObj3);
				DataManager.getSkillData().setSkillEvent(herolist[i].Skill,skillObj3);
				if(!skillObj3.IsActive)
				{
					skillObj3.GlobalSkillEffect = GlobalSkillEffect;//TODO
					//skillObj3.onSkill += herolist[i].Skill.PlayGlobalSkillEffect;
					skillObj3.onGlobalEffect += herolist[i].Skill.PlayGlobalSkillEffect;
					//skillObj3.IsGlobal = true;
				}
			}
				//Debug.Log("herolist[i].HeroTypeId..........................." + herolist[i].HeroTypeId);
		}
	}

	void GetHeroInfo()
	{
		List<EnemyHero> enemyHeroList = DataManager.getBattleUIData().EnemyHeroList;
		List<HeroSelect> playerHeroList = DataManager.getBattleUIData().HeroList;
		List<Fight.Hero> HeroList = new List<Fight.Hero>();
		foreach(HeroSelect hs in playerHeroList)
		{
			Fight.Hero hero = FightData.getSelfHero(hs.nHeroId);
			HeroList.Add(hero);
		}
	} 

	void SetFrontMatrixs()
	{
		for(int i=0;i<3;i++)
		{
			if(PlayerMatrixs[i,1]!=null && PlayerMatrixs[i,0]!=null)PlayerMatrixs[i,1].FrontMatrix = PlayerMatrixs[i,0];
			if(PlayerMatrixs[i,2]!=null)
			{
				if(PlayerMatrixs[i,1]!=null)
				{
					if(PlayerMatrixs[i,0]==null)
					{
						PlayerMatrixs[i,2].FrontMatrix = PlayerMatrixs[i,1];
					}
				}
				else if(PlayerMatrixs[i,0]!=null)
				{
					if(PlayerMatrixs[i,1]==null)
					{
						PlayerMatrixs[i,2].FrontMatrix = PlayerMatrixs[i,0];
					}
				}
			}

			if(EnemyMatrixs[i,1]!=null && EnemyMatrixs[i,0]!=null)EnemyMatrixs[i,1].FrontMatrix = EnemyMatrixs[i,0];
			if(EnemyMatrixs[i,2]!=null)
			{
				if(EnemyMatrixs[i,1]!=null)
				{
					if(EnemyMatrixs[i,0]==null)
					{
						EnemyMatrixs[i,2].FrontMatrix = EnemyMatrixs[i,1];
					}
				}
				else if(EnemyMatrixs[i,0]!=null)
				{
					if(EnemyMatrixs[i,1]==null)
					{
						EnemyMatrixs[i,2].FrontMatrix = EnemyMatrixs[i,0];
					}
				}
			}
		}
	}


	Unit AppendUnit(GameObject _obj, Character _character, int _matrixid) 
	{
		if (_character == null || _obj == null )
		{
			 Debug.LogError("AppendUnit function error"); 
			return null;
		}  
		Unit unit = Common.AddObjComponent<Unit>(_obj);
		unit.Alignment = _character.side == SIDE.LEFT ? _UnitAlignment.Player : _UnitAlignment.Enemy;
		unit.Line = (_character.location - 1) / 3;
		unit.Matrix = _matrixid;
		unit.HeroId = _character.id; 
		if(_character.side == SIDE.LEFT)
		{
			if(unit.Line==0)unit.WayPoints = GetRandomWayPointPos(PlayerWayPointLine0);
			if(unit.Line==1)unit.WayPoints = GetRandomWayPointPos(PlayerWayPointLine1);
			if(unit.Line==2)unit.WayPoints = GetRandomWayPointPos(PlayerWayPointLine2);
		}
		return unit;
	}

	List<Vector3> GetRandomWayPointPos(List<Transform> wp)
	{
		List<Vector3> wayPointPos = new List<Vector3>();
		wayPointPos.Add(wp[Random.Range(0,wp.Count-1)].position + new Vector3(Random.Range(-2.0f,2.0f),0,Random.Range(-2.0f,2.0f)));
		wayPointPos.Add(wp[wp.Count-1].position + new Vector3(Random.Range(-2.0f,2.0f),0,Random.Range(-2.0f,2.0f)));
		return wayPointPos;
	}

    

	UnitAttribute AppendUnitAttribute(GameObject _obj, Character _character,
                                        GameObject unitOverlaybar, Camera mainCamera, 
                                        Camera guiCamera)
	{  
		UnitAttribute attribute = Common.AddObjComponent<UnitAttribute>(_obj); 
		attribute.HP = _character.baseHp; 
		attribute.MaxHP = _character.baseHp;
		attribute.BaseHP = _character.baseHp;
		attribute.BaseMaxHP = _character.baseHp; 
        attribute.BaseDef = _character.baseDef; 
        attribute.ScanRadius = 40;
        attribute.AttackRadius = _character.atkBaseAttackRange * Random.Range(0.9f,1.1f);
        attribute.BaseDamage = _character.baseDamage;
        attribute.BaseAttackSpeed = Mathf.Abs(Mathf.Min(_character.atkBaseSpeed, UnitAttribute.m_cMaxMoveSpeed));
        attribute.BaseMoveSpeed = Mathf.Min(_character.baseSpeed, UnitAttribute.m_CMaxAttackSpeed);  
        attribute.AdjustMoveSpeed =1;  
		attribute.m_eFigherType = _character.armyType; 
		attribute.MainCamera = mainCamera;
		attribute.NGUICamera = guiCamera;
		attribute.OverlayPrefab = unitOverlaybar; 
        attribute.m_StaticData = _character;  
        if (_character.armyType == ARMY_TYPE.HERO)
        {
            attribute.Nature = ((Fight.Hero)_character).Nature;
        } 
		return attribute; 
	} 
	 
	UnitMove AppendUnitMove(GameObject _obj, Character _character, float adjustSpeed, int forwardDist)
	{
		if (_obj == null || _character == null)
		{
			 Debug.LogError("AppendUnitMove function error");
			return null;
		}
		UnitMove unitmove = Common.AddObjComponent<UnitMove>(_obj);
        unitmove.speed = Mathf.Min(_character.baseSpeed, UnitAttribute.m_CMaxAttackSpeed);
		if (_character.side == SIDE.LEFT)
			unitmove.ArrayForwardDist = forwardDist;
		else
			unitmove.ArrayForwardDist = -forwardDist;

		return unitmove;
	}
	 
	UnitSkill AppendUnitSkill(GameObject _obj, Character _character)
	{
		if (_obj == null || _character == null)
		{
			 Debug.LogError("AppendUnitSkill function error");
			return null;
		}
		UnitSkill skill = Common.AddObjComponent<UnitSkill>(_obj);
		skill.HeroSkills = new List<DataMgr.Skill>();
		GameObject tmp_heroobj = Common.CreateGameObject(_character.strResName);
//		GameObject tmp_heroobj = ResourceManager.GetInstance.LoadHero (_character.strResName);
		tmp_heroobj = Instantiate (tmp_heroobj) as GameObject;
		tmp_heroobj.SetActive(false);
		skill.GlobalSkillHero = tmp_heroobj;

//		if (_character.side == SIDE.LEFT)
//		{
//			skill.HeroSkills.Add(SkillTester.GetDefaultRangerMark(skill,RangeMarkShootObj));
//		}
//		else
//		{
//			//			skill.HeroSkills.Add(SkillTester.GetLammasu(skill));
//			skill.HeroSkills.Add(SkillTester.GetLightWingGift(skill));
//		}

//        skill.CurCooldown = 5;
		return skill;
	}

	UnitGizmos AppendUnitGizmos(GameObject _obj)
	{
		if (_obj == null)
		{
			return null;
		}
		UnitGizmos unitgizemos = Common.AddObjComponent<UnitGizmos>(_obj);
		unitgizemos.LineGizmosCubeSize = 0.5f;
		return unitgizemos;
	}

	UnitAnim AppendUnitAnim(GameObject _obj)
	{
		if (_obj == null)
		{
			return null;
		}
		UnitAnim unitAnim = Common.AddObjComponent<UnitAnim>(_obj);
		unitAnim.AnimMode = 0;
		return unitAnim;
	} 

	UnitRepelEffect AppendUnitRepelEffect(GameObject _obj)
	{
		if (_obj == null)
		{
			return null;
		}
		UnitRepelEffect repelEffect = Common.AddObjComponent<UnitRepelEffect>(_obj);
		return repelEffect;
	}

	public AnimationEvent[] AppendAnimationEvent(GameObject _obj)
	{
		if (_obj == null)
		{
			  return null;
		}
		
		List<AnimationEvent> tmp_event = new List<AnimationEvent>();

		Animator[] anitors = _obj.GetComponentsInChildren<Animator>(true);
		for (int i = 0; i < anitors.Length; ++i)
		{
			if (anitors[i] != null)
			{
				//AnimationEvent animevent = Common.AddObjComponent<AnimationEvent>(anitors[i].gameObject);

				//tmp_event.Add(animevent);

				//animevent = null;
			}
		}

		return tmp_event.ToArray();
	}

	UnitAttack AppendUnitAttack(GameObject _obj)
	{
		if (_obj == null)
		{
			return null;
		}
		UnitAttack attack = Common.AddObjComponent<UnitAttack>(_obj); 
		attack.ShootDelay = 0;
		attack.Cooldown = 0.5f;
		attack.IsAttackByAnimEvent = true;
		attack.ScanRequire = true;
		return attack;
	}
   
	UnitMeleeAttack AppendUnitMeleeAttack(GameObject _obj)
	{
		if (_obj == null)
		{
			return null;
		}
		UnitMeleeAttack meleeattack = Common.AddObjComponent<UnitMeleeAttack>(_obj); 
		meleeattack.m_DefaultScaneRadius = 0;
		meleeattack.ChargeDist = 40;
		return meleeattack;
    } 

	private void InitMyMatrixList()
	{
        if(IsSingleMode)
        {
            SpawnUtility.InitTmpPlayerHeroList();
        }
		if (DataManager.getBattleUIData().HeroList != null)
		{
			List<HeroSelect> tmp_herolist = DataManager.getBattleUIData().HeroList;
			if (tmp_herolist == null)
			{
				Logger.LogWarning("tmp_herolist is null");
					   
				return;
			} 
			 
			for (int i = 0; i < tmp_herolist.Count; ++i)
			{
				if (tmp_herolist[i] != null)
				{
					Fight.Hero tmp_hero = FightData.getSelfHero(tmp_herolist[i].nHeroId);
                 
					if (tmp_hero == null)
					{
						Logger.LogWarning("tmp_hero is null");
					   
						return;
					}

					BattleMatrix tmp_matrix = new BattleMatrix(); 
					tmp_matrix.location = tmp_herolist[i].nLocation; 
					tmp_matrix.m_Hero = tmp_hero;
					tmp_matrix.m_Hero.side = SIDE.LEFT;
					tmp_matrix.m_Hero.id = tmp_herolist[i].nHeroId;
					tmp_matrix.m_Hero.location = tmp_herolist[i].nLocation;
                    tmp_herolist[i].nSoldierNum = 16;
					if (tmp_herolist[i].nSoldierNum > 0)
					{
						 
						//Fight.Soldier[] tmp_soldierlist = FightData.getSelfSoldier(tmp_hero.soldierType, tmp_herolist[i].nSoldierNum);
                        Fight.Soldier[] tmp_soldierlist = FightData.getEnemySoldier(tmp_hero.soldierType,1,1, tmp_herolist[i].nSoldierNum);
						if (tmp_soldierlist == null)
						{
							Debug.LogWarning("tmp_soldierlist is null");

							return;
						}

						for (int j = 0; j < tmp_soldierlist.Length; ++j)
						{
							if (tmp_soldierlist[j] != null)
							{
								if (tmp_matrix.m_SoldierList != null)
								{
									tmp_soldierlist[j].side = SIDE.LEFT;
									tmp_soldierlist[j].location = tmp_herolist[i].nLocation;
									tmp_soldierlist[j].id = tmp_herolist[i].nHeroId;
									tmp_matrix.m_SoldierList.Add(tmp_soldierlist[j]);
								}
							}
						}

						tmp_soldierlist = null;
					} 
				   
					AddBattleMatrix(ref m_MyMatrixList, tmp_matrix);

					tmp_matrix = null;

					tmp_hero = null; 
				}
			}

			tmp_herolist = null;
		}
		else
		{
			Debug.LogWarning("BattleUIMgr.me.HeroList is null");
		}
	}
   
	private void InitEnemyMatrixList()
	{
        if (IsSingleMode)
        {
            Debug.Log("InitTmpEnemyHeroList");
            SpawnUtility.InitTmpEnemyHeroList();
        }
		if (DataManager.getBattleUIData().EnemyHeroList != null)
		{
			List<EnemyHero> tmp_herolist = DataManager.getBattleUIData().EnemyHeroList;
            Debug.Log("EnemyHeroList:" + DataManager.getBattleUIData().EnemyHeroList.Count);
			for (int i = 0; i < tmp_herolist.Count; ++i)
			{
				if (tmp_herolist[i] != null)
				{
					Fight.Hero tmp_hero = FightData.getEnemyHero(tmp_herolist[i]);

					if (tmp_hero == null)
					{
						Debug.LogWarning("tmp_hero is null");

						return;
					}

					BattleMatrix tmp_matrix = new BattleMatrix();
					tmp_matrix.location = tmp_herolist[i].nLocation;
					tmp_matrix.m_Hero = tmp_hero;
					tmp_matrix.m_Hero.side = SIDE.RIGHT; 
					tmp_matrix.m_Hero.location = tmp_herolist[i].nLocation; 
				   
					Fight.Soldier[] tmp_soldierList = FightData.getEnemySoldier(tmp_hero.soldierType,
																  tmp_herolist[i].nSoldierArmyLevel,
																  tmp_herolist[i].nSoldierLevel,
																  tmp_herolist[i].nSoldierNum);

					if (tmp_soldierList == null)
					{
						Debug.LogWarning("tmp_soldierlist is null");

						return;
					}

					for (int j = 0; j < tmp_soldierList.Length; ++j)
					{
						if (tmp_soldierList[j] != null)
						{
							if (tmp_matrix.m_SoldierList != null)
							{
								tmp_soldierList[j].side = SIDE.RIGHT;
								tmp_soldierList[j].location = tmp_herolist[i].nLocation;
								tmp_matrix.m_SoldierList.Add(tmp_soldierList[j]);
							}
						}
					}

					if (m_EnemyMatrixList != null)
					{
						m_EnemyMatrixList.Add(tmp_matrix);
					}

					tmp_hero = null;
					tmp_soldierList = null;
					tmp_matrix = null; 
				}
			}

			tmp_herolist = null;
		}
	}
   
	private void InitMyMatrixLocation()
	{
		if (m_MyDicLocation == null)
		{
			Debug.LogWarning("m_MyDicLocation is null");

			return;
		}

		m_MyDicLocation.Add(1, SpawnManager.SingleTon().PlayerSpawnPoints[6]);
		m_MyDicLocation.Add(2, SpawnManager.SingleTon().PlayerSpawnPoints[3]);
		m_MyDicLocation.Add(3, SpawnManager.SingleTon().PlayerSpawnPoints[0]);
		m_MyDicLocation.Add(4, SpawnManager.SingleTon().PlayerSpawnPoints[7]);
		m_MyDicLocation.Add(5, SpawnManager.SingleTon().PlayerSpawnPoints[4]);
		m_MyDicLocation.Add(6, SpawnManager.SingleTon().PlayerSpawnPoints[1]);
		m_MyDicLocation.Add(7, SpawnManager.SingleTon().PlayerSpawnPoints[8]);
		m_MyDicLocation.Add(8, SpawnManager.SingleTon().PlayerSpawnPoints[5]);
		m_MyDicLocation.Add(9, SpawnManager.SingleTon().PlayerSpawnPoints[2]);
	}
   
	private void InitEnemyMatrixLocation()
	{
		if (m_EnemyDicLocation == null)
		{
			Debug.LogWarning("m_EnemyDicLocation is null");

			return;
		}

		m_EnemyDicLocation.Add(1, SpawnManager.SingleTon().EnemySpawnPoints[2]); 
		m_EnemyDicLocation.Add(2, SpawnManager.SingleTon().EnemySpawnPoints[5]); 
		m_EnemyDicLocation.Add(3, SpawnManager.SingleTon().EnemySpawnPoints[8]); 
		m_EnemyDicLocation.Add(4, SpawnManager.SingleTon().EnemySpawnPoints[1]); 
		m_EnemyDicLocation.Add(5, SpawnManager.SingleTon().EnemySpawnPoints[4]); 
		m_EnemyDicLocation.Add(6, SpawnManager.SingleTon().EnemySpawnPoints[7]); 
		m_EnemyDicLocation.Add(7, SpawnManager.SingleTon().EnemySpawnPoints[0]); 
		m_EnemyDicLocation.Add(8, SpawnManager.SingleTon().EnemySpawnPoints[3]); 
		m_EnemyDicLocation.Add(9, SpawnManager.SingleTon().EnemySpawnPoints[6]);
	}

	private void InitMatrixId()
	{
		m_DicMatrixId.Add(1, 2);
		m_DicMatrixId.Add(2, 1);
		m_DicMatrixId.Add(3, 0);
		m_DicMatrixId.Add(4, 2);
		m_DicMatrixId.Add(5, 1);
		m_DicMatrixId.Add(6, 0);
		m_DicMatrixId.Add(7, 2);
		m_DicMatrixId.Add(8, 1);
		m_DicMatrixId.Add(9, 0); 
	}   

    private void InstantiateMatrix(List<BattleMatrix> _matrixlist, Dictionary<int, Transform> _diclocation, int _rownum, int _colnum)
    {
        if (_matrixlist == null || _diclocation == null)
        {
            Debug.LogError("CreateMatrixs function error !!!");

            return;
        }

        for (int i = 0; i < _matrixlist.Count; ++i)
        {
            if (_matrixlist[i] != null)
            {
                //Instantiate Matrix Gameobject  
                GameObject matrix = new GameObject();
                matrix.name = "Matrix" + i;
                UnitMatrix unitMatrix = matrix.AddComponent<UnitMatrix>();

                if (_matrixlist[i].m_Hero != null)
                {
                    //Instantiate Hero GameObject
                    if (_matrixlist[i].m_Hero.side == SIDE.LEFT)
                    {
                        _matrixlist[i].m_Hero.strResName += "_Red";
                    }
                    else if (_matrixlist[i].m_Hero.side == SIDE.RIGHT)
                    {
                        _matrixlist[i].m_Hero.strResName += "_Blue";
                    }
//					GameObject tmp_heroobj = ResourceManager.GetInstance.LoadHero (_matrixlist [i].m_Hero.strResName);
//					tmp_heroobj = Instantiate (tmp_heroobj) as GameObject;
                    GameObject tmp_heroobj = Common.CreateGameObject(_matrixlist[i].m_Hero.strResName);
                    tmp_heroobj.SetActive(false);
                    Common.AddChildObj(tmp_heroobj, matrix, (new Vector3(0, 0, (_rownum - 1) / 2.0f + 1)) * RowPadding);
                    //Set Tag 
                    Common.SetTag(tmp_heroobj, "Soldier");

                    //Add Component for gameobject of hero  
                    Unit unit = AppendUnit(tmp_heroobj, _matrixlist[i].m_Hero, m_DicMatrixId[_matrixlist[i].m_Hero.location]);
                    unit.UnitTroop = ARMY_TYPE.HERO;
                    unitMatrix.Hero = unit;
                    unitMatrix.LineIndex = unit.Line;
                    unitMatrix.MatrixIndex = unit.Matrix;
                    unit.UMatrix = unitMatrix;
                    if (unit.Alignment == _UnitAlignment.Player)
                    {
                        PlayerMatrixs[unit.Line, unit.Matrix] = unitMatrix;
                        PlayerHeros.Add(unit);
                        Debug.Log("unit.HeroTypeId..........................................." + unit.HeroTypeId);
                    }
                    else
                    {
                        EnemyMatrixs[unit.Line, unit.Matrix] = unitMatrix;
                        EnemyHeros.Add(unit);
                    }
                    unit.IsHero = true;
                    unit.DestroyAble = false;
                    unit.HeroTypeId = _matrixlist[i].m_Hero.typeid;
                    UnitAttribute attribute = AppendUnitAttribute(tmp_heroobj, _matrixlist[i].m_Hero, OverlayPrefab, MainCamera, NGUICamera);
//					attribute.Nature = i;
                    UnitMove move = AppendUnitMove(tmp_heroobj, _matrixlist[i].m_Hero, 1.0f, 60);
                    //tmp_heroobj.GetComponent<NavMeshAgent>().radius = tmp_heroobj.GetComponent<NavMeshAgent>().radius * 2;
                    UnitGizmos gizmos = AppendUnitGizmos(tmp_heroobj);
                    UnitAnim unitanim = AppendUnitAnim(tmp_heroobj);
                    UnitSkill skill = AppendUnitSkill(tmp_heroobj, _matrixlist[i].m_Hero);
                    AppendAnimationEvent(tmp_heroobj);
                    switch (_matrixlist[i].m_Hero.soldierType)
                    {
                        case ARMY_TYPE.ARCHER:
                        case ARMY_TYPE.MAGIC:
                            UnitAttack attack = AppendUnitAttack(tmp_heroobj);
                            break;
                        case ARMY_TYPE.CAVALRY:
                        case ARMY_TYPE.PIKEMAN:
                        case ARMY_TYPE.SHIELD:
                            UnitMeleeAttack meleeattack = AppendUnitMeleeAttack(tmp_heroobj);
                            break;
                    }
                    tmp_heroobj.SetActive(true);

                    List<Character> tmp_soldierlist = _matrixlist[i].m_SoldierList;
                    int index = 0;
                    for (int n = 0; n < _rownum; n++)
                    {
                        GameObject Row = new GameObject();
                        Row.name = "Row";
						Common.AddChildObj(Row, matrix, (new Vector3(0, 0, n - (_rownum - 1) / 2.0f)) * RowPadding);
                        Row.name = "Row" + n;

                        for (int m = 0; m < _colnum; m++)
                        {
                            if (index > tmp_soldierlist.Count)
                            {
                                return;
                            }

                            //Instantiate Soldier GameObject
                            GameObject tmp_soldier = Common.CreateGameObject(tmp_soldierlist[index].strResName);
//							GameObject tmp_soldier = ResourceManager.GetInstance.LoadSolider(tmp_soldierlist[index].strResName);//.CreateGameObject(tmp_soldierlist[index].strResName);
							tmp_soldier = Instantiate(tmp_soldier) as GameObject;
                            tmp_soldier.SetActive(false);
                            Common.AddChildObj(tmp_soldier, Row, (new Vector3(m - (_colnum - 1) / 2.0f, 0, 0)) * ColumnPadding);
							tmp_soldier.transform.position = tmp_soldier.transform.position + new Vector3(Random.Range(-0.3f,0.3f),0,Random.Range(-0.3f,0.3f));
                            //Set Tag 
                            Common.SetTag(tmp_soldier, "Soldier");

                            //Add Component for gameobject of soldier
                            unit = AppendUnit(tmp_soldier, tmp_soldierlist[index], m_DicMatrixId[_matrixlist[i].m_Hero.location]);
                            unit.UnitTroop = _matrixlist[i].m_Hero.soldierType;
                            unitMatrix.Soilders.Add(unit);
                            unit.UMatrix = unitMatrix;
                            if (n == 0) unit.DestroyAble = false;
							attribute = AppendUnitAttribute(tmp_soldier, tmp_soldierlist[index], OverlayPrefab, MainCamera, NGUICamera); 
                            attribute.LeaderAbtAdd(_matrixlist[i].m_Hero.baseLead, _matrixlist[i].m_Hero.level);

                            if (_matrixlist[i].m_Hero.soldierType == ARMY_TYPE.CAVALRY) attribute.AdjustMoveSpeed = 10;
                            move = AppendUnitMove(tmp_soldier, tmp_soldierlist[index], 1.0f, 60);
                            gizmos = AppendUnitGizmos(tmp_soldier);
                            unitanim = AppendUnitAnim(tmp_soldier);
                            AppendAnimationEvent(tmp_soldier);
                            switch (_matrixlist[i].m_Hero.soldierType)
                            {
                                case ARMY_TYPE.ARCHER:
                                case ARMY_TYPE.MAGIC:
                                    UnitAttack attack = AppendUnitAttack(tmp_soldier);
                                    break;
                                case ARMY_TYPE.CAVALRY:
                                case ARMY_TYPE.PIKEMAN:
                                case ARMY_TYPE.SHIELD:
                                    UnitMeleeAttack meleeattack = AppendUnitMeleeAttack(tmp_soldier);
                                    break;
                            }


                            //							if(tmp_soldierlist[index].side == SIDE.RIGHT)
                            //							{
                            MaterialsRes[] tmp_res = tmp_soldier.GetComponentsInChildren<MaterialsRes>(true);
                            for (int k = 0; k < tmp_res.Length; ++k)
                            {
                                if (tmp_res[k] != null)
                                {
                                    if (tmp_soldierlist[index].side == SIDE.RIGHT)
                                    {
                                        tmp_res[k].ChangeColor(GetEnemyMaterial(tmp_res[k].m_Material));
                                    }
                                    else
                                    {
                                        tmp_res[k].ChangeColor(PlayerColor);
                                    }
                                }
                            }
                            //							}else{
                            //
                            //							}

                            index++;
                            tmp_soldier.SetActive(true);
                        }
                    }
                }

                matrix.transform.position = _diclocation[_matrixlist[i].location].position;
                matrix.transform.eulerAngles = _diclocation[_matrixlist[i].location].eulerAngles;
            }
        }
    } 

	Dictionary<ARMY_TYPE,Material> EnemyMaterials = new Dictionary<ARMY_TYPE, Material>();
	public Material GetEnemyMaterial(ARMY_TYPE army_Type,Material playerMaterial)
	{
		if(!EnemyMaterials.ContainsKey(army_Type))
		{
			Material material = new Material(playerMaterial);
			material.SetColor("_Color", Color.blue);
			EnemyMaterials.Add(army_Type,material);
		}
		return EnemyMaterials[army_Type];
	}

	Dictionary<Material,Material> m_EnemyMaterials = new Dictionary<Material, Material>();
	public Material GetEnemyMaterial(Material playerMaterial)
	{
		if(!m_EnemyMaterials.ContainsKey(playerMaterial))
		{
			Material material = new Material(playerMaterial);
			material.SetColor("_Color", EnemyColor);
			m_EnemyMaterials.Add(playerMaterial,material);
		}
		return m_EnemyMaterials[playerMaterial];
	}  

	public void CreateBattleMatrixs(int _rownums, int _colnums)
	{
		//if(IsSingleMode)m_MyMatrixList = SpawnUtility.InitTestMyMatrixList(SIDE.LEFT);//TODO
		//if(IsSingleMode)m_EnemyMatrixList = SpawnUtility.InitTestMyMatrixList(SIDE.RIGHT);//TODO
		//m_EnemyMatrixList = SpawnUtility.InitTestMyMatrixList(SIDE.LEFT);//TODO
		if (m_MyMatrixList == null || m_MyDicLocation == null
		  || m_EnemyMatrixList == null || m_EnemyDicLocation == null)
		{
			 Debug.LogError("CreateBattleMatrixs function error !!!");

			return;
		}

		if (_rownums <= 0 || _colnums <= 0)
		{
			 Debug.LogError("row and col parameter error !!!");

			return;
		}

		InstantiateMatrix(m_MyMatrixList, m_MyDicLocation, _rownums, _colnums);

		InstantiateMatrix(m_EnemyMatrixList, m_EnemyDicLocation, _rownums, _colnums);
		InitHeroSkills();
		SetFrontMatrixs();
	}  

	public List<BattleMatrix> GetMyMatrixList()
	{
		if (m_MyMatrixList == null)
		{ 
			 Debug.LogError("m_MyMatrixList is null"); 
 
			return null;
		}

		return m_MyMatrixList;
	}

	public List<BattleMatrix> GetMyEnemyMatrixList()
	{
		if (m_EnemyMatrixList == null)
		{ 
			 Debug.LogError("m_EnemyMatrixList is null"); 

			return null;
		}

		return m_EnemyMatrixList;
	}
  
	public void AddBattleMatrix(ref List<BattleMatrix> _list, BattleMatrix _matrix)
	{
		if (_list == null)
		{ 
			 Debug.LogError("list is null");
 
			return;
		}

		if (_matrix == null)
		{ 
			 Debug.LogError("_matrix is null");
 
			return;
		}

		_list.Add(_matrix);
	}

	void OnDrawGizmos()
	{
		if (PlayerSpawnPoints != null)
		{
			foreach (Transform trans in PlayerSpawnPoints)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawCube(trans.position, Vector3.one * 0.5f);
			}
		}
 
		if (EnemySpawnPoints != null)
		{
			foreach (Transform trans in EnemySpawnPoints)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawCube(trans.position, Vector3.one * 0.5f);
			}
		}

		if(PlayerWayPointLine0.Count > 0)
		{
			foreach (Transform trans in PlayerWayPointLine0)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(trans.position, Vector3.one * 0.5f);
			}
		}
		if(PlayerWayPointLine1.Count > 0)
		{
			foreach (Transform trans in PlayerWayPointLine1)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(trans.position, Vector3.one * 0.5f);
			}
		}
		if(PlayerWayPointLine2.Count > 0)
		{
			foreach (Transform trans in PlayerWayPointLine2)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(trans.position, Vector3.one * 0.5f);
			}
		}

	} 

	public void RemoveHeroFromList(Unit unit)
	{
//		if (onRemoveHero != null)
//			onRemoveHero(unit);
		if(unit.Alignment == _UnitAlignment.Player)
		{
			SpawnManager.SingleTon().PlayerHeros.Remove(unit);
		}
		else
			if(unit.Alignment == _UnitAlignment.Enemy)
		{
			SpawnManager.SingleTon().EnemyHeros.Remove(unit);
		}
	}

	public void onBeforeDead(Unit unit)
	{
		if (onKingSacrifice != null)
			onKingSacrifice(unit);
	}
}
 
