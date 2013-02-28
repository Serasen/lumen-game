using UnityEngine;
using System.Collections;

public class FadeText : MonoBehaviour {
	
	Room myRoom;
	
	float alphaValue;
	
	public GUIInfo[] guiInfos;
	
	[System.Serializable]
	public class GUIInfo {
		public string text;
		public int fontSize;
		public int posX;
		public int posY;
	}
	
	void Start() {
		myRoom = transform.parent.GetComponent<Room>();
		alphaValue = 0f;	
	}
	
	void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			StopCoroutine("fadeOutText");
			StartCoroutine("fadeInText");
		}
	}
	
	void OnTriggerExit(Collider collider) {
		if(collider.tag == "Player") {
			StopCoroutine("fadeInText");
			StartCoroutine("fadeOutText");
		}
	}
	
	void OnGUI() {
		GUIStyle newStyle = new GUIStyle();
		newStyle.alignment = TextAnchor.UpperCenter;
		newStyle.normal.textColor = new Color(1,1,1,alphaValue);
		Vector3 screenPoint = myRoom.mainCamera.camera.WorldToScreenPoint(transform.position);
		Rect guiRect;
		foreach(GUIInfo elem in guiInfos) {
			newStyle.fontSize = elem.fontSize;
			guiRect = new Rect(screenPoint.x + elem.posX, Screen.height - (screenPoint.y + elem.posY), 0, 0);
			GUI.Box(guiRect, elem.text, newStyle);
		}	
	}
	
	IEnumerator fadeInText() {
		while(alphaValue < 1f) {
        	yield return new WaitForSeconds(0.01f);
			alphaValue += 0.1f;
		}
	}
	
	IEnumerator fadeOutText() {
		while(alphaValue > 0f) {
        	yield return new WaitForSeconds(0.01f);
			alphaValue -= 0.1f;
		}
	}
}
