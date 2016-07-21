using UnityEngine;
using System.Collections;

public class RobotSpawnLabel : MonoBehaviour {

	private int mRobotsToSpawn;
	
	void Start(){
		mRobotsToSpawn = Camera.main.GetComponent<MainCameraGUI>().mMaxRobotsReleased;

		if (PlayerData.instance.mUnlimitedRobotsUnlocked) {
			this.gameObject.SetActive(false);
		} else {
			this.gameObject.GetComponent<UILabel> ().text = mRobotsToSpawn.ToString ();
		}
	}

	void UpdateLabel(){

		if(mRobotsToSpawn > 0){
			mRobotsToSpawn--;
			if(this.gameObject.GetComponent<UILabel>() != null){
				this.gameObject.GetComponent<UILabel>().text = mRobotsToSpawn.ToString();
			}
		}
	}
		
	void Destroy(){
		EventHandler.OnRobotSpawn -= UpdateLabel;
	}

	
	void OnEnable(){
		EventHandler.OnRobotSpawn += UpdateLabel;
	}
	
	void OnDisable(){
		EventHandler.OnRobotSpawn -= UpdateLabel;
	}
}
