using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
public class ExpToLose : MonoBehaviour
{
    public bool isTiming = false;

    GameObject expList;
    GameObject loseList;
    Transform expGrid;

    float cumulativeTime = 0;
    float countTime = 2;
    float showTime = 3;
    int times = 1;
    float countCTime = 0;
    bool isChange = false;

	// Use this for initialization
	void Start () {
        expList = transform.FindChild("expList").gameObject;
        loseList = transform.FindChild("loseList").gameObject;
        expGrid = expList.transform.FindChild("Grid");
	}
	
	// Update is called once per frame
	void Update () {
        if (isTiming)
        {
            cumulativeTime += Time.deltaTime;

            if (cumulativeTime > showTime + 2)
            {
                expList.SetActive(false);
                loseList.SetActive(true);
                cumulativeTime = 0;
                isTiming = false;
            }

            foreach (UIBattleResult.ExpItem ei in DataManager.getBattleUIData().battleResult.expItemList)
            {
                int nExp = int.Parse(ei.exp.text);
                float rateTime = 0;

                if (ei.he.exp != 0)
                {
                    rateTime = countTime / ei.he.exp;
                }
                else
                {
                    return;
                }

                if (cumulativeTime > rateTime * times)
                {
                    if (nExp < ei.he.exp)
                    {
                        ++nExp;
                        ei.exp.text = "+" + nExp.ToString();
                        isChange = true;
                    }
                }
            }

            if (isChange)
            {
                ++times;
                isChange = false;
            }
 
        }
        
	}
}
