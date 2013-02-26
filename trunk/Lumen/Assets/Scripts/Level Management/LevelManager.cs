using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public GameObject[] levels;
	
	int levelNumber;
	GameObject currentLevel;
	
	public GameObject iloPrefab;
	
	GameObject iloInstance;
	
	void Awake () {
		iloInstance = (GameObject) Instantiate(iloPrefab);
	}
	
	void Start () {
		levelNumber = 0;
		currentLevel = (GameObject) GameObject.Instantiate(levels[levelNumber]);
		currentLevel.transform.parent = transform;
	}
	
	public void changeLevel() {
		GameObject.Destroy(currentLevel);
		levelNumber++;
		if(levelNumber < levels.Length) {
			currentLevel = (GameObject) GameObject.Instantiate(levels[levelNumber]);
			currentLevel.transform.parent = transform;
		}
	}
	
	public GameObject getIloInstance() {
		return iloInstance;
	}
}
