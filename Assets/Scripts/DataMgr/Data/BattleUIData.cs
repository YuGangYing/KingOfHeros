using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using Packet;
using Network;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
public class HeroSelect
{
    //英雄ID
    public int nHeroId;
    //位置编号
    public int nLocation;
    //士兵数量
    public int nSoldierNum;

    public HeroSelect()
    {
    }
}

public class EnemyHero
{
    //英雄类型
    public int nHeroType;
    //英雄星级
    public int nHeroStar;
    //英雄等级
    public int nHeroLevel;
    //位置编号
    public int nLocation;
    //士兵数量
    public int nSoldierNum;
    //士兵等级
    public int nSoldierLevel;
    //士兵星级
    public int nSoldierArmyLevel;

    public struct SkillInfo
    {
            public int nId;
            public int nLevel;
    }

    List<SkillInfo> _skillList = new List<SkillInfo>();
    public void addSkill(SkillInfo skill)
    {
        this._skillList.Add(skill);
    }

    public SkillInfo[] skilList
    {
        get { return this._skillList.ToArray(); }
    }

    public EnemyHero()
    {
        _skillList.Clear();
    } 
}


namespace DataMgr
{
	public class BattleUIData
	{

		enum SoldierType
		{
			Pike,
			Cavalry,
			Archers,
			Wizard,
			Shield,
		};
		
		public class FightHeroInfo
		{
			public uint idHero;              // ID
			public uint idType;              // 英雄类型
			public byte u8Star;              // 星级
			public ushort usLevel;           // 等级
			public int quality;
			public int armyType;             //部队类型
			public string icon;              //头像
			
			public uint skill1;
			public uint skill2;
			public uint skill3;
		}
		
        public class PveReward
        {
            public uint idBattle;
            public uint idField;
            public byte cbResult;
            public byte cbStar;
            public uint u32Exp;
            public uint u32Coin;
            public uint u32Stone;
            public uint u32Diamond;
            public uint idItemType1;
            public uint u32Amount1;
            public uint idItemType2;
            public uint u32Amount2;

            public Dictionary<uint, uint> dicHeroExp = new Dictionary<uint, uint>();
        }

        public class PveEnterTimes
        {
            public uint idBattle;
            public uint idField;
            public uint enterTimes;
            public uint star;
        }

		public Dictionary<uint, FightHeroInfo> dicFightHero = new Dictionary<uint, FightHeroInfo>();
		public List<EnemyHero> EnemyHeroList = new List<EnemyHero>();
		public List<HeroSelect> HeroList = new List<HeroSelect>();
		public UIBattlePanel battlePanel = new UIBattlePanel();
		public UIBattleResult battleResult = new UIBattleResult();
        public Dictionary<uint, int> dicHeroPosition = new Dictionary<uint, int>();
        public Dictionary<uint, int> dicHeroDieArmy = new Dictionary<uint, int>();
        public Dictionary<uint, PveEnterTimes> dicEnterTime = new Dictionary<uint, PveEnterTimes>();

        public byte cbResult = 0;
        public PveReward m_PveReward = new PveReward();
        public int m_FieldId = 0;
        public int m_BattleId = 0;
        public bool isBattleReturn = false;
        public uint m_totalHPRemain = 0;
        public int m_totalHp = 0;

		public BattleUIData()
		{
		}
		
		public bool init()
		{
            // 注册消息处理函数
            NetworkMgr.me.getClient().RegMsgFunc<MSG_HERO_FIGHT_RESPONSE>(this.onHeroPos);
            NetworkMgr.me.getClient().RegMsgFunc<MSG_HERO_SET_BATTLE_POS_RESPONSE>(this.onBattlePos);
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_BATTLE_PVE_REWARD_EVENT>(this.onPveReward);
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_BATTLE_PVE_ENTERTIMES_RESPONSE>(this.onEnterTimesInfo);
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_BATTLE_PVE_START_RESPONSE>(this.onEnterPveBattle);

            return true;
		}
		
		public void release()
		{
			dicFightHero.Clear();
			EnemyHeroList.Clear();
			HeroList.Clear();
            dicHeroPosition.Clear();
		}

		public void reload()
		{
		}

        public void onHeroPos(ushort id, object ar)
        {
            MSG.Sgt.CheckMessageId<MSG_HERO_FIGHT_RESPONSE>(id);
            MSG_HERO_FIGHT_RESPONSE msg_struct = (MSG_HERO_FIGHT_RESPONSE)ar;

            ManeuverPanel mp = UI.PanelManage.me.GetPanel<ManeuverPanel>(PanelID.ManeuverPanel);
            //mp.dicHeroPosition.Clear();

            for (int i = 0; i < msg_struct.usCnt; ++i)
            {
                uint idHero = msg_struct.lst[i].idHero;
                int pos = (int)msg_struct.lst[i].u8Pos;

              //  mp.dicHeroPosition[idHero] = pos;
            }
        }

        public void SendHeroPos()
        {
            MSG_HERO_FIGHT_REQUEST msg = new MSG_HERO_FIGHT_REQUEST();
            int nIndex = 0;

            msg.usCnt = (ushort)dicHeroPosition.Count;
            msg.lst = new HERO_FIGHT[dicHeroPosition.Count];

            foreach (KeyValuePair<uint, int> kvp in dicHeroPosition)
            {
                HERO_FIGHT hf = new HERO_FIGHT();
                hf.idHero = kvp.Key;
                hf.u8Pos = (byte)kvp.Value;

                msg.lst[nIndex] = hf;
                ++nIndex;
            }

            NetworkMgr.me.getClient().Send(ref msg);
        }

        public void onBattlePos(ushort id, object ar)
        {
            MSG.Sgt.CheckMessageId<MSG_HERO_SET_BATTLE_POS_RESPONSE>(id);
            MSG_HERO_SET_BATTLE_POS_RESPONSE msg = (MSG_HERO_SET_BATTLE_POS_RESPONSE)ar;

            uint error = msg.unErr;
             Debug.LogError("onBattlePos error:" + error.ToString());
        }

        public void onPveReward(ushort id, object ar)
        {
            MSG.Sgt.CheckMessageId<MSG_CLIENT_BATTLE_PVE_REWARD_EVENT>(id);
            MSG_CLIENT_BATTLE_PVE_REWARD_EVENT msg = (MSG_CLIENT_BATTLE_PVE_REWARD_EVENT)ar;

            m_PveReward.idBattle = msg.idBattle;
            m_PveReward.idField = msg.idField;
            m_PveReward.cbResult = msg.cbResult;
            m_PveReward.cbStar = msg.cbMaxStar;
            m_PveReward.u32Coin = msg.u32Coin;
            m_PveReward.u32Stone = msg.u32Stone;
            m_PveReward.u32Diamond = msg.u32Diamond;
            m_PveReward.idItemType1 = msg.idItemType1;
            m_PveReward.u32Amount1 = msg.u32Amount1;
            m_PveReward.idItemType2 = msg.idItemType2;
            m_PveReward.u32Amount2 = msg.u32Amount2;

            for (int i = 0; i < msg.lst.Length; ++i)
            {
                m_PveReward.dicHeroExp[msg.lst[i].idHero] = msg.lst[i].u32AddExp;
            }

            battleResult.UpdateRewardData();
        }

        public void onEnterTimesInfo(ushort id, object ar)
        {
            MSG.Sgt.CheckMessageId<MSG_CLIENT_BATTLE_PVE_ENTERTIMES_RESPONSE>(id);
            MSG_CLIENT_BATTLE_PVE_ENTERTIMES_RESPONSE msg = (MSG_CLIENT_BATTLE_PVE_ENTERTIMES_RESPONSE)ar;

            dicEnterTime.Clear();
            for (int i = 0; i < msg.usCnt; ++i)
            {
                PveEnterTimes pet = new PveEnterTimes();

                pet.idBattle = msg.lst[i].idBattle;
                pet.idField = msg.lst[i].idField;
                pet.enterTimes = msg.lst[i].u16EnterTimes;
                pet.star = msg.lst[i].cbMaxStar;

                dicEnterTime.Add(pet.idField, pet);
            }

            BattlefieldPanel bp = UI.PanelManage.me.GetPanel<BattlefieldPanel>(PanelID.BattlefieldPanel);

            bp.AddBattleItem();
            bp.ShowDefFiled();
        }

        public void onEnterPveBattle(ushort id, object ar)
        {
            MSG.Sgt.CheckMessageId<MSG_CLIENT_BATTLE_PVE_START_RESPONSE>(id);
            MSG_CLIENT_BATTLE_PVE_START_RESPONSE msg = (MSG_CLIENT_BATTLE_PVE_START_RESPONSE)ar;

            if(DataManager.checkErrcode(msg.wErrCode))
            {
                SLG.GlobalEventSet.FireEvent(SLG.eEventType.ChangeScene, new SLG.EventArgs(MainController.SCENE_BATTLE));
            }
        }

        public void SendPVEBattleStartMsg(uint idBattle, uint idField, byte cbBuy)
        {
            MSG_CLIENT_BATTLE_PVE_START_REQUEST msg = new MSG_CLIENT_BATTLE_PVE_START_REQUEST();

            msg.idBattle = idBattle;
            msg.idField = idField;
            msg.u32TotalHP = (uint)GetTotalHP();
            msg.cbBuy = cbBuy;

            NetworkMgr.me.getClient().Send(ref msg);
        }

        public void SendBattlePveEndMsg()
        {
            MSG_CLIENT_BATTLE_PVE_END_REQUEST msg = new MSG_CLIENT_BATTLE_PVE_END_REQUEST();

            msg.idBattle = (uint)m_BattleId;
            msg.idField = (uint)m_FieldId;
            msg.u32TotalHPRemain = m_totalHPRemain;

            if (m_totalHPRemain <= 0 )
            {
                msg.cbResult = 0;
            }
            else
            {
                msg.cbResult = 1;
            }

            //英雄信息
            int nCount = SpawnManager.SingleTon().PlayerHeros.Count;
            msg.usCnt = (ushort)nCount;
            msg.lst = new HERO_BATTLE_END_INFO[nCount];

            for (int i = 0; i < nCount; ++i)
            {
                int id = SpawnManager.SingleTon().PlayerHeros[i].HeroId;
                float curHP = SpawnManager.SingleTon().PlayerHeros[i].Attribute.HP;
                msg.lst[i].idHero = (uint)id;
                msg.lst[i].u32HeroHP = (uint)curHP;

                if (dicHeroDieArmy.ContainsKey((uint)id))
                {
                    msg.lst[i].cbArmyDieAmount = (byte)dicHeroDieArmy[(uint)id];
                }
                else
                {
                    msg.lst[i].cbArmyDieAmount = 0;
                }
                
            }

            NetworkMgr.me.getClient().Send(ref msg);
        }

        public void SendPveEntertimesRequest()
        {
            MSG_CLIENT_BATTLE_PVE_ENTERTIMES_REQUEST msg = new MSG_CLIENT_BATTLE_PVE_ENTERTIMES_REQUEST();

            NetworkMgr.me.getClient().Send(ref msg);
        }

		public void ClearFightHero()
		{
			dicFightHero.Clear();
		}
		
		public void AddFightHero(uint id)
		{
			if (id == 0)
			{
				return;
			}
			
			Packet.HERO_INFO hi = new Packet.HERO_INFO();
            if (DataManager.getHeroData().getItemById(id, ref hi))
			{
				FightHeroInfo fhi = new FightHeroInfo();
				fhi.idHero = id;
				fhi.idType = hi.idType;
				fhi.u8Star = hi.u8Star;
				fhi.usLevel = hi.usLevel;
				fhi.skill1 = hi.unSkill1;
				
				fhi.skill1 = hi.usSkillLvl1;
				fhi.skill2 = hi.unSkill2;
				fhi.skill3 = hi.unSkill3;

                DataMgr.ConfigRow cr = null;
                DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)fhi.idType, out cr);
                if (cr != null)
                {
                    fhi.quality = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.QUALITY);
                    fhi.armyType = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.ARMY_TYPE);
                    fhi.icon = cr.getStringValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.PORTARAIT);
                }
				
				if (dicFightHero.ContainsKey(fhi.idHero))
				{
					dicFightHero.Remove(fhi.idHero);
				}
				
				dicFightHero.Add(fhi.idHero, fhi);
			}
			
		}
		
		public enum Effect
		{
			Adv2, // 优势
			Adv1, // 大优势
			NA,
			Dav1, // 大劣势
			Dav2, // 劣势
		}
		
		public Effect Compare(int c1, int c2)
		{
			if (c1 > 4 || c2 > 4 || c1 < 0 || c1 < 0)
			{
				return Effect.NA;
			}
			
			Effect[,] effects = new[,] {
				{Effect.NA, Effect.Adv2, Effect.Adv2, Effect.Dav2, Effect.Dav2},
				{Effect.Dav2, Effect.NA, Effect.NA, Effect.Adv2, Effect.NA},
				{Effect.Dav2, Effect.NA, Effect.NA, Effect.NA, Effect.Adv2},
				{Effect.Adv2, Effect.Dav2, Effect.NA, Effect.NA, Effect.NA},
				{Effect.Adv2, Effect.NA, Effect.Dav2, Effect.NA, Effect.NA},
			};
			return effects[c1, c2];
		}
		
		public class BattleField
		{
			public int nBattleID;         // 战役ID
            public string strBattleName;  // 战役名称
            public string strBattleDesc;  // 战役描述
			public int nBattlefieldID;    // 关卡ID
            public int nNextfieldID;    // 关卡ID
			public string strFieldName;   // 名称
			public int nStar;             // 星级
			public string strDesc;        // 描述
            public int nStyle;            //关卡类型：0－普通，1－活动，2－精英
			public string strEnemyName;   // 敌方名称
			public int nAwardExp;         // 获得经验
			public int nAwardGold;        // 获得金币
            public int nAwardStone;       // 获得宝石
            public int nAwardDiamond;     // 获得钻石
            public int nItem1;            // 物品1
            public int nItem1Count;       // 物品1数量
            public int nItem2;            // 物品2
            public int nItem2Count;       // 物品2数量
            public int nFieldHasBoss;     // 是否BOSS关卡：0－否，1－小BOSS，2－大BOSS
            public int nFreeTime;         // BOSS关卡免费进入次数
            public int nCharge;           // 收费的钻石数量
		}
		
		public Dictionary<int, BattleField> dic_BattleField = new Dictionary<int, BattleField>();

        public bool _LoadBattleFieldData()
        {
            if (dic_BattleField.Count > 0)
            {
                return true;
            }

            ConfigBase fieldCfg = DataMgr.DataManager.getConfig(CONFIG_MODULE.CFG_BATTLE_FIELD);

            if (fieldCfg == null)
            {
                return false;
            }

            foreach (ConfigRow cr in fieldCfg.rows)
            {
                BattleField bf = new BattleField();

                bf.nBattleID = cr.getIntValue(CFG_BATTLE_FIELD.BATTLE_ID);
                bf.strBattleName = cr.getStringValue(CFG_BATTLE_FIELD.BATTLE_NAME);
                bf.strBattleDesc = cr.getStringValue(CFG_BATTLE_FIELD.BATTLE_DESC);
                bf.nBattlefieldID = cr.getIntValue(CFG_BATTLE_FIELD.BATTLE_FIELD_ID);
                bf.nNextfieldID = cr.getIntValue(CFG_BATTLE_FIELD.NEXT_FIELD_ID);
                bf.strFieldName = cr.getStringValue(CFG_BATTLE_FIELD.BATTLE_FIELD_NAME);
                bf.nStar = cr.getIntValue(CFG_BATTLE_FIELD.BATTLE_FIELD_STAR);
                bf.strDesc = cr.getStringValue(CFG_BATTLE_FIELD.BATTLE_FIELD_DESC);
                bf.nStyle = cr.getIntValue(CFG_BATTLE_FIELD.BATTLE_STYLE);
                bf.strEnemyName = cr.getStringValue(CFG_BATTLE_FIELD.ENEMY_NAME);
                bf.nAwardExp = cr.getIntValue(CFG_BATTLE_FIELD.AWARD_EXP);
                bf.nAwardGold = cr.getIntValue(CFG_BATTLE_FIELD.AWARD_GOLD);
                bf.nAwardStone = cr.getIntValue(CFG_BATTLE_FIELD.AWARD_STONE);
                bf.nAwardDiamond = cr.getIntValue(CFG_BATTLE_FIELD.AWARD_DIAMOND);
                bf.nItem1 = cr.getIntValue(CFG_BATTLE_FIELD.ITEM1_TYPEID);
                bf.nItem1Count = cr.getIntValue(CFG_BATTLE_FIELD.ITEM1_COUNT);
                bf.nItem2 = cr.getIntValue(CFG_BATTLE_FIELD.ITEM2_TYPEID);
                bf.nItem2Count = cr.getIntValue(CFG_BATTLE_FIELD.ITEM2_COUNT);
                bf.nFieldHasBoss = cr.getIntValue(CFG_BATTLE_FIELD.FIELD_HAS_BOSS);
                bf.nFreeTime = cr.getIntValue(CFG_BATTLE_FIELD.FREE_TIMES);
                bf.nCharge = cr.getIntValue(CFG_BATTLE_FIELD.CHARGE);

                if (dic_BattleField.ContainsKey(bf.nBattlefieldID))
                {
                    dic_BattleField.Remove(bf.nBattlefieldID);
                }

                dic_BattleField.Add(bf.nBattlefieldID, bf);
            }

            return true;
        }
		
		public class BattleEnemy
		{
            public int nBattleID;             // 战役ID
            public int nBattlefieldID;        // 关卡ID
			public int nHeroType;             // 英雄类型
			public int nHeroStar;             // 英雄星级
			public int nHeroLevel;            // 英雄等级
			public int nLocation;             // 英雄位置 
			public int nSoldierNum;           // 带兵数量
			public int nSoldierType;          // 士兵类型
			public int nSoldierLevel;         // 士兵等级
			public int nSoldierArmyLevel;     // 士兵星级
			public int nHeroSkill1;
			public int nHeroSkill1Level;
			public int nHeroSkill2;
			public int nHeroSkill2Level;
			public int nHeroSkill3;
			public int nHeroSkill3Level;
		}
		
		public List<BattleEnemy> battleEnemyList = new List<BattleEnemy>();

        public bool _LoadBattleEnemyData()
        {
            if (battleEnemyList.Count > 0)
            {
                return true;
            }

            ConfigBase enemyCfg = DataMgr.DataManager.getConfig(CONFIG_MODULE.CFG_BATTLE_ENEMY);

            if (enemyCfg == null)
            {
                return false;
            }

            foreach (ConfigRow cr in enemyCfg.rows)
            {
                BattleEnemy item = new BattleEnemy();

                item.nBattleID = cr.getIntValue(CFG_BATTLE_ENEMY.BATTLE_ID);
                item.nBattlefieldID = cr.getIntValue(CFG_BATTLE_ENEMY.BATTLE_FIELD_ID);
                item.nHeroType = cr.getIntValue(CFG_BATTLE_ENEMY.HERO_TYPE);
                item.nHeroStar = cr.getIntValue(CFG_BATTLE_ENEMY.HERO_STAR);
                item.nHeroLevel = cr.getIntValue(CFG_BATTLE_ENEMY.HERO_LEVEL);
                item.nLocation = cr.getIntValue(CFG_BATTLE_ENEMY.LOCATION);
                item.nSoldierNum = cr.getIntValue(CFG_BATTLE_ENEMY.SOLDIER_NUM);
                item.nSoldierType = cr.getIntValue(CFG_BATTLE_ENEMY.SOLDIER_TYPE);
                item.nSoldierLevel = cr.getIntValue(CFG_BATTLE_ENEMY.SOLDIER_LEVEL);
                item.nSoldierArmyLevel = cr.getIntValue(CFG_BATTLE_ENEMY.SOLDIER_ARMY_LEVEL);
                item.nHeroSkill1 = cr.getIntValue(CFG_BATTLE_ENEMY.HERO_SKILL1);
                item.nHeroSkill1Level = cr.getIntValue(CFG_BATTLE_ENEMY.HERO_SKILL1_LEVEL);
                item.nHeroSkill2 = cr.getIntValue(CFG_BATTLE_ENEMY.HERO_SKILL2);
                item.nHeroSkill2Level = cr.getIntValue(CFG_BATTLE_ENEMY.HERO_SKILL2_LEVEL);
                item.nHeroSkill3 = cr.getIntValue(CFG_BATTLE_ENEMY.HERO_SKILL3);
                item.nHeroSkill3Level = cr.getIntValue(CFG_BATTLE_ENEMY.HERO_SKILL3_LEVEL);

                battleEnemyList.Add(item);
            }

            return true;
        }
		
		Unit m_curHero;// = new Unit();
		public int m_nCurHeroId = -1;
		
		public void SetCurHero(int nIndex, int id)
		{
			m_nCurHeroId = id;
			GetCurHero();
			
			if (m_nCurHeroId == -1)
			{
				SimpleTouch.me.enabled = false;
                CameraFollow.SingleTon().Follow(null);
			}
			else
			{
				SimpleTouch.me.enabled = true;
				battlePanel.ClearSkillCD();
                CameraFollow.SingleTon().Follow(m_curHero);
			}

            
		}
		
		public int GetCurHeroId()
		{
			return m_nCurHeroId;
		}

        public Unit GetCurHero() 
		{
			if (m_nCurHeroId != -1)
			{
                int nCount = SpawnManager.SingleTon().PlayerHeros.Count;

                for (int i = 0; i < nCount; ++i)
                {
                    if (m_nCurHeroId == SpawnManager.SingleTon().PlayerHeros[i].HeroId)
                    {
                        m_curHero = SpawnManager.SingleTon().PlayerHeros[i];
                    }
                    
                }
			}
			
			return m_curHero;
		}
		
		public void ShowSkillState(int nHero, int nSkill, int nState, int nType)
		{
			battlePanel.UpdateSkillIcon(nHero, nSkill, nState, nType);
		}
		
		public void SetRedHP(float uCurHP, float uMaxHP)
		{
			battlePanel.nCurSelfHP = uCurHP;
			battlePanel.nMaxSelfHP = uMaxHP;
			battlePanel.UpdateSlider();
		}
		
		public void SetBlueHP(float uCurHP, float uMaxHP)
		{
			battlePanel.nCurEnemyHP = uCurHP;
			battlePanel.nMaxEnemyHP = uMaxHP;
			battlePanel.UpdateSlider();
		}
		
		public void SetHeroHPbyIndex(int nIndex, int nCurValue, int nMaxValue)
		{
			battlePanel.SetHeroHP(nIndex, nCurValue, nMaxValue);
		}

        int intervalTime = 0;

        public void BattleUIUpdate()
		{
//             ++intervalTime;
// 
//             if (intervalTime % 3 != 0)
//             {
//                 return;
//             }
// 
//             intervalTime = 0;
            
            //当前英雄
            int curHeroId = GetCurHeroId();
            //血条
            float maxHPLeft = BattleController.SingleTon().MaxPlayerFighting;
            float curHPLeft = BattleController.SingleTon().CurrentPlayerFighting;
            float maxHPRight = BattleController.SingleTon().MaxEnemyFighting;
            float curHPRight = BattleController.SingleTon().CurrentEnemyFighting;

            if (curHPLeft < 0)
            {
                m_totalHPRemain = 0;
            }
            else
            {
                m_totalHPRemain = (uint)curHPLeft;
            }

            SetBlueHP(curHPRight, maxHPRight);
            SetRedHP(curHPLeft, maxHPLeft);

            if (curHPRight > 0)
            {
                cbResult = 1;
            }
            else
            {
                cbResult = 0;
            }

            //能量
            float curAnger = BattleController.SingleTon().CurrentEnergy;
            float maxAnger = BattleController.SingleTon().MaxEnergy;
            battlePanel.SetEngryValue(curAnger, maxAnger);

            //英雄信息
            int nCount = SpawnManager.SingleTon().PlayerHeros.Count;

            for (int i = 0; i < nCount; ++i)
            {
                //英雄血条
                int id = SpawnManager.SingleTon().PlayerHeros[i].HeroId;

                float curHP = SpawnManager.SingleTon().PlayerHeros[i].Attribute.HP;
                float maxHP = SpawnManager.SingleTon().PlayerHeros[i].Attribute.MaxHP;
                battlePanel.UpdateHeroHP(id, curHP, maxHP);

                //英雄技能
                int nSkillCount = SpawnManager.SingleTon().PlayerHeros[i].Skill.HeroSkills.Count;

                for (int j = 0; j < nSkillCount; ++j)
                {

					DataMgr.Skill skill = SpawnManager.SingleTon().PlayerHeros[i].Skill.HeroSkills[j];
					if (skill == null)
						continue;

                    //int skillId = SpawnManager.SingleTon().PlayerHeros[i].Skill.HeroSkills[j].SkillId;
					int skillId = skill.SkillId;
									
                    DataMgr.Skill skillConfig = DataManager.getSkillData().getSkillBySkillID(skillId);

					float curCD = skill.CurCooldown;
					float maxCD = skill.MaxCooldown;

					if (skill != null)
                    {
						int _execMode = 0;

                        if (skillConfig.IsActive)
                        {
                            _execMode = 2;
                        }
                        else
                        {
                            _execMode = 1;
                        }

                        int nStatus = 1;

                        if (curCD > 0 && curCD != maxCD)
                        {
                            nStatus = 2;
                        }

                        if (curAnger < skill.Cost)
                        {
                            nStatus = 2;
                        }

                        battlePanel.UpdateSkillIcon(i, j, nStatus, (int)_execMode);
                    }

                    //选中英雄技能状态
                    if (curHeroId == id && skill != null)
                    {
                        int nIcon = skill.SkillId;
                        string strIconName = nIcon.ToString();
                        battlePanel.SetSkillIcon(j, strIconName);

                        string strGestureName = "gesture" + skill.FingerType.ToString();
                        battlePanel.SetSkillGesture(j, strGestureName);

                        if (curCD > 0 && curAnger >= skill.Cost)
                        {
                            battlePanel.UpdateSkillCD(j, maxCD, maxCD - curCD);
                            battlePanel.RestoreSkill();
                        }

                        if (curHP > 0 && curAnger < skill.Cost)
                        {
                            battlePanel.GarySkillByIndex(j);
                        }

                        if (curHP <= 0)
                        {
                            battlePanel.GraySkill();
                        }
                    }
                }
            }
		}
		
		public void ShowBattleUI(bool bShow)
		{
			battlePanel.SetVisible(bShow);
		}
		
		public void ShowResllt(bool bShow)
		{
			battleResult.SetVisible(bShow);
		}
		
		public int GetCombatValue(uint id)
		{
			int value = 0;
			Packet.HERO_INFO hero = new Packet.HERO_INFO();
            if (!DataManager.getHeroData().getItemById(id, ref hero))
			{
				return 0;
			}

            DataMgr.ConfigRow cr = null;
            DataMgr.CHerroTalbeAttribute.getHeroBaseDetail((int)hero.idType, out cr);
            if (cr == null)
                return 0;

            int atk = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_ATTACK);
            int hp = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_HP);
            int def = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_DEF);
            int lead = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_LEADER);
            int crit = cr.getIntValue(DataMgr.enCVS_HERO_BASE_ATTRIBUTE.BASE_VIOLENCE);
			
			// cur
			int lLv = hero.usLevel;
			int lStar = hero.u8Star;

            DataMgr.ConfigRow crstar = null;
            DataMgr.CHerroTalbeAttribute.getHeroStar((int)hero.idType, lStar, out crstar);
            if (crstar == null)
                return 0;

            int nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_ATTACK);
            int nCurAtc = CardQualityUpdatePanel.GetValue((float)atk, (int)lStar, (int)lLv, nFactor);

            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_HP);
            int nCurHP = CardQualityUpdatePanel.GetValue((float)hp, (int)lStar, (int)lLv, nFactor);

            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_DEF);
            int nCurDef = CardQualityUpdatePanel.GetValue((float)def, (int)lStar, (int)lLv, nFactor);

            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_LEADER);
            int nCurLead = CardQualityUpdatePanel.GetValue((float)lead, (int)lStar, (int)lLv, nFactor);

            nFactor = crstar.getIntValue(DataMgr.enCVS_HERO_STAR_ATTRIBUTE.FACTOR_VIOLENCE);
            int nCurViolence = CardQualityUpdatePanel.GetValue((float)crit, (int)lStar, (int)lLv, nFactor);
			
			uint ad = 0; //远程为0，近战为1  暂无
			//uint skill1 = hero.unSkill1;
			uint skill2 = hero.unSkill2;
			uint skill3 = hero.unSkill3;
			uint slvl1 = hero.usSkillLvl1;
			uint slvl2 = 0;
			uint slvl3 = 0;
			int SNum = 1;
			
			if (skill2 != 0)
			{
				SNum = 2;
				slvl2 = hero.usSkillLvl2;
			}
			
			if (skill3 != 0)
			{
				SNum = 3;
				slvl3 = hero.usSkillLvl3;
			}
			
			value = (int)(1 * nCurHP + 40 * nCurAtc + 20 * nCurDef + 30 * nCurLead + 10 * nCurViolence) * (int)(1 + 0.04 * ad)
				* (int)(1 + 0.02 * SNum + 0.01 * slvl1 + 0.02 * slvl2 + 0.03 * slvl3);
			
			return value;
		}


        public void HeroArmyDie(uint idHero)
        {
            if (dicHeroDieArmy.ContainsKey(idHero))
            {
                ++dicHeroDieArmy[idHero];
            }
            else
            {
                dicHeroDieArmy.Add(idHero, 0);
            }
        }

        public int GetTotalHP()
        {
            m_totalHp = 0;
            
            for (int i = 0; i < HeroList.Count; ++i)
            {
                Fight.Hero tmp_hero = Fight.FightData.getSelfHero(HeroList[i].nHeroId);

                Fight.Soldier[] tmp_soldierlist = Fight.FightData.getSelfSoldier(tmp_hero.soldierType, HeroList[i].nSoldierNum);

                for (int j = 0; j < tmp_soldierlist.Length; ++j)
                {
                    m_totalHp += tmp_soldierlist[j].maxHp;
                }

                m_totalHp += tmp_hero.maxHp;
            }

            return m_totalHp;
        }
	}
}