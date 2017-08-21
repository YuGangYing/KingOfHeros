using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(Unit))]
public class UnitAttribute : MonoBehaviour {

	Unit m_Unit;
	Transform m_Trans;

	public float MaxHP;
	public float BaseMaxHP;
	public float HP;
	public float BaseHP;
    public float BaseDef; 
    public const float DEFAULTSMOVESPEED = 3.5f;
    public float BaseMoveSpeed = 3.5f;
    public float AdjustMoveSpeed;
    public float RealAdjustSpeed;
    public float BaseAttackSpeed;
    public float Damage ;
	public float BaseDamage ;
	//skill addition damage
	public float SkillDamage = 0f;  
    public float AttackRadius;   
    public float ScanRadius = 40; 
    public ARMY_TYPE m_eFigherType;
    public List<Transform> HitTargets = new List<Transform>(); 
	public Transform HeadPoint;
	public Transform ShootPoint;
	public GameObject OverlayPrefab;
    public OverlayBar OverlayBar;  
	public AudioClip ImmuneAudio;

	public Camera MainCamera;
	public Camera NGUICamera; 

 
	public bool isParry = false;
	public int parryRate = 4;

    public const float m_cMaxMoveSpeed = 10; 
    public const float m_CMaxAttackSpeed = 4; 
    public Fight.Character m_StaticData;
    //攻击速度加成百分比 
    public float m_fAttackSpeedAdd ;
    public int Nature;


	void Awake()
	{ 
		m_Trans = transform;

//		if(HitTargets.Count==0)
//		{

	    foreach (Transform tr in transform)
	    {
	        if (tr.name == "HitPoint")
	        {
            HitTargets.Add(tr);
			}else if(tr.name == "HeadPoint")
			{
				HeadPoint = tr;
			}
    	}
		if(HeadPoint==null)
			HeadPoint = transform;
		ShootPoint = GetShootPoint(transform);
		if(ShootPoint==null)
		{
			ShootPoint = transform;
		}
			
		if(HitTargets.Count==0)
		{
			HitTargets.Add (transform);
		}
//		} 
		Damage = BaseDamage + SkillDamage;
        m_Unit = GetComponent<Unit>();

	}

	Transform GetShootPoint(Transform t0)
	{
		Transform t = null;
		foreach(Transform trans in t0)
		{
			if(trans.name == "ShootPoint")
			{
				t = trans;
				break;
			}else{
				t = GetShootPoint(trans);
				if(t!=null)break;
			}
		}
		return t;
	}

	public Unit LastAttacker;
    public void OnDamage(Unit _attacker)
    {
        if (_attacker == null)
        {
            Debug.LogError("_attacker is null");
            return;
        } 
		LastAttacker = _attacker;
        float influence = Fight.FightData.getSoldierInfluence(_attacker.Attribute.m_StaticData.armyType, m_StaticData.armyType); 
         
        float cirtadd = 0;

        cirtadd = GetVlolenceAdd(_attacker);

        float tmp_damage = FormulaTool.me.NormalDamageFormula(_attacker.Attribute.BaseDamage, BaseDef, influence, cirtadd);

		if(_attacker.IsHero)
		{
			BattleController.SingleTon().PlayHitEffect(HitTargets[Random.Range(0,HitTargets.Count)].position,_attacker.Attribute.Nature);
		}
		if(_attacker.IsHero)
			Hit(tmp_damage,true); 
		else
	        Hit(tmp_damage); 
    }

	public void OnDamage(Unit _attacker,bool showUI)
	{
		if (_attacker == null)
		{
			 Debug.LogError("_attacker is null");
			return;
		} 
		LastAttacker = _attacker;
		float influence = Fight.FightData.getSoldierInfluence(_attacker.Attribute.m_StaticData.armyType, m_StaticData.armyType); 
		
		float cirtadd = 0;
		
		cirtadd = GetVlolenceAdd(_attacker);
		
		float tmp_damage = FormulaTool.me.NormalDamageFormula(_attacker.Attribute.BaseDamage, BaseDef, influence, cirtadd);
		
		if(_attacker.IsHero)
		{
			BattleController.SingleTon().PlayHitEffect(HitTargets[Random.Range(0,HitTargets.Count)].position,_attacker.Attribute.Nature);
		}
		
		Hit(tmp_damage,showUI); 
	}



    public void OnSkillDamage(Unit _attacker)
    { 
        if (_attacker == null)
        {
            Debug.LogError("_attacker is null");
            return;
        }
 
        float cirtadd = 0;

        cirtadd = GetVlolenceAdd(_attacker);

        FormulaTool.me.SkillDamageFormula(_attacker.Attribute.BaseDamage, BaseDef, 1, 1, cirtadd);
    }

    //英雄的统御力对士兵的防御和攻击加层 
    public void LeaderAbtAdd(float _leadervalue, int _level)
    {
        if (m_StaticData == null)
        {
            Debug.LogError("m_StaticData is null");
            return;
        }

        float tmp_value = FormulaTool.me.LeadershipAdd(_leadervalue, _level);

        BaseDamage = m_StaticData.baseDamage + m_StaticData.baseDamage * (tmp_value / 100);

        BaseDef = m_StaticData.baseDef + m_StaticData.baseDef * (tmp_value / 100);
    }

    private bool IsVlolence(float _vlaue, float _level, float _targetvalue, float _targetlevel)
    {
        float tmp_violencerate = FormulaTool.me.ViolenceRate(_vlaue, _level, _targetvalue, _targetlevel);

        float tmp_random = Random.Range(1, 1001);

        if (tmp_random < tmp_violencerate)
        {
            return true;
        }

        return false;
    }

    public float GetVlolenceAdd(Unit _attacker)
    {
        if (_attacker == null)
        {
            return 0 ;
        }

        //暴击加层
        float cirtadd = 0;

        //是否暴击  
        switch (_attacker.Attribute.m_StaticData.armyType)
        {
            case ARMY_TYPE.HERO:
                Fight.Hero tmp_attckerhero = (Fight.Hero)(_attacker.Attribute.m_StaticData);
                float tmp_attackcir = tmp_attckerhero.Critical;
                Fight.Hero tmp_hero = null;
                float tmp_cir = 0;
                //note: if be attacked is soldier
                if (m_StaticData.armyType == ARMY_TYPE.HERO)
                {
                    tmp_hero = (Fight.Hero)m_StaticData;
                    tmp_cir = tmp_hero.Critical;
                }
                else
                {
                    tmp_cir = 0;
                }
                if (IsVlolence(tmp_attackcir, _attacker.Attribute.m_StaticData.level, tmp_cir, m_StaticData.level))
                {
                    cirtadd = 1;
                }

                break;
        }

        return cirtadd;
    }

    public void UpgradeAttackSpeed(float _upgrandevlaue)
    {
        m_fAttackSpeedAdd = _upgrandevlaue;
    }

	public void Hit(float damage,bool showUI = false)
	{
		int damage_int = (int)damage;
		if(IsImmuneDamage)
		{
			if(showUI)
			{
				if(OverlayBar!=null)
				{
					OverlayBar.PlayTweener("Immune",Color.blue);
					if (ImmuneAudio!=null)
						AudioSource.PlayClipAtPoint(ImmuneAudio,Camera.main.transform.position);	

				}
			}
		}
		else
		{
			if(showUI)
			{
				if(damage>0)
				{
					if(OverlayBar!=null)OverlayBar.PlayTweener("-" + Mathf.Abs(damage_int).ToString(),Color.red);
				}else{
					if(OverlayBar!=null)OverlayBar.PlayTweener("+" + Mathf.Abs(damage_int).ToString(),Color.green);
				}
			}
			if (isParry) 
			{
				int rate = Random.Range (0, 1000);
				if (rate <= parryRate)
					damage = 0;
			}
			HP = Mathf.Clamp(HP-damage_int,0,MaxHP);
		}
		m_Unit.Anim.AnimMode = 1;
	}

	void Update()
	{
		if(HP <= 0 && m_Unit.UnitStatus != _UnitStatus.Dead)
		{
			/*if(IsRevivable)
			{
				Revive();
			}*/


			if (m_Unit.onBeforeDead != null)
				m_Unit.onBeforeDead();

			if (m_Unit.IsHero)
				SpawnManager.SingleTon().onBeforeDead(m_Unit);



				if (m_Unit.Skill != null && m_Unit.Skill.HeroSkills != null)
				{
					for (int i = 0; i < m_Unit.Skill.HeroSkills.Count; i++)
					{
						if (m_Unit.Skill.HeroSkills[i].isDieTrigger)
						{
							m_Unit.Skill.HeroSkills[i].isDieTrigger = false;
							m_Unit.Skill.HeroSkills[i].onSkill(m_Unit.Skill.HeroSkills[i]);
							//Debug.Log(" isDieTrigger........................." + m_Unit.name);
							break;

						}
					}
				}

				if(HP <= 0 && m_Unit.UnitStatus != _UnitStatus.Dead)
					m_Unit.onDead();

		}
		AdjustHealthBar();
	} 
 
	void AdjustHealthBar()
	{
		if(OverlayPrefab!=null && OverlayBar == null)
		{
			GameObject go = Instantiate(OverlayPrefab) as GameObject;
			go.transform.parent = NGUICamera.transform.parent;
			go.transform.localScale = Vector3.one;
			OverlayBar = go.GetComponent<OverlayBar>();
			if(m_Unit.Alignment==_UnitAlignment.Player)OverlayBar.HealthBar.spriteName = "batgrdUpBarBlue";

		}
//		else
//		{
//			if(OverlayBar!=null)
//			{
//				OverlayBar.gameObject.SetActive(false);
//			}
//		}
		if(MainCamera!=null && NGUICamera!=null && OverlayBar!=null && OverlayBar.gameObject.activeInHierarchy)
		{
			Vector3 pos = MainCamera.WorldToScreenPoint(HeadPoint.position);
			float adjustX = (pos.x  - NGUICamera.pixelWidth / 2)/ MainCamera.pixelWidth ;
			float adjustY = pos.y / MainCamera.pixelHeight ;
			float x = pos.x - MainCamera.pixelWidth / 2;
			float y = pos.y - MainCamera.pixelHeight / 2;

			OverlayBar.transform.localPosition = new Vector3(x+adjustX,y+adjustY,0);
			OverlayBar.transform.localScale = CameraFollow.SingleTon().CurrentSubCameraPos == 1 ? Vector3.one : Vector3.one * 0.5f;
			if(OverlayBar.gameObject.activeInHierarchy && OverlayBar.HealthBarPos.activeInHierarchy)OverlayBar.HealthBar.fillAmount = Mathf.Clamp(HP/MaxHP,0,1);
//			if(m_Unit.Skill!=null)
//			{
//				for(int i = 0 ; i < m_Unit.Skill.HeroSkills.Count ; i++ )
//				{ 
//					if(OverlayBar.SkillBars[i].gameObject.activeInHierarchy)OverlayBar.SkillBars[i].fillAmount = Mathf.Clamp(m_Unit.Skill.HeroSkills[i].CurCooldown/m_Unit.Skill.HeroSkills[i].MaxCooldown,0,1);
// 
//				}
//			}
		}
	}
 
	public bool IsDeadlyThrowed = false;
	public bool IsRangerMarked = false;
	public float RangerMarkedDuration = 0;
	public void OnRangerMarked(float duration)
	{
		if(IsRangerMarked)
		{
			RangerMarkedDuration = Mathf.Max(RangerMarkedDuration,duration);
		}
		else
		{
			RangerMarkedDuration = duration;
			StartCoroutine(_RangerMarkRoutine());
		}
	}

	[HideInInspector] public GameObject EffectMarkPrefab;
	[HideInInspector] private GameObject MarkPrefab;
	IEnumerator _RangerMarkRoutine()
	{
		IsRangerMarked = true;

		//if(OverlayBar!=null && OverlayBar.RangerMark!=null)OverlayBar.RangerMark.gameObject.SetActive(true);
		if (EffectMarkPrefab)
		{
			MarkPrefab = Instantiate(EffectMarkPrefab,m_Trans.position,m_Trans.rotation) as GameObject;		
			MarkPrefab.transform.parent = m_Trans;
		}

		/*while(RangerMarkedDuration>0)
		{
			RangerMarkedDuration = Mathf.Max(0,RangerMarkedDuration-Time.deltaTime);
		}*/
		yield return new WaitForSeconds(RangerMarkedDuration);
		IsRangerMarked = false;
		if (MarkPrefab)
		{
			EffectMarkPrefab = null;
			Destroy(MarkPrefab);
		}
		yield return null;
		//if(OverlayBar!=null && OverlayBar.RangerMark!=null)OverlayBar.RangerMark.gameObject.SetActive(false);
	}

	public bool IsImmuneDamage = false;
	public float ImmuneDuration = 0;
	public void OnImmuneDamage(float duration)
	{
		if(IsImmuneDamage)
		{
			ImmuneDuration = Mathf.Max(ImmuneDuration,duration);
		}
		else
		{
			ImmuneDuration = duration;
			StartCoroutine(_ImmuneDamageRoutine());
		}
	}

	IEnumerator _ImmuneDamageRoutine()
	{
		IsImmuneDamage = true;
		while(ImmuneDuration>0)
		{
			ImmuneDuration =  Mathf.Max(0,ImmuneDuration-Time.deltaTime);
			yield return null;
		}
		IsImmuneDamage = false;
	}
	

	/*public bool IsRevivable = false;
	public float RevivePercent = 0;
	public void Revive()
	{
		HP = MaxHP * RevivePercent;
		IsRevivable = false;
	}*/

	public bool IsBleed;
	public float BleedDamage;
	public void OnBleed(float damage,float damageCooldown)
	{
		if(IsBleed)
		{
			BleedDamage = Mathf.Max(BleedDamage,damage);
		}
		else
		{
			BleedDamage = damage;
			StartCoroutine(_Bleed(damageCooldown));
		}
	}

	IEnumerator _Bleed(float damageCooldown)
	{
		IsBleed = true;
		while(true)
		{
			yield return new WaitForSeconds(damageCooldown);
			if(HP>0)Hit(BleedDamage,true);
		}
	}

	//SurviveSong Friend Skill
	public bool IsSurviveSongByFriend = false;
	public float SurviveSongDamageByFriend = 0;
	public float SurviveSongDurationByFriend = 0;
	public void OnSurviveSongDamageByFriend(float damage,float damageDuration)
	{
		if(IsSurviveSongByFriend)
		{
			SurviveSongDamageByFriend = Mathf.Max(SurviveSongDamageByFriend,damage);
			SurviveSongDurationByFriend = Mathf.Max(SurviveSongDurationByFriend,damageDuration);
		}
		else
		{
			SurviveSongDamageByFriend = damage;
			SurviveSongDurationByFriend = damageDuration;
			StartCoroutine(_OnSurviveSongDamageByFriend(SurviveSongDurationByFriend));
		}
	}
	
	IEnumerator _OnSurviveSongDamageByFriend(float SurviveSongDurationByFriend)
	{
		IsSurviveSongByFriend = true;
		while(SurviveSongDurationByFriend >= 0)
		{
			if(HP>0)Hit(-SurviveSongDamageByFriend,true);
			yield return new WaitForSeconds(1f);
			SurviveSongDurationByFriend -= 1;
			//Debug.Log(m_Trans.name + "......................" + HP);
		}
		IsSurviveSongByFriend = false;
	}
     
	//SurviveSong enemy skill
	public bool IsSurviveSongByEnemy = false;
	public float SurviveSongDamageByEnemy;
	public float SurviveSongDurationByEnemy;
	public void OnSurviveSongDamageByEnemy(float damage,float damageDuration)
	{
		if(IsSurviveSongByEnemy)
		{
			SurviveSongDamageByEnemy = Mathf.Max(SurviveSongDamageByEnemy,damage);
			SurviveSongDurationByEnemy = Mathf.Max(SurviveSongDurationByEnemy,damageDuration);
		}
		else
		{
			SurviveSongDamageByEnemy = damage;
			SurviveSongDurationByEnemy = damageDuration;
			StartCoroutine(_OnSurviveSongDamageByEnemy(SurviveSongDurationByEnemy));
		}
	}
	
	IEnumerator _OnSurviveSongDamageByEnemy(float SurviveSongDurationByEnemy)
	{
		IsSurviveSongByEnemy = true;
		while(SurviveSongDurationByEnemy >= 0)
		{
			if(HP>0)Hit(SurviveSongDamageByEnemy,true);
			yield return new WaitForSeconds(1f);
			SurviveSongDurationByEnemy -= 1;
			//Debug.Log(m_Trans.name + "......................" + HP + ".............time....." + SurviveSongDurationByEnemy);
		}
		IsSurviveSongByEnemy = false;
	}

	//toxtouch
	private bool isToxTouchSkill = false;
	private float ToxTouchDuration = 0;
	private float priorSkillDamage = 0;
	private float currDamage = 0;
	public void OnToxTouchSkill(float toxDamage,float damageDuration)
	{
		if (isToxTouchSkill)
		{
			ToxTouchDuration = Mathf.Max(ToxTouchDuration,damageDuration);
			currDamage = Mathf.Max(currDamage,toxDamage);
		} else
		{
			priorSkillDamage = SkillDamage;
			currDamage = toxDamage;
			SkillDamage = SkillDamage + currDamage;
			ToxTouchDuration = damageDuration;
			StartCoroutine(_OnToxTouchSkill(ToxTouchDuration));
		}
	}
     
	IEnumerator _OnToxTouchSkill(float ToxTouchDuration)
	{
		isToxTouchSkill = true;
		Damage = BaseDamage + SkillDamage;
		//Debug.Log("_OnToxTouchSkill............................." + m_Unit.name);
		//Debug.Log("_OnToxTouchSkill............................." + Damage);
		yield return new WaitForSeconds(ToxTouchDuration);
		SkillDamage = priorSkillDamage;
		Damage = BaseDamage + SkillDamage;
		isToxTouchSkill = false;
	}
      
	public List<Material> TempMats;

	public void OnPetrifactionSkill(float damageDuration)
	{
		if(m_Unit.Attacker!=null){
			m_Unit.Attacker.StopAttack();
			m_Unit.WayPointMoveDone = true;
		}
		if(m_Unit.MeleeAttacker!=null){
			Debug.Log("StopAttack");
			m_Unit.WayPointMoveDone = true;
			m_Unit.MeleeAttacker.StopAttack();
		}
		if(m_Unit.Move!=null)
		{
			Debug.Log("StopMove");
			m_Unit.Move.Movable = false;
//			m_Unit.Move.m_Nav.enabled =false;
		}
		if (m_Unit.GetComponentInChildren<Animation>() != null)
		{
			m_Unit.GetComponentInChildren<Animation>().enabled = false;
		}
		Renderer[] rends = m_Unit.GetComponentsInChildren<Renderer>();
		TempMats = new List<Material>();
		foreach(Renderer rend in rends)
		{
			TempMats.Add(rend.material);
			rend.material = BattleController.SingleTon().DeathMatStone;
		}
		StartCoroutine(_OnPetrifactionSkill(damageDuration));
	}

	IEnumerator _OnPetrifactionSkill(float damageDuration)
	{
		yield return new WaitForSeconds(damageDuration);
		if(m_Unit.Attacker!=null){
			m_Unit.Attacker.ResumeAttack();
		}
		if(m_Unit.MeleeAttacker!=null){
			m_Unit.MeleeAttacker.ResumeAttack();
		}
		if(m_Unit.Move!=null)
		{
			m_Unit.Move.Movable = true;
//			m_Unit.Move.m_Nav.enabled =true;
		}    
		if (m_Unit.GetComponentInChildren<Animation>() != null)
		{
			m_Unit.GetComponentInChildren<Animation>().enabled = true;
		} 
		Renderer[] rends = m_Unit.GetComponentsInChildren<Renderer>();
		for(int i = 0; i < rends.Length ; i++)
		{
			rends[i].material = TempMats[i];
		}
	}


	public bool isPromoteDamage = false;
	public float priorDamage = 0f;
	public float PromoteDuration = 0f;
	
	public void OnPromoteDamage(float rate,float duration)	
	{
		if (!isPromoteDamage) 
		{
			PromoteDuration = duration;
			priorDamage = Damage;
			//Debug.Log("before....heroUnit.Attribute.MaxHP............name...." + m_Unit.name + ".............." + Damage);
			Damage = Damage + Damage * rate;			
			//Debug.Log("after....heroUnit.Attribute.MaxHP............name...." + m_Unit.name + ".............." + Damage);
			StartCoroutine (_onPromoteDamageCoroutine(duration));
		}
	}	
	
	IEnumerator _onPromoteDamageCoroutine(float duration)
	{
		isPromoteDamage = true;
		/*while (PromoteDuration > 0)
		{
			PromoteDuration = Mathf.Max(0,PromoteDuration - Time.deltaTime);
			yield return null;
		}*/
		yield return new WaitForSeconds(duration);
		isPromoteDamage = false;
		Damage = priorDamage;
	}


	public bool isOnHeroMournfulSong = false;
	public float priorHeroMournfulSongDamage = 0f;
	
	public void OnHeroMournfulSong(float rate,float duration)	
	{
		if (!isOnHeroMournfulSong) 
		{
			priorHeroMournfulSongDamage = Damage;
			//Debug.Log("before....heroUnit.Attribute.MaxHP............name...." + m_Unit.name + ".............." + Damage);
			Damage = Damage + Damage * rate;			
			//Debug.Log("after....heroUnit.Attribute.MaxHP............name...." + m_Unit.name + ".............." + Damage);
			StartCoroutine (_OnHeroMournfulSong(duration));
		}
	}	
	
	IEnumerator _OnHeroMournfulSong(float duration)
	{
		isOnHeroMournfulSong = true;
		yield return new WaitForSeconds(duration);
		isOnHeroMournfulSong = false;
		Damage = priorHeroMournfulSongDamage;

		/*if (m_Unit.Skill != null && m_Unit.Skill.HeroSkills != null)
		{
			for (int i = 0; i < m_Unit.Skill.HeroSkills.Count; i++)
			{
				if (m_Unit.Skill.HeroSkills[i].isDieTrigger)
				{
					m_Unit.Skill.HeroSkills[i].isDieTrigger = false;
					//m_Unit.onDead();
					break;					
				}
			}
		}*/
	}


}

