using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineupReckon
{
    // 矩形阵列,总数，相隔距离
    static public Vector3[] RectReckon(int totals, int xnum, float disX, float disZ)
    {
        Vector3[] posList = new Vector3[totals];

        // X轴布兵的数量
        int numX = xnum <= 0 ? Mathf.FloorToInt(Mathf.Sqrt(totals)) + 1 : xnum;
        int tx = 0;

        Vector3 newPos = Vector3.zero;
        float X = -numX * disX * 0.5f + disX * 0.5f; // 起始位置
        float Z = -disZ;
        for (int i = 0; i < totals; ++i)
        {
            newPos.x = X + tx * disX;
            newPos.y = 0f;
            newPos.z = Z;
            posList[i] = newPos;

            tx++;
            if (tx >= numX)
            {
                tx = 0;
                Z -= disZ;
            }
        }

        return posList;
    }

    // 格式形
    // A A A A A A
    //  A A A A A A 
    // A A A A A A 
    // ....
    static public Vector3[] GridReckon(int totals, int xnum, float disX, float disZ)
    {
        Vector3[] posList = new Vector3[totals];

        // X轴布兵的数量
        int numX = xnum <= 0 ? Mathf.FloorToInt(Mathf.Sqrt(totals)) + 1 : xnum;
        int tx = 0;

        Vector3 newPos = Vector3.zero;
        float X = -numX * disX * 0.5f + disX * 0.5f; // 起始位置
        float Z = -disZ;
        float offX = 0f;
        for (int i = 0; i < totals; ++i)
        {
            newPos.x = X + tx * disX + offX;
            newPos.y = 0f;
            newPos.z = Z;
            posList[i] = newPos;

            tx++;
            if (tx >= numX)
            {
                tx = 0;
                Z -= disZ;

                if (offX == 0f)
                    offX = disX;
                else
                    offX = 0f;
            }
        }

        return posList;
    }

    private static GameObject obj = null;

    static GameObject GetTempObj()
    {
        if (obj == null)
        {
            obj = new GameObject();
            obj.transform.position = Vector3.zero;
        }

        return obj;
    }

    public static Vector3 QuaternionToDirection(Quaternion q)
    {
        return Matrix4x4.TRS(Vector3.zero, q, Vector3.one).MultiplyVector(Vector3.forward);
    }

    // 圆形
    // 总数，半径，个数
    static public Vector3[] RoundReckon(int total, float radius, float angle)
    {
        GameObject o = GetTempObj();
        Vector3[] posList = new Vector3[total];
        for (int i = 0; i < total; ++i)
        {
            o.transform.rotation = Quaternion.Euler(new Vector3(0f, angle * i, 0f));
            posList[i] = o.transform.forward * radius;
        }

        return posList;
    }
}