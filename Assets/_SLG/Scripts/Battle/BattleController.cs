using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fight;
using UI;
using DataMgr;
using ThinksquirrelSoftware.Utilities;
using KOH;

public class BattleController : MonoBehaviour {

	
	public static BattleController instance;
	public static BattleController SingleTon(){
		return instance;
	}

	public List<Unit> EnemyUnits;
	public List<Unit> MyUnits;

	public int PlayerDeadArcherCount;
	public int PlayerDeadCavalryCount;
	public int PlayerDeadSpearmenCount;
	public int PlayerDeadMagicCount;
	public int PlayerDeadPeltastCount;

	public List<List<Unit>> MyUnitByLines;
	public List<List<Unit>> EnemyUnitByLines;

	public GameObject ShootObject;
	public GameObject ShootObject1;

	public int MyBaseLayer;
	public int EnemyBaseLayer;
	
	public bool IsBegin = false;
	public bool IsCountDown = false;
	public bool gSkilling = false;

	public GameObject skill0;
	public GameObject skill1;
	public GameObject skill2;

	[HideInInspector]public float MaxPlayerFighting;
	[HideInInspector]public float CurrentPlayerFighting;
	[HideInInspector]public float MaxEnemyFighting;
	[HideInInspector]public float CurrentEnemyFighting;

	public float BaseEnergy = 200;
	public float BaseEnergyRecover = 2;
	public float MaxEnergy = 1000;
	public float CurrentEnergy = 0;
	public float EnergyRecover = 1;
	public float EnemyMaxEnergy = 1000;
	public float EnemyCurEnergy = 0;
	public float EnemyEnergyRecover = 1;

	public Transform FingerEffectTrans;
	
	public GameObject TipFingerGestureCircle;
	public GameObject TipFingerGestureLiner;
	public GameObject TipFingerGestureLongPush;
	public GameObject TipFIngerGestureDubleClick;
	public AudioClip TipFingerAudio;

	public GameObject DefeatEffect;
	public GameObject WinEffect;
	public GameObject StartEffect; 

	public Material DeathMatFire;
	public Material DeathMatMagic;
	public Material DeathMatPosion;
	public Material DeathMatStone;

	public bool isHeroMournfulSong = false;
	public float HeroMournfulSongCooldown = 0;
	public float currHeroMournfulSongCooldown = 0;

	public float DuringSinceBattleBegin;
	public Unit CurrentSelectHero;

	public float FlyAngle = 20;
	public float FlyDistance = 3;
	public float FlySpeed = 20;

	public GameObject HitEffectFire;
	public NcCurveAnimation[] HitEffectFireAnims;
	public GameObject HitEffectMag;
	public NcCurveAnimation[] HitEffectMagAnims;
	public GameObject HitEffectPhy;
	public NcCurveAnimation[] HitEffectPhyAnims;
	public GameObject HitEffectPoi;
	public NcCurveAnimation[] HitEffectPoiAnims;

	public GameObject TerrainObj;
	public Renderer[] TerrainRenders;
	public GameObject GlobalEffect;
	public AudioClip GlobalAudio;
	public GameObject ThunderEffect;
	public AudioClip ThunderAudioClipStart;
	public AudioClip ThunderAudioClip;
	public AudioClip KnifeAudioClip;

	public GameObject ArthurSkill01;
	public Vector3 LongPressPos;
	public LineRenderer Line;
	public GameObject SkillCircle;
	public Vector3 SkillCircleRadius;

	void Awake () {
		
	}

	void Init(){
		if(instance==null){
			instance = this;
			if(ShootObject==null)ShootObject = Resources.Load<GameObject>("ShootObject/Effect_Bow_Trail");
			if(ShootObject1==null)ShootObject1 = Resources.Load<GameObject>("ShootObject/Magic_ball");
			if(Camera.main.GetComponent<Animation>()!=null && Camera.main.GetComponent<Animation>().clip == null)
			{
				Camera.main.GetComponent<Animation>().clip = Resources.Load<AnimationClip>("BattleCam");
				Camera.main.GetComponent<Animation>().Play();
			}
			if(DeathMatFire==null)DeathMatFire = Resources.Load("Effect/DeathColor_Fire") as Material;
			if(DeathMatMagic==null)DeathMatMagic = Resources.Load("Effect/DeathColor_Magic") as Material;
			if(DeathMatPosion==null)DeathMatPosion = Resources.Load("Effect/DeathColor_Poison") as Material;
			if(DeathMatStone==null)DeathMatStone = Resources.Load("Effect/DeathColor_Stone") as Material;
			if(SkillCircle==null)SkillCircle = Resources.Load("Effect/AbilityCircle") as GameObject;
			if(SkillCircle!=null)
			{
				SkillCircle = Instantiate(SkillCircle) as GameObject;
				SkillCircleRadius = SkillCircle.transform.localScale;
				SkillCircle.SetActive(false);
			}
			if(HitEffectFire==null)HitEffectFire = Resources.Load("Effect/HIT_LightEffect/Effect_Hit_Fire_Common") as GameObject;
			if(HitEffectFire!=null)
			{
				HitEffectFire = Instantiate(HitEffectFire) as GameObject;
				HitEffectFireAnims = HitEffectFire.GetComponentsInChildren<NcCurveAnimation>();
				HitEffectFire.SetActive(false);
			}
			if(HitEffectMag==null)HitEffectMag = Resources.Load("Effect/HIT_LightEffect/Effect_Hit_Mag_Common") as GameObject;
			if(HitEffectMag!=null)
			{
				HitEffectMag = Instantiate(HitEffectMag) as GameObject;
				HitEffectMagAnims = HitEffectMag.GetComponentsInChildren<NcCurveAnimation>();
				HitEffectMag.SetActive(false);
			}
			if(HitEffectPhy==null)HitEffectPhy = Resources.Load("Effect/HIT_LightEffect/Effect_Hit_phy_Common") as GameObject;
			if(HitEffectPhy!=null)
			{
				HitEffectPhy = Instantiate(HitEffectPhy) as GameObject;
				HitEffectPhyAnims = HitEffectPhy.GetComponentsInChildren<NcCurveAnimation>();
				HitEffectPhy.SetActive(false);
			}
			if(HitEffectPoi==null)HitEffectPoi = Resources.Load("Effect/HIT_LightEffect/Effect_Hit_Poisoning_Common") as GameObject;
			if(HitEffectPoi!=null)
			{
				HitEffectPoi = Instantiate(HitEffectPoi) as GameObject;
				HitEffectPoiAnims = HitEffectPoi.GetComponentsInChildren<NcCurveAnimation>();
				HitEffectPoi.SetActive(false);
			}
			if(GlobalEffect==null)GlobalEffect = GameObject.Find("GatheringRay");
			if(GlobalAudio==null)
				GlobalAudio = ResourcesManager.GetInstance.GetAudioClipSE (SoundConstant.SE_SKILL_ACTION); 
//				GlobalAudio = Resources.Load("Audios/bf233_se_skill_action") as AudioClip;
			if (TipFingerAudio == null)
				TipFingerAudio = ResourcesManager.GetInstance.GetAudioClipSE (SoundConstant.SE_SKILL_RELEASE01); 
//				TipFingerAudio = Resources.Load("Audios/SE_skillrelease01") as AudioClip;
			if(KnifeAudioClip==null)
				KnifeAudioClip = ResourcesManager.GetInstance.GetAudioClipSE ("MetalLightSliceFlesh2"); 
//				KnifeAudioClip = Resources.Load("Audios/MetalLightSliceFlesh2") as AudioClip;
			if(ThunderEffect!=null)
				ThunderEffect = Resources.Load("Effect/CleopatraVII_Skill03_new") as GameObject;
			if(ArthurSkill01!=null)
				ArthurSkill01 = Resources.Load("Effect/KingArthur_Skill01_new") as GameObject;
			TerrainObj = BattleLoader.GetInstance.terrainGo;
			TerrainRenders = TerrainObj.GetComponentsInChildren<Renderer>();
			StartCoroutine(OnBattleCameraDone());
			SoundManager.GetInstance.PlayBGM (SoundConstant.BGM_BATTLE3);
		}
	
	}

	void Start()
	{
		Init () ;
		InitFingerEvents();
		StartCoroutine(EnergyRoutine());
        SpawnManager.SingleTon().InitBattleData();
        SpawnManager.SingleTon().CreateBattleMatrixs(4, 4);
        InitUnits();
		InitEnergy();
		UIbattleInit.GetInstance.Init ();
	}


	public bool IsPause;
	public float PressTime = 0;
	public bool IsMouseDown = false;
	public float MaxPressTime = 1;
	public Vector3 StartPressPos;
	void Update()
	{
		if(Input.GetKey(KeyCode.H))
		{
			if(!IsPause)
			{
				IsPause = true;
			}
		}
		if(Input.GetMouseButtonDown(0))
		{
			m_BeginDelay = 0;
		}
		if(IsBegin)
		{
			DuringSinceBattleBegin += Time.deltaTime;
			if (isHeroMournfulSong)
			{
				currHeroMournfulSongCooldown = currHeroMournfulSongCooldown + Time.deltaTime;
			}
			if (isHeroMournfulSong && currHeroMournfulSongCooldown >= HeroMournfulSongCooldown)
			{
				restoreDamage();
			}
			FreeLines();
			if(EnemyUnits.Count == 0)
			{
				StartCoroutine(ShowResultPanel());
				PlayWinEffect();
				IsBegin = false;
				foreach(Unit unit in MyUnits)
				{
					unit.onOver();
				}
			}
			else if(MyUnits.Count == 0)
			{
				StartCoroutine(ShowResultPanel());
				PlayDefeatEffect();
				IsBegin = false;
				foreach(Unit unit in EnemyUnits)
				{
					unit.onOver();
				}
			}
			if(Input.GetMouseButtonDown(0))
			{
				IsMouseDown = true;
				PressTime = 0;
				StartPressPos = Input.mousePosition;
			}
			else if(Input.GetMouseButton(0))
			{
				if(Vector3.Distance(StartPressPos,Input.mousePosition) > 20)
				{
					PressTime = 0;
				}
				if(IsMouseDown)PressTime += Time.deltaTime;
				if(PressTime>MaxPressTime)
				{
					PressTime = 0;
					IsMouseDown = false;
					OnFingerLongTap();
				}
			}
			else
			{
				PressTime = 0;
			}

		}
	}

	float m_BeginDelay = 8.5f;
	IEnumerator OnBattleCameraDone()
	{
		while(m_BeginDelay>0)
		{
			m_BeginDelay-=Time.deltaTime;
			yield return null;
		}
		CameraFollow.SingleTon().StartFollow();
		BattleBegin();
	}

	public void PlayHitEffect(Vector3 pos,int nature)
	{
//		Debug.Log(nature);
		switch(nature)
		{
			case 0:_PlayHitEffect(HitEffectFire,HitEffectFireAnims,pos);break;
			case 1:_PlayHitEffect(HitEffectMag,HitEffectMagAnims,pos);break;
			case 2:_PlayHitEffect(HitEffectPoi,HitEffectPoiAnims,pos);break;
			default:_PlayHitEffect(HitEffectPhy,HitEffectPhyAnims,pos);break;
		}		
	}

	void _PlayHitEffect(GameObject effect,NcCurveAnimation[] ncAnims,Vector3 pos)
	{
		if(effect!=null)
		{
			if(!effect.activeInHierarchy)effect.SetActive(true);
			effect.transform.position = pos;
			foreach(NcCurveAnimation anim in ncAnims)
			{
				anim.ResetAnimation();
			}
		}
	}

	IEnumerator ShowResultPanel()
	{
		yield return new WaitForSeconds(3.5f);
		if(DataManager.me!=null)DataManager.getBattleUIData().ShowBattleUI(false);
		if(DataManager.me!=null)DataManager.getBattleUIData().ShowResllt(true);
	}

	public void ChangeSelectHero(Unit unit)
	{
		if(CurrentSelectHero!=unit)
		{
			if(CurrentSelectHero!=null && CurrentSelectHero.UnitMat!=null)
			{
				CurrentSelectHero.UnitMat.ShowDefaultMaterial();
				CurrentSelectHero.transform.localScale = Vector3.one ;
			}
			if(unit != null && unit.UnitMat!=null)
			{
				unit.UnitMat.ShowOutlineMaterial();
				unit.transform.localScale = Vector3.one * 1.3f;
			}
			CurrentSelectHero = unit;
		}
	}

	void InitEnergy()
	{
//		Debug.Log("SpawnManager.SingleTon().PlayerHeros.Count:" + SpawnManager.SingleTon().PlayerHeros.Count);
		MaxEnergy = 400;
		CurrentEnergy = 300;
		EnergyRecover = BaseEnergyRecover * SpawnManager.SingleTon().PlayerHeros.Count;
		EnemyMaxEnergy = BaseEnergy * SpawnManager.SingleTon().EnemyHeros.Count;
		EnemyEnergyRecover = BaseEnergyRecover * SpawnManager.SingleTon().EnemyHeros.Count;
	}
	
	public void PlayTipFingerGestureLiner()
	{
		if(TipFingerGestureLiner!=null)
		{
			GameObject go = Instantiate(TipFingerGestureLiner) as GameObject;
			AudioManager.PlaySound(TipFingerAudio);
			recursionResetLayer(go,8);
			go.transform.parent = FingerEffectTrans;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one * 100;
			go.AddComponent<DestoryForTime>();
		}
	}

	public void PlayTipFingerGestureCircle()
	{
		if(TipFingerGestureCircle!=null)
		{
			GameObject go = Instantiate(TipFingerGestureCircle) as GameObject;
			AudioManager.PlaySound(TipFingerAudio);
			recursionResetLayer(go,8);
			go.transform.parent = FingerEffectTrans;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one * 100;
			go.AddComponent<DestoryForTime>();
		}
	}

	public void PlayTipFingerGestureLongPush()
	{
		if(TipFingerGestureLongPush!=null)
		{
			GameObject go = Instantiate(TipFingerGestureLongPush) as GameObject;
			AudioManager.PlaySound(TipFingerAudio);
			recursionResetLayer(go,8);
			go.transform.parent = FingerEffectTrans;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one * 100;
			go.AddComponent<DestoryForTime>();
		}
	}

	public void PlayTipFingerGestureDoubleClick()
	{
		if(TipFIngerGestureDubleClick!=null)
		{
			GameObject go = Instantiate(TipFIngerGestureDubleClick) as GameObject;
			AudioManager.PlaySound(TipFingerAudio);
			recursionResetLayer(go,8);
			go.transform.parent = FingerEffectTrans;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one * 100;
			go.AddComponent<DestoryForTime>();
		}
	}

	void PlayWinEffect()
	{
		if(WinEffect!=null)
		{
			GameObject go = Instantiate(WinEffect) as GameObject;
			recursionResetLayer(go,8);
			go.transform.parent =UIbattleInit.GetInstance.transform;
			go.transform.localPosition = Vector3.zero;
			Destroy(go,5);
//			go.AddComponent<DestoryForTime>();
			ShowHeroOverlayBar(false);
		}
	}

	void PlayDefeatEffect()
	{
		if(DefeatEffect!=null)
		{
			GameObject go = Instantiate(DefeatEffect) as GameObject;
			recursionResetLayer(go,8);
			go.transform.parent =UIbattleInit.GetInstance.transform;
			go.transform.localPosition = Vector3.zero;
			go.AddComponent<DestoryForTime>();
			ShowHeroOverlayBar(false);
		}
	}

	IEnumerator PlayStartEffect()
	{
		yield return new WaitForSeconds(0.1f);
		GameObject go = Instantiate(StartEffect,Camera.main.transform.position,Quaternion.identity) as GameObject;
		recursionResetLayer(go,8);
		go.transform.parent = UIbattleInit.GetInstance.transform;
		go.transform.localPosition = Vector3.zero;
        DestoryForTime dft = go.AddComponent<DestoryForTime>();
        if (dft != null)
        {
            int n = 1;
            dft.mObj = n; 
        }
//		BattleController.SingleTon().ActiveCameraControll ();
//		BattleBegin();
	}

	public static void recursionResetLayer(GameObject go, int nLayer)
	{
		if (go == null)
			return;
		
		if (go.transform.childCount == 0)
		{
			go.layer = nLayer;
			return;
		}
		
		Transform tfTemp = null;
		for (int i = 0; i < go.transform.childCount; i++)
		{
			tfTemp = go.transform.GetChild(i);
			if (tfTemp.childCount == 0)
				tfTemp.gameObject.layer = nLayer;
			else
				recursionResetLayer(tfTemp.gameObject, nLayer);
		}
	}

//	float EnergyIncreasePerStep = 3;
	IEnumerator EnergyRoutine()
	{
		while(true)
		{
			if(IsBegin)
			{
				CurrentEnergy = Mathf.Clamp(CurrentEnergy + EnergyRecover,0,MaxEnergy);
				EnemyMaxEnergy = Mathf.Clamp(EnemyCurEnergy + EnemyEnergyRecover,0,EnemyMaxEnergy);
			}
			yield return new WaitForSeconds(1);
		}
	}

	public void InitFingerEvents()
	{
		SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Line, OnFingerLine);
		SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Arc, OnFingerArc);
		SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Circle, OnFingerCircle);
//		SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_SingleTap, BattleUIMgr.me.GetSkillTouchSingleTap);
		SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_DoubleTap, OnFingerDoubleTap);
//		SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_LongTap, OnFingerLongTap);
		SimpleTouch.me.enabled = true;
	}

	public bool OnFingerLine(object obj)
	{
		/*if(CurrentEnergy>=20)
		{
			CurrentEnergy -= 20;
			CBattleFightGestureData data1 = obj as CBattleFightGestureData;
			Instantiate(skill0,data1.listVPt[0],Quaternion.identity);
			Debug.Log(data1.listVPt[0]);
			Collider[] colls = Physics.OverlapSphere(new Vector3(data1.listVPt[0].x,data1.listVPt[0].y+1,data1.listVPt[0].z),4);
			StartCoroutine(BeenFly(2,colls,data1.listVPt[0]));
			//Debug.Log("OnFingerLine.............................");
		}*/
		//Debug.Log("Line");
		doSkillByFingerType(1,obj);
		return true;
	}

	public bool OnFingerArc(object obj)
	{
		//Debug.Log ("Arc");
		doSkillByFingerType(2,obj);
		return true;
	}

	public bool OnFingerCircle(object obj)
	{
		/*if(CurrentEnergy>=20)
		{
			CurrentEnergy -= 20;
			CBattleFightGestureData data1 = obj as CBattleFightGestureData;
			Instantiate(skill1,new Vector3(data1.listVPt[0].x,data1.listVPt[0].y+1,data1.listVPt[0].z),Quaternion.identity);
			Collider[] colls = Physics.OverlapSphere(new Vector3(data1.listVPt[0].x,data1.listVPt[0].y+1,data1.listVPt[0].z),4);
			Unit unit;
			foreach(Collider col in colls)
			{
				unit = col.GetComponent<Unit>();
				if(unit!=null && unit.Alignment == _UnitAlignment.Enemy)
				{
					unit.Attribute.Hit(50);
				}
			}
		}*/
		//Debug.Log("Circle");
		doSkillByFingerType(3,obj);
		return true;
	}

	public bool  OnFingerDoubleTap(object obj)
	{
		//Debug.Log("DoubleTap");
		//Debug.Log("OnFingerDoubleTap.............................");
		doSkillByFingerType(5,obj);
		return true;
	}

//	public bool  OnFingerLongTap(object obj)
	public bool  OnFingerLongTap()
	{
		//Debug.Log("LongTap");
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<30))
		{
			LongPressPos = hit.point;
		}
		else
		{
			LongPressPos = Vector3.zero;
		}
		doSkillByFingerType(4,new Object());
		return true;
	}

	public List<Collider> flys;
	IEnumerator BeenFly(float delayTime,Collider[] colls,Vector3 centerPos)
	{
		yield return new WaitForSeconds(delayTime);
		flys = new List<Collider>();
		Unit unit;
		foreach(Collider col in colls)
		{
			unit = col.GetComponent<Unit>();
			if(unit!=null && unit.Alignment == _UnitAlignment.Player)
			{
				unit.Attribute.Hit(10);
				flys.Add(col);
				if(unit!=null){
					if(unit.Attacker!=null){
						unit.Attacker.StopAllCoroutines();
						unit.Attacker.enabled = false;
					}
					if(unit.MeleeAttacker!=null){
						unit.MeleeAttacker.StopAllCoroutines();
						unit.MeleeAttacker.enabled = false;
					}
					if(unit.Move!=null)
					{
						unit.Move.enabled = false;
//						unit.Move.m_Nav.enabled =false;
					}
				}
				Rigidbody rigid = col.gameObject.AddComponent<Rigidbody>();
				rigid.AddExplosionForce(300,centerPos,10);
				rigid.AddForce(Vector3.up * 500);
			}
		}
		StartCoroutine(_Recover());
	}

	float recoverTime = 3;
	IEnumerator _Recover()
	{
		yield return new WaitForSeconds(recoverTime);
		foreach(Collider col in flys)
		{
			Unit unit = col.GetComponent<Unit>();
			Destroy(col.GetComponent<Rigidbody>());
			if(unit!=null){
				StartCoroutine(_ReAttack(unit));
				if(unit.Move!=null)
				{
					unit.Move.enabled = true;
				}
			}
		}
	}
	
	IEnumerator _ReAttack(Unit unit)
	{
		yield return new WaitForSeconds(2);
		if(unit.Attacker!=null){
			unit.Attacker.enabled = true;
			unit.Attacker.StartAllRoutions();
		}
		if(unit.MeleeAttacker!=null){
			unit.MeleeAttacker.enabled = true;
			unit.MeleeAttacker.StartAllRoutions();
		}
	}

	public void InitUnits()
	{
		MyUnitByLines = new List<List<Unit>>();
		EnemyUnitByLines = new List<List<Unit>>();
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Soldier");

		//Hardcode,3 lines of the units
		for(int i = 0 ; i < 3 ; i ++)
		{
			List<Unit> myLine = new List<Unit>();
			MyUnitByLines.Add(myLine);
			List<Unit> enemyLine = new List<Unit>();
			EnemyUnitByLines.Add(enemyLine);
		}

		foreach(GameObject obj in objs)
		{
			UnitAttack ua = obj.GetComponent<UnitAttack>();

			if(ua!=null && ua.ShootObject==null)
			{
				if(obj.GetComponent<Unit>().UnitTroop == ARMY_TYPE.MAGIC)
				{
					ua.ShootObject = ShootObject1;
				}
				else
				{
					ua.ShootObject = ShootObject;
				}
			}
			Unit unit = obj.GetComponent<Unit>();
			if(unit==null)
			{
				continue;
			}
			if(unit.Alignment == _UnitAlignment.Player)
			{
				MyUnitByLines[unit.Line].Add(unit);
				MyUnits.Add(obj.GetComponent<Unit>());
				obj.layer = MyBaseLayer + unit.Line;
				MaxPlayerFighting += unit.Attribute.HP;
				unit.Enemys = EnemyUnits;
				unit.EnemyUnitByLines = EnemyUnitByLines;
				unit.EnemyLayer = EnemyBaseLayer + unit.Line;
				if(unit.Line == 0){
					unit.EnemyLayer1 = EnemyBaseLayer + 1;
					unit.EnemyLayer2 = EnemyBaseLayer + 2;
				}
				if(unit.Line == 1){
					unit.EnemyLayer1 = EnemyBaseLayer + 0;
					unit.EnemyLayer2 = EnemyBaseLayer + 2;
				}
				if(unit.Line == 2){
					unit.EnemyLayer1 = EnemyBaseLayer + 1;
					unit.EnemyLayer2 = EnemyBaseLayer + 0;
				}
			}
			else if(unit.Alignment == _UnitAlignment.Enemy)
			{
				EnemyUnitByLines[unit.Line].Add(unit);
				EnemyUnits.Add(obj.GetComponent<Unit>());
				obj.layer = EnemyBaseLayer + unit.Line;
				MaxEnemyFighting += unit.Attribute.HP;
				unit.Enemys = MyUnits;
				unit.EnemyUnitByLines = MyUnitByLines;
				unit.EnemyLayer = MyBaseLayer + unit.Line;
				if(unit.Line == 0){
					unit.EnemyLayer1 = MyBaseLayer + 1;
					unit.EnemyLayer2 = MyBaseLayer + 2;
				}
				if(unit.Line == 1){
					unit.EnemyLayer1 = MyBaseLayer + 0;
					unit.EnemyLayer2 = MyBaseLayer + 2;
				}
				if(unit.Line == 2){
					unit.EnemyLayer1 = MyBaseLayer + 0;
					unit.EnemyLayer2 = MyBaseLayer + 1;
				}
			}
			CurrentEnemyFighting = MaxEnemyFighting;
			CurrentPlayerFighting = MaxPlayerFighting;
		} 
	}

	void FreeLines()
	{
		for(int i = 0 ; i < MyUnitByLines.Count ; i ++){
			if(CheckReGroupAble(MyUnitByLines[i],EnemyUnitByLines[i]))
			{
				ReSetDefaultTargetPos(MyUnitByLines[i]);
			}
			if(CheckFreeAble(MyUnitByLines[i],EnemyUnitByLines[i]))
			{
				FreeLine(MyUnitByLines[i]);
			}
		}
		for(int i = 0 ; i < EnemyUnitByLines.Count ; i ++){
			if(CheckReGroupAble(EnemyUnitByLines[i],MyUnitByLines[i]))
			{
				ReSetDefaultTargetPos(EnemyUnitByLines[i]);
			}
			if(CheckFreeAble(EnemyUnitByLines[i],MyUnitByLines[i]))
			{
				FreeLine(EnemyUnitByLines[i]);
			}
		}
	}

	bool CheckReGroupAble(List<Unit> line,List<Unit> line1)
	{
		if(line1.Count == 0)
		{
			foreach(Unit unit in line)
			{
				if(unit.IsReGrouped)
				{
					return false;
				}
			}
		}else{
			return false;
		}
		return true;
	}

	void ReSetDefaultTargetPos(List<Unit> line)
	{ 
		List<Unit> matrix0 = new List<Unit>();
		List<Unit> matrix1 = new List<Unit>();
		List<Unit> matrix2 = new List<Unit>();
		for(int i = 0 ; i < line.Count ; i ++)
		{
			if(line[i].Matrix==0){matrix0.Add(line[i]);}
			if(line[i].Matrix==1){matrix1.Add(line[i]);}
			if(line[i].Matrix==2){matrix2.Add(line[i]);}
		}
		Vector3 pos = Vector3.zero;
		if(matrix0.Count>0)pos = matrix0[0].transform.position;
		for(int i = 0 ; i < matrix0.Count ; i ++)
		{
//			matrix0[i].Move.DefaultMoveTarget = new Vector3(pos[0]+(i%5*2),matrix0[i].Move.DefaultMoveTarget[1],pos[2]+(i/5)*2);
//			matrix0[i].Move.m_Nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
			matrix0[i].Move.CurrentTargetPos = new Vector3(pos[0]+(i%5*2),matrix0[i].Move.DefaultMoveTarget[1],pos[2]+(i/5)*2);
			matrix0[i].IsReGrouped = true;
			if(matrix0[i].UnitStatus!=_UnitStatus.Run)matrix0[i].onMove();
		}
		if(matrix1.Count>0)pos = matrix1[0].transform.position;
		for(int i = 0 ; i < matrix1.Count ; i ++)
		{
//			matrix1[i].Move.DefaultMoveTarget = new Vector3(pos[0]+(i%5*2),matrix1[i].Move.DefaultMoveTarget[1],pos[2]+(i/5)*2);
//			matrix1[i].Move.m_Nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
			matrix1[i].Move.CurrentTargetPos = new Vector3(pos[0]+(i%5*2),matrix1[i].Move.DefaultMoveTarget[1],pos[2]+(i/5)*2);
			matrix1[i].IsReGrouped = true;
			if(matrix1[i].UnitStatus!=_UnitStatus.Run)matrix1[i].onMove();
		}
		if(matrix2.Count>0)pos = matrix2[0].transform.position;
		for(int i = 0 ; i < matrix2.Count ; i ++)
		{
//			matrix2[i].Move.DefaultMoveTarget = new Vector3(pos[0]+(i%5*2),matrix2[i].Move.DefaultMoveTarget[1],pos[2]+(i/5)*2);
//			matrix2[i].Move.m_Nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
			matrix2[i].Move.CurrentTargetPos = new Vector3(pos[0]+(i%5*2),matrix2[i].Move.DefaultMoveTarget[1],pos[2]+(i/5)*2);
			matrix2[i].IsReGrouped = true;
			if(matrix2[i].UnitStatus!=_UnitStatus.Run)matrix2[i].onMove();
		} 
	}

	bool CheckFreeAble(List<Unit> line,List<Unit> line1){
		if(line1.Count>0)
		{
			return false;
		}
		int acc = 0;
		foreach(Unit unit in line)
		{
			if(unit.UnitStatus == _UnitStatus.Arrive)
			{
				acc ++;
			}
		}
		if(acc > line.Count * 0.8f){
			return true;
		}else{
			return false;
		}
	}

	void FreeLine(List<Unit> line)
	{
		foreach(Unit unit in line)
		{
			if(unit.GlobleScanMode ==  _GlobleScanMode.Free)return;
			unit.GlobleScanMode =  _GlobleScanMode.Free;
			unit.IsReGrouped = false;
			if(unit.MeleeAttacker != null)
			{
				unit.MeleeAttacker.GlobleScanMode = _GlobleScanMode.Free;
				unit.MeleeAttacker.ResumeAttack();
			}
			if(unit.Attacker != null)
			{
				unit.Attacker.GlobleScanMode = _GlobleScanMode.Free;
				unit.Attacker.ResumeAttack();
			}
		}
	}

	public void BattleBegin()
	{
		StartCoroutine(PlayStartEffect());
		if(SpawnManager.SingleTon().BattleStartClip!=null)AudioManager.PlaySound(SpawnManager.SingleTon().BattleStartClip);
		AudioManager.SingleTon().musicSource.volume = 0.1f;
		StartCoroutine(_DelayShowHealthBar());
		IsBegin = true;
		AudioManager.SingleTon().PlayWhoopAudio();
		_Begin();
	}

	IEnumerator _DelayShowHealthBar()
	{
		yield return new WaitForSeconds(3);
		foreach(Unit u in EnemyUnits)
		{
			ActiveScripts(u);
			u.Move.QuickMove();
			if(u.IsHero && u.Attribute.OverlayBar!=null && u.Attribute.OverlayBar.HealthBarPos!=null)
				u.Attribute.OverlayBar.HealthBarPos.SetActive(true);
		}
		foreach(Unit u in MyUnits)
		{
			ActiveScripts(u);
			u.Move.QuickMove();
			if(u.IsHero && u.Attribute.OverlayBar!=null && u.Attribute.OverlayBar.HealthBarPos!=null)
				u.Attribute.OverlayBar.HealthBarPos.SetActive(true);
		}
	}



	public string TimeStr = "";
	void _Begin()
	{ 
		foreach(Unit u in EnemyUnits)
		{
			u.UnitStatus = _UnitStatus.Run;
			u.Move.MoveToDefaultTarget();
            if (u.Attacker != null)
            {
                u.Attacker.enabled = true;
            }
            if (u.MeleeAttacker != null)
            {
                u.MeleeAttacker.enabled = true;
            } 
		}
		foreach(Unit u in MyUnits)
		{
			u.UnitStatus = _UnitStatus.Run;
			u.Move.MoveToDefaultTarget();
            if (u.Attacker != null)
            {
                u.Attacker.enabled = true;
            }
            if (u.MeleeAttacker != null)
            {
                u.MeleeAttacker.enabled = true;
            } 
		}
	}

	void ActiveScripts(Unit unit)
	{
//		if(unit.NavAgent!=null)unit.NavAgent.enabled = true;
		if(unit.Move!=null)unit.Move.enabled = true;
		if(unit.Attacker!=null)unit.Attacker.enabled = true;
		if(unit.MeleeAttacker!=null)unit.MeleeAttacker.enabled = true;
		if(unit.Attribute!=null)unit.Attribute.enabled = true;
	}

	public GameObject GetHeroById(int heroId)
	{
		foreach(Unit unit in SpawnManager.SingleTon().PlayerHeros)
		{
			if(unit.HeroId == heroId)
			{
				return unit.gameObject;
			}
		}
		return null;
	}

	public void BackToMainCity()
	{
        SLG.GlobalEventSet.FireEvent(SLG.eEventType.ChangeScene, new SLG.EventArgs(MainController.SCENE_MAINCITY));
		//Application.LoadLevel("MainCity");
	}

//	void OnGUI()
//	{
//		if(GUI.Button(new Rect(10,70,100,30),"Shake!"))
//		{
//			CameraShake.Shake();
//		}
//		if(GUI.Button(new Rect(10,70,100,30),"Reload!"))
//		{
//			Application.LoadLevel(2);
//		}
//		if(GUI.Button(new Rect(10,100,100,30),"Pause!"))
//		{
////			IsBegin = false;
//			if(Time.timeScale == 0.3f)
//			{
//				Time.timeScale = 1;
//			}
//			else
//			{
//				Time.timeScale = 0.3f;
//			}
//		}
//	}

	public void doSkillByFingerType(int fingerType,object obj)
	{
		Debug.Log("doSkillByFingerType");

		if (BattleController.SingleTon().IsBegin == false)
			return;
		 
		if (BattleController.SingleTon().gSkilling)
			return;

		int heroId = DataManager.getBattleUIData().GetCurHeroId();
		
		int nCount = SpawnManager.SingleTon().PlayerHeros.Count;
		for (int i = 0;i < nCount;i++)
		{
			Unit player = SpawnManager.SingleTon().PlayerHeros[i];
			if (player.HeroId == heroId)
			{
				int skillCount = player.Skill.HeroSkills.Count;

				/*for (int j = 0; j < skillCount; j++)
				{
					Debug.Log("player.Skill.HeroSkills[j]........................" + player.Skill.HeroSkills[j].SkillId);
				}*/
				for (int j = 0; j < skillCount; j++)
				{
					if ((DataMgr.CFG_FINGERTYPE)player.Skill.HeroSkills[j].FingerType == (DataMgr.CFG_FINGERTYPE)fingerType
					    && player.Skill.HeroSkills[j].CurCooldown >= player.Skill.HeroSkills[j].MaxCooldown
					    && CurrentEnergy >= player.Skill.HeroSkills[j].Cost)
					{
						//Debug.Log("fingerType............................." + fingerType);
						if ((DataMgr.CFG_FINGERTYPE)fingerType == DataMgr.CFG_FINGERTYPE.CIRCLE ||(DataMgr.CFG_FINGERTYPE)fingerType == DataMgr.CFG_FINGERTYPE.DOUBLETAP)
							player.Skill.HeroSkills[j].CircleCenter = getCircleCenter(obj);
						else
							player.Skill.HeroSkills[j].CircleCenter = player.transform.position;

						//Debug.Log("player........." + player.name + ".........." + player.HeroTypeId + ".......skillid....." + player.Skill.HeroSkills[j].SkillId);


						//player.Skill.m_Unit = player;
						//player.Skill.m_Trans = player.transform;
						//DataManager.getSkillData().setSkillEvent(player.Skill,player.Skill.HeroSkills[j]);
						player.Skill.HeroSkills[j].onSkill(player.Skill.HeroSkills[j]);
						break;
					}
				}

			}
		}

	}

	public void ShowHeroOverlayBar(bool isShow)
	{
		if(SpawnManager.SingleTon().PlayerHeros!=null)
		{
			foreach(Unit unit in SpawnManager.SingleTon().PlayerHeros)
			{
				if(unit!=null && unit.Attribute!=null && unit.Attribute.OverlayBar!=null)
				{
					unit.Attribute.OverlayBar.gameObject.SetActive(isShow);
				}
			}
		}
		if(SpawnManager.SingleTon().EnemyHeros!=null)
		{
			foreach(Unit unit in SpawnManager.SingleTon().EnemyHeros)
			{
				if(unit!=null && unit.Attribute!=null && unit.Attribute.OverlayBar!=null)
				{
					unit.Attribute.OverlayBar.gameObject.SetActive(isShow);
				}
			}
		}
	}


	public Vector3 getCircleCenter(object obj)
	{
		CBattleFightGestureData data1 = obj as CBattleFightGestureData;
		Vector3 circleCenter = new Vector3 (data1.listVPt [0].x, data1.listVPt [0].y, data1.listVPt [0].z);
		return circleCenter;
	}


	public void restoreDamage()
	{
		currHeroMournfulSongCooldown = 0;
		isHeroMournfulSong = false;
		int nCount = SpawnManager.SingleTon().PlayerHeros.Count;
		for (int i = 0;i < nCount;i++)
		{
			Unit player = SpawnManager.SingleTon().PlayerHeros[i];
			player.Attribute.SkillDamage = 0;
			player.Attribute.Damage = player.Attribute.BaseDamage + player.Attribute.SkillDamage;
		}
	}
}
