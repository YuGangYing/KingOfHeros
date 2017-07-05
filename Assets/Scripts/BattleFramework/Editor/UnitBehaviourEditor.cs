//CopyRight YingYuGang(232871714@qq.com) 2014-2015
using UnityEngine;
using System.Collections;
using UnityEditor;
using HutongGames.PlayMaker;

namespace ZhiWuDaZhan{

//	[CustomEditor(typeof(UnitBehaviour))]
	//TODO
	public class UnitBehaviourEditor : Editor {

//		UnitBehaviour mTarget;
//		PlayMakerFSM pm;
//
//		string[] mBoolVariableNames;
//		string[] mFloatVariableNames;
//		string[] mIntVariableNames;
//		string[] mGameObjectVariableNames;
//
//		void OnEnable()
//		{
//			mTarget = (UnitBehaviour)target;
//			pm = mTarget.GetComponent<PlayMakerFSM>();
//
//			mBoolVariableNames = GetNames (pm.FsmVariables,typeof(FsmBool));
//			mFloatVariableNames = GetNames (pm.FsmVariables,typeof(FsmFloat));
//			mIntVariableNames = GetNames (pm.FsmVariables,typeof(FsmInt));
//			mGameObjectVariableNames = GetNames (pm.FsmVariables,typeof(FsmGameObject));
//
//			mShootTargetIndex = GetIndexOfVariable (pm.FsmVariables,typeof(FsmGameObject),mTarget.shootTarget);
//			mSearchIntervalIndex = GetIndexOfVariable (pm.FsmVariables,typeof(FsmFloat),mTarget.searchInterval);
//
//		}
//
//		int GetIndexOfVariable(FsmVariables vars, System.Type type, NamedVariable var)
//		{
//			if (var == null || var.Name == null || var.Name == "")
//				return -1;
//			NamedVariable[] namedVars = vars.GetNames (type);
//			for (int i = 0; i < namedVars.Length; i ++) 
//			{
//				if(namedVars[i].Name == var.Name)
//				{
//					return i;
//				}
//			}
//			Debug.LogError (var + "is not exiting!");
//			return -1;
//		}
//
//		string[] GetNames(FsmVariables vars, System.Type type)
//		{
//			NamedVariable[] namedVars = vars.GetNames (type);
//			string[] names = new string[namedVars.Length];
//			for(int i = 0 ; i < namedVars.Length ; i ++)
//			{
//				names[i] = namedVars[i].Name;
//			}
//			return names;
//		}
//
//		int mCurrentShootTargetIndex;
//		int mShootTargetIndex;
//
//		int mCurrentSearchIntervalIndex;
//		int mSearchIntervalIndex; 
//
//		public override void OnInspectorGUI()
//		{
//			serializedObject.Update ();
//			Undo.RecordObject (mTarget,"UnitBehaviour");
//
//			EditorGUILayout.BeginHorizontal ();
//			EditorGUILayout.LabelField ("ShootTarget:");
//			mShootTargetIndex = EditorGUILayout.Popup (mShootTargetIndex, mGameObjectVariableNames);
//			if(mCurrentShootTargetIndex != mShootTargetIndex && mShootTargetIndex!=-1)
//			{
//				mCurrentShootTargetIndex = mShootTargetIndex;
//				mTarget.shootTarget = pm.FsmVariables.GameObjectVariables[mCurrentShootTargetIndex];
//			}
//			EditorGUILayout.EndHorizontal ();
//
//			EditorGUILayout.BeginHorizontal ();
//			EditorGUILayout.LabelField ("SearchInterval:");
//			mSearchIntervalIndex = EditorGUILayout.Popup (mSearchIntervalIndex, mFloatVariableNames);
//			if(mCurrentSearchIntervalIndex != mSearchIntervalIndex && mSearchIntervalIndex!=-1)
//			{
//				mCurrentSearchIntervalIndex = mSearchIntervalIndex;
//				mTarget.searchInterval = pm.FsmVariables.FloatVariables[mCurrentSearchIntervalIndex];
//			}
//			EditorGUILayout.EndHorizontal ();
//
//
////			EditorGUILayout.pop
//			UnityEditor.EditorUtility.SetDirty (mTarget);
//			serializedObject.ApplyModifiedProperties ();
//		}
//
	}

}