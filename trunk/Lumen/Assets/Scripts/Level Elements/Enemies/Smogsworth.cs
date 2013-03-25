using UnityEngine;
using System.Collections;

public class Smogsworth : MonoBehaviour {
	public float speed;
	public float directionChangeInterval;
	public float smogAttackDuration;
	private Vector3 randomDir = new Vector3();
	
	// Use this for initialization
	void Start () {

		rigidbody.velocity = RandomDir();
		StartCoroutine("MoveAbout");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision c) {
		StopCoroutine("MoveAbout");
		StartCoroutine("OneIteration");
		rigidbody.velocity = Vector3.Reflect(rigidbody.velocity,c.contacts[0].normal).normalized*speed;
	}
	
	public void SmogAttack() {
		StopAllCoroutines();
		rigidbody.velocity = Vector3.zero;
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
		renderer.enabled = false;
		yield return new WaitForSeconds(smogAttackDuration);
		renderer.enabled = true;
		rigidbody.velocity = RandomDir();
		StartCoroutine("MoveAbout");
	}
}
