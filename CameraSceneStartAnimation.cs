using UnityEngine;
using System.Collections;

public class CameraSceneStartAnimation : MonoBehaviour {

	public bool mIsEnabled = true;

	[Range(-20, -30)] public float mStartZoomDistance = -20f;//From 0 base is -15
	public float mPauseAtStart;
	public float mAnimationTime;
	public UnlockTheGame mUnlockTheGameScript;

	//private float mHorizontalSideBuffer = 5.0f;
	private Vector3 mSpawnerLocation;

	//I know the spawner location from another script
	[ExecuteInEditMode]
	public void SetCameraStartPosition(Vector2 levelSize, Transform collectorLocation){

		//Calculate position of camera to have collector on the right side of viewport

		Vector3 newStartPosition = new Vector3 (collectorLocation.position.x, collectorLocation.position.y, mStartZoomDistance);

		float tanOppositeLength = (Mathf.Atan (Mathf.Deg2Rad * Camera.main.fieldOfView) * Mathf.Abs(mStartZoomDistance));
		float currentRightViewDistance = newStartPosition.x + tanOppositeLength;
		float currentBottomViewDistance =  newStartPosition.y - tanOppositeLength;

		newStartPosition.x -= (currentRightViewDistance - (levelSize.x * 2));
		newStartPosition.y += (Mathf.Abs (0.0f - currentBottomViewDistance) - CalculateYOffset());
	
		Camera.main.transform.position = newStartPosition;

	}

	void Awake(){

		GameObject[] mSpawners = GameObject.FindGameObjectsWithTag ("Spawner or Collector");

		for (int i = 0; i < mSpawners.Length; i++) {
			if(mSpawners[i].GetComponent<RobotSpawner>() != null){
				mSpawnerLocation = mSpawners[i].transform.position;
			}
		}

		float tanOppositeLength = (Mathf.Atan (Mathf.Deg2Rad * Camera.main.fieldOfView) * Mathf.Abs (UnpackLevel.mCameraStartZoom));
		float currentBottomViewDistance =  mSpawnerLocation.y - tanOppositeLength;
		float currentLeftViewDistance = mSpawnerLocation.x - tanOppositeLength;
		
		mSpawnerLocation.x += (0.0f - currentLeftViewDistance);
		mSpawnerLocation.y += (Mathf.Abs (0.0f - currentBottomViewDistance) - CalculateYOffset());
	}


	public void AnimateCamera(){

		if (mIsEnabled) {
			StartCoroutine ("Animate");
		} else {
			Debug.LogWarning("Animate Camera is Disabled, skipping the animation");
			//Camera.main.transform.position = mSpawnerLocation;
			mUnlockTheGameScript.Unlock ();
			this.GetComponent<CameraBounds> ().enabled = true;
			this.GetComponent<CameraMovement> ().enabled = true;
		}
	}

	IEnumerator Animate(){

		yield return new WaitForSeconds (mPauseAtStart);

		//float lerpStep = 1 / mAnimationTime;
		float lerpValue = 0.0f;

		mSpawnerLocation.y  += CalculateYOffset();

		Vector3 tempPosition = Camera.main.transform.position;
		Vector3 startPosition = Camera.main.transform.position;

		mUnlockTheGameScript.Unlock ();

		while (lerpValue < 1.0f) {

			lerpValue += Time.deltaTime/mAnimationTime;
		
			tempPosition.x = (Mathf.Lerp(startPosition.x, mSpawnerLocation.x, Lineartransformations.SmoothStop4(lerpValue)));
			tempPosition.y = Mathf.Lerp(startPosition.y, mSpawnerLocation.y, Lineartransformations.SmoothStop4(lerpValue));
			tempPosition.z = Mathf.Lerp(startPosition.z, UnpackLevel.mCameraStartZoom, Lineartransformations.SmoothStop4(lerpValue));

			Camera.main.transform.position = tempPosition;

			yield return new WaitForEndOfFrame();
		}	

		this.GetComponent<CameraBounds> ().enabled = true;
		this.GetComponent<CameraMovement> ().enabled = true;
		this.GetComponent<PlayerObjectButtons> ().ToggleButtons (true);


	}

	float CalculateYOffset(){
		return (Mathf.Abs (mStartZoomDistance) * (Mathf.Tan (Camera.main.transform.rotation.x)));
	}
}
