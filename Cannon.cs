using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	public int mStoredRobots;
	public int mLaunchVelocity;
	public int mMaxStorage;
	public float mRotationSpeed;
	public float[] mMaxRotationAngleFloor;
	public AudioContainer mCannonAudioContainer;
	public VFXContainter mVFXContainer;
	public bool isRotating = false;

	//Models
	[SerializeField]private GameObject mFloorArmature;
	[SerializeField]private GameObject mFloorGameObject;
	
	[SerializeField]private GameObject mLaunchPoint;
	[SerializeField]private GameObject mRobot;
	private Vector3 mRotation = new Vector3( 0,0,0);
	private int mRotationDirection = 1;


	private Vector3 mRestRotation = new Vector3 (0, 0, 0);
	private Vector3 mFloorRestRotation = new Vector3(-90, 0, 0);

	void OnTriggerEnter(Collider collider){

		if(collider.tag == "Robot"){
			if(mStoredRobots <= mMaxStorage){
				Destroy(collider.gameObject);
				mStoredRobots++;

			}
		}
	}

	void Start(){

		HideAndRevealTheMount mHideReveal = this.transform.parent.transform.GetComponent<HideAndRevealTheMount>();

		if (mHideReveal != null){
			if(mHideReveal.mIsOnFloor){
				SetOnFloor();
			}
		}
	}

	void Update () {
		
		if (mStoredRobots > 0) {
			RotateCannon ();
		}
		else{
			isRotating = false;
		}
	}

	//Robot should launch out of the cannon in an arch based on the cannon direction and launch velocity
	public void FireRobot(){

		GameObject launchedRobot;
		Vector3 launchVector;

		if(mStoredRobots > 0){
			launchedRobot = Instantiate(mRobot, mLaunchPoint.transform.position, Quaternion.identity) as GameObject;
			launchVector = (mLaunchPoint.transform.position - transform.position).normalized;
		
			launchedRobot.rigidbody.velocity = mLaunchVelocity * launchVector;
			mStoredRobots--;

			if(mCannonAudioContainer != null){
				mCannonAudioContainer.PlayRobotInterractionEffect();
			}

			if(mVFXContainer != null){
				mVFXContainer.PlayPlayerInterraction(0.0f);
			}
		}
	}

	void RotateCannon(){
		if (mRotation.z <= mMaxRotationAngleFloor [0])
			mRotationDirection = -1;
		if (mRotation.z  >= mMaxRotationAngleFloor [1])
			mRotationDirection = 1;



		mRotation.z -= mRotationDirection * mRotationSpeed;
		mFloorArmature.transform.Rotate(0 ,  0, mRotationDirection * mRotationSpeed * -1);

		isRotating = true;
	}

	public void SetOnFloor(){
		mFloorGameObject.SetActive(true);
		mFloorArmature.transform.eulerAngles = mFloorRestRotation;
		mRotation = mRestRotation;

	}

}
