using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
public class TestUI : MonoBehaviour 
{
    //Resource Name
    public string ArchersRes = "";
    public string CavalryRes = "";
    public string PeltastRes = "";
    public string SpearmenRes = "";
    public string MagicRes = "";
    
    //方阵位置对应布阵位置 
    public Dictionary<int, Transform> m_DicLocation = new Dictionary<int, Transform>();

	public static TestUI instance;

	public static TestUI SingleTon()
	{
		return instance;
	}

    //我方 
    public MatrixUI[,] m_MyMatrixUI; 
   
    //敌方 
	public MatrixUI[,] m_EnemyMatrixUI;

    //是否显示 
    public bool m_BView = false;

    //行距 
    public float m_RowDis = 200;

    //列距
    private float m_ColDis = 120; 
     
	void Awake()
    { 
		if(instance==null){instance=this;}  

        m_MyMatrixUI = new MatrixUI[3, 3];

        for (int r = 0; r < 3; ++r)
        {
            for (int c = 0; c < 3; ++c)
            {
                m_MyMatrixUI[r, c] = new MatrixUI();
            }
        }  
     
        m_EnemyMatrixUI = new MatrixUI[3, 3];

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            { 
                m_EnemyMatrixUI[i, j] = new MatrixUI(); 
            }
        } 
    }

	void Start()
	{ 
		 
	}

    void OnGUI()
    {
        m_BView = false;
        if (!m_BView)
        {
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    m_MyMatrixUI[i, j].DrawSetPlane(j * m_ColDis, i * m_RowDis);
                }
            }

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    m_EnemyMatrixUI[i, j].DrawSetPlane(j * m_ColDis + 450, i * m_RowDis);
                }
            }
			//开始
			if (GUI.Button(new Rect(300, 620, 200, 40), "配置完成"))
			{
				StartGame();
			}
        } 
    }

    //开始
    public void StartGame()
    {
        InitMatrixsData(m_MyMatrixUI, SpawnManager.instance.GetMyMatrixList());

        InitMatrixsData(m_EnemyMatrixUI, SpawnManager.instance.GetMyEnemyMatrixList());

        SpawnManager.SingleTon().InitBattleData(); 
        
        SpawnManager.SingleTon().CreateBattleMatrixs(4, 4);  
       
        BattleController.SingleTon().InitUnits();  
    } 

    //Init All Matrixs Data
    public void InitMatrixsData(MatrixUI[,] _matrixui, List<BattleMatrix> _battlematrix)
    {
        if (_matrixui == null || _battlematrix == null)
        {
            Logger.LogError("InitMatrixData function error!!!");
            return;
        }

        //Init Matrix Data
        if (_matrixui != null)
        {
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    BattleMatrix matrix = new BattleMatrix();
                    Fight.Hero hero = new Fight.Hero();
                    hero.location = (j + 1) + i*3 ;
                    hero.side = Fight.SIDE.LEFT;
                    int solidiernums = (int)(_matrixui[i, j].m_fRowValue * _matrixui[i, j].m_fMaxCol);
                    hero.soldierNum = solidiernums;
                    hero.strResName = GetResName(hero.armyType);
                    matrix.m_Hero = hero;
                    for (int n = 0; n < hero.soldierNum; ++n)
                    {
                        Fight.Soldier soldier = new Fight.Soldier();
                        soldier.location = hero.location;
                        soldier.armyType = (ARMY_TYPE)_matrixui[i, j].m_SoldierType; 
                        soldier.strResName = GetResName(soldier.armyType);
                        matrix.m_SoldierList.Add(soldier);
                    }
                    _battlematrix.Add(matrix);  
                }
            }
        }
    }

    //GetResName
    public string GetResName(ARMY_TYPE _typ)
    {
        switch (_typ)
        {
            case ARMY_TYPE.ARCHER:
                return "";
                break;
            case ARMY_TYPE.CAVALRY:

                return "";
                break;
            case ARMY_TYPE.MAGIC:

                return "";
                break;
            case ARMY_TYPE.PIKEMAN:

                return "";
                break;
            case ARMY_TYPE.SHIELD:

                return "";
                break;
        }

        return "";
    }
}
