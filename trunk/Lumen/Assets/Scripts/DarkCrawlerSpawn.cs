using UnityEngine;
using System.Collections;

public class DarkCrawlerSpawn : MonoBehaviour {
	public GameObject toSpawn;
	GameObject spawnInstance;
	
	// Use this for initialization
	void Start () {
	}
	
	void OnEnable() {
		if(spawnInstance == null) {
			spawnInstance = (GameObject) GameObject.Instantiate(toSpawn, transform.position, transform.rotation);
			spawnInstance.transform.parent = transform.parent;
		}
		else {
			spawnInstance.transform.position = transform.position;
			spawnInstance.transform.rotation = transform.rotation;
		}
	}
}
