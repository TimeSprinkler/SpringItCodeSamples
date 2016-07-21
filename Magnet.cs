using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {

	public float mMagneticStrength;
	public float mMaxSpeed;//Limits the maximum speed the robot can obtain while under the influence of the magnet. This will prevent the robot speed from going to infinity when too close to the magnet.  
	//MaxSpeed will remain constant but is nto labelled that way because of Unity will prevent it from being changed in the inspector

	public Texture mPullTexture;
	public Texture mOffTexture;

	[SerializeField] private Renderer[] mMaterialsToToggle;
	private Vector3 mMagnetForceDirection;
	private int mMagnetismDirectionStatus = -1;
	private float mLimitingDistance = 1.5f; //Higher numbers will make the field strength weaker and reach its max strength sooner. AKA, the robot cannot get as close to the magnet

	void Start(){
		if(mMagnetismDirectionStatus == 0){
			ToggleMaterials(mOffTexture);
		}else{
			ToggleMaterials(mPullTexture);
		}
	}

	void OnTriggerStay(Collider collider){
		
		mMagnetForceDirection = (collider.transform.position - this.transform.position).normalized;
		
		if(collider.gameObject.tag == "Robot"){
			float distance = Vector3.Distance(this.transform.position, collider.transform.position);
			float magneticFieldStrength = CalculateMagneticFieldStrength(distance);

			Vector3 tempVector = collider.rigidbody.velocity + (mMagnetismDirectionStatus * mMagnetForceDirection * magneticFieldStrength);
			if(tempVector.magnitude <= mMaxSpeed){
				collider.attachedRigidbody.AddForce (mMagnetismDirectionStatus * mMagnetForceDirection * magneticFieldStrength);

			}else{
				tempVector =  CalculateReducedMagnetForceToBeAdded(collider, magneticFieldStrength);

				collider.attachedRigidbody.AddForce(tempVector);
			}

		}
	}

	private Vector3 CalculateReducedMagnetForceToBeAdded(Collider collider, float magneticFieldStrength){

		//Maintain vector3's direction and then multiplies it by the reduced magnitude so the collider stagnates at the max speed;	
		//First figure out what is contributing more to the overall speed of the new robot so that that direction portion of the vector will get more say to where the robot will go

		Vector3 vectorsAddedTogether = mMagnetismDirectionStatus * mMagnetForceDirection * magneticFieldStrength + collider.rigidbody.velocity;//The direction the robot is currently going added to the direction the magnet wants the robot to go
		float robotProportion = collider.rigidbody.velocity.magnitude/vectorsAddedTogether.magnitude;//Speed the robot is contributing 
		float magnetProportion = 1f - robotProportion;//Speed Magnet is contributing 

		Vector3 magnetVector = mMagnetismDirectionStatus * mMagnetForceDirection * magneticFieldStrength;
		Vector3 robotVector = collider.transform.position;

		Vector3 reducedDirectionVector =  (robotVector *(robotProportion) + magnetProportion * magnetVector).normalized;
		float reducedMagnitude = mMaxSpeed;

		return reducedMagnitude *reducedDirectionVector;
	}

	public void ToggleDirection(){

		switch(mMagnetismDirectionStatus){

		case -1: mMagnetismDirectionStatus = 0;
			ToggleMaterials(mOffTexture);
			break;

		case 0: mMagnetismDirectionStatus = -1;
			ToggleMaterials(mPullTexture);
			break;

		}
	}

	private void ToggleMaterials(Texture texture){

		for(int i = 0; i < mMaterialsToToggle.Length; i++){
			for(int j = 0; j < mMaterialsToToggle[i].materials.Length;j++){
				mMaterialsToToggle[i].materials[j].mainTexture = texture;
			}
		}
	}

	private float CalculateMagneticFieldStrength(float distance){

		if(distance < mLimitingDistance){
			distance = mLimitingDistance;
		}

		float magnecticFieldStrength = (mMagneticStrength * 1f/Mathf.Pow(distance, 2f));

		return magnecticFieldStrength;
	}
}
