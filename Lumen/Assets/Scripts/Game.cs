using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public static Game instance;
	
	public LevelManager levelManager;
	public DataManager dataManager;
	
	// Use this for initialization
	void Start () {
		instance = this;
		dataManager = new DataManager();
		levelManager.initialize();
	}
}
