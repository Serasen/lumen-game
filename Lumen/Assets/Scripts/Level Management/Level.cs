using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {
	
	protected LevelManager levelManager;
	
	public RoomInfo[] rooms;
	public GameObject[] friends;
	protected GameObject[] roomInstances;
	
	Dictionary<string, int> friendMappings;

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
	

	GameObject currentRoom;
	int roomNumber;
	Room roomBehavior;
	LevelData levelData;
	
	protected void Awake () {
		roomInstances = new GameObject[rooms.Length];
		friendMappings = new Dictionary<string, int>(friends.Length);
		for(int i = 0; i < friends.Length; i++) {
			friendMappings.Add(friends[i].gameObject.name, i);
		}
	}
	
	protected virtual void OnEnable() {
		initializeLevelData();
		setCurrentRoom(0, 0);
	}
	
	public void initializeLevelData() {
		levelData = Game.instance.dataManager.GetLevelData();
		if(levelData == null || levelData.rooms == null) {
			levelData = new LevelData(rooms.Length, friends.Length);
			Game.instance.dataManager.SetLevelData(levelData);
		}		
	}
	
	#region between-room movement
	
	public void changeRoom(int keyhole) {
		RoomMapping mapping = rooms[roomNumber].mappings[keyhole];
		for(int i = 0; i < rooms.Length; i++) {
			if(rooms[i].room == mapping.destRoom) {
				setCurrentRoom(i, mapping.destSpawnPoint);
				break;
			}
		}
	}
	
	//Set all state variables and enter room
	public void setCurrentRoom(int number, int spawnPoint) {
		//change pointers
		Game.instance.dataManager.ChangeRoom(number);
		roomNumber = number;
		roomBehavior = null;
		if(roomInstances[roomNumber] == null) {
			//Instantiate room
			roomInstances[roomNumber] = (GameObject) GameObject.Instantiate(rooms[roomNumber].room);
			roomInstances[roomNumber].transform.parent = transform;
			roomInstances[roomNumber].SetActive(false);
		}
		currentRoom = roomInstances[roomNumber];
		getCurrentRoom().enterRoom(spawnPoint);		
	}
	
	#endregion

	public Room getCurrentRoom() {
		if(roomBehavior == null) {
			roomBehavior = currentRoom.GetComponent<Room>();
		}
		return roomBehavior;
	}
	
	public int getCurrentRoomNumber() {
		return roomNumber;	
	}
	
	//Returns whether a friend has been discovered yet
	public bool getFriendStatus(string name) {
		return levelData.friendsSaved[friendMappings[name]];
	}
	
	public void savedFriend(string name) {
		levelData.friendsSaved[friendMappings[name]] = true;	
	}
}
