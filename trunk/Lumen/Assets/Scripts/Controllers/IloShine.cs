using UnityEngine;
using System.Collections;

public class IloShine : MonoBehaviour {
	
	public int maxShine;
	public float maxRange;
	int shine;
	
	Room myRoom;
	float rangeStep;
	
	Light iloLight;
	
	void Awake() {
		iloLight = gameObject.GetComponentInChildren<Light>();
		rangeStep = 1/16f;
	}
	
	// Use this for initialization
	void OnEnable () {
		shine = maxShine;
		iloLight.range = maxRange;
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
		while(iloLight.range > 0) {
			iloLight.range -= rangeStep*maxRange/maxShine;
			yield return new WaitForSeconds(rangeStep);
		}
		iloLight.range = 0;
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
	float startRange;
	
	public void StartFadeLight(Vector3 lightPoint) {
		CancelInvoke();
		StartCoroutine("fadeUpShine");
		StartCoroutine("fadeUpRange");
	}
	
	
	IEnumerator fadeUpShine() {
		while(shine < maxShine) {
        	yield return new WaitForSeconds(0.01f);
			shine += 1;
		}
		shine = maxShine;
		
	}
	
	IEnumerator fadeUpRange() {
		while(iloLight.range < maxRange) {
        	yield return new WaitForSeconds(0.01f/(maxRange/maxShine));
			iloLight.range += 0.1f;
		}
		iloLight.range = maxRange;
	}
	
	public void EndFadeLight() {
		StopCoroutine("fadeUpShine");
		StopCoroutine("fadeUpRange");
		InvokeRepeating("LoseShine",1,1);
		InvokeRepeating("FadeDark",1,rangeStep);
	}
	#endregion
	
	void OnGUI() {
		GUI.Box(new Rect(0, 0, 50, 50), "Shine\n" + shine);
			
	}
}
