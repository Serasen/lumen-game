using UnityEngine;
using System.Collections;

public class Lightball : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter() {
		foreach(Transform child in transform) {
			if(child.name.Equals("Main light")) {
				child.GetComponent<Light>().intensity = 0;	
			}
			else {
				child.GetComponent<Light>().intensity = .35F;
			}
		}
		GetComponent<MeshRenderer>().enabled = false;
	}
}
