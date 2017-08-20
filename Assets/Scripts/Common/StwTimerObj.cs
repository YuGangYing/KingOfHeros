using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
public class StwTimeObj : MonoBehaviour
{
    static private StwTimeObj this_obj = null;
    static public StwTimeObj me
    {
        get
        {
            if (this_obj != null)
                return this_obj;

            GameObject obj = new GameObject("StwTimeObj");
            obj.AddComponent<StwTimeObj>();
            return this_obj;
        }
    }

    void Awake()
    {
        if (this_obj != null)
        {
            Destroy(this);
            Debug.LogError("Multi-StwTimeObj");
            return;
        }

        this_obj = this;

        DontDestroyOnLoad(gameObject);

        // 客户端桢数控制在30桢左右
        m_timer_wheel = new stw.timer_wheel(512, 30, "");
        InvokeRepeating("Run", 0.05f, 0.05f);
    }

    void Start()
    {

    }

    public stw.tmr_t register_timer(float interval, float deltaTime, uint num, stw.call_back fun, System.Object p)
    {
        uint time1 = (uint)Mathf.FloorToInt(interval * 1000);
        uint time2 = (uint)Mathf.FloorToInt(deltaTime * 1000);
        return m_timer_wheel.add_timer(time1, time2, num, fun, p);
    }

    void Run()
    {
        // 0.01秒更新一次
        m_timer_wheel.step();
    }

    float m_lastTime = -1;
    float m_deltaTime = 0f;
    bool isStop = false;
//     void Update()
//     {
// //         if (Input.GetKeyDown(KeyCode.Mouse0))
// //             isStop = !isStop;
// // 
// //         if (isStop)
// //         {
// //             return;
// //         }
// 
//         float newTime = Time.realtimeSinceStartup;
//         if (m_lastTime == -1)
//         {
//             m_lastTime = newTime;
//             return;
//         }
// 
//         m_deltaTime += (newTime - m_lastTime);
//         if (m_deltaTime >= 0.033f)
//         {
//             m_deltaTime = 0f;
//             Run();
//         }
// 
//         m_lastTime = newTime;
//     }

    private stw.timer_wheel m_timer_wheel;
}