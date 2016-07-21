
using UnityEngine;
using System.Collections;

public class LevelNumber : MonoBehaviour {

	public UILabel mAttachedLabel;

	void Start () {
		EventHandler.OnNewLevel += UpdateLabel;
		UpdateLabel();
	}
	
	void OnDestroy(){
		EventHandler.OnNewLevel -= UpdateLabel;
	}

	void UpdateLabel(int id){
		mAttachedLabel.text = "Level " + (id);
	}

	void UpdateLabel(){
		mAttachedLabel.text = "Level " + (Currentlevel.instance.mID);
	}
}
