using UnityEngine;
using System.Collections;

public class FadeInGrid : MonoBehaviour {


	void Awake(){

		Color color = this.renderer.material.color;

		color.a = 0;

		this.renderer.material.color = color;

	}

	public void StartFade(bool state){
		StartCoroutine (Fade (state));
	}

	IEnumerator Fade(bool fadeIn){

		Color color = this.renderer.material.color;

		float alphaTarget;
		float alphaStart;
		float lerpValue = 0.1f;


		if (!fadeIn) {

			alphaStart = 1.0f;
			alphaTarget = 0;
		
			while(color.a > alphaTarget){

				alphaStart -= lerpValue;

				color.a = Lineartransformations.SmoothStart3(alphaStart);

				this.renderer.material.color = color;

				yield return new WaitForEndOfFrame();

			}		
		
		} else {
			alphaStart = 0;
			alphaTarget = 1.0f;

			while(color.a < alphaTarget){

				alphaStart += lerpValue;
				
				color.a = Lineartransformations.SmoothStart3(alphaStart);

				this.renderer.material.color = color;
				
				yield return new WaitForEndOfFrame();
				
			}	
		}

	}
}
