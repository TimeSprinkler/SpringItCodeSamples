using UnityEngine;
using System.Collections;

public class ShowCompanyLogo : MonoBehaviour {


	private float mTimeToShowLogo = 0.75f;

	void Start(){
		StartCoroutine (StartNextScene ());
	}

	IEnumerator StartNextScene(){

		yield return new WaitForSeconds (mTimeToShowLogo);

		this.GetComponent<StartTargetLevel> ().StartLevel ();
	}
}
