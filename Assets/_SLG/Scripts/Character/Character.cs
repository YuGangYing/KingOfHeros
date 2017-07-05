using System;
using UnityEngine;
using System.Collections.Generic;

 

namespace Fight
{
    public enum SIDE
    {
        LEFT,
        RIGHT,
        ALL,
        UNKNOWN
    }

    //角色基类
    public class Character : IEquatable<Character>
    {
        #region define variable

        //HP
        //基础HP（不变）
        protected int _nBaseHp;
        protected int _nMaxHp;
        protected int _nCurHp;
        //DEF
        protected float _fBaseDef;
        protected float _fCurDef;
        //移动速度
        protected float _fBaseSpeed;
        protected float _fCurSpeed;
        //普通攻击参数
        //攻击力
        protected float _fBaseDamage;
        protected float _fCurDamage; 
        //攻击延时
        protected float _atkRelayTime;
        //攻击速度
        protected float _atkBaseSpeed;
        protected float _atkCurSpeed;
        //攻击范围
        protected float _atkRang;
        protected float _curAtkRang;
        //攻击系数
        protected float _atkRatio;

        protected int _nId;
        //角色名
        protected string _strName;
        //角色资源名
        protected string _strResName;
        //
        protected int _nLevel;
        //所属
        protected SIDE _side;
        //
        protected SIDE _enemySide;
        //队列
        //protected ROW _nRowid;
        //方阵编号
        protected int _nLocationId;
        //对象
        protected GameObject _root = null;
        //
        protected Animator _animator = null;
        //士兵类型
        protected ARMY_TYPE _charType;        

        //被施加技能列表
        //SkillResultMgr _skillResultMgr = new SkillResultMgr();
        //被施加技能特效管理
        //SkillResultEffectMgr _effectMgr = null; 
        
        #endregion
        #region define getset

        public int id
        {
            get { return this._nId; }
            set { this._nId = value; }
        }

        public string name
        {
            get { return this._strName; }
            set { this._strName = value; }
        } 
        
        public int baseHp
        {
            get { return this._nBaseHp; }
            set 
            { 
                this._nBaseHp = value;
                this._nCurHp = value;
                this._nMaxHp = value;
            }
        }

        public int maxHp
        {
            get { return this._nMaxHp; }
            set { this._nMaxHp = value; }
        }

        public int curHp
        {
            get { return this._nCurHp; }
        }

        public float atkBaseSpeed
        {
            get { return this._atkBaseSpeed; }
            set { this._atkBaseSpeed = value; }
        }

        public float atkCurSpeed
        {
            get { return this._atkCurSpeed; }
            set { this._atkCurSpeed = value; }
        }

        public float atkBaseAttackRange
        {
            get { return this._atkRang;}
            set{this._atkRang = value;}
        }

        public bool Equals(Character obj)
        {
            if (obj == null)
                return false;
            return this.id == obj.id;
        }

        //加血（）
        public int addHp(uint value)
        {
            if (value == 0)
                return 0;
            if (this.isDeath())//通知复活
            {
            }
            int temp = this._nCurHp + (int)value;
            if (temp > this._nMaxHp)
            {
                int addValue = this.maxHp - this.curHp;
                this._nCurHp = this._nMaxHp;
                return addValue;
            }
            else
            {
                this._nCurHp = temp;
                return (int)value;
            }
        }

        public virtual void release()
        {
            if (this.root != null)
            {
                UnityEngine.GameObject.Destroy(this.root);
                this.root = null;
            }
            
        }

        //返回实际伤害值
        public int onDamage(int value)
        {
            this._nCurHp -= value;
            if (this._nCurHp <= 0)
            {
                int temp = this._nCurHp;
                this._nCurHp = 0;
                
                return temp;
            }
            else
                return value;
        }

        public float baseDef
        {
            get { return this._fBaseDef; }
            set 
            { 
                this._fBaseDef = value;
                this._fCurDef = value;
            }
        }

        public float curDef
        {
            get { return this._fCurDef; }
            set { this._fCurDef = value; }
        }

        public float baseSpeed
        {
            get { return this._fBaseSpeed; }
            set 
            { 
                this._fBaseSpeed = value;
                this._fCurSpeed = value * UnityEngine.Random.Range(0.95f, 1.05f);
            }
        }

        public float curSpeed
        {
            get { return this._fCurSpeed; }
            set { this._fCurSpeed = value; }
        }

        public float baseDamage
        {
            get { return this._fBaseDamage; }
            set 
            {
                this._fBaseDamage = value;
                this._fCurDamage = value;
            }
        }

        public float curDamage
        {
            get { return this._fCurDamage; }
            set { this._fCurDamage = value; }
        }

        public SIDE side
        {
            get { return _side; }
            set 
            {
                _side = value;
                if (_side == SIDE.LEFT)
                    this._enemySide = SIDE.RIGHT;
                else
                    this._enemySide = SIDE.LEFT;
            }
        }

        public SIDE enemySide
        {
            get { return _enemySide; }
        }

        public GameObject root
        {
            get { return this._root; }
            set
            {
                _root = value;
                if (_root != null)
                {
                    this.initRoot();
                }
            }
        }

        public string strResName
        {
            get { return _strResName; }
            set { _strResName = value; }
        }

        public Quaternion rotation
        {
            set
            {
                if (_root != null)
                    _root.transform.rotation = value;
            }
        }

        public Vector3 position
        {
            set
            {
                if (_root != null)
                    _root.transform.position = value;
            }
        }

        public ARMY_TYPE armyType
        {
            get { return this._charType; }
            set { this._charType = value; }
        }

        public int level
        {
            get { return this._nLevel; }
            set { this._nLevel = value; }
        }

        public int location
        {
            get { return this._nLocationId; }
            set
            {
                this._nLocationId = value;
               // this._nRowid = LocationMgr.getRow(value);
            }
        }

        //public ROW row
        //{
        //    get { return _nRowid; }
        //}

        public bool isHero
        {
            get { return this.armyType== ARMY_TYPE.HERO; }
        }

        public float AttackRelayTime
        {
            get { return this._atkRelayTime; }
            set { this._atkRelayTime = value; }
        }

        #endregion
        #region define virtual function

        protected virtual void initRoot()
        {
           
        }

        public virtual void update()
        {
            
        }

        public virtual void setAnimatorBool(string strName, bool bValue)
        {
            if (this._animator != null)
            {
                this._animator.SetBool(strName, bValue);
            }
        }

        #endregion
        #region define public function

        public Character()
        {
            
        }

        //是否死亡 
        public bool isDeath()
        {
            //if (m_BhvBase.GetCurStateId() == GameAi.StateId.DeathStateId)
            //{
            //    return true;
            //}
            return false;
        }  

        //播放特效，N秒后自动删除自身
        public GameObject showEffect(Transform atkObject,string strEffect,float fTime)
        {
            //if (strEffect == null || strEffect == string.Empty)
            //    return null;
            //Debug.Log(strEffect);
            //GameObject prefab = ResourcesMgr.LoadAsset<GameObject>(strEffect);
            //if (prefab == null)
            //    return null;
            //GameObject effectObj = GameObject.Instantiate(prefab) as GameObject;
            //effectObj.transform.parent = this.root.transform;
            //effectObj.transform.localPosition = Vector3.zero;
            //if (atkObject != null)
            //{
            //    this.root.transform.LookAt(atkObject);
            //}
            //effectObj.transform.rotation = this.root.transform.rotation;
            //return effectObj;
            return null;
        }
 
        #endregion

    }
}