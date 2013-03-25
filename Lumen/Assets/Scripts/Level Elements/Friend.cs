using UnityEngine;
using System.Collections;

public class Friend : MonoBehaviour {
	public float bounceDuration;
	public float bounceSpeed;
	public Texture2D sprite;
	private Vector3 bounceVector;
	private Vector3 startPos;

	void Start() {
		transform.parent.gameObject.renderer.material.SetTexture("_MainTex", sprite);
		bounceVector = new Vector3(0,bounceSpeed,0);
		startPos = transform.parent.position;
	}
	
	void OnTriggerEnter(Collider c) {
		if(c.tag == "Player") {
			c.GetComponent<IloController>().enabled = false;
			c.rigidbody.velocity = Vector3.zero;
			StartCoroutine("HappyDance");
		}
	}
	
	private void GetHappy() {
		transform.parent.gameObject.renderer.material.SetTextureOffset("_MainTex", new Vector2(0,0));
	}
	
	IEnumerator HappyDance() {
		yield return new WaitForSeconds(.5f);
		GetHappy();
				
		transform.parent.rigidbody.velocity = bounceVector;
		yield return new WaitForSeconds(bounceDuration);
		transform.parent.rigidbody.velocity = -bounceVector;
		yield return new WaitForSeconds(bounceDuration);
		
		transform.parent.rigidbody.velocity = bounceVector;
		yield return new WaitForSeconds(bounceDuration);
		transform.parent.rigidbody.velocity = -bounceVector;
		yield return new WaitForSeconds(bounceDuration);
		transform.parent.rigidbody.velocity = Vector3.zero;
		
		transform.parent.position = startPos;
		
		StopCoroutine("HappyDance");
	}
}
