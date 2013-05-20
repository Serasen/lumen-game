using UnityEngine;
using System.Collections;

public class Smogsworth : MonoBehaviour {
	public float speed;
	public float directionChangeInterval;
	public float smogAttackDuration;
	private Vector3 randomDir = new Vector3();
	private Vector3 shineZonePos;
	private Vector3 startPos;
	
	// Use this for initialization
	void OnEnable () {
		rigidbody.velocity = RandomDir();
		StartCoroutine("MoveAbout");
		renderer.enabled = true;
	}
	
	void OnDisable() {
		StopAllCoroutines();
	}
	
	void OnCollisionEnter(Collision c) {
		StopCoroutine("MoveAbout");
		StartCoroutine("OneIteration");
		rigidbody.velocity = Vector3.Reflect(rigidbody.velocity,c.contacts[0].normal).normalized*speed;
	}
	
	public void SmogAttack(Vector3 pos) {
		StopAllCoroutines();
		rigidbody.velocity = Vector3.zero;
		shineZonePos = pos;
		startPos = transform.position;
		StartCoroutine("SmogItUp");
	}
	
	private Vector3 RandomDir() {
		randomDir.Set(Random.Range (-1.0F,1.0F), Random.Range(-1.0F,1.0F),0);
		return randomDir.normalized*speed;
	}
	
	IEnumerator MoveAbout() {
		while(true) {
			yield return new WaitForSeconds(directionChangeInterval);
			rigidbody.velocity = RandomDir();
		}
	}
	IEnumerator OneIteration() {
		yield return new WaitForSeconds(directionChangeInterval);
		rigidbody.velocity = RandomDir();
		StartCoroutine("MoveAbout");
	}
	IEnumerator SmogItUp() {
		int iterations = 100;
		Vector3 diff = shineZonePos - startPos;
		Color color = renderer.material.color;
		collider.isTrigger = true;
		for(int i = 0; i < iterations; i++) {
			transform.position += diff/iterations;
			color.a -= 1.0f/iterations;
			renderer.material.color = color;
			transform.localScale *= 1.01f;
			yield return new WaitForSeconds(.02f);
		}
		yield return new WaitForSeconds(smogAttackDuration - 0.02f * iterations);
		for(int i = 0; i < iterations; i++) {
			color.a += 1.0f/iterations;
			renderer.material.color = color;
			transform.localScale /= 1.01f;
			yield return new WaitForSeconds(.02f);
		}
		collider.isTrigger = false;
		rigidbody.velocity = RandomDir();
		StartCoroutine("MoveAbout");
	}
}
