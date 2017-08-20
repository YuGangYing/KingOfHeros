using UnityEngine;
using System.Collections;
using UI;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
public class UIBgNullMsgLogic : MonoBehaviour
{
    public delegate void FunCallBack();
    public FunCallBack mfunClick = null;

	public string mstrActionFun = "Click";

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnClick()
	{
		Logger.LogDebug("UIBgNullMsgLogic::OnClick");

		try
		{
            if (mfunClick != null)
                mfunClick();
			//UICardMgr.singleton.gameObject.SendMessage(mstrActionFun);
		}
		catch (System.Exception ex)
		{
			
		}
	}
}
