#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TutorialManager : MonoBehaviour {
	
	private List<TutorialPage> mCurrentLevelTutorialPages = new List<TutorialPage>();

	//I want the manager to create all the panels, just have the scroll panel 
	public void Build(){

		//mCurrentLevelTutorialPages.Clear ();

		//mCurrentLevelTutorialPages = FindandOrderTutorialPages ();

		//AddInPrefabs ();

		//Determine if this is visible for this level

		//Update Grid and scroll panel size if relevant
		//UpdateGridandScrollpanel();

	}

	private List<TutorialPage> FindandOrderTutorialPages(){

		List<TutorialPage> currentList = new List<TutorialPage> ();
		GameObject[] tempGamObjects = Resources.LoadAll ("Prefabs/Tutorial") as GameObject[];

		for (int i = 0; i < tempGamObjects.Length; i++) {
			if(tempGamObjects[i].GetComponent<TutorialPageInfo>() != null){
				//currentList.Add (tempGamObjects[i].GetComponent<TutorialPageInfo>());
			}
		}

		//OrderCurrentList
		TutorialPage tempStorageLocation;
		bool isInOrder = false;

		while (!isInOrder) {
			isInOrder = true;

			for(int i = 0; i < currentList.Count-1; i++){
				if(currentList[i].mOrderID < currentList[i+1].mOrderID ){

				}else if(currentList[i].mOrderID  > currentList[i+1].mOrderID ){
					isInOrder = false;

					//ShiftPositionOfTwoPages
					tempStorageLocation = currentList[i];
					currentList[i] = currentList[i+1];
					currentList[i+1] = tempStorageLocation;

				}else{//they are equal this is bad
					isInOrder = false;

					currentList[i+1].mOrderID++;
					Debug.LogWarning(currentList[i].mName + "'s Order is the same as " + currentList[i+1].mName + ".  " + currentList[i+1].mName + "'s Order has been changed to " + (currentList[i+1].mOrderID +1));

				}
			}
		}

		return currentList;
	}

	private void AddInPrefabs(){
		for (int i = 0; i < mCurrentLevelTutorialPages.Count; i++) {

			//Find the appropriate prefas and load time in
			GameObject tutPagePrefab = Resources.Load (TutorialEditorWindow.mLoadDirectory + mCurrentLevelTutorialPages[i].mName) as GameObject;
			tutPagePrefab = Instantiate(tutPagePrefab,Vector3.zero, Quaternion.identity) as GameObject;
			gameObject.GetComponentInChildren<TutorialScrollPanel>().AddTutorialPageToScrollPanel(tutPagePrefab);
		}
	}

	void UpdateGridandScrollpanel(){

		Vector2 mPanelSize = this.GetComponent<UIPanel> ().GetViewSize ();

		this.GetComponentInChildren<UIGrid>().cellHeight = mPanelSize.y;
		this.GetComponentInChildren<UIGrid>().cellWidth = mPanelSize.y;
		
	}
	
}
#endif
