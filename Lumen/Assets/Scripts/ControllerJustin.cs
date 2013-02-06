using UnityEngine;
using System.Collections;

public class ControllerJustin : MonoBehaviour {
	public short moveSpeed = 3;
	public short jumpSpeed = 75;
	private float x, y;
	public Vector3 gravityDir = Vector3.up * -1;
	public bool grounded = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		x = Input.GetAxis("Horizontal");
		//y = Input.GetAxis("Vertical");
		if(Input.GetButtonDown("Jump") && grounded)
			Jump();
		transform.Translate(Vector3.right * x * Time.deltaTime * moveSpeed);
		if(!grounded)
			rigidbody.velocity = (gravityDir * jumpSpeed);
			//transform.Translate(gravityDir * jumpSpeed);
		
	}
	
	private void Jump() {
		gravityDir *= -1;
		grounded = false;
	}
	
	void OnCollisionEnter(Collision collisionInfo) {
		if(!grounded) {
			Vector3 newGravityDir = collisionInfo.contacts[0].normal;
			gravityDir.y = newGravityDir.y * -1;
			gravityDir.x = newGravityDir.x * -1;
		}
		grounded = true;
		rigidbody.velocity = Vector3.zero;
	}
}
