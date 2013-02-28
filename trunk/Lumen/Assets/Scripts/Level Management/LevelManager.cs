using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public GameObject[] levels;
	GameObject[] levelInstances;
	
	GameObject currentLevel;
	int levelNumber;
	
	public GameObject iloPrefab;
	
	GameObject iloInstance;
	
	void Awake () {
		iloInstance = (GameObject) Instantiate(iloPrefab);
	}
	
	void Start () {
		levelInstances = new GameObject[levels.Length];
		levelNumber = 0;
		changeLevel(0);
	}
	
	public void changeLevel(int level) {
		if(currentLevel != null) currentLevel.SetActive(false);
		levelNumber = level;
		if(levelInstances[levelNumber] == null) {
			currentLevel = (GameObject) GameObject.Instantiate(levels[levelNumber]); 
			levelInstances[levelNumber] = currentLevel;
			currentLevel.transform.parent = transform;
		}
		else {
			currentLevel = levelInstances[levelNumber];
		}
		currentLevel.SetActive(true);
	}
	
	public GameObject getIloInstance() {
		return iloInstance;
	}
}
