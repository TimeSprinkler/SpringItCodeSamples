//Below is an example of what you should get from the tiled level editor when exported
//The first set should be 8 layers similiar to what is shown below
//{ "height":15,
//"layers":[
//   {
//	"data":[0,0,...]
//"height":15,
//"name":"Level Layer",
//"opacity":1,
//"type":"tilelayer",
//"visible":true,
//"width":20,
//"x":0,
//"y":0
//}, 
// 
//Below this point shows what each fo the tile sets will look like, there should be 8 of them in total as well.  
//The difference with them is that their identifying attribute "firstgrid" will nto increase by 1 for each tile set becasue many of the sets have more than 1 tile.
//"tileheight":256,
//"tilesets":[
// {
//	"firstgid":1,
//	"image":"Tilesets\/Conveyor\/Conveyor.png",
//	"imageheight":256,
//	"imagewidth":512,
//	"margin":0,
//	"name":"Conveyor",
//	"properties":
//	{
//		
//	},
//	"spacing":0,
//	"tileheight":256,
//	"tilewidth":256
// }, 
//
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

[ExecuteInEditMode]
public class UnpackLevel : MonoBehaviour {

	public static int mID;
	public int mNumberOfTileSets = 10;
	public static int mLevelScaling = 2;
	public static float mCameraStartZoom = -15;
	public float mFloorForwardDistance = -1.01f;
	public Vector3 mCameraStartingRotation = new Vector3(15,0,0);

	public static int mTotalNumberOfCurrentPlaceableMechanics = 4;
	public static int[] mConveyorTileInt = {1, 2};
	public static int[] mSpringTileInt = {3, 4};
	public static int[] mFanTileInt= {5, 6, 7, 8};
	public static int[] mMagnetTileInt= {9, 10, 11, 12};
	public static int[] mSpawnerTileInt = {13, 14};
	public static int[] mCollectorTileInt = {15, 16};
	public static int mRobotTileInt = 17;
	public static int mFurnaceTileInt = 18;
	public static int[] mBudgetTileInt = {19, 20, 21, 22};
	public static int mFloorTileInt = 23;
	public static int mWallsTileInt = 24;
	public static int mWindowTileInt = 25;
	public static int mButtonTileInt = 26;
	public static int mDoorTileInt = 27;
	public static int mCannonTileInt = 28;
	public static int mCameraCenterTileInt = 29;

	public static Vector3 mCameraStartingPosition = Vector3.one;

	#if UNITY_EDITOR

	private Create2DPanel m2DPanelScript;

	private Dictionary<string, object> mJSONData;

	//Stuff for loading
	private long mLevelWidth;
	private long mLevelHeight;
	private List<string> mTileNames;

	private List<int> mLevelLayerDataLocations = new List<int>();
	private List<int> mStructuringDataLocations = new List<int>();
	private List<int> mPlaceableDataLocations = new List<int>();
	private List<int> mBackgroundDataLocations = new List<int>();
	private List<int> mAvailiblePrefabs = new List<int>();
	private List<int> mRobotDataLocations = new List<int>();
	private List<int> mBudgetDataLocations = new List<int>();
	private List<int> mInterractableDataLocations = new List<int>();

	//Stuff for building
	private List<GameObject> mLoadedResources = new List<GameObject>();
	private Transform mCollector;

	public void Unpack(TextAsset levelToLoad, int levelNumber) {
	
		mID = levelNumber;
		mJSONData = MiniJSON.Json.Deserialize(levelToLoad.text) as Dictionary<string, object>;
		m2DPanelScript = this.gameObject.GetComponent<Create2DPanel>();
		m2DPanelScript.ClearSpawnerLocations();

		UnpackTileSets();
		UnpackDataLocations();
		BuildLevelLayer();
		BuildStructuringLevel();
		//BuildPlaceableLayer();
		BuildBackgroundLayer();
		BuildDoorsAndSwitches();
		
	}

	void UnpackTileSets(){
	
		mTileNames = new List<string>();
		int j = 0;
	
		while(j < (mJSONData["tilesets"] as List<object>).Count){

			mTileNames.Add(((mJSONData["tilesets"] as List<object>)[j] as Dictionary<string, object>)["name"].ToString());
			j++;
		}
	}

	// Each one of the for loops below are manually added for each new layer added to the editor
	void UnpackDataLocations(){

		mLevelWidth = (long)((mJSONData["layers"] as List<object>)[1] as Dictionary<string, object>)["width"];
		mLevelHeight = (long)((mJSONData["layers"] as List<object>)[1] as Dictionary<string, object>)["height"];
			
		for( int i = 0; i < mLevelHeight*mLevelWidth; i++){
			long dataMiddleMan = (long)(((mJSONData["layers"] as List<object>)[0] as Dictionary<string, object>)["data"] as List<object>)[i];
			mLevelLayerDataLocations.Add((int)dataMiddleMan);
		}

		for( int i = 0; i < mLevelHeight*mLevelWidth; i++){
			long dataMiddleMan = (long)(((mJSONData["layers"] as List<object>)[1] as Dictionary<string, object>)["data"]as List<object>)[i];
			mStructuringDataLocations.Add((int)dataMiddleMan);
		}

		for( int i = 0; i < mLevelHeight*mLevelWidth; i++){
			long dataMiddleMan = (long)(((mJSONData["layers"] as List<object>)[2] as Dictionary<string, object>)["data"]as List<object>)[i];
			mPlaceableDataLocations.Add((int)dataMiddleMan);
		}

		for( int i = 0; i < mLevelHeight*mLevelWidth; i++){
			long dataMiddleMan = (long)(((mJSONData["layers"] as List<object>)[3] as Dictionary<string, object>)["data"]as List<object>)[i];
			mBackgroundDataLocations.Add((int)dataMiddleMan);
		}

		for( int i = 0; i < mLevelHeight*mLevelWidth; i++){
			long dataMiddleMan = (long)(((mJSONData["layers"] as List<object>)[4] as Dictionary<string, object>)["data"]as List<object>)[i];
			mAvailiblePrefabs.Add((int)dataMiddleMan);
		}

		for( int i = 0; i < mLevelHeight*mLevelWidth; i++){
			long dataMiddleMan = (long)(((mJSONData["layers"] as List<object>)[5] as Dictionary<string, object>)["data"]as List<object>)[i];
			mRobotDataLocations.Add((int)dataMiddleMan);
		}

		for( int i = 0; i < mLevelHeight*mLevelWidth; i++){
			long dataMiddleMan = (long)(((mJSONData["layers"] as List<object>)[6] as Dictionary<string, object>)["data"]as List<object>)[i];
			mBudgetDataLocations.Add((int)dataMiddleMan);
		}

		for(int j = 7; j < (mJSONData["layers"] as List<object>).Count;j++){
			for( int i = 0; i < mLevelHeight*mLevelWidth; i++){
				long dataMiddleMan = (long)(((mJSONData["layers"] as List<object>)[j] as Dictionary<string, object>)["data"]as List<object>)[i];
				mInterractableDataLocations.Add((int)dataMiddleMan);

			}
		}
	}

	void BuildLevelLayer(){

		ConvertTileNamesToLoadedResources();
		InstantiateTheLevelObjects();
		BuildSceneItems();

	}

	void ConvertTileNamesToLoadedResources(){
		for(int i = 0; i < mTileNames.Count;i++){

			if(Resources.Load ("Prefabs/" + mTileNames[i], typeof(GameObject)) != null){

				mLoadedResources.Add (Resources.Load("Prefabs/" + mTileNames[i], typeof(GameObject)) as GameObject);
			}
		}
	}

	GameObject FindLoadedResource(string name){

		for(int i = 0;i < mLoadedResources.Count;i++){
			if(mLoadedResources[i].name == name){
				return mLoadedResources[i];
			}
		}

		return null;

	}

	void InstantiateTheLevelObjects(){

		for(int i = 0; i < mLevelLayerDataLocations.Count; i++){
			GameObject tempObject;
			GameObject childToBeRemoved;
			int switchVariable = 0;

			//This is a manual way for me to match each tile found to which type of item is being created for the Level layer Only
			if(mLevelLayerDataLocations[i] == 0) switchVariable = 0;
			if((mLevelLayerDataLocations[i] >= mConveyorTileInt[0]) && (mLevelLayerDataLocations[i] <= mConveyorTileInt[mConveyorTileInt.Length - 1])) 		switchVariable = 1;
			if((mLevelLayerDataLocations[i] >= mSpringTileInt[0]) && (mLevelLayerDataLocations[i] <= mSpringTileInt[mSpringTileInt.Length - 1])) 			switchVariable = 2; 
			if((mLevelLayerDataLocations[i] >= mFanTileInt[0]) && (mLevelLayerDataLocations[i] <= mFanTileInt[mFanTileInt.Length - 1])) 					switchVariable = 3;
			if((mLevelLayerDataLocations[i] >= mSpawnerTileInt[0]) && (mLevelLayerDataLocations[i] <= mSpawnerTileInt[mSpawnerTileInt.Length - 1]))  		switchVariable = 4;
			if((mLevelLayerDataLocations[i] >= mCollectorTileInt[0]) && (mLevelLayerDataLocations[i] <= mCollectorTileInt[mCollectorTileInt.Length - 1])) 	switchVariable = 5;
			if(mLevelLayerDataLocations[i] == mRobotTileInt) 																								switchVariable = 6;
			if((mLevelLayerDataLocations[i] >= mMagnetTileInt[0]) && (mLevelLayerDataLocations[i] <= mMagnetTileInt[mMagnetTileInt.Length - 1]))			switchVariable = 7;
			if(mLevelLayerDataLocations[i] == mCannonTileInt)																								switchVariable = 8;

			switch(switchVariable){

			default:
				break;

			case 0:
				break;

			case 1: //Conveyor

				tempObject = Instantiate(FindLoadedResource("Conveyor"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				tempObject.name = FindLoadedResource("Conveyor").name;

				if(mLevelLayerDataLocations[i] == mConveyorTileInt[0]){
					tempObject.transform.Rotate(0,180, 0);
				}
				break;

			case 2: //Spring
				tempObject = Instantiate(FindLoadedResource("Spring"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				tempObject.name = FindLoadedResource("Spring").name;
				
				if(mLevelLayerDataLocations[i] == mSpringTileInt[0]){
					tempObject.transform.Rotate(0,180, 0);
				}
				break;

			case 3: //Fan
				tempObject = Instantiate(FindLoadedResource("Fan"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				tempObject.name = FindLoadedResource("Fan").name;
				
				if(mLevelLayerDataLocations[i] == mFanTileInt[0]){
					tempObject.transform.Rotate(0, 0, 90);
				}

				if(mLevelLayerDataLocations[i] == mFanTileInt[2]){
					tempObject.transform.Rotate(0, 180, 0);
				}

				if(mLevelLayerDataLocations[i] == mFanTileInt[3]){
					tempObject.transform.Rotate(0, 0, -90);
				}
				break;

			case 4: //Spawner

				tempObject = Instantiate(FindLoadedResource("Spawner"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				tempObject.name = FindLoadedResource("Spawner").name;

				if(mLevelLayerDataLocations[i] == mSpawnerTileInt[0]){
					childToBeRemoved = tempObject.transform.FindChild("SpawnerFacingRight").gameObject;
					tempObject.GetComponent<SpawnerModelManager>().mIncorrectModelName = "SpawnerFacingRight";
				}else{
					childToBeRemoved = tempObject.transform.FindChild("SpawnerFacingLeft").gameObject;
					tempObject.GetComponent<SpawnerModelManager>().mIncorrectModelName = "SpawnerFacingLeft";
				}

				DestroyImmediate(childToBeRemoved);
				m2DPanelScript.AddSpawnerLocation(tempObject.transform.position);
				break;

			case 5: //Collector
				tempObject = Instantiate(FindLoadedResource("Collector"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				tempObject.name = FindLoadedResource("Collector").name;

				if(mLevelLayerDataLocations[i] == mCollectorTileInt[0]){
					childToBeRemoved = tempObject.transform.FindChild("CollectorFacingRight").gameObject;
					tempObject.GetComponent<SpawnerModelManager>().mIncorrectModelName = "CollectorFacingRight";
				}else{
					childToBeRemoved = tempObject.transform.FindChild("CollectorFacingLeft").gameObject;
					tempObject.GetComponent<SpawnerModelManager>().mIncorrectModelName = "CollectorFacingLeft";
				}

				DestroyImmediate(childToBeRemoved);
				m2DPanelScript.AddSpawnerLocation(tempObject.transform.position);
				mCollector = tempObject.transform;

				break;

			case 6: //Robot
				tempObject = Instantiate(FindLoadedResource("Robot"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				tempObject.name = FindLoadedResource("Robot").name;
				
				break;

			case 7: //Magnet
				if(FindLoadedResource("Magnet") != null){

					tempObject = Instantiate(FindLoadedResource("Magnet"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
					tempObject.name = FindLoadedResource("Magnet").name;
					
					if(mLevelLayerDataLocations[i] == mMagnetTileInt[0]){
						tempObject.transform.Rotate(0, 0, 90);
					}
					
					if(mLevelLayerDataLocations[i] == mMagnetTileInt[2]){
						tempObject.transform.Rotate(0, 180, 0);
					}
					
					if(mLevelLayerDataLocations[i] == mMagnetTileInt[3]){
						tempObject.transform.Rotate(0, 0, -90);
					}
				}
				break;
			

			case 8: //Cannon

				tempObject = Instantiate(FindLoadedResource("Cannon"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				tempObject.name = FindLoadedResource("Cannon").name;

				break;
			}
		}
	}

	Vector3 getInstantiatePosition(int incomingData){

		Vector3 tempVector = new Vector3(0,0,0);

		tempVector.x = (incomingData % mLevelWidth) * mLevelScaling;
		tempVector.y = mLevelScaling * (mLevelHeight - (incomingData/mLevelWidth));

		return tempVector;
	}

	void BuildSceneItems(){

		CreateAndSetupMainCamera();
		LetThereBeLight();
		CreateTheBackgroundItems();
		CreateGameManager();
		CreateMusic();
		BuildTutorialManager ();
	}

	void CreateAndSetupMainCamera(){

		GameObject mainCamera = Resources.Load ("Prefabs/Main Camera", typeof(GameObject)) as GameObject;

		//These are all of the variables that need to be set on the Main Camera's scripts
		GameObject tempCamera = Instantiate(mainCamera, new Vector3((mLevelScaling * mLevelWidth)/2, mLevelScaling * mLevelHeight, mCameraStartZoom), Quaternion.identity) as GameObject;
		tempCamera.transform.Rotate(mCameraStartingRotation.x, mCameraStartingRotation.y, mCameraStartingRotation.z);

		tempCamera.GetComponent<PlayerObjectButtons>().mEnableEachObject = EnablePlaceables();
		tempCamera.GetComponent<Budget>().mPlaceableAmounts = SetBudget();
		tempCamera.GetComponentInChildren<LoadPlaceableBar>().Load (tempCamera.GetComponent<Budget>().mPlaceableAmounts);

		int robotsToWin = SetRobotsNeededToWin();

		tempCamera.GetComponent<MainCameraGUI>().mRobotsNeededToWin = robotsToWin;
		tempCamera.GetComponent<MainCameraGUI>().mMaxRobotsReleased = SetRobotsReleased();
		
		tempCamera.GetComponent<CameraSceneStartAnimation> ().SetCameraStartPosition (new Vector2 (mLevelWidth, mLevelHeight), mCollector);

		int totalPlaceables = 0;

		for(int i = 0; i < tempCamera.GetComponent<Budget>().mPlaceableAmounts.Length;i++){
			totalPlaceables += tempCamera.GetComponent<Budget>().mPlaceableAmounts[i];
		}

		GameObject levelSizeHolder = new GameObject("Level Size Holder");
		levelSizeHolder.transform.position = new Vector3((int)mLevelWidth * mLevelScaling, (int)mLevelHeight * mLevelScaling, 0);
		levelSizeHolder.AddComponent("SetTopRightLocation");

		CameraMovement.mLevelSize.x = mLevelWidth * mLevelScaling;
		CameraMovement.mLevelSize.y = mLevelHeight * mLevelScaling;

	}

	void LetThereBeLight(){

		GameObject light = Resources.Load ("Prefabs/Directional light", typeof(GameObject)) as GameObject;

		light.transform.rotation = Quaternion.identity;
		light.transform.Rotate(45, 15, 0);
		Instantiate(light, light.transform.position, light.transform.rotation);

	}
	
	void CreateTheBackgroundItems(){
		
		GameObject background = Resources.Load ("Prefabs/Background/BackgroundObject", typeof(GameObject)) as GameObject;
		GameObject surroundingBackground = Resources.Load("Prefabs/Background/SurroundingBackground", typeof(GameObject)) as GameObject;
		GameObject floorLoader	= Resources.Load ("Prefabs/Background/FloorLoader", typeof(GameObject)) as GameObject;

		GameObject  backgroundObject = Instantiate(surroundingBackground, new Vector3(0,0, mFloorForwardDistance), Quaternion.identity) as GameObject;
		backgroundObject.GetComponent<BuildSurroundingBackground>().SetLevelSize((int)mLevelWidth * mLevelScaling, (int)mLevelHeight * mLevelScaling);

		backgroundObject.GetComponent<BuildFarground>().Build();

		backgroundObject = Instantiate(background, background.transform.position, Quaternion.identity) as GameObject;
		backgroundObject.GetComponent<BuildBackground>().Build(mBackgroundDataLocations, (int)mLevelWidth);

		backgroundObject = Instantiate(floorLoader, new Vector3 (0, 0, mFloorForwardDistance), Quaternion.identity) as GameObject;
		backgroundObject.GetComponent<BuildFloor>().Build(mStructuringDataLocations);



	}

	void CreateGameManager(){
		GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
		if(gameManager == null){
			gameManager = Resources.Load ("Prefabs/GameManagers",typeof(GameObject)) as GameObject;
			GameObject.Instantiate(gameManager, Vector3.zero, Quaternion.identity);
		}

	}

	void CreateMusic(){

		GameObject musicPrefab = Resources.Load ("Prefabs/Musical Storage") as GameObject;
		Instantiate(musicPrefab,Vector3.one, Quaternion.identity);

	}

	void BuildTutorialManager(){

		Camera.main.GetComponentInChildren<TutorialManager>().Build();

	}

	bool[] EnablePlaceables(){

		bool[] tempbool = {false, false, false, false};

		for(int i = 0; i < mLevelHeight * mLevelWidth;i++){

			if((mAvailiblePrefabs[i]== mSpringTileInt[0]) || (mAvailiblePrefabs[i] == mSpringTileInt[mSpringTileInt.Length - 1])) 	tempbool[0] = true; //Spring
			if((mAvailiblePrefabs[i] >= mFanTileInt[0]) && (mAvailiblePrefabs[i] <= mFanTileInt[mFanTileInt.Length - 1])) 			tempbool[1] = true;//Fan
			if((mAvailiblePrefabs[i] >= mMagnetTileInt[0]) && (mAvailiblePrefabs[i] <= mMagnetTileInt[mMagnetTileInt.Length - 1])) 	tempbool[2] = true;//Magnet
			if((mAvailiblePrefabs[i] == mCannonTileInt))																			tempbool[3] = true; //Cannon

		}
	
		return tempbool;
	}

	int[] SetBudget(){

		int[] budget = new int[mTotalNumberOfCurrentPlaceableMechanics];

		for(int i = 0; i < mLevelHeight * mLevelWidth;i++){

			int switchVariable = 0;

			if(mBudgetDataLocations[i] == 0) switchVariable = 0;
			if((mBudgetDataLocations[i] >= mSpringTileInt[0]) && (mBudgetDataLocations[i] <= mSpringTileInt[mSpringTileInt.Length - 1])) 	switchVariable = 1; 
			if((mBudgetDataLocations[i] >= mFanTileInt[0]) && (mBudgetDataLocations[i] <= mFanTileInt[mFanTileInt.Length - 1])) 			switchVariable = 2;
			if((mBudgetDataLocations[i] >= mMagnetTileInt[0]) && (mBudgetDataLocations[i] <= mMagnetTileInt[mMagnetTileInt.Length - 1])) 	switchVariable = 3;
			if(mBudgetDataLocations[i] == mCannonTileInt)																					switchVariable = 4;
			if(mBudgetDataLocations[i] == mCameraCenterTileInt)																				switchVariable = 5;

			if(switchVariable != 0 && switchVariable != 5){
				budget[switchVariable - 1]++;
			}

			if(switchVariable == 5){

				mCameraStartingPosition = getInstantiatePosition(i);
				mCameraStartingPosition.y += CameraMovement.mHeightStartOffset;
				mCameraStartingPosition.z = mCameraStartZoom;

			}		
		}
		return budget;
	}

	int SetRobotsNeededToWin(){

		int robotsToWin = 0;

		for(int i = 0; i < (mLevelHeight * mLevelWidth)/2;i++){
			
			if(mRobotDataLocations[i] == mRobotTileInt) robotsToWin++;

		}

		return robotsToWin;

	}

	int SetRobotsReleased(){

		int robotsReleased = 1000000;

		for(int i = (int)(mLevelHeight*mLevelWidth)/2; i < (mLevelHeight * mLevelWidth);i++){
			
			if(mRobotDataLocations[i] == mRobotTileInt) robotsReleased++;
			
		}

		return robotsReleased;
	}

	void BuildStructuringLevel(){

		//List<Vector3> floorPositions = new List<Vector3>();
		//GameObject wallChunk = Resources.Load("Prefabs/Wall Chunk") as GameObject;

		for(int i = 0; i < mStructuringDataLocations.Count; i++){
			GameObject tempObject;																																																																																																								
			int switchVariable = 0;
			
			//This is a manual way for me to match each tile found to which type of item is being created for the Level layer Only
			if(mStructuringDataLocations[i] == 0) switchVariable = 0;
			if(mStructuringDataLocations[i] == mFloorTileInt) switchVariable = 1;//Floors
			if(mStructuringDataLocations[i] == mWallsTileInt) switchVariable = 2;//Walls
						
			switch(switchVariable){
				
			default:
				break;
				
			case 0:
				break;
				
			case 1: //Floors
				
				tempObject = Instantiate(FindLoadedResource("Floor"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				tempObject.name = FindLoadedResource("Floor").name;
	
				break;
				
			case 2: //Walls
				tempObject = Instantiate(FindLoadedResource("Walls"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				tempObject.name = FindLoadedResource("Walls").name;
				tempObject.transform.Rotate(0, 0, 0);

				break;
			
			}
		}
	}

	void BuildBackgroundLayer(){
		
		for(int i = 0; i < mBackgroundDataLocations.Count; i++){
			GameObject tempObject;																																																																																																								
			int switchVariable = 0;
			
			//This is a manual way for me to match each tile found to which type of item is being created for the Level layer Only
			if((mBackgroundDataLocations[i] == 1) || (mBackgroundDataLocations[i] == 2)) switchVariable = 0;
			if(mBackgroundDataLocations[i]== mFurnaceTileInt) switchVariable = 1;//Furnace
			if(mBackgroundDataLocations[i]== mWindowTileInt) switchVariable = 2;
			
			switch(switchVariable){
				
			default:
				break;
				
			case 0:
				break;
				
			case 1: //Furnace
				
				tempObject = Instantiate(FindLoadedResource("Furnace"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				Vector3 tempVector3 = new Vector3(tempObject.transform.position.x, tempObject.transform.position.y, 0);

				tempObject.transform.position = tempVector3;
				tempObject.name = FindLoadedResource("Furnace").name;
				tempObject.tag = "Furnace";
				
				break;

			case 2: //Window
				
				/*tempObject = Instantiate(FindLoadedResource("Window"),getInstantiatePosition(i), Quaternion.identity) as GameObject;
				Vector3 temporaryVector3 = new Vector3(tempObject.transform.position.x, tempObject.transform.position.y, 0);
				
				tempObject.transform.position = temporaryVector3;
				tempObject.name = FindLoadedResource("Window").name;
				tempObject.tag = "Window";*/
				
				break;


			}

		
		}

		GameObject windowHolder = new GameObject();
		windowHolder.AddComponent<BuildWindows>();
		windowHolder.GetComponent<BuildWindows>().Build(mBackgroundDataLocations, mLevelWidth, mLevelHeight);
	}

	void BuildDoorsAndSwitches(){

		int levelSize = (int)mLevelWidth * (int)mLevelHeight;
		int numberOfLayers = 44;
		int buttonCount = 0;
		Color doorGreen = new Color (53f / 256f, 1.0f, 0.0f);
		Color doorPurple = new Color(166f/256f, 39f/256f, 244f/256f);
		Color doorOrange = new Color (1.0f, 152f / 256f, 0.0f);
		Color[] colors = {doorGreen, doorPurple, doorOrange};

		GameObject door = Resources.Load ("Prefabs/Door",typeof(GameObject)) as GameObject;
		GameObject button = Resources.Load ("Prefabs/Switch",typeof(GameObject)) as GameObject;

		for(int i = 0; i < numberOfLayers; i++){

			GameObject currentDoor = new GameObject();
			currentDoor.name = "new";
			int switchVariable = 0;

			List<GameObject> buttonList = new List<GameObject>();

			for(int j = 0; j < levelSize;j++){

				int newInt = levelSize * i + j;

				if(newInt >= mInterractableDataLocations.Count){
					newInt = 0;
				}

				//This is a manual way for me to match each tile found to which type of item is being created for the Level layer Only
				if(mInterractableDataLocations[newInt] == 0) switchVariable = 0;
				if(mInterractableDataLocations[newInt] == mButtonTileInt) switchVariable = 1;//Switch
				if(mInterractableDataLocations[newInt] == mDoorTileInt) switchVariable = 2;//Door
				
				switch(switchVariable){
					
				default:
					break;
					
				case 0:
					break;

				case 1: //button
					
					buttonList.Add(Instantiate(button,getInstantiatePosition(j), Quaternion.identity) as GameObject);
					Vector3 buttonTempVector3 = new Vector3(buttonList[buttonList.Count - 1].transform.position.x, buttonList[buttonList.Count - 1].transform.position.y, 0);
					
					buttonList[buttonList.Count - 1].transform.position = buttonTempVector3;
					buttonList[buttonList.Count - 1].name = button.name;
					buttonList[buttonList.Count - 1].GetComponentInChildren<SwitchMechanic>().mButtonID = buttonCount;
					++buttonCount;

					break;
					
				case 2: //door

					currentDoor = Instantiate(door, getInstantiatePosition(j), Quaternion.identity) as GameObject;
					Vector3 doorTempVector3 = new Vector3(currentDoor.transform.position.x, currentDoor.transform.position.y, 0);
					
					currentDoor.transform.position = doorTempVector3;
					currentDoor.name = door.name;
					currentDoor.GetComponentInChildren<DoorMechanic>().mColor = colors[i];
														
					break;
				}
			}

			//After everything is found and created time to add the buttons to the door script
			if(buttonList.Count > 0 && currentDoor.name != "new"){
				for(int j = 0; j < buttonList.Count;j++){
					buttonList[j].GetComponentInChildren<SwitchMechanic>().mColor = colors[i];
					buttonList[j].GetComponentInChildren<SwitchMechanic>().mDoorObject = currentDoor.GetComponentInChildren<DoorMechanic>().gameObject;
					currentDoor.GetComponentInChildren<DoorMechanic>().AddSwitch(buttonList[j].GetComponentInChildren<SwitchMechanic>().mButtonID);

				}
			}
		}
	}
	#endif	
}
