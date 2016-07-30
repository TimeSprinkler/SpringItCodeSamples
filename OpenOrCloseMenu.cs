using UnityEngine;
using System.Collections;

public class OpenOrCloseMenu : MonoBehaviour {

	public GameObject mThisMenu;

	public void OpenOrClose(){

		mThisMenu.SetActive(!mThisMenu.activeSelf);

	}

	public void Open(){

		if (!mThisMenu.activeSelf) {
			OpenOrClose();
		}
	}

	public void Close(){

		if (mThisMenu.activeSelf) {
			OpenOrClose();
		}
	}
}
