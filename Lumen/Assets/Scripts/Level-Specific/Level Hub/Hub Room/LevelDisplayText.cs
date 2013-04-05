using UnityEngine;
using System.Collections;

public class LevelDisplayText : FadeText {
	int levelToDisplay;
	LevelData levelData;
	
	
	protected override void OnEnable() {
		levelToDisplay = transform.parent.GetComponent<EnterLevelKeyhole>().levelToEnter;
		levelData = Game.instance.dataManager.GetLevelData(levelToDisplay);
		alphaValue = 0f;
		guiInfos[0].text = "Level " + levelToDisplay;
		if(guiInfos.Length > 2 && levelData != null && levelData.rooms != null) {
			guiInfos[1].text = GetRoomProgress();
			guiInfos[2].text = "Friends Saved: " + levelData.GetNumSavedFriends();
		}
		else {
			guiInfos[1].text = "";	
			guiInfos[2].text = "";
		}
	}
	
	string GetRoomProgress() {
		string progressString = "";
		if(levelData != null) {
			int reachedRooms = levelData.GetNumReachedRooms();
			int totalRooms = levelData.rooms.Length;
			if(reachedRooms < totalRooms) {
				progressString = "Rooms: " + reachedRooms + " of " + totalRooms;
			}
			else {
				int reachedKeyholes = levelData.getNumReachedKeyholes();
				int totalKeyholes = levelData.getNumKeyholes();
				if(reachedKeyholes < totalKeyholes) {
					progressString = "Keyholes Reached: " + reachedKeyholes + " of " + totalKeyholes;	
				}
				else progressString = "Completed!";
			}
		}
		return progressString;
	}
	
}
