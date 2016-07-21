using UnityEngine;
using System.Collections;

public class StartTargetLevel : MonoBehaviour {

	public string targetLevel;

	public void StartLevel(){

		StartAndReset.mIsGameRunning = false;
		if(targetLevel == "Level Select Screen"){

			EventHandler.CallLevelSelectLoaded();
		}
		Application.LoadLevel(targetLevel);
		
	}
}
