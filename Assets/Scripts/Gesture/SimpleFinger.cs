using UnityEngine;
using System.Collections;
//-
public enum enSimpleGestureType
{
    enSGT_None = 0,
    enSGT_Tap,
    enSGT_Drag,
    enSGT_Swipe,
    enSGT_LongTap,
    enSGT_Pinch,
    enSGT_Twist,
    enSGT_Cancel,
    enSGT_Acquisition,
    enSGT_Amount,
};


// finger class
public class CSimpleFinger
{
    public int nIndex = 0;
    public int nTapAmount = 0;
    public enSimpleGestureType enGestureState = enSimpleGestureType.enSGT_None;
    public float fStartTapTime = 0f;
    public float fDeltaTapTime = 0f;

    public Vector2 vStartPos = Vector2.zero;
    public Vector2 vPos = Vector2.zero;
    public Vector2 vDeltaPos = Vector2.zero;
    public Vector2 vOldPos = Vector2.zero;

    public TouchPhase phase;

    public GameObject pickedObject = null;
    public Camera pickedCamera = null;


    public CSimpleFinger() { }
}


public class SimpleTouchInput
{
    #region private members
    private Vector2[] oldMousePosition = new Vector2[2];
    private int[] tapCount = new int[2];
    private float[] startActionTime = new float[2];
    private float[] deltaTime = new float[2];
    private float[] tapeTime = new float[2];

    // Complexe 2 fingers simulation
    private bool bComplex = false;
    private Vector2 deltaFingerPosition;
    private Vector2 oldFinger2Position;
    private Vector2 complexCenter;
    #endregion

    #region Public methods
    // Return the number of touch
    public int TouchCount()
    {

#if ((UNITY_ANDROID || UNITY_IPHONE || UNITY_WINRT || UNITY_BLACKBERRY) && !UNITY_EDITOR) 
		return getTouchCount(true);
#else
        return getTouchCount(false);
#endif

    }

    private int getTouchCount(bool realTouch)
    {
        int count = 0;

        if (realTouch)
        {
            count = Input.touchCount;
        }
        else
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
            {
                count = 1;
                if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(SimpleTouch.me.twistKey) 
                    || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(SimpleTouch.me.swipeKey))
                    count = 2;
                if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(SimpleTouch.me.twistKey) 
                    || Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(SimpleTouch.me.swipeKey))
                    count = 2;
            }
        }

        return count;
    }

    // return in CSimpleFinger structure all informations on an touch
    public CSimpleFinger getMouseTouch(int fingerIndex, CSimpleFinger myFinger)
    {
        CSimpleFinger finger;

        if (myFinger != null)
        {
            finger = myFinger;
        }
        else
        {
            finger = new CSimpleFinger();
            finger.enGestureState = enSimpleGestureType.enSGT_None;
        }


        if (fingerIndex == 1 && (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(SimpleTouch.me.twistKey) 
            || Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(SimpleTouch.me.swipeKey)) )
        {
            finger.nIndex = fingerIndex;
            finger.vPos = oldFinger2Position;
            finger.vDeltaPos = finger.vPos - oldFinger2Position;
            finger.nTapAmount = tapCount[fingerIndex];
            finger.fDeltaTapTime = Time.realtimeSinceStartup - deltaTime[fingerIndex];
            finger.phase = TouchPhase.Ended;

            return finger;
        }

        if (Input.GetMouseButton(0))
        {

            finger.nIndex = fingerIndex;
            finger.vPos = GetPointerPosition(fingerIndex);

            if (Time.realtimeSinceStartup - tapeTime[fingerIndex] > SimpleTouch.me.fSingTapTime)
            {
                tapCount[fingerIndex] = 0;
            }

            if (Input.GetMouseButtonDown(0) || (fingerIndex == 1 && (Input.GetKeyDown(KeyCode.LeftAlt) || 
                Input.GetKeyDown(SimpleTouch.me.twistKey) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(SimpleTouch.me.swipeKey))))
            {
                // Began						
                finger.vPos = GetPointerPosition(fingerIndex);
                finger.vDeltaPos = Vector2.zero;
                tapCount[fingerIndex] = tapCount[fingerIndex] + 1;
                finger.nTapAmount = tapCount[fingerIndex];
                startActionTime[fingerIndex] = Time.realtimeSinceStartup;
                deltaTime[fingerIndex] = startActionTime[fingerIndex];
                finger.fDeltaTapTime = Time.realtimeSinceStartup;
                finger.fStartTapTime = Time.realtimeSinceStartup;
                finger.phase = TouchPhase.Began;

                finger.vStartPos = finger.vPos;

                if (fingerIndex == 1)
                {
                    oldFinger2Position = finger.vPos;
                }
                else
                {
                    oldMousePosition[fingerIndex] = finger.vPos;
                }

                if (tapCount[fingerIndex] == 1)
                {
                    tapeTime[fingerIndex] = Time.realtimeSinceStartup;
                }

                //Logger.LogError("11111  strart :0,  real {0}", finger.nTapAmount );

                return finger;
            }


            finger.vDeltaPos = finger.vPos - oldMousePosition[fingerIndex];

            finger.nTapAmount = tapCount[fingerIndex];
            finger.fDeltaTapTime = Time.realtimeSinceStartup - deltaTime[fingerIndex];
            if (finger.vDeltaPos.sqrMagnitude < 1f)
            {
                finger.phase = TouchPhase.Stationary;
            }
            else
            {
                finger.phase = TouchPhase.Moved;
            }

            oldMousePosition[fingerIndex] = finger.vPos;
            deltaTime[fingerIndex] = Time.realtimeSinceStartup;

            return finger;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            finger.nIndex = fingerIndex;
            finger.vPos = GetPointerPosition(fingerIndex);
            finger.vDeltaPos = finger.vPos - oldMousePosition[fingerIndex];
            finger.nTapAmount = tapCount[fingerIndex];
            finger.fDeltaTapTime = Time.realtimeSinceStartup - deltaTime[fingerIndex];
            finger.phase = TouchPhase.Ended;
            oldMousePosition[fingerIndex] = finger.vPos;

            return finger;
        }


        return null;
    }

    // Get the position of the simulate second finger
    public Vector2 GetSecondFingerPosition()
    {

        Vector2 pos = new Vector2(-1, -1);

        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(SimpleTouch.me.twistKey)) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(SimpleTouch.me.swipeKey)))
        {
            if (!bComplex)
            {
                bComplex = true;
                deltaFingerPosition = (Vector2)Input.mousePosition - oldFinger2Position;
            }
            pos = GetComplex2finger();
            return pos;
        }
        else if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(SimpleTouch.me.twistKey))
        {
            pos = GetPinchTwist2Finger();
            bComplex = false;
            return pos;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(SimpleTouch.me.swipeKey))
        {

            pos = GetComplex2finger();
            bComplex = false;
            return pos;
        }

        return pos;
    }
    #endregion

    #region Private methods
    // Get the postion of simulate finger
    private Vector2 GetPointerPosition(int index)
    {

        Vector2 pos;

        if (index == 0)
        {
            pos = Input.mousePosition;
            return pos;
        }
        else
        {
            return GetSecondFingerPosition();

        }
    }

    // Simulate for a twist or pinc
    private Vector2 GetPinchTwist2Finger()
    {

        Vector2 position;

        if (complexCenter == Vector2.zero)
        {
            position.x = (Screen.width / 2.0f) - (Input.mousePosition.x - (Screen.width / 2.0f));
            position.y = (Screen.height / 2.0f) - (Input.mousePosition.y - (Screen.height / 2.0f));
        }
        else
        {
            position.x = (complexCenter.x) - (Input.mousePosition.x - (complexCenter.x));
            position.y = (complexCenter.y) - (Input.mousePosition.y - (complexCenter.y));
        }
        oldFinger2Position = position;

        return position;
    }

    // complexe Alt + Ctr
    private Vector2 GetComplex2finger()
    {

        Vector2 position;

        position.x = Input.mousePosition.x - deltaFingerPosition.x;
        position.y = Input.mousePosition.y - deltaFingerPosition.y;

        complexCenter = new Vector2((Input.mousePosition.x + position.x) / 2f, (Input.mousePosition.y + position.y) / 2f);
        oldFinger2Position = position;

        return position;
    }
    #endregion
}
