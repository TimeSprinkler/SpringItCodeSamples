#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TutorialEditorWindow : EditorWindow{

	public static GameObject mBaseGameObject;

	public static string mSaveDirectory { 
		get { 
			return "Assets/Resources/Prefabs/Tutorial/"; 
		} 
		private set {
			mSaveDirectory = mSaveDirectory;
		}
	}

	public static string mLoadDirectory { 
		get { 
			return "Prefabs/Tutorial/"; 
		} 
		private set {
			mLoadDirectory = mLoadDirectory;
		}
	}
	//I might need to change to track numbers instead
	public static List<TutorialPage> mTutorialPages = new List<TutorialPage>();

	string mPageName;
	Texture mImage;
	string mDescription;
	int mLevelID = 0;
	int mOrderID = -1;


	[MenuItem ("Spring It/Tutorial Window")]
	public static void ShowWindow(){
		EditorWindow.GetWindow (typeof(TutorialEditorWindow));

	}

	void OnGUI(){
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);

		mPageName = EditorGUILayout.TextField ("Page Name", mPageName);
		mDescription = EditorGUILayout.TextField ("Page Text", mDescription);
		mLevelID = EditorGUILayout.IntField ("LeveID", mLevelID);
		mOrderID = EditorGUILayout.IntField ("OrderID", mOrderID);
		mImage = (Texture) EditorGUILayout.ObjectField("Image", mImage, typeof (Texture), false);

		if(GUILayout.Button("Create Page"))
		{
			CanBuildTutorialPage();
		}

		EditorGUILayout.LabelField("Tutorial Name:           mLevelID.mOrderID");

		//If I could create a listbox to display everything and select each tutorial page in order, that would be ideal
		for(int i = 0; i < mTutorialPages.Count;i++){

			EditorGUILayout.SelectableLabel(mTutorialPages[i].mName + ": " + mTutorialPages[i].mLevelID + "." + mTutorialPages[i].mOrderID);
		}

		EditorGUILayout.SelectableLabel("None");


	}

	void OnEnable(){
		EnabledLoadFromPrefs();
	}

	void OnDisable(){
		SaveToEditorPrefs();
	}

	void OnClose(){
		SaveToEditorPrefs ();
	}

	void OnFocus(){
		SaveToEditorPrefs ();
	}

	void OnDestroy(){
		SaveToEditorPrefs ();
	}
	
	void SaveToEditorPrefs(){
		for (int i = 0; i < mTutorialPages.Count; i++) {
			EditorPrefs.SetString(mTutorialPages[i].mName, mTutorialPages[i].mName + ":" + mTutorialPages[i].mLevelID + ":" + mTutorialPages[i].mOrderID + ":" + mTutorialPages[i].mDescription);
		}
	}

	void LoadFromEditorPrefs(){
		for (int i = 0; i < mTutorialPages.Count; i++) {

			LoadOneTutorialPage(mTutorialPages[i].mName);
		}
	}

	void LoadOneTutorialPage(string incomingTutName){

		if(EditorPrefs.HasKey(incomingTutName)){

			/*string[] tempStrings;
			string name;
			int levelID;
			int orderID;
			string description;
			Texture image;
			
			tempStrings = EditorPrefs.GetString(incomingTutName).Split(':');

			
			name = tempStrings[0];
			levelID = int.Parse(tempStrings[1]);
			orderID = int.Parse(tempStrings[2]);
			description = tempStrings[3];
			
			//Get the image off of the prefab that was created
			GameObject tempObject = Resources.Load (mLoadDirectory + name)as GameObject;


			if(tempObject != null){

				Transform textureTransform = tempObject.transform.Find("Image");


				image = textureTransform.GetComponent<UITexture>().mainTexture;
				TutorialPage tempTutPage = new TutorialPage(name,image, description, levelID, orderID);
				mTutorialPages.Add (tempTutPage);

			}*/
		}
		
	}
	
	void EnabledLoadFromPrefs(){
		
		Object[] currentTutorialPrefabs = Resources.LoadAll (mLoadDirectory);

		for (int i = 0; i < currentTutorialPrefabs.Length; i++) {

			LoadOneTutorialPage(currentTutorialPrefabs[i].name);
		}
	}

	void CanBuildTutorialPage(){

		bool canBuild = true;

		if (mPageName == null) {
			Debug.LogError("No PageName for latest Tutorial Page.  A PageName is required");
			canBuild = false;
		}
		if (mDescription == null) {
			Debug.LogError("No Description for latest Tutorial Page.  A Description is required");
			canBuild = false;
		}
		if (mLevelID == -1) {
			Debug.LogError("No Level ID for latest Tutorial Page.  A LevelID is required");
			canBuild = false;
		}
		if (mImage == null) {
			Debug.LogError("No Image for latest Tutorial Page.  An Image is required");
			canBuild = false;
		}

		if (canBuild == true) {
			mOrderID = CorrectOrderID();

			CreateTutorialPageObject();
		}
	}

	void CreateTutorialPageObject(){

		TutorialPage newTutPage = new TutorialPage (mPageName, mImage, mDescription, mLevelID, mOrderID);

		mTutorialPages.Add (newTutPage);
		CreatePrefab (newTutPage);

		ClearUI ();
	}

	public GameObject CreatePrefab(TutorialPage incomingTutorialPage){

		mBaseGameObject = Resources.Load ("Prefabs/Tutorial/TutorialBaseObject") as GameObject;
		if(mBaseGameObject == null){
			Debug.LogError("mBaseObject is null, ensure prefab is in the correct file location: Prefabs/Tutorial/TutorialBaseObject");
		}

		GameObject tutPage = Instantiate(mBaseGameObject,Vector3.zero, Quaternion.identity) as GameObject;
		AssignLabel (tutPage, incomingTutorialPage.mDescription);	
		AssignImage (tutPage, incomingTutorialPage.mImage);

		if (tutPage.GetComponent<TutorialPageInfo> () == null) {
			tutPage.AddComponent<TutorialPageInfo> ();
		}

		tutPage.GetComponent<TutorialPageInfo> ().mLevelID = incomingTutorialPage.mLevelID;
		tutPage.GetComponent<TutorialPageInfo> ().mPageID = incomingTutorialPage.mOrderID;

		PrefabUtility.CreatePrefab(mSaveDirectory + mPageName + ".prefab", tutPage, ReplacePrefabOptions.Default);
		DestroyImmediate(tutPage);

		return tutPage;

	}

	int CorrectOrderID (){

		int tempOrderID = -1;

		if(mOrderID == -1){
			tempOrderID = 0;
		}

		int nextAvailableOrderID = NextAvailableOrderID (tempOrderID);
		mOrderID = nextAvailableOrderID;

		return mOrderID;
	}

	int NextAvailableOrderID(int tempOrderID){

		for (int i = 0; i < mTutorialPages.Count; i++) {
			if(mTutorialPages[i].mLevelID == mLevelID){
				if(mTutorialPages[i].mOrderID == mOrderID || (mTutorialPages[i].mOrderID == 0 && tempOrderID == 0)){
					return mOrderID++;
				}
			}
		}

		if (mOrderID != -1) {
			return mOrderID;
		} else {
			return tempOrderID;
		}
	}

	void AssignLabel(GameObject tutPage, string description){
		
		tutPage.GetComponentInChildren<UILabel>().text = description;
		
	}
	
	void AssignImage(GameObject tutPage, Texture image){
		
		UITexture[] uitextureObjects = tutPage.GetComponentsInChildren<UITexture>();
		
		for(int i = 0; i < uitextureObjects.Length;i++){
			
			if(uitextureObjects[i].gameObject.name == "Image"){
				uitextureObjects[i].mainTexture = image;
			}
			
		}
	}

	void ClearUI(){

		mPageName = "";
		mImage = null;
		mDescription = "";
		mLevelID = 0;
		mOrderID = -1;


	}


}
#endif
