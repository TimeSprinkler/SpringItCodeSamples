using UnityEngine;
using System.Collections;

public class LinkToMusicButton : MonoBehaviour {

	public string mStoreListingToOpen = "market://details?id=album-Boqnka5pdmqsxi5tjsit4fqduhu";

#if UNITY_ANDROID
	public void OpenInGooglePlay(){

		Application.OpenURL(mStoreListingToOpen);

		//"market://details?id=com.FreebordGame.FreebordTheGame"
	}

#endif



}
