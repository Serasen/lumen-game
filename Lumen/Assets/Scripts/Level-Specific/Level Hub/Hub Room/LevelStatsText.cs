using UnityEngine;
using System.Collections;

public class LevelStatsText : OscillateFadeText {
	int levelToDisplay;
	LevelData levelData;

	protected override void OnEnable() {
		levelToDisplay = transform.parent.GetComponent<EnterLevelKeyhole>().levelToEnter;
		levelData = Game.instance.dataManager.GetLevelData(levelToDisplay);
		alphaValue = 0f;
		if(guiInfos.Length > 1 && levelData != null && levelData.rooms != null) {
			//Debug.Log("level " + levelToDisplay + " , rooms:" + levelData.rooms.Length);
			guiInfos[0].text = GetRoomProgress();
			guiInfos[1].text = GetFriendProgress();
			guiInfos[2].text = "Deaths: " + levelData.deaths;
		}
		else {
			guiInfos[0].text = "";	
			guiInfos[1].text = "";
			guiInfos[2].text = "";
		}
	}
	
	string GetRoomProgress() {
		string progressString = "";
		int reachedRooms = levelData.GetNumReachedRooms();
		int totalRooms = levelData.rooms.Length;
		if(reachedRooms < totalRooms)
			progressString = "Rooms : " + reachedRooms + " of " + totalRooms;
		else {
			int reachedKeyholes = levelData.getNumReachedKeyholes();
			int totalKeyholes = levelData.getNumKeyholes();
			if(reachedKeyholes < totalKeyholes)
				progressString = "Keyholes reached: " + reachedKeyholes + " of " + totalKeyholes;	
			else progressString = "All rooms completed!";
		}
		return progressString;
	}
	
	string GetFriendProgress() {
		string progressString = "";
		int reachedFriends = levelData.GetNumSavedFriends();
		int totalFriends = levelData.friendsSaved.Length;
		if(reachedFriends < totalFriends)
			progressString = "Friends saved : " + reachedFriends + " of " + totalFriends;
		else progressString = "All friends saved!";
		return progressString;		
	}
}
