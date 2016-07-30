using UnityEngine;
using System.Collections;

public class SlideUIElement : MonoBehaviour {

	public Vector3 mEndPosition;

	private Vector3 mStartPosition;

	public IEnumerator Slide(float timeToSlide){

		float lerpStep = 1f/timeToSlide;
		float lerpValue = 0f;
		
		Vector3 tempPosition = transform.localPosition;
		
		while(lerpValue < 1){
			
			lerpValue += lerpStep * Time.deltaTime;

			float xValue = Mathf.Lerp (mStartPosition.x, mEndPosition.x, Lineartransformations.SmoothStart3(lerpValue));
			float yValue = Mathf.Lerp (mStartPosition.y, mEndPosition.y, Lineartransformations.SmoothStart3(lerpValue));

			tempPosition.x = xValue;
			tempPosition.y = yValue;

			transform.localPosition = tempPosition + mStartPosition;
		
			yield return new WaitForEndOfFrame();
		}
	}

	public IEnumerator FadeIn(float timeToFade){

		float lerpStep = 1f/timeToFade;

		float tempAlpha = this.GetComponent<UISprite>().alpha;

		while(tempAlpha < 1){

			tempAlpha += Lineartransformations.Mix2 (0.5f, lerpStep) * Time.deltaTime;

			this.GetComponent<UISprite>().alpha = tempAlpha;

			yield return new WaitForEndOfFrame();
		}

		gameObject.GetComponent<BoxCollider>().enabled = true;
	}
}
