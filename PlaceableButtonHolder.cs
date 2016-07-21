using UnityEngine;
using System.Collections;

public class PlaceableButtonHolder : MonoBehaviour {

	public void OnDragOut(GameObject draggedObject){
		if(draggedObject.GetComponent<PlaceableButton>() != null){
		draggedObject.GetComponent<PlaceableButton>().OverGUI(false);
		}
	}

	public void OnDragOver(GameObject draggedObject){
		if(draggedObject.GetComponent<PlaceableButton>() != null){
			draggedObject.GetComponent<PlaceableButton>().OverGUI(true);
		}
	}
}
