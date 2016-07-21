using UnityEngine;
using System.Collections;

public class RobotScoreLabel : MonoBehaviour {

	public GameObject mCompletedLevelDisplayProjectors;
	public int mEndGameWaitForPromptTime = 1;
	public bool mGameIsEnding = false;

	private int mRobotsToWin;
	private int mRobotsScored = 0;
	private int mRobotsAvailable;

	private bool mIsRunning = false;
	private bool mEndGameIsRunning = false;

	void Awake(){
		mIsRunning = true;
	}

	void Start(){

		mRobotsToWin = Camera.main.GetComponent<MainCameraGUI>().mRobotsNeededToWin;
		mRobotsAvailable = PlayerData.instance.mNumberOfRobots;
		this.gameObject.GetComponent<UILabel>().text = mRobotsAvailable.ToString();

	}

	void Update(){
		if(mIsRunning){
			Currentlevel.mLevelPlayTime += Time.deltaTime;
		}
	}

	void OnEnable(){
		EventHandler.OnRobotScored += UpdateScore;
		EventHandler.OnRobotSpawn += UpdateLabel;
	}

	void OnDisable(){
		EventHandler.OnRobotScored -= UpdateScore;
		EventHandler.OnRobotSpawn -= UpdateLabel;
	}

	void UpdateScore(GameObject robot){
		mRobotsScored++;
			
		if(mRobotsScored >= mRobotsToWin && !mEndGameIsRunning)	StartCoroutine(EndGame());
	}

	public void UpdateLabelAmount(int amount){
		mRobotsAvailable += amount;
		this.gameObject.GetComponent<UILabel>().text = mRobotsAvailable.ToString();

	}

	void UpdateLabel(){
		UpdateLabelAmount (-1);
	}

	IEnumerator EndGame(){

		mEndGameIsRunning = true;

		Currentlevel.instance.mIsLevelComplete = true;
		Currentlevel.instance.ExportData (0);

		SaveAndLoad.Save ();

		Camera.main.GetComponent<StartAndReset>().StopGame();
		EventHandler.CallLevelCompleted();
		mIsRunning = false;

		Camera.main.GetComponentInChildren<CompletedLevelButtons> ().SetAllAlphasCorrectly (0.0f);

		//Rest time scale
		Time.timeScale = 1.0f;

		yield return new WaitForSeconds(mEndGameWaitForPromptTime);

		mCompletedLevelDisplayProjectors.GetComponent<SlideMenu>().StartArmsMoving();
				
	}

	void Destroy(){
		//EventHandler.OnRobotScored -= UpdateLabel;
	}
}
