using UnityEngine;
using System.Collections;

public class CustomUIScaling : MonoBehaviour {

	public static float defaultScreenWidth = 1920f;
	public static float defaultScreenHeight = 1080f;

	public bool mIsLockedOnLeft = false;//this is for when being next to the panel is not 0
	public bool mIsLockedOnRight = false;
	public bool mIsLockedOnTop = false;
	public bool mIsLockedOnBottom = false;


	void Awake(){

		//float currentWidgetHeight = gameObject.GetComponent<UISprite>().localSize.y;
		//float currentWidgetWidth = gameObject.GetComponent<UISprite>().localSize.x;

		float heightAspect = Screen.height/defaultScreenHeight;
		float widthAspect = Screen.width/defaultScreenWidth;

		AdjustAnchors(heightAspect, widthAspect);

	}

	void AdjustAnchors(float heightAspect, float widthAspect){

		if(gameObject.GetComponent<UIRect>() != null){

			if(mIsLockedOnLeft && !mIsLockedOnRight){
				UpdateOnlyRightAnchor(widthAspect);
			}

			if(mIsLockedOnRight && !mIsLockedOnLeft){
				UpdateOnlyLeftAnchor(widthAspect);
			}

			if(!mIsLockedOnRight && !mIsLockedOnLeft){
				gameObject.GetComponent<UIRect>().leftAnchor.absolute = (int)(gameObject.GetComponent<UIRect>().leftAnchor.absolute * widthAspect);
				gameObject.GetComponent<UIRect>().rightAnchor.absolute = (int)(gameObject.GetComponent<UIRect>().rightAnchor.absolute * widthAspect);
			}

			if(mIsLockedOnTop && !mIsLockedOnBottom){
				UpdateOnlyBottomAnchor(widthAspect);
			}
			
			if(mIsLockedOnBottom && !mIsLockedOnTop){
				UpdateOnlyTopAnchor(widthAspect);
			}
			
			if(!mIsLockedOnBottom && !mIsLockedOnTop){			
				gameObject.GetComponent<UIRect>().topAnchor.absolute = (int)(gameObject.GetComponent<UIRect>().topAnchor.absolute * heightAspect);
				gameObject.GetComponent<UIRect>().bottomAnchor.absolute = (int)(gameObject.GetComponent<UIRect>().bottomAnchor.absolute * heightAspect);
			}

		}


	}

	void UpdateOnlyLeftAnchor(float aspectRatio){

		gameObject.GetComponent<UIRect>().leftAnchor.absolute = (int)(gameObject.GetComponent<UIRect>().leftAnchor.absolute * aspectRatio) + (int)(gameObject.GetComponent<UIRect>().rightAnchor.absolute * aspectRatio);

	}

	void UpdateOnlyRightAnchor(float aspectRatio){

		gameObject.GetComponent<UIRect>().rightAnchor.absolute = (int)(gameObject.GetComponent<UIRect>().rightAnchor.absolute * aspectRatio) + (int)(gameObject.GetComponent<UIRect>().leftAnchor.absolute * aspectRatio);
	}

	void UpdateOnlyTopAnchor(float aspectRatio){

		gameObject.GetComponent<UIRect>().topAnchor.absolute = (int)(gameObject.GetComponent<UIRect>().topAnchor.absolute * aspectRatio) + (int)(gameObject.GetComponent<UIRect>().bottomAnchor.absolute * aspectRatio);

	}

	void UpdateOnlyBottomAnchor(float aspectRatio){

		gameObject.GetComponent<UIRect>().bottomAnchor.absolute = (int)(gameObject.GetComponent<UIRect>().bottomAnchor.absolute * aspectRatio) + (int)(gameObject.GetComponent<UIRect>().topAnchor.absolute * aspectRatio);

	}
	
}
