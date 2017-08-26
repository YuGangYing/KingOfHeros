using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KOH;

public class SoundManager : SingleMonoBehaviour<SoundManager> {

	AudioSource mMusicSource;
	AudioSource mMusicSource1;

	protected override void Awake ()
	{
		base.Awake ();
		mMusicSource = gameObject.GetOrAddComponent<AudioSource> ();
		mMusicSource.loop = true;
	}

	public void PlayBGM(string bgm){
		AudioClip clip = ResourcesManager.GetInstance.GetAudioClipBGM(bgm);
		mMusicSource = gameObject.GetOrAddComponent<AudioSource> ();
		mMusicSource.clip = clip;
		mMusicSource.Play ();
	}

}
