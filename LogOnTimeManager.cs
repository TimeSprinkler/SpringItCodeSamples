using UnityEngine;
using System.Collections;
using System;

public class LogOnTimeManager : MonoBehaviour {

	public DateTime mCurrentTime;

	public static TimeSpan mAntiCheatTime = new TimeSpan(19,0,0);//19 hours


	void OnLevelWasLoaded(int levelInt){
		mCurrentTime = System.DateTime.Now;

		if (levelInt == 2) {//2 is Level Select Screen

			PlayerData.instance.AddDailyRobotCheck ();
		}
	}

	void Awake(){
		DontDestroyOnLoad (this.transform);
	}
}
