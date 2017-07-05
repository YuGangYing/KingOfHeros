using UnityEngine;
using UnityEngine.UI;  			//UI命名空间
using UnityEngine.EventSystems;	//事件系统命名空间
using System.Collections;
using System.Collections.Generic;

public class DataFunctionClass : MonoBehaviour
{

	public static bool GetBoolValue (string str)
	{
		bool backValue;
		if (str == "1") {
			backValue = true;
		} else {

			backValue = false;
		}
		return backValue;
	}

	public  static string GetBoolText (bool value)
	{
		string back;
		if (value = true) {
			back = "1";
		} else {
			back = "0";
		}
		return back;
	}

	public static Vector3 GetVector3Value (string str)
	{
		Vector3 backVector3 = new Vector3 ();
		string[] strs = str.Split (new char[1]{','});
		backVector3.x = (float.Parse (strs [0]));
		backVector3.y = (float.Parse (strs [1]));
		backVector3.z = (float.Parse (strs [2]));
		return backVector3;
	}

	public static string GetVector3Text (Vector3 vector3)
	{

		string[] strs = new string[3];
		strs [0] = vector3.x.ToString ();
		strs [1] = vector3.y.ToString ();
		strs [2] = vector3.z.ToString ();

		string back = strs [0] + "," + strs [1] + "," + strs [2];
		return back;
	}

	public static List<List<int>> GetListListIntValue (string str)
	{
		List<List<int>> backListValue = new List<List<int>> ();
		List<int> listChild;
		string[] strs;
		string[] strsTwo;
		strs = str.Split (new char[1]{';'});
		for (int j=0; j<strs.Length; j++) {
			listChild = new List<int> ();
			strsTwo = strs [j].Split (new char[1]{','});
			for (int m=0; m<strsTwo.Length; m++) {
				listChild.Add (int.Parse (strsTwo [m]));
			}
			backListValue.Add (listChild);
		}
		return backListValue;
	}
	public static string GetListListIntText (List<List<int>> ListListValue)
	{
		string backStr = "";

		foreach (List<int> listChild in ListListValue) {

			foreach (int a in listChild) {

				backStr = backStr + a.ToString () + ",";
			}

			backStr = backStr.Substring (0, backStr.Length - 1);
			backStr = backStr + ";";
		}
		backStr = backStr.Substring (0, backStr.Length - 1);
		return backStr;
	}


}
