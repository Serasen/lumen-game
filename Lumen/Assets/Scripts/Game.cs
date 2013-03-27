using UnityEngine;
using System.Collections;

enum GameState {
	PLAY,
	GRADUAL_PAUSE,
	PAUSE
}

enum LevelActions {
	CHANGE_ROOM,
	CHANGE_LEVEL,
	REENTER_ROOM
}

public class Game : MonoBehaviour {
	public static Game instance;
	
	public LevelManager levelManager;
	public DataManager dataManager;
	public GameObject frontPlanePrefab;
	[System.NonSerialized] public float frontPlaneOpacity;
	
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
		
		StartCoroutine("StartupFade");
	}
	
	IEnumerator StartupFade() {
		float waitTime = 0.02f;
		
		GameObject iloTemp = levelManager.getIlo();
		
		iloTemp.GetComponent<IloController>().enabled = false;
		iloTemp.GetComponent<IloShine>().enabled = false;
		frontPlaneOpacity = 1f;
		frontPlane.renderer.material.color = new Color(0,0,0,frontPlaneOpacity);
		yield return new WaitForSeconds(1f);
		
		iloTemp.GetComponent<IloController>().enabled = true;
		iloTemp.GetComponent<IloShine>().enabled = true;
		
		while(frontPlaneOpacity > 0) {
			frontPlaneOpacity -= 0.05f;
			frontPlane.renderer.material.color = new Color(0,0,0,frontPlaneOpacity);
			yield return new WaitForSeconds(waitTime);
		}
		frontPlaneOpacity = 0f;
		
		gameState = (int)GameState.PLAY;
	}
	
	void Update() {
		if(Input.GetKey(KeyCode.P) && gameState == (int)GameState.PLAY) {
			Pause();
			pauseMenu.enabled = true;
		}
	}
	
	public void RelocateFrontPlane() {
		Camera camera = levelManager.getCamera();
		float pixelRatio = (camera.orthographicSize * 2) / camera.pixelHeight; 
		frontPlane.transform.position = camera.transform.position + new Vector3(0,0,0.5f);
		frontPlane.transform.localScale = new Vector3(pixelRatio*Screen.width, 0f, pixelRatio*Screen.height);		
	}
	
	#region Pause
	
	public void Pause() {
		Time.timeScale = 0;
		gameState = (int)GameState.PAUSE;
		frontPlaneOpacity = 0.7f;
		frontPlane.renderer.material.color = new Color(0,0,0,frontPlaneOpacity);
		RelocateFrontPlane();
	}
	
	public void Unpause() {
		Time.timeScale = 1;
		gameState = (int)GameState.PLAY;
		frontPlane.renderer.material.color = Color.clear;
		frontPlaneOpacity = 0;
		frontPlane.renderer.material.color = new Color(0,0,0,frontPlaneOpacity);
	}
		
	public void ReturnToTitle() {
		levelManager.ReturnToTitleScreen();
		Unpause();
	}
	
	#endregion
	
	#region Room Transitions
	
	
	public void RoomTransition(int action, int arg = 0) {
		if(gameState != (int)GameState.GRADUAL_PAUSE) {
		gameState = (int)GameState.GRADUAL_PAUSE;
		StartCoroutine(FadePause(action, arg));
		}
	}
	
	IEnumerator FadePause(int action, int arg) {
		
		float waitTime = 0.02f;
		
		GameObject iloTemp = levelManager.getIlo();
		iloTemp.GetComponent<IloController>().enabled = false;
		iloTemp.GetComponent<IloShine>().enabled = false;
		
		RelocateFrontPlane();
		
		while(frontPlaneOpacity < 1) {
			frontPlaneOpacity += 0.1f;
			frontPlane.renderer.material.color = new Color(0,0,0,frontPlaneOpacity);
			yield return new WaitForSeconds(waitTime);
		}
		frontPlaneOpacity = 1f;
		
		levelManager.getCurrentLevel().getCurrentRoom().gameObject.SetActive(false);
		switch(action) {
			case(int) LevelActions.CHANGE_ROOM:
				levelManager.getCurrentLevel().changeRoom(arg);
				break;
			case(int) LevelActions.CHANGE_LEVEL:
				levelManager.changeLevel(arg);
				break;
			case(int) LevelActions.REENTER_ROOM:
				levelManager.getCurrentLevel().getCurrentRoom().reEnterRoom();
				break;
		}
		
		RelocateFrontPlane();
		
		iloTemp.GetComponent<IloController>().enabled = true;
		iloTemp.GetComponent<IloShine>().enabled = true;
		
		while(frontPlaneOpacity > 0) {
			frontPlaneOpacity -= 0.1f;
			frontPlane.renderer.material.color = new Color(0,0,0,frontPlaneOpacity);
			yield return new WaitForSeconds(waitTime);
		}
		frontPlaneOpacity = 0f;
		gameState = (int)GameState.PLAY;
	}
	
	#endregion
}
