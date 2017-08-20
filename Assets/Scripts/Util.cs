using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLG
{
    public class Util
    {
        public static int ToInt(string v, int def)
        {
            int.TryParse(v, out def);
            return def;
        }
        
        // 枚举组合
        static public int Union(int v1, int v2)
        {
            return (v1 << 16) + v2;
        }

        // "解析一行"
        public static bool Split(ref string src, List<string> vsField, char compart)
        {
            char[] sLine = src.ToCharArray();
            vsField.Clear();

            int line_length = src.Length;
            if (line_length == 0)
                return false;

            string field = "";
            bool bColon = false; // "为冒号内的字符串"
            for (uint i = 0; i < line_length; i++)
            {
                if (bColon == false)
                {
                    if (sLine[i] == compart)  // "为分隔符"
                    {
                        vsField.Add(field);
                        field = "";
                    }
                    else if (sLine[i] == '"') // "为冒号，则在此引号内的字符串算为一个段"
                    {
                        bColon = true;
                        field += sLine[i];
                    }
                    else
                    {
                        field += sLine[i];
                    }
                }
                else
                {
                    field += sLine[i];
                    if (sLine[i] == '"')
                    {
                        bColon = false;
                    }
                }
            }

            if (field.Length > 0)
            {
                vsField.Add(field);
            }

            return true;
        }

        static public byte[] ToByte(ref string s)
        {
            return System.Text.Encoding.GetEncoding("gb2312").GetBytes(s);
        }

        static public string Write(List<List<string>> dataList, char compart)
        {
            string result = "";
            List<string> strList;
            for (int i = 0; i < dataList.Count; ++i)
            {
                strList = dataList[i];
                for (int j = 0; j < strList.Count; ++j)
                {
                    result += strList[j];
                    result += compart;
                }

                result += '\n';
            }

            return result;
        }

        public static bool StringToEnum<T>(string str, out T v)
        {
            try
            {
                v = (T)System.Enum.Parse(typeof(T), str, true);
                return true;
            }
            catch (System.Exception)
            {
                v = default(T);
                return false;
            }
        }

        public static T StringToEnum<T>(string str, T def)
        {
            try
            {
                def = (T)System.Enum.Parse(typeof(T), str, true);
            }
            catch (System.Exception)
            {

            }

            return def;
        }

        // 随机从list当中取nums个数值，保证不会重复提取
        public static T[] RandList<T>(T[] list, int nums)
        {
            nums = Mathf.Min(nums, list.Length);
            int[] indexs = new int[list.Length];
            for (int i = 0; i < indexs.Length; ++i)
                indexs[i] = i;

            T[] result = new T[nums];
            for (int i = 0; i < nums; ++i)
            {
                int randv = UnityEngine.Random.Range(i, indexs.Length);
                if (randv != i)
                {
                    int v = indexs[i];
                    indexs[i] = indexs[randv];
                    indexs[randv] = v;
                }

                result[i] = list[indexs[i]];
            }

            return result;
        }

        // 地形投影到相机范围的多边形
        public static Vector3[] GetCameraScreenTerrain(Camera camera,string strTerrain)
        {
            int layer = 1 << LayerMask.NameToLayer(strTerrain);
            Ray[] rays = new Ray[4];
            rays[0] = camera.ScreenPointToRay(new Vector3(0f, 0f, 0f));
            rays[1] = camera.ScreenPointToRay(new Vector3(camera.pixelWidth, 0f, 0f));
            rays[2] = camera.ScreenPointToRay(new Vector3(camera.pixelWidth, camera.pixelHeight, 0f));
            rays[3] = camera.ScreenPointToRay(new Vector3(0f, camera.pixelHeight, 0f));

            Vector3[] poss = new Vector3[4];
            RaycastHit info;
            for (int i = 0; i < rays.Length; ++i)
            {
                if (Physics.Raycast(rays[i], out info, float.MaxValue, layer) == false)
                    poss[i] = Vector3.zero;
                else
                    poss[i] = info.point;
            }

            return poss;
        }

        /** Checks if \a p is inside the polygon (XZ space)
         * \author http://unifycommunity.com/wiki/index.php?title=PolyContainsPoint (Eric5h5)
         * */
        public static bool ContainsPoint(Vector3[] polyPoints, Vector3 p)
        {
            int j = polyPoints.Length - 1;
            bool inside = false;

            for (int i = 0; i < polyPoints.Length; j = i++)
            {
                if (((polyPoints[i].z <= p.z && p.z < polyPoints[j].z) || (polyPoints[j].z <= p.z && p.z < polyPoints[i].z)) &&
                   (p.x < (polyPoints[j].x - polyPoints[i].x) * (p.z - polyPoints[i].z) / (polyPoints[j].z - polyPoints[i].z) + polyPoints[i].x))
                    inside = !inside;
            }
            return inside;
        }

        static public Vector3 ScreenToWorldPosition(Vector3 screenPosition, Camera camera, int layer, out RaycastHit info)
        {
            Ray ray = camera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out info, float.MaxValue, layer) == false)
                return Vector3.zero;

            return info.point;
        }

        public static Vector3 QuaternionToDirection(Quaternion q)
        {
            return Matrix4x4.TRS(Vector3.zero, q, Vector3.one).MultiplyVector(Vector3.forward);
        }

        static public float GetCamerToTerrainDistance(Camera camera, out RaycastHit info,string strTerrain)
        {
            Ray ray = new Ray(camera.transform.localToWorldMatrix.MultiplyPoint(Vector3.back), QuaternionToDirection(camera.transform.rotation));
            if (Physics.Raycast(ray, out info, float.MaxValue, 1 << LayerMask.NameToLayer(strTerrain)) == false)
            {
                ray.origin = ray.GetPoint(-10000f);
                if (Physics.Raycast(ray, out info, float.MaxValue, 1 << LayerMask.NameToLayer(strTerrain)) == false) 
                    return 0f;

                return -info.distance;
            }

            return info.distance;
        }

        // a和b是线段的两个端点， c是检测点XZSpace
        public static float DistanceFromPointToLine(Vector3 l1, Vector3 l2, Vector3 point)
        {
            // given a line based on two points, and a point away from the line,
            // find the perpendicular distance from the point to the line.
            // see http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
            // for explanation and defination.
            return (float)(Math.Abs((l2.x - l1.x) * (l1.z - point.z) - (l1.x - point.x) * (l2.z - l1.z)) / Math.Sqrt(Math.Pow(l2.x - l1.x, 2f) + Math.Pow(l2.z - l1.z, 2f)));
        }
    }
}

