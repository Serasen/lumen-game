using UnityEngine;
using System.Collections;

public class IloShine : MonoBehaviour {
	
	public int maxShine;
	public float maxIntensity;
	int shine;
	
	float intensityStep;
	
	Light iloLight;
	
	void Awake() {
		shine = maxShine;
		iloLight = gameObject.GetComponentInChildren<Light>();
		intensityStep = 1/30f;
		iloLight.intensity = maxIntensity;
	}
	
	// Use this for initialization
	void Start () {
		InvokeRepeating("LoseShine",0,1);
		InvokeRepeating("FadeDark",0,intensityStep);
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
			CancelInvoke("LoseShine");
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
		float calcShine = startShine + currentDistance*(maxShine - startShine)/maxDistanceToCenter;
		calcShine = Mathf.Clamp(calcShine, shine, maxShine);
		if(calcShine > shine) {
			shine = (int) calcShine;
			iloLight.intensity = startIntensity + currentDistance*(maxIntensity - startIntensity)/maxDistanceToCenter;
			iloLight.intensity = Mathf.Clamp(iloLight.intensity, 0, maxIntensity);
		}
	}
	
	public void EndFadeLight() {
		CancelInvoke();
		float waitForIntensity = iloLight.intensity-maxIntensity*shine/maxShine;
		InvokeRepeating("LoseShine",waitForIntensity,1);
		InvokeRepeating("FadeDark",0,intensityStep);
	}
	#endregion

	bool IsAlive() {
		return (shine > 0);
	}
}
