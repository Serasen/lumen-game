using UnityEngine;
using System.Collections;

public class IloShine : MonoBehaviour {
	
	public int maxShine;
	public float maxIntensity;
	int shine;
	
	Room myRoom;
	float intensityStep;
	
	Light iloLight;
	
	void Awake() {
		iloLight = gameObject.GetComponentInChildren<Light>();
		intensityStep = 1/8f;
	}
	
	// Use this for initialization
	void OnEnable () {
		shine = maxShine;
		iloLight.intensity = maxIntensity;
		StartCoroutine("FadeDark");
		StartCoroutine("LoseShine");
	}
	
	void OnDisable() {
		StopCoroutine("FadeDark");
		StopCoroutine("LoseShine");
	}
	
	bool IsAlive() {
		return (shine > 0);
	}
	
	#region lose shine
	
	IEnumerator FadeDark() {
		while(iloLight.intensity > 0) {
			iloLight.intensity -= intensityStep*maxIntensity/maxShine;
			yield return new WaitForSeconds(intensityStep);
		}
		iloLight.intensity = 0;
		StopCoroutine("FadeDark");
	}
	
	IEnumerator LoseShine() {
		while(shine > 0) {
			yield return new WaitForSeconds(1f);
			shine--;
		}
		StopCoroutine("FadeDark");
		StopCoroutine("LoseShine");
		LevelManager.instance.getCurrentLevel().getCurrentRoom().reEnterRoom();
	}
	#endregion
	
	#region gain shine
	Vector3 shineZoneCenter;
	float maxDistanceToCenter;
	int startShine;
	float startIntensity;
	
	public void StartFadeLight(Vector3 lightPoint) {
		CancelInvoke();
		StartCoroutine("fadeUpShine");
		StartCoroutine("fadeUpIntensity");
	}
	
	
	IEnumerator fadeUpShine() {
		while(shine < maxShine) {
        	yield return new WaitForSeconds(0.01f);
			shine += 1;
		}
		shine = maxShine;
		
	}
	
	IEnumerator fadeUpIntensity() {
		while(iloLight.intensity < maxIntensity) {
        	yield return new WaitForSeconds(0.01f/(maxIntensity/maxShine));
			iloLight.intensity += 0.1f;
		}
		iloLight.intensity = maxIntensity;
	}
	
	public void EndFadeLight() {
		StopCoroutine("fadeUpShine");
		StopCoroutine("fadeUpIntensity");
		InvokeRepeating("LoseShine",1,1);
		InvokeRepeating("FadeDark",1,intensityStep);
	}
	#endregion
	
	void OnGUI() {
		GUI.Box(new Rect(0, 0, 50, 50), "Shine\n" + shine);
			
	}
}
