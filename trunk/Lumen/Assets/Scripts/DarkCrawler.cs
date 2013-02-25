using UnityEngine;
using System.Collections;

public class DarkCrawler : MonoBehaviour {
	private int direction = 0;
	private int speed = 10;
	private bool midair = true;
	private int waypoint = -1;
	private PathFinder pf;
	private Transform lastWaypoint;
	public Transform Ilo;
	
	// Use this for initialization
	void Start () {
		pf = GameObject.Find ("Waypoints").GetComponent<PathFinder>();
	}
	
	// Update is called once per frame
	void Update () {
		//ChooseDirection(); //direction = Ilo.position.x > transform.position.x - 1 ? 1 : -1;
		if(pf.GetIloAt() > waypoint)
			direction = 1;
		else if(pf.GetIloAt() < waypoint)
			direction = -1;
		if(!midair)
			rigidbody.velocity = direction*speed*transform.right;//.normalized;// - transform.up*15f;
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag.Equals("Player"))
			Application.LoadLevel(Application.loadedLevel);
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
	
	private void ChooseDirection() {
		Vector3 ilopos = Ilo.position, mypos = transform.position;
		bool rightisup = transform.right.normalized.y > .5;
		bool leftisup = transform.right.normalized.y < -.5;
		if(ilopos.x > mypos.x + 5 ||
			rightisup && ilopos.y > mypos.y ||
			leftisup && ilopos.y < mypos.y)
				direction = 1;
		else if(ilopos.x < mypos.x -5 ||
			rightisup && ilopos.y < mypos.y ||
			leftisup && ilopos.y > mypos.y)
				direction = -1;
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
