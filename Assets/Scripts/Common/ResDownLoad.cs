using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResDownLoad : SingletonMonoBehaviour<ResDownLoad>
{
    // 下载结束回调
    public delegate void ResDownLoadEnd(Res res, System.Object param);

    // Update is called once per frame
    void Update()
    {
        downloadRes();
    }

    public class Res
    {
        public WWW www;
        public string url;
        public float lasttime; // 最后使用的时间
        public Dictionary<string, Pair<AssetBundleRequest, Type>> assets; // 附加的资源
        public float progress; // 进度
        public class Data
        {
            public Data(ResDownLoadEnd f, Pair<string, Type>[] a, System.Object p, Res res)
            {
                fun = f;
                assets = a;
                param = p;
                bcannel = false;
                m_parent = res;
            }

            private Res m_parent;
            public Res parent { get { return m_parent; } }
            public bool bcannel; // 是否取消掉了
            public ResDownLoadEnd fun;
            public Pair<String, Type>[] assets;
            public System.Object param;
        }

        public List<Data> m_dataList = new List<Data>();
        public bool isDone()
        {
            if (progress >= 1.0f)
                return true;

            return false;
        }

        public void update()
        {
            // 还没有完全加载完成
            if (progress < 1.0f)
            {
                float res_num = 1.0f + (assets == null ? 0 : assets.Count);
                progress = www.progress;
                if (www.isDone == true)
                {
                    if (assets == null)
                        progress = 1.0f;
                    else
                    {
                        if (www.error != null)
                        {
                            Debug.Log("res:" + url + " error:" + www.error);
                        }

                        foreach (KeyValuePair<String, Pair<AssetBundleRequest, Type>> itor in assets)
                        {
                            if (itor.Value.first == null)
                            {
                                if (www.assetBundle.Contains(itor.Key))
                                {
                                    itor.Value.first = www.assetBundle.LoadAssetAsync(itor.Key, itor.Value.second);
                                }
                                else
                                {
                                    Debug.Log("LoadAsync:" + url + "not find res :" + itor.Key);
                                }

                                progress += 1.0f;
                            }
                            else
                            {
                                progress += itor.Value.first.progress;
                            }
                        }
                    }
                }

                progress /= res_num;
            }
        }

        public AssetBundleRequest getAsset(String name)
        {
            Pair<AssetBundleRequest, Type> result = null;
            if (assets.TryGetValue(name, out result) == false)
                return null;

            return result.first;
        }
    }

    Dictionary<string, Res> m_resList;
    protected override void Init()
    {
        m_resList = new Dictionary<string, Res>();
    }

    Res getRes(string url, bool bcreate)
    {
        Res res = null;
        if (m_resList.TryGetValue(url, out res))
            return res;

        if (bcreate)
        {
            res = new Res();
            res.www = new WWW(url);
            res.url = url;
            res.lasttime = 0;

            m_resList.Add(url, res);
        }

        return res;
    }

    public Res.Data loadRes(string url, ResDownLoadEnd fun, System.Object p)
    {
        return loadRes(url, null, fun, p);
    }

    public Res.Data loadRes(string url, Pair<string, Type>[] a, ResDownLoadEnd fun, System.Object p)
    {
        Res res = getRes(url, true);
        Res.Data data = new Res.Data(fun, a, p, res);
        res.m_dataList.Add(data);
        if (a != null && a.Length != 0)
        {
            if (res.assets == null)
                res.assets = new Dictionary<string, Pair<AssetBundleRequest, Type>>();
            for (int i = 0; i < a.Length; ++i)
            {
                if (res.assets.ContainsKey(a[i].first) == false)
                {
                    // 没有包含此资源，则添加下载
                    res.assets.Add(a[i].first, new Pair<AssetBundleRequest, Type>(null, a[i].second));
                }
            }
        }

        return data;
    }

    static float Detal_Delete_Time
    {
        get { return 60f; }
    }

    // 下载资源
    void downloadRes()
    {
        if (m_resList.Count == 0)
            return;

        float cur_time = Time.realtimeSinceStartup;
        Res[] loop_list = new Res[m_resList.Count];
        int l = 0;
        foreach (KeyValuePair<String, Res> itor in m_resList)
        {
            loop_list[l] = itor.Value;
            ++l;
        }

        for (int len = 0; len < loop_list.Length; ++len)
        {
            Res value = loop_list[len];
            if (value.isDone()) // 资源是否已下载完成了
            {
                if (value.m_dataList.Count == 0)
                {
                    if (cur_time - value.lasttime >= Detal_Delete_Time)
                    {
                        //Debug.Log("res delete:" + value.url);

                        // 资源已经没有在下载的，并且在10分钟之间，没有再次请求过此资源，可以卸载掉了
                        if (value.www.error == null)
                        {
                            if (value.www.assetBundle != null)
                            {
                                value.www.assetBundle.Unload(false);
                            }
                            m_resList.Remove(value.url);
                        }
                    }
                }
                else if (value.m_dataList.Count != 0)
                {
                    // 资源回调
                    for (int i = 0; i < value.m_dataList.Count; ++i)
                    {
                        Res.Data data = value.m_dataList[i];
                        if (data.bcannel == false && data.fun != null)
                        {
                            data.fun(value, data.param);
                        }
                    }

                    value.m_dataList.Clear();
                    value.lasttime = cur_time;
                }
            }
            else
            {
                value.update();
            }
        }
    }
}