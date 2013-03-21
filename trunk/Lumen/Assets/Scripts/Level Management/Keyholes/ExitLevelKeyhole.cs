using UnityEngine;
using System.Collections;

public class ExitLevelKeyhole : Keyhole {
	
	protected override void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			myRoom.levelKeyholeReached(keyholeNum, 0);
		}
	}
	
}
