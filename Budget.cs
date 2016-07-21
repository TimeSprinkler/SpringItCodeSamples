using UnityEngine;
using System.Collections;

public class Budget : MonoBehaviour {
	
	private string[] mPlaceableNames;
	public int[] mPlaceableAmounts;
	public UILabel[] mPlaceableUI;

	private int mRemainingBudget;

	void Awake(){

		int totalBudget = 0;

		for(int i = 0; i < mPlaceableAmounts.Length;i++){
			totalBudget+= mPlaceableAmounts[i];
		}

		Currentlevel.instance.UpdateTotalTools(totalBudget);

		EventHandler.OnPlaceFinish += UpdateAllUI;
	}

	void Start(){
		mPlaceableNames = new string[]{"Spring (Placeable)", "Fan (Placeable)", "Magnet (Placeable)", "Cannon (Placeable)"};

		for(int i = 0; i < mPlaceableUI.Length; i++){
			UpdateUI(i);
		}
	}

	void OnDestroy(){
		EventHandler.OnPlaceFinish -= UpdateAllUI;
	}

	public void PlaceObject(string name){

		int i = FindIntByName(name);

		mPlaceableAmounts[i]--;
		UpdateUI(i);
	}

	public void DeleteObject(string name){

		int i = FindIntByName(name);

		mPlaceableAmounts[i]++;
		UpdateUI(i);
	}
		

	public bool AbleToPlace(string name){
	
		int i = FindIntByName(name);

		if(mPlaceableAmounts[i] < 0){
			return false;
		}

		return true;
	}

	int FindIntByName(string name){

		for(int i = 0; i < mPlaceableNames.Length;i++){
			if(mPlaceableNames[i] == name || mPlaceableNames[i] + "(Clone)" == name || mPlaceableNames[i] + " Button" == name ){
				return i;
			}
		}

		return 10;
	}

	void UpdateAllUI(Transform transform){

		for (int i = 0; i < mPlaceableUI.Length; i++) {
			if(mPlaceableAmounts[i] > 0){
				UpdateUI(i);
			}
		}
	}

	void UpdateUI(int identifier){

		mPlaceableUI[identifier].text = mPlaceableAmounts[identifier].ToString();

		if(this.GetComponent<PlayerObjectButtons>().mEnableEachObject[identifier]){
			if(mPlaceableAmounts[identifier] <= 0){
				mPlaceableUI[identifier].parent.GetComponent<PlaceableButton>().ChangeActiveState(false);
			}else{
				mPlaceableUI[identifier].parent.GetComponent<PlaceableButton>().ChangeActiveState(true);
			}
		}
	}
	
}
