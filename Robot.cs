using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {

	public float mRobotDestroyTime = 0.5f;
	public int mRobotPoints = 0;
	public int mConsectutiveSprings = 0;

	public AudioSource mRobotDeathSound;
	public RobotVisualEffect mRobotDeathVFX;

	[HideInInspector] public bool mGoingForwardOnSpawner = false;
	[HideInInspector] public bool mRotationUnlocked = false;
	[HideInInspector] public Vector3 mLockedPosition = Vector3.zero;

	[HideInInspector] public bool mHasLaunched = false;
	[HideInInspector] public float mTimeToReachSpringLaunchPoint = -1.0f;


	private Collider mCollidingObject;

	void OnDestroy() {

		EventHandler.CallRobotDestroyed(this.gameObject);
		
	}

	void Update () {

	}

	void OnTriggerEnter(Collider incomingCollider){

		if(mCollidingObject != null){

			if(incomingCollider.gameObject.tag != "Robot" && incomingCollider.gameObject.tag != "Wall"){
				mCollidingObject = incomingCollider;
				StopCoroutine("DestroyIfUseless");
				
			}
			if(incomingCollider.gameObject.tag == "Floor"){

				StartCoroutine("DestroyIfUseless");
			}
			
		}else{
			mCollidingObject = incomingCollider;
			StopCoroutine("DestroyIfUseless");
			
		}
		
	}

	void OnTriggerExit(Collider incomingCollider){

		if(mCollidingObject != null){
			if(incomingCollider.gameObject.tag == "Floor" || mCollidingObject.gameObject.tag == "Wall"){
				mCollidingObject = null;
			}
		}
	}

	void OnTriggerStay(Collider incomingCollider){

		if(mCollidingObject != null){

			if(incomingCollider.gameObject.tag == "Wall"){
				mCollidingObject = incomingCollider;
			}

			if(incomingCollider.gameObject.tag == "Floor"){
				mCollidingObject = incomingCollider;
			}		
		}

		//Debug.Log (mCollidingObject.name);
	}

	IEnumerator DestroyIfUseless(){

		Vector3 previousPosition = this.transform.position;

		yield return new WaitForSeconds(mRobotDestroyTime);

		if (mCollidingObject != null) {
			if (mCollidingObject.gameObject.tag == "Floor" || mCollidingObject.gameObject.tag == "Wall") {
				this.gameObject.GetComponent<RobotVisualEffect> ().PlayExplosion ();
			} else if (previousPosition == this.transform.position) {
				this.gameObject.GetComponent<RobotVisualEffect> ().PlayExplosion ();

			}
		}

		StartCoroutine (DestroyIfUseless ());//It will keep going until it explodes

	}

}
