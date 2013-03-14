using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {
	PathFinder pf;
	// Use this for initialization
	void Start () {
		pf = null;
	//	pf = transform.parent.GetComponent<PathFinder>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider c) {
		if(c.gameObject.tag.Equals("Player")) {
			if(pf == null) {
				pf = transform.parent.GetComponent<PathFinder>(); 	
			}
			pf.SetIloAt(transform);
		}
		else if (c.gameObject.tag == "DarkCrawler")
			c.gameObject.GetComponent<DarkCrawler>().UpdateWaypoint(transform);
	}
}
