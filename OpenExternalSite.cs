using UnityEngine;
using System.Collections;

public class OpenExternalSite : MonoBehaviour {

	public string mWebsiteToOpen;

	void OnClick(){

		Application.OpenURL(mWebsiteToOpen);

	}
}
