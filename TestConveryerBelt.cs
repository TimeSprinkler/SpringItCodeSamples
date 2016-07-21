using UnityEngine;
using System.Collections;

public class TestConveryerBelt : MonoBehaviour {

	public int mConveyerSpeed;
	public AudioSource mAudioSource;
	//public AudioClip mRobotInteractionSound;
	//public AudioClip mAmbientSound;//This will cause problems if we play it right now

	private Vector3 mCurrentVelocity;
	private Vector3 mMoveDirection;
	
	void Start(){

		UpdateMovingDirection();
		/*if(mAmbientSound != null){
			PlaySound(mAmbientSound);
		}*/

	}
	
	void OnTriggerStay(Collider collider){

		if(collider.gameObject.tag == "Robot"){

			UpdateMovingDirection();
			mCurrentVelocity = collider.attachedRigidbody.velocity;
			collider.attachedRigidbody.velocity = new Vector3 (mMoveDirection.x * mConveyerSpeed, mCurrentVelocity.y, 0);
			//StartCoroutine("PlayRobotInterractionSound");
		}
	}

	void UpdateMovingDirection(){

		mMoveDirection = new Vector3(0,0,0);
		mMoveDirection.x = transform.position.x - transform.parent.transform.position.x;
		mMoveDirection = mMoveDirection.normalized;
	
	}

	/*IEnumerator PlayRobotInterractSound(){
		float robotInterractionClipTime = mRobotInteractionSound.length;
		PlaySound (mRobotInteractionSound);
		yield return new WaitForSeconds(robotInterractionClipTime);
		PlaySound (mAmbientSound);

	}

	void PlaySound(AudioClip clip){
		mAudioSource.clip = clip;
		mAudioSource.Play ();
	}*/

}
