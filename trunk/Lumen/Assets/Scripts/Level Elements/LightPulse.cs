using UnityEngine;
using System.Collections;

public class LightPulse : MonoBehaviour {
	Light myLight;
	public float minIntensity;
	public float maxIntensity;
	
	// Use this for initialization
	void Start () {
		myLight = GetComponent<Light>();
		StartCoroutine("Pulse");
	}
	
	IEnumerator Pulse() {
		while(true) {
			while(myLight.intensity < maxIntensity) {
				myLight.intensity += .01f;
				yield return new WaitForSeconds(.02f);
			}
			while(myLight.intensity > minIntensity) {
				myLight.intensity -= .01f;
				yield return new WaitForSeconds(.02f);
			}
		}
	}
}
