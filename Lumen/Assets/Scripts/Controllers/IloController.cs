using UnityEngine;
using System.Collections;

public class IloController : MonoBehaviour {
	
	Vector3 surfaceNormal; //normal of surface currently occupied
	Vector3 jumpVector;
	
	Vector2 textureScale = new Vector2(1,1);
	
	//float input;
	bool reverseHorizontalInput;
	bool reverseVerticalInput;
	
	public float runSpeed;
	public float jumpSpeed;
	
	IloAudio audioController;
	
	enum STATE {
		WALKING,
		JUMPING,
		JUMPING_FROM_WALL,
		FALLING,
		REFLECTING
	}
	
	int state;
	
	//If angle of surface to descend is greater than this, slide off
	const float maxDescentAngle = 60f;
	//cannot jump up to a surface angle this close to original surface
	const float minTransferAngle = 15f;
	
	void Start() {
		audioController = GetComponent<IloAudio>();
	}

	// Use this for initialization
	void OnEnable () {
		state = (int)STATE.WALKING;
		reverseHorizontalInput = false;
		reverseVerticalInput = false;
		
		surfaceNormal = transform.up;
		jumpVector = -transform.up;
		
	}
	
	void OnDisable () {
		rigidbody.velocity = Vector3.zero;
	}
	
	float GetInput() {
		float input;
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		if(verticalInput == 0) {
			reverseVerticalInput = transform.right.y < 0;
		}
		if(horizontalInput == 0) {
			reverseHorizontalInput = Vector3.Angle(transform.up, Vector3.up) > 95f;
			input = verticalInput * (reverseVerticalInput ? -1 : 1);
		}
		else {
			input = horizontalInput * (reverseHorizontalInput ? -1 : 1);
		}
		if(input > 0) {
			textureScale.Set (1,1);
			renderer.material.SetTextureScale("_MainTex", textureScale);
		}
		else if (input < 0) {
			textureScale.Set(-.85f,1);
			renderer.material.SetTextureScale("_MainTex", textureScale);
		}
		return input;
	}
	
	void Update() {
		switch(state) {
			case (int)STATE.WALKING: Walk_Update(); break;
			case (int)STATE.JUMPING: Jump_Update(); break;
			case (int)STATE.JUMPING_FROM_WALL: break;
			case (int)STATE.FALLING: Fall_Update(); break;
			case (int)STATE.REFLECTING: Reflect_Update(); break;
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		switch(state) {
			case (int)STATE.WALKING: Walk_OnCollisionEnter(collision); break;
			case (int)STATE.JUMPING_FROM_WALL:
			case (int)STATE.JUMPING: Jump_OnCollisionEnter(collision); break;
			case (int)STATE.FALLING: Fall_OnCollisionEnter(collision); break;
			case (int)STATE.REFLECTING: Jump_OnCollisionEnter(collision); break;
		}
	}
	
	public bool isJumping() {
		return (state == (int)STATE.JUMPING);
	}
	
	public bool isMoving() {
		return (rigidbody.velocity != Vector3.zero);
	}
	
	#region walking
	
	public void initiateWalk() {
		state = (int)STATE.WALKING;
	}
	
	void Walk_Update() {
		float input = GetInput();
		
		RaycastHit hit;
		if(Physics.Raycast(transform.position, -surfaceNormal, out hit, transform.localScale.y*2) &&
			Vector3.Angle(surfaceNormal, hit.normal) < maxDescentAngle) 
		{
			surfaceNormal = hit.normal;
			transform.up = Vector3.Lerp(transform.up, surfaceNormal, 10*Time.deltaTime);	
			transform.position = hit.point + surfaceNormal*transform.localScale.y*0.5f;
		}
		else {
			state = (int)STATE.FALLING;
		}
			
		transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z);
		transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
		rigidbody.velocity = input*runSpeed*transform.right.normalized;			
		if(Input.GetButtonDown("Jump"))
		{
			initiateJump();
		}		
	}
	
	void Walk_OnCollisionEnter(Collision collision) {
		RaycastHit hit;
		if(Physics.Raycast(transform.position, -transform.right, out hit, transform.localScale.y) ||
			Physics.Raycast(transform.position, transform.right, out hit, transform.localScale.y)) {
			if(Vector3.Angle(hit.normal, surfaceNormal) < 60f) {
				surfaceNormal = collision.contacts[0].normal;
				transform.position = (hit.point + collision.contacts[0].point)/2 + surfaceNormal*transform.localScale.y*0.5f;
			}
		}
	}
	#endregion
	
	#region jumping
	
		
	public void initiateJump() {
		audioController.playClip((int)Audio.JUMP_BEGIN);
		
		Vector3 leanDirection = GetInput() * transform.right;
		if(Physics.Raycast(transform.position, leanDirection, transform.localScale.y*0.5f)) {
			jumpVector = surfaceNormal;
			state = (int)STATE.JUMPING_FROM_WALL;
		}
		else {
			jumpVector = surfaceNormal + leanDirection;
			state = (int)STATE.JUMPING;
		}
		jumpVector = jumpVector.normalized*jumpSpeed;
		rigidbody.velocity = jumpVector;
	}
	
	void Jump_Update() {
		float input = GetInput();
		rigidbody.velocity += input*transform.right.normalized;
		jumpVector = rigidbody.velocity;		
	}
	
	void Jump_OnCollisionEnter(Collision collision) {
		RaycastHit hit;
		ContactPoint contact = collision.contacts[0];
		if(Vector3.Angle(contact.normal, transform.up) >= minTransferAngle &&
			Physics.Raycast(transform.position, (contact.point - transform.position), out hit)) 
		{
			Vector3 contactNormal = hit.normal;
			if(Vector3.Angle(surfaceNormal, contactNormal) > 150f) {
				reverseHorizontalInput = !reverseHorizontalInput;
				reverseVerticalInput = !reverseVerticalInput;
			}
			if(collision.gameObject.tag != "Mirror") {
				surfaceNormal = contactNormal;
				transform.rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
				initiateWalk();
			}
			else {	
				initiateReflect(hit.normal);
			}			
		}	
	}
	#endregion
	
	#region falling
	void Fall_Update() {
		float input = GetInput();
		Vector3 inputDirection = input*transform.right.normalized;
		rigidbody.velocity += inputDirection;
			
		if(rigidbody.velocity.magnitude < jumpSpeed) {
			rigidbody.AddForce(-surfaceNormal.normalized*jumpSpeed, ForceMode.Acceleration);
		}
		jumpVector = rigidbody.velocity;		
	}
	
	void Fall_OnCollisionEnter(Collision collision) {
		RaycastHit hit;
		ContactPoint contact = collision.contacts[0];
		if(Physics.Raycast(transform.position, (contact.point - transform.position), out hit)) {
			Vector3 contactNormal = hit.normal;
			if(Vector3.Angle(surfaceNormal, contactNormal) > 150f) {
				reverseHorizontalInput = !reverseHorizontalInput;
				reverseVerticalInput = !reverseVerticalInput;
			}
			if(collision.gameObject.tag != "Mirror") {
				surfaceNormal = contactNormal;
				transform.rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
				reverseHorizontalInput = Vector3.Angle(transform.up, Vector3.up) > 95f;
				reverseVerticalInput = transform.right.y < 0;
				initiateWalk();
			}
			else {	
				initiateReflect(hit.normal);
			}
		}		
	}
	#endregion
	
	#region reflecting
	
	public void initiateReflect(Vector3 hitNormal) {
		audioController.playClip((int)Audio.JUMP_BEGIN);
		surfaceNormal = hitNormal;
		transform.up = surfaceNormal;
		jumpVector = Vector3.Reflect(-jumpVector, transform.right).normalized*jumpSpeed;
		rigidbody.velocity = jumpVector;
		transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z);
		state = (int)STATE.REFLECTING;
	}
	
	void Reflect_Update() {
		float input = GetInput();
		Vector3 inputDirection = input*transform.right.normalized;
		rigidbody.velocity += inputDirection;
		jumpVector = rigidbody.velocity;		
	}	
	
	#endregion
}