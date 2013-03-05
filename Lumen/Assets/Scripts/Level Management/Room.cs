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
	
	
	void Start() {
		for(int i = 0; i < keyholes.Length; i++) {
			keyholes[i].GetComponent<Keyhole>().setKeyholeNum(i);	
		}
	}
	
	void OnEnable() {
		iloInstance = LevelManager.instance.getIlo();
	}
	
	public void reEnterRoom() {
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
	
	public void reachedKeyhole(int keyhole) {
		gameObject.SetActive(false);
		LevelManager.instance.getCurrentLevel().changeRoom(keyhole);
	}
	
	public void reachedLevelKeyhole(int levelToEnter) {
		latestSpawnPoint = 0;
		gameObject.SetActive(false);
		LevelManager.instance.changeLevel(levelToEnter);
	}
}
