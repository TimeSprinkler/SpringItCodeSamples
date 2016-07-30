using UnityEngine;
using System.Collections;

public class PlayMusic : MonoBehaviour {
	
	public AudioClip[] mSongStorage;

	[SerializeField]private AudioSource mAudioSource1;
	[SerializeField]private AudioSource mAudioSource2;

	private float mFadeTime = 3.0f;
	private float mCompletedLevelSupressionFactor = 0.75f;

	void Awake(){

		DontDestroyOnLoad(gameObject);

		EventHandler.OnMusicToggle += ChangeVolume;
		EventHandler.OnLevelCompleted += LevelCompleted;
		EventHandler.OnNewLevel += NewLevel;
		EventHandler.OnLevelSelectLoaded += LevelSelectLoaded;

	}

	void Start(){

		if(PlayerData.instance != null){
			if(PlayerData.instance.mMusicVolumeOn){
				EnabledAudioSource().volume = 1f;
			}else{
				EnabledAudioSource().volume = 0f;
			}
		}
	}

	void OnDestroy(){

		EventHandler.OnMusicToggle -= ChangeVolume;
		EventHandler.OnLevelCompleted -= LevelCompleted;
		EventHandler.OnNewLevel -= NewLevel;
		EventHandler.OnLevelSelectLoaded -= LevelSelectLoaded;

	}
	
	void LevelCompleted(){

		if(mAudioSource1.volume > mAudioSource2.volume){
			mAudioSource1.volume = mAudioSource1.volume * mCompletedLevelSupressionFactor;
		}else{
			mAudioSource2.volume = mAudioSource2.volume * mCompletedLevelSupressionFactor;
		}
	}

	public void NewLevel(int id){

		if(EnabledAudioSource().clip != NextSong()){
			DisabledAudioSource().clip = NextSong();
			StartCoroutine(SongTransition(EnabledAudioSource(), DisabledAudioSource()));
		}
	}

	void LevelSelectLoaded(){
		if(EnabledAudioSource().clip != mSongStorage[0]){
			DisabledAudioSource().clip = mSongStorage[0];
			StartCoroutine(SongTransition(EnabledAudioSource(), DisabledAudioSource()));
		}
	}

	AudioClip NextSong(){

		int songArrayInt = (Currentlevel.instance.mID + 9)/10;
		return mSongStorage[songArrayInt];

	}

	void ChangeVolume(bool state){

		if (state) {
			EnabledAudioSource ().volume = 1f;
		} else {
			EnabledAudioSource().volume = 0f;
		}
	}

	AudioSource EnabledAudioSource(){ return RelaventAudioSource(true);}
	AudioSource DisabledAudioSource(){ return RelaventAudioSource(false);}

	AudioSource RelaventAudioSource(bool isEnabled){
		if(mAudioSource1.volume > mAudioSource2.volume){
			if(isEnabled){
				return mAudioSource1;
			}else{
				return mAudioSource2;
			}
		}else{
			if(isEnabled){
				return mAudioSource2;
			}else{
				return mAudioSource1;
			}
		}
	}

	IEnumerator SongTransition(AudioSource fadeOutSource, AudioSource fadeInSource){

		fadeInSource.clip = NextSong();
		fadeInSource.Play();


		fadeInSource.mute = !PlayerData.instance.mMusicVolumeOn;
		fadeOutSource.mute = !PlayerData.instance.mMusicVolumeOn;

		float maxVolume = 1.0f;

		float i = 0.0f;
		float step = 1.0f/mFadeTime;

		while(i <= 1.0f){

			i += step * Time.deltaTime;


			fadeOutSource.volume = maxVolume * Mathf.Lerp (1.0f, 0.0f, i);
			fadeInSource.volume = maxVolume * Mathf.Lerp (0.0f, 1.0f, i);

			yield return new WaitForSeconds(step * Time.deltaTime);
		}

	}



}
