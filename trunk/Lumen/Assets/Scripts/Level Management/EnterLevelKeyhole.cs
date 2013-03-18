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
		if(!isUnlocked) {
			isUnlocked = Game.instance.dataManager.isLevelUnlocked(levelToEnter);
			if(!isUnlocked) {
				renderer.enabled = false;
				collider.enabled = false;
				foreach(Transform child in transform) {
					child.gameObject.SetActive(false);	
				}
			}
		}
		if(isUnlocked) {
			renderer.enabled = true;
			collider.enabled = true;
			foreach(Transform child in transform) {
				child.gameObject.SetActive(true);	
			}				
		}
	}
	
	protected override void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			myRoom.levelKeyholeReached(keyholeNum, levelToEnter);
		}
	}
	
}
