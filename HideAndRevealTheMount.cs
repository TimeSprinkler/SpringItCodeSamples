using UnityEngine;
using System.Collections;

public class HideAndRevealTheMount : MonoBehaviour
{
	private Collider mCurrentCollider;
	[HideInInspector]
	public bool mIsOnFloor = true;

	public void OnTriggerEnter(Collider collider)
	{
		if (collider is BoxCollider) {
			if (collider.gameObject.tag == "LevelPiece") {
				if (collider.gameObject.GetComponent<HideWhenCovered> () != null) {
					mCurrentCollider = collider;
					
					if (gameObject.GetComponent<HideWhenCovered> () != null) {
						gameObject.GetComponent<HideWhenCovered> ().Hide (gameObject);
					}
				}
			}
		}
	}
	
	public void OnTriggerStay(Collider collider)
	{	
		if (collider is BoxCollider) {
			if (collider.gameObject.name == "Floor") {
				this.mIsOnFloor = true;
				if (gameObject.GetComponent<Placeable> () != null) {
					gameObject.GetComponent<Placeable> ().m_IsOnFloor = true;
				}
			}

			if (collider.gameObject.name.Contains ("Wall")) {
				if (gameObject.GetComponent<Placeable> () != null) {
					gameObject.GetComponent<Placeable> ().m_IsOnWall = true;
				}
			}

			if (collider.gameObject.name.Contains ("Conveyor")) {
				if (gameObject.GetComponent<Placeable> () != null) {
					gameObject.GetComponent<Placeable> ().m_IsOnConveyor = true;
				}
			}
		}
	}
	
	public void OnTriggerExit(Collider collider)
	{
		if (collider is BoxCollider) {
			if (collider.gameObject.GetComponent<HideWhenCovered> () != null) {
				collider.GetComponent<HideWhenCovered> ().UnHide (gameObject);

				if (gameObject.GetComponent<HideWhenCovered> () != null) {
					gameObject.GetComponent<HideWhenCovered> ().UnHide (gameObject);
				}

			}
			
			if (collider.gameObject.name == "Floor") {
				this.mIsOnFloor = false;
				if (gameObject.GetComponent<Placeable> () != null) {
					gameObject.GetComponent<Placeable> ().m_IsOnFloor = false;
				}

			}

			if (collider.gameObject.name.Contains ("Wall")) {
				if (gameObject.GetComponent<Placeable> () != null) {
					gameObject.GetComponent<Placeable> ().m_IsOnWall = false;
				}
			}
			if (collider.gameObject.name.Contains ("Conveyor")) {
				if (gameObject.GetComponent<Placeable> () != null) {
					gameObject.GetComponent<Placeable> ().m_IsOnConveyor = false;
				}
			}

			if (mCurrentCollider != null) {
				if (mCurrentCollider.GetInstanceID () == collider.GetInstanceID ()) {
					mCurrentCollider = null;
				}
			}
		}
	}

	public void Attached(bool state)
	{
		if(state)
		{
			EventHandler.OnPlace += DelayedHide;

		}else
		{
			EventHandler.OnPlace -= DelayedHide;
		}
	}

	void OnDestroy()
	{
		EventHandler.OnPlace -=DelayedHide;
	}

	public void DelayedHide()
	{

		if(mCurrentCollider != null && mCurrentCollider.gameObject.GetComponent<HideWhenCovered>() != null)
		{	
			mCurrentCollider.gameObject.GetComponent<HideWhenCovered>().Hide(gameObject);	
		}

		mCurrentCollider = null;
	}
}
