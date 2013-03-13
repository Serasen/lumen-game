using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelManager {
	public GameObject[] levels;
	GameObject[] levelInstances;
	
	GameObject currentLevel;
	int levelNumber;
	
	public GameObject iloPrefab;
	
	GameObject iloInstance;
	
	public void initialize() {
		iloInstance = (GameObject) GameObject.Instantiate(iloPrefab);

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
			currentLevel.transform.parent = Game.instance.transform;
		}
		else {
			currentLevel = levelInstances[levelNumber];
		}
		/*
		 *player stats should sync here! 
		 * 
		 */
		
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
		if(currentCamera == null) {
			currentCamera = getCurrentLevel().getCurrentRoom().mainCamera.camera;	
		}
		return currentCamera;
	}
	
	public GameObject getIlo() {
		return iloInstance;
	}
}
