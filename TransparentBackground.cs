#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class TransparentBackground : MonoBehaviour {

	/*public GameObject mTextureWithAlphaMap;
	
	void Start () {
	
		Texture alphaMap = mTextureWithAlphaMap.renderer.material.mainTexture;
		Texture currentTexture = this.renderer.material.mainTexture;

		Vector2 currentScale = this.renderer.material.mainTextureScale;
		Vector2 currentOffset = this.renderer.material.mainTextureOffset;

		Material transparentNewMaterial;

		if(this.gameObject.name == "Background"){
			transparentNewMaterial = new Material(Shader.Find ("Transparent/Cutout/Diffuse"));

			transparentNewMaterial.SetTexture("_MainTex", currentTexture);
			transparentNewMaterial.mainTextureScale = currentScale;
			transparentNewMaterial.mainTextureOffset = currentOffset;

			transparentNewMaterial.SetTexture ("_CutOff", alphaMap);

			gameObject.renderer.sharedMaterial = transparentNewMaterial;

		}

	}*/

}
#endif
