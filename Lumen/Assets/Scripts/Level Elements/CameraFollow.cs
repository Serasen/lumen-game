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
	
	bool locked;
	bool lockUp;
	bool lockDown;
	bool lockLeft;
	bool lockRight;
	
	void Start() {
		ilo = Game.instance.levelManager.getIlo();
		controller = ilo.GetComponent<IloController>();
		OnEnable();
	}
	
	void OnEnable() {
		isWaitingToExpand = false;
		isReturning = false;
		
		locked = false;
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
		
		if(!isIloVisible() && Game.instance.gameState != (int)GameState.GRADUAL_PAUSE) {
			Game.instance.RoomTransition((int)LevelActions.REENTER_ROOM);
		}
		
		Vector3 desiredPosition = new Vector3(iloPosition.x,iloPosition.y, myPosition.z);
		if(locked) {
			if(	(lockLeft && desiredPosition.x < myPosition.x) ||
		   		(lockRight && desiredPosition.x > myPosition.x))
					desiredPosition.x = myPosition.x;
			if((lockDown && desiredPosition.y < myPosition.y) ||
		   		(lockUp && desiredPosition.y > myPosition.y))
					desiredPosition.y = myPosition.y;
		}
		transform.position = Vector3.Lerp(myPosition, desiredPosition, Time.deltaTime*followSpeed);
	}
	
	void OnTriggerEnter(Collider collider) {
		Vector3 pos = transform.position;
		float scaleX = transform.localScale.x/2;
		float scaleY = transform.localScale.y/2;
		if(Physics.Raycast(pos,transform.up, scaleY)) {
			lockUp = true;
			locked = true;
		}
		if(Physics.Raycast(pos,-transform.up, scaleY)) {
			lockDown = true;
			locked = true;
		}
		if(Physics.Raycast(pos,transform.right, scaleX)) {
			lockRight = true;
			locked = true;
		}
		if(Physics.Raycast(pos,-transform.right, scaleX)) {
			lockLeft = true;
			locked = true;
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
		if(!lockLeft && !lockRight && !lockUp && !lockDown) locked = false;
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
		if(locked && 
			((viewPos.x < 0) || (viewPos.x > 1) ||
			(viewPos.y < 0) || (viewPos.y > 1))) {
			isVisible = false;
		}
		return isVisible;
	}
}
