using UnityEngine;
using System.Collections;

public class CityTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.H))
		{
			Application.LoadLevel("Battlefield");
		}
	}
}
