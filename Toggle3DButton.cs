using UnityEngine;
using System.Collections;

public class Toggle3DButton : MonoBehaviour {

	public Magnet mMagnetObjectToActivate;
	public Cannon mCannonObjectToActivate;
	public Placeable mFanObjectToActivate;

	public void Toggle(){
		bool isMagnet = (mMagnetObjectToActivate != null);
		bool isCannon = (mCannonObjectToActivate != null);
		bool isFan = (mFanObjectToActivate != null);

		if((isMagnet && isCannon)||(isMagnet && isFan)||(isCannon && isFan)){
			Debug.LogError("You cannot have this object be both a Magnet and Cannon or Fan and Magnet or...");
		}

		if(isMagnet){
			mMagnetObjectToActivate.ToggleDirection();
			CallAmbientEffect();
		}

		if(isCannon){
			mCannonObjectToActivate.FireRobot();
			CallAmbientEffect();
		}

		if(isFan){
			mFanObjectToActivate.RotateCounterClockwise();
			CallAmbientEffect();
		}

	}

	void CallAmbientEffect(){

		if(transform.parent.transform.GetComponent<VFXContainter>() != null){

			VFXContainter container = transform.parent.transform.GetComponent<VFXContainter>();

			container.ToggleAmbient();
		}

	}
}
