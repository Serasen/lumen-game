using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
	
	protected LevelManager levelManager;
	protected GameObject iloInstance;
	
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
		levelManager = transform.parent.GetComponent<LevelManager>();
		iloInstance = levelManager.getIloInstance();
		OnEnable();
	}
	
	protected virtual void OnEnable() {
		if(iloInstance) {
			setCurrentRoom(0, 0);
		}
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
		if(roomInstances[roomNumber] == null) {
			roomInstances[roomNumber] = (GameObject) GameObject.Instantiate(rooms[roomNumber].room);
			roomInstances[roomNumber].transform.parent = transform;
			roomInstances[roomNumber].SetActive(false);
		}
		currentRoom = roomInstances[roomNumber];
		roomInstances[roomNumber].GetComponent<Room>().enterRoom(spawnPoint);		
	}
	
	//Leave a particular level
	public virtual void changeLevel(int levelToEnter) {
		levelManager.changeLevel(levelToEnter);
	}
	
	public GameObject getIloInstance() {
		return iloInstance;
	}
}
