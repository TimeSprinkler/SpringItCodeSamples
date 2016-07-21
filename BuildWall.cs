#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildWall : Create2DPanel {
	
	private List<Transform> mWallTransforms = new List<Transform>();
	private List<Vector3> mWallPoints = new List<Vector3>();
	private List<GameObject> mWallPlaneObjects = new List<GameObject>();
	private float mMeshOffset = 0.036f;
	private int mCeilingInt = -99;
	private float mBoxCenterAdjustment = 0.18f;

	private Shader mTileShader;
	private Texture mWallTexture;
	private Object mSideWall;
	private int mPlaceableLayerCheckLayer = 11;

	public void NewWallLocation(Transform incomingTransform){

		mWallTransforms.Add(incomingTransform);
	}

	[ExecuteInEditMode]
	public void FloorWasBuilt(){

		SetWallPoints();

		mTileShader = Shader.Find("Mobile/Diffuse");
		mWallTexture = Resources.Load ("Prefabs/Background/SideWallTexture") as Texture;
		mSideWall = Resources.Load ("Prefabs/Background/SideWall");

		Vector3 startPosition = new Vector3(-1f, CameraMovement.mLevelSize.y + mAdditionalQuadHeight, mZDistance);
		Vector3 endPosition = new Vector3(CameraMovement.mLevelSize.x - 1f, CameraMovement.mLevelSize.y + mAdditionalQuadHeight, mZDistance);

		CreatePanel(startPosition, mWallPoints[0], -2);
		CreatePanel(mWallPoints[mWallPoints.Count-1],endPosition, -1);

		for(int i = 0; i < mWallPoints.Count - 1; i++){

			i = CreatePanel(mWallPoints[i], mWallPoints[i + 1], i);

		}
	
		gameObject.GetComponent<BuildCeiling>().Build(startPosition, endPosition);
	}

	void SetWallPoints(){

		for(int i = 0; i < mWallTransforms.Count; i++){

			Vector3 tempPosition = Vector3.one;
			tempPosition.x = mWallTransforms[i].position.x - mWallTransforms[i].localScale.x/mGlobalQuadScale * mStandardTileSideLength;
			tempPosition.y = mWallTransforms[i].position.y + mWallTransforms[i].localScale.y/mGlobalQuadScale * mStandardTileSideLength;
			tempPosition.z = mZDistance;

			mWallPoints.Add (tempPosition);

			tempPosition.x = mWallTransforms[i].position.x + mWallTransforms[i].localScale.x/mGlobalQuadScale * mStandardTileSideLength;
			tempPosition.y = (mWallTransforms[i].position.y + mWallTransforms[i].localScale.y/mGlobalQuadScale * mStandardTileSideLength);
			tempPosition.z = mZDistance;

			mWallPoints.Add (tempPosition);

		}
	}

	int CreatePanel(Vector3 startPosition, Vector3 endPosition, int i){
		
		GameObject newTile;
		Vector3 tempPosition;

		Vector2 textureScale = new Vector2(1,1);
		string panelType = DeterminePanelType(startPosition, endPosition, i);

		switch(panelType){
					
		default:

			return i;

		case "down": 
			newTile = CreatePlaneObject(180);

			newTile.transform.localPosition = SetTilePosition(startPosition, endPosition);
			newTile.transform.localScale = SetTileScale(startPosition, endPosition);

			tempPosition = new Vector3(newTile.transform.localPosition.x + mMeshOffset, newTile.transform.localPosition.y, newTile.transform.localPosition.z);
			newTile.transform.localPosition = tempPosition;

			i++;
			break;

		case "up":
			newTile = CreatePlaneObject(-180);
		
			newTile.transform.localPosition = SetTilePosition(startPosition, endPosition);
			newTile.transform.localScale = SetTileScale(startPosition, endPosition);
			
			tempPosition = new Vector3(newTile.transform.localPosition.x - mMeshOffset, newTile.transform.localPosition.y, newTile.transform.localPosition.z);
			newTile.transform.localPosition = tempPosition;

			i++;

			break;
	
		}

		AddBoxColliders(newTile);

		newTile.name = "Wall " + i;
		Material tempMaterial = new Material(mTileShader);

		textureScale.x = 1;
		textureScale.y = newTile.transform.localScale.y;



		tempMaterial.mainTexture = mWallTexture;
		tempMaterial.mainTextureScale = textureScale;

		newTile.renderer.material = tempMaterial;

		newTile.layer = mPlaceableLayerCheckLayer;

		newTile.tag = "Wall";

		mWallPlaneObjects.Add (newTile);

		return i;
	}

	string DeterminePanelType(Vector3 startVector, Vector3 endVector, int i){

		if(i == mCeilingInt){
			return "ceiling";
		}

		if(startVector.x == endVector.x){
			if(startVector.y > endVector.y){

				return "down";
			}else{

				return "up";
			}
		}

		return "none";
	}
	
	GameObject CreatePlaneObject(float yRotation){
		
		GameObject tempObject = Instantiate(mSideWall, Vector3.one, Quaternion.identity) as GameObject;

		tempObject.transform.localRotation = Quaternion.identity;
		tempObject.transform.Rotate(0, yRotation, 90 * (180 - yRotation)/180);
		
		return tempObject;
	}
	
	Vector3 SetTilePosition(Vector3 startPosition, Vector3 endPosition){
		
		Vector3 tempPosition = startPosition;
		tempPosition.x = startPosition.x + (endPosition.x - startPosition.x)/2;
		tempPosition.y = startPosition.y + (endPosition.y - startPosition.y)/2;
		tempPosition.z = mZDistance;
		
		return tempPosition;
	}
	
	Vector3 SetTileScale(Vector3 startPosition, Vector3 endPosition){
		
		Vector3 tempScale = new Vector3(1,1,1);
		tempScale.x = mZScale * mMeshScale;
		tempScale.y = Mathf.Abs(startPosition.y - endPosition.y)/(mStandardTileSideLength * mLevelScaling) * mMeshScale;
		
		return tempScale;
	}

	void AddBoxColliders(GameObject newTile){

		//This is done so the collider onthe wall lines up with the flat face of the model
		newTile.AddComponent("BoxCollider");
		Vector3 tempCenter = newTile.GetComponent<BoxCollider>().center;
		tempCenter.x = mBoxCenterAdjustment;
		newTile.GetComponent<BoxCollider>().center = tempCenter;
		
		//Called so the fan can tell where the walls are
		newTile.AddComponent("BoxCollider");
		
		BoxCollider[] boxColliders = newTile.GetComponents<BoxCollider>();
		
		foreach(BoxCollider bc in boxColliders){
			if(bc.center != tempCenter){
				bc.isTrigger = true;
			}
		}

	}
}
#endif