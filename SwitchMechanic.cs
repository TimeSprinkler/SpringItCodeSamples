using UnityEngine;
using System.Collections;

public class SwitchMechanic : MonoBehaviour {

	public GameObject mDoorObject;
	[HideInInspector]
	public int mButtonID;
	public Color mColor;
	public AudioContainer mSwitchAudioContainer;

	private float mLastCall;
	private float mCallInterval = 0.5f;

	void Awake(){

		mDoorObject.GetComponent<DoorMechanic>().AddSwitch(mButtonID);
		gameObject.renderer.material.color = new Color(mColor.r, mColor.g, mColor.b, 113f/256f);

		if(gameObject.GetComponent<Light>() != null){
			gameObject.GetComponent<Light>().color = mColor;
		}

		mLastCall = Time.time;


	}

	void OnTriggerEnter(Collider collider){

		if(collider.tag == "Robot"){
			ActivateDoor();
			if(Time.time >= mLastCall + mCallInterval){
				mLastCall = Time.time;
				//EventHandler.CallRobotPoints(Currentlevel.instance.mSwitchActivationPoints);
			}
		}
	}

	void ActivateDoor(){
		mDoorObject.GetComponent<DoorMechanic>().Switched(mButtonID);

		if(mSwitchAudioContainer != null){
			mSwitchAudioContainer.PlayRobotInterractionEffect();
		}

		this.gameObject.SetActive(false);
	}
}
