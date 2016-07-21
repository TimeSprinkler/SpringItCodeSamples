using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadPlaceableBar : MonoBehaviour {

	public Vector3 mPlaceableButtonLocalSpawnPosition;

	[SerializeField] private List<GameObject> mAllPlaceableButtons = new List<GameObject>(); //In order of the AvailablePlaceables
	[SerializeField] private List<GameObject> mPlaceableBarSquares = new List<GameObject>(); //should be Available Placeables - 1

	private bool mFoundFirstPlaceable = false;

	public void Load(int[] placeableAmounts){

		MakeAllInActive();

		GameObject tempPlaceableBarSquare;

		for(int i = 0; i < placeableAmounts.Length;i++){
			if(placeableAmounts[i] > 0){
				if(mFoundFirstPlaceable == true){
					tempPlaceableBarSquare = ActivateNextPlaceableSquare();
					mAllPlaceableButtons[i].transform.parent = tempPlaceableBarSquare.transform;
					mAllPlaceableButtons[i].transform.localPosition = mPlaceableButtonLocalSpawnPosition;
				}else{
					mFoundFirstPlaceable = true;
					mAllPlaceableButtons[i].transform.parent = this.transform;
					mAllPlaceableButtons[i].transform.localPosition = Vector3.zero;
				}
			}else{
				mAllPlaceableButtons[i].SetActive(false);
			}
		}
	}

	GameObject ActivateNextPlaceableSquare(){

		int i= 0;

		while(mPlaceableBarSquares[i].activeSelf == true){
			i++;

			if(mPlaceableBarSquares[i] == null){
				i--;
				break;
			}
		}
	
		mPlaceableBarSquares[i].SetActive(true);
		return mPlaceableBarSquares[i];
	}

	void MakeAllInActive(){

		for(int i = 0; i < mPlaceableBarSquares.Count; i++){
			mPlaceableBarSquares[i].SetActive(false);
		}

	}
}
