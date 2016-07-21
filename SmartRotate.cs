using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartRotate : MonoBehaviour {

	private string[] mValidTags = {"Floor", "Wall"};
	private string[] mInvalidTags = {"LevelPiece", "Spawner or Collector"};

	private Vector3 mTargetRotation;
	private List<Collider> mColliderList = new List<Collider>();
	
	private int mRotateCounter = 0;

	void OnTriggerEnter(Collider collider){

		if (collider is BoxCollider) {
			mColliderList.Add (collider);

			SmartRotation ();
		}

	}

	void OnTriggerExit(Collider collider){

		if (collider is BoxCollider) {
			mColliderList.Remove (collider);

			SmartRotation ();
		}
	}

	void SmartRotation(){

		if(mColliderList.Count > 0){
			if(ColliderListHasNoInvalidTags()){

				Collider firstCollider = FindFirstValidCollider();

				if(firstCollider != null){

					if(firstCollider.tag == "Floor"){

						while(mRotateCounter != 1){
							Rotate90Degrees();

							mRotateCounter = mRotateCounter%4;
						}
					}else if(firstCollider.tag == "Wall"){

						int switchInt = (int)firstCollider.transform.right.x; 

						float bottomLimit = firstCollider.bounds.center.y - firstCollider.bounds.extents.y;
						float rightLimit = firstCollider.bounds.center.x + firstCollider.bounds.extents.x;

						if(firstCollider.transform.position.x < this.collider.bounds.center.x){

							if(rightLimit < this.transform.position.x)switchInt = -1;

							//if((firstCollider.bounds.extents.x < this.collider.bounds.extents.x) && (-firstCollider.bounds.extents.x > -this.collider.bounds.extents.x))switchInt = 0;

						}

						if(firstCollider.transform.position.y > this.collider.bounds.center.y){

							if(bottomLimit > this.transform.position.y){
								switchInt = 0;
							}
						}

						switch(switchInt){
						//On Ceiling face down
						case 0:
							while(mRotateCounter != 3){
								Rotate90Degrees();
								mRotateCounter = mRotateCounter%4;
							}
							break;
						//Facing Right
						case -1:
							while(mRotateCounter != 0){
								Rotate90Degrees();
								mRotateCounter = mRotateCounter%4;
							}
							break;

						//Facing Left
						case 1:

							while(mRotateCounter != 2){
								Rotate90Degrees();
								mRotateCounter = mRotateCounter%4;
							}
							break;
						}
					}
				}
			}
		}

	}

	bool ColliderListHasNoInvalidTags(){

		for(int i = 0; i < mColliderList.Count;i++){

			if(mColliderList[i] != null){
				if(ContainsTag(mColliderList[i].tag, mInvalidTags)){
					return false;
				}
			}
		}

		return true;
	}

	Collider FindFirstValidCollider(){

		Collider colliderToReturn = null;

		if(mColliderList.Count > 0){
			for(int i = 0; i < mColliderList.Count; i++){
				if(ContainsTag(mColliderList[i].tag, mValidTags)){
					colliderToReturn =  mColliderList[i];

					if(mColliderList[i].tag == mValidTags[0]){
						return  mColliderList[i];
					}

				}
			}
		}

		return colliderToReturn;
	}

	bool ContainsTag(string tag, string[] tagList){

		for(int i = 0; i < tagList.Length; i++){
			if(tagList[i] == tag) return true;
		}

		return false;
	}

	void Rotate90Degrees(){
		
		this.transform.Rotate(0,0,90);
		mRotateCounter++;
			
	}
}
