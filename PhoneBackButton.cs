using UnityEngine;
using System.Collections;

public class PhoneBackButton : MonoBehaviour {


	private string mLevelSelectSceneName = "Level Select Screen";
	private string mMainMenuSceneName = "Main Menu";
	private string mHowToPlaySceneName = "How To Play";
	private string mStoreSceneName = "Store";
	private string mCreditsSceneName = "Credits";

	private GameObject mPausePanel;

	void Start(){

		mPausePanel = GameObject.FindGameObjectWithTag ("PauseMenu");

	}

	void Update(){

		if (Input.GetKeyDown (KeyCode.Escape)) {

			if (Application.loadedLevelName == mLevelSelectSceneName ||
				Application.loadedLevelName == mMainMenuSceneName ||
				Application.loadedLevelName == mHowToPlaySceneName ||
				Application.loadedLevelName == mStoreSceneName ||
				Application.loadedLevelName == mCreditsSceneName) {

				SaveAndLoad.Save();
				Application.Quit ();
			} else {

				if(mPausePanel != null){
					mPausePanel.SetActive(true);
				}
			}
		}
	}

}
