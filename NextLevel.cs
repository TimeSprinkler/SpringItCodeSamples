using UnityEngine;
using System.Collections;

public class NextLevel : MonoBehaviour {


	public void StartNextLevel(){

		if(StartAndReset.mIsGameRunning){

			StartAndReset.mIsGameRunning = !StartAndReset.mIsGameRunning;
		}

		if (Currentlevel.instance.mID % 10 != 0 || Currentlevel.instance.mID == 0) {

			EventHandler.CallNewLevelLoaded (Currentlevel.instance.mID + 1);
	
			if (Currentlevel.instance.mID == 0) {
				Currentlevel.instance.NewLevel (1);
			}
			Application.LoadLevel ((Currentlevel.instance.mID).ToString () + " SpringItLevel");
		} else {

			Application.LoadLevel("Level Select Screen");
		}

	}
}
