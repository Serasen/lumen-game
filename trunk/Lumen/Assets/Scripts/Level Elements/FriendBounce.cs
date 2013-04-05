using UnityEngine;
using System.Collections;

public class FriendBounce : MonoBehaviour {
	int bouncesLeft = 3;
	public int bounceSpeed;
	Vector3 bounceVector;
	Vector3 startPos;
	
	void Start() {
		bounceVector = transform.up * bounceSpeed;
		startPos = transform.position;
	}

	void OnCollisionEnter(Collision c) {
		StopCoroutine("Bouncer");
		if(bouncesLeft > 0) {
			rigidbody.velocity = Vector3.zero;
			bounceVector *= -1;
			StartCoroutine("Bouncer");
			bouncesLeft--;
		}
		else {
			transform.position = startPos;
		}
	}
	
	IEnumerator Bouncer() {
		while(rigidbody.velocity.magnitude < bounceVector.magnitude) {
			rigidbody.AddForce(bounceVector, ForceMode.Acceleration);
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	public void Bounce() {
		StopCoroutine("Bouncer");
		StartCoroutine("Bouncer");
	}
}
