using UnityEngine;
using System.Collections;

public class TutorialPage{

	//Getters and setters needed here
	public string mName{ get; private set; }
	public int mLevelID{ get; private set; }
	public int mOrderID;

	public string mDescription{ get; private set; }
	public Texture mImage{ get; private set; }

	public TutorialPage(string name, Texture image, string description, int levelID, int orderID){

		mName = name;
		mLevelID = levelID;
		mOrderID = orderID;

		mDescription = description;
		mImage = image;
	}



}
