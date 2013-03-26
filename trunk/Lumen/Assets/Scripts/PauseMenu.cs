using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	
	public GUIInfo[] guiInfos;
	public GUIInfo[] options;
	
	Color defaultColor = Color.gray;
	Color selectedColor = Color.white;
	int optionNum;
	bool inputPressed;
	
	void OnEnabled() {
		inputPressed = false;
		optionNum = 0;
	}
	void OnGUI() {
		GUIStyle newStyle = new GUIStyle();
		newStyle.alignment = TextAnchor.MiddleCenter;
		newStyle.normal.textColor = defaultColor;
		Rect guiRect;
		foreach(GUIInfo info in guiInfos) {
			newStyle.fontSize = (int)info.fontSize;
			Vector2 pos = new Vector2(info.posX*Screen.width/100f, (100-info.posY)*Screen.height/100f);
			guiRect = new Rect(pos.x, pos.y, 0,0);
			GUI.Box(guiRect, info.text, newStyle);
		}
		for(int i = 0; i < options.Length; i++) {
			GUIInfo info = options[i];
			Vector2 pos = new Vector2(info.posX*Screen.width/100f, (100-info.posY)*Screen.height/100f);
			guiRect = new Rect(pos.x, pos.y, 0,0);
			if(optionNum != i) {
				newStyle.normal.textColor = defaultColor;
				newStyle.fontSize = (int)info.fontSize;
			}
			else {
				newStyle.normal.textColor = selectedColor;
				newStyle.fontSize = (int)info.fontSize + 2;
			}
			GUI.Box(guiRect, info.text, newStyle);
		}
		
		HandleInput();
	}

	void HandleInput() {
		if(Input.GetKeyDown(KeyCode.DownArrow)) {
			if(!inputPressed && optionNum < options.Length - 1) {
				optionNum++;
				inputPressed = true;
			}
		}
		else if(Input.GetKeyDown(KeyCode.UpArrow)) {
			if(!inputPressed && optionNum > 0) {
				optionNum--;
				inputPressed = true;
			}
		}
		else {
			inputPressed = false;	
		}
		if(Input.GetKeyDown(KeyCode.Return)) {
			switch(optionNum) {
			case 0: //Unpause game
				this.enabled = false;
				Game.instance.Unpause();
				break;
			case 1:
				this.enabled = false;
				Game.instance.ReturnToTitle();
				break;
			case 2:
				Application.Quit();
				break;
			}
		}		
	}
}
