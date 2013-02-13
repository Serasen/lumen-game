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
		midJump = false;
		surfaceNormal = Vector3.up;
	}
	
	void FixedUpdate() {
		 //rigidbody.AddRelativeForce(Vector3.down*1000);
	}
	
	float input;
	float angleToRotate;	
	float jumpDistance;
	Vector3 jumpDirection;
	
	
	public float runSpeed;
	public float jumpSpeed; 
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if(onSurface) {
			if(Physics.Raycast(transform.position, -surfaceNormal, out hit, transform.localScale.y) &&
				Vector3.Angle(surfaceNormal, hit.normal) < 60f) 
			{
				surfaceNormal = hit.normal;
				myNormal = Vector3.Lerp(myNormal, surfaceNormal, 10*Time.deltaTime);
				transform.up = myNormal;
				
				//Hold player to surface	
				transform.position = hit.point + surfaceNormal*transform.localScale.y*0.5f;
			}
			else {
				onSurface = false;
			}
			
			transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z);
			input = Input.GetAxis("Horizontal");
			rigidbody.velocity = input*runSpeed*transform.right.normalized; 
			
			//on-surface movement
					
			if(Input.GetButtonDown("Jump"))
			{
				initiateJump();
			}
		}
		else {
			if(midJump) {
				if(Physics.Raycast(transform.position, surfaceNormal, out hit, transform.localScale.y * 0.5f)) 
				{
					surfaceNormal = hit.normal;
					onSurface = true;
					midJump = false;
				}
			}
			else {
				if(rigidbody.velocity.magnitude < jumpSpeed) {
					rigidbody.AddForce(-surfaceNormal*jumpSpeed, ForceMode.Acceleration);
				}
			}
		}	
	}
	
	void OnCollisionEnter(Collision collision) {
		if(!onSurface ) {
			ContactPoint contact = collision.contacts[0];
			if(!(midJump && Vector3.Angle(contact.normal, transform.up) < 15f)) {
				transform.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
				surfaceNormal = contact.normal;
				onSurface = true;
				midJump = false;
			}
		}
		else {
			if(!Physics.Raycast(transform.position, -transform.right, transform.localScale.y*0.5f) &&
			!Physics.Raycast(transform.position, transform.right, transform.localScale.y*0.5f)) {
				surfaceNormal = collision.contacts[0].normal;
			}
		}
	}
	
	void initiateJump() {
		onSurface = false;
		midJump = true;
		Vector3 leanDirection = input * transform.right;
		if(Physics.Raycast(transform.position, -transform.right, transform.localScale.y*0.5f) ||
		Physics.Raycast(transform.position, transform.right, transform.localScale.y*0.5f)) {
			leanDirection = Vector3.zero;	
		}
		surfaceNormal += leanDirection;
		
		RaycastHit hit;
		//configure Jump
		if(Physics.Raycast(transform.position, surfaceNormal, out hit)) {
			jumpDirection = hit.point - transform.position;
			rigidbody.velocity = jumpDirection.normalized*jumpSpeed;
		}
		else {
			//jump into space
			rigidbody.velocity = (surfaceNormal + leanDirection).normalized*jumpSpeed;
			angleToRotate = 0f;
		}
	}

}