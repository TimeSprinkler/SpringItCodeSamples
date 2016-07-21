using UnityEngine;
using System.Collections;
//using UnityEditor;


public class RestartButton : MonoBehaviour {

	public void RestartLevel(){

		StartAndReset.mIsGameRunning = false;
		Currentlevel.instance.NewLevel (Currentlevel.instance.mID);
		Application.LoadLevel(Currentlevel.instance.mID.ToString() + " SpringItLevel");
	}
}
