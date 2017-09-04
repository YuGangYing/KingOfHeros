using UnityEngine;
using System.Collections;

public class GuiStartMenu : MonoBehaviour {
	
	void OnEnable(){	
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}
	
	void OnGUI() {
	            
		GUI.matrix = Matrix4x4.Scale( new Vector3( Screen.width / 1024.0f, Screen.height / 768.0f, 1 ) );
		
		GUI.Box( new Rect( 0, -4, 1024, 70 ), "" );
		
	}
	
	void On_SimpleTap( Gesture gesture){
	
		if ( gesture.pickObject!=null){
			string levelName= gesture.pickObject.name;

			if (levelName=="OneFinger")
				UnityEngine.SceneManagement.SceneManager.LoadScene("Onefinger");
			else if (levelName=="TwoFinger")		
				UnityEngine.SceneManagement.SceneManager.LoadScene("TwoFinger");
			else if (levelName=="MultipleFinger")		
				UnityEngine.SceneManagement.SceneManager.LoadScene("MultipleFingers");	
			else if (levelName=="MultiLayer")
				UnityEngine.SceneManagement.SceneManager.LoadScene("MultiLayers");
			else if (levelName=="GameController")
				UnityEngine.SceneManagement.SceneManager.LoadScene("GameController");
			else if (levelName=="FreeCamera")
				UnityEngine.SceneManagement.SceneManager.LoadScene("FreeCam");			
			else if (levelName=="ImageManipulation")
				UnityEngine.SceneManagement.SceneManager.LoadScene("ManipulationImage");
			else if (levelName=="Joystick1")
				UnityEngine.SceneManagement.SceneManager.LoadScene("FirstPerson-DirectMode-DoubleJoystick");		
			else if (levelName=="Joystick2")
				UnityEngine.SceneManagement.SceneManager.LoadScene("ThirdPerson-DirectEventMode-DoubleJoystick");
			else if (levelName=="Button")
				UnityEngine.SceneManagement.SceneManager.LoadScene("ButtonExample");			
			else if (levelName=="Exit")
				Application.Quit();						
		}
		
	}
}
