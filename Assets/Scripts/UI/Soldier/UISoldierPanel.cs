using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
namespace UI
{
    public class UISoldierPanel:PanelBase
    {
        const PanelID id = PanelID.SoldierRecruitPanel;
        //招募队列
        List<UIRecruitItem> _recruitList = new List<UIRecruitItem>();
        //解编队列
        List<UIRecruitItem> _unrecruitList = new List<UIRecruitItem>();

        UIRecruitItem.OPER_TYPE _operType;

        //
        UIButton m_btnRecruit = null;
        UIButton m_btnUnRecruit = null;
        UILabel m_lblCapacity = null;
        UILabel m_lblWarn = null;
        UILabel m_lblCoin = null;
        UILabel m_lblLv = null;

        public UISoldierPanel()
        {
            _recruitList.Capacity = 5;
            _unrecruitList.Capacity = 5;
        }

        protected override void Initimp(List<GameObject> prefabs)
        {
            if(prefabs == null)
                return;

            PanelTools.setLabelText(Root, "title/name", 19000);
            Transform btnList = findChild(Root.transform,"btnList");
            if (btnList != null)
            {
                setBtnFunc(btnList, "btnClose",onClose);
                /*UIButton btnConfirm = */setBtnFunc(btnList, "btnConfirm", onConfirm);
                m_btnRecruit = setBtnFunc(btnList, "btnCruit", onRecruit);
                m_btnUnRecruit = setBtnFunc(btnList, "btnDismiss", onUnRecruit);

                if(m_btnRecruit!=null)
                    PanelTools.setLabelText(m_btnRecruit.gameObject, "Label", 19002);
                if (m_btnUnRecruit != null)
                    PanelTools.setLabelText(m_btnUnRecruit.gameObject, "Label", 19003);
            }

            m_lblLv = findChild<UILabel>(Root.transform,"title/level");

            Transform lblList = findChild(Root.transform, "lblList");
            if (lblList != null)
            {
                m_lblCapacity = findChild<UILabel>(lblList, "spaceNo");
                m_lblWarn = findChild<UILabel>(lblList, "warn");
                m_lblCoin = findChild<UILabel>(lblList, "coin");
                PanelTools.setLabelText(lblList.gameObject, "space", 19001);
            }

            Transform Soldiers = findChild(Root.transform, "Soldiers");
            if (Soldiers == null)
                return;

            addItem(Soldiers, "archer", ARMY_TYPE.ARCHER);
            addItem(Soldiers, "cavalry", ARMY_TYPE.CAVALRY);
            addItem(Soldiers, "pike", ARMY_TYPE.PIKEMAN);
            addItem(Soldiers, "sheild", ARMY_TYPE.SHIELD);
            addItem(Soldiers, "magic", ARMY_TYPE.MAGIC);

            showUnRecruit(false);
            showRecruit(true);
        }

        void addItem(Transform Soldiers, string strName, ARMY_TYPE type)
        {
            if (Soldiers == null)
                return;
            Transform soldier = findChild(Soldiers, strName);
            UIRecruitItem item1 = new UIRecruitItemAdd(type);
            item1.parentPanel = this;
            item1.setRoot(soldier.gameObject);
            _recruitList.Add(item1);
            UIRecruitItem item2 = new UIRecruitItemReduce(type);
            
            item2.setRoot(soldier.gameObject);
            item2.parentPanel = this;
            _unrecruitList.Add(item2);
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        public override string GetResPath()
        {
            return "Soldier.prefab";
        }
        
        protected override void onShow()
        {
            showUnRecruit(false);
            showRecruit(true);
            _operType = UIRecruitItem.OPER_TYPE.Recruit;
            base.onShow();
        }

        void onClose(GameObject obj)
        {
            showUnRecruit(false);
            showRecruit(false);
            this.SetVisible(false);
        }

        void onRecruit(GameObject obj)
        {
            onCloseUnRecruit(true);
        }

        void onUnRecruit(GameObject obj)
        {
            onCloseRecruit(true);
        }

        void onConfirm(GameObject obj)
        {
            List<RecruitData> _list = new List<RecruitData>();
            if (this._operType == UIRecruitItem.OPER_TYPE.Recruit)//招募时
            {
                foreach (UIRecruitItem item in _recruitList)
                {
                    if (item.operCount > 0)
                    {
                        RecruitData data = new RecruitData();
                        data.armyType = userTechInfo.getArmyTypeId(item.armyType);
                        data.count = item.operCount;
                        _list.Add(data);
                        item.clear();
                    }
                }

                if (_list.Count > 0)
                    DataManager.getSoldierData().sendRecruit(_list.ToArray());
            }
            else //解编时
            {
                foreach (UIRecruitItem item in _unrecruitList)
                {
                    if (item.operCount > 0)
                    {
                        RecruitData data = new RecruitData();
                        data.armyType = userTechInfo.getArmyTypeId(item.armyType);
                        data.count = item.operCount;
                        _list.Add(data);
                        item.clear();
                    }
                }
                if (_list.Count > 0)
                    DataManager.getSoldierData().sendUnRecruit(_list.ToArray());
            }
        }

        //显示招募
        void showRecruit(bool bFlag)
        {
            foreach (UIRecruitItem item in _recruitList)
            {
                item.clear();
                item.show(bFlag);
            }
            if(bFlag)
                this._operType = UIRecruitItem.OPER_TYPE.Recruit;
        }

        //显示解编
        void showUnRecruit(bool bFlag)
        {
            foreach (UIRecruitItem item in _unrecruitList)
            {
                item.clear();
                item.show(bFlag);
            }
            if (bFlag)
                this._operType = UIRecruitItem.OPER_TYPE.UnRecruit;
        }

        //关闭
        void onCloseRecruit(bool bOpenUnRecruit)
        {
            if (this._operType == UIRecruitItem.OPER_TYPE.Recruit)
            {
                bool bUpdate = false;
                foreach(UIRecruitItem item in _recruitList)
                {
                    if (item.operCount > 0)
                    {
                        bUpdate = true;
                        break;
                    }
                }

                if (bUpdate)
                {
                    string title = DataManager.getLanguageMgr().getString(19010);
                    string message = DataManager.getLanguageMgr().getString(19011);
                    MessageBoxMgr.ShowMessageBox(title, message, onCancelRecruit,new SLG.EventArgs(bOpenUnRecruit), false);
                }
                else
                {
                    showRecruit(false);
                    showUnRecruit(bOpenUnRecruit);
                }
            }
        }

        //检测是否还可以招募
        public bool canRecruit(int cost)
        {
            int nCostCoin = 0;
            //数量判断
            int nCapacity = getCapacity();
            int nCurCount = getCurArmyCount();
            int totalRecruit = 0;
            foreach (UIRecruitItem item in _recruitList)
            {
                totalRecruit += item.operCount;
                nCostCoin += item.operCount * item.costCoin();
            }
            if (nCapacity <= (nCurCount + totalRecruit))
                return false;
            //金币判断
            int nCurCoin = (int)DataManager.getUserData().Data.coin;
            if (nCurCoin < (nCostCoin + cost))
                return false;
            return true;
        }

        public bool onCancelRecruit(SLG.EventArgs obj)
        {
            showRecruit(false);
            if (obj != null)
            {
                bool bFlag = (bool)obj.m_obj;
                showUnRecruit(bFlag);
            }
            return true;
        }

        public void onCloseUnRecruit(bool bOpenRecruit)
        {
            if (this._operType == UIRecruitItem.OPER_TYPE.UnRecruit)
            {
                bool bUpdate = false;
                foreach (UIRecruitItem item in _unrecruitList)
                {
                    if (item.operCount > 0)
                    {
                        bUpdate = true;
                        break;
                    }
                }

                if (bUpdate)
                {
                    string title = DataManager.getLanguageMgr().getString(19010);
                    string message = DataManager.getLanguageMgr().getString(19012);
                    MessageBoxMgr.ShowMessageBox(title, message, onCancelUnRecruit, new SLG.EventArgs(bOpenRecruit), false);
                }
                else
                {
                    showUnRecruit(false);
                    showRecruit(bOpenRecruit);
                }
            }
        }

        public bool onCancelUnRecruit(SLG.EventArgs obj)
        {
            showUnRecruit(false);
            if (obj != null)
            {
                bool bFlag = (bool)obj.m_obj;
                showRecruit(bFlag);
            }
            return true;
        }

        //实时显示货币情况
        void showMoney()
        {
            string strWarn = "";
            string strCoin = "";
            if (this._operType == UIRecruitItem.OPER_TYPE.Recruit)
            {
                strWarn = DataManager.getLanguageMgr().getString(19005);
                strCoin = DataManager.getLanguageMgr().getString(19006);
                int total = 0;
                foreach (UIRecruitItemAdd item in _recruitList)
                    total += (item.operCount * item.costCoin());
                strCoin = string.Format(strCoin, DataManager.getUserData().Data.coin, total);
            }
            else
            {
                strWarn = DataManager.getLanguageMgr().getString(19007);
                strCoin = DataManager.getLanguageMgr().getString(19008);

                int total = 0;
                foreach (UIRecruitItem item in _unrecruitList)
                    total += (item.operCount * item.costCoin()/2);

                strCoin = string.Format(strCoin, total);
            }

            if (m_lblCoin != null)
                m_lblCoin.text = strCoin;
            if (m_lblWarn != null)
                m_lblWarn.text = strWarn;
        }

        void showCapacity()
        {
            if (m_lblCapacity == null)
                return;
            if (m_lblLv)
                m_lblLv.text = "LV " + (int)DataManager.getBuildData().GetBuildLev(BuildType.BARRACKS);
            string strCapacity = DataManager.getLanguageMgr().getString(19004);
            m_lblCapacity.text = string.Format(strCapacity, getCurArmyCount(), getCapacity());
        }

        public override void update()
        {
            base.update();
            if (this._operType == UIRecruitItem.OPER_TYPE.Recruit)
            {
                foreach (UIRecruitItem item in _recruitList)
                    item.update();
            }
            else
            {
                foreach (UIRecruitItem item in _unrecruitList)
                    item.update();
            }
            showMoney();
            showCapacity();
        }

        protected override void ReleaseImp()
        {
            base.ReleaseImp();
            _recruitList.Clear();
            _recruitList = null;
            _unrecruitList.Clear();
            _unrecruitList = null;
        }


        public static int getCurArmyCount()
        {
            return (int)DataManager.getUserData().Data.usArmyArcher +
                       (int)DataManager.getUserData().Data.usArmyCavalry +
                       (int)DataManager.getUserData().Data.usArmyInfantry +
                       (int)DataManager.getUserData().Data.usArmySpell +
                       (int)DataManager.getUserData().Data.usArmyShield;
        }

        public static int getCapacity()
        {
            //训练场等级
            int nLv = (int)DataManager.getBuildData().GetBuildLev(BuildType.BARRACKS);
            //
            ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_BUILDING_LEVEL);
            if (config == null)
                return 0;
            ConfigRow row = config.getRow(CFG_BUILDING_LEVEL.BUIDING_TYPEID, (int)BuildType.BARRACKS,
                                          CFG_BUILDING_LEVEL.LEVEL, nLv);
            if (row == null)
                return 0;
            return row.getIntValue(CFG_BUILDING_LEVEL.DATA1,0);
        }

        /*以下为和本功能无关的公共函数*/
        public static GameObject findChild(GameObject tfParent, string strName)
        {
            if(tfParent==null)
                return null;
            Transform obj = findChild(tfParent.transform, strName);
            if (obj != null)
                return obj.gameObject;
            return null;
        }

        public static Transform findChild(Transform tfParent, string strName)
        {
            if (strName == "" || tfParent == null)
                return null;

            string strTag = "";
            int nIndex = strName.IndexOf(",");
            if (nIndex != -1)
            {
                strTag = strName.Substring(0, nIndex);
                strName = strName.Substring(nIndex + 1);
            }
            else
            {
                strTag = strName;
            }

            Transform tfObject = tfParent.FindChild(strTag);

            int nGroupIndex = strName.IndexOf("militaryGroup");

            if (tfObject == null)
            {
                Transform dragPanelT = tfParent.FindChild("dragPanel");
                return findChild(dragPanelT, strName);
            }

            if (strTag == strName && nIndex == -1)
                return tfObject;
            else
                return findChild(tfObject, strName);
        }

        public static UILabel setLabelText(Transform tfParent, string strName, string strText)
        {
            Transform temp = findChild(tfParent,strName);
            if (temp != null)
            {
                UILabel lable = temp.gameObject.GetComponent<UILabel>();
                if (lable != null)
                {
                    lable.text = strText;
                    return lable;
                }
            }
            return null;
        }

        public delegate void callFunc();

        public static T findChild<T>(Transform tfParent, string strName) where T : MonoBehaviour
        {
            Transform obj = findChild(tfParent,strName);
            if (obj == null)
                return null;
            T objCom = obj.gameObject.GetComponent<T>();
            return objCom;
        }

        public static UIButton setBtnFunc(Transform tfParent, string strName, UIEventListener.VoidDelegate func)
        {
            Transform temp = findChild(tfParent, strName);
            if (temp != null)
            {
                UIButton btn = temp.gameObject.GetComponent<UIButton>();
                if (btn != null)
                {
                    UIEventListener.Get(btn.gameObject).onClick = func;
                    return btn;
                }
            }
            return null;
        }
    }
}