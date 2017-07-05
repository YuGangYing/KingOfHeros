using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//-
public class UICardWinMgr : MonoBehaviour
{
#region class defined

    public enum enCardWinType
    {
        enCWT_Invalid = -1,
        enCWT_HeroList = 0,
        enCWT_IllustratedList,
        enCWT_HeroDetail,
        enCWT_IllustratedDetail,
        enCWT_LevelUpdate,
        enCWT_QualityUpdate,
        enCWT_Amount,
    }

    [System.Serializable]
    public class UIWinBaseData
    {
        public enCardWinType menWinId = enCardWinType.enCWT_Invalid;
        public List<GameObject> mlistPerfabGameObj = null;

        public UIWinBaseData()
        {
            mlistPerfabGameObj = new List<GameObject>();
        }


    }

#endregion

#region variable defined

    public List<UIWinBaseData> mlistWin = null;
    
#endregion

	// Use this for initialization
	void Start () 
    {
        mlistWin = new List<UIWinBaseData>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public bool isWinById(enCardWinType en)
    {
        foreach (UIWinBaseData itemWin in mlistWin)
        {
            if (itemWin.menWinId == en)
                return true;
        }

        return false;
    }

    public bool setWinVisibleById(enCardWinType en, bool bVisible)
    {
        foreach (UIWinBaseData itemWin in mlistWin)
        {
            if (itemWin.menWinId == en)
            {
                
                return true;
            }
        }

        return false;
    }
}
