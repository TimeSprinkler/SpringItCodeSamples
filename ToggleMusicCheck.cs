using UnityEngine;
using System.Collections;

public class ToggleMusicCheck : MonoBehaviour {

	public UIToggle mMusicToggle;

	private bool mMusicState = true;
	
	void Awake(){

		mMusicState = PlayerData.instance.mMusicVolumeOn;

		Toggle (mMusicState);

		EventHandler.OnMusicToggle += Toggle;
		UIEventListener.Get(this.gameObject).onClick += this.Clicked;
	}
	
	void OnDestroy(){
		EventHandler.OnMusicToggle -= Toggle;
		UIEventListener.Get(this.gameObject).onClick -= this.Clicked;
	}
	
	void Toggle(bool state){
		mMusicToggle.value = state;
		mMusicState = state;
	}

	void Clicked(GameObject gameObject){
		PlayerData.instance.UpdateMusicVolume (!mMusicState);
	}

}
