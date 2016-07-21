using UnityEngine;
using System.Collections;

public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public static T instance
	{
		get
		{
			if(_instance == null)
			{
				GenerateInstance();				
			}

			return _instance;
		}
	}

	public static  bool isValid
	{
		get
		{
			return _instance != null;
		}
	}

	private static void GenerateInstance()
	{
		GameObject obj = new GameObject();
		obj.name = "_" + typeof(T).ToString();
		_instance = obj.AddComponent<T>();
	}
}