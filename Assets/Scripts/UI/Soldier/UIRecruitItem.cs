using System;
using System.Collections.Generic;
using DataMgr;
using UnityEngine;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
#pragma warning disable 0114
namespace UI
{
    //招募ITEM
	public class UIRecruitItem
	{
        protected enum OPER_STEP
        {
            NONE,
        }

        public enum OPER_TYPE
        {
            Recruit, //招募
            UnRecruit //解编
        }

        //士兵类型
        protected ARMY_TYPE m_armyType;
        //操作类型
        protected OPER_TYPE m_operType;
        //操作数量
        protected int m_operCount;
        //
        protected bool m_bPress = false;
        protected float m_fPressTime = 0f;
        protected float m_fPreTime = 0f;//上一次处理时间 

        //操作按钮
        protected UIButton m_btnOper = null;
        protected UILabel m_showCount = null;
        protected UIButton m_btnSprite = null;
        protected UILabel m_lblPerCoin = null;

        protected UIRecruitItem()
        {
        }

        public int operCount
        {
            get { return this.m_operCount; }
        }

        public ARMY_TYPE armyType
        {
            get { return m_armyType; }
        }

        public UISoldierPanel parentPanel
        {
            get;
            set;
        }
        
        public UIRecruitItem(ARMY_TYPE armyType)
        {
            this.m_armyType = armyType;
        }

        public virtual void update()
        {
            if (m_bPress)//按下
            {
                if (Time.time - this.m_fPressTime < 1.5)//1.5秒后进入高速模式
                    return;
                else if (Time.time - this.m_fPressTime < 4.5)//前3秒，10个每秒
                {
                    if (Time.time - m_fPreTime > 0.1f)
                        this.onOper(1);
                }
                else //20个每秒
                {
                    if (Time.time - m_fPreTime > 0.05f)
                        this.onOper(1);
                }
            }
            showCurCount();
            if (m_lblPerCoin != null)
                m_lblPerCoin.text = this.costCoin().ToString();
        }

        public void show(bool flag)
        {
            if (m_btnOper != null)
                m_btnOper.gameObject.active = flag;
            if (m_btnSprite != null)
            {
                if (flag)
                    UIEventListener.Get(m_btnSprite.gameObject).onPress = onPress;
                else
                    UIEventListener.Get(m_btnSprite.gameObject).onPress = null;
            }
        }

        public virtual void setRoot(GameObject root)
        {
            if (root == null)
                return;
            m_lblPerCoin = UISoldierPanel.findChild<UILabel>(root.transform, "coin,Label");
            m_showCount = UISoldierPanel.findChild<UILabel>(root.transform,"operCount");
            m_btnSprite = UISoldierPanel.findChild<UIButton>(root.transform, "btnSprite");
            if(m_btnOper!=null)
                UIEventListener.Get(m_btnOper.gameObject).onClick = onMicOper;
        }

        protected void onMicOper(GameObject go)
        {
            if (this.m_operCount > 0)
                this.m_operCount--;
        }

        void onPress(GameObject go, bool state)
        {
            m_bPress = state;
            m_fPressTime = Time.time;
            //第一次按下直接处理一次
            if (m_bPress)
                this.onOper(1);
        }

        public void clear()
        {
            m_operCount = 0;
            showCurCount();
        }

        protected void showCurCount()
        {
            if (m_showCount != null)
            {
                int nCurCount = this.getHaveCount();
                if (this.m_operType == OPER_TYPE.Recruit)
                    m_showCount.text = nCurCount + " + " + this.m_operCount;
                else
                    m_showCount.text = nCurCount + " - " + this.m_operCount;

                if (m_btnOper != null && m_btnOper.gameObject!=null)
                    m_btnOper.gameObject.SetActive(this.operCount > 0);
            }
        }

        protected virtual void onOper(int count)
        {
            this.m_fPreTime = Time.time;
        }

        //获得已经拥有的数量
        protected int getHaveCount()
        {
            switch (m_armyType)
            {
                case ARMY_TYPE.ARCHER:
                    return DataManager.getUserData().Data.usArmyArcher;
                    break;
                case ARMY_TYPE.CAVALRY:
                    return DataManager.getUserData().Data.usArmyCavalry;
                    break;
                case ARMY_TYPE.PIKEMAN:
                    return DataManager.getUserData().Data.usArmyInfantry;
                    break;
                case ARMY_TYPE.SHIELD:
                    return DataManager.getUserData().Data.usArmyShield;
                    break;
                case ARMY_TYPE.MAGIC:
                    return DataManager.getUserData().Data.usArmySpell;
                    break;
                default:
                    break;
            }
            return 0;
        }

        //计算消耗金币数量
        public int costCoin()
        {
            //当前士兵级别
            TechItem army = userTechInfo.getUserArmyInfo(m_armyType);
            if (army == null)
                return 0;
            ConfigRow row = userTechInfo.getArmyLevelCfg(m_armyType, army.nArmyLevel, army.nStarLevel);
            if (row == null)
                return 0;
            return row.getIntValue(CFG_ARMY_LEVEL.COST_COIN, 0);
        }
	}

    //增加士兵
    public class UIRecruitItemAdd : UIRecruitItem
    {
        public UIRecruitItemAdd(ARMY_TYPE armyType):base(armyType)
        {
            this.m_operType = UIRecruitItem.OPER_TYPE.Recruit;
        }

        public override void setRoot(GameObject root)
        {
            if (root == null)
                return;
            m_btnOper = UISoldierPanel.findChild<UIButton>(root.transform, "btnReduce");
            base.setRoot(root);
        }

        protected override void onOper(int count)
        {
            base.onOper(count);
            if(canRecruit())
                this.m_operCount++;
        }

        bool canRecruit()
        {
            if (this.parentPanel == null)
                return false;
            return parentPanel.canRecruit(this.costCoin());
        }
    }

    //减少士兵
    class UIRecruitItemReduce : UIRecruitItem
    {
        public UIRecruitItemReduce(ARMY_TYPE armyType): base(armyType)
        {
            this.m_operType = UIRecruitItem.OPER_TYPE.UnRecruit;
        }

        public override void setRoot(GameObject root)
        {
            if (root == null)
                return;
            m_btnOper = UISoldierPanel.findChild<UIButton>(root.transform, "btnAdd");
            base.setRoot(root);
        }

        protected override void onOper(int count)
        {
            base.onOper(count);
            if(canReduce())
                this.m_operCount++;
        }

        bool canReduce()
        {
            //判断总数是否为0
            if (getHaveCount() > this.m_operCount)
                return true;
            else
                return false;
        }
    }
}
