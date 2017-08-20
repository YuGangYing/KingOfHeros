using UnityEngine;
using System.Collections;
using DataMgr;
using Packet;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649

namespace UI
{
    //兵种属性页面
    public class UITechArmyInfo
    {
        UILabel m_atk = null;
        UILabel m_def = null;
        UILabel m_hp = null;
        UILabel m_name = null;
        UILabel m_speciality = null;
        UISprite m_stars = null;

        Transform m_model = null;
        Transform m_soldier = null;

        GameObject m_Soldier = null;

        const float starWidth = 0.2f;

		TechItem _curArmyInfo = null;

        //用户信息
        ARMY_TYPE m_armyType;

        public UITechArmyInfo()
        {
        }

        public void init(ARMY_TYPE type)
        {
            m_armyType = type;
        }

        public void update()
        {
			TechItem newItem = userTechInfo.getUserArmyInfo(this.m_armyType);
			if(newItem==null)
				return;
			if(this._curArmyInfo==null || 
                this._curArmyInfo.nLevel != newItem.nLevel ||
                this._curArmyInfo.armyType != newItem.armyType)
			{
				this._curArmyInfo = newItem.Clone();
				showArmyInfo();
			}

            if(m_soldier!=null)
            {
                MessageBoxPanel box = PanelManage.me.GetPanel<MessageBoxPanel>(PanelID.MessageBoxPannel);
                if (box != null)
                {
                    m_soldier.gameObject.SetActive(!box.IsVisible());
                }
            }
        }
		
		void showArmyInfo()
		{	
			ConfigRow armyLevelInfo = userTechInfo.getArmyLevelCfg(_curArmyInfo.armyType,_curArmyInfo.nLevel);
			if (armyLevelInfo == null)
                return;
            if (m_name != null)
				m_name.text = DataManager.getLanguageMgr().getString(armyLevelInfo.getStringValue(CFG_ARMY_LEVEL.NAME_ID));
            if(m_speciality!=null)
				m_speciality.text = DataManager.getLanguageMgr().getString(armyLevelInfo.getStringValue(CFG_ARMY_LEVEL.PROPERTY_ID));
            if (m_atk != null)
                m_atk.text = armyLevelInfo.getStringValue(CFG_ARMY_LEVEL.ATTACK);
            if (m_def != null)
                m_def.text = armyLevelInfo.getStringValue(CFG_ARMY_LEVEL.DEFENCE);
            if (m_hp != null)
                m_hp.text = armyLevelInfo.getStringValue(CFG_ARMY_LEVEL.HP);
            //星级
            if(m_stars!=null)
                m_stars.fillAmount = userTechInfo.getArmyStar(this._curArmyInfo.nLevel) * starWidth;

            //model
            if (m_model != null) //加载模型
            {
				ConfigRow armyType = userTechInfo.getArmyCfg(m_armyType);
				if(armyType==null)
					return;
				UnityEngine.Object preb = ResourceCenter.LoadAsset<GameObject>(ResourceCenter.soldierPrebPath + armyType.getStringValue(CFG_ARMY.FBX_FILE));
                if (preb != null)
                {
                    if (m_Soldier != null)
                        GameObject.Destroy(m_Soldier);
                    m_Soldier = GameObject.Instantiate(preb) as GameObject;
                    if (m_Soldier != null)
                    {
                        setObjLayer(m_Soldier, m_model.gameObject.layer);
                        m_Soldier.transform.parent = m_model;
                        m_Soldier.transform.localPosition = new Vector3(0,-100,0);
                        m_Soldier.transform.localScale = new Vector3(100, 100, 100);
                        m_Soldier.transform.rotation = Quaternion.Euler(0, 180, 0);
                        UIEventListener.Get(m_soldier.gameObject).onDrag = onRotate;
                        Animation animation = m_Soldier.GetComponentInChildren<Animation>();

                        if (animation != null)
                        {
                            animation.CrossFade("StandBy01");
                            animation.wrapMode = WrapMode.Loop;
                        }
                        //m_Soldier.AddComponent<ArmyAnim>();
                    }
                }
            }
        }

        //播放动作

        void onRotate (GameObject go, Vector2 delta)
        {
            if (go == null)
                return;
            float roate_Speed = 10.0f;//旋转速度
            if (m_Soldier!=null)
                m_Soldier.transform.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * roate_Speed, 0f) * m_Soldier.transform.localRotation;
        }

        void setObjLayer(GameObject obj,int nLayer)
        {
            if (obj == null)
                return;
            obj.layer = nLayer;
            int nCount = obj.transform.GetChildCount();
            for (int n = 0; n < nCount; n++)
            {
                Transform child = obj.transform.GetChild(n);
                setObjLayer(child.gameObject, nLayer);
            }
        }

        public void setRoot(Transform pageObj)
        {
            if (pageObj == null)
                return;
            //左边页面-基本属性
            Transform armyData = UISoldierPanel.findChild(pageObj, "attribute");
            if (armyData != null)
            {
                //
                PanelTools.setLabelText(armyData.gameObject, "hp", 29008);
                PanelTools.setLabelText(armyData.gameObject, "atc", 29009);
                PanelTools.setLabelText(armyData.gameObject, "def", 29010);
                PanelTools.setLabelText(armyData.gameObject, "speciality", 29015);
                m_name = UISoldierPanel.setLabelText(armyData, "name", "name");
                m_atk = UISoldierPanel.setLabelText(armyData, "atcNo", "0");
                m_def = UISoldierPanel.setLabelText(armyData, "defNo", "0");
                m_hp = UISoldierPanel.setLabelText(armyData, "hpNo", "0");
                m_speciality = UISoldierPanel.setLabelText(armyData, "specialityNo", "None");
                //星级
                Transform stars = UISoldierPanel.findChild(armyData, "stars");
                if (stars != null)
                    m_stars = stars.gameObject.GetComponent<UISprite>();
            }
            m_soldier = UISoldierPanel.findChild(pageObj.transform, "Soldier");
            m_model = UISoldierPanel.findChild(pageObj.transform, "Soldier,model");
        }
    }
}