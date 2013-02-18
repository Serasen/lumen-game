using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
	public GameObject[] rooms;
	GameObject[] roomInstances;
	
	[System.Serializable]
	public class RoomToRoomMapping {
		public int sourceRoom;
		public int sourceKeyhole;
		public int destRoom;
		public int destSpawnPoint;
	}
	
	public RoomToRoomMapping[] roomToRoomMappings;
	
	int roomNumber;
	GameObject currentRoom;

	void Awake () {
		roomInstances = new GameObject[rooms.Length];
		for(int i = 0; i < rooms.Length; ++i) {
			roomInstances[i] = (GameObject) GameObject.Instantiate(rooms[i]);
			roomInstances[i].transform.parent = transform;
			roomInstances[i].SetActive(false);
		}
	}
	
	void Start() {
		roomNumber = 0;
		currentRoom = roomInstances[roomNumber];
		currentRoom.SetActive(true);
		roomInstances[roomNumber].GetComponent<Room>().enterRoom(0);
		
	}
	
	public void changeRoom(int keyhole) {
		foreach(RoomToRoomMapping mapping in roomToRoomMappings) {
			if(mapping.sourceRoom == roomNumber && mapping.sourceKeyhole == keyhole) {
				Debug.Log(mapping.destRoom + " is dest!");
				roomNumber = mapping.destRoom;
				roomInstances[roomNumber].GetComponent<Room>().enterRoom(mapping.destSpawnPoint);
				break;
			}
		}
	}
}
