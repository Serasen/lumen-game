using UnityEngine;
using System.Collections;

public class FadeText : MonoBehaviour {
	
	Camera mainCamera;
	
	float alphaValue;
	
	public GUIInfo[] guiInfos;
	
	[System.Serializable]
	public class GUIInfo {
		public string text;
		public float fontSize;
		public float posX;
		public float posY;
	}
	
	float pixelRatio;
	
	void Start() {
		mainCamera = Game.instance.levelManager.getCamera();
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
		pixelRatio = (mainCamera.orthographicSize * 2) / mainCamera.pixelHeight;
		
		GUIStyle newStyle = new GUIStyle();
		newStyle.alignment = TextAnchor.UpperCenter;
		newStyle.normal.textColor = new Color(1,1,1,alphaValue);
		Rect guiRect;
		
		foreach(GUIInfo elem in guiInfos) {
			newStyle.fontSize = (int) (elem.fontSize/pixelRatio);
			Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.position + new Vector3(elem.posX, elem.posY, 0));
			guiRect = new Rect(screenPoint.x, Screen.height - (screenPoint.y), 0, 0);
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
