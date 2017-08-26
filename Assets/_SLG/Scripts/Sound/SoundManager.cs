using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KOH;

public class SoundManager : SingleMonoBehaviour<SoundManager> {

	protected override void Awake ()
	{
		base.Awake ();
	}

	public void PlayBGM(string bgm){
		AudioClip clip = ResourcesManager.GetInstance.GetAudioClipBGM(bgm);
	}

}
