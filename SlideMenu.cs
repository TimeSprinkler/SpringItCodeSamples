using UnityEngine;
using System.Collections;

public class SlideMenu : MonoBehaviour {

	public GameObject mMenuPanel;
	public float mTimeforMenuToMove;
	public float mMenuMovementDelayTime;
	public UIMenuAnimations mUIMenuAnimationsScript;
	public UIPanel mPanelToFade;

	public Vector3 mDisplayStartPosition;
	public Vector3 mDisplayEndPosition = Vector3.zero;

	void Start(){

		//ScaleValuesToScreenSize();
	}

	void ScaleValuesToScreenSize(){

		if(Screen.width != CustomUIScaling.defaultScreenWidth || Screen.height != CustomUIScaling.defaultScreenHeight){

			float widthRatio = Screen.width/CustomUIScaling.defaultScreenWidth;
			float heightRatio = Screen.height/CustomUIScaling.defaultScreenHeight;
			float ratio;

			if(widthRatio < heightRatio){
				ratio = widthRatio;
			}else{
				ratio = heightRatio;
			}

			mMenuPanel.transform.localPosition = mDisplayStartPosition * ratio;
			mMenuPanel.transform.localScale = mMenuPanel.transform.localScale * ratio;
			mDisplayEndPosition = mDisplayEndPosition * ratio;

		}
	}

	public void StartArmsMoving(){

		StartCoroutine("MoveArmsIntoScreen");

	}

	IEnumerator MoveArmsIntoScreen(){

		if (mUIMenuAnimationsScript != null) {
			mUIMenuAnimationsScript.StartAnimations();
		}

		yield return new WaitForSeconds(mMenuMovementDelayTime);

		float lerpStep = 1f/mTimeforMenuToMove;
		float lerpValue = 0f;
		
		Vector3 tempPosition = transform.localPosition;
			
		while(lerpValue < 1){
			
			lerpValue += lerpStep * Time.deltaTime;
			
			if(mDisplayStartPosition.y != mDisplayEndPosition.y){
				float yValue = Lineartransformations.SmoothStop3(lerpValue);
				tempPosition.y = Mathf.Lerp (mDisplayStartPosition.y, mDisplayEndPosition.y, yValue);
			}else{
				if(mDisplayStartPosition.x != mDisplayEndPosition.x){
					float xValue = Lineartransformations.SmoothStop3(lerpValue);
					tempPosition.x = Mathf.Lerp (mDisplayStartPosition.y, mDisplayEndPosition.y, xValue);
				}
			}
					
			mMenuPanel.transform.localPosition = tempPosition;

			yield return new WaitForEndOfFrame();

		}

		gameObject.GetComponent<PauseGame>().Pause();



		if(this.GetComponent<DisplayLevelResults>() != null){
			this.GetComponent<DisplayLevelResults>().Animate();
		}



	}
	

}
