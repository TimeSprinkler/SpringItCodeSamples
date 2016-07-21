using UnityEngine;
using System.Collections;

public class SpeedUpButton : MonoBehaviour {
	
	public GameObject mTimeScaleManager;

	public void OnClick(){

		mTimeScaleManager.GetComponent<TimeScaleManager>().IncreaseSpeed();
	}
}
