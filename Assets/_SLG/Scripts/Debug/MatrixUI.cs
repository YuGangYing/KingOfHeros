using UnityEngine;
using System.Collections;

//兵种类型 
public enum SoldierType
{ 
    Peltast = 1 ,    //盾兵
    pikeman,         //枪兵 
    Archers,         //弓箭兵 
    cavalry,         //骑兵  
} 

public class MatrixUI
{  
    //最大行数 
	public float m_fMaxRow = 10;
    
    //最小行数 
	public float m_fMinRow = 2;

    //行数 
	public float m_fRowValue = 4; 

    //最大列数 
	public float m_fMaxCol = 10;

    //最小列数 
	public float m_fMinCol = 2;

    //列数
	public float m_fColValue = 5; 

    //最大攻击力 
	public float m_fMaxAttck = 20;

    //最小攻击力 
	public float m_fMinAttack = 1;

    //攻击力 
	public float m_fAttackValue = 1;

	public PopList m_PopList = new PopList();

    //兵种类型 
	public SoldierType m_SoldierType = SoldierType.Archers;

    //是否被选中 
    public bool m_bSelect = false;

    public void OnGUI()
    {
        //我方方阵 
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3;  ++j)
            {
                DrawSetPlane(j*200, i*200);
            }
        } 
    } 

    //绘制方阵设置面板 
    public void DrawSetPlane(float _x, float _y)
    { 
        m_bSelect = GUI.Toggle(new Rect(5 + _x, 5 + _y, 100, 30), m_bSelect, "方阵");

        //行数 
        GUI.Label(new Rect(5 + _x, 35 + _y, 50, 30), "行数");
        GUI.Label(new Rect(40 + _x, 35 + _y, 50, 30), ((int)m_fRowValue).ToString());
        m_fRowValue = GUI.HorizontalSlider(new Rect(5 + _x, 60 + _y, 100, 30), m_fRowValue, m_fMinRow, m_fMaxRow);

        //列数 
        GUI.Label(new Rect(5 + _x, 80 + _y, 50, 30), "列数");
        GUI.Label(new Rect(40 + _x, 80 + _y, 50, 30), ((int)m_fColValue).ToString());
        m_fColValue = GUI.HorizontalSlider(new Rect(5 + _x, 105 + _y, 100, 30), m_fColValue, m_fMinCol, m_fMaxCol);

        //攻击力 
        GUI.Label(new Rect(5 + _x, 125 + _y, 50, 30), "攻击力");
        GUI.Label(new Rect(50 + _x, 125 + _y, 50, 30), ((int)m_fAttackValue).ToString());
        m_fAttackValue = GUI.HorizontalSlider(new Rect(5 + _x, 145 + _y, 100, 30), m_fAttackValue, m_fMinAttack, m_fMaxAttck);

        //兵种 
        m_PopList.DrawPopList(ref m_SoldierType, 5 + _x, 135 + 30 + _y); 
    }

    //获取行数 
    public int GetRow()
    {
        return (int)m_fRowValue;
    }

    //获取列数 
    public int GetCol()
    {
        return (int)m_fColValue;
    }

    //获取攻击力 
    public int GetAttackValue()
    {
        return (int)m_fAttackValue;
    }

    //设置士兵类型 
    public void SetSoldierType( SoldierType _type)
    {
        m_PopList.SetSoldierType(ref m_SoldierType, _type);
    }
}
