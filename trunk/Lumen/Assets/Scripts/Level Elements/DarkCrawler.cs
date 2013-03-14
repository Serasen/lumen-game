using UnityEngine;
using System.Collections;

public class DarkCrawler : MonoBehaviour {
	private int direction = 0;
	private int speed = 10, fallspeed = 10;
	private bool midair = true;
	private int waypoint = -1;
	private PathFinder pf;
	private Transform lastWaypoint;
	Room myRoom;
	
	// Use this for initialization
	void Start () {
		pf = transform.parent.GetComponent<DarkCrawlerSpawn>().waypoints.GetComponent<PathFinder>();
	}
	
	// Update is called once per frame
	void Update () {
		if(pf.GetIloAt() > waypoint)
			direction = -1;
		else if(pf.GetIloAt() < waypoint)
			direction = 1;
		if(!midair)
			rigidbody.velocity = direction*speed*transform.right;
		else {
			rigidbody.velocity = transform.up*fallspeed;
		}

	}
	
	void FixedUpdate() {
		
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag.Equals("Player")) {
			waypoint = -1;
			Game.instance.levelManager.getCurrentLevel().getCurrentRoom().reEnterRoom();
		}
	}
	
	void OnCollisionStay(Collision collision) {
		transform.rotation = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
		Vector3 eulerAngles = transform.rotation.eulerAngles;
    	eulerAngles = new Vector3(0, 0, eulerAngles.z+180);
    	transform.rotation = Quaternion.Euler(eulerAngles);
		midair = false;
	}
	
	void OnCollisionExit(Collision collision) {
		midair = true;
		rigidbody.velocity = transform.up*fallspeed;
	}
	
	void OnCollisionStay() {
		midair = false;
	}
	
	public void UpdateWaypoint(Transform t) {
		if(!t.Equals(lastWaypoint)) {
			if(waypoint == -1)
				waypoint = pf.GetWaypoints().IndexOf(t);
			else if(direction == -1)
				waypoint++;
			else if(direction == 1)
				waypoint--;
			lastWaypoint = t;
		}
	}
}
