using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class UnpackLevelDesignerData : UnitySingleton<UnpackLevelDesignerData> {

	private TextAsset mJSONDoc;
	private Dictionary<string, object> mJSONData;

	private int mID;
	
	private List<object> mLevels;

	public void LoadDesignerData(){
		mJSONDoc = Resources.Load ("Designer JSON", typeof(TextAsset)) as TextAsset;
		mJSONData = MiniJSON.Json.Deserialize(mJSONDoc.text) as Dictionary<string, object>;

		mLevels = (List<object>)(mJSONData["levels"]);
		PlayerData.instance.mLevelDataList = new List<LevelData> ();

		for(int j = 0; j < mLevels.Count; j++){

			object idString = (mLevels[j] as Dictionary<string, object>)["ID"];
			mID = int.Parse(idString.ToString());

			LevelData mLoadingLevel = new LevelData(mID);

			mLoadingLevel.mRequiredToolsForColors = LoadInToolColors(j);
			mLoadingLevel.mRequiredMovesForColors = LoadInMoveColors(j);
			mLoadingLevel.mRequiredRobotsForColors = LoadInRobotColors(j);

			PlayerData.instance.mLevelDataList.Add(mLoadingLevel);

		}

	}

	private int LoadInToolColors(int j){

		int mToolColor;

		long tools = (long)((mLevels[j] as Dictionary<string, object>)["tool colors"]);
		mToolColor = (int)tools;

		return mToolColor;
	}

	private int LoadInMoveColors(int j){

		int mMoveColor;
		
		long moves = (long)((mLevels[j] as Dictionary<string, object>)["move colors"]);
		mMoveColor = (int)moves;

		return mMoveColor;
	}

	private int LoadInRobotColors(int j){

		int mRobotColors;

		long robots = (long)((mLevels[j] as Dictionary<string, object>)["robot colors"]);
		mRobotColors = (int)robots;
		

		return mRobotColors;
	}
}
