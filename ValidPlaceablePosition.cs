using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ValidPlaceablePosition : MonoBehaviour {
	
	[HideInInspector]
	public bool mIsValidPosition;
	public int mPlaceableHazardLayerInt = 12;
	
	private Collider mMainBodyCollidingObject;
	private string[] mIllegalObjectsTagList = {"Placeable", "Foreground", "Furnace", "Window", "Spawner or Collector", "WallInside"}; 
	
	void OnTriggerExit(Collider incomingCollider){

		if (incomingCollider is BoxCollider) {
			if (mMainBodyCollidingObject == incomingCollider) {
				mMainBodyCollidingObject = null;
			}

			IsValidPosition ();
		}
	}
	
	void OnTriggerEnter(Collider incomingCollider){

		if (incomingCollider is BoxCollider) {
				if (mMainBodyCollidingObject != null) {
					if (incomingCollider.gameObject.layer == mPlaceableHazardLayerInt) {
						mMainBodyCollidingObject = incomingCollider;
					}
				} else {
					mMainBodyCollidingObject = incomingCollider;
				}

				IsValidPosition ();
		}
	}
	
	public bool IsValidPosition(){
		
		mIsValidPosition = true;
		IsThisPlaceableASpring();
		IsThisPlaceableAFan();
		IsThisPlaceableACannon();
		IsThisAnIllegalBackgroundSpace();
		
		return mIsValidPosition;
	}
	
	void IsThisAnIllegalBackgroundSpace(){
		if(mMainBodyCollidingObject != null){
			if(HasAnIllegalTag(mMainBodyCollidingObject.tag)){
				mIsValidPosition = false;
			}
		}
	}
	
	bool HasAnIllegalTag(string incomingTag){
		for(int i = 0; i < mIllegalObjectsTagList.Length;i++){
			if(incomingTag == mIllegalObjectsTagList[i]){
				return true;
			}
		}
		
		return false;
	}

	void IsThisPlaceableASpring(){
		if(this.gameObject.name.Contains ("Spring")){
			mIsValidPosition = this.gameObject.GetComponent<Placeable>().m_IsOnFloor;
		}
	}

	void IsThisPlaceableAFan(){
		if(this.gameObject.name.Contains ("Fan")){
			if(this.gameObject.GetComponent<Placeable>().m_IsOnFloor && !this.gameObject.GetComponent<Placeable>().m_IsOnConveyor){
				mIsValidPosition = true;
			}else{
				mIsValidPosition = false;
			}

			if(this.gameObject.GetComponent<Placeable>().m_IsOnWall){
				mIsValidPosition = true;
			}
		}
	}

	void IsThisPlaceableACannon(){
		if(this.gameObject.name.Contains ("Cannon")){
			mIsValidPosition = this.gameObject.GetComponent<Placeable>().m_IsOnFloor;
		}
	}

}
