using UnityEngine;
using System.Collections;

public class UIMenuAnimations : MonoBehaviour {

	public UIPanel mPanelToOpen;
	public UIPanel mPanelToOpen2;
	public UIPanel mPanelToClose;
	public UIPanel mPanelToClose2;

	public float mScreenOnDelay;
	public float mTimeForScreenToTurnOn;
	public float mTimeForScreenToTurnOff;

	public void StartAnimations(){

		if (!this.gameObject.activeSelf) {
			this.gameObject.SetActive(true);
		}

		if(mPanelToClose != null){
			StartCoroutine("FadeOff");

		}

		if (mPanelToOpen != null) {
			StartCoroutine("FadePanelOn");
		}

	}

	IEnumerator FadePanelOn(){

		mPanelToOpen.alpha = 0.0f;
		mPanelToOpen.gameObject.SetActive(true);

		if(mPanelToOpen2 != null){
			mPanelToOpen2.alpha = 0.0f;
			mPanelToOpen2.gameObject.SetActive(true);
		}

		yield return new WaitForSeconds(mScreenOnDelay);
		
		float lerpStep = 1f/mTimeForScreenToTurnOn;
		float lerpValue = 0f;
		
		float alpha = mPanelToOpen.alpha;
		
		while(lerpValue < 1){
			
			lerpValue += lerpStep * Time.deltaTime;
			
			alpha = Lineartransformations.SmoothStart4(lerpValue);
				
			mPanelToOpen.alpha = alpha;

			if(mPanelToOpen2 != null){
				mPanelToOpen2.alpha = alpha;
			}
			
			yield return new WaitForEndOfFrame();
			
		}
	}

	IEnumerator FadeOff(){

		mPanelToClose.alpha = 1.0f;

		if(mPanelToClose2 != null){
			mPanelToClose2.alpha = 1.0f;
		}
		
		float lerpStep = 1f/mTimeForScreenToTurnOff;
		float lerpValue = 1f;
		
		float alpha = mPanelToClose.alpha;

		while(lerpValue > 0){
			
			lerpValue -= lerpStep * Time.deltaTime;
			
			alpha = Lineartransformations.Mix4(0.5f, lerpValue);

			mPanelToClose.alpha = alpha;

			if(mPanelToClose2 != null){
				mPanelToClose2.alpha = alpha;
			}
			yield return new WaitForEndOfFrame();
			
		}

		mPanelToClose.gameObject.SetActive(false);

		if(mPanelToClose2 != null){
			mPanelToClose2.gameObject.SetActive(false);
		}

	}
}
