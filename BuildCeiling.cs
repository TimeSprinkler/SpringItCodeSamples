#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class BuildCeiling : Create2DPanel {

	private int mPlaceableLayerCheckLayer = 11;
	private float mLengthOfMeshEnd = 0.1f;
	private float mOffsetOfMeshInYDirection = -0.03f;

	public void Build(Vector3 start, Vector3 end){

		GameObject tempObject = CreatePlaneObject();
		tempObject.transform.Rotate(0,0,90);

		tempObject.transform.position = SetTilePosition(start, end);
		tempObject.transform.localScale = SetTileScale(start, end);

		Shader tileShader = Shader.Find("Mobile/Diffuse");

		Material tempMaterial = new Material(tileShader);
				
		Texture wallTexture = Resources.Load ("Prefabs/Background/SideWallTexture") as Texture;
		
		tempMaterial.mainTexture = wallTexture;
		tempMaterial.mainTextureScale = new Vector2( 1, tempObject.transform.localScale.y);

		tempObject.renderer.material = tempMaterial;

		tempObject.tag = "Wall";
	}

	public override GameObject CreatePlaneObject(){
		
		GameObject tempObject = Instantiate(Resources.Load ("Prefabs/Background/SideWall"), Vector3.one, Quaternion.identity) as GameObject;
		
		tempObject.transform.localRotation = Quaternion.identity;
		tempObject.transform.forward = Vector3.back;

		tempObject.AddComponent("MeshCollider");
		tempObject.AddComponent("BoxCollider");

		tempObject.layer = mPlaceableLayerCheckLayer;
		
		return tempObject;
	}

	Vector3 SetTilePosition(Vector3 startPosition, Vector3 endPosition){
		
		Vector3 tempPosition = startPosition;
		tempPosition.x = (endPosition.x - mLevelScaling/2)/2;
		tempPosition.y = CameraMovement.mLevelSize.y + mAdditionalQuadHeight + mOffsetOfMeshInYDirection;
		tempPosition.z = mZDistance;
		
		return tempPosition;
	}
	
	Vector3 SetTileScale(Vector3 startPosition, Vector3 endPosition){
		
		Vector3 tempScale = new Vector3(1,1,1);
		tempScale.x = mZScale * mMeshScale;
		tempScale.y = Mathf.Abs(startPosition.x - endPosition.x)/(mStandardTileSideLength * mLevelScaling) * mMeshScale + 2 * mLengthOfMeshEnd;
		
		return tempScale;
	}
	
}
#endif