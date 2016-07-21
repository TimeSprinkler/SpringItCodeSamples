using UnityEngine;
using System.Collections;

public class SpawnerModelManager : MonoBehaviour {
	
	public string mIncorrectModelName;
	
	public void Reset(){
		
		Destroy(transform.FindChild(mIncorrectModelName).gameObject);
	}

}