#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class BuildFarground : MonoBehaviour {

	public Vector3 spawnPosition = new Vector3(15, 8, 20);

	public void Build(){

		GameObject fargroundStorage;

		if (UnpackLevel.mID < 20) {
			fargroundStorage = Resources.Load ("Prefabs/Background/Farground Day")  as GameObject;
		} else {
			fargroundStorage = Resources.Load ("Prefabs/Background/Farground Night")  as GameObject;
		}

		Instantiate(fargroundStorage, spawnPosition, Quaternion.identity);

	}
}
#endif
