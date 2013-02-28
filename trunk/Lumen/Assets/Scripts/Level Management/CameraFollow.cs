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
		myRoom = transform.parent.GetComponent<Room>();
		ilo = myRoom.getIloInstance();
		controller = ilo.GetComponent<IloController>();		
		OnEnable();
	}
	
	void OnEnable() {
		if(ilo) {
			initialSize = camera.orthographicSize;
			if(expandCamera) InvokeRepeating("AdjustCameraSize",0, 1/60f);
		}
	}
	
	void OnDisable() {
		CancelInvoke();	
	}
	
	// Update is called once per frame
	void Update () {
		myPosition = transform.position;
		iloPosition = ilo.transform.position;
		Vector3 desiredPosition = new Vector3(iloPosition.x,iloPosition.y, myPosition.z);
		transform.position = Vector3.Lerp(myPosition, desiredPosition, Time.deltaTime*followSpeed);
	}
	
	void AdjustCameraSize() {
		if(controller.isJumping() && camera.orthographicSize < initialSize*2) {
			CancelInvoke();
			InvokeRepeating("IncreaseCameraSize", 1f, 1/60f);
		}
		else  {
			Invoke("ResetCameraSize", 0);
		}
	}
	
	void IncreaseCameraSize() {
		if(controller.isJumping() && camera.orthographicSize < initialSize*2) {
			camera.orthographicSize += 0.1f;
		}
		else {
			CancelInvoke();
			InvokeRepeating("AdjustCameraSize", 0, 1/60f);
		}
	}
	
	void ResetCameraSize() {
		if(camera.orthographicSize > initialSize) {
			camera.orthographicSize -= 0.1f;
		}
		else {
			CancelInvoke("ResetCameraSize");	
		}
	}
}
