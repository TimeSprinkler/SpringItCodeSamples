using UnityEngine;
using System.Collections;

public class EndGoal : MonoBehaviour {

	public AudioContainer mCollectorAudioContainer;
	public int mSpawnerLayer;
	public SpawnerDoor mSpawnerDoorScript;

	void OnTriggerEnter(Collider collider){
		
		if(collider.gameObject.tag == "Robot"){
			EventHandler.CallRobotScored(collider.gameObject);
			collider.gameObject.layer = mSpawnerLayer;

			mSpawnerDoorScript.OpenDoor();

			GivePlayerOneRobotBack();

			if(mCollectorAudioContainer != null){
				mCollectorAudioContainer.PlayRobotInterractionEffect();
			}
		}
	}

	void GivePlayerOneRobotBack(){

		PlayerData.instance.mNumberOfRobots++;
		Camera.main.GetComponent<MainCameraGUI> ();
	}
}
