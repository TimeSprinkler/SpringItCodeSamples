using UnityEngine;
using System.Collections;

public class Placeable : MonoBehaviour 
{
	public float m_RotationDegrees;
	public AudioSource mConstructionSoundEffect;
	[HideInInspector]
	public bool m_HasBeenChargedForMount = false;
	[HideInInspector]
	public bool m_IsOnFloor = false;
	[HideInInspector]
	public bool m_IsOnWall = false;
	[HideInInspector]
	public bool m_IsOnConveyor = false;

	[HideInInspector]
	public Vector3 m_HomePosition;
	[HideInInspector]
	public bool m_HaveHomePosition = false;
	public static float m_PlacementDelayTime = 0.5f;

	public bool m_ReadIsAttachedToMouse{ get{ return m_IsAttachedToMouse;}}

	private float m_SnapSize;			
	private bool m_IsAttachedToMouse = false;
	private Camera m_Camera;
	private Transform m_CameraTransform;
	private Transform m_Transform;
	private float m_TimeSinceCreation = 0.0f;
	private bool m_BeingPlaced = false;

	private ValidPlaceablePosition m_ValidPlaceablePositionInstance;
	private PlaceableMovementTransitionMode m_PlaceableMovementTransitionModeInstance;

	void Awake () 
	{
		m_Camera = Camera.main;
		m_CameraTransform = m_Camera.transform;
		m_Transform = transform;
		m_ValidPlaceablePositionInstance = gameObject.GetComponent<ValidPlaceablePosition>();
		m_PlaceableMovementTransitionModeInstance = gameObject.GetComponent<PlaceableMovementTransitionMode>();

	}

	void Start(){
		EventHandler.OnSFXToggle += UpdateSFXVolume;
	}

	void OnDestroy(){
		EventHandler.OnSFXToggle -= UpdateSFXVolume;
		EventHandler.CallPlaceableDeleted (this.transform);
		StopAllCoroutines();
	}
	
	void Update () 
	{	
		m_TimeSinceCreation += Time.deltaTime;
		
		if(m_IsAttachedToMouse || m_TimeSinceCreation < 0.1f) 
		{	
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = m_Transform.position.z - m_CameraTransform.position.z;
			mousePosition = m_Camera.ScreenToWorldPoint(mousePosition);

			gameObject.GetComponent<PlaceableMovementTransitionMode>().MakeTransparent();

			if(!m_ValidPlaceablePositionInstance.IsValidPosition())
			{
				m_PlaceableMovementTransitionModeInstance.ChangeColor();
			}

			m_Transform.position = this.CalculateNewPosition(mousePosition);	
		}
	}
	
	public void AttachToMouse(float snapSize) 
	{
		m_SnapSize = snapSize;
		m_IsAttachedToMouse = true;
		ToggleMechanicsScript(true);

		if (this.GetComponent<SphereCollider> () != null) {
			this.GetComponent<SphereCollider> ().enabled = false;
		}

		if(!m_HaveHomePosition)
		{
			m_HomePosition = this.transform.position;

			m_HaveHomePosition = true;
		}
	}

	private void ToggleMechanicsScript(bool state)
	{
		if(gameObject.GetComponent<HideAndRevealTheMount>() != null)
		{
			gameObject.GetComponent<HideAndRevealTheMount>().Attached(state);
		}

	}

	public void Place(Vector3 mouseLocation)
	{		
		m_IsAttachedToMouse = false;

		if(gameObject.GetComponent<ValidPlaceablePosition>().mIsValidPosition)
		{

		}
		else
		{
			ReturnToHomePosition();

		}

		StartCoroutine(DelayPlacement());	
		m_HaveHomePosition = false;
	}
		
	private Vector3 CalculateNewPosition(Vector2 mousePosition)
	{
		if(this.m_SnapSize == 0)
		{
			return new Vector3(mousePosition.x, mousePosition.y, m_Transform.position.z);
		}

		Vector2 remainder = new Vector2(mousePosition.x % m_SnapSize, mousePosition.y % m_SnapSize);
		Vector2 offset = new Vector2(0, 0);
		
		if(remainder.x >= m_SnapSize / 2)
		{
			offset.x = m_SnapSize;
		}
		
		if(remainder.y >= m_SnapSize / 2)
		{
			offset.y = m_SnapSize;
		}
		
		Vector3 newPosition = m_Transform.position;
		newPosition.x = mousePosition.x - remainder.x + offset.x;
		newPosition.y = mousePosition.y - remainder.y + offset.y;
		
		return newPosition;
	}

	public void RotateCounterClockwise()
	{
		if(m_RotationDegrees == 180)
		{		
			transform.Rotate (0, m_RotationDegrees, 0);		
		}
		else
		{
			transform.Rotate(0,0, m_RotationDegrees);
		} 
	}
	
	public float TimeSinceCreation()
	{
		return m_TimeSinceCreation;	
	}

	void ReturnToHomePosition()
	{
		if( m_HomePosition == PlayerObjectButtons.mDefaultHomePosition)
		{
			m_Camera.GetComponent<PlayerObjectButtons>().DeleteObject(gameObject.name);
			GameObject.Destroy(gameObject);
			EventHandler.CallDeleteTool();
			
		}
		else
		{
			gameObject.transform.position = m_HomePosition;
			m_HaveHomePosition = false;
			m_PlaceableMovementTransitionModeInstance.MakeOpaque();
			m_BeingPlaced = false;
			EventHandler.CallPlace();
			ToggleMechanicsScript(false);
		}
	}

	void OnTriggerStay(Collider collider)
	{
		if (collider is BoxCollider) {
			if (m_BeingPlaced) {
				if (collider.tag == "Robot") {
					Destroy (collider.gameObject);
				}
			}
		}
	}

	IEnumerator DelayPlacement()
	{

		m_BeingPlaced = true;
		EventHandler.CallPlace();

		yield return new WaitForSeconds(m_PlacementDelayTime);
		//mConstructionSoundEffect.Play ();
		if(gameObject.GetComponent<ValidPlaceablePosition>().mIsValidPosition){

			m_PlaceableMovementTransitionModeInstance.MakeOpaque();
			m_BeingPlaced = false;

			ToggleMechanicsScript(false);
		}else{

			ReturnToHomePosition();
		}
	
		if (this.GetComponent<SphereCollider> () != null) {
			this.GetComponent<SphereCollider> ().enabled = true;
		}

		EventHandler.CallPlaceFinish (this.transform);


	}

	void UpdateSFXVolume(bool state){
		mConstructionSoundEffect.mute = !state;
	}
}
