using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
	
	public SpawnPoint[] spawnPoints;
	public GameObject[] keyholes;
	
	[System.Serializable]
	public class SpawnPoint {
		public GameObject spawnPoint;
		public GameObject cameraStartPosition;
	}
	
	public GameObject mainCamera;

	GameObject iloInstance;
	int latestSpawnPoint;
	RoomData roomData;
	
	void Start() {
		int i = 0;
		foreach(GameObject g in keyholes) {
			g.GetComponent<Keyhole>().setKeyholeNum(i++);	
		}
	}
	
	void OnEnable() {
		//initialize room data
		RoomData tempData = Game.instance.dataManager.GetRoomData();
		if(tempData == null) {
			roomData = new RoomData(keyholes.Length);
			Game.instance.dataManager.SetRoomData(roomData);
		}
		else {
			roomData = new RoomData(tempData);	
		}
		
		iloInstance = Game.instance.levelManager.getIlo();
	}
	
	public void reEnterRoom() {
		
		roomData.deaths++;
		Game.instance.dataManager.SetRoomData(roomData);
		
		gameObject.SetActive(false);
		enterRoom(latestSpawnPoint);
	}
	
	public void enterRoom(int point) {
		Transform spawn = spawnPoints[point].spawnPoint.transform;
		Vector3 cameraPoint = spawnPoints[point].cameraStartPosition.transform.position;
		
		iloInstance.transform.position = spawn.position;
		iloInstance.transform.rotation = spawn.rotation;		
		iloInstance.transform.parent = this.transform;
		mainCamera.transform.position = new Vector3(cameraPoint.x, cameraPoint.y, mainCamera.transform.position.z);		
		latestSpawnPoint = point;
		gameObject.SetActive(true);
	}
	
	public void keyholeReached(int keyhole) {
		roomData.keyholes[keyhole] = true;
		Game.instance.dataManager.SetRoomData(roomData);

		Game.instance.RoomTransition((int)LevelActions.CHANGE_ROOM, keyhole);
		
	}
	
	public void levelKeyholeReached(int keyhole, int levelToEnter) {
		roomData.keyholes[keyhole] = true;
		Game.instance.dataManager.SetRoomData(roomData);
		
		latestSpawnPoint = 0;
		Game.instance.RoomTransition((int)LevelActions.CHANGE_LEVEL, levelToEnter);
	}
}
