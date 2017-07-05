using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
public class UIHeroRoot : MonoBehaviour
{
    #region defined constance variable

    [SerializeField]
    float mfScale = 3.0f;
    [SerializeField]
    float mfIncrease = 0.0f;
    [SerializeField]
    float mfInc = 1.0f;
    [SerializeField]
    float mfOriginal = 0.7f;

    private bool mbScale = true;
    private int mnCurrentIndex = CConstance.INVALID_ID;
    private int mnCurSelectIndex = CConstance.INVALID_ID;

    private Vector3 mVRecordScale = new Vector3();


    // 视图摄像机
    UIDraggableCamera mDraggableViewCamera = null;

    #endregion

	// Use this for initialization
	void Start ()
    {
        //CardHeroListPanel chlp = (CardHeroListPanel)PanelManage.me.getPanel(PanelID.CardHeroListPanel);
        //if (chlp != null)
        //{
        //    mDraggableViewCamera = chlp.dragViewCamera;
        //}
	}

    public void setCamera(ref UIDraggableCamera uidc)
    {
        mDraggableViewCamera = uidc;
    }
	
	// Update is called once per frame
	void Update ()
    {
        _ProcessViewCameraAndItemDistance();
	}

    void _ProcessViewCameraAndItemDistance()
    {
        if (UICardMgr.singleton.itemAmount == 0)
        {
            return;
        }

        if (mDraggableViewCamera == null)
            return;

        if (mDraggableViewCamera.GetComponent<Camera>() == null)
            return;

        mnCurrentIndex = CConstance.INVALID_ID;

        Camera ca = mDraggableViewCamera.GetComponent<Camera>();
        List<float> listDistance = new List<float>();

        for (int i = 0; i < UICardMgr.singleton.itemAmount; i++)
        {
            Vector3 v3 = ca.transform.position - UICardMgr.singleton.getItemByIndex(i).goIcon.transform.position;
            float f = _OnPower(v3);

            listDistance.Add(f);
        }

        List<float> listTemp = new List<float>();
        for (int i = 0; i < listDistance.Count; i++)
        {
            listTemp.Add(listDistance[i]);
        }

        // 排序 距离最近放在前面
        //		Logger.LogDebug(listTemp.Count.ToString());
        for (int i = 0; i < listTemp.Count; i++)
        {
            for (int j = 0; j < listTemp.Count - i - 1; j++)
            {
                if (listTemp[j] >= listTemp[j + 1])
                {
                    float temp = listTemp[j];
                    listTemp[j] = listTemp[j + 1];
                    listTemp[j + 1] = temp;
                }
            }
        }

        // calculate scale
        for (int j = 0; j < listDistance.Count; j++)
        {
            if (listTemp[0] == listDistance[j])
            {
                mnCurrentIndex = j;
                //UICardMgr.singleton.getItemByIndex(j).goIcon.transform.localScale = new UnityEngine.Vector3(mfIncrease, mfIncrease, mfIncrease);
                Vector3 newV3 = new Vector3(mfIncrease + mfInc,
                                            mfIncrease + mfInc,
                                            mfIncrease + mfInc);

                /*Logger.LogDebug("   j:" + j.ToString() + "  newV3:" + newV3.ToString());*/

                UICardMgr.singleton.getItemByIndex(j).goIcon.transform.localScale = newV3;
                UICardLogic ucl = UICardMgr.singleton.getItemByIndex(j).goIcon.GetComponent<UICardLogic>();
                if (ucl != null)
                    ucl.DoSelect = true;

                break;
            }
        }

        // reset depth
        for (int j = 0; j < UICardMgr.singleton.itemAmount; j++)
        {
            GameObject go = UICardMgr.singleton.getItemByIndex(j).goIcon;
            UISprite uis = go.transform.FindChild("Sprite").GetComponent<UISprite>();

            if (mnCurrentIndex == j)
            {
                uis.depth = 3;
                UISprite uisStar = UICardMgr.FindChild<UISprite>(uis.transform, "Select");
                uisStar.depth = 2;
                uisStar = UICardMgr.FindChild<UISprite>(uis.transform, "Star");
                uisStar.depth = 4;
                continue;
            }
            else
            {
                uis.depth = 1;
                UISprite uisStar = UICardMgr.FindChild<UISprite>(uis.transform, "Select");
                uisStar.depth = 1;
                uisStar = UICardMgr.FindChild<UISprite>(uis.transform, "Star");
                uisStar.depth = 2;
            }

            UICardLogic ucl = go.GetComponent<UICardLogic>();
            if(ucl != null)
                ucl.resetBgColor();

            go.transform.localScale = new UnityEngine.Vector3(mfOriginal, mfOriginal, mfOriginal);
        }

    }

    float _OnPower(Vector3 v3)
    {
        float fx = UnityEngine.Mathf.Pow(v3.x, 2.0f);
        float fy = UnityEngine.Mathf.Pow(v3.y, 2.0f);
        float fz = UnityEngine.Mathf.Pow(v3.z, 2.0f);

        //Logger.LogDebug("   x:" + fx.ToString() + "  y:" + fy.ToString() + "  z:" + fz.ToString());
        float fTotal = fx + fy + fz;

        return fTotal;
    }

    void _OnScale()
    {
        if (mnCurrentIndex == CConstance.INVALID_ID)
        	return;
        
        if (!mbScale)
        	return;
        
        mVRecordScale.x += Time.deltaTime * 10.0f;
        if (mfScale <= mVRecordScale.x)
        {
        	mVRecordScale.x = mfScale;
        	mbScale = false;
        	
        	Logger.LogDebug(" _onScale:    mbScale = false");
        }

        UICardMgr.singleton.getItemByIndex(mnCurrentIndex).goIcon.transform.localScale = mVRecordScale;
    }
}
