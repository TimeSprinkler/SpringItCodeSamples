using UnityEngine;
using System.Collections;

public class InterractableObject : MonoBehaviour {

	public GameObject mActivatedObject;
	
	void OnTriggerStay(Collider collider){
		
		if(collider.gameObject.tag == "Robot"){
			mActivatedObject.animation.Play();
			this.gameObject.SetActive(false);
		}
	}
}
