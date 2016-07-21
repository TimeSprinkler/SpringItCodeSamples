using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotSpawner : MonoBehaviour {

	public float mSpawnTime;
	public AudioContainer mSpawnerAudioContainer;
	public SpawnerDoor mSpawnerDoorScript;

	[SerializeField] private GameObject mRobot;
	[SerializeField] private Transform mSpawnPoint;
	[SerializeField] private int mRobotsToSpawn = 0;
	
	private float mTimeSinceCreation = 0.0f;
	private float mNextSpawn;
	private bool mCanSpawnRobot = true;

	private List<GameObject> mRobotList = new List<GameObject>();

	void Update(){

		mTimeSinceCreation += Time.deltaTime;

		if(mRobotsToSpawn > 0){
			if(mTimeSinceCreation > (mNextSpawn)){
				mRobotList.Add(Instantiate(mRobot, mSpawnPoint.position, mRobot.transform.rotation) as GameObject);
				mRobotList[mRobotList.Count - 1].GetComponent<Robot>().mLockedPosition = new Vector3(0,90,0);
				EventHandler.CallRobotSpawned();
				mNextSpawn = mTimeSinceCreation + mSpawnTime;
				mRobotsToSpawn--;
				mCanSpawnRobot = true;
			}
		}
	}

	public void SpawnOneRobot(){

		if(mCanSpawnRobot){
			mSpawnerDoorScript.OpenDoor();

			mRobotsToSpawn++;
			mCanSpawnRobot = false;
			Camera.main.GetComponent<MainCameraGUI>().mMaxRobotsReleased--;

			if(mSpawnerAudioContainer != null){
				mSpawnerAudioContainer.PlayRobotInterractionEffect();
			}
		}
	}
}
