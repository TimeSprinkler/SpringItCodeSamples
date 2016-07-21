using UnityEngine;
using System.Collections;

public class Placer : MonoBehaviour 
{
	public LayerMask mLayersToHit;

	private float m_SnapSize;
	private Placeable m_CurrentObject;
	private Toggle3DButton m_SelectedObject;
	private Camera m_Camera;
	private GameObject m_PlayModeCamera;
	private GameObject m_GridObject;

	//TO prevent people form quickly dragging things into the game and breaking it
	public float m_MouseHoldLimit = 0.2f;

	public bool ObjectAttached { get { return m_CurrentObject != null; } }
	public bool ObjectSelected { get { return m_SelectedObject != null; } }

	void Start () 
	{
		m_SnapSize = UnpackLevel.mLevelScaling;
		m_Camera = Camera.main;
		m_PlayModeCamera = GameObject.FindGameObjectWithTag("PlaymodeCamera");
		m_GridObject = GameObject.FindGameObjectWithTag ("Grid");

	}
	
	void Update () 
	{	
		if(Input.GetMouseButtonDown(0)) 
		{			
			Ray screenRay = m_Camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(screenRay, out hit, Mathf.Infinity, mLayersToHit))
			{
				if(hit.collider.tag != "MovingPart")
				{
					Transform curTransform = hit.transform;
					do
					{
						AttachPlaceable(curTransform.GetComponent<Placeable>(), false);
						SelectObject(curTransform);
						curTransform = curTransform.parent;

					}
					while(curTransform != null && !ObjectAttached);

				}

				if(hit.collider.tag == "TrashCan")
				{
					hit.collider.GetComponent<TrashCanDelete>().DeleteObject();
				}
			}
		}

		CheckForMouseRelease();
		CheckForTrashCan();
	}
	
	public void DeleteSelectedPlaceable()
	{
		if(ObjectAttached)
		{
			ToggleGridObject (false);
			this.GetComponent<PlayerObjectButtons>().DeleteObject(m_CurrentObject.name);
			Destroy(m_CurrentObject.gameObject);
			m_CurrentObject = null;
			EventHandler.CallDeleteTool();
		}
	}

	public void CheckForMouseRelease()
	{
		if(Input.GetMouseButtonUp(0))
		{
			ToggleGridObject (false);
			if(ObjectAttached)
			{
				if(m_CurrentObject.m_ReadIsAttachedToMouse)
				{
					if(!CheckForTrashCan())
					{
						DetachCurrentObject();
					}
					else
					{
						DeleteSelectedPlaceable();
					}
				} 
				else if(ObjectSelected)
				{
					StopCoroutine("DelayAttach");
					m_SelectedObject.Toggle();	
				}
			}
			else 
			{
				if(ObjectSelected)
				{
					StopCoroutine("DelayAttach");
					m_SelectedObject.Toggle();
				}
			}


			m_SelectedObject = null;
			m_CurrentObject = null;
		}


	}
	
	public void AttachPlaceable(Placeable newPlaceable, bool spawnedFromMenu) 
	{
		if(newPlaceable == null)
		{
			ToggleGridObject (false);
			return;
		}

		m_CurrentObject = newPlaceable;
		
		if(spawnedFromMenu)
		{
			ToggleGridObject (true);
			m_CurrentObject.AttachToMouse(m_SnapSize);

		} 
		else 
		{
			if(m_CurrentObject.GetComponentInChildren<Cannon>() != null)
			{
				if(m_CurrentObject.GetComponentInChildren<Cannon>().mStoredRobots > 0)
				{
					m_CurrentObject.GetComponentInChildren<PlaceableMovementTransitionMode>().Flash();

				}else
				{
					StartCoroutine("DelayAttach");

				}
			}else
			{
				StartCoroutine("DelayAttach");
			}
		}
	}

	IEnumerator DelayAttach()
	{

		yield return new WaitForSeconds(m_MouseHoldLimit);

		ToggleGridObject (true);
		if(m_CurrentObject != null)
		{
			m_CurrentObject.AttachToMouse(m_SnapSize);
		}
	}

	public void SelectObject(Transform transform)
	{
		if(transform == null)
		{
			ToggleGridObject (false);
			return;
		}

		if((transform.GetComponent<Toggle3DButton>() != null))
		{
			m_SelectedObject = transform.GetComponent<Toggle3DButton>();
		}
		else if(transform.GetComponentInChildren<Toggle3DButton>() != null)
		{
			m_SelectedObject = transform.GetComponentInChildren<Toggle3DButton>();
		}
		else 
		{
			ToggleGridObject (false);
			m_SelectedObject = null;
		}
	}
	
	public void DetachCurrentObject() 
	{
		m_SelectedObject = null;


		if(!ObjectAttached)
			return;
		
		Vector3 mouseLocation = m_Camera.ScreenToWorldPoint(Input.mousePosition);


		if(gameObject.GetComponent<Budget>().AbleToPlace(m_CurrentObject.name))
		{
			m_CurrentObject.Place(mouseLocation);
			EventHandler.CallMoveObject();
		}else{
			DeleteSelectedPlaceable();
		}

		m_CurrentObject = null;

	}

	bool CheckForTrashCan()
	{
		if(this.m_PlayModeCamera.camera == null)
		{
			return false;
		}

		Ray screenRay = m_PlayModeCamera.camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(screenRay, out hit))
		{
			return (hit.collider.tag == "TrashCan");
				
		}
		
		return false;
	}

	void ToggleGridObject(bool state)
	{
		m_GridObject.GetComponent<FadeInGrid> ().StartFade(state);
	}

}
