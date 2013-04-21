using UnityEngine;
using System.Collections;

public class Blobber : MonoBehaviour {
	private int LEFT = 1;
	private int RIGHT = 2;
	public float minAngle, maxAngle;
	public float speed;
	public float jumpPeriod;
	public GameObject sludge;
	private float sludgeHeight;
	private float myHeight;
	
	void Start () {
		sludgeHeight = sludge.transform.localScale.y;
		myHeight = transform.localScale.y;
	}
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "Player") {
			collision.gameObject.transform.GetComponentInChildren<Light>().range = 0;
			Game.instance.RoomTransition((int)LevelActions.REENTER_ROOM);
		}
		else {
			Vector3 eulerAngles = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal).eulerAngles;
			transform.eulerAngles = new Vector3(0, 0, eulerAngles.z);
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			rigidbody.Sleep();
			StartCoroutine("Jump");
			//audio.Play ();
		}
	}
	
	void OnDisable() {
		rigidbody.velocity = Vector3.zero;
	}
	
	void OnEnable() {
		StartCoroutine("Jump");
	}
	
	IEnumerator Jump() {
		yield return new WaitForSeconds(jumpPeriod);
		audio.Play();
		int dir = Random.Range(LEFT, RIGHT+1);
		if(dir == LEFT) {
			rigidbody.velocity = (transform.up - transform.right*Random.Range(minAngle,maxAngle))*speed;
		}
		else if(dir == RIGHT) {
			rigidbody.velocity = (transform.up + transform.right*Random.Range(minAngle,maxAngle))*speed;
		}
		GameObject blackhole = (GameObject) GameObject.Instantiate(sludge, transform.position /*- transform.up*(myHeight/2-sludgeHeight/2)*/, transform.rotation);
		blackhole.transform.parent = transform.parent;
	}
}
