using UnityEngine;
using System.Collections;

public class UnlockTheGame : MonoBehaviour {

	public UIMenuAnimations mPanelAnimationsScrpt;

	public void Unlock(){
		if (mPanelAnimationsScrpt != null) {
			mPanelAnimationsScrpt.StartAnimations ();
		} else {

		}
	}
}
