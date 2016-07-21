using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompletedLevelButtons : MonoBehaviour {

	public List<SlideUIElement> mButtonsToAnimate;
	public float mTimeForButtonToSlide;
	public float mTimeBetweenButtonSlides;
	public AudioClip mButtonSlideSound;

	void Start(){

	}

	public IEnumerator StartAnimation(){

		for(int i = 0; i < mButtonsToAnimate.Count;i++){

			//mButtonsToSlide[i].StartCoroutine("Slide", mTimeForButtonToSlide);
			mButtonsToAnimate[i].StartCoroutine("FadeIn",mTimeForButtonToSlide);

			if(mButtonSlideSound != null){
				gameObject.GetComponent<AudioSource>().clip = mButtonSlideSound;
				gameObject.GetComponent<AudioSource>().Play();
			}

			yield return new WaitForSeconds(mTimeBetweenButtonSlides);

		}

		SetAllAlphasCorrectly (1.0f);
	}

	public void SetAllAlphasCorrectly(float alpha){

		for(int i = 0; i < mButtonsToAnimate.Count;i++){

			mButtonsToAnimate[i].GetComponent<UISprite>().alpha = alpha;
		}
	}

	public void SkipAnimationsLoad(){

		for(int i = 0; i < mButtonsToAnimate.Count;i++){
			
			mButtonsToAnimate[i].GetComponent<UISprite>().alpha = 1.0f;
		}

	}

}
