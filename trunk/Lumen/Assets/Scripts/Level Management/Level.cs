using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
	
	protected LevelManager levelManager;
	
	public RoomInfo[] rooms;
	protected GameObject[] roomInstances;
	
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
	
	protected void Awake () {
		roomInstances = new GameObject[rooms.Length];
	}
	
	protected virtual void Start() {
		levelManager = Game.instance.levelManager;
	}
	
	protected virtual void OnEnable() {
		setCurrentRoom(0, 0);
	}
	
	public void changeRoom(int keyhole) {
		RoomMapping mapping = rooms[roomNumber].mappings[keyhole];
		for(int i = 0; i < rooms.Length; i++) {
			if(rooms[i].room == mapping.destRoom) {
				setCurrentRoom(i, mapping.destSpawnPoint);
				break;
			}
		}
	}
	
	public void setCurrentRoom(int number, int spawnPoint) {
		roomNumber = number;
		roomBehavior = null;
		
		if(roomInstances[roomNumber] == null) {
			roomInstances[roomNumber] = (GameObject) GameObject.Instantiate(rooms[roomNumber].room);
			roomInstances[roomNumber].transform.parent = transform;
			roomInstances[roomNumber].SetActive(false);
		}
		currentRoom = roomInstances[roomNumber];
		getCurrentRoom().enterRoom(spawnPoint);		
	}
	
	Room roomBehavior;
	public Room getCurrentRoom() {
		if(roomBehavior == null) {
			roomBehavior = currentRoom.GetComponent<Room>();
		}
		return roomBehavior;
	}
}
