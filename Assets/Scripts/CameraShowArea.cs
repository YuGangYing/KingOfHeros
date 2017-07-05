using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLG;
//Line End
// 相机显示区域
[ExecuteInEditMode]
public class CameraShowArea : MonoBehaviour
{
    public static CameraShowArea Instance;
    static public string terrainLayrerName = "moveTerrain";

    void Awake()
    {
        Instance = this;
    }

    public Camera Camera; // 相机
    public bool IsSet = false;
//    public float currentDistance;

    public Vector3 CameraPosition;
    public Quaternion CameraQuaternion;
    public Vector3[] AreaPolyPoints = new Vector3[4];
//     public float minDistance = 30f;
//     public float maxDistance = 100f;
    public float[] Angles = new float[4]; // 多边形的一个内角

    // 中心位置
    public Vector3 CenterPosition;

    public float currentDisitance;
    public bool isReset = false;

    public void OnDrawGizmos()
    {
        if (Camera)
        {
            Vector3[] pos = SLG.Util.GetCameraScreenTerrain(Camera,terrainLayrerName);
            Gizmos.DrawLine(pos[0], pos[1]);
            Gizmos.DrawLine(pos[1], pos[2]);
            Gizmos.DrawLine(pos[2], pos[3]);
            Gizmos.DrawLine(pos[3], pos[0]);
            if (IsSet == true)
            {
                IsSet = false;
                AreaPolyPoints = pos;
                
                CameraPosition = Camera.transform.position;
                CameraQuaternion = Camera.transform.rotation;

                if (Angles == null || Angles.Length != 4)
                    Angles = new float[4];

                Angles[0] = Mathf.Acos(Vector3.Dot((pos[0] - pos[1]).normalized, (pos[0] - pos[3]).normalized)) * Mathf.Rad2Deg;
                Angles[1] = Mathf.Acos(Vector3.Dot((pos[1] - pos[0]).normalized, (pos[1] - pos[2]).normalized)) * Mathf.Rad2Deg;
                Angles[2] = Mathf.Acos(Vector3.Dot((pos[2] - pos[1]).normalized, (pos[2] - pos[3]).normalized)) * Mathf.Rad2Deg;
                Angles[3] = Mathf.Acos(Vector3.Dot((pos[3] - pos[2]).normalized, (pos[3] - pos[0]).normalized)) * Mathf.Rad2Deg;

                CenterPosition = RPGCamera.Instance.Target.position;
//                 ConsoleSelf.me.addText("CosAngle:{0}", Angle * Mathf.Rad2Deg);
//                 SinAngle = Vector3.Dot(pos[0] - pos[1], pos[0] - pos[3]);
            }

            Color color = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(AreaPolyPoints[0], AreaPolyPoints[1]);
            Gizmos.DrawLine(AreaPolyPoints[1], AreaPolyPoints[2]);
            Gizmos.DrawLine(AreaPolyPoints[2], AreaPolyPoints[3]);
            Gizmos.DrawLine(AreaPolyPoints[3], AreaPolyPoints[0]);
            Gizmos.color = color;
        }
    }

    // 检测下相机位置是否有超出范围
    void LateUpdate()
    {
		if (RPGCamera.Instance==null || RPGCamera.Instance.Camera == null)
            return;
        if(isReset && !checkArea())
        {
            // 需要在0.5秒内完成转换
            Vector3 offset = Vector3.Lerp(RPGCamera.Instance.Target.position, CenterPosition, Time.deltaTime*2);

            RPGCamera.Instance.Target.position = offset;
        }
    }

    public bool checkArea()
    {
        Vector3[] currentArea = Util.GetCameraScreenTerrain(RPGCamera.Instance.Camera, terrainLayrerName);

        List<int> indexList = new List<int>();
        for (int i = 0; i < currentArea.Length; ++i)
        {
            if (!Util.ContainsPoint(AreaPolyPoints, currentArea[i]))
            {
                indexList.Add(i);
                break;
            }
        }

        // 超出显示范围了
        if (indexList.Count != 0)
            return false;
        return true;
    }

    Vector3 GetOffsetValue(List<int> pointList, Vector3[] maxPoints, Vector3[] newPoints)
    {
        if (pointList.Count <= 1)
            return Vector3.zero;

        if (pointList.Count == 2)
        {
            if (pointList[0] == 0 && pointList[1] == 1)
            {
                float distance = Util.DistanceFromPointToLine(maxPoints[0], maxPoints[1], newPoints[1]);
                distance = distance / Mathf.Sin(Angles[1] * Mathf.Deg2Rad);
                ToolDrawGizmos.me.Begin();
                ToolDrawGizmos.me.DrawSphere(maxPoints[0], 1f);
                ToolDrawGizmos.me.DrawSphere(maxPoints[1], 1f);
                ToolDrawGizmos.me.DrawSphere(newPoints[1], 1f);
                ToolDrawGizmos.me.End();

                return new Vector3(0f, distance, 0f);
            }
            else if (pointList[0] == 0 && pointList[1] == 3)
            {
                float distance = Util.DistanceFromPointToLine(maxPoints[0], maxPoints[3], newPoints[3]);
                distance = distance / Mathf.Sin(Angles[3] * Mathf.Deg2Rad);
                return new Vector3(distance, 0f, 0f);
            }
            else if (pointList[0] == 1 && pointList[1] == 2)
            {
                float distance = Util.DistanceFromPointToLine(maxPoints[1], maxPoints[2], newPoints[1]);
                distance = distance / Mathf.Sin(Angles[2] * Mathf.Deg2Rad);
                return new Vector3(-distance, 0f, 0f);
            }
            else if (pointList[0] == 2 && pointList[1] == 3)
            {
                float distance = Util.DistanceFromPointToLine(maxPoints[2], maxPoints[3], newPoints[3]);
                ToolDrawGizmos.me.Begin();
                ToolDrawGizmos.me.DrawSphere(maxPoints[2], 1f);
                ToolDrawGizmos.me.DrawSphere(maxPoints[3], 1f);
                ToolDrawGizmos.me.DrawSphere(newPoints[3], 1f);
                ToolDrawGizmos.me.End();
                distance = distance / Mathf.Sin(Angles[3] * Mathf.Deg2Rad);
                return new Vector3(0f, -distance, 0f);
            }
            else
            {
                return Vector3.zero;
            }
        }
        else if (pointList.Count == 3)
        {
            if (!pointList.Contains(0))
            {
                float distance1 = Util.DistanceFromPointToLine(maxPoints[1], maxPoints[2], newPoints[1]);
                float distance2 = Util.DistanceFromPointToLine(maxPoints[3], maxPoints[2], newPoints[3]);
                distance1 = distance1 / Mathf.Sin(CameraShowArea.Instance.Angles[2] * Mathf.Deg2Rad);
                distance2 = distance2 / Mathf.Sin(CameraShowArea.Instance.Angles[0] * Mathf.Deg2Rad);
                return new Vector3(-distance1, -distance2, 0f);
            }
            else if (!pointList.Contains(1))
            {
                float distance1 = Util.DistanceFromPointToLine(maxPoints[0], maxPoints[3], newPoints[0]);
                float distance2 = Util.DistanceFromPointToLine(maxPoints[3], maxPoints[2], newPoints[2]);
                distance1 = distance1 / Mathf.Sin(CameraShowArea.Instance.Angles[3] * Mathf.Deg2Rad);
                return new Vector3(distance1, -distance2, 0f);
            }
            else if (!pointList.Contains(2))
            {
                float distance1 = Util.DistanceFromPointToLine(maxPoints[0], maxPoints[3], newPoints[3]);
                float distance2 = Util.DistanceFromPointToLine(maxPoints[3], maxPoints[2], newPoints[2]);
                distance1 = distance1 / Mathf.Sin(CameraShowArea.Instance.Angles[3] * Mathf.Deg2Rad);
                return new Vector3(distance1, distance2, 0f);
            }
            else
            {
                float distance1 = Util.DistanceFromPointToLine(maxPoints[1], maxPoints[2], newPoints[2]);
                float distance2 = Util.DistanceFromPointToLine(maxPoints[0], maxPoints[1], newPoints[0]);
                distance1 = distance1 / Mathf.Sin(CameraShowArea.Instance.Angles[3] * Mathf.Deg2Rad);
                return new Vector3(distance1, -distance2, 0f);
            }
        }
        else
        {
            return Vector3.zero;
        }
    }
}
