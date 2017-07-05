using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestMultiRes
{
    // һ����Դ����
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

        public String url; // ��Դ·��
        public List<Pair<string, System.Type>> assets; // ������Դ

        // ��Դ�������֮��ص�������
        public ResDownLoad.Res.Data m_res; // ���صľ��
    }

    // ��Դ�������
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

        // ��ʼ����
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

        // �õ���ǰ�Ľ���
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

        // ��Դ����֮��Ļص�
        void OneResEnd(ResDownLoad.Res dRes, System.Object p)
        {
            Res res = (Res)(p);

            m_endResMap.Add(res.url, res);
            ++m_resNum;

            // ȫ�����������
            if (m_resNum == m_resList.Count)
            {
                m_fun(m_endResMap, m_param);
            }
        }

        List<Res> m_resList; // ��Դ
        int m_resNum; // ��Դ���ؼ���

        RequestMultiResEnd m_fun; // �ص�����
        System.Object m_param; // ����

        Dictionary<String, Res> m_endResMap = new Dictionary<String, Res>();
    }
}