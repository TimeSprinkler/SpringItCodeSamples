using UnityEngine;
using System.Collections;

public class TimeScaleManager : MonoBehaviour
{
	public float mMinimumScale = 1f;
	public float mMaximumScale = 4f;
	public float mStartingValue = 1f;
	public GameObject mSpeedUpButton;
	public GameObject mSlowDownButton;
	public GameObject mSpeedLabel;

	private BoundedValue mCurrentTimeScale;
	private float mPreviousSpeed;

	private void Awake()
	{
		this.mCurrentTimeScale = new BoundedValue(this.mMinimumScale, this.mMaximumScale);
		this.mCurrentTimeScale.currentValue = this.mStartingValue;

		Time.timeScale = this.mCurrentTimeScale.currentValue;
	}

	private void Update()
	{
		if(this.mCurrentTimeScale.currentValue != 0){
			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.mCurrentTimeScale.IncreaseValue();
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.mCurrentTimeScale.DecreaseValue();
			}
			else
			{
				return;
			}
		}

		mSpeedLabel.GetComponent<UILabel>().text = mCurrentTimeScale.currentValue.ToString();
		Time.timeScale = this.mCurrentTimeScale.currentValue;
	}

	private void UpdateUI()
	{
	
		mSpeedLabel.GetComponent<UILabel>().text = mCurrentTimeScale.currentValue.ToString();
		Time.timeScale = this.mCurrentTimeScale.currentValue;
	}
	
	public void IncreaseSpeed()
	{
		this.mCurrentTimeScale.IncreaseValue();
		UpdateUI();
	}

	public void DecreaseSpeed()
	{
		this.mCurrentTimeScale.DecreaseValue();
		UpdateUI();
	}

	public void PauseOrUnpauseGame(bool pause)
	{
		if(!pause)
		{

			this.mCurrentTimeScale = new BoundedValue(mMinimumScale, mMaximumScale);
			this.GetComponent<CameraMovement>().enabled = true;
			this.mCurrentTimeScale.currentValue = mPreviousSpeed;

		}else
		{

			mPreviousSpeed = this.mCurrentTimeScale.currentValue;
			this.GetComponent<CameraMovement>().enabled = false;

			this.mCurrentTimeScale = new BoundedValue(0, mMaximumScale);
			this.mCurrentTimeScale.currentValue = 0;

		}

		Time.timeScale = this.mCurrentTimeScale.currentValue;

	}

	public void ResetSpeed()
	{
		this.mCurrentTimeScale.currentValue = 1.0f;
		Time.timeScale = this.mCurrentTimeScale.currentValue;

	}

}
