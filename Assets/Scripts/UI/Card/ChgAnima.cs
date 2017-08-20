using UnityEngine;
using System.Collections.Generic;

//[ExecuteInEditMode]
//[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Interaction/ChgArmor")]
public class ChgAnima :/* UIEquipmentSlot*/MonoBehaviour
{
    public CSolider mSolider = null;

    public GameObject mObjEffect = null;
    public GameObject mEffectParent = null;

    void Start()
    {
        //(CSolider)transform.GetComponent("CSolider");
        if (mSolider != null)
        {
            //Debug.Log("set Status = ChgAnima start 0000 ");
            //mSolider.setStatus(CSolider.HERO_STATUS.ST_IDLE);
            //InvokeRepeating("onTimer", 0.3f, 1.3f);
        }
    }

    public void setRandomAnima(bool bCreateEffect = false)
    {
        if (mSolider == null)
            return;

        int n = Random.Range(3, 5);
        CSolider.HERO_STATUS en = (CSolider.HERO_STATUS)n;

        mSolider.setStatus(en);

        //InvokeRepeating("onTimer", 1.6f, 0.3f);
        Invoke("onTimer", 1.3f);

        if (bCreateEffect)
        {
            createEffect();
        }
    }

    void onTimer()
    {
        //mSolider.setStatus(CSolider.HERO_STATUS.ST_IDLE);
        //mSolider.animation.wrapMode = WrapMode.Loop;
    }

    void Destroy()
    {
    }

    void Update()
    {
        //Transform parentPanel = transform.parent.parent;
        //UIAnchor pos = (UIAnchor)transform.GetComponent("UIAnchor");
        //Debug.Log("set Status = beg  " + pos.position);
        //transform.position = new UnityEngine.Vector3();

        //pos.relativeOffset.x = -0.18f;
        //pos.relativeOffset.y = -0.38f;


        
        //transform.rotation = new UnityEngine.Quaternion();

        //foreach (Transform item in parentPanel)
        //{
        //    GameObject itemGo = item.gameObject;

        //    if (itemGo.name == "UIModel")
        //    {
        //        Transform itemModel = itemGo.transform;
        //        Transform OrcPivot = itemModel.GetChild(1);
        //        if (OrcPivot != null)
        //        {
        //            OrcPivot.GetChild(1).position = new UnityEngine.Vector3(0.2f, 0.1f, 0.0f);
        //            OrcPivot.GetChild(1).rotation = new UnityEngine.Quaternion();

        //            break;
        //        }
        //    }
        //}

        //Debug.Log("set Status = Update");
    }

    public void createEffect()
    {
        if (mObjEffect != null && mEffectParent != null)
        {
            NGUITools.AddChild(mEffectParent, mObjEffect);
        }
    }






}