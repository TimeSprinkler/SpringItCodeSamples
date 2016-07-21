using UnityEngine;
using System.Collections;

public class RobotPointsLabel : MonoBehaviour {

	/*private int mPoints;
	
	void Awake(){
		this.gameObject.GetComponent<UILabel>().text = "0";
	}
	
	void UpdateLabel(GameObject robot, int points){

		mPoints += (Currentlevel.instance.mCompletedRobotPoints + points);
		Currentlevel.instance.mPoints = mPoints;

		this.gameObject.GetComponent<UILabel>().text = mPoints.ToString();
	}

	void UpdateLabel(int points){
		mPoints += (points);
		Currentlevel.instance.mPoints = mPoints;
		
		this.gameObject.GetComponent<UILabel>().text = mPoints.ToString();
	}

	void Destroy(){
		EventHandler.OnRobotScored -= UpdateLabel;
		EventHandler.OnRobotPoints -= UpdateLabel;
	}
	
	void OnEnable(){
		EventHandler.OnRobotScored += UpdateLabel;
		EventHandler.OnRobotPoints += UpdateLabel;
	}
	
	void OnDisable(){
		EventHandler.OnRobotScored -= UpdateLabel;
		EventHandler.OnRobotPoints -= UpdateLabel;
	}*/
}
