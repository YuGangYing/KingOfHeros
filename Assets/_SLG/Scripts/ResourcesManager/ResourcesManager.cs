using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif
namespace KOH
{
	public class ResourcesManager : SingleMonoBehaviour<ResourcesManager>
	{

		protected override void Awake ()
		{
			base.Awake ();
		}

		#region Audios

		public AudioClip GetAudioClip (string audioName)
		{
			return null;
		}

		public AudioClip LoadAudioClip (string path, string audioName)
		{
			return null;
		}

		public AudioClip GetAudioSE (string audioName)
		{
			return null;
		}

		public AudioClip GetAudioBGM (string audioName)
		{
			return null;
		}

		public AudioClip GetCharacterVoice (string name)
		{
			return null;
		}

		public AudioClip GetCharacterVoice (MVoiceCSVStructure voiceCSV)
		{
			return null;
		}

		public AudioClip GetCharacterVoice (int voiceId)
		{
			return null;
		}

		public float GetCharacterVoiceLength (int voiceId)
		{
			AudioClip clip = GetCharacterVoice (voiceId);
			if (clip != null) {
				return clip.length;
			}
			return 0;
		}

		#endregion

	
		public Sprite GetSprite (string path)
		{
			return null;
		}

		//get character head icon by character id
		public Sprite GetCharacterIconById (int charaId)
		{
			return null;
		}

		public GameObject GetCharacterPrefab (int charaId, int sortLayer = 1)
		{
			return null;
		}

		public byte[] GetCSV (string csvName)
		{
			return null;
		}
	}
}

public enum SoundSE
{
	SE01,
	SE02,
	SE03,
	SE04,
	SE05,
	SE06,
	SE07,
	SE08,
	SE09,
	SE10,
	SE11,
	SE12,
	SE13,
	SE14,
	SE15
}

public enum SoundBGM
{
	BGM01,
	BGM02,
	BGM03
}