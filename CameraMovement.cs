using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public static float mMouseMoveZone = 12;
	[SerializeField]private float mCameraScrollMultiplier;
	[SerializeField]private float mStiffness;
	[SerializeField]private float mDampening;

	//These 3 static values are used for Building the levels in the editor and should not be used elsewhere
	public static Vector2 mLevelSize = new Vector2(20, 15);
	public static float mHeightStartOffset = 5.45f;

	private CameraBounds mCameraBounds;
	private Placer mPlacer;

	private bool mReadyToDrag;
	private Vector2 mDesiredPosition;
	private Vector2 mVelocity;
	private bool mIsUnLocked = true;

	void Awake() {
		this.mCameraBounds = this.GetComponent<CameraBounds>();
		this.mPlacer = this.GetComponent<Placer>();

		Gesture.onDraggingE += OnDragging;

		this.mVelocity = Vector3.zero;
	}

	void Start(){
		transform.position = Camera.main.transform.position;

		//This needs to be in Start so the UnpackLevel script has time to assign mCameraStartingPosition
		this.mDesiredPosition = transform.position;

	}

	void OnDestroy() {
		Gesture.onDraggingE -= OnDragging;
	}

	void Update () {
		if(mIsUnLocked){
			this.mReadyToDrag = true;
		}
	
		Vector3 myPosition = this.transform.position;
		float zPos = myPosition.z;

		Vector2 twoDPosition = new Vector2(myPosition.x, myPosition.y);
		
		// Calculate spring force
		Vector2 stretch = twoDPosition - this.mDesiredPosition;
		Vector2 force = -this.mStiffness * stretch - this.mDampening * this.mVelocity;

		// Apply acceleration
		this.mVelocity = force * Time.deltaTime / Time.timeScale;

		if(this.mVelocity.sqrMagnitude < 1f)
		{
			this.mVelocity = Vector3.zero;
			this.transform.position = new Vector3(this.mDesiredPosition.x, this.mDesiredPosition.y, zPos);
		}

		twoDPosition += this.mVelocity * Time.deltaTime / Time.timeScale;
		this.transform.position = new Vector3(twoDPosition.x, twoDPosition.y, zPos);
	}

	void OnDrawGizmos() {
		Gizmos.DrawSphere(new Vector3(this.mDesiredPosition.x, this.mDesiredPosition.y, this.transform.position.z), 2);
	}

	void OnDragging(DragInfo dragInfo){
		if(!this.mReadyToDrag || this.mPlacer.ObjectAttached) {
			return;
		}

		Vector3 tempDesiredPosition = this.mCameraBounds.GetBoundedPosition(this.mDesiredPosition - (new Vector2(dragInfo.delta.x, dragInfo.delta.y) *  this.mCameraScrollMultiplier));
		this.mDesiredPosition = new Vector2(tempDesiredPosition.x, tempDesiredPosition.y);
	}


	//Because the game cannot scroll as fast when completely zoomed out as when completely zoomed in


	public void ToggleCameraMovment(bool state){

		mReadyToDrag = state;
		mIsUnLocked = state;
	}

	float DetermineGreatervalue(float value1, float value2){

		if(value1 > value2){
			return value1;
		}else{
			return value2;
		}
	}

	void OnGUI(){


	}
	
}
