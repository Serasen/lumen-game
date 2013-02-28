using UnityEngine;
using System.Collections;

public class EndOfLevelKeyhole : Keyhole {
	protected override void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			myRoom.reachedKeyhole(keyholeNum);
		}
	}
	
}
