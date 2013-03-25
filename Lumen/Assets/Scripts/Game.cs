using UnityEngine;
using System.Collections;

enum GameState {
		PLAY,
		PAUSE
}

public class Game : MonoBehaviour {
	public static Game instance;
	
	public LevelManager levelManager;
	public DataManager dataManager;
	public GameObject frontPlanePrefab;
	
	GameObject frontPlane;
	PauseMenu pauseMenu;
	
	[System.NonSerialized] public int gameState;
	
	void Start() {
		instance = this;
		dataManager = new DataManager();
		levelManager.initialize();
		pauseMenu = GetComponent<PauseMenu>();
		frontPlane = (GameObject) Instantiate(frontPlanePrefab);
		frontPlane.renderer.material.color = Color.clear;
	}
	
	void Update() {
		if(Input.GetKey(KeyCode.P)) {
			Pause();
			pauseMenu.enabled = true;
		}
	}
	
	public void Pause() {
		Time.timeScale = 0;
		gameState = (int)GameState.PAUSE;
		frontPlane.renderer.material.color = Color.black;
	}
	
	public void Unpause() {
		Time.timeScale = 1;
		gameState = (int)GameState.PLAY;
		frontPlane.renderer.material.color = Color.clear;
	}
}
