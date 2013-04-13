using UnityEngine;
using System.Collections;

public class Sludge : MonoBehaviour {
	public float liveDuration;
	public float stickDuration;
	
	void Start () {
		StartCoroutine("Die");
	}
	
	void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			collider.GetComponent<IloMeta>().DisableController(stickDuration);
			audio.Play();
			collider.gameObject.transform.rotation = transform.rotation;
		}
	}
	
	IEnumerator Die() {
		yield return new WaitForSeconds(liveDuration);
		StartCoroutine("Shrink");
	}
	
	IEnumerator Shrink() {
		while(transform.localScale.y > 0) {
			transform.localScale -= Vector3.up * .1f;
			transform.position -= transform.up * .07f;
			yield return new WaitForSeconds(.2f);
		}
		Destroy(this.gameObject);
	}
}
