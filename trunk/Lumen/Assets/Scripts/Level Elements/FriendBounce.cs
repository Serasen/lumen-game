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
		if(bouncesLeft > 0) {
			rigidbody.velocity = Vector3.zero;
			bounceVector *= -1;
			Bounce ();
			bouncesLeft--;
		}
		else {
			transform.position = startPos;
		}
	}
	
	public void Bounce() {
		rigidbody.AddForce(bounceVector);
	}
}
