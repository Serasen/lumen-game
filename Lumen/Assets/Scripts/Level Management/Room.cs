using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
	
	public GameObject[] spawnPoints;
	public GameObject[] keyholes;
	
	public GameObject iloPrefab;
	GameObject iloInstance;
	
	Level myLevel;
	int latestSpawnPoint;
	
	void Start() {
		myLevel = transform.parent.GetComponent<Level>();
		for(int i = 0; i < spawnPoints.Length; i++) {
			spawnPoints[i].GetComponent<SpawnPoint>().setSpawnPointNum(i);	
		}
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
			iloInstance = (GameObject) GameObject.Instantiate(iloPrefab, spawn.position, spawn.rotation);
			iloInstance.transform.parent = transform;	
		}
		else {
			iloInstance.transform.position = spawn.position;
			iloInstance.transform.rotation = spawn.rotation;
		}
		latestSpawnPoint = point;
		gameObject.SetActive(true);
	}
	
	public void reachedKeyhole(int keyhole) {
		gameObject.SetActive(false);
		myLevel.changeRoom(keyhole);
	}
}
