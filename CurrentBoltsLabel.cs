using UnityEngine;
using System.Collections;

public class CurrentBoltsLabel : MonoBehaviour {


	void Start () {

		UpdateBolts ();
		InvokeRepeating ("UpdateBolts", 2f, 2f);
	}

	private void UpdateBolts(){
		int currentBolts = PlayerData.instance.mNumberOfStarsEarned;
		this.GetComponent<UILabel> ().text = "x " + currentBolts;
	}
}
