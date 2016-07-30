#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;

public class BuildSurroundingBackground : Create2DPanel {

	private GameObject mLeftSidePanel;
	private GameObject mRightSidePanel;
	private GameObject mTopPanel;
	
	public float mLevelWidth;
	public float mLevelHeight;

	public void SetLevelSize(float levelWidth, float levelHeight){
		mLevelWidth = levelWidth;
		mLevelHeight = levelHeight + mAdditionalQuadHeight * mLevelScaling/2;

	}

	[ExecuteInEditMode]
	public void FloorWasBuilt(){

		CreatePanel("left");
		CreatePanel("right");
		CreatePanel("top");
	}

	void CreatePanel(string panelIdentifier){
		
		GameObject newTile = CreatePlaneObject();
		Vector3 startPosition = Vector3.one;
		Vector3 endPosition = Vector3.one;

		switch(panelIdentifier){

		case "left":

			startPosition.x = -mAdditionalQuadHeight * mLevelScaling * mStandardTileSideLength;
			startPosition.y = mLevelHeight;
			startPosition.z = 0f;

			endPosition.x = 0f;
			endPosition.y = mLowestYPosition;
			endPosition.z = 0f;

			mLeftSidePanel = newTile;
			mLeftSidePanel.name = "LeftSidePanel";
			break;
		
		case "right":

			startPosition.x = mLevelWidth;
			startPosition.y = mLevelHeight;
			startPosition.z = 0f;
			
			endPosition.x = mLevelWidth + mAdditionalQuadHeight * mLevelScaling * mStandardTileSideLength;
			endPosition.y = mLowestYPosition;
			endPosition.z = 0f;
			
			mRightSidePanel = newTile;
			mRightSidePanel.name = "RightSidePanel";
			break;

		case "top":

			startPosition.x = -mAdditionalQuadHeight * mLevelScaling * mStandardTileSideLength;
			startPosition.y = mLevelHeight;
			startPosition.z = 0f;
			
			endPosition.x = mLevelWidth + mAdditionalQuadHeight * mLevelScaling * mStandardTileSideLength;
			endPosition.y = mLevelHeight + mLevelScaling * mStandardTileSideLength;
			endPosition.z = 0f;
			
			mTopPanel = newTile;
			mTopPanel.name = "TopSidePanel";
			break;
		}

		newTile.transform.localPosition = SetTilePosition(startPosition, endPosition);
		
		newTile.transform.localScale = SetTileScale(startPosition, endPosition);
		
		Vector2 textureScale = new Vector2(1,1);
		textureScale.x = newTile.transform.localScale.x/mGlobalQuadScale;
		textureScale.y = newTile.transform.localScale.y/mGlobalQuadScale;

		newTile.renderer.material = SetupShaderAndMaterial(textureScale);
	}
	
	Vector3 SetTilePosition(Vector3 startPosition, Vector3 endPosition){
		
		Vector3 tempPosition = startPosition;
		tempPosition.x = (endPosition.x + startPosition.x)/2 - mLevelScaling/2;
		tempPosition.y = (endPosition.y + startPosition.y)/2;
		tempPosition.z = 0;

		return tempPosition;
	}
	
	Vector3 SetTileScale(Vector3 startPosition, Vector3 endPosition){
		
		Vector3 tempScale = new Vector3(1,1,1);
		tempScale.x = (Mathf.Abs (startPosition.x * mGlobalQuadScale - endPosition.x* mGlobalQuadScale)) /(mStandardTileSideLength * mLevelScaling);
		tempScale.y = Mathf.Abs(startPosition.y* mGlobalQuadScale - endPosition.y* mGlobalQuadScale)/(mStandardTileSideLength * mLevelScaling);
		
		return tempScale;
	}
	
	Material SetupShaderAndMaterial(Vector2 textureScale){


		Texture floorTexture = Resources.Load ("Prefabs/Background/Foreground") as Texture;
		
		Shader tileShader = Shader.Find("Mobile/Diffuse");
		Material newMaterial = new Material(tileShader);
		
		newMaterial.mainTexture = floorTexture;
		newMaterial.mainTextureScale = textureScale;
				
		ChangeTextureOffset(newMaterial);

		return newMaterial;
	}
	
	void ChangeTextureOffset(Material newMaterial){

		Vector2 textureOffset = Vector2.one;

		if(mTopPanel == null){
			if(mRightSidePanel == null){
				textureOffset.x = 0.0f;
				textureOffset.y = 0.0f;
			}else{
				textureOffset.x = newMaterial.mainTextureScale.x;
				textureOffset.y = 0.0f;
			}
		}
		else{

			textureOffset.x = 0f;
			textureOffset.y = mLeftSidePanel.transform.localScale.y + newMaterial.mainTextureOffset.y;

		}

		newMaterial.mainTextureOffset = textureOffset;
	}
	
}
#endif
