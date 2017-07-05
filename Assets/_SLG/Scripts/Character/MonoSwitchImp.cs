using UnityEngine;
using System.Collections;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
public class MonoSwitchImp : MonoBehaviour
{
    private Transform camera;
    private float m_moveSpeed; // 移动速度
    private float m_angleSpeed; // 转动速度
    private float m_totalTime;

    private Vector3 m_position;
    private Quaternion m_rot;

    private float m_targetYaw;
    private float m_targetPitch;
    private float m_distance;
    private float m_beginTime;
    private Transform m_bindTarget = null;
    private bool m_isEnd = true;
    public bool IsEnd { get { return m_isEnd; } }
    public bool IsForce = false; // 强制更新
    public bool IsMovePosition = true;

    void Start()
    {
        camera = this.transform;
        m_isEnd = true;
        IsForce = false;

        this.m_position = this.transform.position;
        this.m_rot = this.transform.rotation;
        Logger.LogError("{0}   {1}", m_position, m_rot);
    }

    // 相机移动的位置与朝向,切换的总时长
    public void Switching(Vector3 positon, Quaternion rot, float totalTime)
    {
        m_position = positon;
        m_rot = rot;
        m_bindTarget = null;
        m_totalTime = totalTime;

        m_moveSpeed = 2 * (this/*camera*/.transform.position - positon).magnitude / m_totalTime;
        m_angleSpeed = Quaternion.Angle(this/*camera*/.transform.rotation, rot) / m_totalTime;
        IsForce = true;
        m_isEnd = false;
    }

    // 相机绑定的目标
    public void Switching(Transform bindTarget, float targetYaw, float targetPitch, float distance, float totalTime)
    {
        m_beginTime = Time.realtimeSinceStartup;
        m_bindTarget = bindTarget;
        m_targetYaw = targetYaw;
        m_targetPitch = targetPitch;
        m_distance = distance - 10;
        m_totalTime = totalTime;

        Vector3 newPosition = Vector3.zero;
        Quaternion newRotation = Quaternion.identity;
        GetNewValue(ref newPosition, ref newRotation);
		m_moveSpeed = 2 * (this/*camera*/.transform.position - newPosition).magnitude / m_totalTime;
        m_angleSpeed = Quaternion.Angle(this/*camera*/.transform.rotation, newRotation) / m_totalTime;
        IsForce = true;
        m_isEnd = false;
    }

    private void GetNewValue(ref Vector3 newPostion, ref Quaternion newRotation)
    {
        Vector3 _offset = new Vector3(0, 0, -m_distance);
        _offset = Quaternion.Euler(m_targetPitch, m_targetYaw, 0) * _offset;

        Vector3 lastPosition = this/*camera*/.transform.position;
        Quaternion lastRotation = this/*camera*/.transform.rotation;

        this/*camera*/.transform.position = m_bindTarget.position + _offset;
        this/*camera*/.transform.LookAt(m_bindTarget.position);

        newPostion = this/*camera*/.transform.position;
        newRotation = this/*camera*/.transform.rotation;

        this/*camera*/.transform.position = lastPosition;
        this/*camera*/.transform.rotation = lastRotation;
    }

    public bool update()
    {
        Vector3 newPosition = Vector3.one;
        Quaternion newRotation = Quaternion.identity;
        if (m_bindTarget != null)
        {
            Vector3 _offset = new Vector3(0, 0, -m_distance);
            _offset = Quaternion.Euler(m_targetPitch, m_targetYaw, 0) * _offset;

            GetNewValue(ref newPosition, ref newRotation);
        }
        else
        {
            newPosition = m_position;
            newRotation = m_rot;
        }

        //             float time = m_totalTime - (Time.realtimeSinceStartup - m_beginTime);
        //             m_moveSpeed = (camera.transform.position - m_bindTarget.transform.position).magnitude / time;
        //             m_angleSpeed = Quaternion.Angle(camera.transform.rotation, newRotation) / time;

        return Set(newPosition, newRotation);
    }

    protected bool Set(Vector3 newPosition, Quaternion newRotation)
    {
        bool bEnd = true;
        // 位置
        if(this.IsMovePosition)
        {
            Vector3 current = this/*camera*/.transform.position;
            Vector3 direction = newPosition - current;
            float distance = Time.deltaTime * m_moveSpeed;
            if (distance * distance >= direction.sqrMagnitude)
            {
                // 超过了当前距离
            }
            else
            {
                // 还未超过
                newPosition = current + direction.normalized * distance;
                bEnd = false;
            }

            this/*camera*/.transform.position = newPosition;
        }

        // 朝向
        {
            Quaternion current = this/*camera*/.transform.rotation;
            float angleValue = Time.deltaTime * m_angleSpeed;
            float totalAngle = Quaternion.Angle(current, newRotation);
            if (angleValue >= totalAngle)
            {

            }
            else
            {
                newRotation = Quaternion.Lerp(current, newRotation, angleValue / totalAngle);
                bEnd = false;
            }

            this/*camera*/.transform.rotation = newRotation;
        }

        return bEnd;
    }

    void Update()
    {
        if (m_isEnd == false || IsForce == true)
        {
            IsForce = false;
            m_isEnd = update();
        }
    }
}
