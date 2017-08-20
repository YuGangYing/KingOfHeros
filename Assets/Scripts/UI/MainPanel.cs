using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Packet;
using Network;
using SLG;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
namespace UI
{
    public class MainPanel : PanelBase
    {
        public const PanelID id = PanelID.MainPanel;
		private UILabel [] lbs;
		private UISprite[] pbs;

        private UILabel labelError;
        private UILabel labelBuildQueue;
        private UILabel labelCollegeQueue;
        UISprite coverSprite;

        QueueProcess m_BuildQueue;
        QueueProcess m_CollegeQueue;

        UIInput m_input;

        GameObject pveBtn;
        GameObject pvpBtn;
        GameObject _settingBtn;

        GameObject m_fightBtn;
        GameObject m_fightBtnEffect;

        GameObject m_shopBtn;
        GameObject m_shopBtnEffect;
        
        GameObject m_sword1;
        GameObject m_sword2;
        GameObject m_sword3;

        GameObject m_sword1Btn;
        GameObject m_sword2Btn;
        GameObject m_sword3Btn;

        GameObject addDefenceBtn;
        GameObject addWorkerBtn;
        GameObject addRedBtn;

        UILabel workerNoLabel;

        public MainPanel()
        {
			lbs = new UILabel[5];
            pbs = new UISprite[2];
        }

        public override string GetResPath()
        {
            return "MainUI.prefab";
        }

		public void SetMainLBs(string [] val)
		{
			for (int i=0; i<Math.Min(lbs.Length, val.Length); i++) {
				if( lbs [i] != null ) {
					lbs [i].text = val [i];
				}
			}
		}

		public void SetMainPBs(float [] val)
		{
			for (int i=0; i<Math.Min(pbs.Length, val.Length); i++) {
				if( pbs [i] != null ) {
					pbs [i].fillAmount = val [i];
				}
			}
		}

        protected override void Initimp(List<GameObject> prefabs)
        {
            PanelTools.FindChild(Root, "shop").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnShopClick));
            PanelTools.FindChild(Root, "mail").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnMailClick));
            PanelTools.FindChild(Root, "hero").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnHeroClick));
            PanelTools.FindChild(Root, "Achievement").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(_OnAchievementClick));
            PanelTools.FindChild(Root, "fight").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnFightClick));
            PanelTools.FindChild(Root, "PVE").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnPVEBtnClick));
            PanelTools.FindChild(Root, "PVP").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnPVPBtnClick));
            PanelTools.FindChild(Root, "ChatBtn").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnChatBtnClick));
            PanelTools.FindChild(Root, "setting").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(_OnSettingClik));
            PanelTools.FindChild(Root, "top").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnDevelopmentClick));
           
//            UIEventListener.Get(PanelTools.FindChild(Root, "shop")).onClick = OnShopClick;
//            UIEventListener.Get(PanelTools.FindChild(Root, "mail")).onClick = OnMailClick;
//            UIEventListener.Get(PanelTools.FindChild(Root, "hero")).onClick = OnHeroClick;
//            UIEventListener.Get(PanelTools.FindChild(Root, "Achievement")).onClick = _OnAchievementClick;
//            UIEventListener.Get(PanelTools.FindChild(Root, "fight")).onClick = OnFightClick;
//            UIEventListener.Get(PanelTools.FindChild(Root, "PVE")).onClick = OnPVEBtnClick;
//            UIEventListener.Get(PanelTools.FindChild(Root, "PVP")).onClick = OnPVPBtnClick;
//            UIEventListener.Get(PanelTools.FindChild(Root, "ChatBtn")).onClick = OnChatBtnClick;
//            UIEventListener.Get(PanelTools.FindChild(Root, "setting")).onClick = _OnSettingClik;
//            UIEventListener.Get(PanelTools.FindChild(Root, "top")).onClick = OnDevelopmentClick;

            addDefenceBtn = PanelTools.FindChild(Root, "addDefenceBtn");
            addWorkerBtn = PanelTools.FindChild(Root, "addWorkerBtn");
            addRedBtn = PanelTools.FindChild(Root, "addRedBtn");

            addDefenceBtn.AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(_OnClickAddDef));
            addWorkerBtn.AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(_OnClickAddWorker));
            addRedBtn.AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(_OnClickAddRed));

//            UIEventListener.Get(addDefenceBtn).onClick = _OnClickAddDef;
//            UIEventListener.Get(addWorkerBtn).onClick = _OnClickAddWorker;
//            UIEventListener.Get(addRedBtn).onClick = _OnClickAddRed;

            workerNoLabel = PanelTools.FindChild(Root, "workerNo").GetComponent<UILabel>();

            coverSprite = PanelTools.FindChild(Root, "cover").GetComponent<UISprite>();
            PanelTools.FindChild(Root, "btn_send").AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnSendMsgClick));
//            UIEventListener.Get(PanelTools.FindChild(Root, "btn_send")).onClick = OnSendMsgClick;
            m_input = PanelTools.Find<UIInput>(Root, "sendMsg/Input");

            labelError = PanelTools.FindChild(Root, "Error").GetComponent<UILabel>();
            labelBuildQueue = PanelTools.FindChild(Root, "BuildQueue").GetComponent<UILabel>();
            labelCollegeQueue = PanelTools.FindChild(Root, "CollegeQueue").GetComponent<UILabel>();

            m_BuildQueue = Root.GetComponent<QueueProcess>();
            if (m_BuildQueue == null) m_BuildQueue = labelBuildQueue.gameObject.AddComponent<QueueProcess>();
			m_BuildQueue.SetQueueInfo(DataMgr.QueueData.QUEUE_TYPE.TYPE_BUILD, this.UpdateBuildQueue);

            m_CollegeQueue = Root.GetComponent<QueueProcess>();
            if (m_CollegeQueue == null) m_CollegeQueue = labelBuildQueue.gameObject.AddComponent<QueueProcess>();
			m_CollegeQueue.SetQueueInfo(DataMgr.QueueData.QUEUE_TYPE.TYPE_TECHNOLOGY, this.UpdateCollegeQueue);

			lbs [0] = PanelTools.FindChild (Root, "powerNo").GetComponent<UILabel> ();
			lbs [1] = PanelTools.FindChild (Root, "goldNo").GetComponent<UILabel> ();
			lbs [2] = PanelTools.FindChild (Root, "stoneNo").GetComponent<UILabel> ();
			lbs [3] = PanelTools.FindChild (Root, "dimNo").GetComponent<UILabel> ();
			lbs [4] = PanelTools.FindChild (Root, "defNo").GetComponent<UILabel> ();

            pbs[0] = PanelTools.FindChild(Root, "goldBar").GetComponent<UISprite>();
            pbs[1] = PanelTools.FindChild(Root, "stoneBar").GetComponent<UISprite>();

			for (int i=0; i<5; i++)
			{
				lbs[i].text = "N/A";
			}

            m_fightBtn = PanelTools.FindChild(Root, "fight");
            m_fightBtnEffect = PanelTools.FindChild(m_fightBtn, "effect");
            m_shopBtn = PanelTools.FindChild(Root, "shop");
            m_shopBtnEffect = PanelTools.FindChild(m_shopBtn, "effect");


            m_sword1 = PanelTools.FindChild(Root, "sword1");
            m_sword2 = PanelTools.FindChild(Root, "sword2");
            m_sword3 = PanelTools.FindChild(Root, "sword3");

            m_sword1Btn = PanelTools.FindChild(m_sword1, "sword1Btn");
            m_sword2Btn = PanelTools.FindChild(m_sword2, "sword2Btn");
            m_sword3Btn = PanelTools.FindChild(m_sword3, "sword3Btn");

//            UIEventListener.Get(m_sword2Btn).onClick = OnPVEBtnClick;
//            UIEventListener.Get(m_sword1Btn).onClick = OnDevelopmentClick;
//            UIEventListener.Get(m_sword3Btn).onClick = OnDevelopmentClick;

            m_sword2Btn.AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnPVEBtnClick));
            m_sword1Btn.AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnDevelopmentClick));
            m_sword3Btn.AddMissingComponent<UIEventTrigger>().onClick.Add(new EventDelegate(OnDevelopmentClick));


            GlobalEventSet.SubscribeEvent(eEventType.RefreshMainUI, id, this.OnRefresh);
            GlobalEventSet.SubscribeEvent(eEventType.ShowError, id, this.OnShowError);
            GlobalEventSet.SubscribeEvent(eEventType.UpdateQueueTime, this.UpdatePanel);

            pveBtn = UISoldierPanel.findChild(Root, "downBarLeft,PVE");
            pvpBtn = UISoldierPanel.findChild(Root, "downBarLeft,PVP");

            _settingBtn = UICardMgr.findChild(Root, "downBarLeft,setting");

            SetVisible(true);
            ShowCover(false);

            InitPlayerData();
        }

		public bool OnRefresh(SLG.EventArgs obj)
		{
            /*Item usx = Game.Sgt.QueryItem(0x8000);

            lbs[1].text = string.Format("{0}", Game.Sgt.QueryItemData(1, usx).Data[0]);
            lbs[2].text = string.Format("{0}", Game.Sgt.QueryItemData(2, usx).Data[0]);
            lbs[3].text = string.Format("{0}", Game.Sgt.QueryItemData(3, usx).Data[0]);*/		

			lbs[1].text = string.Format("{0}", RewardPanel.getCoins().ToString());
			lbs[2].text = string.Format("{0}", RewardPanel.getMagicStones().ToString());
			lbs[3].text = string.Format("{0}", RewardPanel.getDimonds().ToString());
			
			return true;
		}

        public void InitPlayerData()
        {
            lbs[1].text = string.Format("{0}", (long)DataManager.getUserData().Data.coin);
            lbs[2].text = string.Format("{0}", (long)DataManager.getUserData().Data.stone);
            lbs[3].text = string.Format("{0}", (long)DataManager.getUserData().Data.rmb);
        }

        public void UpdateBuildQueue(float fTime)
        {
            if (fTime > 0)
            {
                workerNoLabel.text = "1 / 1";
                labelBuildQueue.text = "建筑冷却时间：" + ((uint)fTime + 1).ToString() + "s";
            }
            else
            {
                workerNoLabel.text = "0 / 1";
                labelBuildQueue.text = "建筑队列空闲";
            }
        }

        public void UpdateCollegeQueue(float fTime)
        {
            if (fTime > 0)
            {
                labelCollegeQueue.text = "科技冷却时间：" + ((uint)fTime + 1).ToString() + "s";
            }
            else
            {
                labelCollegeQueue.text = "科技队列空闲";
            }
        }

        bool UpdatePanel(SLG.EventArgs go)
        {
            m_BuildQueue.UpdateQueueTime();
            m_CollegeQueue.UpdateQueueTime();
            return true;
        }

        public bool OnShowError(SLG.EventArgs obj)
        {
            string strError = (string)obj.m_obj;
            labelError.text = strError;
            labelError.gameObject.SetActive(true);
            return false;
        }

        public override PanelID GetPanelID()
        {
            return id;
        }

        protected void OnMailClick()
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.MailPanel);
            if (panelBase != null)
            {
                panelBase.ToggleVisible();
            }
        }

        protected void OnChatBtnClick()
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.ChatPanel);
            if (panelBase != null)
            {
                panelBase.ToggleVisible();
            }
        }

        protected void OnHeroClick()
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.CardIllustratedListPanel);
            if (panelBase != null)
            {
                panelBase.ToggleVisible();
            }
        }

        protected void OnShopClick()
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.ShopPanel);
            if (panelBase != null)
            {
                panelBase.ToggleVisible();
            }

            ShowBtnEffect("ButtonClick_V2", m_shopBtn);

        }

        protected void OnSendMsgClick()
        {
            if (m_input != null && m_input.value != "")
            {
                string msgStr = m_input.value;

                MSG_CLIENT_TALK msg_struct = new MSG_CLIENT_TALK();
                msg_struct.unTxtAttribute = 0;
                msg_struct.szSender = "1";
                msg_struct.szReceiver = "1";
                msg_struct.szWords = msgStr;

                NetworkMgr.me.getClient().Send(ref msg_struct);

                m_input.value = "";
            }
        }

        protected void OnFightClick()
        {
            Application.LoadLevel("Battlefield_Gobi01");
            return;
            if (m_sword1Btn.active)
            {
                TweenPosition.Begin(m_sword1.gameObject, 0.1f, new Vector3(-405, 30, 0));
                TweenPosition.Begin(m_sword2.gameObject, 0.1f, new Vector3(-377, 27, 0));
                TweenPosition.Begin(m_sword3.gameObject, 0.1f, new Vector3(-358, 14, 0));

                m_sword1Btn.SetActive(false);
                m_sword2Btn.SetActive(false);
                m_sword3Btn.SetActive(false);
            }
            else
            {
                TweenPosition.Begin(m_sword1.gameObject, 0.1f, new Vector3(-405, 60, 0));
                TweenPosition.Begin(m_sword2.gameObject, 0.1f, new Vector3(-362, 50, 0));
                TweenPosition.Begin(m_sword3.gameObject, 0.1f, new Vector3(-330, 28, 0));

                m_sword1Btn.SetActive(true);
                m_sword2Btn.SetActive(true);
                m_sword3Btn.SetActive(true);
                ShowBtnEffect("ButtonClick_V2", m_fightBtn);
            }
        }

        protected void OnPVEBtnClick()
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.BattlefieldPanel);
            if (panelBase != null)
            {
                panelBase.ToggleVisible();
            }
        }

        protected void OnPVPBtnClick()
        {

        }

        protected void _OnAchievementClick()
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.AchievenmentPanel);

            if (panelBase != null)
            {
                panelBase.ToggleVisible();
            }
        }

        public void ShowCover(bool bShow)
        {
            coverSprite.gameObject.SetActive(bShow);
        }

        protected void _OnSettingClik()
        {
            PanelBase panelBase = PanelManage.me.getPanel(PanelID.SettingPanel);

            if (panelBase != null)
            {
                panelBase.ToggleVisible();
            }
        }

        protected void _OnClickAddDef()
        {
//             string strContent = "ARE YOU SURE BUY DEFEND?";
// 
//             MessageBoxMgr.ShowMessageBox("Confirm", strContent, OnAddDefConfirm, null);

            //under crazy development！
            string strContent = "66350009";
            strContent = DataManager.getLanguageMgr().getString(strContent);

            MessageBoxMgr.ShowMessageBoxEvent showMessageBoxEvent = new MessageBoxMgr.ShowMessageBoxEvent();
            showMessageBoxEvent.strTitleText = "King of Heroes (Alpha.)";
            showMessageBoxEvent.strContentText = strContent;
            showMessageBoxEvent.bHidePrevious = false;
            showMessageBoxEvent.btnFlag = MESSBOX_FLAG.MB_CONFIRM;
            showMessageBoxEvent.ContentTextClolor = new Color(1.0f, 248.0f / 255.0f, 185.0f / 255.0f);
            MessageBoxMgr.ShowMessageBox(showMessageBoxEvent);
            
            ShowBtnEffect("ButtonClickCross_V2", addDefenceBtn);

        }

        protected bool OnAddDefConfirm(SLG.EventArgs obj)
        {
            return true;
        }

        protected void _OnClickAddRed()
        {
//             string strContent = "ARE YOU SURE BUY MASONRY?";
// 
//             MessageBoxMgr.ShowMessageBox("Confirm", strContent, OnAddRedConfirm, null);

            //under crazy development！

            string strContent = "66350009";
            strContent = DataManager.getLanguageMgr().getString(strContent);

            MessageBoxMgr.ShowMessageBoxEvent showMessageBoxEvent = new MessageBoxMgr.ShowMessageBoxEvent();
            showMessageBoxEvent.strTitleText = "King of Heroes (Alpha.)";
            showMessageBoxEvent.strContentText = strContent;
            showMessageBoxEvent.bHidePrevious = false;
            showMessageBoxEvent.btnFlag = MESSBOX_FLAG.MB_CONFIRM;
            showMessageBoxEvent.ContentTextClolor = new Color(1.0f, 248.0f / 255.0f, 185.0f / 255.0f);
            MessageBoxMgr.ShowMessageBox(showMessageBoxEvent);

            ShowBtnEffect("ButtonClickCross_V2", addRedBtn);
        }

        protected bool OnAddRedConfirm(SLG.EventArgs obj)
        {
            return true;
        }

        protected void _OnClickAddWorker()
        {
//             string strContent = "ARE YOU SURE BUY QUEUE?";
// 
//             MessageBoxMgr.ShowMessageBox("Confirm", strContent, OnAddWorkerConfirm, null);

            //under crazy development！
            string strContent = "66350009";
            strContent = DataManager.getLanguageMgr().getString(strContent);

            MessageBoxMgr.ShowMessageBoxEvent showMessageBoxEvent = new MessageBoxMgr.ShowMessageBoxEvent();
            showMessageBoxEvent.strTitleText = "King of Heroes (Alpha.)";
            showMessageBoxEvent.strContentText = strContent;
            showMessageBoxEvent.bHidePrevious = false;
            showMessageBoxEvent.btnFlag = MESSBOX_FLAG.MB_CONFIRM;
            showMessageBoxEvent.ContentTextClolor = new Color(1.0f, 248.0f / 255.0f, 185.0f / 255.0f);
            MessageBoxMgr.ShowMessageBox(showMessageBoxEvent);

            ShowBtnEffect("ButtonClickCross_V2", addWorkerBtn);
        }

        protected bool OnAddWorkerConfirm(SLG.EventArgs obj)
        {
            return true;
        }

        GameObject UIeffectCamera = null;
        protected void ShowBtnEffect(string strName, GameObject go)
        {
            BuildInfoPanel bip = PanelManage.me.GetPanel<BuildInfoPanel>(PanelID.BuildInfoPanel);
            bip.SetVisible(false);

            if (UIeffectCamera == null)
            {
                string strCamera = "Prefabs/UI/Effect/Camera";
                GameObject CameraPrefab = DataMgr.ResourceCenter.LoadAsset<GameObject>(strCamera);
                UIeffectCamera = (GameObject)UnityEngine.Object.Instantiate(CameraPrefab);
                UIeffectCamera.name = "UIeffectCamera";
                UIeffectCamera.transform.position = new UnityEngine.Vector3(0, 500, 0);
            }



            UIViewport viewport = UIeffectCamera.GetComponent<UIViewport>();

            GameObject UICamera = UnityEngine.GameObject.Find("UICamera");
            viewport.sourceCamera = UICamera.GetComponent<Camera>();

            GameObject br = PanelTools.FindChild(go, "br");
            GameObject tl = PanelTools.FindChild(go, "tl");
            viewport.topLeft = tl.transform;
            viewport.bottomRight = br.transform;

            string strEffect = "Prefabs/UI/Effect/" + strName;
            GameObject effectPrefab = DataMgr.ResourceCenter.LoadAsset<GameObject>(strEffect);
            if (effectPrefab != null)
            {
                GameObject effect = NGUITools.AddChild(UIeffectCamera, effectPrefab);
                effect.transform.localPosition = new UnityEngine.Vector3(0,0,1);

//                 GameObject effect = GameObject.Instantiate(effectPrefab) as GameObject;
//                 effect.name = strName;
//                 effect.transform.parent = UICamera.transform;
//                 effect.transform.localPosition = new UnityEngine.Vector3(0, 0, 1);
            }
        }

        protected void OnDevelopmentClick()
        {
            string strContent = "66350009";
            strContent = DataManager.getLanguageMgr().getString(strContent);

            MessageBoxMgr.ShowMessageBoxEvent showMessageBoxEvent = new MessageBoxMgr.ShowMessageBoxEvent();
            showMessageBoxEvent.strTitleText = "King of Heroes (Alpha.)";
            showMessageBoxEvent.strContentText = strContent;
            showMessageBoxEvent.bHidePrevious = false;
            showMessageBoxEvent.btnFlag = MESSBOX_FLAG.MB_CONFIRM;
            showMessageBoxEvent.ContentTextClolor = new Color(1.0f, 248.0f / 255.0f, 185.0f / 255.0f);
            MessageBoxMgr.ShowMessageBox(showMessageBoxEvent);
        }

    }
}