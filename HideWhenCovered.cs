using UnityEngine;
using System.Collections;

public class HideWhenCovered : MonoBehaviour {

	public GameObject[] mAllChildren;

	private GameObject mCurrentObject;


	void Awake(){

		EventHandler.OnPlaceFinish += CheckIfPlaceableIsOnThisPosition;
		EventHandler.OnPlaceableDeleted += PlaceableDeleted;

	}

	void OnDestroy(){

		EventHandler.OnPlaceFinish -= CheckIfPlaceableIsOnThisPosition;
		EventHandler.OnPlaceableDeleted -= PlaceableDeleted;

	}


	public void Hide(GameObject incoming){

		if(mCurrentObject == null){
			mCurrentObject = incoming;
		}

		if (incoming.transform.position == this.transform.position) {
			for (int i = 0; i < mAllChildren.Length; i++) {
				mAllChildren [i].SetActive (false);
			}
		} else {
			UnHide(incoming);
		}

	}

	public void UnHide(GameObject incoming){

		if(mCurrentObject != null){
			if(incoming.GetInstanceID() == mCurrentObject.GetInstanceID()){

				for(int i = 0; i < mAllChildren.Length; i++){
					mAllChildren[i].SetActive(true);
				}

				mCurrentObject = null;
			}
		}	
	}

	private void CheckIfPlaceableIsOnThisPosition(Transform transform){

		if (transform.position == this.transform.position) {
			Hide (transform.gameObject);
		} else {
			UnHide(transform.gameObject);
		}
	}

	private void PlaceableDeleted(Transform transform){

		if (transform.position == this.transform.position) {
			UnHide (transform.gameObject);
		}

	}
}
