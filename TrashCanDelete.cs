using UnityEngine;
using System.Collections;

public class TrashCanDelete : MonoBehaviour {

	public void DeleteObject(){	

		Camera.main.GetComponent<Placer>().DeleteSelectedPlaceable();
	}

}
