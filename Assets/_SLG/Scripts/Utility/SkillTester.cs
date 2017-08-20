using System;
using UnityEngine;

public class SkillTester
{
	public SkillTester ()
	{
	}

	// 游侠印记
	public static Skill GetDefaultRangerMark(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.RangerMark;
		skill.MaxCooldown = 20;
		skill.CurCooldown = 0;//TODO
		skill.SkillEffectDuration = 10;
		skill.SkillAnimDuration = 2;
		skill.SkillAttackRadius = 40;
		skill.Damage = 101;
		skill.Cost = 150;
		skill.SkillPrefab = shootPrefab;
		//skill.CastAudioClip = SpawnManager.SingleTon().RobinHanSkillAudio01;
		//skill.HitEffectPrefab = SpawnManager.SingleTon().RichardSkillEffect03;
		return skill;
	}

	//急速狙击
	public static Skill GetDefaultRapidSnipe(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.RapidSnipe;
		skill.MaxCooldown = 20;
		skill.CurCooldown = 0;//TODO
		skill.SkillAnimDuration = 2;
		skill.SkillAttackRadius = 40;
		skill.Damage = 20;
		skill.AdditionDamage = 10;
		skill.Cost = 160;
		skill.SkillPrefab = shootPrefab;
		//skill.CastAudioClip = SpawnManager.SingleTon().RobinHanSkillAudio02;
		//skill.HitEffectPrefab = SpawnManager.SingleTon().RobinHanSkillHitEffect02;
		return skill;
	}

	//翼狮 
	public static Skill GetLammasu(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.Lammasu;
		skill.SkillPrefab = shootPrefab;
		skill.MaxCooldown = 20;
		skill.CurCooldown = 0;//TODO
		skill.SkillAnimDuration = 2;	
//		skill.SkillAttackRadius = 5;
		skill.SkillEffectDuration = 500;
		skill.Cost = 150;
		//skill.CastAudioClip = SpawnManager.SingleTon().RichardSkillAudio01;
		//skill.HitEffectPrefab = SpawnManager.SingleTon().RichardSkillEffect01;
		return skill;
	}

	//光翼恩赐
	public static Skill GetLightWingGift(UnitSkill unitSkill)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.LightWingGift;
		skill.Damage = 0.5f;
		skill.MaxCooldown = 20;
		skill.CurCooldown = 0;
		skill.SkillAnimDuration = 2;
		skill.SkillAttackRadius = 40;
		skill.SkillEffectDuration = 3;
		skill.Cost = 150;
		//skill.CastAudioClip = SpawnManager.SingleTon().RichardSkillAudio02;
		//skill.HitEffectPrefab = SpawnManager.SingleTon().RichardSkillEffect02;
		return skill;
	}

	//狮心不朽
	public static Skill GetLionHeartImmotal(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		skill.Damage = 0.5f;
		skill.SkillPrefab = shootPrefab;
		//skill.onSkill += unitSkill.LionHeartImmotral;
		return skill;
	}

	//夺命投掷
	public static Skill GetDeadlyThrow(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.DeadlyThrow;
		skill.Damage = 5;
		skill.MaxCooldown = 20;
		skill.CurCooldown = 0;
		skill.SkillAnimDuration = 2;
		skill.SkillAttackRadius = 40;
		skill.SkillPrefab = shootPrefab;
		//skill.CastAudioClip = SpawnManager.SingleTon().SpartaSkillAudio01;
		//skill.HitEffectPrefab = SpawnManager.SingleTon().SpartaSkillEffect01;
		return skill;
	}

	//战争渴望
	public static Skill GetDesireWarSkill(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.DesireWarSkill;
		skill.MaxCooldown = 3;
		skill.CurCooldown = 1;//TODO
		skill.SkillEffectDuration = 1;
		skill.SkillAnimDuration = 2;
		skill.SkillAttackRadius = 20;
		skill.Damage = 10;
		skill.Cost = 150;
		skill.MaxHitCount = 8;
		skill.SkillPrefab = shootPrefab;
		return skill;
	}

	//存亡之歌
	public static Skill GetSurviveSongSkill(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.SurviveSongSkill;
		skill.MaxCooldown = 20;
		skill.CurCooldown = 0;//TODO
		skill.SkillEffectDuration = 1;
		skill.SkillAnimDuration = 2;
		skill.SkillAttackRadius = 20;
		skill.Damage = 10;
		skill.Cost = 150;
		skill.MaxHitCount = 8;
		skill.SkillPrefab = shootPrefab;
		return skill;
	}

	//剧毒之触
	public static Skill GetToxTouchSkill(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.ToxTouchSkill;
		skill.MaxCooldown = 20;
		skill.CurCooldown = 0;//TODO
		skill.SkillEffectDuration = 1;
		skill.SkillAnimDuration = 2;
		skill.SkillAttackRadius = 20;
		skill.Damage = 50f;
		skill.Cost = 150;
		skill.MaxHitCount = 10;
		skill.SkillPrefab = shootPrefab;
		return skill;
	}

	//石化凝视
	public static Skill GetPetrifactionSkill(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.PetrifactionSkill;
		skill.MaxCooldown = 10;
		skill.CurCooldown = 1;//TODO
		skill.SkillEffectDuration = 10;
		skill.SkillAnimDuration = 3;
		skill.SkillAttackRadius = 40;
		skill.Damage = 50f;
		skill.Cost = 150;
		skill.MaxHitCount = 10;
		skill.SkillPrefab = shootPrefab;
		return skill;
	}

	//众志成城
	public static Skill GetUnityIsStrengthSkill(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.UnityIsStrengthSkill;
		skill.MaxCooldown = 10;
		skill.CurCooldown = 1;//TODO
		skill.SkillEffectDuration = 1;
		skill.SkillAnimDuration = 3;
		skill.SkillAttackRadius = 10;
		skill.Damage = 0.33f;
		skill.Cost = 150;
		skill.MaxHitCount = 10;
		skill.SkillPrefab = shootPrefab;
		return skill;
	}

	//王者牺牲
	public static Skill GetKingSacrificeSkill(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.KingSacrificeSkill;
		skill.MaxCooldown = 10;
		skill.CurCooldown = 1;//TODO
		skill.SkillEffectDuration = 1;
		skill.SkillAnimDuration = 3;
		skill.SkillAttackRadius = 10;
		skill.Damage = 0.22f;
		skill.Cost = 150;
		skill.MaxHitCount = 10;
		skill.SkillPrefab = shootPrefab;
		return skill;
	}

	//英雄哀歌
	public static Skill GetHeroMournfulSongSkill(UnitSkill unitSkill,GameObject shootPrefab)
	{
		Skill skill = new Skill();
		//skill.onSkill += unitSkill.HeroMournfulSongSkill;
		skill.MaxCooldown = 10;
		skill.CurCooldown = 1;//TODO
		skill.SkillEffectDuration = 1;
		skill.SkillAnimDuration = 3;
		skill.SkillAttackRadius = 10;
		skill.Damage = 0.4f;
		skill.Cost = 150;
		skill.MaxHitCount = 5;
		skill.SkillPrefab = shootPrefab;
		skill.isDieTrigger = true;
		skill.IsActive = false;
		return skill;
	}
}
