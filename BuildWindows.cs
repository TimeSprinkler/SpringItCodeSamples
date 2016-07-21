#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BuildWindows: MonoBehaviour{

	private string mFilePath = "Prefabs/Background/Windows/Window";

	private float mWindowZSpawnDistance = 1.15f;

	private GameObject mWallWindow1; //8/8 filled in
	private GameObject mWallWindow2; //7/8 filled in
	private GameObject mWallWindow2R; //Reversed Version
	private GameObject mWallWindow3; //6/8 filled in
	private GameObject mWallWindow4; //6/6 filled
	private GameObject mWallWindow5; //5/6 filled 2 by 2 square with 1 of\
	private GameObject mWallWindow5R; //Reversed Version
	private GameObject mWallWindow6; //5/6 u shape
	private GameObject mWallWindow7; //4/6 t shape
	private GameObject mWallWindow8; //4/4 filled
	private GameObject mWallWindow9; //3/4 filled

	//Tiles start at 29.  the upper right corner is the only number I need form each tile
	private int mWindow1TileCorner = 148;//119
	private int mWindow1VTileCorner = 46;//17
	private int mWindow2TileCorner = 256;//227
	private int mWindow2bTileCorner = 252;//223
	private int mWindow2cTileCorner = 200;//171
	private int mWindow2dTileCorner = 260;//231
	private int mWindow2eTileCorner = 158;//129
	private int mWindow2fTileCorner = 156;//127
	private int mWindow2gTileCorner = 52;//23
	private int mWindow2hTileCorner = 54;//25
	private int mWindow3TileCorner = 204;//175
	private int mWindow3bTileCorner = 152;//123
	private int mWindow3cTileCorner = 48;//19
	private int mWindow3dTileCorner = 50;//21
	private int mWindow4TileCorner = 41;//12
	private int mWindow4vTileCorner = 44;//15
	private int mWindow5TileCorner = 242;//213
	private int mWindow5bTileCorner = 190;//161
	private int mWindow5cTileCorner = 138;//109
	private int mWindow5dTileCorner = 86;//57
	private int mWindow5eTileCorner = 212;//183
	private int mWindow5fTileCorner = 214;//185
	private int mWindow5gTileCorner = 134;//105
	private int mWindow5hTileCorner = 136;//107
	private int mWindow6TileCorner = 141;//112
	private int mWindow6bTileCorner = 89;//60
	private int mWindow6cTileCorner = 144;//115
	private int mWindow6dTileCorner = 146;//117
	private int mWindow7TileCorner = 245;//216
	private int mWindow7bTileCorner = 193;//164
	private int mWindow7cTileCorner = 222;//193
	private int mWindow7dTileCorner = 224;//195
	private int mWindow8TileCorner = 34;//5
	private int mWindow9TileCorner = 30;//1
	private int mWindow9bTileCorner = 32;//3
	private int mWindow9cTileCorner = 82;//53
	private int mWindow9dTileCorner = 84;//55
	
	private int mLevelWidth;
	private int mLevelHeight;

	public void Build(List<int> backgroundTileInts, long levelWidth, long levelHeight){

		LoadInWallWindows();

		mLevelWidth = (int) levelWidth;
		mLevelHeight = (int) levelHeight;

		for(int i = 0; i < backgroundTileInts.Count; i++){																																																																																																							
			int switchVariable = 0;

			//This is a manual way for me to match each tile found to which window.  Only one tile per window should be matched
			if(backgroundTileInts[i] == mWindow1TileCorner || backgroundTileInts[i] == mWindow1VTileCorner) 	switchVariable = 1;//Window1
	
			if(backgroundTileInts[i] == mWindow2TileCorner || backgroundTileInts[i] == mWindow2bTileCorner||
			   backgroundTileInts[i] == mWindow2cTileCorner || backgroundTileInts[i] == mWindow2dTileCorner||
			   backgroundTileInts[i] == mWindow2eTileCorner || backgroundTileInts[i] == mWindow2fTileCorner||
			   backgroundTileInts[i] == mWindow2gTileCorner || backgroundTileInts[i] == mWindow2hTileCorner) 	switchVariable = 2;//Window2

			if(backgroundTileInts[i] == mWindow3TileCorner || backgroundTileInts[i] == mWindow3bTileCorner||
			   backgroundTileInts[i] == mWindow3cTileCorner || backgroundTileInts[i] == mWindow3dTileCorner) 	switchVariable = 3;//Window3

			if(backgroundTileInts[i] == mWindow4TileCorner || backgroundTileInts[i] == mWindow4vTileCorner) 	switchVariable = 4;//Window4

			if(backgroundTileInts[i] == mWindow5TileCorner || backgroundTileInts[i] == mWindow5bTileCorner||
			   backgroundTileInts[i] == mWindow5cTileCorner || backgroundTileInts[i] == mWindow5dTileCorner||
			   backgroundTileInts[i] == mWindow5eTileCorner || backgroundTileInts[i] == mWindow5fTileCorner||
			   backgroundTileInts[i] == mWindow5gTileCorner || backgroundTileInts[i] == mWindow5hTileCorner) 	switchVariable = 5;//Window2

			if(backgroundTileInts[i] == mWindow6TileCorner || backgroundTileInts[i] == mWindow6bTileCorner||
			   backgroundTileInts[i] == mWindow6cTileCorner || backgroundTileInts[i] == mWindow6dTileCorner)  switchVariable = 6;//Window6

			if(backgroundTileInts[i] == mWindow7TileCorner || backgroundTileInts[i] == mWindow7bTileCorner||
			   backgroundTileInts[i] == mWindow7cTileCorner || backgroundTileInts[i] == mWindow7dTileCorner)  switchVariable = 7;//Window7

			if(backgroundTileInts[i] == mWindow8TileCorner) switchVariable = 8;//Window8

			if(backgroundTileInts[i] == mWindow9TileCorner || backgroundTileInts[i] == mWindow9bTileCorner||
			   backgroundTileInts[i] == mWindow9cTileCorner || backgroundTileInts[i] == mWindow9dTileCorner)  switchVariable = 9;//Window9

			int oppositeCornerPosition = DetermineOppositeCornerPosition(backgroundTileInts[i], i);
			Vector3 spawnPosition = Vector3.Lerp(getInstantiatePosition(i),getInstantiatePosition(oppositeCornerPosition), 0.5f);	
			int rotationAmount = DetermineRotationAmount(backgroundTileInts[i]);
			bool flipTexture = IsTextureFlipped(backgroundTileInts[i]);

			GameObject tempObject;

			switch(switchVariable){
			
			case 0:
				break;
				
			case 1: //Window1
								
				tempObject = Instantiate(mWallWindow1,spawnPosition, Quaternion.identity) as GameObject;
				tempObject.transform.Rotate(0,0,rotationAmount);
				tempObject.name = "Window1";

				break;
				
			case 2: //Window2
												
				if(flipTexture){
					tempObject = Instantiate(mWallWindow2R,spawnPosition, Quaternion.identity) as GameObject;
					
				} else{
					tempObject = Instantiate(mWallWindow2,spawnPosition, Quaternion.identity) as GameObject;
				}

				tempObject.transform.Rotate(0,0,rotationAmount);
				tempObject.name = "Window2";
				
				break;
				
			case 3: //Window3
				
				tempObject = Instantiate(mWallWindow3,spawnPosition, Quaternion.identity) as GameObject;
				
				tempObject.transform.Rotate(0,0,rotationAmount);
				tempObject.name = "Window3";
				
				break;
			
			case 4: //Window4
				
				tempObject = Instantiate(mWallWindow4,spawnPosition, Quaternion.identity) as GameObject;

				tempObject.transform.Rotate(0,0,rotationAmount);
				tempObject.name = "Window4";
				
				break;
	

			case 5: //Window5

				if(flipTexture){
					tempObject = Instantiate(mWallWindow5R,spawnPosition, Quaternion.identity) as GameObject;

				} else{
					tempObject = Instantiate(mWallWindow5,spawnPosition, Quaternion.identity) as GameObject;
				}

				tempObject.transform.Rotate(0,0,rotationAmount);
				tempObject.name = "Window5";
				
				break;

			case 6: //Window6
				
				tempObject = Instantiate(mWallWindow6,spawnPosition, Quaternion.identity) as GameObject;
				
				tempObject.transform.Rotate(0,0,rotationAmount);
				tempObject.name = "Window6";
				
				break;

			case 7: //Window7
				
				tempObject = Instantiate(mWallWindow7,spawnPosition, Quaternion.identity) as GameObject;
				
				tempObject.transform.Rotate(0,0,rotationAmount);
				tempObject.name = "Window7";
				
				break;

			case 8: //Window8
				
				tempObject = Instantiate(mWallWindow8,spawnPosition, Quaternion.identity) as GameObject;

				tempObject.name = "Window8";
				
				break;

			case 9: //Window9
				
				tempObject = Instantiate(mWallWindow9,spawnPosition, Quaternion.identity) as GameObject;
				
				tempObject.transform.Rotate(0,0,rotationAmount);
				tempObject.name = "Window9";
				
				break;
				
			default:
				break;
			}
		}
	}

	void LoadInWallWindows(){

		mWallWindow1 = Resources.Load (mFilePath + "1", typeof(GameObject)) as GameObject;
		//Debug.Log (mWallWindow1.name);

		mWallWindow2 = Resources.Load (mFilePath + "2") as GameObject;
		mWallWindow3 = Resources.Load (mFilePath + "3") as GameObject;
		mWallWindow2R = Resources.Load (mFilePath + "2R") as GameObject;
		mWallWindow4 = Resources.Load (mFilePath + "4") as GameObject;
		mWallWindow5 = Resources.Load (mFilePath + "5") as GameObject;
		mWallWindow5R = Resources.Load (mFilePath + "5R") as GameObject;
		mWallWindow6 = Resources.Load (mFilePath + "6") as GameObject;
		mWallWindow7 = Resources.Load (mFilePath + "7") as GameObject;
		mWallWindow8 = Resources.Load (mFilePath + "8") as GameObject;
		mWallWindow9 = Resources.Load (mFilePath + "9") as GameObject;

	}

	Vector3 getInstantiatePosition(int incomingData){
		
		Vector3 tempVector = new Vector3(0,0,0);
		
		tempVector.x = (incomingData % mLevelWidth) * UnpackLevel.mLevelScaling;
		tempVector.y = UnpackLevel.mLevelScaling * (mLevelHeight - (incomingData/mLevelWidth));
		tempVector.z = mWindowZSpawnDistance;

		return tempVector;
	}

	bool IsTextureFlipped(int tileValue){

		if(tileValue == mWindow2bTileCorner || tileValue == mWindow2cTileCorner||
		   tileValue == mWindow2fTileCorner || tileValue == mWindow2hTileCorner||
		   tileValue == mWindow5bTileCorner || tileValue == mWindow5cTileCorner||
		   tileValue == mWindow5gTileCorner || tileValue == mWindow5fTileCorner){		
			return true;
		
		}else{
			return false;
		}
	}

	int DetermineRotationAmount(int tileValue){

		int switchVariable = 0;

		if(tileValue == mWindow1VTileCorner || tileValue == mWindow2gTileCorner||
		   tileValue == mWindow2fTileCorner || tileValue == mWindow3dTileCorner||
		   tileValue == mWindow4vTileCorner || tileValue == mWindow5hTileCorner||
		   tileValue == mWindow5fTileCorner || tileValue == mWindow6dTileCorner||
		   tileValue == mWindow7cTileCorner || tileValue == mWindow9cTileCorner){			switchVariable = 1;//Rotate 90
			
		}else if(tileValue == mWindow2dTileCorner || tileValue == mWindow2cTileCorner||
		         tileValue == mWindow3bTileCorner || tileValue == mWindow5bTileCorner||
		         tileValue == mWindow5dTileCorner || tileValue == mWindow6bTileCorner||
		         tileValue == mWindow7bTileCorner || tileValue == mWindow9dTileCorner){ 	switchVariable = 2;//Rotate 180
			
		}else if(tileValue == mWindow2eTileCorner || tileValue == mWindow2hTileCorner||
		         tileValue == mWindow3cTileCorner || tileValue == mWindow5eTileCorner||
		         tileValue == mWindow5gTileCorner || tileValue == mWindow6cTileCorner||
		         tileValue == mWindow7dTileCorner || tileValue == mWindow9bTileCorner){		switchVariable = 3;//Rotate 270
			
		}

		switch(switchVariable){

		default:
			return 0;

		case 1:
			return 90;

		case 2:
			return 180;
		
		case 3:
			return 270;
		

		}
	}

	int DetermineOppositeCornerPosition(int tileValue, int i){

		int switchVariable = 0;

		if(tileValue == mWindow1TileCorner || tileValue == mWindow2TileCorner||
		   tileValue == mWindow2bTileCorner || tileValue == mWindow2cTileCorner||
		   tileValue == mWindow2dTileCorner || tileValue == mWindow3TileCorner||
		   tileValue == mWindow3bTileCorner){												switchVariable = 1;//Case 2by4

		}else if(tileValue == mWindow1VTileCorner || tileValue == mWindow2eTileCorner||
		   		 tileValue == mWindow2fTileCorner || tileValue == mWindow2gTileCorner||
		   		 tileValue == mWindow2hTileCorner || tileValue == mWindow3cTileCorner||
		   		 tileValue == mWindow3dTileCorner){ 										switchVariable = 2;//Case 2by4 Vertical

		}else if(tileValue == mWindow4TileCorner || tileValue == mWindow5TileCorner||
		         tileValue == mWindow5bTileCorner || tileValue == mWindow5cTileCorner||
		         tileValue == mWindow5dTileCorner || tileValue == mWindow6TileCorner||
		         tileValue == mWindow6bTileCorner|| tileValue == mWindow7TileCorner||
		         tileValue == mWindow7bTileCorner )	{										switchVariable = 3;//Case 2by3

		}else if(tileValue == mWindow4vTileCorner || tileValue == mWindow5eTileCorner||
		         tileValue == mWindow5fTileCorner || tileValue == mWindow5gTileCorner||
		         tileValue == mWindow5hTileCorner || tileValue == mWindow6cTileCorner||
		         tileValue == mWindow6dTileCorner|| tileValue == mWindow7cTileCorner||
		         tileValue == mWindow7dTileCorner )	{										switchVariable = 4;//Case 2by3 Vertical

		}else if(tileValue == mWindow8TileCorner || tileValue == mWindow9TileCorner||
		         tileValue == mWindow9bTileCorner || tileValue == mWindow9bTileCorner||
		         tileValue == mWindow9cTileCorner || tileValue == mWindow9dTileCorner)		switchVariable = 5;//Case 2by2

		switch(switchVariable){
			
		default:
			return 0;

		case 1:
			return i + mLevelWidth + 3;
					
		case 2:
			return i + 3 * mLevelWidth + 1;

		case 3:
			return i + mLevelWidth + 2;

		case 4:
			return i + 2 * mLevelWidth + 1;

		case 5:
			return i + mLevelWidth + 1;


		}
	}

}
#endif
