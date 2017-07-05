using UnityEngine;
using System.Collections;

public class SimpleCameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    Vector2 mStartMousePos;
    Vector2 mCurrentMousePos;
    public float mouseSpeed = 1;

    Vector3 mStartCameraPos;
	// Update is called once per frame
	void Update () {
        #region mouse move controll
        if(Input.GetMouseButtonDown(1))
        {
            mStartMousePos = Input.mousePosition;
            mStartCameraPos = GetComponent<Camera>().transform.position;
        }
        if(Input.GetMouseButton(1))
        {
            mCurrentMousePos = Input.mousePosition;

            GetComponent<Camera>().transform.position = mStartCameraPos + (Vector3)(mCurrentMousePos - mStartMousePos);
        }
        #endregion
	}
}
