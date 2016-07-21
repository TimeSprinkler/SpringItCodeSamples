using UnityEngine;
using System.Collections;

public class CloseMenuWhenInactive : MonoBehaviour {

	public float mInactiveLimit;

	[SerializeField]
	private GameObject mMainMenuIcon;
	private Vector3 mMousePosition;
	private bool mTouchOnMenu;
	//private float mHoverAwayTime = 0.0f;
	private MainMenuOptionsButton mMainMenuOptionsScript;


	void Start(){

		mMainMenuOptionsScript = mMainMenuIcon.GetComponent<MainMenuOptionsButton>();
	}

	void Opened(){

		//Should reassign this everytime the gameObject is reactived
		mTouchOnMenu = true;
		//mHoverAwayTime = 0.0f;
	}

	void Update(){

		mMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mTouchOnMenu = true;

		if(Input.GetMouseButtonDown(0)||(Input.touches.Length > 0)){

			mTouchOnMenu = CheckMousePosition();

			/*foreach(Touch touch in Input.touches){
				if(!CheckTouchPosition(touch.position)){

					mTouchOnMenu = false;
				}
			}*/
		}
		if(mMainMenuOptionsScript != null){
			if(!mTouchOnMenu && !mMainMenuOptionsScript.mIsLerping){
				StartCoroutine(DelayedClose());
			}
		}else{
			this.GetComponent<OpenOrCloseMenu>().OpenOrClose();
		}
	}

	bool CheckTouchPosition(Vector2 touch2DPosition){

		Vector3 touch3DPosition = Camera.main.ScreenToWorldPoint(touch2DPosition);

		bool isTouchOnMenu = collider.bounds.Contains(touch3DPosition);

		return isTouchOnMenu;
	}

	bool CheckMousePosition(){
	
		bool isTouchOnMenu = collider.bounds.Contains(mMousePosition);

		return isTouchOnMenu;
	}
	
	IEnumerator DelayedClose(){
		yield return new WaitForSeconds(.5f);
		mMainMenuIcon.GetComponent<MainMenuOptionsButton>().CloseMenu();
	}
}
