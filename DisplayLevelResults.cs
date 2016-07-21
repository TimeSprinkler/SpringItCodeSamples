using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayLevelResults : MonoBehaviour {

	public List<UILabel> mLabels = new List<UILabel>(); //Make this 3 long with Moves, then robots, then tools labels
	public UILabel mTimeLabel;

	public float mScaleUpNumberTime;
	public float mPauseBetweenNumberScales;
	public float mStarsLoadTime;

	public AudioSource mAudioSource;
	public AudioClip mSoundDuringNumberIncrease;
	public AudioClip mSoundAfterNumberIncrease;

	public string mBlueMarkerSpriteName;
	public string mYellowMarkerSpriteName;
	public string mRedMarkerSpriteName;

	private float mStars = 0;
	private int mLabelsTracker = 0;
	private bool mSkipAnimationsCalled = false;

	public StarOnOff[] mStarOnOff;

	void Awake(){

		for(int i = 0; i < mStarOnOff.Length;i++){
			mStarOnOff[i].StarOff();
		}

	}

	public void SetLabelsAtStartingValues(){

		mLabels[0].text = "0";
		mLabels[1].text = "0";
		mLabels[2].text = "0";
		mTimeLabel.text = "0:00";
	}

	//Animate a number increasing 
	//then have the color change of the number
	//Have the appropriate number of stars appear on the scene. (maybe  fill up mechanics which means stars need to be divided into halves) or you could do it with a cutout shader and changing the cutoff value
	//Then time is revealed an the Best Time is shown if it is a new record


	public void Animate(){

		Camera.main.GetComponent<Placer> ().enabled = false;
		Camera.main.GetComponent<CameraMovement> ().enabled = false;

		float completionTime = (Time.realtimeSinceStartup - Currentlevel.instance.mTimeOfLevelStart) * 100;

		Currentlevel.mLevelPlayTime = Mathf.Round (completionTime)/100;
		Camera.main.GetComponent<TimeScaleManager>().ResetSpeed();

	
		ScaleUpNextNumber (mLabels [mLabelsTracker]);

		if (this.transform.FindChild ("Background Metal") != null) {
			this.transform.FindChild ("Background Metal").GetComponent<BoxCollider> ().enabled = true;
		}

	}

	void ScaleUpNextNumber(UILabel label){

		switch(mLabelsTracker){

		case 0:
			StartCoroutine(AnimateLabelNumberIncrease(label, Currentlevel.instance.mMoves));
			break;

		case 1:
			StartCoroutine(AnimateLabelNumberIncrease(label, Currentlevel.instance.mRobotsUsed));
			break;

		case 2:
			StartCoroutine(AnimateLabelNumberIncrease(label,Currentlevel.instance.mMaxToolsUsed));
			break;

		default:
			break;
		}

	}

	IEnumerator ScaleUpTime(){

		if (!mSkipAnimationsCalled) {

			yield return new WaitForSeconds(mPauseBetweenNumberScales);

			float lerpStep = Currentlevel.mLevelPlayTime / mScaleUpNumberTime;
			float lerpValue = 0;
			mTimeLabel.text = lerpValue.ToString ();
			
			while (lerpValue < Currentlevel.mLevelPlayTime) {
				
				lerpValue += lerpStep;
				double tempLerpValue = System.Math.Round (lerpValue, 2);
				mTimeLabel.text = tempLerpValue.ToString ();

				
				if (mSoundDuringNumberIncrease != null) {
					mAudioSource.clip = mSoundDuringNumberIncrease;
					mAudioSource.Play ();
				}
				
				yield return new WaitForEndOfFrame ();
			}
		}

		yield return new WaitForEndOfFrame ();

	}

	IEnumerator AnimateLabelNumberIncrease(UILabel label, float finalNumber){

		yield return new WaitForSeconds(mPauseBetweenNumberScales);

		if (!mSkipAnimationsCalled) {

			float lerpStep = mScaleUpNumberTime / finalNumber;
			int lerpValue = 0;
			label.text = lerpValue.ToString ();

			while (lerpValue < finalNumber) {

				lerpValue++;
				label.text = lerpValue.ToString ();

				if (mSoundDuringNumberIncrease != null) {
					mAudioSource.clip = mSoundDuringNumberIncrease;
					mAudioSource.Play ();
				}

				yield return new WaitForSeconds (lerpStep);
			}

			RatePerformance ();
		}
	}

	void RatePerformance(){

		int rating = 0;

		switch(mLabelsTracker){

		case 0:
			rating = RateMoves();
			break;

		case 1:
			rating = RateRobots();
			break;

		case 2:
			rating = RateTools();
			break;

		}

		mStars += ChangeLabelColor(rating);
		mLabelsTracker++;
		if(mLabelsTracker < mLabels.Count){
			ScaleUpNextNumber(mLabels[mLabelsTracker]);
		}else{
			StartCoroutine(AddStars());
		}
	}

	float ChangeLabelColor(int rating){

		switch(rating){

		case 1:
			mLabels[mLabelsTracker].transform.parent.GetComponent<UISprite>().spriteName = mBlueMarkerSpriteName;

			return 1;

		case 0:
			mLabels[mLabelsTracker].transform.parent.GetComponent<UISprite>().spriteName = mRedMarkerSpriteName;
			break;

		}

		return 0;

	}

	IEnumerator AddStars(){

		if (!mSkipAnimationsCalled) {
			int currentStars = Mathf.CeilToInt (mStars) / 1;

			for (int i = 0; i < currentStars; i++) {

				mStarOnOff [i].StarOn (mStarsLoadTime);
							
				yield return new WaitForSeconds (mStarsLoadTime);
			}

			Currentlevel.instance.CompleteLevel (currentStars);
			CompleteLevel ();
		}

	}

	void CompleteLevel(){

		StartCoroutine("ScaleUpTime");

		gameObject.GetComponent<CompletedLevelButtons>().StartCoroutine("StartAnimation");

		mSkipAnimationsCalled = true;
	}

	/*     	//Tools used are rated <=             
			//Moves used are rated <=
			//Robots used are rated <=
			//Words on Level select should represent the color they received for that score.
	*/

	int RateMoves(){

		if (Currentlevel.instance.mID == 0) {
			return 1;
		}

		if(Currentlevel.instance.mMoves > Currentlevel.mLevelData.mRequiredMovesForColors){
			return 0;
		}else {
			return 1;
		}
	}

	int RateTools(){

		if (Currentlevel.instance.mID == 0) {
			return 1;
		}

		if(Currentlevel.instance.mMaxToolsUsed > Currentlevel.mLevelData.mRequiredToolsForColors){
			return 0;
		}else{
			return 1;
		}
	}

	int RateRobots(){

		if (Currentlevel.instance.mID == 0) {
			return 1;
		}

		if(Currentlevel.instance.mRobotsUsed > Currentlevel.mLevelData.mRequiredRobotsForColors){
			return 0;
		}else{
			return 1;
		}
	}

	public void SkipAnimations(){

		if (!mSkipAnimationsCalled) {

			mSkipAnimationsCalled = true;
			StopAllCoroutines ();

			int rating = 0;
			mLabelsTracker = 0;

			double tempLerpValue = System.Math.Round (Currentlevel.mLevelPlayTime, 2);
			mTimeLabel.text = tempLerpValue.ToString ();

			mLabels [0].text = Currentlevel.instance.mMoves.ToString ();
			rating = RateMoves ();
			mStars += ChangeLabelColor (rating);
			ChangeLabelColor (rating);

			mLabelsTracker++;
			mLabels [1].text = Currentlevel.instance.mRobotsUsed.ToString ();

			rating = RateTools ();
			mStars += ChangeLabelColor (rating);
			ChangeLabelColor (rating);

			mLabelsTracker++;
			mLabels [2].text = Currentlevel.instance.mMaxToolsUsed.ToString ();
			rating = RateRobots ();
			mStars += ChangeLabelColor (rating);
			ChangeLabelColor (rating);

			for (int i = 0; i < Mathf.CeilToInt(mStars)/1 && i < 3; i++) {

				mStarOnOff [i].StarOn ();
			}

			Currentlevel.instance.CompleteLevel((int)mStars);

			//gameObject.GetComponent<CompletedLevelButtons>().SkipAnimationsLoad();

			gameObject.GetComponent<CompletedLevelButtons>().StartCoroutine("StartAnimation");

		}

	}



}
