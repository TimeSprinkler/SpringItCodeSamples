#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class BuildTutorial : MonoBehaviour {

	public GameObject mMechanicToTeach;
	public GameObject mGoButton;

	public GameObject mHoloMechanic;
	public GameObject mConveyorToHide;

	public float mGoButtonDelay;
	public float mHoloMechanicDelay;

	void Awake(){
		Currentlevel.instance.TutorialLevel(0);
	}

	void Start(){
		EventHandler.OnRobotSpawn += ToggleOnMechanic;

		StartCoroutine("HighlightGoButton");

	}

	void OnDestroy(){
		EventHandler.OnRobotSpawn -= ToggleOnMechanic;
	}

	IEnumerator HighlightGoButton(){

		yield return new WaitForSeconds(mGoButtonDelay);
		ToggleGoButton(true);
	}

	public void ToggleOnMechanic(){
		StartCoroutine("DelayHoloMechanic");

	}

	public void MechanicPlaced(){

		mHoloMechanic.SetActive(false);
		ToggleGoButton(true);
	}

	public void ToggleGoButton(bool state){

		mGoButton.GetComponent<BoxCollider>().enabled = state;
		mGoButton.GetComponentInChildren<UISprite>().color = Color.white;
	}

	IEnumerator DelayHoloMechanic(){
		yield return new WaitForSeconds(mHoloMechanicDelay);

		mHoloMechanic.SetActive(true);
		mConveyorToHide.SetActive(false);
		mMechanicToTeach.GetComponent<UISprite>().color = Color.white;
		mMechanicToTeach.GetComponent<BoxCollider>().enabled = true;
		mMechanicToTeach.transform.parent.GetComponent<BoxCollider>().enabled = true;
		
		ToggleGoButton(false);

	}
}
#endif
