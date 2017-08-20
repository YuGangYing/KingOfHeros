using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
public enum enSimpleGraphicsType
{
    enSGT_Invalid = 0,
    enSGT_SingleTap,
    enSGT_DoubleTap,
    enSGT_LongTap,
    enSGT_Line,
    enSGT_Arc,
    enSGT_Circle,
    enSGT_TwoFingerDrag,
    enSGT_TwoFingerZoomIn,
    enSGT_TwoFingerZoomOut,
    enSGT_Amount,
}

public class CTwoFingerData
{
    public Vector2 vStartAveragePos = Vector2.zero;
    public Vector2 vAveragePos = Vector2.zero;
    public Vector2 vDelPos = Vector2.zero;
    public float fOldFingerDistance = 0f;
    public float fRealFingerDis = 0f;

    public CTwoFingerData() { reset(); }

    public void reset()
    {
        vStartAveragePos = Vector2.zero;
        vAveragePos = Vector2.zero;
        vDelPos = Vector2.zero;
        fOldFingerDistance = 0f;
        fRealFingerDis = 0f;
    }
}


public delegate bool CallBackSimpleTouch(object obj);
public class SimpleTouch : SingletonMonoBehaviour<SimpleTouch>
{
    public float fSingTapTime = 0.20f;
    public float fLongTapTime = 0.8f;
    public float fNormalLineDeltaLeng = 14f;
    public float fNormalLineRate = 0.3f;
    public float fPointArea = 14f;
    public int nSlopeChangeTimes = 4;
    public float fDeltaPointPointDistance = 0.005f;
    private const float ZERO_FLOAT = 0.00005f;

    //public string strTrailPrefabsName = "Trail";
    public GameObject goPrefabsTrail = null;
    GameObject mgoTrail = null;
    
    public bool IsShowBugInfo = false;
    private bool mbLastShowBugInfo = false;
    public GameObject goTextBugInfo = null;
    private TextMesh mClsTxtMesh = null;

    SimpleTouchInput mClsTouchIO = null;

    [HideInInspector]
    private int mMaxFingerAmount = 10;
    CSimpleFinger[] mclsFingers = null;

    List<Vector2> mlistSreenPoint = null;
    List<float> mlistNormalDistance = null;
    List<Vector2> mlistNewPoints = new List<Vector2>();

    Dictionary</*enSimpleGraphicsType*/int, CallBackSimpleTouch> mmapCallBack = null;

    // two finger
    public KeyCode twistKey = KeyCode.LeftAlt;      // 连结,交结
    public KeyCode swipeKey = KeyCode.LeftControl;  // 操作杆

    CTwoFingerData mclsTwo = new CTwoFingerData();

    public bool Enable { get { return this.enabled; } set { this.enabled = value; } }



    public SimpleTouch() { }

    protected override void Init()
    {
        base.Init();

        mclsFingers = new CSimpleFinger[mMaxFingerAmount];
        mlistSreenPoint = new List<Vector2>();
        mlistNormalDistance = new List<float>();

        mmapCallBack = new Dictionary<int/*enSimpleGraphicsType*/, CallBackSimpleTouch>();

        mClsTouchIO = new SimpleTouchInput();

        _CleanPonit();
    }

    void _CleanPonit()
    {
        mlistSreenPoint.Clear();

        if (mClsTxtMesh != null)
        {
            mClsTxtMesh.text = "";
        }

        if (mgoTrail != null)
        {
            GameObject.Destroy(mgoTrail);
            mgoTrail = null;
        }
    }

    public bool addAddHandle(enSimpleGraphicsType en, CallBackSimpleTouch fun)
    {
        if (en >= enSimpleGraphicsType.enSGT_Amount && en <= enSimpleGraphicsType.enSGT_Invalid)
        {
            return false;
        }
        int n = (int)en;

        if (!mmapCallBack.ContainsKey(n))
            mmapCallBack[n] = fun;
        else
            mmapCallBack[n] += fun;

        return true;
    }

    public bool removeHandle(enSimpleGraphicsType en, CallBackSimpleTouch fun)
    {
        if (en >= enSimpleGraphicsType.enSGT_Amount && en <= enSimpleGraphicsType.enSGT_Invalid)
        {
            return false;
        }
        int n = (int)en;

        if (!mmapCallBack.ContainsKey(n))
        {
            return false;
        }

        mmapCallBack[n] -= fun;

        return true;
    }

    public void cleanAllHandle()
    {
        mmapCallBack.Clear();
    }

    void Start()
    {
        _ResetBugInfo();
    }

    void Update()
    {
        if (!this.Enable || UICamera.hoveredObject != null)
        {
            return;
        }

		if(DoubleClick())
		{
			_OnIsDoubleTap();
		}
		else
		{
        	_BugInfoUpdate();
        	_UpdateTouch();
		}
    }


    void _BugInfoUpdate()
    {
        bool IsChange = false;
        if (mbLastShowBugInfo != IsShowBugInfo)
        {
            mbLastShowBugInfo = IsShowBugInfo;
            IsChange = true;
        }

        if (IsChange)
        {
            _ResetBugInfo();
        }
    }

    void _ResetBugInfo()
    {
        if (mClsTxtMesh == null && IsShowBugInfo)
        {
            //Font ft = new Font("LithosPro-Black");

            //GameObject go = new GameObject("TouchShowInfo");
            //mClsTxtMesh = go.AddComponent<TextMesh>();
            //mClsTxtMesh.font = ft;
            //MeshRenderer mr = go.GetComponent<MeshRenderer>();
            //if (mr != null)
            //{
            //    mr.materials[0] = new UnityEngine.Material("LithosPro-Black");
            //}
            if (goTextBugInfo != null)
            {
                GameObject go = Instantiate(goTextBugInfo, Vector3.zero, Quaternion.identity) as GameObject;
                if(go != null)
                    mClsTxtMesh = go.GetComponent<TextMesh>();
            }

        }

        if (mClsTxtMesh != null)
        {
            mClsTxtMesh.gameObject.SetActive(IsShowBugInfo);
        }
    }

    bool _UpdateTouch()
    {
        int touchCount = _GetTouchCount();

#if ((UNITY_ANDROID || UNITY_IPHONE || UNITY_WINRT || UNITY_BLACKBERRY) && !UNITY_EDITOR) 
        _OnTouchs(true, touchCount);
#else
        _OnTouchs(false, touchCount);
#endif

        return true;
    }

    int _GetTouchCount()
    {
        if (mClsTouchIO == null)
            return 0;

        return mClsTouchIO.TouchCount();
    }

    void _OnTouchs(bool realTouch, int touchCount)
    {
        if (realTouch)
        {
            _OnRealTouch(touchCount);
        }
        else
        {
            _OnMouseTouch(touchCount);
        }

        if (touchCount == 1)
        {
            _OnOneTouch();
        }
        else if (touchCount == 2)
        {
            _CleanTrail();
            _OnTwoTouch();
        }
        else
        {
            try
            {
                mclsTwo.reset();
                _ResetTouch();
                _CleanTrail();
            }
            catch (System.Exception)
            {
                throw;
            }

        }
        
    }

    void _OnRealTouch(int touchCount)
    {
        CSimpleFinger[] tmpArray = new CSimpleFinger[mMaxFingerAmount];
        mclsFingers.CopyTo(tmpArray, 0);

        _ResetTouch();
        for (var i = 0; i < touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);

            int t = 0;
            while (t < tmpArray.Length && t < mMaxFingerAmount && mclsFingers[i] == null)
            {
                if (tmpArray[t] != null)
                {
                    if (tmpArray[t].nIndex == touch.fingerId)
                    {
                        mclsFingers[i] = tmpArray[t];
                    }
                }
                t++;
            }

            if (mclsFingers[i] == null)
            {
                mclsFingers[i] = new CSimpleFinger();
                mclsFingers[i].nIndex = touch.fingerId;
                mclsFingers[i].phase = TouchPhase.Began;

                mclsFingers[i].fStartTapTime = Time.realtimeSinceStartup;
                mclsFingers[i].vStartPos = touch.position;
                mclsFingers[i].vOldPos = touch.position;
                mclsFingers[i].vPos = touch.position;
            }
            else
            {
                mclsFingers[i].phase = touch.phase;
            }

            mclsFingers[i].vOldPos = mclsFingers[i].vPos;
            mclsFingers[i].vPos = touch.position;
            mclsFingers[i].vDeltaPos = mclsFingers[i].vPos - mclsFingers[i].vOldPos; // touch.deltaPosition;
            mclsFingers[i].nTapAmount = touchCount;

            mclsFingers[i].fDeltaTapTime = Time.realtimeSinceStartup - mclsFingers[i].fStartTapTime;
        }
    }

    void _ResetTouch()
    {
        for (int i = 0; i < mMaxFingerAmount; i++)
        {
            mclsFingers[i] = null;
        }
    }

    void _OnMouseTouch(int touchCount)
    {
        //Debug.Log("touchCount:" + touchCount.ToString());

        int i = 0;
        while (i < touchCount)
        {
            mclsFingers[i] = mClsTouchIO.getMouseTouch(i, mclsFingers[i]) as CSimpleFinger;
            //mclsFingers[i].nTapAmount = touchCount;
            i++;
        }
    }

    #region ------ process one finger touch ------

    float mfTapStartTime = 0f;
    void _OnOneTouch()
    {
        if (mclsFingers == null || (mclsFingers != null && mclsFingers.Length == 0))
            return;

        int nIndex = 0;
        CSimpleFinger sf = mclsFingers[nIndex];
        if (sf == null)
            return;

        if (sf.phase == TouchPhase.Began)
        {
            this.mfTapStartTime = Time.realtimeSinceStartup;

            sf.enGestureState = enSimpleGestureType.enSGT_Acquisition;

            _CleanBugInfo();
        }

        mlistSreenPoint.Add(sf.vPos);

        _OnTouchTrail(sf.vPos);

        bool bEnd = false;
        bool bLongTap = false;
        bool bSinglePoint = false;
        bool bDoublePoint = false;

        float fDeltaOverTapTime = 0f;

        if (sf.phase == TouchPhase.Canceled)
            sf.enGestureState = enSimpleGestureType.enSGT_Cancel;

        if (sf.phase == TouchPhase.Ended || sf.phase == TouchPhase.Canceled)
        {
            bEnd = true;

            // 单击时间Ctrl
            fDeltaOverTapTime = Time.realtimeSinceStartup - this.mfTapStartTime;
            if (fDeltaOverTapTime < this.fSingTapTime && sf.nTapAmount < 2)
                bEnd = false;

            if (fDeltaOverTapTime >= fLongTapTime)
                bLongTap = true;
            else
            {
                bool bValidArea = _FingerInToValidArea(sf);
                if (bValidArea)
                {
                    if (sf.nTapAmount < 2 && bEnd)
                    {
                        //simple no do
                        bSinglePoint = true;
                    }
                }
            }
        }

        //Logger.LogError("strart :0,  real {0},   ret:{1}   tapDelta:{2}   {3}  {4}",
        //    sf.nTapAmount, sf.phase.ToString(), fDeltaOverTapTime, this.mfTapStartTime, sf.enGestureState.ToString());

        // double point
        if (sf.nTapAmount == 2 && sf.phase == TouchPhase.Stationary && sf.enGestureState == enSimpleGestureType.enSGT_Acquisition)
        {
            bDoublePoint = true;
        }

        bool bDone = false;
        if (bSinglePoint)
        {
            bDone = true;
            _OnIsSimpleTap();
        }
        else if (bDoublePoint)
        {
            bDone = true;
            _OnIsDoubleTap();
        }
        else if (bLongTap)
        {
            bDone = true;
            _OnIsLongTap();
        }
        else
        {
            if (bEnd)
            {
                bDone = true;
                _CalculateResult();
            }
        }

        if (bDone)
        {
            mclsFingers[nIndex] = null;
        }
    }

    void _CalculateResult()
    {
        if (mlistSreenPoint.Count == 0)
            return;

        mlistNewPoints.Clear();
        float fCalcuTime = Time.deltaTime;
        mlistNewPoints = _ClearDirtyData(mlistSreenPoint);
        int nIndx = _OnRate(ref mlistNewPoints);
        float fCallTime = Time.deltaTime - fCalcuTime;

        _CleanTrail();
    }

    List<Vector2> _ClearDirtyData(List<Vector2> points)
    {
        List<Vector2> out_data = new List<Vector2>();
        Vector2[] point_array = points.ToArray();
        //多个步进，至少是2步进
        int clean_count = 0;
        for (int i = 0; i < points.Count - 1; )
        {
            Vector2 pos1 = point_array[i];
            out_data.Add(pos1);
            int step1 = i;
            int step2 = i + 1;

        _ClearDirtyData_Loop_begin:
            Vector2 pos2 = point_array[step2];
            float delta_x = Mathf.Abs(pos2.x - pos1.x);
            float delta_y = Mathf.Abs(pos2.y - pos1.y);

            if (delta_x <= fDeltaPointPointDistance && delta_y <= fDeltaPointPointDistance)
            {
                //Debug.Log("pos[" + step2.ToString() + "] - pos[" + step1.ToString() + "]= (" + delta_x.ToString("f3") + "," + delta_y.ToString("f3") + ")");
                //多余的点，不再计数
                ++step2;
                ++clean_count;
                if (step2 < points.Count - 1)
                {
                    goto _ClearDirtyData_Loop_begin;
                }
            }
            else
            {
                out_data.Add(pos2);
            }
            i = step2 + 1;
        }

        //Debug.Log("clean count : " + clean_count.ToString());
        return out_data;
    }

    int _OnRate(ref List<Vector2> points)
    {
        int count = points.Count;
        float k_value;
        float b_value;
        int y_line;

        float over_rate = 0f;
        int nIndex = 0;
        string[] strRest = { "Unkown", "Circle", "Arc", "Line", };
        int nVaryCount = 0;

        Vector2[] pos_array = points.ToArray();
        mlistNormalDistance.Clear();

        V3ScreenPoint = Vector3.zero;

        Vector2 vCenter = _CentroId(ref points);
        V3ScreenPoint = new Vector3(vCenter.x, vCenter.y, 10);

        _FindLineValue(pos_array[0], pos_array[count - 1], out y_line, out k_value, out b_value);
        if (y_line == 1)
        {
            int over_length_size = 0;
            foreach (Vector2 pos1 in pos_array)
            {
                Vector2 pos2 = _GetLineCrossPos(pos1, k_value, b_value);
                float lineValue = _GetLineCrossValue(pos1, pos2);

                if (lineValue >= fNormalLineDeltaLeng)
                {
                    ++over_length_size;
                }

                mlistNormalDistance.Add(lineValue);
            }

            over_rate = over_length_size * 1.0f / count;
			Debug.Log("over_rate..........................................." + over_rate);
            if (over_rate > fNormalLineRate)
            {
                //nVaryCount = _GetSlopeVaryTimes(points);
                //if (nVaryCount >= 4)
                //{
                //    nIndex = 1;
                //    _OnIsCircle();
                //}
                //else if (nVaryCount >= 2)
                //{
                //    nIndex = 2;
                //    _OnIsArc();
                //}
                //else
                //{
                //    nIndex = 0;
                //}
                nVaryCount = _CheckSlopeVary(points);
                switch (nVaryCount)
                {
                    case 1:
                        nIndex = 2;
                        _OnIsArc();
                        break;
                    case 2:
                        nIndex = 1;
                        _OnIsCircle();
                        break;
                    default:
                        nIndex = 0;
                        break;
                }
            }
            else
            {
                nIndex=3;
                _OnLine();
            }
        }
        /*else
        {
             nIndex=3;
            _OnLine();
        }*/


        string str = string.Format("Rest is {0}, \n  Normal_Lenght{1},\n  rate:{2} overRate:{3},\n  PiontCount:{4} ChangeTimes:{5},\n",
                           strRest[nIndex], fNormalLineDeltaLeng, fNormalLineRate, over_rate, points.Count, nVaryCount);
        _AddBugInfo(str);

        return nIndex;
    }

    #region ------ drag all point check ------
    void _FindLineValue(Vector2 pos1, Vector2 pos2, out int y_line, out float k_value, out float b_value)
    {
        //若y_line=0, 则公式为 y=kx+b
        //若y_line=1, 则公式为 x=b
        if (pos1.x == pos2.x)
        {
            y_line = 0;
            k_value = 1;
            b_value = pos1.x;
        }
        else
        {
            y_line = 1;
            k_value = (pos1.y - pos2.y) / (pos1.x - pos2.x);
            b_value = pos1.y - k_value * pos1.x;
        }
    }

    Vector2 _GetLineCrossPos(Vector2 pos, float k_value, float b_value)
    {
        //先求法线， y=mx+c, m=-1/k, c得依据点来计算
        if (Mathf.Abs(k_value) < 0.001f)
        {
            return new Vector2(pos.x, pos.y - b_value);
        }
        float m_value = -1 / k_value;
        float c_value = pos.y - m_value * pos.x;
        //计算两直线的交点
        float x0 = (c_value - b_value) / (k_value - m_value);
        float y0 = (k_value * c_value - m_value * b_value) / (k_value - m_value);
        return new Vector2(x0, y0);
    }

    float _GetLineCrossValue(Vector2 pos1, Vector2 pos2)
    {
        //求两点间距离
        float x = pos2.x - pos1.x;
        float y = pos2.y - pos1.y;
        return Mathf.Sqrt(x * x + y * y);
    }

    int _GetSlopeVaryTimes(List<Vector2> points)
    {
        //求斜率的正负数变换次数
        //0及无穷大不予计算
        int count = points.Count;
        Vector2[] pos_array = points.ToArray();
        int nVaryCount = 0;
        bool bPositive = false;
        int nStep = 0;
        for (int i = 0; i < count - 1; ++i)
        {
            if (pos_array[i + 1].x == pos_array[i].x)
            {
                continue;
            }
            //计算正负值
            double slope_value = (pos_array[i + 1].y - pos_array[i].y) / (pos_array[i + 1].x - pos_array[i].x);
            bool local_flag;
            if (slope_value <= 0.0005f)
                local_flag = false;
            else
                local_flag = true;

            if (nStep++ == 0)
            {
                //初始值保存
                bPositive = local_flag;
                nVaryCount = 1;
            }

            if (local_flag != bPositive)
            {
                //正负数有变化
                ++nVaryCount;
                bPositive = local_flag;
            }
        }

        return nVaryCount;
    }

    // 返回 1 是弧， 返回 2 是圆， 返回 0 是无法识别
    int _CheckSlopeVary(List<Vector2> points)
    {
        //求斜率的正负数变换次数
        //0及无穷大不予计算
        int count = points.Count;
        Vector2[] pos_array = points.ToArray();
        int nVaryCount = 0;
        bool bPositive = false;
        int nStep = 0;
        List<bool> delta_x_postive_list = new List<bool>();
        List<bool> delta_y_postive_list = new List<bool>();
        for (int i = 0; i < count - 1; ++i)
        {
            if (pos_array[i + 1].x == pos_array[i].x)
            {
                continue;
            }
            //计算正负值
            double slope_value = (pos_array[i + 1].y - pos_array[i].y) / (pos_array[i + 1].x - pos_array[i].x);
            bool local_flag;
            if (slope_value <= ZERO_FLOAT)
                local_flag = false;
            else
                local_flag = true;

            bool delta_x_postive = false;
            bool delta_y_postive = false;
            if ((pos_array[i + 1].y - pos_array[i].y) >= ZERO_FLOAT)
                delta_y_postive = true;
            if ((pos_array[i + 1].x - pos_array[i].x) >= ZERO_FLOAT)
                delta_x_postive = true;

            if (nStep++ == 0)
            {
                //初始值保存
                bPositive = local_flag;
                nVaryCount = 1;
                delta_x_postive_list.Add(delta_x_postive);
                delta_y_postive_list.Add(delta_y_postive);
            }

            if (local_flag != bPositive)
            {
                //正负数有变化
                ++nVaryCount;
                bPositive = local_flag;
                delta_x_postive_list.Add(delta_x_postive);
                delta_y_postive_list.Add(delta_y_postive);
            }
        }

        //通过正负状态来判断线的状态
        int result = -1;
        // 斜率变化3次以内，认为是弧
        if (nVaryCount <= 3)
        {
            if (_SlopeTrend(delta_x_postive_list) && _SlopeTrend(delta_y_postive_list))
            {
                //是弧
                result = 1;
            }
            else
            {
                // 不可识别
                result = 0;
            }
        }
        // 斜率变化4次以上，为为是圆
        else
        {
            if (_SlopeTrend(delta_x_postive_list) && _SlopeTrend(delta_y_postive_list))
            {
                //是圆
                result = 2;
            }
            else
            {
                //不可识别
                result = 0;
            }
        }

        return result;
    }

    //判断斜率变化是否连续的
    // 即 + + - - 循环方式
    bool _SlopeTrend(List<bool> slopes)
    {
        int count = slopes.Count;
        bool[] slope_array = slopes.ToArray();
        bool slope_0 = slope_array[0];
        bool result = true;
        SlopeTrend slope_trend = new SlopeTrend();

        for (int i = 0; i <= count - 1; i += 4)
        {
            string value1 = "";
            int step = 0;
            for (int j = i; j < i + 4 && j <= count - 1; ++j, ++step)
            {
                slope_trend.SetTrend(step, slope_array[j]);
                value1 += slope_array[j].ToString() + ",";
            }
            Debug.Log("test>" + value1);

            if (!slope_trend.IsLoop())
            {
                result = false;
                break;
            }
        }

        return result;
    }
    #endregion

    bool _FingerInToValidArea(CSimpleFinger finger)
    {
        if ((finger.vPos - finger.vStartPos).sqrMagnitude <= (fPointArea * fPointArea))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region ------ process call back function ------
    void _OnIsSimpleTap()
    {
        _OnTap(enSimpleGraphicsType.enSGT_SingleTap);
    }

    void _OnIsDoubleTap()
    {
        _OnTap(enSimpleGraphicsType.enSGT_DoubleTap);
    }

    void _OnIsLongTap()
    {
        _OnTap(enSimpleGraphicsType.enSGT_LongTap);
    }

    void _OnIsCircle()
    {
        _OnTap(enSimpleGraphicsType.enSGT_Circle);
    }

    void _OnIsArc()
    {
        _OnTap(enSimpleGraphicsType.enSGT_Arc);
    }

    void _OnLine()
    {
        _OnTap(enSimpleGraphicsType.enSGT_Line);
    }

    void _OnTap(enSimpleGraphicsType en)
    {
        if (mmapCallBack.Count == 0)
        {
            return;
        }

        int n = (int)en;
        if (!mmapCallBack.ContainsKey(n))
        {
            return;
        }

        Vector3 v;
        Screen2World(V3ScreenPoint, out v);
        CBattleFightGestureData cbfgd = new CBattleFightGestureData(en, v, 0f);

        CallBackSimpleTouch fun = mmapCallBack[n];
        if (fun != null)
        {
            fun(cbfgd);
        }
    }

    #endregion

    void _CleanTrail()
    {
        mlistSreenPoint.Clear();

        if (mgoTrail == null)
            return;

        GameObject.Destroy(mgoTrail);
        mgoTrail = null;
    }

    void _OnTouchTrail(Vector2 vScreenPos)
    {
        Vector3 vWorldPos = getTouchToWordlPoint(vScreenPos, 10f);
        if (mgoTrail == null && goPrefabsTrail != null)
        {
            //GameObject go = Resources.Load(str/*Trail*/);
            mgoTrail = Instantiate(goPrefabsTrail, vWorldPos, Quaternion.identity) as GameObject;
        }
        else
        {
            if(mgoTrail != null)
                mgoTrail.transform.position = vWorldPos;
        }
    }

    public Vector3 getTouchToWordlPoint(Vector2 vSreenPos, float z, bool worldZ = false)
    {
        if (!worldZ)
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(vSreenPos.x, vSreenPos.y, z));
        }
        else
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(vSreenPos.x, vSreenPos.y, z - Camera.main.transform.position.z));
        }
    }

    void _AddBugInfo(string str)
    {
        if (mClsTxtMesh == null)
            return;

        mClsTxtMesh.text += string.Format("{0}\n", str);
    }

    void _CleanBugInfo()
    {
        if (mClsTxtMesh == null)
            return;

        mClsTxtMesh.text = "";
    }

    public void getTrailPoint(ref List<Vector2> lt)
    {
        lt.Clear();

        lt = mlistNewPoints;
    }

    #endregion

    #region ------ process two finger touch ------

    void _OnTwoTouch()
    {
        if (mclsFingers == null)
            return;
        if (mclsFingers != null && mclsFingers.Length < 2)
        {
            return;
        }

        CSimpleFinger finger0 = mclsFingers[0];
        CSimpleFinger finger1 = mclsFingers[1];


        Vector2 vTemp = Vector2.zero;
        if (finger0.phase == TouchPhase.Began || finger1.phase == TouchPhase.Began)
        {
            mclsTwo.fOldFingerDistance = Mathf.Abs(Vector2.Distance(finger0.vPos, finger1.vPos));
            vTemp =  finger0.vPos + finger1.vPos;
            Logger.LogDebug("_OnTwoTouch begin:{0}", vTemp);
            mclsTwo.vStartAveragePos = vTemp / 2;

            return;
        }

        bool bEnd = false;
        if ((finger0.phase == TouchPhase.Ended || finger1.phase == TouchPhase.Ended)
            || (finger0.phase == TouchPhase.Canceled || finger1.phase == TouchPhase.Canceled))
        {
            bEnd = true;
        }

        // long tap
        float f0 = finger0.fDeltaTapTime + finger0.fStartTapTime;
        float f1 = finger1.fDeltaTapTime + finger1.fStartTapTime;
        if ((finger0.vDeltaPos == Vector2.zero && f0 >= fLongTapTime)
            || (finger1.vPos == Vector2.zero && f1 >= fLongTapTime ))
        {
            mclsTwo.fRealFingerDis = mclsTwo.fOldFingerDistance;
            mclsTwo.vAveragePos = mclsTwo.vStartAveragePos;
            return;
        }

        mclsTwo.fRealFingerDis = Mathf.Abs(Vector2.Distance(finger0.vPos, finger1.vPos));
        vTemp =  finger0.vPos + finger1.vPos;
        //Logger.LogDebug("_OnTwoTouch move:{0}", vTemp);
        string str = string.Format("_OnTwoTouch move:{0}", vTemp);
        //ConsoleSelf.me.addText(str);
        mclsTwo.vAveragePos = vTemp / 2;

        mclsTwo.vDelPos = mclsTwo.vAveragePos - mclsTwo.vStartAveragePos;

        //Logger.LogDebug("_OnTwoTouch delta:{0}", mclsTwo.vDelPos);
        //str = string.Format("_OnTwoTouch delta:{0}", mclsTwo.vDelPos);
        //ConsoleSelf.me.addText(str);

        bool bMove = false;
        if (!_FingerInToValidArea(finger0) || !_FingerInToValidArea(finger1))
            bMove = true;

        float fTempDis = Mathf.Abs(mclsTwo.fRealFingerDis - mclsTwo.fOldFingerDistance);
        //str = string.Format("_OnTwoTouch delta:{0} real:{1}  old:{2}", fTempDis, mclsTwo.fRealFingerDis, mclsTwo.fOldFingerDistance);
        //ConsoleSelf.me.addText(str);

        float dot = Vector2.Dot(finger0.vDeltaPos.normalized, finger1.vDeltaPos.normalized);
        //str = string.Format("_OnTwoTouch dot product:{0} no0:{1}  no1:{2}", dot, finger0.vDeltaPos.normalized, finger1.vDeltaPos.normalized);
        //ConsoleSelf.me.addText(str);

        if (dot > 0)  // maybe drag or long tap
        {
            if (bMove)
            {
                object obj = mclsTwo;//-mclsTwo.vDelPos;
                _OnTwoFingerDrag(obj);
            }
        }
        else if (mclsTwo.fRealFingerDis < mclsTwo.fOldFingerDistance) // zoom in
        {
            float fInDis = mclsTwo.fRealFingerDis - mclsTwo.fOldFingerDistance;
            _OnTwoFingerZoom(mclsTwo, true);
            //mclsTwo.fRealFingerDis = mclsTwo.fOldFingerDistance;
        }
        else if (mclsTwo.fRealFingerDis >= mclsTwo.fOldFingerDistance)  // zoom out
        {
            float fOutDis = mclsTwo.fRealFingerDis - mclsTwo.fOldFingerDistance;
            _OnTwoFingerZoom(mclsTwo, false);
            //mclsTwo.fRealFingerDis = mclsTwo.fOldFingerDistance;
        }

        if (bEnd)
        {
            mclsTwo.reset();
            _ResetTouch();
        }
    }

    void _OnTwoFingerDrag(object obj)
    {
        int n = (int)enSimpleGraphicsType.enSGT_TwoFingerDrag;
        if (!mmapCallBack.ContainsKey(n))
        {
            return;
        }
        CallBackSimpleTouch fun = mmapCallBack[n];
        if (fun != null)
        {
            fun(obj);
        }
    }

    void _OnTwoFingerZoom(object fDis, bool bZoomIn)
    {
        CallBackSimpleTouch fun = null;
        int n = 0;
        if (bZoomIn)
        {
            n = (int)enSimpleGraphicsType.enSGT_TwoFingerZoomIn;
            if (!mmapCallBack.ContainsKey(n))
            {
                return;
            }
            fun = mmapCallBack[n];
            if (fun != null)
                fun((object)fDis);
        }
        else
        {
            n = (int)enSimpleGraphicsType.enSGT_TwoFingerZoomOut;
            if (!mmapCallBack.ContainsKey(n))
            {
                return;
            }
            fun = mmapCallBack[n];
            if (fun != null)
                fun((object)fDis);
        }
    }

    #endregion



    public Vector3 V3ScreenPoint { get; set; }

    Vector2 _CentroId(ref List<Vector2> points)
    {
        float x = 0.0f, y = 0.0f;
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 point = points[i];
            x += point.x;
            y += point.y;
        }

        x /= points.Count;
        y /= points.Count;

        return new Vector2(x, y);
    }

    public static bool Screen2World(Vector3 inV, out Vector3 outV)
    {
        outV = Vector3.zero;
        //Vector3 v3 = V3ScreenPoint;
        inV.z = 0f;
        Ray clsRay = Camera.main.ScreenPointToRay(inV);
        RaycastHit info;
//        if (Physics.Raycast(clsRay, out info, 1000f, 1 << LayerMask.NameToLayer("Terrain")) == false)
		if (Physics.Raycast(clsRay, out info, 1000f, 1 << LayerMask.NameToLayer("Ground")) == false)
            return false;

        outV = info.point;

        return true;
    }

	float clicked = 0;
	float clicktime = 0;
	float clickdelay = 0.5f;
	bool DoubleClick(){
		if (Input.GetMouseButtonDown (0)) {
			clicked++;
			if (clicked == 1) clicktime = Time.time;
		}        
		if (clicked > 1 && Time.time - clicktime < clickdelay) {
			clicked = 0;
			clicktime = 0;
			return true;
		} else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;        
		return false;
	}
}