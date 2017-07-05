using UnityEngine;
using System.Collections;

public class DestoryForTime : MonoBehaviour 
{
    public float fTime = 2.5f;
    public object mObj = null;

	// Use this for initialization
	void Start ()
    {
        Destroy(this.gameObject, this.fTime);
	}
	
	// Update is called once per frame
// 	void Update () {
// 	
// 	}

    void OnDestroy()
    {
        SLG.EventArgs eObj = new SLG.EventArgs((object)this.mObj);
        SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyChangeFightState, eObj);
    }
}
