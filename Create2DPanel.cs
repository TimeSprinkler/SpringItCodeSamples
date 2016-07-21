#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Create2DPanel : MonoBehaviour {

	[SerializeField] protected float mBackgroundZPosition = 1.24f;
	[SerializeField] protected int mStandardTileSideLength = 5;
	[SerializeField] protected int mLowestYPosition = -13;
	[SerializeField] protected int mTileHeightOffset = 0;
	[SerializeField] protected int mLevelScaling = 2;
	[SerializeField] protected float mShininessValue = 0.4f;
	protected float mGlobalQuadScale = 10f;
	protected float mAdditionalQuadHeight = 1f;
	protected float mZDistance = 0.245f;
	protected float mZScale = 0.25f;
	protected float mMeshScale = 5f;
	protected static Vector3[] mLowestFloors;

	private string mLayerString = "Foreground";
	private int mLayerInt = 12;
	
	public static List<Vector3> mSpawnerLocations = new List<Vector3>();

	void Awake(){
		mLevelScaling = UnpackLevel.mLevelScaling;
	}
		
	public void AddSpawnerLocation(Vector3 newLocation){
		mSpawnerLocations.Add (newLocation);
	}

	public void ClearSpawnerLocations(){

		mSpawnerLocations = new List<Vector3>();
	}

	public virtual GameObject CreatePlaneObject(){
		
		GameObject tempObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		
		tempObject.transform.parent = this.transform;
		tempObject.transform.localRotation = Quaternion.identity;
		tempObject.transform.Rotate(0,0,0);
		tempObject.collider.isTrigger = true;
		tempObject.tag = mLayerString;
		tempObject.layer = mLayerInt;
		
		return tempObject;
	}
}
#endif
