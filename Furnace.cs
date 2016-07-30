using UnityEngine;
using System.Collections;

public class Furnace : MonoBehaviour {

	public AudioContainer mFurnaceAudioContainer;
	public GameObject mFire;

	public float mFireTime;

	void OnTriggerEnter(Collider collider){
		
		if(collider.gameObject.tag == "Robot"){

			if(mFurnaceAudioContainer != null){
				mFurnaceAudioContainer.PlayRobotInterractionEffect();
			}

			if(mFire != null){
				StartCoroutine("SpewFire");
			}

			collider.gameObject.GetComponent<RobotVisualEffect>().PlayExplosion();
		}
	}

	IEnumerator SpewFire(){

		mFire.SetActive(true);
		yield return new WaitForSeconds(mFireTime);

		mFire.SetActive(false);
	}
}
