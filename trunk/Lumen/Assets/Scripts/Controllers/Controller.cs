using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
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
	
	bool locked = false;

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
			rigidbody.velocity = Input.GetAxis("Horizontal")*runSpeed*transform.right.normalized; 
			//rigidbody.AddRelativeForce(new Vector3(Input.GetAxis("Horizontal"), 0, 0),ForceMode.Impulse);	
			
			if(Input.GetButtonDown("Jump")) {
				onSurface = false;
			}
		}
		else {
			if(!locked) {
				if(Physics.Raycast(transform.position, surfaceNormal, out hit)) {
					locked = true;
					jumpDirection = hit.point - transform.position;
					jumpDistance = jumpDirection.magnitude - transform.localScale.y*0.5f;
					angleToRotate = Vector3.Angle(transform.up, hit.normal) * Mathf.Deg2Rad;
					rigidbody.velocity = jumpDirection.normalized*jumpSpeed;
					StartCoroutine("Do");
				}
				else {
					rigidbody.velocity = surfaceNormal.normalized*jumpSpeed;
					angleToRotate = 0f;
				}
			}
			transform.RotateAround(Vector3.forward, angleToRotate*Time.deltaTime/(jumpDistance/jumpSpeed));
		}
		//Prevent player from flipping in Z when completely upside down
		transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z);
		
		
	}
	
	IEnumerator Do() {
        yield return new WaitForSeconds(jumpDistance/jumpSpeed);
		surfaceNormal = transform.up;
        onSurface = true;
		locked = false;
    }

}