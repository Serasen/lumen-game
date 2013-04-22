using UnityEngine;
using System.Collections;

public class EnterLevelKeyhole : Keyhole {
	public int levelToEnter;
	bool isUnlocked;
	
	protected override void Start() {
		base.Start();
		isUnlocked = false;	
	}
	
	void OnEnable() {
		isUnlocked = Game.instance.dataManager.isLevelUnlocked(levelToEnter);
		renderer.enabled = isUnlocked;
		collider.enabled = isUnlocked;
		foreach(Transform child in transform) {
			child.gameObject.SetActive(isUnlocked);	
		}
	}
	
	protected override void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			myRoom.levelKeyholeReached(keyholeNum, levelToEnter);
		}
	}
	
}
