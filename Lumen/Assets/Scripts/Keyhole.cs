using UnityEngine;
using System.Collections;

public class Keyhole : MonoBehaviour {
	int keyholeNum;
	Room myRoom;
	
	void Start() {
		myRoom = transform.parent.GetComponent<Room>();	
	}
	
	public void setKeyholeNum(int num) {
		keyholeNum = num;	
	}
	
	void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			myRoom.reachedKeyhole(keyholeNum);
		}
	}
	
}
