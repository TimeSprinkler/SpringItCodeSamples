using UnityEngine;
using System.Collections;

public class StartNextSceneManually : MonoBehaviour {

	public float mTimeToWait = 10;

	private float mTimeSinceCreation;

	void Start () {
		mTimeSinceCreation = 0.0f;


	}
	
	// Update is called once per frame
	void Update () {

		mTimeSinceCreation += Time.deltaTime;

		if(mTimeSinceCreation > mTimeToWait){

			Application.LoadLevel(Application.loadedLevel + 1);
		}

	}
}
