using UnityEngine;
using System.Collections;

public class LevelSelectEventHandler : UnitySingleton<LevelSelectEventHandler>  {


	public delegate void LevelIconSelected(GameObject incomingGameObject);
	public static event LevelIconSelected OnLevelIconSelected;
	public static void CallLevelIconSelected(GameObject incomingGameObject){
		if(OnLevelIconSelected != null){
			OnLevelIconSelected(incomingGameObject);
		}
	}

	public delegate void StartButtonPressed();
	public static event StartButtonPressed OnStartButtonPress;
	public static void CallStartButtonPressed(){
		if (OnStartButtonPress != null) {
			OnStartButtonPress();
		}
	}

	public delegate void DailyRobotCheckCalled(int robots);
	public static event DailyRobotCheckCalled OnDailyCheckCalled;
	public static void CallDailyRobotCheck(int robots){
		if (OnDailyCheckCalled != null) {
			OnDailyCheckCalled(robots);
		}

	}



}
