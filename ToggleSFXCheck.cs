using UnityEngine;
using System.Collections;

public class ToggleSFXCheck : MonoBehaviour {

	public UIToggle mSFXToggle;

	private bool mSFXState = true;
	
	void Awake(){
		
		mSFXState = PlayerData.instance.mSFXVolumeOn;

		Toggle (mSFXState);

		EventHandler.OnSFXToggle += Toggle;
		UIEventListener.Get(this.gameObject).onClick += this.Clicked;
	}

	void OnDestroy(){
		EventHandler.OnSFXToggle -= Toggle;
		UIEventListener.Get(this.gameObject).onClick -= this.Clicked;
	}

	void Toggle(bool state){
		mSFXToggle.value = state;
		mSFXState = state;
	}

	void Clicked(GameObject gameObject){
		PlayerData.instance.UpdateSFXVolume (!mSFXState);
	}
}
