using UnityEngine;
using System.Collections;

public class GlowingHalo : MonoBehaviour {


	public Light mHalo;
	[HideInInspector]
	public Color mColor;

	void Awake(){

		if(mHalo != null){
			//mHalo.color = mColor;
		}
	}

}
