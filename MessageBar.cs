using UnityEngine;
using System.Collections;

public class MessageBar : MonoBehaviour {

	public int mTextLimit = 32;
	public float mMessageDelayTime = 0.5f;
	public UILabel mInfoTextLabel;
	public UITexture mInfoTexture;

	private string mCurrentMessage;
	private string mDisplayString = " ";
	private bool mLevelHasMessage;
	private int mStringCounter = 0;

	public void NewMessage(string newMessage){

		mCurrentMessage = newMessage;
		mLevelHasMessage = true;
		HideInfoBar(false);

		if(mCurrentMessage == ""){
			HideInfoBar(true);
			mLevelHasMessage = false;
		}

		StartCoroutine("CycleMessage");

	}

	IEnumerator CycleMessage(){

		while(mLevelHasMessage){

			string tempString;

			for(int i = 0; mStringCounter < mCurrentMessage.Length; i++){

				tempString = mCurrentMessage.Split(' ')[i] + " ";

				mDisplayString += tempString;

				mInfoTextLabel.text = mDisplayString;

				yield return new WaitForSeconds(mMessageDelayTime);
				mStringCounter += tempString.Length;
			}

			yield return new WaitForSeconds(mMessageDelayTime * 3);
			mStringCounter = 0;
			mDisplayString = "";

		}
	}

	void HideInfoBar(bool state){

		mInfoTexture.enabled = !state;
	}
}
