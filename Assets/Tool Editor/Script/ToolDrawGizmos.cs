using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ToolDrawGizmos : MonoBehaviour
{
    static ToolDrawGizmos this_obj = null;
    public static ToolDrawGizmos me
    {
        get
        {
            if (this_obj == null)
            {
                GameObject obj = new GameObject("ToolDrawGizmos");
                obj.AddComponent<ToolDrawGizmos>();
            }

            return this_obj;
        }
    }

    void Awake()
    {
        this_obj = this;
    }

    class Data
    {
        public Color color = Color.yellow;
        public List<Vector3> drawLine; // »­Ïß
        public List<Vector4> drawSphere; // »­Ô²
		public List<Vector4> drawCircle;
	}

    public int Begin()
    {
#if UNITY_EDITOR
        m_only_id++;
        m_current = new Data();
        m_drawList[m_only_id] = m_current;
        return m_only_id;     
#else
        return -1;
#endif
    }

    public void DrawColor(Color color)
    {
#if UNITY_EDITOR
        m_current.color = color;
#endif
    }

    public void DrawLine(Vector3 from, Vector3 to)
    {
#if UNITY_EDITOR
        if (m_current.drawLine == null)
        {
            m_current.drawLine = new List<Vector3>();
        }

        m_current.drawLine.Add(from);
        m_current.drawLine.Add(to);
#endif
    }

    public void DrawSphere(Vector3 center, float radius)
    {
#if UNITY_EDITOR
        if (m_current.drawSphere == null)
        {
            m_current.drawSphere = new List<Vector4>();
        }

        m_current.drawSphere.Add(new Vector4(center.x, center.y, center.z, radius));
#endif
    }

	public void DrawCircle(Vector3 center, float radius)
	{
#if UNITY_EDITOR
        if (m_current.drawSphere == null)
		{
			m_current.drawCircle = new List<Vector4>();
		}
		
		m_current.drawCircle.Add(new Vector4(center.x, center.y, center.z, radius));
#endif
	}

    public void End()
    {
#if UNITY_EDITOR
        m_current = null;
#endif
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        foreach (KeyValuePair<int, Data> itr in m_drawList)
        {
            Data d = itr.Value;
            UnityEngine.Gizmos.color = d.color;
            if (d.drawLine != null)
            {
                for (int i = 0; (i + 1) < d.drawLine.Count; )
                {
                    UnityEngine.Gizmos.DrawLine(d.drawLine[i], d.drawLine[i + 1]);
                    i += 2;
                }
            }

            if (d.drawSphere != null)
            {
                Vector4 v;
                for (int i = 0; i < d.drawSphere.Count; ++i)
                {
                    v = d.drawSphere[i];
                    UnityEngine.Gizmos.DrawWireSphere(new Vector3(v.x, v.y, v.z), v.w);
                }
            }
			if (d.drawCircle != null)
			{
				Vector4 v;
				for (int i = 0; i < d.drawCircle.Count; ++i)
				{
					v = d.drawCircle[i];
					drawCircle(new Vector3(v.x, v.y, v.z), v.w);
				}
			}
        }

        m_drawList.Clear();
#endif
    }

	private void drawCircle(Vector3 center, float radius)
	{
#if UNITY_EDITOR
        float fTheta = 0.1f;
		Vector3 beginPoint = Vector3.zero;
		Vector3 firstPoint = Vector3.zero;
		for (float theta = 0; theta < 2 * Mathf.PI; theta += fTheta)
		{
			float x = radius * Mathf.Cos(theta);
			float z = radius * Mathf.Sin(theta);
			Vector3 endPoint = new Vector3(x, 0, z);
			if (theta == 0)
			{
				firstPoint = endPoint;
			}
			else
			{
				Gizmos.DrawLine(beginPoint, endPoint);
			}
			beginPoint = endPoint;
		}
		Gizmos.DrawLine(firstPoint, beginPoint);
#endif
	}

    int m_only_id = 0;
    Data m_current;
    Dictionary<int, Data> m_drawList = new Dictionary<int, Data>();
}
