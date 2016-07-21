using UnityEngine;
using System.Collections;

public class AudioContainer : MonoBehaviour {

	[SerializeField] private AudioClip mRobotInterractionSound;
	[SerializeField] private AudioClip mAmbientSound;
	[SerializeField] private AudioClip mConstructionSound;
	[SerializeField] private AudioClip mInvalidLocationSound;

	[SerializeField] private AudioSource mAudioSource;

	void Start(){
		if (mAudioSource == null) {
			Debug.LogError("No AudioSource on GameObject " + this.gameObject.name +" with " + this.name);
		}

		if(mAmbientSound != null){
			PlaySound (mAmbientSound);
		}
	}

	void PlaySound(AudioClip clip){
		if (PlayerData.instance.mSFXVolumeOn) {
			mAudioSource.clip = clip;
			mAudioSource.Play ();
		}
	}

	public void PlayRobotInterractionEffect(){StartCoroutine(PlaySoundEffect(mRobotInterractionSound));}
	public void PlayConstructionEffect(){ StartCoroutine(PlaySoundEffect(mConstructionSound));}
	public void PlayInvalidLocationEffect(){ StartCoroutine(PlaySoundEffect(mInvalidLocationSound));}

	IEnumerator PlaySoundEffect(AudioClip audioClip){
		if(audioClip != null){
			float clipTime = audioClip.length;
			PlaySound (audioClip);
			yield return new WaitForSeconds(clipTime);
			
			if(mAmbientSound != null){
				PlaySound(mAmbientSound);
			}
		}
	}

	public void TurnOffAmbientSound(){mAudioSource.Stop();}
}
