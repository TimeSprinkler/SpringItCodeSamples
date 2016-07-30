using UnityEngine;
using System.Collections;

public class PlaceableMovementTransitionMode : MonoBehaviour {

	public GameObject[] mAllChildrenWithMaterials;
	public GameObject[] mAllMovementColliders;
	public Color mInvalidSpotColor;
	public Color mTransparentColor;
	public string mNormalShaderName;

	public GameObject mPlaceableGlowObject;
	public Color mGlowColor;

	private Shader mTransparentShader;
	private Shader mNormalShader;

	private float mFlashTime = 0.25f;
	
	void Awake(){
		mTransparentShader = Shader.Find ("Transparent/VertexLit");
		mNormalShader = Shader.Find (mNormalShaderName);
	}

	public void MakeTransparent(){ChangeShader(mTransparentShader, false);}
	public void MakeOpaque(){ChangeShader(mNormalShader, true);}

	void ChangeShader(Shader incomingShader, bool state){
		for(int j = 0; j <mAllChildrenWithMaterials.Length;j++){
			for(int i = 0; i < mAllChildrenWithMaterials[j].renderer.materials.Length; i++){
				mAllChildrenWithMaterials[j].renderer.materials[i].shader = incomingShader;
				mAllChildrenWithMaterials[j].renderer.materials[i].color = mTransparentColor;
				ToggleColliders(state);
			}
		}

		//state here is false for transparent meaning the glow shouldbe revealed

		if (mPlaceableGlowObject != null) {
			mPlaceableGlowObject.SetActive (!state);
			if (mPlaceableGlowObject.activeSelf) {
				mPlaceableGlowObject.renderer.material.color = mGlowColor;
			}
		}
	}

	public void ChangeColor(){
		for(int j = 0; j <mAllChildrenWithMaterials.Length;j++){
			for(int i = 0; i < mAllChildrenWithMaterials[j].renderer.materials.Length; i++){
				mAllChildrenWithMaterials[j].renderer.materials[i].color = mInvalidSpotColor;
			}
		}
		if (mPlaceableGlowObject != null) {
			if (mPlaceableGlowObject.activeSelf) {
				mPlaceableGlowObject.renderer.material.color = mInvalidSpotColor;
			}
		}
	}

	void ToggleColliders(bool colliderState){

		for(int i = 0; i < mAllMovementColliders.Length; i++){
			mAllMovementColliders[i].collider.enabled = colliderState;
		}
	}

	public void Flash(){

		MakeTransparent();
		ChangeColor();
		StartCoroutine("Wait");
		ChangeColor();
		MakeOpaque();
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds(mFlashTime);
	}
	
}
