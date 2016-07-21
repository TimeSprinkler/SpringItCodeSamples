using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelPieceList : MonoBehaviour {

	public GameObject mStoredItemLabel;
	public GameObject[] mStoredItemArray;
	public UITable levelGrid;

	public void CreateLevelPieceList(List<GameObject> incomingList){

		mStoredItemArray = new GameObject[incomingList.Count];

		for(int i = 0; i < incomingList.Count;i++){

			GameObject newStoredItem = GameObject.Instantiate(mStoredItemLabel, mStoredItemLabel.transform.position, Quaternion.identity) as GameObject;
			Vector3 currentLevelPiecePosition = incomingList[i].transform.position;

			newStoredItem.transform.parent = levelGrid.transform;
			newStoredItem.transform.localScale = Vector3.one;
			newStoredItem.GetComponent<UILabel>().text = incomingList[i].name + "   [" + currentLevelPiecePosition.x + "," + currentLevelPiecePosition.y + "," + currentLevelPiecePosition.z + "]";
			mStoredItemArray[i] = newStoredItem;
		}

		this.levelGrid.Reposition();

	}

	public void ClearList(){

		for(int i = 0; i < mStoredItemArray.Length;i++){

			Destroy(mStoredItemArray[i]);
		}
	}
}
