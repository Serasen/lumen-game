using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
	
	public SpawnPoint[] spawnPoints;
	
	public GameObject[] keyholes;
	bool[] reachedKeyholes; //for serialization
	
	[System.Serializable]
	public class SpawnPoint {
		public GameObject spawnPoint;
		public GameObject cameraStartPosition;
	}
	
	public GameObject mainCamera;
	
	GameObject iloInstance;
	int latestSpawnPoint;
	
	
	void Start() {
		int i = 0;
		reachedKeyholes = new bool[keyholes.Length];
		
		foreach(GameObject g in keyholes) {
			g.GetComponent<Keyhole>().setKeyholeNum(i++);	
		}
	}
	
	void OnEnable() {
		iloInstance = Game.instance.levelManager.getIlo();
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
	
	public void keyholeReached(int keyhole) {
		updateReachedKeyhole(keyhole);
		gameObject.SetActive(false);
		Game.instance.levelManager.getCurrentLevel().changeRoom(keyhole);
		
	}
	
	public void levelKeyholeReached(int keyhole, int levelToEnter) {
		updateReachedKeyhole(keyhole);
		latestSpawnPoint = 0;
		gameObject.SetActive(false);
		Game.instance.levelManager.changeLevel(levelToEnter);
	}
	
	public void updateReachedKeyhole(int keyhole) {
		reachedKeyholes[keyhole] = true;
	}
}
