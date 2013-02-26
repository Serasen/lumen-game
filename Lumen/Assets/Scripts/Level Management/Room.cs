using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
	
	public GameObject[] spawnPoints;
	public GameObject[] keyholes;
	
	public class SpawnPoint {
		public GameObject spawnPoint;
		public GameObject cameraStartPosition;
		public void placeAt() {
		}
	}
	
	
	public GameObject iloPrefab;
	GameObject iloInstance;
	
	Level myLevel;
	int latestSpawnPoint;
	
	void Start() {
		
		myLevel = transform.parent.GetComponent<Level>();
		iloInstance = myLevel.getIloInstance();
		for(int i = 0; i < keyholes.Length; i++) {
			keyholes[i].GetComponent<Keyhole>().setKeyholeNum(i);	
		}
	}
	
	public void reEnterRoom() {
		gameObject.SetActive(false);
		enterRoom(latestSpawnPoint);
	}
	
	public void enterRoom(int point) {
		Transform spawn = spawnPoints[point].transform;
		if(iloInstance == null) {
			myLevel = transform.parent.GetComponent<Level>();
			iloInstance = myLevel.getIloInstance();
		}
		
		iloInstance.transform.position = spawn.position;
		iloInstance.transform.rotation = spawn.rotation;		
		iloInstance.transform.parent = this.transform;
		
		latestSpawnPoint = point;
		gameObject.SetActive(true);
	}
	
	public void reachedKeyhole(int keyhole) {
		gameObject.SetActive(false);
		myLevel.changeRoom(keyhole);
	}
	
	public GameObject getIloInstance() {
		return iloInstance;
	}
	
	public GameObject getLatestSpawnPoint() {
		return spawnPoints[latestSpawnPoint];
	}
}
