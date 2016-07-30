#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {
	
	public delegate void LevelLoadedCallback();
	
	public const string GameScene = "GameScene";
	
	public int lastLoadedLevelIndex { get; private set; }
	
	private List<string> allLevels; 
	
	public static LevelLoader instance
	{
		get
		{
			if(LevelLoader.internalInstance == null)
			{
				GameObject obj = new GameObject();
				GameObject.DontDestroyOnLoad(obj);
				obj.name = "levelLoader";
				LevelLoader.internalInstance = obj.AddComponent<LevelLoader>();
			}
			
			return LevelLoader.internalInstance;
		}
	}
	
	private static LevelLoader internalInstance;
	
	private void Awake() { 
		this.allLevels = new List<string>(); 
		this.lastLoadedLevelIndex = 0;
	}
	
	public void AddLevel(string levelName) {
		this.allLevels.Add(levelName);
	}
	
	public bool LoadLevel(int levelIndex) {
		if(this.allLevels.Count > levelIndex) {
			return this.LoadLevel(this.allLevels[levelIndex]);
		}
		
		return false;
	}
	
	public bool LoadLevel(string levelName)  {
		int levelIndex = this.allLevels.IndexOf(levelName);
		if(levelIndex >= 0) {
			this.lastLoadedLevelIndex = levelIndex;

			//EventHandler.CallNewLevelLoaded();

			this.StartCoroutine(this.DoLoad(Resources.Load<TextAsset>(levelName)));
			return true;
		}
		
		return false;
	}
	
	public IEnumerator DoLoad(TextAsset level) {
		Application.LoadLevel(LevelLoader.GameScene);

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		
		GameObject levelUnpacker = new GameObject();
		UnpackLevel unpacker = levelUnpacker.AddComponent<UnpackLevel>();
		levelUnpacker.AddComponent<Create2DPanel>();
		unpacker.Unpack(level, lastLoadedLevelIndex);

		GameObject.Destroy(unpacker);
	}

	[ExecuteInEditMode]
	public void DoPreLoad(TextAsset level, int currentLevel){

		GameObject levelUnpacker = new GameObject();
		UnpackLevel unpacker = levelUnpacker.AddComponent<UnpackLevel>();
		levelUnpacker.AddComponent<Create2DPanel>();
		unpacker.Unpack(level, currentLevel);

		unpacker.gameObject.SetActive(false);

	}
}
#endif
