using UnityEngine;
using System.Collections;

public class OpenOrClosePauseMenu : MonoBehaviour {

	public GameObject mThisMenu;


	public void OpenOrClose(){
		
		mThisMenu.SetActive(!mThisMenu.activeSelf);

		if(mThisMenu.activeSelf){

			if(mThisMenu.GetComponent<PauseGame>() != null){
				mThisMenu.GetComponent<PauseGame>().Pause();
			}
		}else{
			if(mThisMenu.GetComponent<PauseGame>() != null){
				mThisMenu.GetComponent<PauseGame>().Resume();
			}
		}
		
	}
}
