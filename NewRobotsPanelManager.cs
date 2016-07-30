using UnityEngine;
using System.Collections;

public class NewRobotsPanelManager : MonoBehaviour {


	public GameObject mNewRobotsSprite;
	public GameObject mNegativeRobotsSprite;
	public GameObject mBackground;

	void Awake(){

		LevelSelectEventHandler.OnDailyCheckCalled += GainedRobots;
		mBackground.SetActive (false);

	}

	void OnDestroy(){
		LevelSelectEventHandler.OnDailyCheckCalled -= GainedRobots;
	}

	public void GainedRobots(int numOfRobots){

		if (!PlayerData.instance.mUnlimitedRobotsUnlocked) {

			mBackground.SetActive(true);

			if(numOfRobots > 0){
				mNewRobotsSprite.SetActive(true);

			}else if(numOfRobots < 0){
				mNegativeRobotsSprite.SetActive(true);
	
			}
		}

	}

}
