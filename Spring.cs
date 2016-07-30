using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {
	
	public float mLaunchHeight;
	public float mLaunchDistance;
	public Animation mSpringAnimation;
	[Range (0.1f, 1.0f)]
	public float mSpeedCorrectionSmoothness = 0.6f;

	private float mXLaunchSpeed;
	private float mYLaunchSpeed;
	private Vector3 mRobotTravelDirection = new Vector3(0,0,0);

	private bool mCoroutineIsRunning = false;

	void Awake(){
		if(mSpringAnimation != null){
			mSpringAnimation.Play();
		}
	}

	void Start(){

		CalculateLaunchSpeeds();

	}

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Robot") {
					
			collider.GetComponent<Robot>().mTimeToReachSpringLaunchPoint = SetLaunchTime(collider);
		}
	}

	float SetLaunchTime(Collider collider){
		mRobotTravelDirection = collider.attachedRigidbody.velocity;//This is velocity Initial
		
		float ySize = this.GetComponent<BoxCollider>().size.y;
		float yTravelDistance = ySize*2;
		float sqrtValue = (mRobotTravelDirection.y + 2f * Physics.gravity.y * yTravelDistance);
		float velocityFinal = Mathf.Sqrt(sqrtValue);

		return (velocityFinal - mRobotTravelDirection.y)/Physics.gravity.y;
	}

	void OnTriggerExit(Collider collider){
		if (collider.gameObject.tag == "Robot") {
			collider.GetComponent<Robot>().mHasLaunched = false;
		}
	}

		
	void OnTriggerStay(Collider collider){

		if(collider.gameObject.tag == "Robot"){
			mRobotTravelDirection = collider.attachedRigidbody.velocity;
			CalculateLaunchSpeeds();

			if(!collider.GetComponent<Robot>().mHasLaunched){
				if(AtLaunchPoint(collider)){
					LaunchRobot(collider);
				}else{
					ModifyLaunch(collider);
				}
			}
			//Only modify horizontal when lerping
		}
	}

	void ModifyLaunch(Collider collider){

		if (!mCoroutineIsRunning) {
			StartCoroutine(ModifyRobotSpeed(collider));	
		
		}
	}

	IEnumerator ModifyRobotSpeed(Collider collider){

		mCoroutineIsRunning = true;

		float timeToReachLP = collider.GetComponent<Robot> ().mTimeToReachSpringLaunchPoint;
		float launchPoint = this.GetComponent<BoxCollider> ().center.x;
		float currentRobotPosition = collider.transform.position.x;
		float distanceToPoint = currentRobotPosition - launchPoint;

		while (timeToReachLP > 0) {

			currentRobotPosition = collider.transform.position.x;
			distanceToPoint = currentRobotPosition - launchPoint;
			//Change the vel by a percentage of the velocity needed and round up

			float velocityRequired = distanceToPoint/timeToReachLP;


			//Modify Velocity
			Vector3 tempVector = collider.rigidbody.velocity;
			tempVector.x = mSpeedCorrectionSmoothness * velocityRequired + (1.0f - mSpeedCorrectionSmoothness) * tempVector.x;
			collider.rigidbody.velocity = tempVector;

			timeToReachLP -= Time.deltaTime;

			//mSpeedCorrectionSmoothness
			yield return new WaitForEndOfFrame();
		}

		mCoroutineIsRunning = false;
	}

	void LaunchRobot(Collider collider){

		collider.attachedRigidbody.velocity = new Vector3 (mXLaunchSpeed, mYLaunchSpeed, 0);

		FaceSpringInCorrectDirection();		
		
		if(gameObject.GetComponent<VFXContainter>() != null){
			gameObject.GetComponent<VFXContainter>().PlayRobotInterraction(0.0f);
			if(mSpringAnimation != null){
				mSpringAnimation.Play();
			}
		}
		
		if(gameObject.GetComponent<AudioContainer>() != null){
			gameObject.GetComponent<AudioContainer>().PlayRobotInterractionEffect();
		}
	}

	
	void CalculateLaunchSpeeds(){
		
		float gravity = Physics.gravity.magnitude;
		float totalFlightTime;

		mRobotTravelDirection.y = 0;
		mRobotTravelDirection.z = 0;
		mRobotTravelDirection = mRobotTravelDirection.normalized;

		mYLaunchSpeed = Mathf.Sqrt (2f *gravity *mLaunchHeight);
		totalFlightTime = 2f * mYLaunchSpeed/gravity;
		mXLaunchSpeed = mRobotTravelDirection.x * mLaunchDistance/totalFlightTime;
		
	}
	void FaceSpringInCorrectDirection(){

		Vector3 springFacingDirection = transform.parent.GetComponentInChildren<Animation>().transform.right;

		//transform.right need to be -1 for the animation to play right
		if(mRobotTravelDirection == transform.right){//Robot Going Right

			if(mRobotTravelDirection == springFacingDirection){//Spring facing left, turn around
				transform.parent.GetComponentInChildren<Animation>().gameObject.transform.Rotate( 0, 180, 0);
			}
		}else if(mRobotTravelDirection == springFacingDirection){//Robot going left spring not facing left, turn around

			transform.parent.GetComponentInChildren<Animation>().gameObject.transform.Rotate(0, 180, 0);
		}
	}

	bool AtLaunchPoint(Collider collider){


		return true;
	}
}
