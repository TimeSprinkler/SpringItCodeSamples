using UnityEngine;
using System.Collections;

public class StartAndReset : MonoBehaviour {
	
	public GameObject[] mStoredPrefabs;
	
	private GameObject[] mAllObjects;
	private Vector3[] mAllLevelPiecePositions;
	private Quaternion[] mAllLevelPieceRotations;
	private string[] mLevelPieceNames;
	public static bool mIsGameRunning = false;
	
	private GameObject[] mAllMovingParts;
	private string[] mMovingPartNames;
	private InBuildDebugging mDebuggingScript;


	void OnDestroy() {
		if(BotlingTrail.isValid){
			BotlingTrail.instance.Reset();
		}
	}	

	void Update (){

		if(Input.GetKeyDown(KeyCode.Space)){
			if(!mIsGameRunning){
				StartGame();
			}
			else{

				StopGame();
			}
		}
	}

	public void StartGame(){

		if(!mIsGameRunning){
			BotlingTrail.instance.Reset();
			BotlingTrail.instance.DisableRendering();
			mIsGameRunning = true;

		}
	}
		
	public void StopGame(){
		if(mIsGameRunning){
			BotlingTrail.instance.EnableRendering();
			DeleteRobots();
			mIsGameRunning = false;
		}
	}

	void DeleteRobots(){
		GameObject[] allRobots = GameObject.FindGameObjectsWithTag("Robot");

		for(int i = 0; i < allRobots.Length; i++){
			Destroy(allRobots[i]);
		}	
	}

	
	void TogglePlacer(bool state){
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Placer>().enabled = state;
		
	}

	public bool IsGameRunning(){
		return mIsGameRunning;
	}

}
