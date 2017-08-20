using UnityEngine;
using System.Collections;

public class SimpleMoveForward : MonoBehaviour 
{
    [SerializeField]
    private float speed = 1;

	void Update ()
	{
	    transform.position += transform.forward * (speed * Time.deltaTime);
	}
}
