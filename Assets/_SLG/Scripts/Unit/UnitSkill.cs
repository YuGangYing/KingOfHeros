using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fight;
using DataMgr;

public class UnitSkill : MonoBehaviour 
{
	//skill list 
	public SkillBase[] m_SkillBaseList;

	public Unit m_Unit;
	public Transform m_Trans;
	public UnitAnim m_Anim;

	public List<DataMgr.Skill> HeroSkills = null;
	public bool IsAuto = true;
	public float Delay;

	public GameObject SkillPrefab;

//	public float MaxCooldown = 2;//Global Cooldown
//	public float CurCooldown = 0;

	public bool IsSkilling = false;
	public bool IsSkillDone = false;

	public GameObject GlobalSkillHero;


	public delegate void OnCooldownOver(DataMgr.Skill skill);
	public OnCooldownOver onCooldownOver;

	public DataMgr.Skill currSkill;

	void Awake()
	{
		Delay = Random.Range(1, 4);
		m_Unit = GetComponent<Unit>();
		m_Anim = GetComponent<UnitAnim>();
		m_Trans = transform;
		if (m_Unit != null && m_Unit.Skill == null) m_Unit.Skill = this;
	} 

	void Update () 
	{
		if(BattleController.SingleTon().IsBegin)
		{
			SkillRoutine();
		} 
	}
	
	void SkillRoutine()
	{
//		HeroCooldown();
		foreach(DataMgr.Skill skill in HeroSkills)
		{
			if (skill != null)
			{
				Cooldown(skill);
				if(skill.IsActive)
				{
	//				Debug.Log("Skill");
					//Cooldown(skill);
					UseSkill(skill);
				}
			}
		}
	}

//	void HeroCooldown()
//	{
//		if(CurCooldown < MaxCooldown){
//			CurCooldown = Mathf.Clamp(CurCooldown+Time.deltaTime,0,MaxCooldown);
//		}
//	}

	void Cooldown(DataMgr.Skill skill)
	{
//		Debug.Log("skill.CurCooldown:" + skill.CurCooldown + " ;skill.MaxCooldown: " + skill.MaxCooldown + ";bool:" + (skill.CurCooldown - skill.MaxCooldown));
		if(skill.CurCooldown < skill.MaxCooldown){

			skill.CurCooldown = Mathf.Clamp(skill.CurCooldown+Time.deltaTime,0,skill.MaxCooldown);
		}
		else
		{
			if (onCooldownOver != null)
				onCooldownOver(skill);
		}
	}

	void UseSkill(DataMgr.Skill skill)
	{
//		if(skill.CurCooldown==skill.MaxCooldown && CurCooldown==MaxCooldown && !IsSkilling){
		if(skill.CurCooldown==skill.MaxCooldown && !IsSkilling){
//			switch(skill.Type)
//			{
//				case SkillType.Circle:CircleSkill(skill);break;
//				case SkillType.CircleFly:CircleExploseSkill(skill);break;
//				case SkillType.Whirlwind:MeleeWhirlwindSkill(skill);break;
//				default:break;
//			}
			skill.onSkill(skill);
		}
	}

	public void PlayGlobalSkillEffect(DataMgr.Skill skill)
	{
		BattleController.SingleTon().gSkilling = true;
		StartCoroutine(DarkTerrain(0.7f));
		StartCoroutine(_PlayGlobalSkillEffect(skill));
	}

	IEnumerator _PlayGlobalSkillEffect(DataMgr.Skill skill)
	{
		if(SpawnManager.SingleTon().BaseCastSkillEffect!=null)
		{
			GameObject go = Instantiate(SpawnManager.SingleTon().BaseCastSkillEffect,m_Trans.position,Quaternion.identity) as GameObject;
			go.transform.parent = m_Trans;
		}
//		Debug.Log(skill.FingerType);
		if(skill.FingerType == 1){
//			Vector3 pos = m_Trans.position;
//			pos = new Vector3(pos.x,pos.y+20,pos.z);
			BattleController.SingleTon().PlayTipFingerGestureLiner();
//
//			GameObject go = Instantiate(BattleController.SingleTon().TipFingerGestureLiner,m_Trans.position,Quaternion.identity) as GameObject;
//			go.transform.localScale = Vector3.one * 2;
		}
		else if(skill.FingerType == 3)
		{
			BattleController.SingleTon().PlayTipFingerGestureCircle();
		}
		else if(skill.FingerType == 4)
		{
			BattleController.SingleTon().PlayTipFingerGestureLongPush();
		}
		else if(skill.FingerType == 5)
		{
			BattleController.SingleTon().PlayTipFingerGestureDoubleClick();
		}
//			BattleController.SingleTon().PlayTipFingerGestureLiner();
		float waitTime = 0.5f;
		if(skill.GlobalSkillEffect!=null)
			waitTime = 0.7f;
		else
			waitTime = 0;
		if(skill.SkillId==20032)
		{
//			AudioManager.PlaySound(BattleController.SingleTon().ThunderAudioClipStart);
		}
		yield return new WaitForSeconds(waitTime);

		if((CFG_PLAYEFFECT)skill.IsGlobal == (CFG_PLAYEFFECT)1)
		//if(skill.IsGlobal)
		{
			Debug.Log("skill.IsGlobal.................." + skill.IsGlobal);
			RealTimeUtility rtu = skill.GlobalSkillEffect.GetComponent<RealTimeUtility>();
			if(skill.SkillId==20032)Thunder(rtu,skill.Damage,skill.SkillAttackRadius);
			rtu.unitkill = this;
			rtu.currskill = skill; 
			rtu.m_Trans = m_Trans;
			rtu.CurrentHero = GlobalSkillHero;

//			rtu.CurrentAnimState = skill.GlobalSkillAnimName;
			rtu.Play();
		}else if((CFG_PLAYEFFECT)skill.IsGlobal == (CFG_PLAYEFFECT)0){
		//}else{
			Debug.Log("skill.IsGlobal.................." + skill.IsGlobal);
			RealTimeUtility rtu = skill.GlobalSkillEffect.GetComponent<RealTimeUtility>();
			if(skill.SkillId==20032)Thunder(rtu,skill.Damage,skill.SkillAttackRadius);
			rtu.unitkill = this;
			rtu.currskill = skill; 
			rtu.m_Trans = m_Trans;
			string skillName = "";
			/*if(HeroSkills.IndexOf(skill)==0)
			{
				skillName = "Skill01";
			}else{
				skillName = "Skill02";
			}*/
			skillName = "Skill0" + skill.SkillAnimIndex;
			rtu.Play1(m_Unit.Anim.m_AnimationList[0],skillName);
		}
		/*else if((CFG_PLAYEFFECT)skill.IsGlobal == (CFG_PLAYEFFECT)2)
		{
			RealTimeUtility rtu = skill.GlobalSkillEffect.GetComponent<RealTimeUtility>();
			rtu.CurrentHero = GlobalSkillHero;
			rtu.Play();

			yield return new WaitForSeconds(0.8f);
			string skillName = "";
			if(HeroSkills.IndexOf(skill)==0)
			{
				skillName = "Skill01";
			}else{
				skillName = "Skill02";
			}
			rtu.Play1(m_Unit.Anim.m_AnimationList[0],skillName);
		}*/
	}

	public float thunderRadio = 10;
	public void Thunder(RealTimeUtility rtu,float damage,float radius)
	{
		Debug.Log("Thunder");

		Collider[] cols = Physics.OverlapSphere(BattleController.SingleTon().LongPressPos,radius);
		List<Unit> cols1 = new List<Unit>();
//		cols1.AddRange(cols);
		foreach(Collider c in cols)
		{
			Unit unit = c.GetComponent<Unit>();
			if(unit!=null && unit.Alignment != m_Unit.Alignment)
			{
				cols1.Add(unit);
			}
		}

		for(int i = 0 ; i < cols1.Count ; i ++)
		{
			Unit unit = cols1[i];
			int j = Random.Range(0,cols1.Count);
			Unit unit1 = cols1[j];
			cols1[i] = unit1;
			cols1[j] = unit;
		}
		Vector3 pos;
		int hitIndex;
		for(int i = 0 ; i < 20 ; i ++)
		{
			if(i < cols1.Count)
			{
//				hitIndex = Random.Range(0,m_Unit.Enemys.Count);
				rtu.PlayThunder(cols1[i].transform.position, i*0.1f,cols1[i].Attribute,m_Unit,damage);
//				cols1[i].Attribute.LastAttacker = m_Unit;
//				cols1[i].Attribute.Hit(2000,true);
			}else{
				pos = BattleController.SingleTon().LongPressPos + new Vector3(Random.Range(-10,10),0,Random.Range(-10,10));
				rtu.PlayThunder(pos, i*0.1f,null,null,0);
			}
//			rtu.PlayThunder(m_Trans.position + new Vector3(Random.Range(-10,10),0,Random.Range(-10,10)), i*0.1f);
		}
	}

	float startT = 0;
	IEnumerator DarkTerrain(float delay)
	{
		float t = 0;
		float t1 = 0;
		bool isPlay = true;
		while(isPlay)
		{
			if(BattleController.SingleTon().TerrainRenders!=null)
			{
				t1 = Mathf.Max(0,1 - t);
				startT = t1;
				foreach(Renderer r in BattleController.SingleTon().TerrainRenders)
				{
					r.material.SetColor("_Color",Color.white * t1);
				}
			}
			if(t >= delay)
			{
				StartCoroutine(LightTerrain());
				isPlay = false;
			}
			t += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator LightTerrain()
	{
		yield return new WaitForSeconds(1);
		float t = 0;
		float t1 = 0;
		bool isPlay = true;
//		Debug.Log("LightTerrain");
		if(BattleController.SingleTon().GlobalEffect)BattleController.SingleTon().GlobalEffect.SetActive(false);
		while(isPlay)
		{
			t += Time.deltaTime;
			t1 = Mathf.Min(1,t+startT);
//			Debug.Log("LightTerrain1");
			foreach(Renderer r in BattleController.SingleTon().TerrainRenders)
			{
				r.material.SetColor("_Color",Color.white * t1);
			}
			if(t>=2)
			{
				isPlay = false;
			}	
			yield return null;
		}
	}

	/*
	 * 1.Single 2.Remote 3.Hero target only 4.One-Time ignore defense Damage 5.apend a timed mark to target
	*/
	Unit m_targetUnit = null;
	UnitRepelEffect repel;
	public void RangerMark(DataMgr.Skill skill)
	{
		StartCoroutine(_RangerMark(skill));
	}

	IEnumerator _RangerMark(DataMgr.Skill skill)
	{

		//m_targetUnit = null;

		scaneRadius = skill.SkillAttackRadius;
		/*colls = Physics.OverlapSphere(m_Trans.position,scaneRadius);

		foreach(Collider col in colls)
		{
			//if(m_targetUnit!=null && m_targetUnit.Alignment != m_Unit.Alignment && m_targetUnit.IsHero)
			if(col.GetComponent<Unit>()!=null && col.GetComponent<Unit>().Alignment != m_Unit.Alignment && col.GetComponent<Unit>().IsHero)
			{
				m_targetUnit = col.GetComponent<Unit>();
				break;
			}
		}*/


		m_targetUnit = FindHero(m_Unit.EnemyLayer,scaneRadius,1);
		if (m_targetUnit == null)
		{
			m_targetUnit = FindHero(m_Unit.EnemyLayer1,scaneRadius,2);
		}
		if (m_targetUnit == null)
		{
			m_targetUnit = FindHero(m_Unit.EnemyLayer2,scaneRadius,2);
		}


		if (m_targetUnit == null)
		{
			m_targetUnit = m_Unit.Attacker.AttackTarget;
		}

		//GameObject attackEffect = Instantiate(skill.SkillPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
		//StartCoroutine(_Destory(attackEffect,skill.SkillAnimDuration));
		//if((CFG_PLAYEFFECT)skill.IsGlobal == (CFG_PLAYEFFECT)1)
			//yield return new WaitForSeconds(0.7f);	

		if (skill.ShootEffectPrefab && m_targetUnit != null)
		{
			m_Unit.transform.LookAt(m_targetUnit.transform);
			currSkill = skill;
			m_Unit.StopMoveAndAttack();
			m_Unit.transform.LookAt(m_targetUnit.transform);
			yield return new WaitForSeconds(0.2f);
			skill.onGlobalEffect(skill);
			yield return new WaitForSeconds(0.7f);

			if(skill.CastAudioClip!=null)
				AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
			if (m_targetUnit && m_targetUnit.GetComponent<UnitAttribute>() != null)
			m_targetUnit.GetComponent<UnitAttribute>().EffectMarkPrefab = skill.HitEffectPrefab;
			if(m_Unit.Alignment == _UnitAlignment.Player)BattleController.SingleTon().CurrentEnergy -= skill.Cost;
			skill.CurCooldown = 0;
			IsSkilling = true;
			m_Unit.SkillAnimIndex = 1;
			//m_Unit.onSkill();

		//if((CFG_PLAYEFFECT)skill.IsGlobal == (CFG_PLAYEFFECT)0 || (CFG_PLAYEFFECT)skill.IsGlobal == (CFG_PLAYEFFECT)1)
			yield return new WaitForSeconds(0.3f);	

		//if (skill.ShootEffectPrefab && m_targetUnit != null)
		//{
//			ShootObject shootObj = null;
			if(skill.ShootAudioClip!=null)
				AudioSource.PlayClipAtPoint(skill.ShootAudioClip,Camera.main.transform.position);
					

			//Vector3 arrowPos = new Vector3(m_Trans.position.x,m_Trans.position.y + 1,m_Trans.position.z);	
			//GameObject go = Instantiate(skill.ShootEffectPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
			m_Unit.transform.LookAt(m_targetUnit.transform);
			Vector3 arrowPos = new Vector3(m_Trans.position.x,m_Trans.position.y + 1,m_Trans.position.z);
			//GameObject go = Instantiate(skill.ShootEffectPrefab,arrowPos,m_Trans.rotation) as GameObject;
			GameObject go = Instantiate(skill.ShootEffectPrefab,m_Unit.Attribute.ShootPoint.position,m_Trans.rotation) as GameObject;
			StartCoroutine(_Destory(go,5));
			/*DirctShoot shoot = go.AddComponent<DirctShoot>();
			shoot.CurrSkill = skill;
			shoot.Attacker = m_Unit;
			shoot.hitEffect = skill.HitEffectPrefab;*/	
			StartCoroutine(_DirctShoot(go.transform,40,100,skill.Damage,skill.HitEffectPrefab));
		
			Vector3 targetPos = new Vector3(m_targetUnit.transform.position.x,m_Trans.position.y + 1,m_targetUnit.transform.position.z);
			go.transform.LookAt(targetPos);


			//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		}
		//GetClipLength
		float length = m_Unit.GetComponent<UnitAnim>().GetClipLength(AnimationTyp.SKILL,1);
		Debug.Log("skill length.........................." + length);

		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		//yield return null;
	}

	/*
	 * 1.Single 2.Remote 3.Hero target only 4.One-Time RangerMark addition Damage
	 */
	
	public Unit FindHero(int EnemyLayer, float scaneRadius, int Scale)
	{
		Unit EnemyHero = null;
		Collider[] cols = Physics.OverlapSphere(m_Trans.position, scaneRadius * Scale, 1 << EnemyLayer);
		//if(cols.Length > 0)
		//{
			foreach(Collider col in cols)
			{
				if(col.GetComponent<Unit>()!=null && col.GetComponent<Unit>().Alignment != m_Unit.Alignment && col.GetComponent<Unit>().IsHero)
				{
					EnemyHero = col.GetComponent<Unit>();
					break;
				}
			}
		//}
		return EnemyHero;
	}


	List<Unit> targetList;
	Unit SoldierTarget;
	public void RapidSnipe(DataMgr.Skill skill)
	{
		StartCoroutine(_RapidSnipe(skill));
	}

	IEnumerator _RapidSnipe(DataMgr.Skill skill)
	{	
		currSkill = skill;
		targetList = new List<Unit>();
		//if (targetList != null)
			//targetList.Clear();
		//m_Unit.StopMoveAndAttack();
		m_targetUnit = null;
		ShootObject shootObj = null;

		scaneRadius = skill.SkillAttackRadius;
		//colls = Physics.OverlapSphere(skill.CircleCenter,scaneRadius);
		colls = Physics.OverlapSphere(m_Trans.position,scaneRadius);
		foreach(Collider col in colls)
		{
			if(col.GetComponent<Unit>()!=null && col.GetComponent<Unit>().Alignment != m_Unit.Alignment && col.GetComponent<Unit>().IsHero)
				//if(col.GetComponent<Unit>() != null && col.GetComponent<Unit>().Alignment != m_Unit.Alignment)
			{
				m_targetUnit = col.GetComponent<Unit>();
				break;
			}
		}
			
		if (m_targetUnit == null)
		{
			m_targetUnit = m_Unit.Attacker.AttackTarget;
		}

		//yield return new WaitForSeconds(0.7f);

		/*skill.CurCooldown = 0;
		IsSkilling = true;
		m_Unit.SkillAnimIndex = 2;
		m_Unit.onSkill();
		StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		yield return null;*/


		if (m_targetUnit != null)
		{
			m_Unit.StopMoveAndAttack();

			m_Unit.transform.LookAt(m_targetUnit.transform);
			skill.onGlobalEffect(skill);
			yield return new WaitForSeconds(0.7f);

			if(skill.CastAudioClip!=null)AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);						
			GameObject attackEffect = Instantiate(skill.SkillPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
			StartCoroutine(_Destory(attackEffect,skill.SkillAnimDuration));
			//yield return new WaitForSeconds(0.7f);

			if(m_Unit.Alignment == _UnitAlignment.Player)BattleController.SingleTon().CurrentEnergy -= skill.Cost;
			skill.CurCooldown = 0;
			IsSkilling = true;
			m_Unit.SkillAnimIndex = 2;

			//m_Unit.onSkill();
			//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
			m_Anim.TransitionSkill(skill.SkillAnimIndex);
			yield return new WaitForSeconds(0.4f);		
			if(skill.ShootAudioClip!=null)AudioSource.PlayClipAtPoint(skill.ShootAudioClip,Camera.main.transform.position);						
			yield return new WaitForSeconds(0.3f);		

			targetList.Add(m_targetUnit);
			int j = 0;
			colls = Physics.OverlapSphere(m_targetUnit.transform.position,5);
			foreach(Collider col in colls)
			{
				//if(m_targetUnit!=null && m_targetUnit.Alignment != m_Unit.Alignment)
				if(col.GetComponent<Unit>() != null && col.GetComponent<Unit>().Alignment != m_Unit.Alignment)
				{
					SoldierTarget = col.GetComponent<Unit>();
					targetList.Add(SoldierTarget);
					//Debug.Log("m_targetUnit.Alignment.............1.............." + SoldierTarget.Alignment);
					j += 1;
					if (j > 4)
						break;
					
				}
			}

			//if(skill.HitAudioClip!=null)AudioSource.PlayClipAtPoint(skill.HitAudioClip,Camera.main.transform.position);						
			//yield return new WaitForSeconds(0.2f);	
			for (int i = 0; i < 6; i++)
			{
				yield return new WaitForSeconds(0.1f);

				if (skill.ShootEffectPrefab)
				{
					GameObject go = Instantiate(skill.ShootEffectPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
					StartCoroutine(_Destory(go,skill.SkillAnimDuration));
					shootObj = Common.AddObjComponent<ShootObject>(go);
					shootObj.maxShootAngle = 20;
					shootObj.speed = 40;
					shootObj.Damage = skill.Damage;
					shootObj.IsRapidSnipe = true;
					shootObj.ShowDamageText = true;
					shootObj.AdditionDamage = skill.AdditionDamage;
					if (skill.HitEffectPrefab)
					{
						shootObj.HitAffect = skill.HitEffectPrefab;
					}
				
					if (targetList.Count > 0)
					{
						if (i < targetList.Count && targetList[i])
							shootObj.Shoot(targetList[i].transform,m_Unit);
						else
							shootObj.Shoot(targetList[targetList.Count -1].transform,m_Unit);
					}
				}
			}
			if(skill.HitAudioClip!=null)AudioSource.PlayClipAtPoint(skill.HitAudioClip,Camera.main.transform.position);						
		}
		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
	}

	//
	public void RangerCharm(DataMgr.Skill skill)
	{
		currSkill = skill;
		//m_Unit.StopMoveAndAttack();	
		if (skill.SkillPrefab) 
		{
			GameObject go = Instantiate(skill.SkillPrefab, m_Trans.position, m_Trans.rotation) as GameObject;
			StartCoroutine(_Destory(go, skill.SkillAnimDuration));
		}

		//m_Unit.Attribute.BaseAttackSpeed = m_Unit.Attribute.BaseAttackSpeed + m_Unit.Attribute.BaseAttackSpeed * skill.Damage;
        m_Unit.Attribute.m_fAttackSpeedAdd = skill.Damage;
        skill.IsActive = false;
		skill.CurCooldown = 0;
		//		CurCooldown = 0;
		IsSkilling = true;
		if(m_Unit.Alignment == _UnitAlignment.Player)
			BattleController.SingleTon().CurrentEnergy -= skill.Cost;
		//m_Unit.onSkill();
		//m_Anim.TransitionSkill(skill.SkillAnimIndex);

		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
	}

	/*
	 * 1.Single 2.self cast only 3.Immuen all damage for x second 
	 */
	public void Lammasu(DataMgr.Skill skill)
	{
		StartCoroutine(_Lammasu(skill));
	}

	IEnumerator _Lammasu(DataMgr.Skill skill)
	{
		currSkill = skill;
		skill.onGlobalEffect(skill);
		yield return new WaitForSeconds(0.7f);
		Debug.Log("_Lammasu");
		if (skill.SkillPrefab) 
		{
			Debug.Log(skill.SkillPrefab.name);
			GameObject go = Instantiate(skill.SkillPrefab, m_Trans.position, m_Trans.rotation) as GameObject;
			go.transform.parent = m_Trans;
			go.transform.localScale = Vector3.one;
			StartCoroutine(_Destory(go, skill.SkillEffectDuration));
		}

		if(skill.CastAudioClip!=null)
			AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
		m_Unit.Attribute.OnImmuneDamage(skill.SkillEffectDuration);
		m_Unit.Attribute.ImmuneAudio = skill.HitAudioClip;
		skill.CurCooldown = 0;
//		CurCooldown = 0;
		IsSkilling = true;
		if(m_Unit.Alignment == _UnitAlignment.Player)BattleController.SingleTon().CurrentEnergy -= skill.Cost;
		//m_Unit.onSkill();
		//m_Anim.TransitionSkill(skill.SkillAnimIndex);
		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		yield return new WaitForSeconds(0.7f);

		if(skill.HitEffectPrefab)
		{
			GameObject go = Instantiate(skill.HitEffectPrefab, m_Trans.position, m_Trans.rotation) as GameObject;
			go.transform.parent = m_Trans;
			go.transform.localScale = Vector3.one;
			StartCoroutine(_Destory(go, skill.SkillEffectDuration));
		}
	}

	/*
	 * 1.Single 2.alignment only 3.heal x% lost HP for alignment, heal x%*1.5 for self
	 */
	public void LightWingGift(DataMgr.Skill skill)
	{
		StartCoroutine(_LightWingGift(skill));
	}

	IEnumerator _LightWingGift(DataMgr.Skill skill)
	{
		//yield return new WaitForSeconds(0.7f);
		currSkill = skill;
		scaneRadius = skill.SkillAttackRadius;
		//colls = Physics.OverlapSphere(skill.CircleCenter,scaneRadius);
		colls = Physics.OverlapSphere(m_Unit.transform.position,scaneRadius);
		Unit curUnit = null;
		foreach(Collider col in colls)
		{
			m_targetUnit = col.GetComponent<Unit>();
			if(m_targetUnit!=null && m_targetUnit.Alignment == m_Unit.Alignment && m_targetUnit.IsHero)
			{
				if(curUnit==null)
				{
					curUnit = m_targetUnit;
				}
				else if(m_targetUnit.Attribute.MaxHP - m_targetUnit.Attribute.HP > curUnit.Attribute.MaxHP - curUnit.Attribute.HP)
				{
					curUnit = m_targetUnit;
				}
			}
		}
		if(curUnit != null)
		{
			if(skill.SkillPrefab!=null)
			{
				//GameObject temGo = Instantiate(skill.SkillPrefab,skill.CircleCenter,Quaternion.identity) as GameObject;
				GameObject temGo = Instantiate(skill.SkillPrefab,m_Unit.transform.position,Quaternion.identity) as GameObject;
				//temGo.transform.eulerAngles = new Vector3(90,0,0);
				StartCoroutine(_Destory(temGo,skill.SkillAnimDuration));
			}
			skill.onGlobalEffect(skill);
			yield return new WaitForSeconds(0.7f);

			//if(skill.HitAudioClip!=null)AudioSource.PlayClipAtPoint(skill.HitAudioClip,Camera.main.transform.position);
			if(skill.HitEffectPrefab!=null)
			{
				//GameObject temGo = Instantiate(skill.SkillPrefab,skill.CircleCenter,Quaternion.identity) as GameObject;
				GameObject HitEffect = Instantiate(skill.HitEffectPrefab,m_Unit.transform.position,Quaternion.identity) as GameObject;
				//temGo.transform.eulerAngles = new Vector3(90,0,0);
				StartCoroutine(_Destory(HitEffect,skill.SkillAnimDuration));
			}
			
			if(m_Unit.Alignment == _UnitAlignment.Player)BattleController.SingleTon().CurrentEnergy -= skill.Cost;
			skill.CurCooldown = 0;
			IsSkilling = true;
			//m_Unit.onSkill();
			//m_Anim.TransitionSkill(skill.SkillAnimIndex);
			//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));

			yield return new WaitForSeconds(0.7f);	
			if(curUnit == m_Unit)
			{
				curUnit.Attribute.Hit(-1 * (curUnit.Attribute.MaxHP - curUnit.Attribute.HP) * skill.Damage * 1.5f,true);
			}else{
				curUnit.Attribute.Hit(-1 * (curUnit.Attribute.MaxHP - curUnit.Attribute.HP) * skill.Damage,true);
			}

			//yield return new WaitForSeconds(0.4f);			
			if(skill.HitAudioClip!=null)AudioSource.PlayClipAtPoint(skill.HitAudioClip,Camera.main.transform.position);
		}
	}

	/*
	 * 1.Passive 2.Single 3.Remote 4.Self 5.One time 6.Revive self by x% HP when HP == 0,controlled by UnitAttribute
	 */
	public void LionHeartImmotral(DataMgr.Skill skill)
	{
		m_Unit.onBeforeDead += CallbackLionHeartImmotral;
		//m_Unit.Attribute.IsRevivable = true;
		//m_Unit.Attribute.RevivePercent = skill.Damage;
		//skill.IsActive = false;
	}

	void CallbackLionHeartImmotral()
	{
		foreach (DataMgr.Skill skill in HeroSkills) 
		{
			if ((DataMgr.CFG_SKILLCONDITION)skill.Condition == DataMgr.CFG_SKILLCONDITION.DEAD && skill.OperCount == 1)
			{
				skill.OperCount = 0;
				Debug.Log("CallbackLionHeartImmotral");
				StartCoroutine(_ImmotralCroutine());
				m_Unit.Attribute.HP = m_Unit.Attribute.MaxHP * skill.Damage;
				GameObject effect = Instantiate(skill.SkillPrefab,m_Trans.position, m_Trans.rotation) as GameObject;
				StartCoroutine(_Destory(effect,skill.SkillAnimDuration));
				if(skill.CastAudioClip!=null)
					AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
				break;
			}
		}
	}

	IEnumerator _ImmotralCroutine()
	{
		m_Unit.MeleeAttacker.StopAttack();
		m_Unit.Move.Movable = false;
		m_Unit.Anim.m_AnimationList[0].Play("Skill03");
		yield return new WaitForSeconds(0.1f);
		m_Unit.Anim.m_AnimationList[0]["Skill03"].speed = 0;
		yield return new WaitForSeconds(3f);
		m_Unit.Anim.m_AnimationList[0]["Skill03"].speed = 1;
		m_Unit.MeleeAttacker.ResumeAttack();
		m_Unit.Move.Movable = true;
	}


	/*
	 * 1.Active 2.Single 3.Remote 4.Enemy hero 5.Debuff 6.x damage per 5 seconds untill death
	 */
	public void DeadlyThrow(DataMgr.Skill skill)	
	{
		StartCoroutine(_DeadlyThrow(skill));
	}

	IEnumerator _DeadlyThrow(DataMgr.Skill skill)	
	{

		//yield return new WaitForSeconds(0.7f);	
		currSkill = skill;
		bool isFind = false;
		ShootObject shootObj = null;

		//m_Unit.onSkill();
		//yield return new WaitForSeconds(0.7f);	

		scaneRadius = skill.SkillAttackRadius;
//		colls = Physics.OverlapSphere(m_Trans.position,scaneRadius);
//		foreach(Collider col in colls)
//		{
//			m_targetUnit = col.GetComponent<Unit>();
//			if(m_targetUnit!=null && m_targetUnit.Alignment != m_Unit.Alignment && m_targetUnit.IsHero)
//			{
//				m_Unit.gameObject.transform.LookAt(m_targetUnit.transform);
//				isFind = true;
//				break;
//			}
//		}

//		if (skill.ShootEffectPrefab && m_targetUnit != null)
		if (skill.ShootEffectPrefab)
		{

			skill.onGlobalEffect(skill);
			//yield return new WaitForSeconds(0.71f);
			yield return new WaitForSeconds(0.1f);
			if(skill.CastAudioClip!=null)AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
			yield return new WaitForSeconds(0.2f);
			if(skill.ShootAudioClip!=null)AudioSource.PlayClipAtPoint(skill.ShootAudioClip,Camera.main.transform.position);
			yield return new WaitForSeconds(0.4f);
			if(skill.HitAudioClip!=null)AudioSource.PlayClipAtPoint(skill.HitAudioClip,Camera.main.transform.position);
			
			GameObject go = Instantiate(skill.ShootEffectPrefab,m_Unit.Attribute.ShootPoint.position,m_Trans.rotation) as GameObject;
			StartCoroutine(_DirctShoot(go.transform,40,10,skill.Damage,skill.HitEffectPrefab));

			StartCoroutine(_Destory(go,skill.SkillAnimDuration));
//			shootObj = Common.AddObjComponent<ShootObject>(go);
//			shootObj.Damage = skill.Damage;
//			shootObj.speed = 15;
//			shootObj.level = true;
//			shootObj.BleedCooldown = skill.SkillEffectDuration;
//			shootObj.ShowDamageText = true;
//			if (skill.SkillPrefab)
//				shootObj.HitAffect = skill.SkillPrefab;
		}
			
//		if (isFind)
//		{
//			shootObj.Shoot(m_targetUnit.transform,m_Unit);
//			shootObj.onHit += shootObj.OnDeadlyThrowHit;
//		}
//		else
//			shootObj.Shoot(m_Unit.transform,m_Unit);

		if(m_Unit.Alignment == _UnitAlignment.Player)BattleController.SingleTon().CurrentEnergy -= skill.Cost;
		skill.CurCooldown = 0;
		IsSkilling = true;
		//m_Unit.onSkill();
		//m_Anim.TransitionSkill(skill.SkillAnimIndex);
		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		/*if(skill.CastAudioClip!=null)AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
		//GameObject go = Instantiate(skill.ShootEffectPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
		GameObject go = Instantiate(skill.SkillPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
							


		yield return new WaitForSeconds(0.7f);	
		scaneRadius = skill.SkillAttackRadius;
		colls = Physics.OverlapSphere(m_Trans.position,scaneRadius);
		int i = 0;
		foreach(Collider col in colls)
		{
			m_targetUnit = col.GetComponent<Unit>();
			if(m_targetUnit!=null && m_targetUnit != m_Unit && m_targetUnit.Alignment != m_Unit.Alignment && !m_targetUnit.IsHero)
			//if (m_targetUnit!=null && m_targetUnit != m_Unit && !m_targetUnit.IsHero)
			{								
				m_targetUnit.onRepel();
				i += 1;
				if (i > 5)
					break;
			}
		}*/

		yield return null;
	}

	IEnumerator _DirctShoot(Transform trans , float dist , float speed,float damage,GameObject hitEffect)
	{
		bool isHit = false;
		Transform shootObj = trans;
		Vector3 startPos = shootObj.position;
		RaycastHit[] hits = Physics.SphereCastAll(startPos,1,transform.TransformDirection(Vector3.forward),Mathf.Infinity);
		Debug.Log(hits.Length);
		float t = 0;
		float baseSpeed = speed;
		float speedDup = 8;
		float slowTime = 0.5f;
		foreach(RaycastHit hit in hits)
		{
			Unit unit = hit.transform.GetComponent<Unit>();
			if(unit!=null && unit.Alignment!=m_Unit.Alignment)
			{
				float delay = 0;
				if(Vector3.Distance(startPos,unit.transform.position) < slowTime * speed)
				{
					delay = Vector3.Distance(startPos,unit.transform.position) / speed;
				}
				else
				{
					delay = slowTime + (Vector3.Distance(startPos,unit.transform.position) - slowTime * speed)/speed/speedDup;
				}
				StartCoroutine(_DelayDirctHit(unit,damage,delay,hitEffect));
			}
		}
		while(!isHit)
		{
			t += Time.deltaTime;
			if(t>slowTime)
			{
				speed = speedDup * baseSpeed;
			}
			if(Vector3.Distance(startPos,shootObj.position) < dist)
			{

				shootObj.Translate(Vector3.forward * speed * Time.deltaTime);
			}
			else
			{
				isHit = true;
				shootObj.gameObject.SetActive(false);
			}
			yield return null;
		}
	}
	
	IEnumerator _DelayDirctHit(Unit unit,float damage,float delay,GameObject hitEffect)
	{
		yield return new WaitForSeconds(delay);
		if (hitEffect != null)
		{
			Instantiate(hitEffect,unit.transform.position,Quaternion.identity);
		}
		damage = Random.Range(damage/2,damage);
		unit.Attribute.Hit(damage,true);
		Debug.Log("DeadlyThrow........................................" + unit.name);
	}



	public void CallBackDesireWarSkill(Unit unit)
	{
		if(unit.IsHero && unit.Alignment == _UnitAlignment.Enemy)
		{		
			foreach (DataMgr.Skill skill in HeroSkills) 
			{
				if ((DataMgr.CFG_SKILLCONDITION)skill.Condition == DataMgr.CFG_SKILLCONDITION.DEAD)
				{
					m_Unit.Attribute.Damage = m_Unit.Attribute.Damage + m_Unit.Attribute.Damage * skill.Damage;				
					GameObject effect = Instantiate(skill.SkillPrefab,m_Trans.position, m_Trans.rotation) as GameObject;
					StartCoroutine(_Destory(effect,skill.SkillAnimDuration));
					if(skill.CastAudioClip!=null)
						AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
					if(m_Unit.Alignment == _UnitAlignment.Player)BattleController.SingleTon().CurrentEnergy -= skill.Cost;
					skill.CurCooldown = 0;
					IsSkilling = true;
					m_Unit.onSkill();
					//m_Anim.TransitionSkill(skill.SkillAnimIndex);
					break;
				}
			}
		}
	}

	public void DesireWarSkill(DataMgr.Skill skill)
	{
		SpawnManager.SingleTon().onRemoveHero += CallBackDesireWarSkill;
		//m_Unit.onAfterDead += CallBackDesireWarSkill;
	}

	public void StrickCrucial(DataMgr.Skill skill)
	{
		StartCoroutine(_StrickCrucial(skill));
	}

	IEnumerator _StrickCrucial(DataMgr.Skill skill)
	{
		currSkill = skill;
		m_Unit.StopMoveAndAttack();
		scaneRadius = skill.SkillAttackRadius;
		colls = Physics.OverlapSphere(m_Trans.position,scaneRadius);
		foreach(Collider col in colls)
		{
			m_targetUnit = col.GetComponent<Unit>();
			if(m_targetUnit!=null && m_targetUnit.Alignment != m_Unit.Alignment && m_targetUnit.IsHero && skill.ShootEffectPrefab)
			{
				m_Unit.transform.LookAt(m_targetUnit.transform);
				break;
			}
		}

		skill.onGlobalEffect(skill);
		if(skill.CastAudioClip!=null)AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
		StartCoroutine(_SkillCircle(skill.SkillAttackRadius));
		yield return new WaitForSeconds(0.71f);
		GameObject go = Instantiate(skill.SkillPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
		StartCoroutine(_Destory(go,skill.SkillAnimDuration));
		//yield return new WaitForSeconds(0.2f);
		

		if(m_Unit.Alignment == _UnitAlignment.Player)BattleController.SingleTon().CurrentEnergy -= skill.Cost;
		//if(skill.CastAudioClip!=null)AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);

		//GameObject go = Instantiate(skill.SkillPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
		//StartCoroutine(_Destory(go,skill.SkillAnimDuration));


		int k = 0;
		float time = 0.1f;
		scaneRadius = skill.SkillAttackRadius;
		colls = Physics.OverlapSphere(m_Trans.position,scaneRadius);

		foreach(Collider col in colls)
		{
			m_targetUnit = col.GetComponent<Unit>();
			if(m_targetUnit!=null && m_targetUnit.Alignment != m_Unit.Alignment && skill.ShootEffectPrefab)
			{
				m_targetUnit.Attribute.LastAttacker = m_Unit;
				StartCoroutine(_DelayDirctHit(m_targetUnit,skill.Damage,time,null));
				time += 0.05f;
				/*float damage = Random.Range(skill.Damage /2,skill.Damage);
				m_targetUnit.Attribute.LastAttacker = m_Unit;
				if(m_targetUnit.Attribute.IsDeadlyThrowed)
					m_targetUnit.Attribute.Hit(damage + damage * 0.5f,true);
				else				
					m_targetUnit.Attribute.Hit(damage,true);*/

				k += 1;
			}
			if (k > 5)
				break;
		}



		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		skill.CurCooldown = 0;
		IsSkilling = true;
		//m_Unit.onSkill();
		//m_Anim.TransitionSkill(skill.SkillAnimIndex);
		
		scaneRadius = skill.SkillAttackRadius;
		colls = Physics.OverlapSphere(m_Trans.position,scaneRadius);
		int i = 0;
		foreach(Collider col in colls)
		{
			m_targetUnit = col.GetComponent<Unit>();
			//if(m_targetUnit!=null && m_targetUnit.Alignment != m_Unit.Alignment && m_targetUnit.IsHero && skill.ShootEffectPrefab)
			if(m_targetUnit!=null && m_targetUnit.Alignment != m_Unit.Alignment && !m_targetUnit.IsHero)
			{
				//m_targetUnit.onRepel();
				i += 1;
				if (i > 5)
					break;
			}
		}
	}


	/*IEnumerator _StrickCrucial(DataMgr.Skill skill)
	{
		scaneRadius = skill.SkillAttackRadius;
		colls = Physics.OverlapSphere(skill.CircleCenter,scaneRadius);
		foreach(Collider col in colls)
		{
			m_targetUnit = col.GetComponent<Unit>();
			if(m_targetUnit!=null && m_targetUnit.Alignment != m_Unit.Alignment && m_targetUnit.IsHero && skill.ShootEffectPrefab)
			{
				m_Unit.transform.LookAt(m_targetUnit.transform);
				break;
			}
		}

		yield return new WaitForSeconds(0.7f);

		ShootObject shootObj = null;
		if(m_Unit.Alignment == _UnitAlignment.Player)BattleController.SingleTon().CurrentEnergy -= skill.Cost;
		
		if(skill.CastAudioClip!=null)AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
		GameObject go = Instantiate(skill.ShootEffectPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
		StartCoroutine(_Destory(go,skill.SkillAnimDuration));
		shootObj = Common.AddObjComponent<ShootObject>(go);
		shootObj.Damage = skill.Damage;
		if (skill.SkillPrefab)
			shootObj.HitAffect = skill.SkillPrefab;
		shootObj.IsStrickCrucial = true;
		shootObj.ShowDamageText = true;
		shootObj.speed = 40;
		shootObj.level = true;
		shootObj.AdditionDamage = skill.Damage * 0.5f;
		StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		skill.CurCooldown = 0;
		IsSkilling = true;
		m_Unit.onSkill();

		scaneRadius = skill.SkillAttackRadius;
		colls = Physics.OverlapSphere(skill.CircleCenter,scaneRadius);
		//colls = Physics.OverlapSphere(m_Trans.position,scaneRadius);
		int i = 0;
		foreach(Collider col in colls)
		{
			m_targetUnit = col.GetComponent<Unit>();
			//if(m_targetUnit!=null && m_targetUnit.Alignment != m_Unit.Alignment && m_targetUnit.IsHero && skill.ShootEffectPrefab)
			if(m_targetUnit!=null && m_targetUnit.Alignment != m_Unit.Alignment && skill.ShootEffectPrefab)
			{
				m_Unit.transform.LookAt(m_targetUnit.transform);
				m_targetUnit.onRepel();
				shootObj.Shoot(m_targetUnit.transform,m_Unit);
				i += 1;
				if (i > 5)
					break;
			}
		}
	}*/

	public void SkillDamageEnhanceSkill(DataMgr.Skill skill)
	{
		if(m_Unit.Attribute.HP/m_Unit.Attribute.MaxHP <= 0.4 )
		{
			if(!skill.IsSkilled)
			{
				foreach(DataMgr.Skill sk in HeroSkills)
				{
					if(sk != skill)
					{
						sk.Damage += skill.Damage;
						sk.SkillEffectDuration += skill.SkillEffectDuration;
					}
					skill.IsSkilled = true;
				}
			}
		}else{
			if(skill.IsSkilled)
			{
				foreach(DataMgr.Skill sk in HeroSkills)
				{
					if(sk != skill)
					{
						sk.Damage -= skill.Damage;
						sk.SkillEffectDuration -= skill.SkillEffectDuration;
					}
					skill.IsSkilled = false;
				}
			}
		}
	}

	Collider[] colls ;
	float scaneRadius;
	//Whirlwind,skill Scan radius must less than attack radius
	public void MeleeWhirlwindSkill(Skill skill)
	{
		scaneRadius = Mathf.Max(5,skill.SkillAttackRadius - 5);
		colls = Physics.OverlapSphere(m_Trans.position,scaneRadius);
		Unit unit;
		foreach(Collider col in colls)
		{
			unit = col.GetComponent<Unit>();
			if(unit!=null && unit.Alignment != m_Unit.Alignment)
			{
				skill.CurCooldown = 0;
//				CurCooldown = 0;
				IsSkilling = true;
				m_Unit.onSkill();
				//m_Anim.TransitionSkill(skill.SkillAnimIndex);
				StartCoroutine(_WhirlwindSkill(skill.SkillAttackRadius,skill.Damage,5));
				//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
				break;
			}
		}
	}

	IEnumerator _WhirlwindSkill(float attackRadius,float damagePerSecond,int duration)
	{
		float i = 0;
		while(true)
		{
			yield return new WaitForSeconds(0.5f);
			colls = Physics.OverlapSphere(m_Trans.position,attackRadius);
			Unit unit;
			foreach(Collider col in colls)
			{
				unit = col.GetComponent<Unit>();
				if(unit!=null && unit.Alignment != m_Unit.Alignment)
				{
					unit.Attribute.Hit(damagePerSecond,true);
				}
			}
			i += 0.5f;
			if(i>=duration)break;
		}
	}

	public void ShootEnemyHeroDamageSkill(Skill skill)
	{
		GameObject go = Instantiate(skill.SkillPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
		ShootObject shoot = go.AddComponent<ShootObject>();
		shoot.maxShootAngle = 0;
		shoot.Damage = skill.Damage;
//		shoot.Shoot();
	}

	//MeleeSector,skill Scan radius must less than attack radius
	public void MeleeSectorForwardSkill(Skill skill)
	{
		scaneRadius = Mathf.Max(5,skill.SkillAttackRadius - 5);
		colls = Physics.OverlapSphere(m_Trans.position,scaneRadius);

		Unit unit;
		foreach(Collider col in colls)
		{
			unit = col.GetComponent<Unit>();
			if(unit!=null && unit.Alignment != m_Unit.Alignment)
			{
				if(Common.CheckPosInForwardSector(m_Trans.position,unit.transform.position,60))
				{
					skill.CurCooldown = 0;
//					CurCooldown = 0;
					IsSkilling = true;
					m_Unit.onSkill();
					//m_Anim.TransitionSkill(skill.SkillAnimIndex);
					_MeleeSectorForwardSkill(skill.SkillAttackRadius,skill.Damage,skill.MaxHitCount);
					//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
					break;
				}
			}
		}
	}

	public void _MeleeSectorForwardSkill(float attackRadius,float damage,int maxImpactCount)
	{
		colls = Physics.OverlapSphere(m_Trans.position,attackRadius);
		Unit unit;
		int i = 0;
		foreach(Collider col in colls)
		{
			unit = col.GetComponent<Unit>();
			if(unit!=null && unit.Alignment != m_Unit.Alignment)
			{
				if(Common.CheckPosInForwardSector(m_Trans.position,unit.transform.position,60))
				{
					i ++ ;
					if(unit.Attribute!=null)unit.Attribute.Hit(damage,true);
					if(i>=maxImpactCount)break;
				}
			}
		}
	}

	//Circle damage skill
	public void CircleSkill(Skill skill)
	{
		Unit target = GetSkillTarget();
		if(target != null)
		{
			if(skill.SkillPrefab!=null)
			{
				GameObject effect = Instantiate(skill.SkillPrefab,target.transform.position,Quaternion.identity) as GameObject;
				StartCoroutine(_Destory(effect,5));
			}
			Collider[] colls = Physics.OverlapSphere(target.transform.position,skill.SkillAttackRadius);
			Unit unit;
			foreach(Collider col in colls)
			{
				unit = col.GetComponent<Unit>();
				if(unit!=null && unit.Alignment != m_Unit.Alignment)
				{
					unit.Attribute.Hit(skill.Damage,true);
				}
			}
			skill.CurCooldown = 0;
//			CurCooldown = 0;
			IsSkilling = true;
			m_Unit.onSkill();
			//m_Anim.TransitionSkill(skill.SkillAnimIndex);
			//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		}
	}

	//TODO
	public void CircleExploseSkill(Skill skill)
	{
		Unit target = GetSkillTarget();
		if(target != null)
		{
			if(skill.SkillPrefab!=null)
			{
				GameObject effect = Instantiate(skill.SkillPrefab,target.transform.position,Quaternion.identity) as GameObject;
				StartCoroutine(_Destory(effect,5));
			}
			Collider[] colls = Physics.OverlapSphere(target.transform.position,skill.SkillAttackRadius);
			Unit unit;
			foreach(Collider col in colls)
			{
				unit = col.GetComponent<Unit>();
				if(unit!=null && unit.Alignment != m_Unit.Alignment)
				{
					unit.Attribute.Hit(skill.Damage,true);
				}
			}
			BeenFly(colls,target.transform.position);
			skill.CurCooldown = 0;
//			CurCooldown = 0;
			IsSkilling = true;
			m_Unit.onSkill();
			//m_Anim.TransitionSkill(skill.SkillAnimIndex);
			//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		}
	}
	
	//crosscut
	void CrossCutSkill(Skill skill)
	{
		Collider[] colls = Physics.OverlapSphere(m_Trans.position, skill.SkillAttackRadius);
		int i = 0;
		Unit unit;
		
		foreach(Collider coll in colls)
		{
			unit = coll.GetComponent<Unit>();
			if ((unit != null) && (unit.Alignment != m_Unit.Alignment))
			{
				unit.Attribute.Hit(skill.Damage,true);				
				i++;
			}
			if (i >= 2)
				break;
		}
		
		GameObject effect = Instantiate (skill.SkillPrefab, m_Trans.position, Quaternion.identity) as GameObject;	
		StartCoroutine(_Destory(effect,5));
										
		skill.CurCooldown = 0;
//		CurCooldown = 0;
		IsSkilling = true;
		m_Unit.onSkill();
		//m_Anim.TransitionSkill(skill.SkillAnimIndex);
		//StartCoroutine (_SkillDuration(skill.SkillDuration));		
	}
	
	//RoundTableStep
	void RoundTableStep(Skill skill)
	{
		m_Unit.Attribute.OnPromoteDamage(skill.Damage,skill.SkillDuration);
		skill.CurCooldown = 0;
//		CurCooldown = 0;
		IsSkilling = true;
		m_Unit.onSkill();
		//m_Anim.TransitionSkill(skill.SkillAnimIndex);
		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
	}	
	
	//parry
	void parryRate()
	{
		m_Unit.Attribute.isParry = true;
		m_Unit.Attribute.parryRate = 4;
	}

	//survivesongskill
	public void SurviveSongSkill(DataMgr.Skill skill)
	{	
		StartCoroutine(_SurviveSongSkill(skill));
	}

	IEnumerator _SurviveSongSkill(DataMgr.Skill skill)
	{
		currSkill = skill;
		skill.onGlobalEffect(skill);
//		yield return new WaitForSeconds(0.7f);

		GameObject go = Instantiate(skill.SkillPrefab,m_Trans.position,Quaternion.identity) as GameObject;
		yield return new WaitForSeconds(0.7f);
		Debug.Log(skill.SkillAttackRadius);
		StartCoroutine(_SkillCircle(skill.SkillAttackRadius));
		StartCoroutine(_Destory(go,skill.SkillAnimDuration));
		if(skill.CastAudioClip!=null)AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
		skill.CurCooldown = 0;
		IsSkilling = true;
		//m_Unit.onSkill();
		//m_Anim.TransitionSkill(skill.SkillAnimIndex);
		StartCoroutine(_SurviveSongSkillDamage(skill));
		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
	}

	IEnumerator _SkillCircle(float radius)
	{
		if(BattleController.SingleTon().SkillCircle)
		{
			BattleController.SingleTon().SkillCircle.transform.position = m_Trans.position;
			BattleController.SingleTon().SkillCircle.transform.localScale = BattleController.SingleTon().SkillCircleRadius * radius * 2;
			BattleController.SingleTon().SkillCircle.SetActive(true);
		}
		yield return new WaitForSeconds(2);
		if(BattleController.SingleTon().SkillCircle)
		{
			BattleController.SingleTon().SkillCircle.SetActive(false);
		}
	}

	IEnumerator _SkillCircle(float radius,Vector3 pos)
	{
		if(BattleController.SingleTon().SkillCircle)
		{
			BattleController.SingleTon().SkillCircle.transform.position = pos;
			BattleController.SingleTon().SkillCircle.transform.localScale = BattleController.SingleTon().SkillCircleRadius * radius * 2;
			BattleController.SingleTon().SkillCircle.SetActive(true);
		}
		yield return new WaitForSeconds(3);
		if(BattleController.SingleTon().SkillCircle)
		{
			BattleController.SingleTon().SkillCircle.SetActive(false);
		}
	}

	IEnumerator _SurviveSongSkillDamage(DataMgr.Skill skill)
	{

		yield return new WaitForSeconds(0.7f);
		float j = 0;
		//yield return new WaitForSeconds(1f);
		Collider[] colls = Physics.OverlapSphere(m_Trans.position,skill.SkillAttackRadius);    
		Debug.Log("skill.SkillAttackRadius:" + skill.SkillAttackRadius);
		foreach(Collider coll in colls)
		{
			Unit unit = coll.GetComponent<Unit>();                                
//			if (unit != null && unit.IsHero)
			if (unit != null )
			{
				if (unit.Alignment == m_Unit.Alignment)
				{
					unit.Attribute.OnSurviveSongDamageByFriend(skill.Damage,skill.SkillAnimDuration);
				}
				else if(unit.Alignment != m_Unit.Alignment)
				{
					unit.Attribute.OnSurviveSongDamageByEnemy(skill.Damage,skill.SkillAnimDuration);
				}
				j += 1;
			}
			if (j >= skill.MaxHitCount)
				break;
			
		}   
		yield return null;	
						
	}
			

	
	//petrifactionskill
	public void PetrifactionSkill(DataMgr.Skill skill)
	{
		currSkill = skill;
		skill.onGlobalEffect(skill);
		//yield return new WaitForSeconds(0.7f);

		if (skill.SkillPrefab)
		{
			GameObject go = Instantiate(skill.SkillPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
			StartCoroutine(_Destory(go,skill.SkillAnimDuration));
			if(skill.CastAudioClip!=null)AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
			
			skill.CurCooldown = 0;
			IsSkilling = true;
			//m_Unit.onSkill();
			//m_Anim.TransitionSkill(skill.SkillAnimIndex);
			//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		}

		Collider[] colls = Physics.OverlapSphere(m_Trans.position, skill.SkillAttackRadius);

		foreach(Collider coll in colls)
		{
			Unit unit = coll.GetComponent<Unit>();
			if (unit != null && unit.IsHero && unit.Alignment != m_Unit.Alignment)
			{
				StartCoroutine(_LineRay(unit.Attribute.HitTargets[0]));
				unit.Attribute.OnPetrifactionSkill(skill.SkillEffectDuration);
				break;
			}
		}
	}

	public float LineDuring = 0.8f;
	IEnumerator _LineRay(Transform target)
	{
		float t = 0;
		float startTime = Time.realtimeSinceStartup;
		BattleController.SingleTon().Line.enabled = true;
		BattleController.SingleTon().Line.SetPosition(1,m_Unit.Attribute.HitTargets[0].position);
		while(t<2)
		{
			BattleController.SingleTon().Line.SetPosition(0,m_Unit.Attribute.HitTargets[0].position);
			t = Time.realtimeSinceStartup - startTime;
			Vector3 v = Vector3.Lerp(m_Unit.Attribute.HitTargets[0].position,target.position,t/LineDuring);
			BattleController.SingleTon().Line.SetPosition(1,v);
			yield return null;
		}
		BattleController.SingleTon().Line.enabled = false;
	}

	//toxtouch skill
	public void ToxTouchSkill(DataMgr.Skill skill)
	{
	    StartCoroutine(_ToxTouchSkill(skill));
	}

	IEnumerator _ToxTouchSkill(DataMgr.Skill skill)
	{
		currSkill = skill;
		StartCoroutine(_SkillCircle(skill.SkillAttackRadius,BattleController.SingleTon().LongPressPos));
		skill.onGlobalEffect(skill);
		yield return new WaitForSeconds(0.7f);

//		GameObject go = Instantiate(skill.SkillPrefab,m_Trans.position,Quaternion.identity) as GameObject;
//		StartCoroutine(_Destory(go,skill.SkillAnimDuration));
//		if(skill.CastAudioClip!=null)AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
		skill.CurCooldown = 0;
		IsSkilling = true;
		//m_Unit.onSkill();
		//m_Anim.TransitionSkill(skill.SkillAnimIndex);
//		StartCoroutine(_ToxTouchSkillDamage(skill));
		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		
	}

	IEnumerator _ToxTouchSkillDamage(DataMgr.Skill skill)
	{
		Collider[] colls = Physics.OverlapSphere(m_Trans.position, skill.SkillAttackRadius);
		int i = 0;

		//yield return new WaitForSeconds(1f);
		foreach (Collider coll in colls)
		{
			Unit unit = coll.GetComponent<Unit>();
			if (unit != null && unit.IsHero && unit.Alignment == m_Unit.Alignment)
			{
				float damage = skill.Damage * m_Unit.Attribute.Damage;
				unit.Attribute.OnToxTouchSkill(damage,skill.SkillEffectDuration);
				i += 1;				
			}
			if (i >= skill.MaxHitCount)
				break;
		}
		yield return null;

	}


	public void UnityIsStrengthSkill(DataMgr.Skill skill)
	{
		currSkill = skill;
		if (skill.SkillPrefab)
		{
			GameObject go = Instantiate(skill.SkillPrefab, m_Trans.position, Quaternion.identity) as GameObject;
			StartCoroutine(_Destory(go, 5));
		}
		skill.CurCooldown = 0;
		IsSkilling = true;
		//m_Unit.onSkill();
		//m_Anim.TransitionSkill(skill.SkillAnimIndex);
		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));

		int nCount = SpawnManager.SingleTon().PlayerHeros.Count;
		for (int i = 0; i < nCount; i++)
		{
			Unit heroUnit = SpawnManager.SingleTon().PlayerHeros [i];
			//if (heroUnit.HeroId != m_Unit.HeroId)
			//{

				heroUnit.Attribute.MaxHP = Mathf.RoundToInt(heroUnit.Attribute.MaxHP + m_Unit.Attribute.BaseMaxHP * skill.Damage);
				heroUnit.Attribute.HP = Mathf.RoundToInt(heroUnit.Attribute.HP + m_Unit.Attribute.BaseHP * skill.Damage);
				BattleController.SingleTon().MaxPlayerFighting = Mathf.RoundToInt(BattleController.SingleTon().MaxPlayerFighting + m_Unit.Attribute.BaseMaxHP * skill.Damage);
				skill.IsActive = false;
			//}
			if (i >= skill.MaxHitCount -1)
				break;
		}

	}

	public void KingSacrificeSkill(DataMgr.Skill skill)
	{		
		StartCoroutine(_KingSacrificeSkill1(skill));
	}

	IEnumerator _KingSacrificeSkill1(DataMgr.Skill skill)
	{
		skill.onGlobalEffect(skill);
		yield return new WaitForSeconds(0.7f);
		StartCoroutine(_SkillCircle(skill.SkillAttackRadius,BattleController.SingleTon().LongPressPos));
		if (skill.SkillPrefab)
		{
			SpawnManager.SingleTon().onKingSacrifice = SetKingSacrifice;
			GameObject go = Instantiate(skill.SkillPrefab, BattleController.SingleTon().LongPressPos, Quaternion.identity) as GameObject;
//			if(BattleController.SingleTon().LongPressPos!=Vector3.zero)
//			{
//				go.transform.position = BattleController.SingleTon().LongPressPos;
//			}
			StartCoroutine(_Destory(go, skill.SkillAnimDuration));
			if(skill.CastAudioClip!=null)
				AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
			if(m_Unit.Alignment == _UnitAlignment.Player)
				BattleController.SingleTon().CurrentEnergy -= skill.Cost;

			Collider[] cols = Physics.OverlapSphere(go.transform.position,3);
			float i = 0.5f;
			for(int j =0 ; j < 10; j ++)
			{
				StartCoroutine(DelayAudio(i+j*0.1f));
			}
			foreach(Collider col in cols)
			{
				Unit unit = col.GetComponent<Unit>();
				if(unit!=null && unit.Alignment!=m_Unit.Alignment)
				{
					i = Mathf.Min(2,i+0.1f);
					StartCoroutine(DelayDamage(unit,i,Random.Range(skill.Damage/2,skill.Damage)));
//					unit.Attribute.LastAttacker = m_Unit;
//					unit.Attribute.Hit(3000,true);
				}
				i += 0.05f;
			}

//			StartCoroutine(DelayDamage(go.transform.position,2.5f));
			skill.CurCooldown = 0;
			IsSkilling = true;
			//m_Unit.onSkill();
			//m_Anim.TransitionSkill(skill.SkillAnimIndex);
			StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		}
	}
	IEnumerator DelayAudio(float delay)
	{
		yield return new WaitForSeconds(delay);
		AudioManager.PlaySound(BattleController.SingleTon().KnifeAudioClip,m_Trans.position);
	}


	IEnumerator DelayDamage(Unit unit, float delay,float damage)
	{
		yield return new WaitForSeconds(delay);

		unit.Attribute.LastAttacker = m_Unit;
		unit.Attribute.Hit(damage,true);
	}

	IEnumerator _KingSacrificeSkill(DataMgr.Skill skill)
	{
		//Debug.Log(" name........................." + m_Unit.name);
		//StartCoroutine(_KingSacrificeSkill(skill.SkillAnimDuration,skill.Damage));
		currSkill = skill;
		skill.onGlobalEffect(skill);
		yield return new WaitForSeconds(0.7f);

		if (skill.SkillPrefab)
		{
			SpawnManager.SingleTon().onKingSacrifice = SetKingSacrifice;
			GameObject go = Instantiate(skill.SkillPrefab, BattleController.SingleTon().LongPressPos, Quaternion.identity) as GameObject;
//			if(BattleController.SingleTon().LongPressPos!=Vector3.zero)
//			{
//				go.transform.position = BattleController.SingleTon().LongPressPos;
//			}
			StartCoroutine(_Destory(go, skill.SkillAnimDuration));
			if(skill.CastAudioClip!=null)
				AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
			if(m_Unit.Alignment == _UnitAlignment.Player)
				BattleController.SingleTon().CurrentEnergy -= skill.Cost;
			skill.CurCooldown = 0;
			IsSkilling = true;
			//m_Unit.onSkill();
			//m_Anim.TransitionSkill(skill.SkillAnimIndex);
			//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
		}		
	}


	void SetKingSacrifice(Unit unit)
	{
		foreach (DataMgr.Skill skill in HeroSkills) 
		{
			if ((DataMgr.CFG_SKILLCONDITION)skill.Condition == DataMgr.CFG_SKILLCONDITION.BEFOREDEAD &&
				skill.CurCooldown < skill.MaxCooldown)
			{
				if (unit.Alignment == m_Unit.Alignment && unit.HeroId != m_Unit.HeroId)
				{
					unit.Attribute.HP = m_Unit.Attribute.HP * skill.Damage;
					m_Unit.Attribute.HP = 0;
					break;
				}
			}
		}		
	}

	public void HeroMournfulSongSkill(DataMgr.Skill skill)
	{
		m_Unit.onBeforeDead += CallbackHeroMournfulSong;	
	}


	public void CallbackHeroMournfulSong()
	{
		foreach (DataMgr.Skill skill in HeroSkills) 
		{
			if ((DataMgr.CFG_SKILLCONDITION)skill.Condition == DataMgr.CFG_SKILLCONDITION.DEAD && skill.OperCount == 1)
			{
				skill.OperCount = 0;
				
				GameObject effect = Instantiate(skill.SkillPrefab,m_Trans.position, m_Trans.rotation) as GameObject;
				StartCoroutine(_Destory(effect,skill.SkillAnimDuration));	
				if(skill.CastAudioClip!=null)
					AudioSource.PlayClipAtPoint(skill.CastAudioClip,Camera.main.transform.position);
				skill.CurCooldown = 0;
				IsSkilling = true;
				//m_Unit.onSkill();
				//m_Anim.TransitionSkill(skill.SkillAnimIndex);
				//onCooldownOver += restoreDamage;
				BattleController.SingleTon().isHeroMournfulSong = true;
				BattleController.SingleTon().currHeroMournfulSongCooldown = 0;
				BattleController.SingleTon().HeroMournfulSongCooldown = skill.SkillEffectDuration;
				//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));

				int nCount = SpawnManager.SingleTon().PlayerHeros.Count;				
				for (int i = 0; i < nCount; i++)
				{
					Unit heroUnit = SpawnManager.SingleTon().PlayerHeros [i];
					if (heroUnit.HeroId != m_Unit.HeroId)
					{
						//heroUnit.Attribute.Damage = heroUnit.Attribute.Damage * skill.Damage;
						heroUnit.Attribute.SkillDamage = heroUnit.Attribute.BaseDamage * skill.Damage;
						heroUnit.Attribute.Damage = heroUnit.Attribute.BaseDamage + heroUnit.Attribute.SkillDamage;
					}
					if (i >= skill.MaxHitCount -1)
						break;
				}

			}
		}
	}


	public void restoreDamage(DataMgr.Skill skill)
	{
		if ((DataMgr.CFG_SKILLCONDITION)skill.Condition == DataMgr.CFG_SKILLCONDITION.DEAD)
		{
			int nCount = SpawnManager.SingleTon().PlayerHeros.Count;				
			for (int i = 0; i < nCount; i++)
			{
				Unit heroUnit = SpawnManager.SingleTon().PlayerHeros [i];
				if (heroUnit.HeroId != m_Unit.HeroId)
				{
					heroUnit.Attribute.SkillDamage = 0;
					heroUnit.Attribute.Damage = heroUnit.Attribute.BaseDamage + heroUnit.Attribute.SkillDamage;
				}
				if (i >= skill.MaxHitCount -1)
					break;
			}
		}
	}


	public void VitalPartAttack(Skill skill)
	{
		GameObject go = Instantiate(skill.SkillPrefab, m_Trans.position, Quaternion.identity) as GameObject;
		StartCoroutine(_Destory(go, 5));
		skill.CurCooldown = 0;
		IsSkilling = true;
		m_Unit.onSkill();
		//m_Anim.TransitionSkill(skill.SkillAnimIndex);
		//StartCoroutine(_SkillDuration(skill.SkillAnimDuration));
	}
	
	public List<Collider> flys;
	public void  BeenFly(Collider[] colls,Vector3 centerPos)
	{
		flys = new List<Collider>();
		Unit unit;
		foreach(Collider col in colls)
		{
			unit = col.GetComponent<Unit>();
			if(unit!=null && unit.Alignment != m_Unit.Alignment)
			{
				flys.Add(col);
				if(unit!=null){
					if(unit.Attacker!=null){
						unit.Attacker.StopAttack();
					}
					if(unit.MeleeAttacker!=null){
						unit.MeleeAttacker.StopAttack();
					}
					if(unit.Move!=null)
					{
						unit.Move.enabled = false;
//						unit.Move.m_Nav.enabled =false;
					}
				}
				Rigidbody rigid = col.gameObject.AddComponent<Rigidbody>();
				if (rigid != null)
				{
					rigid.AddExplosionForce(300, centerPos, 10);
					rigid.AddForce(Vector3.up * 500);
				}
			}
		}
		StartCoroutine(_RecoverMove());
	}
	
	float recoverTime = 3;
	IEnumerator _RecoverMove()
	{
		yield return new WaitForSeconds(recoverTime);
		foreach(Collider col in flys)
		{
			Unit unit = col.GetComponent<Unit>();
			Destroy(col.GetComponent<Rigidbody>());
			if(unit!=null){
				StartCoroutine(_RecoverAttack(unit));
				if(unit.Move!=null)
				{
					unit.Move.enabled = true;
//					unit.Move.m_Nav.enabled =true;
				}
			}
		}
	}
	
	IEnumerator _RecoverAttack(Unit unit)
	{
		yield return new WaitForSeconds(2);
		if(unit.Attacker!=null){
			unit.Attacker.ResumeAttack();
		}
		if(unit.MeleeAttacker!=null){
			unit.MeleeAttacker.ResumeAttack();
		}
	}

	IEnumerator _SkillDuration(float duration)
	{
		yield return new WaitForSeconds(duration);
		BattleController.SingleTon().gSkilling = false;
		IsSkilling = false;
		IsSkillDone = true;
		Debug.Log("skill name............._SkillDuration............ " + currSkill.SkillId);
		//Debug.Log("Skill Done true................................" + Time.realtimeSinceStartup);
	}

	public void SetSkillDone()
	{
		m_Unit.Move.Movable = true;
		BattleController.SingleTon().gSkilling = false;
		IsSkilling = false;
		IsSkillDone = true;
	}

	Unit GetSkillTarget()
	{
		if(m_Unit.Attacker!=null)
		{
			return m_Unit.Attacker.AttackTarget;
		}
		if(m_Unit.MeleeAttacker!=null)
		{
			return m_Unit.MeleeAttacker.AttackTarget;
		}
		return null;
	}

	IEnumerator _Destory(GameObject obj,float delay)
	{
		yield return new WaitForSeconds(delay);
		//Debug.Log("Destroy.............................." + obj.name);
		if (obj) 
		{
			//Debug.Log("goooooooooo......................." + obj.name);
			obj.SetActive (false);
		}
		//Destroy(obj);

	}

	public _ScanPriority ScanPriority = _ScanPriority.Nearest;

	void getTarget(DataMgr.Skill skill,out Unit AttackTarget)
	{
		AttackTarget = null;
		Collider[] cols;
		cols = Physics.OverlapSphere(m_Trans.position, skill.SkillAttackRadius,1<<m_Unit.EnemyLayer);
		if(BattleController.SingleTon().DuringSinceBattleBegin >= 30 && cols.Length == 0)
		{
			//cols = Physics.OverlapSphere(m_Trans.position , skill.SkillAttackRadius * 10 , 1<<m_Unit.EnemyLayer);
			cols = Physics.OverlapSphere(m_Trans.position , skill.SkillAttackRadius * 10);
		}

		if(cols.Length>0){
			Unit currentTarget=null;
			if(ScanPriority==_ScanPriority.Nearest)
			{
				float dist=Mathf.Infinity;
				Collider currentCollider=cols[0];
				foreach(Collider col in cols)
				{
					if (col != null)
					{
						float currentDist = Vector3.Distance(m_Trans.position, col.transform.position);
						if (currentDist < dist)
						{
							currentCollider = col;
							dist = currentDist;
						}
					} 
				}
				currentTarget=currentCollider.gameObject.GetComponent<Unit>();
				if(currentTarget!=null && currentTarget.Attribute.HP>0 ) 
				{
					AttackTarget = currentTarget;
				}
			}
			else if(ScanPriority==_ScanPriority.Default)
			{
				Collider currentCollider=cols[Random.Range(0, cols.Length)];
				currentTarget=currentCollider.gameObject.GetComponent<Unit>();
				if(currentTarget!=null && currentTarget.Attribute.HP>0 )
				{
					ScanPriority=_ScanPriority.Nearest;
					AttackTarget=currentTarget;
				}
			}
		}
	}
	
}


public enum SkillType{Circle,CircleFly,Whirlwind}
[System.Serializable]
public class Skill
{
	public delegate void OnSkill(Skill skill);
	public OnSkill onSkill;
	
	public bool IsSkilled;
	public int SkillId;
	public float MaxCooldown = 10;
	public float CurCooldown = 0;
	public float SkillDuration = 1.333f;
	public float SkillEffectDuration = 3;
	public float SkillAnimDuration = 2;
	public float Cost = 50;
	public float Damage;
	public float AdditionDamage;
	public float Condition;
//	public float SkillScaneRadius = 20;
	public float SkillAttackRadius = 5;
	public int MaxHitCount = 6;  
	public bool isDieTrigger = false;
	//public bool isDesireWarSkill = false;
	public bool IsActive = true;
	public GameObject SkillPrefab;
	public GameObject HitEffectPrefab;
	public AudioClip CastAudioClip;
	public AudioClip HitAudioClip;
	public SkillType Type;
	public Skill[] SubSkills;
}



