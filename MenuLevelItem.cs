using UnityEngine;
using System.Collections;

public class MenuLevelItem : MonoBehaviour {

	public int mLevelLoadInt;
	public bool mIsUnlocked = false;
	public GameObject[] mBoltSprites;

	private string mLevelIconGlowSprite = "LevelIconGlow";
	private string mLevelIconSprite = "LevelIcon";
	private string mGoldBoltSpriteName = "Bolt-Gold";

	void Awake(){

		SetLabelNumber ();

		CheckIfLocked ();
		LockOrUnlockLevel ();
	}

	void Start () {

		UIEventListener.Get(this.gameObject).onClick += this.SelectObject;
		EventHandler.OnSliderDragEnd += RedrawPanel;
		LevelSelectEventHandler.OnLevelIconSelected += this.AnIconIsSelected;

		UpdateStars();
		//CheckIfCompleted();
	}

	void OnDestroy(){
		EventHandler.OnSliderDragEnd -= RedrawPanel;
		UIEventListener.Get(gameObject).onClick -= this.SelectObject;
		EventHandler.OnSliderDragEnd += RedrawPanel;
		LevelSelectEventHandler.OnLevelIconSelected -= this.AnIconIsSelected;
	}

	private void SelectObject(GameObject incomingObject){

		if (incomingObject = this.gameObject) {
			incomingObject.GetComponent<UISprite> ().spriteName = mLevelIconGlowSprite;
			LevelSelectEventHandler.CallLevelIconSelected (incomingObject);
		}

	}

	private void AnIconIsSelected(GameObject incomingGameObject){
		if (this.gameObject != incomingGameObject) {
			gameObject.GetComponent<UISprite> ().spriteName = mLevelIconSprite;
		}
	}

	public void LoadLevel() {

		if(Application.CanStreamedLevelBeLoaded(mLevelLoadInt.ToString() + " SpringItLevel")){
			EventHandler.CallNewLevelLoaded(mLevelLoadInt);
			Application.LoadLevel(mLevelLoadInt.ToString() + " SpringItLevel");
		}else{
			Debug.LogWarning("No Level to load for " + mLevelLoadInt);
		}
	}

	public void ChangeStarLevel(int numberOfStars){

		for (int i = 0; i < numberOfStars && i < 3; i++) {
			mBoltSprites[i].GetComponent<UISprite>().spriteName = mGoldBoltSpriteName;
		}
	}

	private void UpdateStars(){
		int stars = PlayerData.instance.mLevelDataList[mLevelLoadInt].mNumberOfStars;
		ChangeStarLevel(stars);

	}

	private void CheckIfCompleted(){
		if(PlayerData.instance.mLevelDataList[mLevelLoadInt - 1].mIsComplete){

		}
	}

	void RedrawPanel(){
		gameObject.GetComponentInChildren<UIPanel>().SetDirty();
	}

	private void CheckIfLocked(){

		//I need a current player state that tracks their progress  Do somethign with mLevel

		//Debug.Log ("Count: " + PlayerData.instance.mLevelDataList.Count + " Level: " + mLevelLoadInt);

		if (PlayerData.instance.mLevelDataList [mLevelLoadInt - 1] != null) {
			if (PlayerData.instance.mLevelDataList [mLevelLoadInt - 1].mIsComplete) {
				mIsUnlocked = true;
			} else {
				mIsUnlocked = false;
			}
		}

		if (mLevelLoadInt == 1) {
			mIsUnlocked = true;
		}

	}

	private void LockOrUnlockLevel(){

		if (mIsUnlocked) {
			//Swap Texture to Sprite and find the appropriate disabled Sprite
			Color newColor = Color.white;
			
			gameObject.GetComponentInChildren<UISprite> ().color = newColor;
			this.gameObject.GetComponent<BoxCollider> ().enabled = true;
		} else {
		
			
			Color newColor = new Color (0.686f, 0.686f, 0.686f);
			
			gameObject.GetComponentInChildren<UISprite> ().color = newColor;
			this.gameObject.GetComponent<BoxCollider> ().enabled = false;
		}
	}

	private void SetLabelNumber(){

		this.gameObject.GetComponentInChildren<UILabel> ().text = mLevelLoadInt.ToString ();
	}

}