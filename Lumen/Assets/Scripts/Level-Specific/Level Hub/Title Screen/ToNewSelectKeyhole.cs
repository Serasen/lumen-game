using UnityEngine;
using System.Collections;

public class ToNewSelectKeyhole : Keyhole {
	protected override void OnTriggerEnter(Collider collider) {
		int levelToEnter = 1;
		int saveFile = 1;
		if(collider.tag == "Player") {
			if(Game.instance.dataManager.hasSaved()) {
				myRoom.keyholeReached(keyholeNum);				
			}
			else {
				Game.instance.dataManager.StartNewGame(saveFile);
				Game.instance.levelManager.initializeGameData();
				Game.instance.levelManager.getCurrentLevel().initializeLevelData();
				Game.instance.RoomTransition((int)LevelActions.CHANGE_LEVEL, levelToEnter);
			}
		}
	}
}
