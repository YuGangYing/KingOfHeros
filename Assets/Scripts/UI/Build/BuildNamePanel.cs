using UnityEngine;
using System.Collections;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
public class BuildNamePanel : MonoBehaviour
{
    public UILabel labelName;
    public Camera camera;

	// Use this for initialization
	void Start ()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(camera.transform);

        float dis = Vector3.Distance(PutBuild.me.camera.transform.position, transform.position);
        float scale = dis / 200;
        transform.localScale = new Vector3(scale, scale, scale);
	}

    public void SetBuildName (string strBuildName)
    {
        labelName.text = strBuildName;
    }
}
