using UnityEngine;
using System.Collections;

public class CurrentSelectedLevel : MonoBehaviour {

	MenuLevelItem mCurrentLevelItem;


	void Start(){
		LevelSelectEventHandler.OnLevelIconSelected += NewIconSelected;
		LevelSelectEventHandler.OnStartButtonPress += LoadCurrentSelectedLevel;
	}

	void OnDestroy(){
		LevelSelectEventHandler.OnLevelIconSelected -= NewIconSelected;
		LevelSelectEventHandler.OnStartButtonPress -= LoadCurrentSelectedLevel;
	}

	private void NewIconSelected(GameObject incomingGameObject){

		mCurrentLevelItem = incomingGameObject.GetComponent<MenuLevelItem> ();

	}

	private void LoadCurrentSelectedLevel(){
		if (mCurrentLevelItem != null) {
			mCurrentLevelItem.LoadLevel ();
		}
	}
}
