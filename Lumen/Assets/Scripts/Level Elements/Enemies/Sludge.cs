using UnityEngine;
using System.Collections;

public class Sludge : MonoBehaviour {
	public float liveDuration;
	public float stickDuration;
	
	void Start () {
		StartCoroutine("Die");
	}
	
	void Update() {
		transform.Rotate(0,0,.2f);
	}
	
	void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			collider.GetComponent<IloMeta>().DisableController(stickDuration);
			audio.Play();
		}
	}
	
	void OnDisable() {
		Destroy(this.gameObject);
	}
	
	IEnumerator Die() {
		yield return new WaitForSeconds(liveDuration);
		StartCoroutine("Shrink");
	}
	
	IEnumerator Shrink() {
		while(transform.localScale.y > 0) {
			transform.localScale -= (Vector3.up + Vector3.right) * .1f;
			//transform.position -= transform.up * .07f;
			yield return new WaitForSeconds(.1f);
		}
		Destroy(this.gameObject);
	}
}
