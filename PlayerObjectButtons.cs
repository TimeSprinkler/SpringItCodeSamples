using UnityEngine;
using System.Collections;

public class PlayerObjectButtons : MonoBehaviour {
	
	public GameObject[] mAvailableObjects;
	public bool[] mEnableEachObject;
	public GameObject[] mButtonLocations;
	public Camera mGUICamera;
	public LayerMask mGUIMask;
	public bool mIsOverGUI = true;

	public static Vector3 mDefaultHomePosition = new Vector3(1,1,1);
	private Vector3[] mButtonHomePositions;
	private GameObject mCreatedObject;
	private Vector3 mLastDragPosition;
	private Vector3 mousePosition;
	private CameraMovement mCameraMovement;
		
	void Start () {
		EnableButtons();
	
		mButtonHomePositions = new Vector3[mButtonLocations.Length];
		mCameraMovement = gameObject.GetComponent<CameraMovement>();

		for(int i = 0; i < mButtonHomePositions.Length; i++){
			mButtonHomePositions[i] = mButtonLocations[i].transform.localPosition;
		}

		if (!this.gameObject.name.Contains ("Tutorial")) {

			ToggleButtons (false);
		}
	}

	public void ToggleButtons(bool state){
		for(int i = 0; i <mEnableEachObject.Length; i++){
			mButtonLocations[i].GetComponent<BoxCollider>().enabled = state;
			if(state){
				mButtonLocations[i].GetComponent<UIButton>().state = UIButton.State.Normal;
			}else{
				mButtonLocations[i].GetComponent<UIButton>().state = UIButton.State.Disabled;
			}
			
		}
	}

	void EnableButtons(){

		for(int i = 0; i <mEnableEachObject.Length; i++){
			mButtonLocations[i].SetActive(mEnableEachObject[i]);

		}
	}

	public void OnIconDrag(GameObject button){

		mCameraMovement.ToggleCameraMovment(false);
		Destroy (button.GetComponent<PlaceableButtonHolder>());
		mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, (this.transform.position.z * -1));
		mLastDragPosition = Camera.main.ScreenToWorldPoint(mousePosition);
		mLastDragPosition.z = 0;

	}

	public void ButtonReleased(GameObject button){
		for(int i = 0; i < mAvailableObjects.Length; i++){
			if(mAvailableObjects[i].name + " Button" == button.name){
				mCameraMovement.ToggleCameraMovment(true);
				button.AddComponent<PlaceableButtonHolder>();
				ReturnToHome(button);
				break;
			}
		}
	}

	public void OffUI(GameObject button){
		HideIcon(button);

		for(int i = 0; i < mButtonLocations.Length; i++){
			if(mButtonLocations[i].name == button.name){
				PlaceObject(i);
				break;
			}
		}

	}

	public void OnUI(GameObject button){
		RevealIcon(button);
		gameObject.GetComponent<Placer>().DeleteSelectedPlaceable();
		
	}
		
	public void DeleteObject(string importedName){
		
		int identifier = 5;
		for(int i = 0; i< mAvailableObjects.Length; i++){
			if (importedName == mAvailableObjects[i].name + "(Clone)"){
				identifier = i;	
			}
		}

		gameObject.GetComponent<Budget>().DeleteObject(mAvailableObjects[identifier].name);
	}
	
	void PlaceObject(int identifier){
		mCreatedObject = Instantiate(mAvailableObjects[identifier], mLastDragPosition, Quaternion.identity) as GameObject;
		this.GetComponent<Placer>().AttachPlaceable(mCreatedObject.GetComponent<Placeable>(), true);

		mCreatedObject.GetComponent<Placeable>().m_HomePosition = mDefaultHomePosition;
		mCreatedObject.GetComponent<Placeable>().m_HaveHomePosition = true;
		mCreatedObject.GetComponent<Placeable>().AttachToMouse(UnpackLevel.mLevelScaling);
		this.GetComponent<Budget>().PlaceObject(mCreatedObject.name);
		EventHandler.CallPlaceTool();
		mButtonLocations [identifier].GetComponent<PlaceableButton> ().ChangeActiveState (false);

	}

	void ReturnToHome(GameObject button){
		for(int i = 0; i < mAvailableObjects.Length; i++){
			if(mAvailableObjects[i].name + " Button" == button.name){

				RevealIcon(button);
				button.transform.localPosition = mButtonHomePositions[i];
				break;
			}
		}
	}

	void HideIcon(GameObject button){ChangeIconVisibility(button, false);}
	void RevealIcon(GameObject button){ChangeIconVisibility(button, true);}

	void ChangeIconVisibility(GameObject button, bool state){

		if(button.GetComponentInChildren<UITexture>() != null){
			button.GetComponentInChildren<UITexture>().enabled = state;
		}

		if(button.GetComponent<UISprite>() != null){
			button.GetComponent<UISprite>().enabled = state;
		}

		button.GetComponentInChildren<UILabel>().enabled = state;
	}

}	