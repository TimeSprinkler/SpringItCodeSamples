using UnityEngine;
using System.Collections;

public class PlaceableButton : MonoBehaviour {

	private PlayerObjectButtons mPOBScript;

	void Start(){
		mPOBScript = Camera.main.GetComponent<PlayerObjectButtons>();
	}

	public void OnDragEnd(){
		StartCoroutine("DelayButtonReleaseCall");

	}
	public void OnDragStart(){
		mPOBScript.OnIconDrag(this.gameObject);
	}

	public void OverGUI(bool isOverGUI){
		if(isOverGUI){
			mPOBScript.OnUI(this.gameObject);
		}else{
			mPOBScript.OffUI(this.gameObject);
		}
	}

	IEnumerator DelayButtonReleaseCall(){

		yield return new WaitForSeconds (0.01f);
		mPOBScript.ButtonReleased(this.gameObject);
	}

	public void ChangeActiveState(bool state){

		this.GetComponent<BoxCollider>().enabled = state;

		if (state) {
			this.GetComponent<UIButtonColor> ().defaultColor = Color.white;
			this.GetComponent<UIButtonColor> ().hover = Color.white;
			this.GetComponent<UIButtonColor> ().pressed = Color.white;
			this.GetComponent<BoxCollider>().enabled = true;
		} else {
			this.GetComponent<UIButtonColor> ().defaultColor = Color.gray;
			this.GetComponent<UIButtonColor> ().hover = Color.gray;
			this.GetComponent<UIButtonColor> ().pressed = Color.gray;
			this.GetComponent<BoxCollider>().enabled = false;
		}
	
	}
}
