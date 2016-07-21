using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsButton : MonoBehaviour {
	
	public RobotScoreLabel mRobotScoreLabel;
	public OpenOrCloseMenu mOutOfRobotsMenu;

#if UNITY_IOS
	[SerializeField] string mIOSGameID = "1042861";
#elif UNITY_ANDROID
	[SerializeField] string mAndroidGameID = "1042860";
#endif
	[SerializeField] bool mEnableTestMode;

	void Awake(){
		
		string gameId = null;
		
		#if UNITY_IOS // If build platform is set to iOS...
		gameId = mIOSGameID;
		#elif UNITY_ANDROID // Else if build platform is set to Android...
		gameId = mAndroidGameID;
		#endif
		
		if (string.IsNullOrEmpty(gameId)) { // Make sure the Game ID is set.
			Debug.LogError("Failed to initialize Unity Ads. Game ID is null or empty.");
		} else if (!Advertisement.isSupported) {
			//Debug.LogWarning("Unable to initialize Unity Ads. Platform not supported.");
		} else if (Advertisement.isInitialized) {
			//Debug.Log("Unity Ads is already initialized.");
		} else {
			//Debug.Log(string.Format("Initialize Unity Ads using Game ID {0} with Test Mode {1}.",
			//gameId, mEnableTestMode ? "enabled" : "disabled"));
			Advertisement.Initialize(gameId, mEnableTestMode);
		}

		if (Advertisement.isSupported) {
			Advertisement.Initialize(gameId);
			//Debug.Log ("Initialize");
		}
	}

	public void ShowAd(){

		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowResult;

		if (Advertisement.isSupported) {
			Advertisement.Show (null, options);
			
		}

	}

	private void HandleShowResult(ShowResult result){

		switch (result)
		{
		case ShowResult.Finished:

			PlayerData.instance.mNumberOfRobots++;
			mRobotScoreLabel.UpdateLabelAmount(1);
			this.transform.parent.transform.GetComponent<UIMenuAnimations>().StartAnimations();
			//this.transform.parent.gameObject.SetActive(false);
			Camera.main.GetComponent<MainCameraGUI>().mMaxRobotsReleased = PlayerData.instance.mNumberOfRobots;
			//mOutOfRobotsMenu.Open();

			break;
		case ShowResult.Skipped:
			Debug.LogWarning ("Video was skipped.");
			break;
		case ShowResult.Failed:
			Debug.LogError ("Video failed to show.");
			break;
		}
	}
}
