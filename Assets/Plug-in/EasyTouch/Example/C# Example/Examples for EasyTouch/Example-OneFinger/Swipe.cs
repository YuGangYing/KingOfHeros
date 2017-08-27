using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swipe : MonoBehaviour {

	private TextMesh textMesh;
	private GameObject trail;
    List<Vector2> listPt = new List<Vector2>();

	// Subscribe to events
	void OnEnable(){
		EasyTouch.On_SwipeStart += On_SwipeStart;
		EasyTouch.On_Swipe += On_Swipe;
		EasyTouch.On_SwipeEnd += On_SwipeEnd;		
	}

	void OnDisable(){
		UnsubscribeEvent();
		
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SwipeStart -= On_SwipeStart;
		EasyTouch.On_Swipe -= On_Swipe;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;	
	}
	
	void Start(){
		textMesh = GameObject.Find("LastSwipeText").transform.gameObject.GetComponent("TextMesh") as TextMesh;
	}
	
	// At the swipe beginning 
	private void On_SwipeStart( Gesture gesture){
		
		// Only for the first finger
		if (gesture.fingerIndex==0 && trail==null){ 
			
			// the world coordinate from touch for z=5
			Vector3 position = gesture.GetTouchToWordlPoint(5);
			trail = Instantiate( Resources.Load("Trail"),position,Quaternion.identity) as GameObject;

            listPt.Add(gesture.position);
		}
	}
	
	// During the swipe ddd
	private void On_Swipe(Gesture gesture){
		
		if (trail!=null){
			
			// the world coordinate from touch for z=5
			Vector3 position = gesture.GetTouchToWordlPoint(5);
			trail.transform.position = position;

            listPt.Add(gesture.position);
		}
	}
	
	// At the swipe end 
	private void On_SwipeEnd(Gesture gesture){
		
		if (trail!=null){
			Destroy(trail);
            listPt.Add(gesture.position);			
			// Get the swipe angle
			float angles = gesture.GetSwipeOrDragAngle();
			textMesh.text = "Last swipe : " + gesture.swipe.ToString() + " /  vector : " + gesture.swipeVector.normalized + " / angle : " + angles.ToString("f2") + " / " + gesture.deltaPosition.x.ToString("f5");
		}
				
	}
}
