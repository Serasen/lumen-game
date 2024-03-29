using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelManager {
	
	public GameObject[] levels;
	public GameObject iloPrefab;
	
	GameObject[] levelInstances;
	GameObject iloInstance;
	GameObject currentLevel;
	int levelNumber;
	GameData gameData;
	
	public void initialize() {
		iloInstance = (GameObject) GameObject.Instantiate(iloPrefab);
		initializeGameData();
		levelInstances = new GameObject[levels.Length];
		levelNumber = 0;
		changeLevel(0);
	}
	
	public void initializeGameData() {
		//initialize Game Data content
		gameData = Game.instance.dataManager.GetGameData();
		if(gameData == null) {		
			gameData = new GameData(levels.Length);
			Game.instance.dataManager.SetGameData(gameData);
		}

	}
	
	public void changeLevel(int level) {
		Game.instance.dataManager.ChangeLevel(level);
		if(currentLevel != null) {
			//Tell LevelHub where to return
			if(levelNumber == 0) {
				((LevelHub) getCurrentLevel()).setLevelEntered(level);
			}
			currentLevel.SetActive(false);
		}
		
		levelBehavior = null;
		
		levelNumber = level;
		if(levelInstances[levelNumber] == null) {
			//Instantiate new  level
			currentLevel = (GameObject) GameObject.Instantiate(levels[levelNumber]); 
			levelInstances[levelNumber] = currentLevel;
			currentLevel.transform.parent = Game.instance.transform;
		}
		else {
			currentLevel = levelInstances[levelNumber];
		}
		
		currentLevel.SetActive(true);
	}
	
	public void ReturnToTitleScreen() {
		int level = 0;
		Game.instance.dataManager.ChangeLevel(level);
		if(currentLevel != null) {
			currentLevel.SetActive(false);
		}
		
		levelBehavior = null;
		
		levelNumber = level;
		currentLevel = levelInstances[levelNumber];
		((LevelHub) getCurrentLevel()).setLevelEntered(level);
		currentLevel.SetActive(true);
	}
	
	#region getters 
	Level levelBehavior;
	public Level getCurrentLevel() {
		if(levelBehavior == null) {
			levelBehavior = currentLevel.GetComponent<Level>();
		}
		return levelBehavior;
	}
	
	public int getCurrentLevelNumber() {
		return levelNumber;	
	}
	
	Camera currentCamera;
	public Camera getCamera() {
		currentCamera = getCurrentLevel().getCurrentRoom().mainCamera.camera;	
		return currentCamera;
	}
	
	public GameObject getIlo() {
		return iloInstance;
	}
	#endregion
}
