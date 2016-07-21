using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelData {

	public int mID;
	public bool mIsUnlocked;

	private bool isComplete = false;
	public bool mIsComplete{

		get{ return isComplete;}
		set{
			if(value){
				isComplete = true;
			}
		}
	}
	
	private int numberOfStars = 0;	
	public int mNumberOfStars{
		
		get{ return numberOfStars;}
		set{
			if(numberOfStars < value){
				numberOfStars = value;
			}
		}	
	}

	public int mNumberOfFails = 0;
	public float mTimeToComplete = 0.0f;
	
	public int mMoves;
	public int mRobotsUsed;
	public int mToolsUsed;
	
	public int mRequiredToolsForColors = 2;
	public int mRequiredMovesForColors = 15;
	public int mRequiredRobotsForColors = 1;


	public LevelData(int id){
	
		mID = id;
		mMoves = 0;
		mRobotsUsed = 0;
		mToolsUsed = 0;
		mNumberOfStars = 0;
		mNumberOfFails = 0;
		mTimeToComplete = 0.0f;
	}
}
