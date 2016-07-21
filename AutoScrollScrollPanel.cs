using UnityEngine;
using System.Collections;

public class AutoScrollScrollPanel : MonoBehaviour {

	private float mStartPause = 2.0f;

	private float mScrollStep = 2.5f;
	private float mStartOffset;

	void Start(){

		StartCoroutine (ScrollCredits());
		mStartOffset = this.GetComponent<UIPanel> ().clipOffset.y;
	}


	IEnumerator ScrollCredits(){

		yield return new WaitForSeconds(mStartPause);

		while (this.GetComponent<UIPanel>().clipOffset.y < this.GetComponent<UIPanel> ().GetViewSize().y) {

			ChangeOffset(mScrollStep);

			yield return new WaitForEndOfFrame();

		}

		yield return new WaitForSeconds(mStartPause/2f);


		this.GetComponent<UIPanel> ().clipOffset = new Vector2(0.0f,  mStartOffset);

		StartCoroutine (ScrollCredits());

	}

	void ChangeOffset(float amount){

		Vector2 tempPosition = this.GetComponent<UIPanel>().clipOffset;

		tempPosition.y += amount;

		this.GetComponent<UIPanel>().clipOffset = tempPosition;
	}

}
