#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildBackground : Create2DPanel {

	public string mWallNormalName;
	public float mStandardBackgroundSize = 15;
	public Material mBackgroundMaterial;

	private GameObject mBackgroundContainer;
	private Texture mGridmat;
	private GameObject mFloorWallChunk;

	private List<int> mGridLocations = new List<int>();
	private Texture2D mAlphaTexture;
	private int mLevelWidth;
	private GameObject mPlane;//To Store the Texture


	[ExecuteInEditMode]
	public void Build(List<int> dataLocations, int levelWidth) {

		mBackgroundContainer = Resources.Load ("Prefabs/Background/TextureContainer") as GameObject;
		mGridmat = Resources.Load ("Prefabs/Background/Grid(Toggle)") as Texture;
		mLevelWidth = levelWidth;
		CreateGridLocationsArray(dataLocations);
		CreateGridObject ();

	}

	void CreateGridLocationsArray(List<int> dataLocations){
		for(int i = 0; i < dataLocations.Count;i++){
			mGridLocations.Add (dataLocations[i]);
		}
	}

	public void SpawnBlocks(){
	
		Vector3 spawnPosition;
		Vector3 spawnScale;
		int startingLocation = 0;
		int endingLocation;

		int whileLoopBreaker = 0;

		while(!AllSpacesInvalid()){

			startingLocation = FindFirstValidSpace();
			endingLocation = FindSecondValidSpace(startingLocation);

			Vector3 startingPosition = ConvertIntToPosition(startingLocation);
			Vector3 endingPosition = ConvertIntToPosition(endingLocation);

			spawnPosition = SetTilePosition(startingPosition, endingPosition);
			spawnScale = SetTileScale(startingPosition, endingPosition);
					
			SpawnSingleBlock(spawnPosition, spawnScale);
		
			MakeSpacesInvalid(startingLocation, endingLocation);

			whileLoopBreaker++;

			if(whileLoopBreaker > 100){
				Debug.LogError ("Break SpawnBlocks - Windows Probably very complex. Come talk to me - Love Brandon");
				break;
			}

		}

		SpawnInClearWindowPeice();
	}

	void PrintGrid(){

		string printString = " ";

		for(int i = 0; i < mGridLocations.Count;i++){
			printString += mGridLocations[i] + " , ";
		}

		Debug.Log(printString);
	}

	void SpawnSingleBlock(Vector3 spawnPosition, Vector3 spawnScale){

		GameObject textureContainer = Instantiate(mBackgroundContainer, spawnPosition, Quaternion.identity) as GameObject;
		Renderer tempRenderer = textureContainer.GetComponentInChildren<Renderer>();

		GameObject tempObject = tempRenderer.gameObject;

		Vector3 tempPosition = new Vector3(spawnPosition.x, spawnPosition.y, tempObject.transform.position.z);

		tempObject.transform.position = tempPosition;
		tempObject.transform.Rotate(0,0,0);
					
		textureContainer.transform.localScale = spawnScale;

		Material tempMaterial;

		tempMaterial = new Material(Shader.Find ("Mobile/Diffuse"));

		tempMaterial.mainTexture = mBackgroundMaterial.mainTexture;

		tempMaterial.mainTextureScale = new Vector2 ( textureContainer.transform.localScale.x/mStandardBackgroundSize, textureContainer.transform.localScale.y/mStandardBackgroundSize);
		tempMaterial.mainTextureOffset = SetTextureOffset (tempPosition, textureContainer.transform.localScale);

		tempObject.renderer.material = tempMaterial;

	}

	Vector3 ConvertIntToPosition(int incomingInt){

		Vector3 position = Vector3.one;
		position.x = ((incomingInt)%mLevelWidth) * mLevelScaling;
		position.y = mLevelScaling * (mGridLocations.Count/mLevelWidth - incomingInt/mLevelWidth);

		return position;
	}

	Vector3 SetTilePosition(Vector3 startPosition, Vector3 endPosition){
		
		Vector3 tempPosition = startPosition;
		tempPosition.x = ((endPosition.x) - startPosition.x)/2 + startPosition.x;
		tempPosition.y = endPosition.y + (startPosition.y - endPosition.y)/2;
		tempPosition.z = mBackgroundZPosition;
		
		return tempPosition;
	}
	
	Vector3 SetTileScale(Vector3 startPosition, Vector3 endPosition){
		
		Vector3 tempScale = new Vector3(1,1,1);
		tempScale.x = (endPosition.x + 1 - startPosition.x + 1);
		tempScale.y = (startPosition.y + 1 - (endPosition.y - 1));

		return tempScale;
	}

	void CreateAlphaTexture(List<int> dataLocations, int textureSize){

		mAlphaTexture = new Texture2D(textureSize, textureSize,TextureFormat.ARGB32, false);

		for(int i = 0; i < dataLocations.Count;i++){
			if(IsInvalidSpace(dataLocations[i])){

				int x = i%mLevelWidth;
				int y = textureSize - i/mLevelWidth;

				mAlphaTexture.SetPixel(x,y, Color.black);
			}
		}

		mAlphaTexture.Apply();

		mPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);

		Material alphaMat = new Material(Shader.Find ("Transparent/Cutout/Diffuse"));
		alphaMat.mainTexture = mAlphaTexture;
		mPlane.renderer.material = alphaMat;

	}

	bool IsInvalidSpace(int tileValue){

		int[] validSpaces = {0,18,30,33,49, 50, 108, 111, 114, 116, 127, 128, 130, 133, 140, 142, 156, 159, 171, 172, 178, 181, 187, 188, 204, 207, 213, 214, 216, 219, 221, 222, 225, 226, 242, 245, 247, 255, 256, 274, 277, 289};
	
		for(int i = 0; i < validSpaces.Length;i++){
			if(tileValue == validSpaces[i]){
				return false;
			}
		}

		return true;	
	}

	
	int FindFirstValidSpace(){

		for(int i = 0; i < mGridLocations.Count;i++){
			if(!IsInvalidSpace(mGridLocations[i])){
				return i;
			}
		}

		return 0;
	}

	int FindNextInvalidSpaceMinusOne(int startingLocation, int stopWidthPoint){

		for(int i = startingLocation; i <= stopWidthPoint; i++){

			if(IsInvalidSpace(mGridLocations[i])){

				return i-1;
			}		
		}
		
		return (stopWidthPoint);
	}

	int MeasureAreaBetweenSpaces(int startInt, int endInt){

		int height = 1 + endInt/mLevelWidth - startInt/mLevelWidth;
		int width = 1 + endInt%mLevelWidth - startInt%mLevelWidth;

		return height * width;

	}	

	void MakeSpacesInvalid(int startInt, int endInt){

		int heightStart = startInt/mLevelWidth;
		int heightEnd = endInt/mLevelWidth;

		for(int i = heightStart; i <= heightEnd;i++){
			for(int j = startInt%mLevelWidth;j <= endInt%mLevelWidth;j++){
				mGridLocations[i * mLevelWidth + j] = -1;
			}
		}
	}

	bool AllSpacesInvalid(){

		for(int i = 0; i < mGridLocations.Count;i++){
			if(!IsInvalidSpace(mGridLocations[i])){
				return false;
			}
		}
		return true;
	}

	int FindSecondValidSpace(int startingInt){
			
			int stopWidthPoint = (mLevelWidth - 1) + (mLevelWidth * (startingInt/mLevelWidth));
			int endInt = FindNextInvalidSpaceMinusOne(startingInt, stopWidthPoint);
			
			while(true){
				
				if(ValidAcrossARow(startingInt, endInt)){
					startingInt += mLevelWidth;
					endInt += mLevelWidth;
				}else{
					endInt -= mLevelWidth;
					break;
				}

				if(startingInt >= mGridLocations.Count){
					endInt -= mLevelWidth;
					break;
				}

				if(endInt >= mGridLocations.Count){
					endInt -= mLevelWidth;
					endInt = mGridLocations.Count - 1;
				}
			}

		return endInt;
	}


	Vector2 SetTextureOffset(Vector3 position, Vector3 scale){

		Vector2 offset = Vector2.zero;

		Vector3 upperLeftPosition = position;

		//Distance upperleft positon form top left
		Vector3 topLeftPosition = new Vector3 (-1.0f , 1.0f + (mGridLocations.Count / mLevelWidth) * 2f , upperLeftPosition.z);

		Vector2 distanceFromTopLeft = Vector2.zero;
		distanceFromTopLeft.x = Mathf.Abs(topLeftPosition.x - upperLeftPosition.x  + scale.x/2f);
		distanceFromTopLeft.y = topLeftPosition.y - upperLeftPosition.y  + scale.y/2f;

		offset.x = distanceFromTopLeft.x / mStandardBackgroundSize;
		offset.y = distanceFromTopLeft.y / mStandardBackgroundSize;

		return offset;

	}

	bool ValidAcrossARow(int startInt, int endInt){
		
		for(int i = startInt; i <= endInt; i++){
			if(IsInvalidSpace(mGridLocations[i])){
				return false;
			}		
		}
		
		return true;

	}

	void SpawnInClearWindowPeice(){

		GameObject windowClearPanel = Resources.Load ("Prefabs/Background/ClearWindowPanel") as GameObject;
		Vector3 startPosition = ConvertIntToPosition(0);
		Vector3 endPosition = ConvertIntToPosition(mGridLocations.Count - 1);

		Vector3 position = SetTilePosition(startPosition,endPosition);
		position.z = 1.5f;

		GameObject tempObject = Instantiate(windowClearPanel, position, Quaternion.identity) as GameObject;
		tempObject.transform.localScale = SetTileScale(startPosition,endPosition);

	}

	void CreateGridObject(){

		GameObject gridObject = GameObject.CreatePrimitive (PrimitiveType.Quad);

		Vector3 centerPostion = new Vector3((mLevelWidth - 1), (1 + mGridLocations.Count/mLevelWidth), 1.15f); //1.15f is visible z spot, and 40 is the hidden behind the farground spot

		gridObject.transform.position = centerPostion;

		Vector3 scale = Vector3.one;

		scale.x = mLevelWidth * 2;
		scale.y = (mGridLocations.Count / mLevelWidth) * 2;

		gridObject.transform.localScale = scale;

		Material tempMat = new Material (Shader.Find ("Transparent/Diffuse"));

		tempMat.mainTexture = mGridmat;
		tempMat.mainTextureScale = new Vector2 (scale.x/4, scale.y/4);

		gridObject.GetComponent<MeshRenderer> ().material = tempMat;

		gridObject.tag = "Grid";
		gridObject.AddComponent<FadeInGrid> ();

	}
}
#endif
