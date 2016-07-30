using UnityEngine;
using System.Collections;

public class RetryLevel : MonoBehaviour {

	public void RestartLevel(){
		
		if(StartAndReset.mIsGameRunning){
			
			StartAndReset.mIsGameRunning = !StartAndReset.mIsGameRunning;
		}

		Currentlevel.instance.ExportData (0);
		Application.LoadLevel(Application.loadedLevel);
		
	}
}
