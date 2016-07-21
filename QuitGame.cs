using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	public void SaveAndQuit(){

		SaveAndLoad.Save ();
		Application.Quit();

	}
}
