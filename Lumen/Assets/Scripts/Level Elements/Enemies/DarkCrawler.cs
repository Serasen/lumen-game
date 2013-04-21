using UnityEngine;
using System.Collections;

public class DarkCrawler : MonoBehaviour {
	private int direction = 0;
	public int speed = 10, fallspeed = 10;
	public bool midair = true;
	public int waypoint = -1;
	private PathFinder pf;
	Room myRoom;
	
	// Use this for initialization
	void Start () {
		pf = transform.parent.GetComponent<DarkCrawlerSpawn>().waypoints.GetComponent<PathFinder>();
	}
	
	// Update is called once per frame
	void Update () {
		if(pf.GetIloAt() > waypoint)
			direction = 1;
		else if(pf.GetIloAt() < waypoint)
			direction = -1;
		if(waypoint == -1) direction *= -1;
		if(!midair)
			rigidbody.velocity = direction*speed*transform.right;
		else {
			rigidbody.velocity = -transform.up*fallspeed;
		}

	}
	
	void OnEnable() {
		waypoint = -1;
		midair = true;
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag.Equals("Player")) {
			Game.instance.RoomTransition((int)LevelActions.REENTER_ROOM);
			collision.gameObject.transform.GetComponentInChildren<Light>().range = 0;
		}
		else {
			Vector3 eulerAngles = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal).eulerAngles;
			transform.eulerAngles = new Vector3(0, 0, eulerAngles.z);
			midair = false;
		}

	}
	
	void OnCollisionExit(Collision collision) {
		midair = true;
		rigidbody.velocity = -transform.up*fallspeed;
	}
	
	void OnCollisionStay() {
		midair = false;
	}
	
	public void UpdateWaypoint(Transform t) {
		waypoint = pf.GetWaypoints().IndexOf(t);
	}
}
