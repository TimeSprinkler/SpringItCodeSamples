using UnityEngine;
using System.Collections;

public class AudioContainerUI : MonoBehaviour {


	public AudioClip mOnClickSound;
	public AudioClip mEventTriggeredSound;
	public AudioClip mInvalidEventSound;

	[SerializeField] private AudioSource mAudioSource;

	void Awake(){

		if (mAudioSource == null) {
			Debug.LogError("No AudioSource on GameObject " + this.gameObject.name +" with " + this.name);
		}
	}

	public void PlayOnClickSound(){ StartCoroutine(PlaySoundEffect(mOnClickSound));}
	public void PlayEventTriggerSound(){ StartCoroutine(PlaySoundEffect(mEventTriggeredSound));}
	public void PlayInvalidLocationEffect(){ StartCoroutine(PlaySoundEffect(mInvalidEventSound));}

	void PlaySound(AudioClip clip){
		if (PlayerData.instance.mSFXVolumeOn) {
			mAudioSource.clip = clip;
			mAudioSource.Play ();
		}
	}

	IEnumerator PlaySoundEffect(AudioClip audioClip){
		if(audioClip != null){
			float clipTime = audioClip.length;
			PlaySound (audioClip);
			yield return new WaitForSeconds(clipTime);
		}
	}
}
