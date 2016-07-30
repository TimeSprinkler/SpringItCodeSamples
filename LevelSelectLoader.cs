using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectLoader : MonoBehaviour {
	
	/*public string levelPrefix;
	public GameObject levelItemPrefab;

	public List<GameObject> mLevelGroupList;

	// Use this for initialization
	void Awake(){

	}

	void Start () {

		int totalLevels = 0;
		int currentLevelSuffix = 1;
		string levelName = this.levelPrefix + currentLevelSuffix.ToString();
		TextAsset currentLevel = Resources.Load<TextAsset>(levelName);

		while(currentLevel != null){
			totalLevels++;

			currentLevelSuffix++;
			levelName = this.levelPrefix + currentLevelSuffix.ToString();
			currentLevel = Resources.Load<TextAsset>(levelName);
		}

		currentLevelSuffix = 1;

		for(int i = 0; i < mLevelGroupList.Count ; i++){

			int startLevelSuffix = currentLevelSuffix;
			currentLevelSuffix += 9;
			if(currentLevelSuffix <= totalLevels){

				mLevelGroupList[i].GetComponent<LSLoadLevelGroup>().levelPrefix = levelPrefix;
				mLevelGroupList[i].GetComponent<LSLoadLevelGroup>().levelItemPrefab = levelItemPrefab;
				mLevelGroupList[i].GetComponent<LSLoadLevelGroup>().LoadLevelGroup(startLevelSuffix, currentLevelSuffix);

			}else{
				mLevelGroupList[i].GetComponent<LSLoadLevelGroup>().NoLevelsToLoad();
			}

			currentLevelSuffix++;
		}


	}*/
}
