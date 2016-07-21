using UnityEngine;
using System.Collections;

public class Fan : MonoBehaviour {
	
	public float mFanSpeed;
	public AudioContainer mFanAudioContainer;
	private Vector3 mFanDirection;

	void OnTriggerStay(Collider collider){

		mFanDirection = (transform.position - transform.parent.transform.position).normalized;

		if(collider.gameObject.tag == "Robot"){
			collider.attachedRigidbody.AddForce (mFanDirection * mFanSpeed);

			if(mFanAudioContainer != null){
				mFanAudioContainer.PlayRobotInterractionEffect();
			}
		}
	}
}
