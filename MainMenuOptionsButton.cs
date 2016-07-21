using UnityEngine;
using System.Collections;

public class MainMenuOptionsButton : MonoBehaviour {

	public string mLerpHorizontalAnchor;
	public string mLerpVerticalAnchor;
	public float mHorizontalExpandTime;
	public float mVerticalExpandTime;
	public Vector2 mFinalExpansionDistance;
	public Vector2 mStartingAnchorDistance;
	public bool mIsLerping = false;
	public Animation mAttachedAnimation;
	public string mHorizontalAnimationName;
	public string mVerticalAnimationName;
	public bool mIsOpen = false;
	
	public GameObject mMenuObject;
	public GameObject[] mMenuButtons;

	[SerializeField]
	private GameObject mUISpriteObject;

	private float mCurrentLerpTime;
	private bool mIsOpening;

	private float mLerpHorizontalTarget;
	private float mLerpHorizontalStart;
	private float mLerpVerticalTarget;
	private float mLerpVerticalStart;
	
	void Update(){

		if(mIsLerping){
			LerpMenu();
		}
	}

	void OnClick(){

		OpenMenu();
	}

	void ChangeComponentVisibility(bool state){
		this.collider.enabled = state;
		mUISpriteObject.GetComponent<UISprite>().enabled = state;

	}

	public void CloseMenu(){

		ToggleButtons();
		mLerpHorizontalStart = mFinalExpansionDistance.x;
		mLerpHorizontalTarget = mStartingAnchorDistance.x;
		mLerpVerticalStart = mFinalExpansionDistance.y;
		mLerpVerticalTarget = mStartingAnchorDistance.y;
		mCurrentLerpTime = 0.0f;
		mIsOpening = false;
		mIsLerping = true;
		if(Camera.main.GetComponent<TimeScaleManager>() != null){
			Camera.main.GetComponent<TimeScaleManager>().PauseOrUnpauseGame(mIsOpening);
		}

		mIsOpen = false;
	}

	public void OpenMenu(){

		ChangeComponentVisibility(false);
		mMenuObject.SetActive(true);
		mLerpHorizontalStart = mStartingAnchorDistance.x;
		mLerpHorizontalTarget = mFinalExpansionDistance.x;
		mLerpVerticalStart = mStartingAnchorDistance.y;
		mLerpVerticalTarget = mFinalExpansionDistance.y;
		mCurrentLerpTime = 0.0f;
		mIsOpening = true;
		mIsLerping = true;

		mIsOpen = true;
	}

	private void LerpMenu(){

		if(mIsOpening){

			LerpHorizontal();
		}else{

			LerpVertical();
		}
	}

	void LerpHorizontal(){

		switch (mLerpHorizontalAnchor){

		default: break;

		case "right": LerpHorizontalRight();
			break;

		case "left": LerpHorizontalLeft();
			break;

		}
	}

	void LerpVertical(){
		
		switch (mLerpVerticalAnchor){
			
		default: break;
			
		case "top": LerpVerticalTop();
			break;
			
		case "bottom": LerpVerticalBottom();
			break;	
		}
	}

	bool CheckIfHorizontalStarted(){

		bool hasStarted = false;

		switch (mLerpHorizontalAnchor){
			
		default: break;

		case "right": 
			hasStarted = (mMenuObject.GetComponent<UISprite>().rightAnchor.absolute == (int)mLerpHorizontalStart);
			break;
			
		case "left": 
			hasStarted = (mMenuObject.GetComponent<UISprite>().leftAnchor.absolute == (int)mLerpHorizontalStart);
			break;
			
		}

		return hasStarted;
	}

	bool CheckIfVerticalStarted(){
		
		bool hasStarted = false;
		
		switch (mLerpVerticalAnchor){
			
		default: break;
			
		case "top": 
			hasStarted = (mMenuObject.GetComponent<UISprite>().topAnchor.absolute == (int)mLerpVerticalStart);
			break;
			
		case "bottom": 
			hasStarted = (mMenuObject.GetComponent<UISprite>().bottomAnchor.absolute == (int)mLerpVerticalStart);
			break;
		}
		
		return hasStarted;
	}

	void LerpTransition(){

		bool rightAtTarget = mMenuObject.GetComponent<UISprite>().rightAnchor.absolute >= mLerpHorizontalTarget;
		bool leftAtTarget = mMenuObject.GetComponent<UISprite>().leftAnchor.absolute <= mLerpHorizontalTarget;
		bool topAtTarget = mMenuObject.GetComponent<UISprite>().topAnchor.absolute >= mLerpVerticalTarget;
		bool botAtTarget = mMenuObject.GetComponent<UISprite>().bottomAnchor.absolute <= mLerpVerticalTarget;

		if(mIsOpening){
			if(mLerpHorizontalAnchor == "right"){
				if(rightAtTarget){
					if(mLerpVerticalAnchor == "top"){
						if(topAtTarget){
							EndLerp();
						}else{
							LerpVertical();
						}
					}else if(mLerpVerticalAnchor == "bottom"){
						if(botAtTarget){
							EndLerp();
						}else{
							LerpVertical();
						}
					}
				}else{
					return;
				}
			}else if(mLerpHorizontalAnchor == "left"){
				if(leftAtTarget){
					if(mLerpVerticalAnchor == "top"){
						if(topAtTarget){
							EndLerp();
						}else{
							LerpVertical();
						}
					}else if(mLerpVerticalAnchor == "bottom"){
						if(botAtTarget){
							EndLerp();
						}else{
							LerpVertical();
						}
					}
				}else{
					return;
				}
			}
		}else{

			rightAtTarget = mMenuObject.GetComponent<UISprite>().rightAnchor.absolute <= mLerpHorizontalTarget;
			leftAtTarget = mMenuObject.GetComponent<UISprite>().leftAnchor.absolute >= mLerpHorizontalTarget;
			topAtTarget = mMenuObject.GetComponent<UISprite>().topAnchor.absolute <= mLerpVerticalTarget;
			botAtTarget = mMenuObject.GetComponent<UISprite>().bottomAnchor.absolute >= mLerpVerticalTarget;

			if(mLerpVerticalAnchor == "top"){
				if(topAtTarget){
					if(mLerpHorizontalAnchor == "right"){
						if(rightAtTarget){
							EndLerp();
						}else{
							LerpHorizontal();
						}
					}else if(mLerpHorizontalAnchor == "left"){
						if(leftAtTarget){
							EndLerp();
						}else{
							LerpHorizontal();
						}
					}
				}else{
					return;
				}
			}else if(mLerpVerticalAnchor == "bottom"){
				if(botAtTarget){
					if(mLerpHorizontalAnchor == "right"){
						if(rightAtTarget){
							EndLerp();
						}else{
							LerpHorizontal();
						}
					}else if(mLerpHorizontalAnchor == "left"){
						if(leftAtTarget){
							EndLerp();
						}else{
							LerpHorizontal();
						}
					}
				}else{
					return;
				}
			}
		}
	}

	void LerpHorizontalLeft(){
		float lerpFraction;
		mCurrentLerpTime += Time.deltaTime;

		if(CheckIfHorizontalStarted()){
			mCurrentLerpTime = 0.01f;
			mCurrentLerpTime += Time.deltaTime;
		}

		lerpFraction = mCurrentLerpTime / mHorizontalExpandTime;
		if(lerpFraction >= 1.01f) lerpFraction = 1.01f;
		mMenuObject.GetComponent<UISprite>().leftAnchor.absolute = (int)((mLerpHorizontalTarget - mLerpHorizontalStart) * lerpFraction + mLerpHorizontalStart);

		PlayAnimation(mHorizontalAnimationName);

		LerpTransition();
	}

	void LerpHorizontalRight(){
		float lerpFraction;
		mCurrentLerpTime += Time.deltaTime;
		
		if(CheckIfHorizontalStarted()){
			mCurrentLerpTime = 0.01f;
			mCurrentLerpTime += Time.deltaTime;
		}
		
		lerpFraction = mCurrentLerpTime / mHorizontalExpandTime;
		if(lerpFraction >= 1.01f) lerpFraction = 1.01f;
		mMenuObject.GetComponent<UISprite>().rightAnchor.absolute = (int)((mLerpHorizontalTarget - mLerpHorizontalStart) * lerpFraction + mLerpHorizontalStart);

		PlayAnimation(mHorizontalAnimationName);

		LerpTransition();
	}


	void LerpVerticalTop(){
		float lerpFraction;
		mCurrentLerpTime += Time.deltaTime;
		
		if(CheckIfVerticalStarted()){
			mCurrentLerpTime = 0.01f;
			mCurrentLerpTime += Time.deltaTime;
		}

		lerpFraction = mCurrentLerpTime/mVerticalExpandTime;
		if(lerpFraction >= 1.01f) lerpFraction = 1.01f;
		mMenuObject.GetComponent<UISprite>().topAnchor.absolute = (int)((mLerpVerticalTarget - mLerpVerticalStart) * lerpFraction + mLerpVerticalStart);
	
		PlayAnimation(mVerticalAnimationName);

		LerpTransition();

	}

	void LerpVerticalBottom(){
		float lerpFraction;
		mCurrentLerpTime += Time.deltaTime;
		
		if(CheckIfVerticalStarted()){
			mCurrentLerpTime = 0.01f;
			mCurrentLerpTime += Time.deltaTime;
		}

		lerpFraction = mCurrentLerpTime/mVerticalExpandTime;

		if(lerpFraction >= 1.01f) lerpFraction = 1.01f;
		mMenuObject.GetComponent<UISprite>().bottomAnchor.absolute = (int)((mLerpVerticalTarget - mLerpVerticalStart) * lerpFraction + mLerpVerticalStart);

		PlayAnimation(mVerticalAnimationName);

		LerpTransition();

	}

	void EndLerp(){
		if(mIsOpening){	
			mIsLerping = false;
			ToggleButtons();
			if(Camera.main.GetComponent<TimeScaleManager>() != null){
				Camera.main.GetComponent<TimeScaleManager>().PauseOrUnpauseGame(mIsOpening);
			}

		}else{
			mIsLerping = false;
			ChangeComponentVisibility(true);
			mMenuObject.SetActive(false);
		}
	}

	void ToggleButtons(){

		foreach(GameObject button in mMenuButtons){
			button.SetActive(!button.activeSelf);
		}
	}

	void PlayAnimation(string animationName){

		if(mAttachedAnimation == null){
			return;
		}

		AnimationState animationState = mAttachedAnimation.animation[animationName];

		if(mIsOpening){
			animationState.speed = 1.0f;
			mAttachedAnimation.Play(animationName);
		}else{
			animationState.speed = -1.0f;
			mAttachedAnimation.Play(animationName);
		}
	}
	
}
