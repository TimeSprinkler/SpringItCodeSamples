using UnityEngine;
using System.Collections;

public class PlayGame : MonoBehaviour {

	public string mTutorial;
	public string mLevelSelectName;

	private bool mReveal = false;

	//Eventually I want to load the next level the player needs to beat unless that next level has alreadly been beaten

	void Awake(){
		SaveAndLoad.Load();
		SaveAndLoad.Save();
	}

	public void Play(){
		mReveal = true;
		if(PlayerData.instance.mLastTutorialSeen < 5){
			Application.LoadLevel(mTutorial);
		}else{
			Application.LoadLevel(mLevelSelectName);
		}
		mReveal = false;

	}

	void OnGUI(){
		if(mReveal){
			GUI.Label(new Rect(10, 30, Screen.width - 10, Screen.height - 10), "Called");
		}

	}
}
