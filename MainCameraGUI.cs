using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
public class MainCameraGUI : MonoBehaviour {
	
	public GameObject mGoButton;
	public GameObject mRobotsLeftToSpawnLabel;
	public GameObject mOutOfRobotsMenu;
	public int mRobotsNeededToWin;
	public int mMaxRobotsReleased;
	public GameObject mMainSpawner;

	public int mOutOfRobotsWaitTimeUntilPrompt = 1;
		
	public static GameObject mPlayModeCamera;

	private int mCompletedRobots;

	//This needs to be on awake because other scripts depend on these static variables being defined
	void Awake () {

		EventHandler.OnRobotDestroyed += CheckIfNoRobotsAreActive;

		mGoButton.GetComponent<UIButton>().enabled = true;
		FindSpawner();

		mPlayModeCamera = GameObject.FindGameObjectWithTag("PlaymodeCamera").gameObject;


	}

	void Start(){
		GameObject.FindGameObjectWithTag("MusicSource").GetComponent<PlayMusic>().NewLevel(Currentlevel.instance.mID);

		SetRobotsAvailable ();

	}

	void OnDestroy(){

		EventHandler.OnRobotDestroyed -= CheckIfNoRobotsAreActive;
	}

	void Update() {


		if(mPlayModeCamera.activeSelf){

			CheckForButtonClicks();
		}
	}

	void FindSpawner(){

		GameObject[] potentialSpawners = GameObject.FindGameObjectsWithTag("Spawner or Collector");

		for(int i = 0; i < potentialSpawners.Length; i++){
			if(potentialSpawners[i].GetComponentInChildren<RobotSpawner>() != null){
					mMainSpawner = potentialSpawners[i];
			}
		}
	}
		
	public bool CanSpawnRobot(){
		
		if(0 < mMaxRobotsReleased){
		
			return true;
		}
		else{
			return false;	
		}	
	}

	void CheckForButtonClicks(){
		
		UIEventListener.Get(mGoButton).onClick += ButtonClicked;
	}
	
	void ButtonClicked(GameObject button){
		if(CanSpawnRobot()){

			mMainSpawner.GetComponentInChildren<RobotSpawner>().SpawnOneRobot();

		}else{
			CheckIfNoRobotsAreActive(gameObject);
		}
	}

	void CheckIfNoRobotsAreActive(GameObject robot){

		bool noRobotsRemain = false;

		if(!CanSpawnRobot()){
			GameObject[] robotsActive = GameObject.FindGameObjectsWithTag("Robot");
			if(robotsActive.Length == 0){
				noRobotsRemain = true;
			}

			GameObject[] cannons = GameObject.FindGameObjectsWithTag("Cannon");
			for(int i = 0; i < cannons.Length; i++){
				if(cannons[i].GetComponent<Cannon>().mStoredRobots > 0){
					noRobotsRemain = false;
				}
			}
		}

		if (PlayerData.instance.mNumberOfRobots > 0) {
			noRobotsRemain = false;
		}

		if(noRobotsRemain){
			StartCoroutine(PromptLevelRestart());
		}
	}

	IEnumerator PromptLevelRestart(){

		bool noRobotsRemain = false;

		yield return new WaitForSeconds(mOutOfRobotsWaitTimeUntilPrompt);

		GameObject[] robotsActive = GameObject.FindGameObjectsWithTag("Robot");
		if(robotsActive.Length == 0){
			noRobotsRemain = true;
		}
	
		if(noRobotsRemain){
			if(!mOutOfRobotsMenu.activeSelf){
				mOutOfRobotsMenu.GetComponent<OpenOrCloseMenu>().OpenOrClose();
			}
			if(mOutOfRobotsMenu.transform.parent.gameObject.GetComponent<UIMenuAnimations>() != null){
				mOutOfRobotsMenu.transform.parent.gameObject.GetComponent<UIMenuAnimations>().StartAnimations();
				mOutOfRobotsMenu.transform.parent.GetComponent<OpenOrCloseMenu>().Close ();
			}
		}
	}

	void SetRobotsAvailable(){

		if (PlayerData.instance.mUnlimitedRobotsUnlocked) {
			mMaxRobotsReleased = 1000000;
		} else {
			mMaxRobotsReleased = PlayerData.instance.mNumberOfRobots;
		}

	}

	void OnApplicationQuit() {
		PlayerData.instance.mNumberOfRobots = mMaxRobotsReleased;
		SaveAndLoad.Save ();

	}

	void OnApplicatiomPause() {
		PlayerData.instance.mNumberOfRobots = mMaxRobotsReleased;
		SaveAndLoad.Save ();
	}

}
