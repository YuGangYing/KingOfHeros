using UnityEngine;
using System.Collections;

public delegate void initFunc();
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
public class PanelMouse : MonoBehaviour
{
    public Transform mgoTraget = null;
    public Transform mgoComponent = null;

    public float speed = 1f;

    public bool mbUpdate = false;
    public ChgAnima ca;

    public initFunc initCallBack = null;

	// Use this for initialization
	void Start () 
    {
        mgoTraget = this.transform.parent.FindChild("SBD");
        mgoComponent = mgoTraget.FindChild("SBD");
        ca = mgoComponent.GetComponent<ChgAnima>();
        if (initCallBack != null)
            initCallBack();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (mbUpdate)
        {
            mbUpdate = false;
            try
            {
                //ca.setRandomAnima();
            }
            catch (System.Exception ex)
            {
                ConsoleSelf.me.addText("UICardLogic::OnItemDetailClick  action is not finding");
            }
        }
	}

    void OnDrag (Vector2 delta)
    {
        if (mgoTraget != null)
        {
            mgoTraget.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * mgoTraget.localRotation;
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * transform.localRotation;
        }
    }
}
