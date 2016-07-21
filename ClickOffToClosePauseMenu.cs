using UnityEngine;
using System.Collections;

public class ClickOffToClosePauseMenu : MonoBehaviour {

	public GameObject mThisMenu;

	void Start(){		
		EventHandler.OffGUIClick += CloseMenu;	
	}
	
	void OnDestroy(){
		EventHandler.OffGUIClick -= CloseMenu;
	}
	
	void CloseMenu(){
		
		mThisMenu.SetActive(false);
		
		if(mThisMenu.GetComponent<PauseGame>() != null){
			mThisMenu.GetComponent<PauseGame>().Resume();
		}
		
	}
}
