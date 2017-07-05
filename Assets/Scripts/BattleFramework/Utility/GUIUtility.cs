using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ZhiWuDaZhan{

	public static class GUIUtility {
		
		static public GUIStyle GetErrorGUIStyle()
		{
			GUIStyle errorLabelStyle = new GUIStyle ();
			errorLabelStyle.normal.textColor = Color.red;
			errorLabelStyle.fontStyle = FontStyle.Bold;
			errorLabelStyle.richText = true;
			return errorLabelStyle;
		}

	}

}
