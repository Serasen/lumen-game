using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	Room myRoom;
	GameObject ilo;
	public float followDelay;
	float initialSize;
	IloController controller;
	
	Vector3 iloPosition;
	Vector3 myPosition;
	
	void Start() {
		myRoom = transform.parent.GetComponent<Room>();
		ilo = myRoom.getIloInstance();
		controller = ilo.GetComponent<IloController>();		
		initialSize = camera.orthographicSize;
		InvokeRepeating("AdjustCameraSize",0, 1/60f);
	}
	
	void OnEnable() {
		if(ilo) {
			Vector3 spawnPosition = myRoom.getLatestSpawnPoint().transform.position;
			transform.position = new Vector3(spawnPosition.x,spawnPosition.y, transform.position.z);
			
			initialSize = camera.orthographicSize;
			InvokeRepeating("AdjustCameraSize",0, 1/60f);
		}
	}
	
	void OnDisable() {
		if(myRoom) {
		}
		CancelInvoke();	
	}
	
	// Update is called once per frame
	void Update () {
		myPosition = transform.position;
		iloPosition = ilo.transform.position;
		Vector3 desiredPosition = new Vector3(iloPosition.x,iloPosition.y, myPosition.z);
		transform.position = Vector3.Lerp(myPosition, desiredPosition, Time.deltaTime*followDelay);
	}
	
	void AdjustCameraSize() {
		if(controller.isJumping()) {
			Invoke("IncreaseCameraSize", 1f);
		}
		else {
			CancelInvoke("IncreaseCameraSize");
			
			if(camera.orthographicSize != initialSize) {
				CancelInvoke("AdjustCameraSize");
				InvokeRepeating("ResetCameraSize", 0, 1/60f);
			}
		}
	}
	
	void IncreaseCameraSize() {
			camera.orthographicSize += 0.1f;
	}
	
	void ResetCameraSize() {
		if(camera.orthographicSize > initialSize) {
			camera.orthographicSize -= 0.2f;	
		}
		else {
			CancelInvoke();
			camera.orthographicSize = initialSize;
			InvokeRepeating("AdjustCameraSize", 0, 1/60f);
		}
	}
}
