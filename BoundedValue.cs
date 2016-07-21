using UnityEngine;

public class BoundedValue 
{
	private float _currentValue;
	public float currentValue
	{
		get
		{
			return this._currentValue;
		}

		set
		{
			this._currentValue = Mathf.Clamp(value, this.mMinimumValue, this.mMaximumValue);
		}
	}

	private float mMinimumValue;
	private float mMaximumValue;

	public BoundedValue(float min, float max)
	{
		this.mMinimumValue = min;
		this.mMaximumValue = max;
	}

	public void IncreaseValue()
	{
		this._currentValue = Mathf.Min(this._currentValue * 2, this.mMaximumValue);
	}

	public void DecreaseValue()
	{
		this._currentValue = Mathf.Max(this._currentValue / 2, this.mMinimumValue);
	}
}
