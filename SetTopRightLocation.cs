using UnityEngine;
using System.Collections;

public class SetTopRightLocation : MonoBehaviour {

	void Awake(){
		Camera.main.GetComponent<CameraBounds>().mTopRightTransform = this.transform;
	}
}
