using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {
	public Transform rotationTransform;
	public Vector3 rotationVector;

	// Use this for initialization
	void Start () {
		rotationVector = rotationTransform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		rotationVector.x += .5f;
		rotationTransform.eulerAngles = rotationVector;
	}
}
