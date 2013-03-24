using UnityEngine;
using System.Collections;

public class ShineZone : MonoBehaviour {
	private float blackoutDuration;
	private bool litup = true;
	private Light thelight;
	private Collider ilo;
	
	void Start() {
		foreach(Transform child in transform.parent) {
			if(child.gameObject.name == "Light") {
				thelight = child.gameObject.light;
			}
		}
	}
	void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			ilo = collider;
			if(litup) {
				collider.GetComponent<IloShine>().StartFadeLight();
			}
		}
		else if(collider.tag == "Smogsworth") {
			Smogsworth s = collider.GetComponent<Smogsworth>();
			blackoutDuration = s.smogAttackDuration;
			s.SmogAttack();
			StartCoroutine("Blackout");
		}
	}
	
	void OnTriggerExit(Collider collider) {
		if(collider.tag == "Player") 
		{
			ilo = null;
			collider.GetComponent<IloShine>().EndFadeLight();
		}
	}
	
	IEnumerator Blackout() {
		if(ilo != null) {
			ilo.GetComponent<IloShine>().EndFadeLight();
		}
		litup = false;
		float temp = thelight.intensity;
		thelight.intensity = 0;
		yield return new WaitForSeconds(blackoutDuration);
		thelight.intensity = temp;
		litup = true;
		if(ilo != null) {
			ilo.GetComponent<IloShine>().StartFadeLight();
		}
	}
}
