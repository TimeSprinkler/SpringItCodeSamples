using UnityEngine;
using System.Collections;

public class RobotVisualEffect : MonoBehaviour {

	public AudioClip mLightningSound;
	public AudioClip mExplosionSound;
	public BotSplosion mRobotExplosionScript;
	public float mVisualEffectTime;

	public void PlayExplosion(){StartCoroutine("RobotExplosion");}

	IEnumerator RobotExplosion(){
		yield return new WaitForSeconds(mVisualEffectTime/2f);
		mRobotExplosionScript.enabled = true;
		PlaySound(mExplosionSound);
		yield return new WaitForSeconds(mVisualEffectTime);

		Destroy(this.gameObject);

	}
	
	void PlaySound(AudioClip clip){
		AudioSource source = this.gameObject.GetComponent<AudioSource>();

		source.clip = clip;
		source.Play ();
	}
}
