using UnityEngine;
using System.Collections;

public class IloMeta : MonoBehaviour {
	IloController iloControl;
	float disableTime;
	private Vector2 offset = new Vector2(.5f,0);
	
	// Use this for initialization
	void Start () {
		iloControl = gameObject.GetComponent<IloController>();
	}
	
	public void DisableController(float t) {
		disableTime = t;
		StartCoroutine("DisableControllerRoutine");
	}
	
	IEnumerator DisableControllerRoutine() {
		iloControl.enabled = false;
		renderer.material.SetTextureOffset("_MainTex", renderer.material.GetTextureOffset("_MainTex") + offset);
		yield return new WaitForSeconds(disableTime);
		iloControl.enabled = true;
		renderer.material.SetTextureOffset("_MainTex", renderer.material.GetTextureOffset("_MainTex") - offset);
	}
}
