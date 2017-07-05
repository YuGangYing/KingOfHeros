using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//下拉框状态 
public enum PopListState
{ 
    Open,   //打开 

    Close,  //关闭 
}



//下拉框 
public class PopList
{	 
    //状态
    private PopListState m_State = PopListState.Close;

    //当前名字 
    private string m_sCurName = "弓箭兵";

    public void DrawPopList(ref SoldierType _type, float _x, float _y)
    { 
        switch (m_State)
        {
            case PopListState.Close:
                {
                    if (GUI.Button(new Rect(_x, _y, 100, 30), m_sCurName))
                    {
                        SetState(PopListState.Open);
                    } 
                }
                break;
            case PopListState.Open:
                { 
                    if (GUI.Button(new Rect(_x, _y, 100, 30), "弓箭兵"))
                    {
                        SetState(PopListState.Close);

                        m_sCurName = "弓箭兵";

                        _type = SoldierType.Archers;
                    }

                    if (GUI.Button(new Rect(_x, 30 + _y, 100, 30), "骑兵"))
                    {
                        SetState(PopListState.Close);

                        m_sCurName = "骑兵";

                        _type = SoldierType.cavalry;
                    }

                    if (GUI.Button(new Rect(_x, 60 + _y, 100, 30), "盾兵"))
                    {
                        SetState(PopListState.Close);

                        m_sCurName = "盾兵";

                        _type = SoldierType.Peltast;
                    }

                    if (GUI.Button(new Rect(_x, 90 + _y, 100, 30), "枪兵"))
                    {
                        SetState(PopListState.Close);

                        m_sCurName = "枪兵";

                        _type = SoldierType.pikeman;
                    }
                }
                break;
        }
       
        
    }

    //设置状态 
    public void SetState(PopListState _state)
    {
        m_State = _state;
    }

    //设置兵种类型 
    public void SetSoldierType(ref SoldierType _t ,SoldierType _type)
    {
        switch (_type)
        {
            case SoldierType.Archers:
                m_sCurName = "弓箭兵";
                break;
            case SoldierType.cavalry:
                m_sCurName = "骑兵";
                break;
            case SoldierType.Peltast:
                m_sCurName = "枪兵";
                break;
            case SoldierType.pikeman:
                m_sCurName = "盾兵";
                break;
        }

        _t = _type;
    }

}
