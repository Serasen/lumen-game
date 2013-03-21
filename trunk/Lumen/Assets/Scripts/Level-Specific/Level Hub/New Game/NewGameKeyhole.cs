using UnityEngine;
using System.Collections;

public class NewGameKeyhole : Keyhole {
	public int saveFile;
	
	protected override void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			Game.instance.dataManager.StartNewGame(saveFile);
			Game.instance.levelManager.initializeGameData();
			Game.instance.dataManager.ChangeLevel(0);
			Game.instance.levelManager.getCurrentLevel().initializeLevelData();
			myRoom.keyholeReached(keyholeNum);
		}
	}
	
}
