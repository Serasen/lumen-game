using UnityEngine;
using System.Collections;

public class ShineZone : MonoBehaviour {
	
	void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") 
		{
			collider.GetComponent<IloShine>().StartFadeLight(transform.position);
		}
	}
	
	void OnTriggerExit(Collider collider) {
		if(collider.tag == "Player") 
		{
			collider.GetComponent<IloShine>().EndFadeLight();
		}
	}
}
