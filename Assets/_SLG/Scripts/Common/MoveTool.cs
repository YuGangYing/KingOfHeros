/*
* 文件名称：MoveTool.cs 
* 摘    要：gameobject移动行为工具  
* 作    者：龚敬 
* 完成日期：2014年3月23日   
*/

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
//朝向 
public enum ORIENTATION
{
    //前
    Forward,

    //后 
    Backward,

    //左
    Left,

    //右
    Right
}

//路径数组信息 
[System.Serializable]
public class WayPointArrayInfo
{
    //路径数组索引 
    public int m_iIndex;

    //路径数组 
    public  List<Vector3> m_WayPointArray;
}

public class MoveTool : Singleton<MoveTool> 
{ 
    //物体正面朝向旋转到指定的目标点 
    //参数1：自己位置组件 
    //参数2: 目标点位置组件 
    //参数3：旋转速度 
    public void RotateToTagetPoint(Transform _Src, Vector3 _des, float _rotatespeed)
    {
        if (_Src == null || _des == null)
        {
            return;
        }

        if (_rotatespeed < 0)
        {
            Debug.LogError("旋转速度参数不合法");

            return;
        }

        Quaternion rotation = Quaternion.LookRotation(_des - _Src.position);

        _Src.rotation = Quaternion.Slerp(_Src.rotation, rotation, Time.deltaTime * _rotatespeed);
    }

    //移动(物体朝向：前方,后方,左方,右方)
    //参数1:位置组件 
    //参数2:朝向 
    //参数3：速度 
    public void MoveByOrientation(Transform _src, ORIENTATION _orientation, float _speed)
    {
        if (_src == null)
        {
            return;
        }

        if (_speed < 0)
        {
            Debug.LogError("速度参数不合法");

            return;
        }
        switch (_orientation)
        {
            case ORIENTATION.Forward:
                _src.Translate(Vector3.forward * Time.deltaTime * _speed);
                break;
            case ORIENTATION.Backward:
                _src.Translate(Vector3.back * Time.deltaTime * _speed);
                break;
            case ORIENTATION.Left:
                _src.Translate(Vector3.left * Time.deltaTime * _speed);
                break;
            case ORIENTATION.Right:
                _src.Translate(Vector3.right * Time.deltaTime * _speed);
                break;
        }
    }

    //向目标移动
    //参数1:自己位置组件 
    //参数2:目标位置组件
    //参数3:速度 
    //参数4:角速度 
    public void MoveTarget(Transform _src, Vector3 _target, float _speed, float _anglespeed)
    {
        if (_target == null || _src == null)
        {
            return;
        }

        if (_speed < 0)
        {
            Debug.LogError("速度参数不合法");

            return;
        }

        RotateToTagetPoint(_src, _target, _anglespeed);

        MoveByOrientation(_src, ORIENTATION.Forward, _speed);
    }

    //移动（角色控制器）
    public void MoveTargetByController(CharacterController _c,Transform _src, Vector3 _target, float _speed, float _anlespeed)
    {
        if (_c == null)
        {
            Logger.LogDebug("_c is null");

            return;
        }

        RotateToTagetPoint(_src, _target, _anlespeed);

        Vector3 tmp_dir = (_target - _src.position);

        _c.Move(tmp_dir * _speed * Time.deltaTime);
    }

    //向一个点移动  
    public void MoveToPoint(Transform _src, Vector3 _Pos)
    {
        Vector3 dir = (_Pos - _src.position).normalized;

        if (_src == null)
        {
            Logger.LogDebug("_src is null");

            return;
        }

        _src.position += dir * Time.deltaTime *2;
    }

    //检测是否在半径范围内 
    public bool CheckRadiusIn(Vector3 _src, Vector3 _des, float _radius)
    {
        float distiance = Vector3.Distance(_des, _src); 

        if (distiance <= _radius)
        {
            return true;
        }

        return false;
    }

    //根据所给的路径点列表来移动目标 
    //参数1:路径数组信息 
    //参数2:需要移动的gameobject的位置组件  
    //参数3:移动速度 
    //参数4：离将要移动的点到达某个距离的时候切换到下一个点 
    //返回值:true 路径移动完成,false 路径移动未完成 
    public bool MoveByWayPoints(WayPointArrayInfo _info, Transform _src, float _movespeed, float _distance)
    {
        if (_info == null)
        {
            Debug.LogError("_info is null");

            return false;
        }

        if (_src == null)
        {
            Debug.LogError("_src is null");

            return false;
        }

        if (_movespeed == 0)
        {
            Debug.LogError("_movespeed is Error");

            return false;
        }

        if (_info.m_iIndex < 0 || _info.m_iIndex > _info.m_WayPointArray.Count - 1)
        {
            Debug.LogError("_info.m_iIndex is out of array!!! + index is " + _info.m_iIndex);

            return false;
        }

        //向目标点移动  
        MoveTarget(_src, _info.m_WayPointArray[_info.m_iIndex], _movespeed, 10); 

        Vector3 divc3 = _info.m_WayPointArray[_info.m_iIndex] - _src.position;

        float dic = Vector3.Magnitude(divc3);

        if (dic <= _distance)
        {
            _info.m_iIndex++;

            if (_info.m_iIndex > _info.m_WayPointArray.Count - 1)
            {
                _info.m_iIndex = _info.m_WayPointArray.Count - 1;

                return true;
            }
        }

        
     
        return false;
    }

    public bool MoveByWayPointsByContoller(CharacterController _c, WayPointArrayInfo _info, Transform _src, float _movespeed, float _distance)
    {
        if (_c == null)
        { 
            Logger.LogDebug("_c is null");

            return false;
        }

        if (_info == null)
        { 
            Logger.LogDebug("_info is null");

            return false;
        }

        if (_src == null)
        {
            Logger.LogDebug("_src is null");

            return false;
        }

        if (_movespeed == 0)
        { 
            Logger.LogDebug("_movespeed is Error");

            return false;
        }

        if (_info.m_iIndex < 0 || _info.m_iIndex > _info.m_WayPointArray.Count - 1)
        { 
            Logger.LogDebug("_info.m_iIndex is out of array!!! + index is " + _info.m_iIndex);

            return false;
        }

        if (CheckRadiusIn(_src.position, _info.m_WayPointArray[_info.m_iIndex], _distance))
        {
            _info.m_iIndex++;

            if (_info.m_iIndex > _info.m_WayPointArray.Count - 1)
            {
                // _info.m_iIndex = 0;
                _info.m_iIndex = _info.m_WayPointArray.Count - 1;

                return true;
            }
        }

        //向目标点移动   
        MoveTargetByController(_c, _src, _info.m_WayPointArray[_info.m_iIndex], _movespeed, 10);

        return false;
    } 
}
