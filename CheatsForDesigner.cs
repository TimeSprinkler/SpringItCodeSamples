#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class CheatsForDesigner : UnitySingleton<CheatsForDesigner> {

	public int mNumberOfCheatStars;
	public bool mReset = false;
	public bool mUnlimitedRobotsUnlocked = false;
	public int mNumberOfStartingRobots = 50;

	void Start(){

		DontDestroyOnLoad(gameObject);


	}

	void Awake(){

		if (mReset) {
			Debug.Log ("Reset");
			PlayerData.NewData();
			SaveAndLoad.Save();
			mReset = false;
		}

	}

	void OnLevelWasLoaded(){

		if (PlayerData.instance != null) {
			PlayerData.instance.UpdateTotalStartsEarned();
		}

	}


}
#endif
