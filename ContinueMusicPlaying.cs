using UnityEngine;
using System.Collections;

public class ContinueMusicPlaying : MonoBehaviour {

	void Awake() {

		GameObject currentMusic = GameObject.FindGameObjectWithTag("MusicSource");

		if (currentMusic != this.gameObject) {
			Destroy(this.gameObject);
			return;
		} else {
			currentMusic = this.gameObject;
		}

		DontDestroyOnLoad(this.gameObject);
	}
		
}
