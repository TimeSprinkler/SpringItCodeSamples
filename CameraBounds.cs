using UnityEngine;
using System.Collections;

public class CameraBounds: MonoBehaviour {

	//Camera view port has a size so all edges do not start from the same point.
	public int mScreenViewBuffer = 3;
	public Vector2 mScreenManualOffset = new Vector2(0, 2);
	public Transform mTopRightTransform;

	private Vector3 mBottomLeft;
	private Vector3 mTopRight;

	private Camera mCamera;
	private float mCameraViewAngle;

	void Start(){
		mCamera = Camera.main;
		mCameraViewAngle = mCamera.fieldOfView/2;

		SetCameraBounds();

		mBottomLeft.x -= mScreenViewBuffer;
		mBottomLeft.y -= mScreenViewBuffer;
		mTopRight.x += mScreenViewBuffer;
		mTopRight.y += mScreenViewBuffer;

	}

	void SetCameraBounds(){
		mBottomLeft = new Vector3( 0, 0, 0);

		//Remove this if statement for full game
		if(mTopRightTransform != null){
			mTopRight = mTopRightTransform.position;
		}else{
			mTopRight = new Vector3(40, 16, 0);
		}
		
	}

	public Vector3 GetBoundedPosition(Vector3 proposedPosition, bool zMovedCondition = false){

		//when you pass a Vector2 into a Vector3 the z is set to 0
		if(proposedPosition.z == 0  && mCamera != null){
			proposedPosition.z = mCamera.transform.position.z;
		}

		Vector3 cameraPositonAlongLevelCenterline = new Vector3((mBottomLeft.x + mTopRight.x)/2f + mScreenViewBuffer + mScreenManualOffset.x, (mBottomLeft.y + mTopRight.y)/2f  + mScreenViewBuffer + mScreenManualOffset.y, proposedPosition.z);

		proposedPosition.x = Mathf.Clamp(proposedPosition.x, this.LeftLimit(cameraPositonAlongLevelCenterline), this.RightLimit(cameraPositonAlongLevelCenterline));
		proposedPosition.y = Mathf.Clamp(proposedPosition.y, this.DownLimit(cameraPositonAlongLevelCenterline), this.UpLimit(cameraPositonAlongLevelCenterline));
		proposedPosition.z = Camera.main.transform.position.z;

		return proposedPosition;
	}

	//For the Limit functions Z should be negative
	float RightLimit(Vector3 centerLinePosition){

		float xRightLimit = mTopRight.x + centerLinePosition.z * Mathf.Tan(mCameraViewAngle * Mathf.PI/180f) + mScreenManualOffset.x;
	
		return xRightLimit;
	}

	float LeftLimit(Vector3 centerLinePosition){


		float xLeftLimit = mBottomLeft.x - centerLinePosition.z * Mathf.Tan(mCameraViewAngle * Mathf.PI/180f) + mScreenManualOffset.x;
		
		return xLeftLimit;
	}

	float DownLimit(Vector3 centerLinePosition){

		float yDownLimit = mBottomLeft.y - centerLinePosition.z * Mathf.Tan(mCameraViewAngle * Mathf.PI/180f) + mScreenManualOffset.y;

		return yDownLimit;
	}

	float UpLimit(Vector3 centerLinePosition){
				
		float yUpLimit = mTopRight.y + centerLinePosition.z * Mathf.Tan(mCameraViewAngle * Mathf.PI/180f) + mScreenManualOffset.y;

		return yUpLimit;
	}
	
}
