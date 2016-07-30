using UnityEngine;
using System.Collections;

public class StarOnOff : MonoBehaviour {

	public float mRotationToTravel;

	public GameObject mStarOn;
	public GameObject mStarOff;
	public GameObject mStarSparkle;

	public Animation mStarSpinAnimation;

	public AudioSource mDisplayLevelResultsAudioSource;
	public AudioClip mSoundEffect;

	void Start(){
		mStarOn.SetActive (true);
		mStarOn.GetComponent<UISprite> ().alpha = 0.0f;
	}

	public void StarOn(float timeToFade){

		StartCoroutine (FadeIn (timeToFade));
		StartCoroutine (FadeOut (timeToFade));
		StartCoroutine (SpinBolts (timeToFade));

		if (mSoundEffect != null){
			mDisplayLevelResultsAudioSource.clip = mSoundEffect;
			mDisplayLevelResultsAudioSource.Play ();
		}

		//Shake Screen

	}

	public void StarOff(){
		mStarOn.SetActive(false);
		mStarOff.SetActive(true);
	}

	public void StarOn(){
		mStarOn.SetActive(true);
		mStarOn.GetComponent<UISprite> ().alpha = 1.0f;
		mStarOff.SetActive(false);
	}

	//screen shakes
	//phone slightly vibrates?

	IEnumerator FadeIn(float timeToFade){

		float lerpStep = 1f/timeToFade;
		float lerpValue = 0.0f;
		float tempAlpha = mStarOn.GetComponent<UISprite>().alpha;

		while(lerpValue < 1){

			lerpValue += lerpStep * Time.deltaTime;

			tempAlpha = Lineartransformations.Mix2 (0.5f, lerpValue);
			
			mStarOn.GetComponent<UISprite>().alpha = tempAlpha;
			
			yield return new WaitForEndOfFrame();
		}

		if (mStarSparkle != null) {
			mStarSparkle.SetActive (true);
		}
		//Camera.main.GetComponentInChildren<ScreenShake> ().Shake(timeToFade/3f);

#if UNITY_ANDROID
#if UNITY_IPHONE

		Handheld.Vibrate ();

#endif
#endif

	}

	IEnumerator FadeOut(float timeToFade){

		float lerpStep = 1f/timeToFade;
		float lerpValue = 1.0f;

		float tempAlpha = mStarOff.GetComponent<UISprite>().alpha;
		
		while(lerpValue > 0){

			lerpValue -= lerpStep * Time.deltaTime;

			tempAlpha = Lineartransformations.Mix2 (0.5f, lerpValue);
			
			mStarOff.GetComponent<UISprite>().alpha = tempAlpha;
			
			yield return new WaitForEndOfFrame();
		}

	}

	IEnumerator SpinBolts(float timeToSpin){

		float lerpStep = 1.0f/timeToSpin;
		float lerpValue = 0.0f;
		float rotationAmountPerFrame = (mRotationToTravel/timeToSpin) * Time.fixedDeltaTime;

		float currentRotation = 0.0f;
		float tempRotation = 0.0f;

		while(lerpValue < 1){

			lerpValue += lerpStep * Time.deltaTime;

			tempRotation = rotationAmountPerFrame * Lineartransformations.SmoothStop2 (lerpValue);
			currentRotation += tempRotation;

			mStarOff.transform.Rotate(0,0, tempRotation);
			mStarOn.transform.Rotate(0,0,tempRotation);
			
			yield return new WaitForEndOfFrame();
		}
	}

}
