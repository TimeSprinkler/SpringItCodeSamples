using UnityEngine;
using System.Collections;

public class Currentlevel: MonoBehaviour{

	public int mID;

	public static Currentlevel instance{
		get{
			if(Currentlevel.mCurrentInstance == null){
				GameObject obj = new GameObject();
				GameObject.DontDestroyOnLoad(obj);
				obj.name = "CurrentLevelData";
				Currentlevel.mCurrentInstance = obj.AddComponent<Currentlevel>();
			}
			
			return Currentlevel.mCurrentInstance;
		}
	}
	
	private static Currentlevel mCurrentInstance;

	public static LevelData	mLevelData;
	
	[HideInInspector]public int mStars;
	[HideInInspector]public int mMoves;
	[HideInInspector]public int mRobotsUsed;

	[HideInInspector]public int mMaxToolsUsed = 0;
	[HideInInspector]public int mToolsUsed;

	[HideInInspector]public bool mIsLevelComplete = false;
	[HideInInspector]public float mTimeOfLevelStart;

	public static int mNumberOfFails = 0;
	public static float mLevelPlayTime = 0.0f;

	private int mMaxTools;

	void OnApplicationQuit(){
		SaveAndLoad.Save ();
	}

	public void OnApplicationPause(){
		SaveAndLoad.Save ();
	}

	public void NewLevel(int id){
		ClearLevelStats();

		mLevelPlayTime = 0.0f;
		mTimeOfLevelStart = Time.realtimeSinceStartup;
		mNumberOfFails = 0;

		ImportLevelData(id);
		mCurrentInstance.mID = id;
	
		EventHandler.OnRobotSpawn += RobotCreated;
		EventHandler.OnObjectMove += ObjectMoved;
		EventHandler.OnToolPlaced += PlaceTool;
		EventHandler.OnDeleteTool += DeleteTool;
	}

	public void UpdateTotalRobots(int totalRobots){
		
		mRobotsUsed = 0;
	}

	public void UpdateTotalTools(int totalTools){
		
		mMaxTools = totalTools;
		mToolsUsed = 0;
		mMaxToolsUsed = 0;
	}

	public void ClearLevelStats(){
		mStars = 0;
		mMoves = 0;
		mToolsUsed = 0;
		mRobotsUsed = 0;


		EventHandler.OnRobotSpawn -= RobotCreated;
		EventHandler.OnObjectMove -= ObjectMoved;
		EventHandler.OnToolPlaced -= PlaceTool;
		EventHandler.OnDeleteTool -= DeleteTool;
	}

	//When a robot is instantiated into the scene, relavent level data must be updated
	private void RobotCreated(){
		mRobotsUsed++;

		if (Currentlevel.instance.mID != 0) {
			PlayerData.instance.mNumberOfRobots--;
		}

		SaveAndLoad.Save ();
	}

	private void ImportLevelData(int id){

		mLevelData = PlayerData.instance.RetrieveLevelData(id);
		UpdateTotalTools(mLevelData.mToolsUsed);
	}

	private void ObjectMoved(){

		mMoves++;
	}

	private void PlaceTool(){

		if (mToolsUsed <= mMaxTools) {
			mToolsUsed++;

			if (mToolsUsed > mMaxToolsUsed) {
				mMaxToolsUsed = mToolsUsed;
			}
		}

		if (Currentlevel.instance.mID == 0) {
			mToolsUsed++;
		}
	}

	private void DeleteTool(){

		mToolsUsed--;

	}

	public void CompleteLevel(int stars){
	
		mIsLevelComplete = true;

		ExportData (stars);
	}

	//Overwrite the Level data and store it back with player data
	public void ExportData(int stars){

		if(stars >= mLevelData.mNumberOfStars){
			
			mLevelData.mIsComplete = mIsLevelComplete;
			mLevelData.mMoves = mMoves;
			mLevelData.mRobotsUsed = mRobotsUsed;
			mLevelData.mToolsUsed = mToolsUsed;
			
			mLevelData.mTimeToComplete = mLevelPlayTime;
			mLevelData.mNumberOfFails = mNumberOfFails;
			
			mLevelData.mNumberOfStars = stars;
			
			PlayerData.instance.StoreLevelData(mLevelData);
		}

	}

	public void TutorialLevel(int levelNumber){

		instance.NewLevel(0);
		instance.mID = levelNumber * 10;
	}

	public void SetAsTutorialLevel(){

		LevelData tempLevel = PlayerData.instance.RetrieveLevelData (0);

		tempLevel.mID = 0;
		mID = 0;

		mLevelData = tempLevel;
		UpdateTotalTools(mLevelData.mToolsUsed);
	}

}
