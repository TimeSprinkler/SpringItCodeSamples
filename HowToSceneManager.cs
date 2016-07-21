using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HowToSceneManager : MonoBehaviour {

	public List<GameObject> mTutorialPages = new List<GameObject>();


	private float mTutorialPageSize;
	private float mTweenTime = 0.2f;

	private int mMaxPanelSize;

	void Awake () {

		int numberOfHiddenPanels = HideTutorialPanelsNotYetReached ();

		mTutorialPageSize = this.GetComponent<UIGrid> ().cellWidth;
		mMaxPanelSize = (int)mTutorialPageSize * (this.transform.childCount - numberOfHiddenPanels - 1);

	}

	int HideTutorialPanelsNotYetReached(){

		int currentTutorialPanelReached = PlayerData.instance.mLastTutorialSeen;
		int returnInt = 0;

		for (int i = 0; i < mTutorialPages.Count; i++) {		

			int tutorialPageInt = DetermineTutorialPageInt(mTutorialPages[i].GetComponent<TutorialPageInfo>());

			if(tutorialPageInt >= currentTutorialPanelReached){
				mTutorialPages[i].SetActive(false);
				returnInt++;
			}
		}

		return returnInt;
	}

	int DetermineTutorialPageInt(TutorialPageInfo pageInfo){

		int pageIntValue = pageInfo.mLevelID * 10 + pageInfo.mPageID;

		return pageIntValue;

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

	private void ChangeOffset(float value){
		
		Vector3 moveAmount = Vector3.zero;
		moveAmount.x += value; 
		this.gameObject.GetComponent<UIScrollView>().MoveRelative(moveAmount);
	}
	
	public void mMovePageRight(){
		if (this.gameObject.GetComponent<UIPanel> ().clipOffset.x < mMaxPanelSize) {		
			StartCoroutine (TweenToNewTutorialPage (true));
		}
	}
	
	public void mMovePageLeft(){
		if (this.gameObject.GetComponent<UIPanel> ().clipOffset.x > mTutorialPageSize) {
			StartCoroutine (TweenToNewTutorialPage (false));
		}
	}
}
