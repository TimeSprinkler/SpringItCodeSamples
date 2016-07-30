using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickOffToClose : MonoBehaviour {

	public MainMenuOptionsButton mScriptWithMenuClose;
	public OpenOrCloseMenu mOpenOrCloseScripts;

	void Start(){

		EventHandler.OffGUIClick += CloseMenu;

	}

	void OnDestroy(){
		EventHandler.OffGUIClick -= CloseMenu;
	}

	void CloseMenu(){
		if(mScriptWithMenuClose != null){
			if(mScriptWithMenuClose.mIsOpen){
				mScriptWithMenuClose.CloseMenu();
			}
		}

		if(mOpenOrCloseScripts != null){
			if(mOpenOrCloseScripts.mThisMenu.activeSelf){
				mOpenOrCloseScripts.OpenOrClose();
			}
		}
	}


}
