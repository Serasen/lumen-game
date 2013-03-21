using UnityEngine;
using System.Collections;

public class LoadGameKeyhole : Keyhole {
	public int saveFile;
	
	void OnEnable() {
		if(!Game.instance.dataManager.DataExists(saveFile)) {
			renderer.enabled = false;
			collider.enabled = false;
			foreach(Transform child in transform) {
				child.gameObject.SetActive(false);	
			}
		}
		else {
			renderer.enabled = true;
			collider.enabled = true;
			foreach(Transform child in transform) {
				child.gameObject.SetActive(true);	
			}			
		}
	}
	
	protected override void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			Game.instance.dataManager.Load(saveFile);
			Game.instance.levelManager.initializeGameData();
			Game.instance.dataManager.ChangeLevel(0);
			Game.instance.levelManager.getCurrentLevel().initializeLevelData();
			myRoom.keyholeReached(keyholeNum);
		}
	}
	
}
