using UnityEngine;
using System.Collections;

public class LevelHub : Level {
	public GameObject hubRoom;
	int hubRoomNum;
	int lastLevelEntered;
	
	protected override void Start() {
		for(int i = 0; i < rooms.Length; i++) {
			if(rooms[i].room == hubRoom) {
				hubRoomNum = i;
				break;
			}
		}

		levelManager = transform.parent.GetComponent<LevelManager>();
		lastLevelEntered = 0;
		iloInstance = levelManager.getIloInstance();
		if(iloInstance) {
			setCurrentRoom(0, 0);
		}
	}
	
	//Not called on startup, but every time after
	protected override void OnEnable() {
		if(iloInstance) {
			/* assumes keyhole 0 is from level hub
			 * and keyholes 1-n are to levels
			 * */
			setCurrentRoom(hubRoomNum, lastLevelEntered);
		}
	}
	
		//Leave a particular level
	public override void changeLevel(int levelToEnter) {
		lastLevelEntered = levelToEnter;
		base.changeLevel(levelToEnter);
	}
}
