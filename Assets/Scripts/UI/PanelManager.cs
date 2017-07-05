using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public abstract class PanelFactoryBase
    {
        public abstract PanelBase create();
    }

    public class PanelFactory<T> : PanelFactoryBase where T : PanelBase, new()
    {
        public PanelFactory()
        {

        }

        public override PanelBase create()
        {
            return new T();
        }
    }

    public delegate void PanelInitEnd(PanelBase panel, object user_data);

    public class PanelManage
    {
        static PanelManage this_obj = null;
        static public PanelManage me
        {
            get
            {
                if (this_obj == null)
                {
                    PanelManage obj = new PanelManage();
                    obj.Init();
                    this_obj = obj;
                }

                return this_obj;
            }
        }

        static public void Release()
        {
            if (this_obj == null)
                return;
            if (this_obj.m_panelList != null)
            {
                foreach (KeyValuePair<PanelID,PanelBase> panel in this_obj.m_panelList)
                {
                    if(panel.Value!=null)
                        panel.Value.Release();
                }
                this_obj.m_panelList.Clear();
            }
            if(this_obj.m_panelFactory!=null)
                this_obj.m_panelFactory.Clear();
            this_obj.m_root = null;
            this_obj = null;
        }

        public void update()
        {
            if (m_panelList != null)
            {
                foreach (KeyValuePair<PanelID, PanelBase> panel in this_obj.m_panelList)
                {
                    if (panel.Value != null)
                        panel.Value.update();
                }
            }
        }

        public PanelBase getPanel(PanelID id)
        {
            Debug.Log("getPanel");
            PanelBase panel = null;
            if (m_panelList.TryGetValue(id, out panel))
                return panel;

            List<string> srcList = this.getResource(id);
            if (srcList == null || srcList.Count == 0)
                return null;

            List<GameObject> objList = new List<GameObject>();
            foreach(string item in srcList)
            {
                GameObject obj = DataMgr.ResourceCenter.LoadAsset<GameObject>(DataMgr.ResourceCenter.panelPrebPath + item);
                if (obj != null)
                    objList.Add(obj);
            }
            return createPanel(id, objList);
        }

        public T GetPanel<T>(PanelID id) where T : PanelBase
        {
            return (T)getPanel(id);
        }

        public GameObject getRoot()
        {
            return m_root;
        }

        public void AddPanelFactory(PanelID id, PanelFactoryBase factory)
        {
            if (m_panelFactory.ContainsKey(id) == true)
            {
                Debug.Log("panel id:" + id + "no factory!");
                return;
            }

            m_panelFactory.Add(id, factory);
        }

        public PanelBase createPanel(PanelID id, List<GameObject> prefabs)
        {
            if (m_loading.Contains(id) == true)
                return null;
            if (prefabs == null || prefabs.Count < 1)
                return null;

            PanelFactoryBase factory = null;
            if (m_panelFactory.TryGetValue(id, out factory) == false)
            {
                Logger.LogError(id.ToString() + " factory null!");
                throw new Exception(id.ToString() + "factory null!");
            }

            PanelBase panelbase = factory.create();
            panelbase.Init(prefabs);
 
            m_panelList[id] = panelbase;
            if (id == PanelID.MainPanel)
            {
                mainPanel = (MainPanel)panelbase;
            }
            return panelbase;
        }

        protected GameObject m_root;

        private Dictionary<PanelID, PanelBase> m_panelList = new Dictionary<PanelID,PanelBase>();

        private Dictionary<PanelID, PanelFactoryBase> m_panelFactory = new Dictionary<PanelID, PanelFactoryBase>();

        private List<PanelID> m_loading = new List<PanelID>(); // 加载当中的面板

        MainPanel mainPanel;
        PanelBase openedPanel;

        public void ChangeOpenedPanel(PanelBase panel)
        {
            if (openedPanel == panel || panel == null)
            {
                mainPanel.ShowCover(false);
                openedPanel = null;
                return;
            }

            if (openedPanel != null)
            {
                if(openedPanel!=mainPanel)
                    openedPanel.SetVisible(false);
            }

            openedPanel = panel;
            mainPanel.ShowCover(true);
            SLG.GlobalEventSet.FireEvent(SLG.eEventType.OpenedPanel, null);
        }

        public bool IsHaveShowPanel()
        {
            if (openedPanel != null)
            {
                if (openedPanel == mainPanel)
                    return false;
                return true;
            }

            return false;
        }

        // 初始化
        public bool Init()
        {
            m_panelList = new Dictionary<PanelID, PanelBase>();
            m_root = GameObject.FindObjectOfType<UICamera>().gameObject;
            return true;
        }

        public void addResource(PanelID id, string strName)
        {
            List<string> objList = getResource(id);
            if (objList == null)
            {
                objList = new List<string>();
                this.m_panelResource.Add(id, objList);
            }
            objList.Add(strName);
        }

        public List<string> getResource(PanelID id)
        {
            List<string> objList = null;
            if (!this.m_panelResource.TryGetValue(id, out objList))
                return null;
            return objList;
        }
        Dictionary<PanelID, List<string>> m_panelResource = new Dictionary<PanelID, List<string>>();
    }
}