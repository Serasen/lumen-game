using UnityEngine;
using System.Collections;

public class IloController : MonoBehaviour {
	
	bool onSurface;
	bool midJump;
	bool jumpFromWall;
	
	Vector3 surfaceNormal; //normal of surface currently occupied
	Vector3 jumpVector;
	
	float input;
	bool reverseHorizontalInput;
	bool reverseVerticalInput;
	
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
		jumpFromWall = false;
		reverseHorizontalInput = false;
		reverseVerticalInput = false;
		
		surfaceNormal = transform.up;
		jumpVector = -transform.up;
		
	}
	
	float GetInput() {
		float input;
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		if(horizontalInput == 0) {
			reverseHorizontalInput = Vector3.Angle(transform.up, Vector3.up) > 95f;
			if(verticalInput == 0) {
				reverseVerticalInput = transform.right.y < 0;
				input = 0;
			}
			else {
				input = verticalInput * (reverseVerticalInput ? -1 : 1);
			}
		}
		else {
			input = horizontalInput * (reverseHorizontalInput ? -1 : 1);
		}
		return input;
	}

	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		input = GetInput();
		
		if(onSurface) {
			
			if(Physics.Raycast(transform.position, -surfaceNormal, out hit, transform.localScale.y*2) &&
				Vector3.Angle(surfaceNormal, hit.normal) < maxDescentAngle) 
			{
				//Debug.Log("hit surface!");
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
			
			rigidbody.velocity = input*runSpeed*transform.right.normalized;
			//on-surface movement
					
			if(Input.GetButtonDown("Jump"))
			{
				initiateJump();
			}
		}
		else {
			Vector3 inputDirection = input*transform.right.normalized;
			if(!jumpFromWall) rigidbody.velocity += inputDirection;
			
			if(!midJump && rigidbody.velocity.magnitude < jumpSpeed) {
				rigidbody.AddForce(-surfaceNormal.normalized*jumpSpeed, ForceMode.Acceleration);
			}
		}	
	}
	
	void OnCollisionEnter(Collision collision) {
		RaycastHit hit;
		if(!onSurface ) {
			ContactPoint contact = collision.contacts[0];
			
			if(Physics.Raycast(transform.position, (contact.point - transform.position), out hit) &&
				!(midJump && Vector3.Angle(hit.normal, transform.up) < minTransferAngle)) 
			{
				Vector3 contactNormal = hit.normal;
				if(Vector3.Angle(surfaceNormal, contactNormal) > 150f) {
					reverseHorizontalInput = !reverseHorizontalInput;
					reverseVerticalInput = !reverseVerticalInput;
				}
				
				surfaceNormal = contactNormal;
				
				transform.rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
				
				if(!midJump) {
					reverseHorizontalInput = Vector3.Angle(transform.up, Vector3.up) > 95f;
					reverseVerticalInput = transform.right.y < 0;
				}
				
				onSurface = true;
				midJump = false;
				jumpFromWall = false;
				
			}
		}
		else if(Physics.Raycast(transform.position, -transform.right, out hit, transform.localScale.y) ||
			Physics.Raycast(transform.position, transform.right, out hit, transform.localScale.y)) {
			if(Vector3.Angle(hit.normal, surfaceNormal) < 60f) {
				surfaceNormal = collision.contacts[0].normal;
				transform.position = (hit.point + collision.contacts[0].point)/2 + surfaceNormal*transform.localScale.y*0.5f;
			}
		}
	}
	
	void initiateJump() {
		onSurface = false;
		midJump = true;
		Vector3 leanDirection = GetInput() * transform.right;
		if(Physics.Raycast(transform.position, leanDirection, transform.localScale.y*0.5f)) {
			jumpFromWall = true;
		}
		jumpVector = (surfaceNormal + (jumpFromWall ? Vector3.zero : leanDirection)).normalized*jumpSpeed;
		rigidbody.velocity = jumpVector;
	} 
}