using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialScrollPanel : MonoBehaviour {

	public float mTweenTime = 0.2f;
	
	public GameObject mCloseButton;

	protected int mTutorialPageSize;
	[HideInInspector] public int mMaxPanelSize;
	[HideInInspector] public int mNumberOfTutorialPages = 0;

	void Awake(){

		mNumberOfTutorialPages = transform.childCount;
		if (mNumberOfTutorialPages <= 0) {
			transform.parent.gameObject.SetActive(false);
			Camera.main.GetComponent<CameraSceneStartAnimation>().AnimateCamera();
		}

		int thisTutorialViewNumber = Currentlevel.instance.mID * 10 + mNumberOfTutorialPages;

		if (thisTutorialViewNumber < PlayerData.instance.mLastTutorialSeen) {
			transform.parent.gameObject.SetActive(false);
			Camera.main.GetComponent<CameraSceneStartAnimation>().AnimateCamera();
		}
	}

	void Start(){

		
		mTutorialPageSize = (int)this.GetComponent<UIPanel> ().GetViewSize ().y;
		mMaxPanelSize = mTutorialPageSize * (mNumberOfTutorialPages - 1);

		this.GetComponent<UIGrid> ().Reposition ();

	}

	IEnumerator TweenToNewTutorialPage(bool moveToRight){

		float time = 0;
		float totalMoveDistance = 0;
		float previousMoveDistance = 0;
		float lerpInterval = 1 / mTweenTime;

		while (time < 1) {

			time += Time.deltaTime * lerpInterval;

			if (moveToRight) {
				totalMoveDistance = -mTutorialPageSize * Lineartransformations.SmoothStop3(time); 
			} else {
				totalMoveDistance = mTutorialPageSize * Lineartransformations.SmoothStop3(time); 
			}

			ChangeOffset(totalMoveDistance - previousMoveDistance);
			previousMoveDistance = totalMoveDistance;

			yield return new WaitForEndOfFrame();
		}
	}

	[ExecuteInEditMode]
	public void AddTutorialPageToScrollPanel(GameObject tutPage){

		tutPage.transform.parent = this.transform;
		tutPage.transform.localPosition = Vector3.zero;
		tutPage.transform.localScale = Vector3.one;
		mNumberOfTutorialPages++;

		//mTutorialPageSize =tutPage.GetComponent<UITexture>().width;

	}

	private void ChangeOffset(float value){

		Vector3 moveAmount = Vector3.zero;
		moveAmount.x += value; 
		this.gameObject.GetComponent<UIScrollView>().MoveRelative(moveAmount);
	}

	public void mMovePageRight(){

		if (this.gameObject.GetComponent<UIPanel> ().clipOffset.x < mMaxPanelSize) {		
			StartCoroutine (TweenToNewTutorialPage (true));
		} else {
			CloseTutorialPage();
		}
	}

	public void mMovePageLeft(){
		if (this.gameObject.GetComponent<UIPanel> ().clipOffset.x > 0) {
			StartCoroutine (TweenToNewTutorialPage (false));
		}
	}

	public void CloseTutorialPage(){
		PlayerData.instance.CloseTutorialPage (mNumberOfTutorialPages);

		Camera.main.GetComponent<CameraSceneStartAnimation> ().AnimateCamera ();
	}



}
