using UnityEngine;
using System.Collections;

public class LevelHub : Level {
	public GameObject hubRoom;
	int hubRoomNum;
	int lastLevelEntered;
	
	void Start() {
		
		for(int i = 0; i < rooms.Length; i++) {
			if(rooms[i].room == hubRoom) {
				hubRoomNum = i;
				break;
			}
		}
		lastLevelEntered = 0;
		setCurrentRoom(0, 0);
	}
	
	//Not called on startup, but every time after
	protected override void OnEnable() {
		initializeLevelData();
		/* assumes spawn point 0 is from level hub
		 * and spawn points 1-n are to levels
		 * */
		setCurrentRoom(hubRoomNum, lastLevelEntered);
	}
	
		//Leave a particular level
	public void setLevelEntered(int levelToEnter) {
		lastLevelEntered = levelToEnter;
	}
}
