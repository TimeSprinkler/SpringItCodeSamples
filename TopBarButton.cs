using UnityEngine;
using System.Collections;

public class TopBarButton : MonoBehaviour {

	public int mStarsRequiredToUnlock;
	public string mUnlockedTopBarSpriteName;

	public int mLevelSelectPageSize;
	public float mTweenTime = 0.2f;
	public UIPanel mScrollPanel;
	public GameObject mRequiredBoltsLabel;

	[HideInInspector] public int mMaxPanelSize;

	void Start () {

		mMaxPanelSize = mLevelSelectPageSize * (5);


		CheckStarsAndUnlock ();
	}

	void CheckStarsAndUnlock(){
		PlayerData.instance.UpdateTotalStartsEarned ();


		if (mStarsRequiredToUnlock <= PlayerData.instance.mNumberOfStarsEarned) {
			UnlockThisBar();
		}

	}

	void UnlockThisBar(){
		transform.parent.GetComponent<UISprite> ().spriteName = mUnlockedTopBarSpriteName;
		this.GetComponent<BoxCollider> ().enabled = true;

		if (mRequiredBoltsLabel != null) {
			mRequiredBoltsLabel.SetActive (false);
		}
	}

	IEnumerator TweenToNewPage(bool moveToRight){
		
		float time = 0;
		float totalMoveDistance = 0;
		float previousMoveDistance = 0;
		float lerpInterval = 1 / mTweenTime;
		
		while (time < 1) {
			
			time += Time.deltaTime * lerpInterval;
			
			if (moveToRight) {
				totalMoveDistance = -mLevelSelectPageSize * Lineartransformations.SmoothStop3(time); 
			} else {
				totalMoveDistance = mLevelSelectPageSize * Lineartransformations.SmoothStop3(time); 
			}
			
			ChangeOffset(totalMoveDistance - previousMoveDistance);
			previousMoveDistance = totalMoveDistance;
			
			yield return new WaitForEndOfFrame();
		}
	}

	private void ChangeOffset(float value){
		
		Vector3 moveAmount = Vector3.zero;
		moveAmount.x += value; 
		mScrollPanel.gameObject.GetComponent<UIScrollView>().MoveRelative(moveAmount);
	}

	public void mMovePageRight(){
		if (mScrollPanel.clipOffset.x < mMaxPanelSize) {		
			StartCoroutine (TweenToNewPage (true));
		}
	}
	
	public void mMovePageLeft(){
		if (mScrollPanel.clipOffset.x  > 0) {
			StartCoroutine (TweenToNewPage (false));
		}
	}

}
