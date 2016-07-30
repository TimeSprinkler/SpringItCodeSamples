using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class PlayerData{

	public static PlayerData instance;

	private int mNumberofStars = 0;
	public int mNumberOfStarsEarned{

		get{ return mNumberofStars;}
		set{ 
			if (value > mNumberofStars) {
				mNumberofStars = value;
			}
		}
	}

	public int mLevelsInGame = 51;
	public List<LevelData> mLevelDataList;
	public LevelData mCurrentSelectedLevel;

	public bool mMusicVolumeOn = true;
	public bool mSFXVolumeOn = true;

	public int mNumberOfRobots = 30;//strictly the starting value

	private int mLastTutorialSeenValue = 0;//strictly the starting value
	public int mLastTutorialSeen{

		get{ return mLastTutorialSeenValue;}

		set{
			if(value > mLastTutorialSeenValue){
				mLastTutorialSeenValue = value;
			}
		}
	}

	public int mNumberOfRobotsPerDay = 5;

	private bool mUnlimitedRobots = false;
	public bool mUnlimitedRobotsUnlocked{

		get{ return mUnlimitedRobots;}

		set{
			if (value) {
				mUnlimitedRobots = value;
			}
		}
	}

	public DateTime mTimeToAwardNewRobots = new DateTime(2016, 1, 1);
	public DateTime mLastLogonTime = new DateTime(2016, 1, 1);

	//This is the serializableData
	public int[] mTimeToAwardNewRobotsArray ={2016, 1, 1};
	public int[] mLastLogonTimeArray = {2016, 1, 1};


	public static void NewData(){

		if(PlayerData.instance == null){
			instance = new PlayerData();
			UnpackLevelDesignerData.instance.LoadDesignerData();

			PlayerData.instance.mNumberOfRobots = 30;

			PlayerData.instance.mLastLogonTime = DateTime.Now;
			PlayerData.instance.mTimeToAwardNewRobots = PlayerData.instance.mLastLogonTime.AddDays(1.0);

			PlayerData.instance.mLastLogonTimeArray = PlayerData.instance.ConvertToIntArray(PlayerData.instance.mLastLogonTime);
			PlayerData.instance.mTimeToAwardNewRobotsArray = PlayerData.instance.ConvertToIntArray(PlayerData.instance.mTimeToAwardNewRobots);
#if UNITY_ANDROID
			PlayerData.instance.mUnlimitedRobotsUnlocked = PlayerData.instance.CheckPurchases();

#endif
		}


	}

	private void LoadInNewData(PlayerData newData){
		if(PlayerData.instance != null){
			PlayerData.instance = newData;
		}
	}

	public LevelData RetrieveLevelData(int id){
		for(int i =0; i < instance.mLevelDataList.Count;i++){
			if(instance.mLevelDataList[i].mID == id){
				return instance.mLevelDataList[i];
			}
		}

		return new LevelData(id);
	}

	public void StoreLevelData(LevelData levelData){

		instance.mLevelDataList[levelData.mID].mRobotsUsed = levelData.mRobotsUsed;
		instance.mLevelDataList[levelData.mID].mToolsUsed = levelData.mToolsUsed;
		instance.mLevelDataList[levelData.mID].mMoves = levelData.mMoves;
		instance.mLevelDataList[levelData.mID].mIsComplete = levelData.mIsComplete;
		instance.mLevelDataList[levelData.mID].mNumberOfStars = levelData.mNumberOfStars;
		instance.mLevelDataList[levelData.mID].mTimeToComplete = levelData.mTimeToComplete;

		UpdateTotalStartsEarned ();
		SaveAndLoad.Save ();
	}

	public void UpdateMusicVolume(bool state){		
		mMusicVolumeOn = state;
		EventHandler.CallMusicToggled (state);
	}
	
	public void UpdateSFXVolume(bool state){
		mSFXVolumeOn = state;
		EventHandler.CallSFXToggled (state);
	}

	public void UpdateTotalStartsEarned(){

		int tempStars = 0;

		for(int i = 1; i < mLevelDataList.Count; i++) {		//thsi starts at 1 so the tutorial is not counted
			tempStars += mLevelDataList[i].mNumberOfStars;

		}

		instance.mNumberOfStarsEarned = tempStars;

#if UNITY_EDITOR
		//GameObject tempGameObject = GameObject.FindGameObjectWithTag ("GameManager");
		//if (tempGameObject.GetComponent<CheatsForDesigner>() != null) {
			//instance.mNumberOfStarsEarned += tempGameObject.GetComponent<CheatsForDesigner> ().mNumberOfCheatStars;
		//}
#endif
	}

	public void AddDailyRobotCheck(){

		mLastLogonTime = ConvertToDateTime (mLastLogonTimeArray);
		mTimeToAwardNewRobots = ConvertToDateTime (mTimeToAwardNewRobotsArray);

		DateTime currentTime = System.DateTime.Now;

		if (currentTime < (mLastLogonTime - LogOnTimeManager.mAntiCheatTime)) {
			//We need to display a message here
			mNumberOfRobots -= mNumberOfRobotsPerDay;

			LevelSelectEventHandler.CallDailyRobotCheck(-10);

		}else if (currentTime >= mTimeToAwardNewRobots) {//Award robots

			mNumberOfRobots += mNumberOfRobotsPerDay;

			mTimeToAwardNewRobots = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day);
			mTimeToAwardNewRobots = new DateTime(mTimeToAwardNewRobots.Year, mTimeToAwardNewRobots.Month, mTimeToAwardNewRobots.Day + 1);

			LevelSelectEventHandler.CallDailyRobotCheck(10);
		}

		mLastLogonTime = currentTime;

		mLastLogonTimeArray = ConvertToIntArray (mLastLogonTime);
		mTimeToAwardNewRobotsArray = ConvertToIntArray (mTimeToAwardNewRobots);

#if UNITY_EDITOR
		GameObject tempGameObject = GameObject.FindGameObjectWithTag ("GameManager");
		if (tempGameObject.GetComponent<CheatsForDesigner>() != null) {
			if(tempGameObject.GetComponent<CheatsForDesigner>().mUnlimitedRobotsUnlocked){
				mUnlimitedRobotsUnlocked = true;
			}
		}
#endif

		if (PlayerData.instance.mUnlimitedRobotsUnlocked) {
			mNumberOfRobots = 999;//When the if statement is commented out. Everyone gets unlimited robots for playtesting
		}
	}

	public void CloseTutorialPage(int numberOfTutorialPages){

		mLastTutorialSeen = Currentlevel.instance.mID * 10 + numberOfTutorialPages;
	}

	void OnApplicationQuit(){
		SaveAndLoad.Save ();
	}

	int[] ConvertToIntArray(DateTime date){

		int[] array = new int[3];

		array [0] = date.Year;
		array [1] = date.Month;
		array [2] = date.Day;

		return array;
	}

	DateTime ConvertToDateTime(int[] array){

		DateTime date = new DateTime (array [0], array [1], array [2]);

		return date;
	}

	private bool CheckPurchases(){

		int itemBalance = Soomla.Store.StoreInventory.GetItemBalance ("unlimited_robots");

		if (itemBalance > 0) {
			return true;
		} else {
			return false;
		}

	}

}
