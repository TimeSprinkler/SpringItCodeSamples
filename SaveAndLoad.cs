using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveAndLoad{
	public static PlayerData mSavedData;
	public static List<LevelData> mLevelData = new List<LevelData>();

	private string mSaveLocation;

	public static void Save(){

				if (PlayerData.instance == null) {
						PlayerData.NewData ();
				}

				if (Application.platform == RuntimePlatform.IPhonePlayer) {
						System.Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
				} 
		
				mSavedData = PlayerData.instance;
				mLevelData = PlayerData.instance.mLevelDataList;
				FileStream playerFile;
				FileStream levelFile;

				BinaryFormatter binaryFormatter = new BinaryFormatter ();
#if UNITY_ANDROID|| UNITY_EDITOR


			playerFile = File.Create(Application.persistentDataPath + "/savedPlayerData.sig");
			binaryFormatter.Serialize(playerFile, mSavedData);
			playerFile.Close ();
			
			levelFile = File.Create (Application.persistentDataPath + "/savedLevelData.sig");
			
			for (int i = 0; i < mLevelData.Count; i++) {
				binaryFormatter.Serialize (levelFile, mLevelData[i]);
			}
			
			levelFile.Close ();

#endif

#if UNITY_IOS
				if (File.Exists (Application.persistentDataPath + "/savedPlayerData.sig")) {
						Debug.Log ("File Exists, recreating player data");

						File.Delete (Application.persistentDataPath + "/savedPlayerData.sig");
						playerFile = File.Create (Application.persistentDataPath + "/savedPlayerData.sig");
						binaryFormatter.Serialize (playerFile, mSavedData);
						playerFile.Close ();
				}

				if(File.Exists(Application.persistentDataPath + "/savedLevelData.sig")){
						Debug.Log ("File Exists, recreating level data");

						File.Delete (Application.persistentDataPath + "/savedLevelData.sig");
						levelFile = File.Create (Application.persistentDataPath + "/savedLevelData.sig");
			
						for (int i = 0; i < mLevelData.Count; i++) {
								binaryFormatter.Serialize (levelFile, mLevelData [i]);
						}
			
						levelFile.Close ();
				} 

				if (!File.Exists (Application.persistentDataPath + "/savedPlayerData.sig")) {
						Debug.Log ("File does not exist, creating player data");

						playerFile = File.Create (Application.persistentDataPath + "/savedPlayerData.sig");
						binaryFormatter.Serialize (playerFile, mSavedData);
						playerFile.Close ();
				}

				if(!File.Exists (Application.persistentDataPath + "/savedLevelData.sig")){
			   			Debug.Log ("File does not exist, creating level data");

						levelFile = File.Create (Application.persistentDataPath + "/savedLevelData.sig");
	
						for (int i = 0; i < mLevelData.Count; i++) {
								binaryFormatter.Serialize (levelFile, mLevelData [i]);
						}

						levelFile.Close ();
						}
#endif
				}

	public static void Load(){
		//Debug.Log (Application.persistentDataPath);
		if(File.Exists(Application.persistentDataPath + "/savedPlayerData.sig")){

			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				System.Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
			} 

			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream playerFile = File.Open (Application.persistentDataPath + "/savedPlayerData.sig", FileMode.Open, FileAccess.Read);

			SaveAndLoad.mSavedData = (PlayerData)binaryFormatter.Deserialize(playerFile);
			PlayerData.instance = mSavedData;
			playerFile.Close ();

		}

		if(File.Exists(Application.persistentDataPath + "/savedLevelData.sig")){

			long position = 0;
			while(true){
				LevelData levelData = ReadLevelFromPath(Application.persistentDataPath + "/savedLevelData.sig", ref position);
				if(levelData == null) break;
				SaveAndLoad.mLevelData.Add (levelData);
			}
			PlayerData.instance.mLevelDataList = mLevelData;
			
			//UnpackLevelDesignerData.instance.LoadDesignerData();
		}
	}

	private static LevelData ReadLevelFromPath(string path, ref long position){

		BinaryFormatter binaryFormatter = new BinaryFormatter();

		LevelData levelData = null;
		using (FileStream levelFile = File.Open (path, FileMode.Open, FileAccess.Read)) {
			if(position < levelFile.Length){
				levelFile.Seek (position, SeekOrigin.Begin);
				levelData = (LevelData)binaryFormatter.Deserialize(levelFile);
				position = levelFile.Position;

			}
		}

		return levelData;

	}

}
