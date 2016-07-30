using UnityEngine;
using System.Collections;

public class VFXContainter : MonoBehaviour {

	//Animations
	[SerializeField] private Animation mRobotInterractionAnimation;
	[SerializeField] private Animation mAmbientAnimation;
	[SerializeField] private Animation mPlayerInterractionAnimation;

	//VFXs
	[SerializeField] private ParticleSystem mRobotInterractionVFX;
	[SerializeField] private ParticleSystem mAmbientVFX;
	[SerializeField] private ParticleSystem mPlayerInterractionVFX;

	private Animation currentAnimation;
	private ParticleSystem currentVFX;
	private bool mAmbientIsPlaying;

	void Start(){
		if(mAmbientAnimation != null){
			PlayAnimation(mAmbientAnimation);
		}

		if(mAmbientVFX != null){
			PlayVFX (mAmbientVFX);
		}
		mAmbientIsPlaying = true;

	}
	
	public void PlayRobotInterraction(float delayTime){ StartCoroutine("PlayRobotInterractionRoutine", delayTime);}
	
	void PlayAnimation(Animation animation){
		if(currentAnimation != null){
			currentAnimation.Stop();
		}

		currentAnimation= animation;
		animation.Play();
	}

	void PlayVFX(ParticleSystem vfx){
		if(currentVFX != null){
			currentVFX.Clear();
		}
	
		currentVFX = vfx;
		vfx.Play();
	}

	public void ToggleAmbient(){

		if(mAmbientIsPlaying){
			StopAmbient();
		}else{
			StartAmbient();
		}

		mAmbientIsPlaying = !mAmbientIsPlaying;
	}

	public void StopAmbient(){
		if(mAmbientAnimation != null){
			mAmbientAnimation.Stop ();
		}
		
		if(mAmbientVFX != null){
			mAmbientVFX.Clear ();
			mAmbientVFX.Stop ();
		}
	}
	
	public void StartAmbient(){
		if(mAmbientAnimation != null){
			mAmbientAnimation.Play ();
		}
		
		if(mAmbientVFX != null){
			mAmbientVFX.Clear ();
			mAmbientVFX.Play ();
		}
	}

	IEnumerator PlayRobotInterractionRoutine(float delayTime){
		if(mRobotInterractionAnimation != null){
			PlayAnimation (mRobotInterractionAnimation);
			yield return new WaitForSeconds(delayTime);
			
			if(mRobotInterractionVFX != null){
				PlayVFX(mRobotInterractionVFX);
			}
		}

		if(mRobotInterractionVFX != null && mRobotInterractionAnimation == null){
			PlayVFX (mRobotInterractionVFX);
		}

	}

	public void PlayPlayerInterraction(float delayTime){ StartCoroutine("PlayPlayerInterractionRoutine", delayTime);}
	
	IEnumerator PlayPlayerInterractionRoutine(float delayTime){
		if(mPlayerInterractionAnimation != null){
			PlayAnimation (mPlayerInterractionAnimation);
			yield return new WaitForSeconds(delayTime);
			if(mAmbientAnimation != null){
				PlayAnimation(mAmbientAnimation);
			}
			
			if(mRobotInterractionVFX != null){
				PlayVFX(mPlayerInterractionVFX);
			}
		}
		
		if(mPlayerInterractionVFX != null && mPlayerInterractionAnimation == null){
			PlayVFX (mPlayerInterractionVFX);
		}
	}
}
