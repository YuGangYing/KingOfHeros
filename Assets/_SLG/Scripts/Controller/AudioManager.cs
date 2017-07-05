using UnityEngine;
using System.Collections;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
public class AudioManager : MonoBehaviour {

	static private AudioObject[] audioObject;
	
	static public AudioManager audioManager;
	
	static private float musicVolume=.75f;
	static private float sfxVolume=.75f;
	
	static private Transform cam;
	
	public float minFallOffRange=10;
	
	public AudioClip[] musicList;
	public bool playMusic=true;
	public bool shuffle=false;
	private int currentTrackID=0;
	public AudioSource musicSource;

	public AudioClip PeltastFightAudio;
	public AudioClip ArcherFightAudio;
	public AudioClip MagicFightAudio;
	public AudioClip SpearFightAudio;
	public AudioClip CavalryFightAudio;

	public AudioClip WhoopAudio;

	public AudioClip actionFailedSound;
	private GameObject thisObj;

	public static AudioManager instance;
	public static AudioManager SingleTon()
	{
		return instance;
	}
	
	public void PlayPeltastFightAudio()
	{
		if(!m_IsPlayingSpearFightAudio && !m_IsPlayingPeltastFightAudio && PeltastFightAudio!=null)
		{
			m_IsPlayingPeltastFightAudio = true;
			StartCoroutine(_PlayPeltastFightAudioRoutine());
		}
	}
	
	public void PlayArcherFightAudio()
	{
		if(!m_IsPlayingArcherFightAudio && ArcherFightAudio!=null)
		{
//			Debug.Log("PlayArcherFightAudio");
			m_IsPlayingArcherFightAudio = true;
			StartCoroutine(_PlayArcherFightAudioRoutine());
		}
	}

	public void PlayMagicFightAudio()
	{
		if(!m_IsPlayingMagicFightAudio && MagicFightAudio!=null)
		{
			m_IsPlayingMagicFightAudio = true;
			StartCoroutine(_PlayMagicFightAudioRoutine());
		}
	}

	public void PlaySpearFightAudio()
	{
		if(!m_IsPlayingSpearFightAudio && SpearFightAudio!=null)
		{
			m_IsPlayingSpearFightAudio = true;
			StartCoroutine(_PlaySpearFightAudioRoutine());
		}
	}

	public void PlayCavalryFightAudio()
	{
		if(!m_IsPlayingSpearFightAudio && !m_IsPlayingPeltastFightAudio && CavalryFightAudio!=null)
		{
			m_IsPlayingCavalryAudio = true;
			StartCoroutine(_PlayCavalryFightAudioRoutine());
		}
	}

	bool m_IsPlayingPeltastFightAudio = false;
	IEnumerator _PlayPeltastFightAudioRoutine()
	{
		AudioManager.PlaySound(PeltastFightAudio);
		yield return new WaitForSeconds(1.886f);
		m_IsPlayingPeltastFightAudio = false;
	}
	
	bool m_IsPlayingArcherFightAudio = false;
	IEnumerator _PlayArcherFightAudioRoutine()
	{
		AudioManager.PlaySound(ArcherFightAudio);
		yield return new WaitForSeconds(1.59f);
		m_IsPlayingArcherFightAudio = false;
	}

	bool m_IsPlayingMagicFightAudio = false;
	IEnumerator _PlayMagicFightAudioRoutine()
	{
		AudioManager.PlaySound(MagicFightAudio);
		yield return new WaitForSeconds(2.0f);
		m_IsPlayingMagicFightAudio = false;
	}
	
	bool m_IsPlayingSpearFightAudio = false;
	IEnumerator _PlaySpearFightAudioRoutine()
	{
		AudioManager.PlaySound(SpearFightAudio);
		yield return new WaitForSeconds(2.0f);
		m_IsPlayingSpearFightAudio = false;
	}

	bool m_IsPlayingCavalryAudio = false;
	IEnumerator _PlayCavalryFightAudioRoutine()
	{
		AudioManager.PlaySound(CavalryFightAudio);
		yield return new WaitForSeconds(2.038f);
		m_IsPlayingCavalryAudio = false;
	}

	public void PlayWhoopAudio()
	{
		musicList[0] = WhoopAudio;
		StartCoroutine(ToggleMusicRoutine());
	}

	IEnumerator ToggleMusicRoutine()
	{
		float curTime = 0;
		musicSource.volume = 0.9f;
		while(curTime<2)
		{
//			Debug.Log(musicSource.volume);
			musicSource.volume -= Time.deltaTime / 2;
			curTime += Time.deltaTime / 2;
			yield return null;
		}
		StopCoroutine("MusicRoutine");
		StartCoroutine(MusicRoutine());
		curTime = 0;
		musicSource.volume = 0;
		while(curTime<2)
		{
			musicSource.volume += Time.deltaTime / 2;
			musicSource.volume = Mathf.Min(0.1f,musicSource.volume);
			curTime += Time.deltaTime / 2;
			yield return null;
		}
	}


	void Awake(){
		//Init();
		if(instance==null)
		{
			instance=this;
		}
		thisObj=gameObject;
		
		cam=Camera.main.transform;
		
		if(playMusic && musicList!=null && musicList.Length>0){
			GameObject musicObj=new GameObject();
			musicObj.name="MusicSource";
			musicObj.transform.position=cam.position;
			musicObj.transform.parent=cam;
			musicSource=musicObj.AddComponent<AudioSource>();
			musicSource.loop=false;
			musicSource.playOnAwake=false;
			musicSource.volume=musicVolume;
			
			musicSource.ignoreListenerVolume=true;
			
			StartCoroutine(MusicRoutine());
		}
		
		audioObject=new AudioObject[20];
		for(int i=0; i<audioObject.Length; i++){
			GameObject obj=new GameObject();
			obj.name="AudioSource";
			
			AudioSource src=obj.AddComponent<AudioSource>();
			src.playOnAwake=false;
			src.loop=false;
			src.minDistance=minFallOffRange;
			
			Transform t=obj.transform;
			t.parent=thisObj.transform;
			
			audioObject[i]=new AudioObject(src, t);
		}
		
		AudioListener.volume=sfxVolume;
		
		if(audioManager==null) audioManager=this;
	}
	
	static public void Init(){
		if(audioManager==null){
			GameObject objParent=new GameObject();
			objParent.name="AudioManager";
			audioManager=objParent.AddComponent<AudioManager>();
		}		
	}

	public IEnumerator MusicRoutine(){
		while(true){
			if(shuffle) musicSource.clip=musicList[Random.Range(0, musicList.Length)];
			else{
				musicSource.clip=musicList[currentTrackID];
				currentTrackID+=1;
				if(currentTrackID==musicList.Length) currentTrackID=0;
			}

			musicSource.Play();
			
			yield return new WaitForSeconds(musicSource.clip.length-0.05f);
		}
	}
	
	//check for the next free, unused audioObject
	static private int GetUnusedAudioObject(){
		for(int i=0; i<audioObject.Length; i++){
			if(!audioObject[i].inUse){
				return i;
			}
		}
		
		//if everything is used up, use item number zero
		return 0;
	}
	
	//this is a 3D sound that has to be played at a particular position following a particular event
	static public void PlaySound(AudioClip clip, Vector3 pos){
		if(audioManager==null) Init();
		
		int ID=GetUnusedAudioObject();
		
		audioObject[ID].inUse=true;
		
		audioObject[ID].thisT.position=pos;
		audioObject[ID].source.clip=clip;
		audioObject[ID].source.Play();

		float duration=audioObject[ID].source.clip.length;
		audioManager.StartCoroutine(audioManager.ClearAudioObject(ID, duration));
	}
	
	//this no position has been given, assume this is a 2D sound
	static public void PlaySound(AudioClip clip){
		if(audioManager==null) Init();
		
		int ID=GetUnusedAudioObject();
		
		audioObject[ID].inUse=true;
		
		audioObject[ID].source.clip=clip;
		audioObject[ID].source.Play();
		
		float duration=audioObject[ID].source.clip.length;
		
		audioManager.StartCoroutine(audioManager.ClearAudioObject(ID, duration));
	}
	
	//a sound routine for 2D sound, make sure they follow the listener, which is assumed to be the main camera
	static IEnumerator SoundRoutine2D(int ID, float duration){
		while(duration>0){
			audioObject[ID].thisT.position=cam.position;
			yield return null;
		}
		
		//finish playing, clear the audioObject
		audioManager.StartCoroutine(audioManager.ClearAudioObject(ID, 0));
	}
	
	//function call to clear flag of an audioObject, indicate it's is free to be used again
	private IEnumerator ClearAudioObject(int ID, float duration){
		yield return new WaitForSeconds(duration);
		
		audioObject[ID].inUse=false;
	}
	
	static public void SetSFXVolume(float val){
		sfxVolume=val;
		AudioListener.volume=val;
	}
	
	static public void SetMusicVolume(float val){
		musicVolume=val;
		if(audioManager && audioManager.musicSource){
			audioManager.musicSource.volume=val;
		}
	}
	
	public static float GetMusicVolume(){ return musicVolume; }
	public static float GetSFXVolume(){ return sfxVolume; }
}


[System.Serializable]
public class AudioObject{
	public AudioSource source;
	public bool inUse=false;
	public Transform thisT;
	
	public AudioObject(AudioSource src, Transform t){
		source=src;
		thisT=t;
	}
}