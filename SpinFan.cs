using UnityEngine;
using System.Collections;

public class SpinFan : MonoBehaviour {

	public int mRotationSpeed = 5;

	void Update(){

		gameObject.transform.Rotate(new Vector3(mRotationSpeed * Time.deltaTime, 0, 0));
	}
}
