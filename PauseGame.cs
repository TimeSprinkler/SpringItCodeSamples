using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseGame : MonoBehaviour {

	private string mMovingObjectTag = "MovingPart";
	private string mVisualEffectsTag = "VisualEffect";
	private string mRobotTag = "Robot";

	private List<GameObject> mPausedRobots = new List<GameObject>();
	private List<Vector3> mPausedRobotVelocitys = new List<Vector3>();

	public void Pause(){

		StopRobotsMovement();

		ToggleMovingThings(false);
		ToggleEffects(false);


	}

	public void Resume(){

		ResumeRobotsMovement();

		ToggleMovingThings(true);
		ToggleEffects(true);
	}

	void ToggleMovingThings(bool state){

		GameObject[] movingObjects = GameObject.FindGameObjectsWithTag(mMovingObjectTag);

		for(int i = 0; i < movingObjects.Length;i++){

			if(movingObjects[i].GetComponent<MonoBehaviour>() != null){
				movingObjects[i].GetComponent<MonoBehaviour>().enabled = state;
				if(movingObjects[i].GetComponent<Collider>() != null){
					movingObjects[i].GetComponent<Collider>().enabled = state;
				}
			}
		}
	}

	void ToggleEffects(bool state){

		GameObject[] visualEffectObjects = GameObject.FindGameObjectsWithTag(mVisualEffectsTag);

		for(int i = 0; i < visualEffectObjects.Length;i++){

			if(visualEffectObjects[i].GetComponent<ParticleSystem>() != null){
				if(state){
					visualEffectObjects[i].GetComponent<ParticleSystem>().Play();
				}else{
					visualEffectObjects[i].GetComponent<ParticleSystem>().Pause ();
				}
			}

			if(visualEffectObjects[i].GetComponent<Animation>() != null){
				visualEffectObjects[i].GetComponent<Animation>().enabled = state;
			}

		}

	}

	void StopRobotsMovement(){

		GameObject[] robots = GameObject.FindGameObjectsWithTag(mRobotTag);
		
		for(int i = 0; i < robots.Length;i++){
			
			mPausedRobots.Add (robots[i]);
			mPausedRobotVelocitys.Add (mPausedRobots[i].rigidbody.velocity);

			mPausedRobots[i].rigidbody.isKinematic = true;
			mPausedRobots[i].rigidbody.useGravity = false;
		}

	}

	void ResumeRobotsMovement(){

		for(int i = 0; i < mPausedRobots.Count;i++){
			
			mPausedRobots[i].rigidbody.isKinematic = false;
			mPausedRobots[i].rigidbody.useGravity = true;
			mPausedRobots[i].rigidbody.velocity = mPausedRobotVelocitys[i];
		
		}

		mPausedRobots.Clear();
		mPausedRobotVelocitys.Clear ();

	}
}
