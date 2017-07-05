using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class FBXLoadUtility : MonoBehaviour {

	public bool Load;
	public GameObject[] AnimPrefabs;
	public static Dictionary<string,string> animStringReplace;

	// Use this for initialization
	void Start () {

	}

	void InitAnimDic()
	{
		if(animStringReplace==null)
		{
			animStringReplace = new Dictionary<string, string>();
			animStringReplace.Add("Attack1","Attack01");
			animStringReplace.Add("Attack2","Attack02");
			animStringReplace.Add("Attack3","Attack03");

			animStringReplace.Add("Death1","Death01");
			animStringReplace.Add("Death2","Death02");


		}
	}

	// Update is called once per frame
	void Update () {
		if(!Load)
		{
			InitAnimDic();
			foreach(AnimationState state in GetComponent<Animation>())
			{
				GetComponent<Animation>().clip = null;
				GetComponent<Animation>().RemoveClip(state.name);
			}
			foreach(GameObject go in AnimPrefabs)
			{
				string name = go.GetComponent<Animation>().clip.name;
				if(animStringReplace.ContainsKey(name))
				{
					name = animStringReplace[name];
				}
				GetComponent<Animation>().AddClip(go.GetComponent<Animation>().clip,name);
			}
			Load = true;
		}
	}
}
