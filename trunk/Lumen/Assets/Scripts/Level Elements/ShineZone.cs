using UnityEngine;
using System.Collections;

public class ShineZone : MonoBehaviour {
	private float blackoutDuration;
	private float startIntensity;
	private float intensityStep = 1/16f;
	private bool litup = true;
	private Light theLight;
	private Collider ilo;
	private Smogsworth smogs;
	
	void Start() {
		foreach(Transform child in transform.parent) {
			if(child.gameObject.name == "Light") {
				theLight = child.gameObject.light;
				startIntensity = theLight.intensity;
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
			StopAllCoroutines();
			smogs = collider.GetComponent<Smogsworth>();
			smogs.StopAllCoroutines();
			smogs.rigidbody.velocity = Vector3.zero;
			blackoutDuration = smogs.smogAttackDuration;
			StartCoroutine("FadeDark");
		}
	}
	
	void OnTriggerExit(Collider collider) {
		if(collider.tag == "Player") 
		{
			ilo = null;
			collider.GetComponent<IloShine>().EndFadeLight();
		}
	}
	
	void OnDisable() {
		StopAllCoroutines();
		litup = true;
		if(theLight != null)
			theLight.intensity = startIntensity;
	}
	
	IEnumerator FadeDark() {
		while(theLight.intensity > 0) {
			theLight.intensity -= intensityStep;
			yield return new WaitForSeconds(intensityStep);
		}
		theLight.intensity = 0;
		StartCoroutine("Blackout");
		StopCoroutine("FadeDark");
	}
	
	IEnumerator Blackout() {
		smogs.SmogAttack();
		if(ilo != null) {
			ilo.GetComponent<IloShine>().EndFadeLight();
		}
		litup = false;
		theLight.intensity = 0;
		yield return new WaitForSeconds(blackoutDuration);
		litup = true;
		if(ilo != null) {
			ilo.GetComponent<IloShine>().StartFadeLight();
		}
		StartCoroutine("FadeLight");
		StopCoroutine("Blackout");
	}
	
	IEnumerator FadeLight() {
		while(theLight.intensity < startIntensity) {
			theLight.intensity += intensityStep;
			yield return new WaitForSeconds(intensityStep);
		}
		theLight.intensity = startIntensity;
		StopCoroutine("FadeLight");
	}
}
