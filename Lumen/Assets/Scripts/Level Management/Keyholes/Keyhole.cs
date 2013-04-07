using UnityEngine;
using System.Collections;

public class Keyhole : MonoBehaviour {
	protected int keyholeNum;
	protected Room myRoom;
	
	protected virtual void Start() {
		myRoom = Game.instance.levelManager.getCurrentLevel().getCurrentRoom();
	}
	
	public void setKeyholeNum(int num) {
		keyholeNum = num;	
	}
	
	protected virtual void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			myRoom.keyholeReached(keyholeNum);
		}
	}
}
