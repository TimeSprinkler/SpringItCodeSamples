#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BuildScenes {

	//private static string tutorialPrefix = "Game Data/tutorial level";
	private static string levelPrefix = "Game Data/playtestlvl";
	private static string mSaveLocation = "Assets/Resources/Game Scenes/";
	private static string mSceneName = " SpringItLevel.unity";
	//private static string mTutorialName = " SpringItTutorial.unity";

	[MenuItem("Spring It/Build Scenes")]
	private static void BuildGameScenes(){

		int currentLevelSuffix = 1;

		string levelName = levelPrefix + currentLevelSuffix.ToString();
		TextAsset currentLevel = Resources.Load<TextAsset>(levelName);

		if(Selection.objects.Length > 0){

			List<string> mNameList = new List<string>();

			for(int i = 0; i < Selection.objects.Length; i++){

				mNameList.Add (Selection.objects[i].ToString());

			}

			for(int i = 0; i < mNameList.Count; i++){
			
				currentLevelSuffix = int.Parse(mNameList[i].Split (' ')[0]);
				levelName = levelPrefix + currentLevelSuffix.ToString();

				currentLevel = Resources.Load<TextAsset>(levelName);

				if(currentLevel == null){
					break;
				}

				EditorApplication.NewScene();
				GameObject.DestroyImmediate(Camera.main.gameObject);
				
				LevelLoader.instance.DoPreLoad(currentLevel,currentLevelSuffix);
				EditorApplication.SaveScene(mSaveLocation + currentLevelSuffix.ToString () + mSceneName);
			}

		}
		else{
			while(currentLevel != null){

				levelName = levelPrefix + currentLevelSuffix.ToString();
				currentLevel = Resources.Load<TextAsset>(levelName);

				if(currentLevel == null){
					break;
				}
		
				EditorApplication.NewScene();
				GameObject.DestroyImmediate(Camera.main.gameObject);

				LevelLoader.instance.DoPreLoad(currentLevel,currentLevelSuffix);
				EditorApplication.SaveScene(mSaveLocation + currentLevelSuffix.ToString () + mSceneName);
		

				currentLevelSuffix++;

			}
		}
	}

	[MenuItem("Spring It/Build Tutorials")]
	private static void BuildTutorials(){

		Debug.LogWarning ("Don't do that");

		//int currentTutorialSuffix = 1;
		
		//string levelName = tutorialPrefix + currentTutorialSuffix.ToString();
		//TextAsset currentLevel = Resources.Load<TextAsset>(levelName);

		//cuurently disabled so people don't ruin things
		/*while(currentLevel != null){
			
			levelName = tutorialPrefix + currentTutorialSuffix.ToString();
			currentLevel = Resources.Load<TextAsset>(levelName);
			
			if(currentLevel == null){
				break;
			}
			
			EditorApplication.NewScene();
			GameObject.DestroyImmediate(Camera.main.gameObject);
						
			LevelLoader.instance.DoPreLoad(currentLevel,currentTutorialSuffix);
			EditorApplication.SaveScene(mSaveLocation + currentTutorialSuffix.ToString () + mTutorialName);
			
			
			currentTutorialSuffix++;			
		}*/
	}

	[MenuItem("Spring It/Add Scenes To Build")]
	private static void AddScenesToBuild(){

		int i = 0;

		if(Selection.objects.Length > 0){

			List<EditorBuildSettingsScene> buildList = new List<EditorBuildSettingsScene>();

			foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes){
				buildList.Add (scene);
			}

			foreach(Object selectedObject in Selection.objects){
				string[] sceneName = AssetDatabase.GetAssetPath(selectedObject).Split('.');

				if(sceneName[1] == "unity"){
					if(!buildList.Exists(x => x.path == AssetDatabase.GetAssetPath(selectedObject))){

						EditorBuildSettingsScene scene= new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(selectedObject), true);
						buildList.Add (scene);
						i++;

					}else{
						Debug.LogWarning("AddScenesToBuild: Please select scene files (.unity)");
						return;
					}
				}

			}

			EditorBuildSettings.scenes = buildList.ToArray();
			Debug.Log("AddScenesToBuild: " + i + " scenes added to build. " + (Selection.objects.Length -i) + " scenes already exist.");
		}


	}
}
#endif