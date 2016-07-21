using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageStorage : MonoBehaviour {

	public List<string> mMessages;

	void Awake(){

		GameObject currentStorage = GameObject.FindGameObjectWithTag("InfoBarStorage");

		if (currentStorage != this.gameObject) {
			Destroy(this.gameObject);
			return;
		} else {
			currentStorage = this.gameObject;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	public void NewLevelLoaded(){

		//GameObject.FindGameObjectWithTag("InfoBar").GetComponent<MessageBar>().NewMessage(mMessages[mCurrentLevel]);
	}
}
