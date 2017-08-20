using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
public class Drawer : MonoBehaviour {
    public static Color defaultColor = Color.red;
    public static float defaultWidth = 0.6f;
    int nIndex = 0;
    Dictionary<int, VectorLine> lineMgr = new Dictionary<int, VectorLine>();

    Color mCurColor;
    float mCurWidth;

    [SerializeField]
    public bool show
    {
        set
        {
            foreach (KeyValuePair<int, VectorLine> item in lineMgr)
            {
                item.Value.active = value;
            }
        }
    }

	void Start()
    {
        mCurWidth = defaultWidth;
        mCurColor = defaultColor;
        setLine(defaultColor, mCurWidth);
	}

    public void setLine(Color color, float fWidth)
    {
        mCurWidth = fWidth;
        mCurColor = color;
    }

    public void remove(int theIndex)
    {
        VectorLine line;
        if (lineMgr.TryGetValue(theIndex, out line))
        {
            Vector.DestroyLine(ref line);
            lineMgr.Remove(theIndex);
        }
    }
    //划线
    public int drawLine(Vector3 begin, Vector3 end)
    {
        Vector3[] ptList = new Vector3[2];
        ptList[0] = begin;
        ptList[1] = end;

        VectorLine line = new VectorLine("temp", ptList, mCurColor, null, mCurWidth);
        Vector.DrawLine3DAuto(line);

        line.vectorObject.transform.parent = this.gameObject.transform;
        lineMgr.Add(nIndex,line);
        return nIndex++;
    }

    //画圆形
    public int drawCircle(Vector3 centre, float fRadius)
    {
        Vector3 [] lineList = new Vector3 [200];
        VectorLine line = new VectorLine("circle", lineList, mCurColor, null, mCurWidth);
        Vector.MakeCircleInLine(line, centre,Vector3.down, fRadius, 100);
        Vector.DrawLine3DAuto(line);

        line.vectorObject.transform.parent = this.gameObject.transform;
        lineMgr.Add(nIndex, line);
        return nIndex++;
    }

    public int drawRect(Vector3 topLeft, Vector3 bottomRight)
    {
        Vector3[] lineList = new Vector3[8];
        VectorLine line = new VectorLine("rect", lineList, mCurColor, null, mCurWidth);
        Vector.MakeRectInLine(line, topLeft, bottomRight);
        Vector.DrawLine3DAuto(line);

        line.vectorObject.transform.parent = this.gameObject.transform;
        lineMgr.Add(nIndex, line);
        return nIndex++;
    }

    public int drawFan(Vector3 centre, float fAngle, float fRadius)
    {
        float f1 = fAngle / 2 * Mathf.Deg2Rad;
        float f2 = fAngle / 4 * Mathf.Deg2Rad;
        float fX1 = fRadius * Mathf.Sin(f1);
        float fZ1 = fRadius * Mathf.Cos(f1);
        float fX2 = fRadius * Mathf.Sin(f2);
        float fZ2 = fRadius * Mathf.Cos(f2);

        Vector3 begin1 = new Vector3(transform.position.x - fX1, transform.position.y, transform.position.z + fZ1);
        Vector3 begin2 = new Vector3(transform.position.x - fX2, transform.position.y, transform.position.z + fZ2);
        Vector3 end1 = new Vector3(transform.position.x + fX1, transform.position.y, transform.position.z + fZ1);
        Vector3 end2 = new Vector3(transform.position.x + fX2, transform.position.y, transform.position.z + fZ2);
        Vector3 middle = new Vector3(transform.position.x, transform.position.y, transform.position.z + fRadius);

        Vector3[] ptList = new Vector3[4] { begin1, begin2, end2, end1 };

        Vector3[] lineList = new Vector3[200];
        VectorLine line = new VectorLine("rect", lineList, mCurColor, null, mCurWidth);

        Vector.MakeCurveInLine(line, ptList);
        Vector.DrawLine3DAuto(line);

        line.vectorObject.transform.parent = this.gameObject.transform;
        lineMgr.Add(nIndex, line);
        return nIndex++;
    }

    public void clear()
    {
        foreach (KeyValuePair<int, VectorLine> item in lineMgr)
        {
            VectorLine line = item.Value;
            //item.Value.active = false;
            Vector.DestroyLine(ref line);
        }
        lineMgr.Clear();
    }

    public void cleanByIndex(int nIndex)
    {
        if (!lineMgr.ContainsKey(nIndex))
            return;

        lineMgr.Remove(nIndex);
    }
}
