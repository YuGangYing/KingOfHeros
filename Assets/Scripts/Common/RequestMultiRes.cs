using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestMultiRes
{
    // 一份资源请求
    public class Res
    {
        public Res(String u, List<Pair<string, System.Type>> s)
        {
            url = u;
            assets = s;
        }

        public Res(String u)
        {
            url = u;
        }

        public void add(String n, Type t)
        {
            if (assets == null)
            {
                assets = new List<Pair<string, System.Type>>();
            }

            assets.Add(new Pair<string, System.Type>(n, t));
        }

        public float progress
        {
            get { return m_res.parent.progress; }
        }

        public String url; // 资源路径
        public List<Pair<string, System.Type>> assets; // 附加资源

        // 资源加载完成之后回调的数据
        public ResDownLoad.Res.Data m_res; // 下载的句柄
    }

    // 资源加载完成
    public delegate void RequestMultiResEnd(Dictionary<String, Res> s, System.Object p);

    public class Ress
    {
        public Ress(RequestMultiResEnd f, System.Object p)
        {
            m_fun = f;
            m_param = p;
        }

        public void add(Res res)
        {
            if (m_resList == null)
            {
                m_resList = new List<Res>();
            }

            m_resList.Add(res);
        }

        // 开始下载
        public bool begin()
        {
            if (m_resList == null)
                return false;

            foreach (Res res in m_resList)
            {
                res.m_res = ResDownLoad.me.loadRes(res.url, res.assets.ToArray(), OneResEnd, res);
            }

            return true;
        }

        // 得到当前的进度
        public float progress
        {
            get
            {
                float progress = 0.0f;
                foreach (Res res in m_resList)
                    progress += res.progress;

                return progress / m_resList.Count;
            }
        }

        // 资源下载之后的回调
        void OneResEnd(ResDownLoad.Res dRes, System.Object p)
        {
            Res res = (Res)(p);

            m_endResMap.Add(res.url, res);
            ++m_resNum;

            // 全部下载完成了
            if (m_resNum == m_resList.Count)
            {
                m_fun(m_endResMap, m_param);
            }
        }

        List<Res> m_resList; // 资源
        int m_resNum; // 资源下载计数

        RequestMultiResEnd m_fun; // 回调函数
        System.Object m_param; // 参数

        Dictionary<String, Res> m_endResMap = new Dictionary<String, Res>();
    }
}