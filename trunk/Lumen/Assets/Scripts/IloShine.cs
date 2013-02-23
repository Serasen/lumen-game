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
		intensityStep = 1/30f;
	}
	
	// Use this for initialization
	void OnEnable () {
		shine = maxShine;
		iloLight.intensity = maxIntensity;
		InvokeRepeating("LoseShine",1,1);
		InvokeRepeating("FadeDark", 1,intensityStep);
	}
	
	void OnDisable() {
		CancelInvoke();	
	}
	
	bool IsAlive() {
		return (shine > 0);
	}
	
	#region lose shine
	void FadeDark() {
		if(iloLight.intensity > 0) {
			iloLight.intensity -= intensityStep*maxIntensity/maxShine;
		}
		else {
			CancelInvoke("FadeDark");
		}
	}
	
	void LoseShine() {
		shine--;
		if(!IsAlive()) {
			CancelInvoke();
			transform.parent.GetComponent<Room>().reEnterRoom();
		}	
	}
	#endregion
	
	#region gain shine
	Vector3 shineZoneCenter;
	float maxDistanceToCenter;
	int startShine;
	float startIntensity;
	
	public void StartFadeLight(Vector3 lightPoint) {
		this.shineZoneCenter = lightPoint;
		maxDistanceToCenter = (shineZoneCenter - transform.position).magnitude;
		startShine = shine;
		startIntensity = iloLight.intensity;
		CancelInvoke();
		InvokeRepeating("FadeLight",0,intensityStep);
	}
	
	void FadeLight() {
		float currentDistance = maxDistanceToCenter - (shineZoneCenter - transform.position).magnitude;
		float calcShine = Mathf.Ceil(startShine + currentDistance*(maxShine - startShine)/maxDistanceToCenter);
		float calcIntensity = startIntensity + currentDistance*(maxIntensity - startIntensity)/maxDistanceToCenter;
		shine = Mathf.Clamp((int) calcShine, shine, maxShine);
		iloLight.intensity = Mathf.Clamp(calcIntensity, iloLight.intensity, maxIntensity);
	/*	if(calcShine >= shine) {
			shine = (int) calcShine;
			iloLight.intensity = startIntensity + currentDistance*(maxIntensity - startIntensity)/maxDistanceToCenter;
			iloLight.intensity = Mathf.Clamp(iloLight.intensity, 0, maxIntensity);
		} */
	}
	
	public void EndFadeLight() {
		CancelInvoke();
		float waitForIntensity = iloLight.intensity-maxIntensity*shine/maxShine;
		if(waitForIntensity > 0) {
			InvokeRepeating("LoseShine",1 + waitForIntensity,1);
			InvokeRepeating("FadeDark",1,intensityStep);
		}
		else {
			InvokeRepeating("LoseShine", 1,1);
			InvokeRepeating("FadeDark", 1 + waitForIntensity,intensityStep);			
		}
	}
	#endregion
	
	void OnGUI() {
		GUI.Box(new Rect(0, 0, 50, 50), "Shine\n" + shine);
			
	}
}
