using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataMgr;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
public class BattleCameraOperator : MonoBehaviour 
{
    public enum enCameraState
    {
        enCS_Invalid = 0,
        enCS_Auto,
        enCS_Target,
        enCS_Amount,
    }

    public GameObject BattleGameObj { get; set; }
    public Transform mCameraRoot = null;
	public CameraPathBezierAnimator mCameraAnimator1 ;
	public CameraPathBezierAnimator mCameraAnimator2 ;
    private Camera mMainCamera = null;
    public Camera camera { get { return mMainCamera; } }
    public int CameraState { get; set; }  // 0:invalid   1:run camera1  2:run camera2

    private Vector2 mvDeltaPos = Vector2.zero;
    private Vector2 mvMoveDeltaPos = Vector2.zero;
    public float MoveSpeed = 0.1f;
    public bool CanOperate = false;
    public float RayLength = 5f;
    public float MaxDistance = 38.9f;
    public float MinDistance = 7.9f;

    // lookat target 
    public float TargetDistance = 25f;
    public float TargetMoveSpeed = 3f;
    public float TargetYaw = 5f;
    public float TargetPitch = 50f;

    private List<Vector3> mlistPos = new List<Vector3>();

    MonoSwitchImp mSwitch = null;
    GameObject mgoSaveCamera = null;
    enCameraState State { get; set; }

	void Start()
	{
        this.State = enCameraState.enCS_Invalid;
        this.mMainCamera = this.GetComponent<Camera>();
        this.init(0);
		startMove();
		mCameraAnimator1.AnimationFinished += BattleController.SingleTon().BattleBegin;
		mCameraAnimator1.AnimationFinished += ActiveSimpleTouch;
        SLG.GlobalEventSet.SubscribeEvent(SLG.eEventType.NodifyChangeFightState, this._ChangeState);
	}

	void ActiveSimpleTouch()
	{
		SimpleTouch.me.enabled = true;
		mCameraAnimator1.gameObject.SetActive(false);
		mCameraAnimator2.gameObject.SetActive(false);
	}

    bool _ChangeState(SLG.EventArgs obj)
    {
        int n = (int)obj.m_obj;
        if (n == 1)
        {
            this.CanOperate = true;
        }
        else
            return false;

        return true;
    }

    void LateUpdate()
    { 
        onRun();
		if (DataManager.me!=null && DataManager.getBattleUIData()!=null && DataManager.getBattleUIData().GetCurHeroId() != CConstance.INVALID_ID)
        {
            if (this.mgoSaveCamera == null)
            {
                mgoSaveCamera = new GameObject();
				mgoSaveCamera.name = "mgoSaveCamera";
                this.mgoSaveCamera.transform.position = Vector3.zero;
            }

            if (this.State != enCameraState.enCS_Target)
            {
                this.State = enCameraState.enCS_Target;
                mgoSaveCamera.transform.position = this.camera.transform.position;
                mgoSaveCamera.transform.rotation = this.camera.transform.rotation;

                mgoSaveCamera.transform.eulerAngles = this.camera.transform.eulerAngles;
                mgoSaveCamera.transform.forward = this.camera.transform.forward;
            }
            GameObject go = BattleController.SingleTon().GetHeroById(DataManager.getBattleUIData().GetCurHeroId());

            if (go != null) this.IsLookAt(go.transform);
        }
        else
        {
            if (this.mgoSaveCamera == null)
            {
                mgoSaveCamera = new GameObject();
                this.mgoSaveCamera.transform.position = Vector3.zero;
            }
            if (this.State != enCameraState.enCS_Auto && this.mgoSaveCamera.transform.position != Vector3.zero)
            {
                this.State = enCameraState.enCS_Auto;
                this.IsLookAt(this.mgoSaveCamera.transform, true);
                this.mgoSaveCamera.transform.position = Vector3.zero;
            }
        }
    }

    public bool init(int nGateLv)
    {
        mlistPos.Clear();
        this.CanOperate = false;
        this.CameraState = 0;
        SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_TwoFingerDrag, this._OnDrag);
        SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_TwoFingerZoomIn, this._OnZoomIn);
        SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_TwoFingerZoomOut, this._OnZoomOut);
        return true;
    }

    public void startMove()
    {
        try
        {
            this.CameraState = 1;
            mCameraAnimator1.Play();
            //BattleMain.me.State = BattleMain.enFightingState.enFS_CameraPosEffect;
        }
        catch (System.Exception ex)
        {
			Debug.LogError(ex != null ? ex.Message : "<null>");
            throw;
        }
    }

    public void moveCamera2()
    {
        this.CameraState = 2;
        mCameraAnimator2.Play();

    }

    public bool isAnimateStop(int nIndex)
    {
        try
        {
            if(!mCameraAnimator1.isPlaying && nIndex == 1)
            {
                return true;
                //BattleMain.me.State = BattleMain.enFightingState.enFS_BeginEffect;
            }
            else if (!mCameraAnimator2.isPlaying && nIndex == 2)
            {
                return true;
            }
        }
        catch (System.Exception ex)
        {
			Debug.LogError(ex != null ? ex.Message : "<null>");
            return false;
        }

        return false;
    }

    public bool onRun()
    {
        _OnMove();
        return true;
    }

    void _OnMove()
    {
        //if (mvDeltaPos == Vector2.zero)
        //{
        //    return;
        //}

        //mvMoveDeltaPos.x += this.MoveSpeed;

        //if (mvMoveDeltaPos.x >= mvDeltaPos.x && this.MoveSpeed >= 0)
        //{
        //    mvMoveDeltaPos.x = mvDeltaPos.x;
        //    mvDeltaPos = Vector2.zero;
        //}
        //else if (mvMoveDeltaPos.x <= mvDeltaPos.x)
        //{         
        //    mvMoveDeltaPos.x = mvDeltaPos.x;
        //    mvDeltaPos = Vector2.zero;
        //}

        //float fx = mvMoveDeltaPos.x + mCameraRoot.position.x;
        //Vector3 v3 = new Vector3(fx, mCameraRoot.position.y, mCameraRoot.position.z);
        //mCameraRoot.position = v3;

        //float fDelta = Time.deltaTime - mvDeltaPos.x;
        //if (Time.deltaTime >= this.MoveSpeed || fDelta >= this.MoveSpeed)
        //{
        //    mvDeltaPos.x = Time.deltaTime;
        //    mvDeltaPos.y = 0.0f;
        //}
        //else
        //    mvDeltaPos.y += fDelta;

        do
        {
            if (mlistPos.Count == 0)
                break;

            Vector3 vTemp = mlistPos[0];
            Vector3 current = mCameraRoot.position;
            Vector3 direction = vTemp - current;
            float distance = Time.deltaTime * this.MoveSpeed;

            if (distance * distance >= direction.sqrMagnitude)
            {
                mlistPos.RemoveAt(0);
            }
            else
            {
                Vector3 newPosition = current + direction.normalized * distance;
            }
        } while (false);
    }

    bool _OnZoomIn(object obj)
    {
        if(!this.CanOperate)
            return false;

        CTwoFingerData cls = (CTwoFingerData)obj;
        //Logger.LogDebug("_OnZoomIn:{0}", fDis);
        try
        {
            Ray clsRay = mMainCamera.ScreenPointToRay(cls.vAveragePos);
            RaycastHit info;
            if (Physics.Raycast(clsRay, out info, float.MaxValue, 1 << LayerMask.NameToLayer("BoxCollision")) == false)
                return false;

            //ConsoleSelf.me.addText(string.Format("_OnZoomIn {0}", info.distance));
            if (info.distance >= this.MaxDistance)
                return false;

            float fDis = cls.fRealFingerDis - cls.fOldFingerDistance;
            float zoom = Time.deltaTime * fDis * this.MoveSpeed;

            Vector3 oldPosition = mCameraRoot.position;
            Vector3 direction = LineupReckon.QuaternionToDirection(mCameraRoot.rotation);
            Ray ray = new Ray(oldPosition, direction);
            Vector3 newPosition = ray.GetPoint(zoom);

            //mCameraRoot.position = newPosition;

            mlistPos.Add(newPosition);
        }
        catch (System.Exception ex)
        {
            return false;        	
        }

        return true;
    }

    bool _OnZoomOut(object obj)
    {
        if (!this.CanOperate)
            return false;

        CTwoFingerData cls = (CTwoFingerData)obj;
        //Logger.LogDebug("_OnZoomOut:{0}", fDis);
        try
        {
            Ray clsRay = mMainCamera.ScreenPointToRay(cls.vAveragePos);
            RaycastHit info;
            if (Physics.Raycast(clsRay, out info, float.MaxValue, 1 << LayerMask.NameToLayer("BoxCollision")) == false)
                return false;

            //ConsoleSelf.me.addText(string.Format("_OnZoomOut {0}", info.distance));
            if (info.distance <= this.MinDistance)
                return false;

            float fDis = cls.fRealFingerDis - cls.fOldFingerDistance;
            float zoom = Time.deltaTime * fDis * this.MoveSpeed;
            Vector3 oldPosition = mCameraRoot.position;
            Vector3 direction = LineupReckon.QuaternionToDirection(mCameraRoot.rotation);
            Ray ray = new Ray(oldPosition, direction);
            Vector3 newPosition = ray.GetPoint(zoom);

            //mCameraRoot.position = newPosition;
            mlistPos.Add(newPosition);
        }
        catch (System.Exception ex)
        {
			Debug.LogError(ex != null ? ex.Message : "<null>");
            return false;
        }

        return true;
    }

    bool _OnDrag(object obj)
    {
        if (!this.CanOperate)
            return false;

        CTwoFingerData cls = (CTwoFingerData)obj;

        //Collider cr = mMainCamera.collider;
        Ray clsRay = mMainCamera.ScreenPointToRay(cls.vAveragePos);
        RaycastHit info, infoex;
        if (Physics.Raycast(clsRay, out info, float.MaxValue, 1 << LayerMask.NameToLayer("BoxCollision")) == false)
            return false;

        Transform tfLeft = UICardMgr.findChild(mCameraRoot.parent, "CollisionObjRoot,CubeLeft");
        Transform tfRight = UICardMgr.findChild(mCameraRoot.parent, "CollisionObjRoot,CubeRight");

        Vector3 vDirLeft = mCameraRoot.position - tfLeft.position;
        Vector3 vDirRight = mCameraRoot.position - tfRight.position;
        bool bLeft = Physics.Raycast(mCameraRoot.position, vDirLeft, out info, 5f, (1 << 14) | (1 << 15));

        bool bRight = Physics.Raycast(mCameraRoot.position, vDirRight, out infoex, 5f, (1 << 14) | (1 << 15));


        bool bTempLeft = false;
        bool bTempRight = false;
        if(cls.vStartAveragePos.x > cls.vAveragePos.x)
            bTempLeft = true;
        if(cls.vStartAveragePos.x < cls.vAveragePos.x)
            bTempRight = true;

        //ConsoleSelf.me.addText(string.Format("_OnDrag {0}  L:{1}  {2}  R:{3}", info.distance, bTempLeft, infoex.distance, bTempRight));

        if (bRight && bTempRight)
            return false;

        if (bLeft && bTempLeft)
            return false;


        float zoom = -(Time.deltaTime * cls.vDelPos.x/* * this.MoveSpeed*/);
        float fx = zoom + mCameraRoot.position.x;
        Vector3 v3 = new Vector3(fx, mCameraRoot.position.y, mCameraRoot.position.z);
        //mCameraRoot.position = v3;

        mlistPos.Add(v3);
        return true;
    }

    public void onDrawGizmos()
    {
        this.RayLength = 5f;
        if (mCameraRoot == null)
            return;

        Transform tfLeft = UICardMgr.findChild(mCameraRoot.parent, "CollisionObjRoot,CubeLeft");
        Transform tfRight = UICardMgr.findChild(mCameraRoot.parent, "CollisionObjRoot,CubeRight");

        if (tfLeft && tfRight)
        {
            //Color cl = Gizmos.color;

            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(mCameraRoot.position, this.RayLength);
            Gizmos.DrawLine(mCameraRoot.position, tfLeft.position);

            Gizmos.color = Color.blue;
            //Gizmos.DrawWireSphere(TargetPosition, currentMaxDistance);
            Gizmos.DrawLine(mCameraRoot.position, tfRight.position);
        }
    }

    float targetYaw = 0f;
    public void IsLookAt(Transform tfTarget, bool isCamera = false)
    {
        if (!this.CanOperate)
            return;

        if (mSwitch == null)
        {
            Transform tfCame = UICardMgr.findChild(this.transform.parent, "Main Camera");
            if(tfCame != null)
                mSwitch = tfCame.GetComponent<MonoSwitchImp>();
        }
        if (mSwitch == null)
        {
            return;
        }

        if (isCamera)
        {
            mSwitch.Switching(tfTarget.position, tfTarget.rotation, this.TargetMoveSpeed);
        }
        else
        {
            mSwitch.Switching(tfTarget, this.TargetYaw, this.TargetPitch, this.TargetDistance, this.TargetMoveSpeed);
        }
    }
}
