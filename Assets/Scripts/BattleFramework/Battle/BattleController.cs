//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleFramework{
	public class BattleController : MonoBehaviour {

		public GameObject plantPrefab;//TODO should multi plants
		public GameObject zombiePrefab;//TODO should multi zombie

		public List<GameObject> plants;
		public List<GameObject> zombies;

		public int lineCount = 5;

		public int interval = 2;
		public int zombieCount = 20;//TODO 

		public int gridLength = 11;
		public int gridWidth = 5;
		public GameObject[,] grids = new GameObject[5,11]; 

		public List<Wave> waves;

		public GameObject shootPrefab;

		static BattleController instance;
		static public BattleController SingleTon()
		{
			if(instance==null)
			{
				GameObject go = new GameObject();
				go.name = "_BattleController";
				instance = go.AddComponent<BattleController>();
			}
			return instance;
		}
		
		void Awake()
		{
			if(instance==null)
			{
				instance = this;
			}
		}

		// Use this for initialization
		void Start () {
			InitPlants ();
			InitBattleGrid ();
			InitPools ();
			InitWaves ();
			StartCoroutine (SpawnZombies());
		}

		#region double click
		bool mClick;
		float mClickInterval = 0.5f;
		float mCurrentInterval;
		void Update(){
			if(Input.GetMouseButtonDown(0))
			{
				if(mClick)
				{
					OnDoubleClick();
					mClick = false;
					mCurrentInterval = 0;
				}
				else
				{
					mClick = true;
					mCurrentInterval = 0;
				}
			}
			if(mClick)
			{
				mCurrentInterval += Time.deltaTime;
				if(mCurrentInterval > mClickInterval)
				{
					mCurrentInterval = 0;
					mClick = false;
				}
			}
			mCurrentInterval += Time.deltaTime;
		}

		Transform selectedPlant;
		void OnDoubleClick(){
			Debug.Log ("OnDoubleClick");
//			Physics.Raycast (Camera.main.transform.position,);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit,Mathf.Infinity,1<<Configs.plantLayer))
			{
				if(hit.transform.GetComponent<Renderer>()!=null)
				{
					hit.transform.GetComponent<Renderer>().material.color = Color.red;
					hit.transform.GetComponent<UnitController>().OnSkill();
					if(selectedPlant!=null && selectedPlant!=hit.transform)
					{
						selectedPlant.GetComponent<Renderer>().material.color = Color.white;
					}
					selectedPlant = hit.transform;
				}
			}
		}
		#endregion

		void Skill()
		{
				
		}


		//Init pool gameobjects,e.g shoot objects,hit effects;
		void InitPools()
		{
			if (shootPrefab == null)
				shootPrefab = Resources.Load<GameObject> ("Prefabs/ShootObject");
			PoolManager.SingleTon ().AddPool (shootPrefab,10,null);
		}

		public GameObject SpawnShootObject(Vector3 pos,Quaternion qua)
		{
			return PoolManager.SingleTon ().poolDictionary [shootPrefab].Spawn (pos,qua);
		}

		//TODO
		IEnumerator SpawnZombies()
		{
			for(int waveIndex=0;waveIndex < waves.Count;waveIndex++)
			{
				yield return new WaitForSeconds(waves[0].delayTime);
				for(int i = 0;i < waves[0].zombies.Count;i ++)
				{
					yield return new WaitForSeconds(waves[0].zombies[i].delayTime);
					waves[0].zombies[i].zombie.SetActive(true);
					int lineIndex = waves[0].zombies[i].zombie.GetComponent<UnitController>().attr.lineIndex;
					waves[0].zombies[i].zombie.transform.position = new Vector3(grids[lineIndex,10].transform.position.x,1,grids[lineIndex,10].transform.position.z);
				}

			}
		}

		void InitWaves()
		{
			if (zombiePrefab == null) 
			{
				zombiePrefab = Resources.Load<GameObject>("Prefabs/Zombie");
				zombiePrefab.SetActive (false);
			}
			waves = new List<Wave> ();
			Wave w0 = new Wave ();
			w0.delayTime = 2;
			w0.zombies = new List<SpawnInfo> ();
			SpawnInfo spawnInfo = new SpawnInfo ();
			spawnInfo.delayTime = 0;
			spawnInfo.zombie = Instantiate (zombiePrefab) as GameObject;
			UnitAttribute attr = spawnInfo.zombie.GetComponent<UnitAttribute> ();
			attr.lineIndex = 2;
			w0.zombies.Add (spawnInfo);

			spawnInfo = new SpawnInfo ();
			spawnInfo.delayTime = 0.5f;
			spawnInfo.zombie = Instantiate (zombiePrefab) as GameObject;
			attr = spawnInfo.zombie.GetComponent<UnitAttribute> ();
			attr.lineIndex = 1;
//			unit.attr.moveAble = true;
			w0.zombies.Add (spawnInfo);
			waves.Add (w0);
		}

		void InitBattleGrid()
		{
			Texture2D tex = TextureUtility.DrawPane (1,Color.green,20,20);
			Shader meshShader = Shader.Find("Diffuse");
			Material meshMaterial = new Material(meshShader);
			meshMaterial.mainTexture = tex;
			GameObject gridPrefab = MeshUtility.DrawSquareGameObject (meshMaterial, Color.white, 2);
			gridPrefab.hideFlags = HideFlags.HideInHierarchy;
			for(int i=0;i<gridWidth;i++)
			{
				for(int j=0;j<gridLength;j++)
				{
					grids[i,j] = Instantiate(gridPrefab,new Vector3((-2 + i) * interval,0,j*2),Quaternion.identity) as GameObject;
#if UNITY_EDITOR
					grids[i,j].hideFlags = HideFlags.HideInHierarchy;
#endif
				}
			}

		}

		void InitPlants()
		{
			if(plantPrefab==null)
				plantPrefab=Resources.Load<GameObject>("Prefabs/Plant");
			lineCount = Mathf.Clamp (lineCount,0,5);
			interval = Mathf.Max (interval,1);
			for(int i=0;i < lineCount;i ++)
			{
				plants.Add(Instantiate(plantPrefab,new Vector3((-2 + i) * interval,1,0),Quaternion.identity) as GameObject);
			}
		}

		public class Wave
		{
			public float delayTime;
			public List<SpawnInfo> zombies;
		}

		public class SpawnInfo
		{
			public float delayTime;
			public GameObject zombie;
		}

		public GameObject GetPlantShootTargetPosByLine(int lineIndex)
		{
			lineIndex = Mathf.Min (lineIndex,gridWidth);
			return grids [lineIndex, gridLength - 1];
		}

	}
}