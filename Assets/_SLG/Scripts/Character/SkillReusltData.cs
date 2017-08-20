using System;
using System.Collections.Generic;
using DataMgr;
using UnityEngine;

namespace Fight
{
    public class SkillReusltData
    {
         public float repeatInterval
        {
            get;
            set;
        }

        public SkillReusltData()
        {

        }

        public Character attacker
        {
            get;
            set;
        }

        public ConfigRow config
        {
            get;
            set;
        }

        //技能等级
        public int skillLevel
        {
            get;
            set;
        }

        public int skillId
        {
            get;
            set;
        }

        public float stateTime
        {
            get;
            set;
        }

        public int doneTimes
        {
            get;
            set;
        }

        public SKILL_REUSLT_REPEAT_TYPE repeatType
        {
            get;
            set;
        }

        public void clear()
        {
            this.stateTime = 0;
            this.doneTimes = 0;
        }

        public int id
        {
            get;
            set;
        }

        public SKILL_RESULT_TYPE resultType
        {
            get;
            set;
        }

        public void init()
        {
//             if (this.config == null)
//                 return;
//             this.id = this.config.getIntValue(CFG_SKILL_RESULT.ID, 0);
//             this.repeatInterval = this.config.getFloatValue(CFG_SKILL_RESULT.REPEATINTERVAL, 0f);
//             this.repeatType = this.config.getEnumValue(CFG_SKILL_RESULT.REPEATTYPE, SKILL_REUSLT_REPEAT_TYPE.UNKNOWN);
//             //效果类型
//             this.resultType = this.config.getEnumValue<SKILL_RESULT_TYPE>(CFG_SKILL_RESULT.RESULTTYPE, SKILL_RESULT_TYPE.UNKNOWN);
//             this._condition = this.config.getEnumValue<SKILL_RESULT_CONDITION>(CFG_SKILL_RESULT.CONDITION, SKILL_RESULT_CONDITION.NONE);
            //注册死亡消息
            //if (this._condition == SKILL_RESULT_CONDITION.DEATH)
            //    CharacterMgr.me.addDeathFunc(this.onDeathFunc);

            this.doneTimes = 0;
        }

        //技能实施
        public void onExec(object paramData = null)
        {
            //if (this.config == null || this.attacker == null)
            //    return;
            //this.stateTime = Time.time;
            //this.doneTimes++;

            //Character[] charList = getResultObjs(paramData);
            //if (charList == null || charList.Length == 0)
            //    return;
            //for (int n = 0; n < charList.Length; n++)
            //{
            //    //检测触发条件是否满足
            //    if (!checkCondition(charList[n]))
            //        continue;
            //    onExecObj(charList[n], paramData);
            //}
        }

        void onExecObj(Character obj, CBattleFightGestureData paramData = null)
        {
            //SkillResultBase result = SkillResultMgr.getResult(resultType);
            //if (result == null)
            //    return;
            //result.attacker = this.attacker;
            //result.resultId = this.id;
            //result.skillId = this.skillId;
            //result.atkObject = obj;
            //result.level = this.skillLevel;
            //result.resultType = resultType;
            //result.param = paramData;
            //result.init(this.config);
            ////加入
            //obj.addSkillResult(result);
        }

        //根据效果配置获取目标对象群
        Character[] getResultObjs(object paramData)
        {
            //if (this.config == null || this.attacker ==null)
            //    return null;

            ////获取对象范围
            //Character[] charList = getObjectsByType();
            //if (charList == null || charList.Length == 0)
            //    return null;
            ////按范围筛选
            //charList = getObjectsByRang(charList, paramData);
            //if (charList == null || charList.Length == 0)
            //    return null;
            ////按数量排序和筛选
            //charList = getObjectsBySort(charList);
            //if (charList == null || charList.Length == 0)
            //    return null;
            //return charList;
            return null;
        }

        //根据效果排序方式进行筛选
        Character[] getObjectsBySort(Character[] srcList)
        {
            //if (srcList == null)
            //    return null;
            //if (srcList.Length <= 1)
            //    return srcList;
            ////排序指标
            //SKILL_RESULT_OBJECSORT sortType = this.config.getEnumValue<SKILL_RESULT_OBJECSORT>(CFG_SKILL_RESULT.SORTTYPE, SKILL_RESULT_OBJECSORT.UNKNOWN);
            //if (sortType == SKILL_RESULT_OBJECSORT.UNKNOWN)
            //    return srcList;
            ////升降序规则
            //SKILL_RESULT_SORTMODE sortMode = SKILL_RESULT_SORTMODE.ASCEND;

            //List<Character> destList = new List<Character>();
            //foreach (Character item in srcList)
            //    destList.Add(item);
            ////先排序
            //if (sortType == SKILL_RESULT_OBJECSORT.HP)//按生命值
            //{
            //    destList.Sort(delegate(Character left, Character right)
            //    {
            //        if (sortMode == SKILL_RESULT_SORTMODE.ASCEND)//升序
            //        {
            //            if (left.curHp >= right.curHp)
            //                return 1;
            //            else
            //                return -1;
            //        }
            //        else//降序
            //        {
            //            if (left.curHp < right.curHp)
            //                return 1;
            //            else
            //                return -1;
            //        }
            //        return 0;
            //    });
            //}

            ////取最大数量数
            //int nMaxNumer = this.config.getIntValue(CFG_SKILL_RESULT.MAXNUMBER, 0);
            //if (nMaxNumer > 0)
            //    destList.RemoveRange(nMaxNumer, destList.Count - nMaxNumer);
            //return destList.ToArray();
            return null;
        }

        //根据攻击范围筛选
        Character[] getObjectsByRang(Character[] srcList, object paramData)
        {
            //if (srcList == null)
            //    return null;
            //if (paramData == null)
            //    return srcList;

            //SKILL_RANG rangType = this.config.getEnumValue<SKILL_RANG>(CFG_SKILL_RESULT.RANGTYPE, SKILL_RANG.UNKNOWN);
            ////范围
            //float rangValue1 = this.config.getFloatValue(CFG_SKILL_RESULT.RANGVALUE1, 0f);
            //float rangValue2 = this.config.getFloatValue(CFG_SKILL_RESULT.RANGVALUE2, 0f);
            ////范围类型判断（暂时未处理）
            //switch (rangType)
            //{
            //    case SKILL_RANG.ALL://无需范围判断直接返回读写
            //        return srcList;
            //    case SKILL_RANG.CIRLE_ME://以自身
            //        //                  return LocationMgr.me.getCharacterListInCircle();
            //        break; ;
            //    case SKILL_RANG.CIRCLE_OTHER:
            //        break;
            //    case SKILL_RANG.FAN:
            //        break;
            //    default:
            //        break;
            //}

            return null;
        }

        //根据目标类型筛选
        Character[] getObjectsByType()
        {
            ////对象类型
            //SKILL_SIDE skillSide = this.config.getEnumValue<SKILL_SIDE>(CFG_SKILL_RESULT.SIDE, SKILL_SIDE.UNKNOWN);
            //SIDE side = SIDE.UNKNOWN;
            //switch (skillSide)
            //{
            //    case SKILL_SIDE.ALL:
            //        side = SIDE.ALL;
            //        break;
            //    case SKILL_SIDE.SELF:
            //        side = this.attacker.side;
            //        break;
            //    case SKILL_SIDE.ENEMY:
            //        side = this.attacker.enemySide;
            //        break;
            //    default:
            //        return null;
            //}

            ////对象类型
            //SKILL_OBJTYPE objType = this.config.getEnumValue<SKILL_OBJTYPE>(CFG_SKILL_RESULT.OBJECTYPE, SKILL_OBJTYPE.UNKNOWN);
            //switch (objType)
            //{
            //    case SKILL_OBJTYPE.ALL:
            //        return CharacterMgr.me.getCharacterList(side);
            //    case SKILL_OBJTYPE.ALL_ROW://英雄所在行
            //        return CharacterMgr.me.getCharacterListRow(side, attacker.location);
            //    case SKILL_OBJTYPE.ALL_MATRIX:
            //        return CharacterMgr.me.getCharacterList(side, attacker.location);
            //    case SKILL_OBJTYPE.HERO:
            //        return CharacterMgr.me.getHeroList(side);
            //    case SKILL_OBJTYPE.HERO_ROW:
            //        return CharacterMgr.me.getHeroListRow(side, attacker.location);
            //    case SKILL_OBJTYPE.SELF:
            //    case SKILL_OBJTYPE.HERO_MATRIX:
            //        return CharacterMgr.me.getHeroList(side, attacker.location);
            //    case SKILL_OBJTYPE.SOLDIER:
            //        return CharacterMgr.me.getSoldierList(side);
            //    case SKILL_OBJTYPE.SOLDIER_MATRIX:
            //        return CharacterMgr.me.getSoldierList(side, attacker.location);
            //    case SKILL_OBJTYPE.SOLDIER_ROW:
            //        return CharacterMgr.me.getSoldierListRow(side, attacker.location);
            //    default:
            //        return null;
            //}
            return null;
        }

        //按死亡条件执行
        public void onDeathFunc(Character deathObj)
        {
            //if (deathObj == null || this.config == null || this.attacker == null)
            //      return;
            //  //当前没有死亡
            //  if (!deathObj.isDeath())
            //      return;
  
            //  //敌我双方判断
            //  SKILL_SIDE side = config.getEnumValue<SKILL_SIDE>(CFG_SKILL_RESULT.CONDITIONSIDE, SKILL_SIDE.UNKNOWN);
            //  if (side == SKILL_SIDE.UNKNOWN)
            //      return;
            //  else if (side == SKILL_SIDE.ENEMY)//敌人
            //  {
            //      if (deathObj.side == this.attacker.side)
            //          return;
            //  }
            //  else if (side == SKILL_SIDE.ENEMY)//本方
            //  {
            //      if (deathObj.side != this.attacker.side)
            //          return;
            //  }
  
            //  //对象类型判断
            //  SKILL_OBJTYPE objType = config.getEnumValue<SKILL_OBJTYPE>(CFG_SKILL_RESULT.CONDITIONOBJTYPE, SKILL_OBJTYPE.UNKNOWN);
            //  if (objType == SKILL_OBJTYPE.UNKNOWN)
            //      return;
            //  else if (objType == SKILL_OBJTYPE.SELF)
            //  {
            //      if (this.attacker.id != deathObj.id)
            //          return;
            //  }
            //  else if (objType == SKILL_OBJTYPE.HERO)
            //  {
            //      if (!deathObj.isHero)
            //          return;
            //  }
            //  else if (objType == SKILL_OBJTYPE.SOLDIER)
            //  {
            //      if (deathObj.isHero)
            //          return;
            //  }
            //  //执行技能
            //  this.onExec(null);
          }

        //检测是否符合触发条件(判断暂缺)
        public bool checkCondition(Character atkObject)
        {
            //if (atkObject == null)
            //    return false;
            ////无条件限制
            //if (this.config == null)
            //    return false;
            //float fValue = this.config.getFloatValue(CFG_SKILL_RESULT.CONDITIONVALUE, 0f);
            //switch (this._condition)
            //{
            //    case SKILL_RESULT_CONDITION.NONE://无条件执行
            //        return true;
            //    case SKILL_RESULT_CONDITION.HPLEVEL://生命最低值
            //        if (atkObject.curHp / atkObject.baseHp < fValue)
            //            return true;
            //        break;
            //    case SKILL_RESULT_CONDITION.EFFECT://具有某种特效
            //        if (atkObject.resultMgr.haveEeffect((int)fValue))
            //            return true;
            //        break;
            //    case SKILL_RESULT_CONDITION.DEATH://死亡类型，通过回调函数处理。
            //        return false;
            //        break;
            //    default:
            //        break;
            //}
            //return false;
            return false;
        }
    }
}
