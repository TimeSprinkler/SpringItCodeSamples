using UnityEngine;
using System.Collections;

public class EventHandler : UnitySingleton<EventHandler> {

	/*public delegate void RobotCreated();
	public static event RobotCreated OnRobotCreated;
	public static void CallRobotCreated(){
		if(OnRobotCreated != null){
			OnRobotCreated();
		}
	}*/

	public delegate void RobotDestroyed(GameObject robot);
	public static event RobotDestroyed OnRobotDestroyed;
	public static void CallRobotDestroyed(GameObject robot){
		if(OnRobotDestroyed != null){
			OnRobotDestroyed(robot);
		}
	}

	public delegate void RobotScored(GameObject robot/*, int points*/);
	public static event RobotScored OnRobotScored;
	public static void CallRobotScored(GameObject robot/*, int points*/){
		if(OnRobotScored != null){
			OnRobotScored(robot/*, int points*/);
		}
	}

	public delegate void RobotSpawned();
	public static event RobotSpawned OnRobotSpawn;
	public static void CallRobotSpawned(){
		if(OnRobotSpawn != null){
			OnRobotSpawn();
		}
	}

	public delegate void MoveObject();
	public static event MoveObject OnObjectMove;
	public static void CallMoveObject(){
		if(OnObjectMove != null){
			OnObjectMove();
		}
	}

	public delegate void PlaceTool();
	public static event PlaceTool OnToolPlaced;
	public static void CallPlaceTool(){
		if(OnToolPlaced != null){
			OnToolPlaced();
		}
	}

	public delegate void DeleteTool();
	public static event DeleteTool OnDeleteTool;
	public static void CallDeleteTool(){
		if(OnDeleteTool != null){
			OnDeleteTool();
		}
	}

	public delegate void PlaceObject();
	public static event PlaceObject OnPlace;
	public static void CallPlace(){
		if(OnPlace != null){
			OnPlace();
		}
	}

	public delegate void PlaceFinished(Transform transform);
	public static event PlaceFinished OnPlaceFinish;
	public static void CallPlaceFinish(Transform transform){
		if(OnPlaceFinish != null){
			OnPlaceFinish(transform);
		}
	}

	public delegate void ToggleMusicVolume(bool state);
	public static event ToggleMusicVolume OnMusicToggle;
	public static void CallMusicToggled(bool state){
		if(OnMusicToggle != null){
			OnMusicToggle(state);
		}
	}

	public delegate void ToggleSFXVolume(bool state);
	public static event ToggleSFXVolume OnSFXToggle;
	public static void CallSFXToggled(bool state){
		if(OnSFXToggle != null){
			OnSFXToggle(state);
		}
	}

	public delegate void LevelComplete();
	public static LevelComplete OnLevelCompleted;
	public static void CallLevelCompleted(){
		if(OnLevelCompleted != null){
			OnLevelCompleted();
		}
	}

	public delegate void NewLevelLoaded(int id);
	public static NewLevelLoaded OnNewLevel;
	public static void CallNewLevelLoaded(int id){
		if(OnNewLevel != null){
			Currentlevel.instance.NewLevel(id);
		}
	}

	public delegate void LevelSelectLoaded();
	public static LevelSelectLoaded OnLevelSelectLoaded;
	public static void CallLevelSelectLoaded(){
		if(OnLevelSelectLoaded != null){
			OnLevelSelectLoaded();
		}
	}

	public delegate void SliderDragEnd();
	public static SliderDragEnd OnSliderDragEnd;
	public static void CallSliderDragEnd(){
		if(OnSliderDragEnd != null){
			OnSliderDragEnd();
		}
	}

	public delegate void ClickOffGUI();
	public static ClickOffGUI OffGUIClick;
	public static void CallClickOffGUI(){
		if(OffGUIClick != null){
			OffGUIClick();
		}
	}

	public delegate void PlaceableDeleted(Transform transform);
	public static PlaceableDeleted OnPlaceableDeleted;
	public static void CallPlaceableDeleted(Transform transform){
		if (OnPlaceableDeleted != null) {
			OnPlaceableDeleted(transform);
		}
	}
}
