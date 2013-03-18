using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	Room myRoom;
	GameObject ilo;
	public float followSpeed;
	public bool expandCamera;
	float initialSize;
	IloController controller;
	
	Vector3 iloPosition;
	Vector3 myPosition;
	
	void Start() {
		ilo = Game.instance.levelManager.getIlo();
		controller = ilo.GetComponent<IloController>();	
		OnEnable();
	}
	
		bool lockUp = false;
		bool lockDown = false;
		bool lockLeft = false;
		bool lockRight = false;
	
	void OnEnable() {
		
		isWaitingToExpand = false;
		isReturning = false;
		
		lockUp = false;
		lockDown = false;
		lockLeft = false;
		lockRight = false;
		
		AdjustCameraBounds();
		
		initialSize = camera.orthographicSize;
		if(ilo) {
			if(expandCamera) InvokeRepeating("AdjustCameraSize",0, 1/60f);
		}
	}
	
	void OnDisable() {
		camera.orthographicSize = initialSize;
		CancelInvoke();	
	}
	
	// Update is called once per frame
	void Update () {
		myPosition = transform.position;
		iloPosition = ilo.transform.position;
		
		if(!isIloVisible()) {
			Game.instance.levelManager.getCurrentLevel().getCurrentRoom().reEnterRoom();
		}
		
		Vector3 desiredPosition = new Vector3(iloPosition.x,iloPosition.y, myPosition.z);
		if((lockLeft && desiredPosition.x < myPosition.x) ||
		   (lockRight && desiredPosition.x > myPosition.x))
				desiredPosition.x = myPosition.x;
		if((lockDown && desiredPosition.y < myPosition.y) ||
		   (lockUp && desiredPosition.y > myPosition.y))
				desiredPosition.y = myPosition.y;
		
		transform.position = Vector3.Lerp(myPosition, desiredPosition, Time.deltaTime*followSpeed);
	}
	
	void OnTriggerEnter(Collider collider) {
		Vector3 pos = transform.position;
		float scaleX = transform.localScale.x/2;
		float scaleY = transform.localScale.y/2;
		if(Physics.Raycast(pos,transform.up, scaleY)) {
			lockUp = true;
		}
		if(Physics.Raycast(pos,-transform.up, scaleY)) {
			lockDown = true;	
		}
		if(Physics.Raycast(pos,transform.right, scaleX)) {
			lockRight = true;	
		}
		if(Physics.Raycast(pos,-transform.right, scaleX)) {
			lockLeft = true;	
		}
	}
	
	void OnCollisionExit(Collision collision) {
		ContactPoint contact = collision.contacts[0];
		int x = Mathf.RoundToInt(contact.normal.x);
		int y = Mathf.RoundToInt(contact.normal.y);
		Debug.Log(x + " " + y);
		if(x != 0) {
			if(x > 0) lockLeft = false;
			else lockRight = false;
		}
		else {
			if(y > 0) lockDown = false;
			else lockUp = false;
		}
	}
	
	void OnTriggerExit(Collider collider) {
		Vector3 pos = transform.position;
		float scaleX = transform.localScale.x/2;
		float scaleY = transform.localScale.y/2;
		if(!Physics.Raycast(pos,transform.up, scaleY)) {
			lockUp = false;
		}
		if(!Physics.Raycast(pos,-transform.up, scaleY)) {
			lockDown = false;	
		}
		if(!Physics.Raycast(pos,transform.right, scaleX)) {
			lockRight = false;	
		}
		if(!Physics.Raycast(pos,-transform.right, scaleX)) {
			lockLeft = false;	
		}
	}
	
	#region adjust camera size
	
	bool isWaitingToExpand;
	bool isReturning;
	
	void AdjustCameraSize() {
		
		if(controller.isJumping() && camera.orthographicSize < initialSize*2) {
			if(!isWaitingToExpand) {
				StartCoroutine("IncreaseCameraSize");
			}
		}
		else if(!isReturning){
			StopCoroutine("IncreaseCameraSize");
			isWaitingToExpand = false;
			StartCoroutine("ResetCameraSize");
		}
	}

	IEnumerator IncreaseCameraSize() {
		isWaitingToExpand = true;
		yield return new WaitForSeconds(1);
		isReturning = false;
		StopCoroutine("ResetCameraSize");
		while(controller.isJumping() && camera.orthographicSize < initialSize*2) {
			yield return new WaitForSeconds(1/60f);
			camera.orthographicSize += 0.1f;
		}
	}
	
	IEnumerator ResetCameraSize() {
		isReturning = true;
		while(camera.orthographicSize > initialSize) {
			yield return new WaitForSeconds(1/60f);
			camera.orthographicSize -= 0.2f;
		}	
	}
	
	#endregion
	
	void AdjustCameraBounds() {
		float pixelRatio = (camera.orthographicSize * 2) / camera.pixelHeight;
		transform.localScale = new Vector3(pixelRatio*Screen.width, pixelRatio*Screen.height, 10);
	}
	
	bool isIloVisible() {
		Vector3 viewPos = camera.WorldToViewportPoint(ilo.transform.position);
		bool isVisible = true;
		if((lockLeft && viewPos.x < 0) || 
			(lockRight && viewPos.x > 1) ||
			(lockDown && viewPos.y < 0) ||
			(lockUp && viewPos.y > 1)) {
			isVisible = false;
		}
		return isVisible;
	}
}
