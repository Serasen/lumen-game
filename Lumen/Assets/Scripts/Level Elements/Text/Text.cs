using UnityEngine;
using System.Collections;

	[System.Serializable]
	public class GUIInfo {
		public string text;
		public float fontSize;
		public float posX;
		public float posY;
	}

public class Text : MonoBehaviour {
		
	protected Camera mainCamera;
	
	protected float alphaValue;
	protected float prePauseAlpha;
	
	public GUIInfo[] guiInfos;
	
	protected virtual void OnEnable() {
		alphaValue = 1f;	
	}
	
	protected void OnDisable() {
		alphaValue = 0f;	
	}
	
	protected virtual void OnGUI() {
		float nowAlpha;
		if(Game.instance.gameState == (int)GameState.PAUSE) {
			nowAlpha = 0f;
		}
		else nowAlpha = alphaValue;
		
		float pixelRatio = (mainCamera.orthographicSize * 2) / mainCamera.pixelHeight;
		
		GUIStyle newStyle = new GUIStyle();
		newStyle.alignment = TextAnchor.UpperCenter;
		newStyle.normal.textColor = new Color(1,1,1,nowAlpha);
		Rect guiRect;
		
		foreach(GUIInfo elem in guiInfos) {
			newStyle.fontSize = (int) (elem.fontSize/pixelRatio);
			Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.position + new Vector3(elem.posX, elem.posY, 0));
			guiRect = new Rect(screenPoint.x, Screen.height - (screenPoint.y), 0, 0);
			GUI.Box(guiRect, elem.text, newStyle);
		}
	}
}
