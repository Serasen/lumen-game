using UnityEngine;
using System.Collections;

public class IloController : MonoBehaviour {
	
	bool onSurface;
	bool midJump;
	
	Vector3 surfaceNormal; //normal of surface currently occupied
	Vector3 myNormal; //normal of player before correction to surface


	// Use this for initialization
	void Start () {
		onSurface = true;
		if(surfaceNormal == Vector3.zero) {
				surfaceNormal = Vector3.up;
		}
	}
	
	void FixedUpdate() {
		 //rigidbody.AddRelativeForce(Vector3.down*1000);
	}

	float angleToRotate;	
	float jumpDistance;
	Vector3 jumpDirection;
	
	
	public float runSpeed = 15f;
	public float jumpSpeed = 15f;
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if(onSurface) {
			if(Physics.Raycast(transform.position, -surfaceNormal, out hit)) {
				surfaceNormal = hit.normal;
				myNormal = Vector3.Lerp(myNormal, surfaceNormal, 10*Time.deltaTime);
				transform.up = myNormal;
				//Hold player to surface
				transform.position = hit.point + surfaceNormal*transform.localScale.y*0.5f;
			}
			transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z);
			float input = Input.GetAxis("Horizontal");
			//on-surface movement
			rigidbody.velocity = input*runSpeed*transform.right.normalized; 
					
			if(Input.GetButtonDown("Jump")) {
				onSurface = false;
				Vector3 leanDirection = input * transform.right;
				surfaceNormal += leanDirection;
				
				//configure Jump
				if(Physics.Raycast(transform.position, surfaceNormal, out hit)) {
					jumpDirection = hit.point - transform.position;
					jumpDistance = jumpDirection.magnitude - transform.localScale.y*0.5f;
					angleToRotate = Vector3.Angle(transform.up, hit.normal) * Mathf.Deg2Rad;
					rigidbody.velocity = jumpDirection.normalized*jumpSpeed;
				}
				else {
					//jump into space
					rigidbody.velocity = (surfaceNormal + leanDirection).normalized*jumpSpeed;
					angleToRotate = 0f;
				}
			}
		}
		else {
			if(Physics.Raycast(transform.position, surfaceNormal, out hit, transform.localScale.y + 0.1f)) {
				surfaceNormal = hit.normal;
				onSurface = true;
			}
			else {
				transform.RotateAround(Vector3.forward, angleToRotate*Time.deltaTime/(jumpDistance/jumpSpeed));
			}
		}
		//Prevent player from flipping in Z when completely upside down
		
		
	}
	
	void OnCollisionEnter(Collision collision) {
		if(!onSurface ) {
			ContactPoint contact = collision.contacts[0];
			transform.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
			surfaceNormal = contact.normal;
			onSurface = true;
		}
	}

}