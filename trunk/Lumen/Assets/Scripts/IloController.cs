using UnityEngine;
using System.Collections;

public class IloController : MonoBehaviour {
	
	bool onSurface;
	bool midJump;
	
	Vector3 surfaceNormal; //normal of surface currently occupied
	Vector3 jumpVector;
	
		
	float input;
	bool reverseInput;
	
	public float runSpeed;
	public float jumpSpeed;
	
	//If angle of surface to descend is greater than this, slide off
	const float maxDescentAngle = 60f;
	//cannot jump up to a surface angle this close to original surface
	const float minTransferAngle = 15f;

	// Use this for initialization
	void OnEnable () {
		onSurface = true;
		midJump = false;
		reverseInput = false;
		
		surfaceNormal = transform.up;
		jumpVector = -transform.up;
	}

	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		input = Input.GetAxis("Horizontal");
		if(input == 0) {
			reverseInput = Vector3.Angle(transform.up, Vector3.up) > 95f;
		}
		
		if(onSurface) {
			if(Physics.Raycast(transform.position, -surfaceNormal, out hit, transform.localScale.y*2) &&
				Vector3.Angle(surfaceNormal, hit.normal) < maxDescentAngle) 
			{
				Debug.Log("hit surface!");
				surfaceNormal = hit.normal;
				transform.up = Vector3.Lerp(transform.up, surfaceNormal, 10*Time.deltaTime);
				
				//Hold player to surface	
				transform.position = hit.point + surfaceNormal*transform.localScale.y*0.5f;
			}
			else {
				onSurface = false;
			}
			
			transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z);
			transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
			
			rigidbody.velocity = input*runSpeed*(reverseInput ? -1 : 1)*transform.right.normalized;
			//on-surface movement
					
			if(Input.GetButtonDown("Jump"))
			{
				initiateJump();
			}
		}
		else {
			rigidbody.velocity += input*(reverseInput ? -1 : 1)*transform.right.normalized;
			if(!midJump && rigidbody.velocity.magnitude < jumpSpeed) {
				rigidbody.AddForce(-surfaceNormal.normalized*jumpSpeed, ForceMode.Acceleration);
			}
		}	
	}
	
	void OnCollisionEnter(Collision collision) {
		if(!onSurface ) {
			ContactPoint contact = collision.contacts[0];
			RaycastHit hit;
			if(!(midJump && Vector3.Angle(contact.normal, transform.up) < minTransferAngle) &&
				Physics.Raycast(transform.position, -contact.normal, out hit)) 
			{
				Vector3 contactNormal = hit.normal;
					
				if(Vector3.Angle(surfaceNormal, contactNormal) > 150f) reverseInput = !reverseInput;
				
				surfaceNormal = contactNormal;
				
				transform.rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
				onSurface = true;
				midJump = false;
			}
		}
		else if(!isOnWall()) {
			surfaceNormal = collision.contacts[0].normal;
		}
	}
	
	void initiateJump() {
		onSurface = false;
		midJump = true;
		Vector3 leanDirection = input * (reverseInput ? -1 : 1) * transform.right;
		if(Physics.Raycast(transform.position, leanDirection, transform.localScale.y)) {
			leanDirection = Vector3.zero;	
		}
		jumpVector = (surfaceNormal + leanDirection).normalized*jumpSpeed;
		rigidbody.velocity = jumpVector;
	}
	
	bool isOnWall() {
		return (Physics.Raycast(transform.position, -transform.right, transform.localScale.y) ||
				Physics.Raycast(transform.position, transform.right, transform.localScale.y));
	}

}