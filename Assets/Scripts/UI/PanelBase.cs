using System;
using System.Collections.Generic;
using UnityEngine;
using SLG;

namespace UI
{
    public abstract class PanelBase 
    {
        const PanelID id = PanelID.Null;

        public PanelBase()
        {

        }

        public GameObject Root { get; protected set; }

        static string BasePath
        {
            get { return "file://" + Application.dataPath + "/Data/ArtResources/UI/"; }
        }

        public abstract string GetResPath();

        public void Init(List<GameObject> prefabs)
        {
            if (prefabs[0] == null)
            {
                Debug.Log("prefabs[0] = null");
                return;
            }

            Root = (GameObject)GameObject.Instantiate(prefabs[0]);

            Root.transform.parent = PanelManage.me.getRoot().transform;
            Root.transform.localScale = Vector3.one;
            Root.transform.localPosition = prefabs[0].transform.localPosition;
            Initimp(prefabs);

            if (m_clostBtn != null)
            {
                UIEventListener.Get(m_clostBtn).onClick = OnCloseBtnClick;
            }

            isInit = true;
        }

        protected GameObject m_clostBtn = null;

        public void Init(PanelInitEnd fun, System.Object user_data)
        {
            // ������Դ
            //             string url = BasePath + GetResPath();
            //             ResDownLoad.me.loadRes(url, ResLoadEnd, new System.Object[2] { fun, user_data });

            UnityEngine.Object t = UnityEditor.AssetDatabase.LoadAssetAtPath("Data/ArtResources/UI/BuildListPanel", typeof(GameObject));
            Root = (GameObject)GameObject.Instantiate(t);

            Root.transform.parent = PanelManage.me.getRoot().transform;

            Initimp(null);

            if (m_clostBtn != null)
            {
                UIEventListener.Get(m_clostBtn).onClick = OnCloseBtnClick;
            }

            isInit = true;
        }

        protected void OnCloseBtnClick(GameObject go)
        {
            SetVisible(false);
        }

        public void ResLoadEnd(ResDownLoad.Res res, System.Object param)
        {
            // ��Դ��������
            if (res.www.error != null)
            {
                Debug.Log(string.Format("Panel Init Error! url:{0} error:{1}", res.url, res.www.error));
                return;
            }

            Root = (GameObject)GameObject.Instantiate(res.www.assetBundle.mainAsset);
            Root.transform.parent = PanelManage.me.getRoot().transform;

            Initimp(null);

            SetVisible(false);
        }

        public abstract PanelID GetPanelID();

        protected abstract void Initimp(List<GameObject> prefabs);

        public void Release()
        {
            ReleaseImp();
        }

        protected virtual void ReleaseImp()
        {

        }

        public virtual void SetVisible(bool value)
        {
            if (Root == null)
                return;
            if (Root.activeSelf == value)
                return;

            Root.SetActive(value);
            if (value)
            {
                onShow();
            }
            else
            {
                onHide();
            }

            GlobalEventSet.FireEvent((value ? eEventType.PanelShow : eEventType.PanelHide), GetPanelID(), new SLG.EventArgs(this));
        }

        protected bool isInit = false;
        protected virtual void onShow()
        {
            if (isInit)
            {
                if(this.GetPanelID()!= PanelID.MainPanel)
                   AudioCenter.me.play(AudioMgr.AudioName.UI_OPEN);
                PanelManage.me.ChangeOpenedPanel(this);
            }
        }

        protected virtual void onHide()
        {
            if (isInit)
            {
                if(this.GetPanelID()!= PanelID.MessageBoxPannel)//messagebox���Լ��ṩ�˲���
                    AudioCenter.me.play(AudioMgr.AudioName.UI_CLOSE);
                PanelManage.me.ChangeOpenedPanel(null);
            }
        }

        public void ToggleVisible()
        {
            SetVisible(!IsVisible());
        }

        public bool IsVisible()
        {
            return Root.activeSelf;
        }

        public virtual void update()
        {
        }
    }
}