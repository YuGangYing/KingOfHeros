using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;
using System.Collections.Generic;

/*Animator animator = gameObj.GetComponentInChildren<Animator>();
			if(animator!=null)
			{
				animator.Update(10f);
			}
			*/


public class AnimatorTest :EditorWindow
{
	GameObject gameObj;
	Animator animator;
	List<string> clipList = new List<string>();

	static public void create (GameObject obj)
	{
		AnimatorTest test = (AnimatorTest)EditorWindow.GetWindow(typeof(AnimatorTest),true,"动作测试");
		test.gameObj = obj;
		test.init();
	}

	double m_lastTime = EditorApplication.timeSinceStartup;
	void init()
	{
		animator = gameObj.GetComponentInChildren<Animator>();

		AnimatorOverrideController controller = new AnimatorOverrideController();
		controller.runtimeAnimatorController = animator.runtimeAnimatorController;// UnityEditor.Animations.AnimatorController. .GetEffectiveAnimatorController(animator);
		foreach(AnimationClipPair item in controller.clips)
		{
			clipList.Add(item.originalClip.name);
		}
	}

	void OnGUI()
	{
		if(animator==null)
			return;
		GUILayout.BeginVertical();
		GUILayout.Label("", GUILayout.Width(20f));

		foreach(string item in clipList)
		{
			animator.SetBool(item,false);
			if(GUILayout.Button(item,GUILayout.Width(120),GUILayout.Height(30)))
			{
				animator.SetBool(item,true);
			}
		}
	}

	void Update ()
	{
		if(animator==null)
			return;
		animator.Update((float)(EditorApplication.timeSinceStartup - m_lastTime));
		m_lastTime = EditorApplication.timeSinceStartup;
	}
}
