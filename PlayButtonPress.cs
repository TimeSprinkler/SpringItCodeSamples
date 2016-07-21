using UnityEngine;
using System.Collections;

public class PlayButtonPress : MonoBehaviour {

	//Creates the button press effect 
	public void ChangeShader(){
		this.renderer.material.shader = Shader.Find ("Unlit/Transparent Cutout");
	}
}
