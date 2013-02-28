using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
	
	LevelManager levelManager;
	GameObject iloInstance;
	
	public RoomInfo[] rooms;
	GameObject[] roomInstances;
	
	[System.Serializable]
	public class RoomInfo {
		public GameObject room;
		public RoomMapping[] mappings;
	}
	
	[System.Serializable]
	public class RoomMapping {
		public GameObject destRoom;
		public int destSpawnPoint;
	}
	
	int roomNumber;
	GameObject currentRoom;
	
	void Awake () {
		roomInstances = new GameObject[rooms.Length];
		for(int i = 0; i < rooms.Length; ++i) {
			roomInstances[i] = (GameObject) GameObject.Instantiate(rooms[i].room);
			roomInstances[i].transform.parent = transform;
			roomInstances[i].SetActive(false);
		}
	}
	
	void Start() {
		levelManager = transform.parent.GetComponent<LevelManager>();
		iloInstance = levelManager.getIloInstance();
		OnEnable();
	}
	
	void OnEnable() {
		if(iloInstance) {
			roomNumber = 0;
			currentRoom = roomInstances[roomNumber];
			currentRoom.SetActive(true);
			roomInstances[roomNumber].GetComponent<Room>().enterRoom(0);
		}
	}
	
	public void changeRoom(int keyhole) {
		RoomMapping mapping = rooms[roomNumber].mappings[keyhole];
		for(int i = 0; i < rooms.Length; i++) {
			if(rooms[i].room == mapping.destRoom) {
				roomNumber = i;
				currentRoom = mapping.destRoom;
				roomInstances[roomNumber].GetComponent<Room>().enterRoom(mapping.destSpawnPoint);
				break;
			}
		}
	}
	
	public void changeLevel(int levelToEnter) {
		roomNumber = 0;
		currentRoom = null;
		levelManager.changeLevel(levelToEnter);
	}
	
	public GameObject getIloInstance() {
		return iloInstance;
	}
}
