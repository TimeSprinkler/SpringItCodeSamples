using UnityEngine;
using System.Collections;

public class SlowDownButton : MonoBehaviour {

	public GameObject mTimeScaleManager;
	
	public void OnClick(){

		mTimeScaleManager.GetComponent<TimeScaleManager>().DecreaseSpeed();
	}
}
