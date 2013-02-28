using UnityEngine;
using System.Collections;

public class DarkCrawler : MonoBehaviour {
	private int direction = 0;
	private int speed = 10;
	private bool midair = true;
	private int waypoint = -1;
	private PathFinder pf;
	private Transform lastWaypoint;
	Room myRoom;
	
	// Use this for initialization
	void Start () {
		myRoom = transform.parent.GetComponent<Room>();
		pf = GameObject.Find("Waypoints").GetComponent<PathFinder>();
	}
	
	// Update is called once per frame
	void Update () {
		if(pf.GetIloAt() > waypoint)
			direction = 1;
		else if(pf.GetIloAt() < waypoint)
			direction = -1;
		if(!midair)
			rigidbody.velocity = direction*speed*transform.right;
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag.Equals("Player"))
			myRoom.reEnterRoom();
	}
	
	void OnCollisionStay(Collision collision) {
		transform.rotation = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
		Vector3 eulerAngles = transform.rotation.eulerAngles;
    	eulerAngles = new Vector3(0, 0, eulerAngles.z);
    	transform.rotation = Quaternion.Euler(eulerAngles);
		midair = false;
	}
	
	void OnCollisionExit(Collision collision) {
		midair = true;
		rigidbody.velocity = -transform.up*5;
	}
	
	void OnCollisionStay() {
		midair = false;
	}
	
	public void UpdateWaypoint(Transform t) {
		if(!t.Equals(lastWaypoint)) {
			if(waypoint == -1)
				waypoint = pf.GetWaypoints().IndexOf(t);
			else if(direction == 1)
				waypoint++;
			else if(direction == -1)
				waypoint--;
			lastWaypoint = t;
		}
	}
}