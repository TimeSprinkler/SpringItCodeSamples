using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialLevelEvents : TutorialScrollPanel {

	public List<GameObject> mPanelRevealList = new List<GameObject>();

	public const int mSendRobotPanel = 3;
	public const int mPanPanel = 2;
	public const int mLastPanel = 5;

	public float mPanTime = 2.0f;
	public float mPanelFadeTime = 1.0f;
	public float mWaitTimeAfterRobotSpawn = 14f;
	public UIPanel mTutorialUIPanel;
	public BoxCollider mNextButtonCollider;
		
	private int currentPanel = 0;
	[SerializeField]protected UIPanel mPlayPanel;
	[SerializeField]private UIPanel mButtonPanel;

	[Range(-15, -30)]
	public float mStartZoomDistance;//From 0 base is -15
	public float mPauseAtStart;
	public float mAnimationTime;


	void Start(){

		EventHandler.OnRobotDestroyed += RobotDestroyed;

		Currentlevel.instance.SetAsTutorialLevel ();

		mTutorialPageSize = (int)mTutorialUIPanel.GetComponent<UIGrid>().cellWidth;
		mMaxPanelSize = mTutorialPageSize * mPanelRevealList.Count;
		
		mTutorialUIPanel.GetComponent<UIGrid> ().Reposition ();
	}

	public void NextPanelButton(){
		currentPanel++;

		CheckNextPanelChoice ();
	}

	void CheckNextPanelChoice(){

		switch (currentPanel) {
		
		default:
			MovePageRight();
			break;

		case mSendRobotPanel:
			MovePageRight();
			RevealPlayPanel();
			break;

		case mPanPanel:
			HideTutorial();
			StartCoroutine("PanFromStartToFinish");
			break;	

		case mLastPanel:
			RevealButtonPanel();
			RevealPlayPanel();
			MovePageRight();
			UpdatePlayerDataWithTutorialCompletion();
			break;
		}
	}

	public void SpawnRobotButton(){
		if (currentPanel <= mSendRobotPanel) {
			HideTutorial();
			StartCoroutine(WaitForRobottoDestroy());
		}
	}

	public void DragSpringIcon(){
		HideTutorial ();
	}

	void OnDestroy(){
		EventHandler.OnRobotDestroyed -= RobotDestroyed;

	}

	IEnumerator PanFromStartToFinish(){

		yield return new WaitForSeconds (mPanTime);

		PanCompleted ();
	}

	void PanCompleted(){
		//Debug.Log ("Pan Complete");
		RevealTutorial();
		NextPanelButton ();

	}

	void RobotDestroyed(GameObject robot){
		if (currentPanel < (mSendRobotPanel + 1)) {
			RevealTutorial();
			HidePlayPanel();
			NextPanelButton();
			StopCoroutine(WaitForRobottoDestroy());
		}
	}

	void RevealPlayPanel (){StartCoroutine (RevealPanel(mPlayPanel));}
	void HidePlayPanel(){StartCoroutine (HidePanel(mPlayPanel));}

	void RevealTutorial(){ StartCoroutine(RevealPanel(mTutorialUIPanel));}
	void HideTutorial(){StartCoroutine (HidePanel (mTutorialUIPanel));}

	void RevealButtonPanel(){StartCoroutine (RevealPanel (mButtonPanel));}

	IEnumerator HidePanel(UIPanel panel){
		panel.alpha = 1.0f;

		panel.gameObject.SetActive(false);
				
		//yield return new WaitForSeconds(mScreenOnDelay);
		
		float lerpStep = 1f/mPanelFadeTime;
		float lerpValue = 1.0f;
		float alpha = panel.alpha;
	
		while (lerpValue > 0) {
				
			lerpValue -= lerpStep * Time.deltaTime;
			
			alpha = Lineartransformations.SmoothStart4 (lerpValue);
				
			panel.alpha = alpha;
				
			yield return new WaitForEndOfFrame ();

		}
		
		if (panel != mTutorialUIPanel) {
			ToggleNextButton (true);//The next button only exsists when the play panel is not here
		}

	}

	IEnumerator RevealPanel(UIPanel panel){
		panel.alpha = 0.0f;
		panel.gameObject.SetActive(true);
		
		float lerpStep = 1f/mPanelFadeTime;
		
		float lerpValue = 0f;
		float alpha = panel.alpha;
		

		while(lerpValue < 1) {
				
			lerpValue += lerpStep * Time.deltaTime;
				
				
			alpha = Lineartransformations.SmoothStart4 (lerpValue);
				
			panel.alpha = alpha;
				
			yield return new WaitForEndOfFrame ();
				
		}
		
		if (panel != mTutorialUIPanel) {
			ToggleNextButton (false);
		}
	}

	
	float CalculateYOffset(){
		return (Mathf.Abs (mStartZoomDistance) * (Mathf.Tan (Camera.main.transform.rotation.x)));
	}

	IEnumerator WaitForRobottoDestroy(){

		yield return new WaitForSeconds (mWaitTimeAfterRobotSpawn);

		RobotDestroyed (new GameObject ());

	}

	private void ChangeOffset(float value){
		
		Vector3 moveAmount = Vector3.zero;
		moveAmount.x += value; 
		mTutorialUIPanel.gameObject.GetComponent<UIScrollView>().MoveRelative(moveAmount);
	}
	
	public void MovePageRight(){
		if (mTutorialUIPanel.clipOffset.x < mMaxPanelSize) {		
			StartCoroutine (TweenToNewTutorialPage (true));
		} else {
			CloseTutorialPage();
		}
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

	void ToggleNextButton(bool toggleOn){

		mNextButtonCollider.enabled = toggleOn;

	}

	void UpdatePlayerDataWithTutorialCompletion(){

		PlayerData.instance.mLastTutorialSeen = 5;

	}

}
