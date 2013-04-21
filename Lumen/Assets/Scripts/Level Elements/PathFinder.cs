using UnityEngine;
using System.Collections;

public class PathFinder : MonoBehaviour {
	ArrayList waypoints = new ArrayList();
	public int iloAt = 5;
	
	// Use this for initialization
	void Start () {
		waypoints.AddRange(GetComponentsInChildren<Transform>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetIloAt(Transform t) {
		iloAt = waypoints.IndexOf(t);
	}
	
	public int GetIloAt() {
		return iloAt;
	}
	
	public ArrayList GetWaypoints() {
		return waypoints;
	}
}
