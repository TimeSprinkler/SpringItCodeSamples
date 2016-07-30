#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildFloor : Create2DPanel {
	
	private Vector3 mCurrentFloorPosition;
	private Vector2 mRemainingTextureOffset = new Vector2 (0,0);
	private float mTotalTextureLengthX;

	[ExecuteInEditMode]
	public void Build(List<int> mDataLocations){

		mLowestFloors = new Vector3[(int)CameraMovement.mLevelSize.x/2];
		FindLowestFloors(mDataLocations);

		mLevelScaling = UnpackLevel.mLevelScaling;

		for(int i = 0; i < mLowestFloors.Length-1;){

			mCurrentFloorPosition = mLowestFloors[i];
			if(i != FindNextFloorTileInRow(i)){
				i = FindNextFloorTileInRow(i);
			}

			CreateFloorTile(mCurrentFloorPosition, mLowestFloors[i]);
			i++;

		}

		GameObject.FindGameObjectWithTag("PanelBackground").GetComponent<BuildSurroundingBackground>().FloorWasBuilt();//Send to Panel Creation
		gameObject.GetComponent<BuildWall>().FloorWasBuilt();
		GameObject.Find ("BackgroundObject(Clone)").GetComponent<BuildBackground>().SpawnBlocks();

	}

	//Go through each column(mLevelSize.x) and findthe lowest place a 19(Floor) appears
	//The values fill horizontally so for a level 10 long you need to check data point 0, 10, 20 ,30 ,40...
	void FindLowestFloors(List<int> mDataLocations){

		int currentLowestFloorDataPoint = 0;
		int	floorLayer = UnpackLevel.mFloorTileInt;

		for(int i = 0; i < CameraMovement.mLevelSize.x/2; i++){

			for(int j = 0; j < mDataLocations.Count; j = j + (int)CameraMovement.mLevelSize.x/2){

				if(mDataLocations[i + j] == floorLayer){
					currentLowestFloorDataPoint = i + j;
				}
			}
			mLowestFloors[i] = ConvertToPosition((currentLowestFloorDataPoint));

		}
	}

	Vector3 ConvertToPosition(int gridLocation){

		Vector3 convertedPosition = new Vector3(0,0,0);

		convertedPosition.y = (int)CameraMovement.mLevelSize.y/2 - gridLocation/(int)(CameraMovement.mLevelSize.x/2);
		convertedPosition.x = gridLocation%(CameraMovement.mLevelSize.x/2);

		return convertedPosition;

	}

	void CreateFloorTile(Vector3 startPosition, Vector3 endPosition){

		GameObject newTile = CreatePlaneObject();
		newTile.name = "FloorPanel";

		newTile.transform.localPosition = SetTilePosition(startPosition, endPosition);

		newTile.transform.localScale = SetTileScale(startPosition, endPosition);

		Vector2 textureScale = new Vector2(1,1);
		textureScale.x = newTile.transform.localScale.x/mGlobalQuadScale;
		textureScale.y = newTile.transform.localScale.y/mGlobalQuadScale;

		newTile.renderer.material = SetupShaderAndMaterial(textureScale);

		this.gameObject.GetComponent<BuildWall>().NewWallLocation(newTile.transform);
	}
	

	Vector3 SetTilePosition(Vector3 startPosition, Vector3 endPosition){

		Vector3 tempPosition = startPosition;
		tempPosition.x = startPosition.x * mLevelScaling + (endPosition.x - startPosition.x);
		tempPosition.y = mLowestYPosition + ((startPosition.y * mLevelScaling - mLevelScaling/2) - mLowestYPosition)/2;
		tempPosition.z = 0;

		return tempPosition;
	}

	Vector3 SetTileScale(Vector3 startPosition, Vector3 endPosition){

		Vector3 tempScale = new Vector3(1,1,1);
		tempScale.x = (Vector3.Distance(startPosition, endPosition) + (mLevelScaling/2)) /mStandardTileSideLength * mGlobalQuadScale;
		tempScale.y = Mathf.Abs((((startPosition.y * mLevelScaling) - mLevelScaling/2 - mLowestYPosition)/2)/mStandardTileSideLength) * mGlobalQuadScale;

		return tempScale;
	}

	Material SetupShaderAndMaterial(Vector2 textureScale){

		Texture floorTexture = Resources.Load ("Prefabs/Background/Foreground") as Texture;

		Shader tileShader = Shader.Find("Mobile/Diffuse");
		Material newMaterial = new Material(tileShader);
		
		newMaterial.mainTexture = floorTexture;
		newMaterial.mainTextureScale = textureScale;

		ChangeTextureOffset(newMaterial);

		mTotalTextureLengthX = mTotalTextureLengthX + textureScale.x;

		return newMaterial;
	}

	void ChangeTextureOffset(Material newMaterial){

		if(mTotalTextureLengthX != 0){

			mRemainingTextureOffset.x = mTotalTextureLengthX%1 - 1;

			if(mRemainingTextureOffset.x > 0 || mRemainingTextureOffset.y > 0){

				newMaterial.mainTextureOffset = new Vector2(mRemainingTextureOffset.x, 0f /*mRemainingTextureOffset.y - newMaterial.mainTextureScale.y*/);
			}

		}else{

			mRemainingTextureOffset.x = (mRemainingTextureOffset.x - newMaterial.mainTextureScale.x%1)%1;
			mRemainingTextureOffset.y = newMaterial.mainTextureScale.y;
		}
	}

	int FindNextFloorTileInRow(int incomingInt){

		int nextTile = incomingInt;

		for(int i = incomingInt; i < mLowestFloors.Length; i++){
			if(mLowestFloors[incomingInt].y == mLowestFloors[i].y){
				nextTile = i;
			}
			else{
				return nextTile;
			}
		}

		return nextTile;
	}	
}
#endif
