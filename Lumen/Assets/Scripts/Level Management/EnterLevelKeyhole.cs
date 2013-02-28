using UnityEngine;
using System.Collections;

public class EnterLevelKeyhole : Keyhole {
	public int levelToEnter;
	
	protected override void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			myRoom.reachedLevelKeyhole(levelToEnter);
		}
	}
	
}
