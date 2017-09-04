using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

public class XCodeBuild  {
	[PostProcessBuild]
	public static void OnPostprocessBuild (BuildTarget buildTarget, string path)
	{
		//if (buildTarget != BuildTarget.iPhone) { // For Unity < 5
		if (buildTarget != BuildTarget.iOS) {
			Debug.LogWarning ("Target is not iOS. AdColonyPostProcess will not run");
			return;
		}
		string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
		PBXProject proj = new PBXProject ();
		proj.ReadFromString (File.ReadAllText (projPath));
		string target = proj.TargetGuidByName ("Unity-iPhone");
		proj.SetBuildProperty (target, "ENABLE_BITCODE", "NO");
		proj.SetBuildPropertyForConfig(target, "DEBUG_INFORMATION_FORMAT", "DWARF");
		File.WriteAllText (projPath, proj.WriteToString ());
	}
}
