using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public static LevelManager instance;
	
	public GameObject[] levels;
	GameObject[] levelInstances;
	
	GameObject currentLevel;
	int levelNumber;
	
	public GameObject iloPrefab;
	
	GameObject iloInstance;
	
	void Awake () {
		iloInstance = (GameObject) Instantiate(iloPrefab);
		instance = this;
	}
	
	void Start () {
		levelInstances = new GameObject[levels.Length];
		levelNumber = 0;
		changeLevel(0);
	}
	
	public void changeLevel(int level) {
		
		if(currentLevel != null) {
			if(levelNumber == 0) ((LevelHub) getCurrentLevel()).setLevelEntered(level);
			currentLevel.SetActive(false);
		}
		
		levelBehavior = null;
		currentCamera = null;
		
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
	
	Level levelBehavior;
	public Level getCurrentLevel() {
		if(levelBehavior == null) {
			levelBehavior = currentLevel.GetComponent<Level>();
		}
		return levelBehavior;
	}
	
	Camera currentCamera;
	public Camera getCamera() {
		return getCurrentLevel().getCurrentRoom().mainCamera.camera;
	}
	
	public GameObject getIlo() {
		return iloInstance;
	}
}
