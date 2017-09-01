using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KOH
{
	public class CameraController :SingleMonoBehaviour<CameraController>
	{

		public Camera camera;

		protected override void Awake ()
		{
			base.Awake ();
		}

		bool mIsPress = false;
		public float moveSpeed = 10f;

		void LateUpdate ()
		{
			Move ();
		}

		void Move ()
		{
			if (Input.GetKey (KeyCode.A)) {
				Vector3 left = new Vector3 (transform.right.x,0, transform.right.z).normalized * -moveSpeed * Time.deltaTime;
				transform.position += left;
			}
			if (Input.GetKey (KeyCode.S)) {
				Vector3 down = new Vector3 (transform.forward.x,0, transform.forward.z).normalized * -moveSpeed * Time.deltaTime;
				transform.position += down;
			}
			if (Input.GetKey (KeyCode.D)) {
				Vector3 right = new Vector3 (transform.right.x,0, transform.right.z).normalized * moveSpeed * Time.deltaTime;
				transform.position += right;
			}
			if (Input.GetKey (KeyCode.W)) {
				Vector3 up = new Vector3 (transform.forward.x,0, transform.forward.z).normalized * moveSpeed * Time.deltaTime;
				transform.position += up;
			}
		}
	}
}
