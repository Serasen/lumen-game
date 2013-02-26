using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {
	
	void OnTriggerEnter(Collider collider) {
		Debug.Log("GOT IT!");
		if(collider.gameObject.tag == "Player") {
			collider.gameObject.GetComponent<IloController>().initiateReflect();
		}
	}
}
