using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	void OnClick(){

		Destroy(GameObject.FindGameObjectWithTag("MusicSource"));
		Application.LoadLevel("MainMenu");
	}
}
