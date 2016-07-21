using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorMechanic : MonoBehaviour {

	public Animation mActivationAnimation;
	public AudioContainer mDoorAudioContainer;
	public Color mColor;
	public Color mDangerColor;
	public float mFlashTime;


	private List<int> mSwitchList = new List<int>();

	void Awake(){
		for(int i = 0; i < this.renderer.materials.Length; i++){
			this.renderer.materials[i].color = mColor;
		}
	}

	public void AddSwitch(int buttonID){
		mSwitchList.Add (buttonID);
	}

	public void Switched(int buttonID){
	
		mSwitchList.Remove (buttonID);

		if(mSwitchList.Count <= 0){
			TriggerEvent();
		}
	}

	private void TriggerEvent(){
		if(mActivationAnimation != null){
			mActivationAnimation.Play();
		}
		if(mDoorAudioContainer != null){
			mDoorAudioContainer.PlayRobotInterractionEffect();
		}
		this.gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider collider){
		if(collider.tag == "Robot"){
			if(mDoorAudioContainer != null){
				mDoorAudioContainer.PlayRobotInterractionEffect();
			}

			StartCoroutine("FlashColor");

			collider.gameObject.GetComponent<RobotVisualEffect>().PlayExplosion();
			collider.enabled = false;
		}
	}

	IEnumerator FlashColor(){

		Material[] materials = this.renderer.materials;
		Color baseColor = this.renderer.material.color;

		foreach(Material m in materials){
			m.color = mDangerColor;
		}

		yield return new WaitForSeconds(mFlashTime);

		foreach(Material m in materials){
			m.color = baseColor;
		}
	}
}
