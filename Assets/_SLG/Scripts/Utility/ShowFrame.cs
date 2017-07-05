using UnityEngine;
using System.Collections;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
public class ShowFrame : MonoBehaviour 
{
	

	public  float updateInterval = 0.5F;
	
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	
	void Start()
	{
		timeleft = updateInterval;  
	}

	string m_FPS;
	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 )
		{
			// display two fractional digits (f2 format)
			float fps = accum/frames;
			string format = System.String.Format("{0:F2} FPS",fps);
			m_FPS = format;

			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}

	GUIStyle style = new GUIStyle();
	void OnGUI()
	{
//		style.fontSize = 35;
//	
//		GUI.color = Color.red;
//		GUI.Label(new Rect(10,Screen.height - 40,100,30),m_FPS,style);
	}

}
