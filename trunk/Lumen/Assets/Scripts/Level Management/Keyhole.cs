using UnityEngine;
using System.Collections;

public class Keyhole : MonoBehaviour {
	public Texture2D texture;
	protected int keyholeNum;
	protected Room myRoom;
	
	void Start() {
		myRoom = transform.parent.GetComponent<Room>();	
	}
	
	public void setKeyholeNum(int num) {
		keyholeNum = num;	
	}
	
	protected virtual void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			myRoom.reachedKeyhole(keyholeNum);
		}
	}
	
}
