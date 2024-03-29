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
	protected GUIStyle style;
	
	protected void Start() {
		style = new GUIStyle();	
	}
	
	protected virtual void OnEnable() {
		alphaValue = 1f;
	}
	
	protected void OnDisable() {
		alphaValue = 0f;	
	}
	
	protected virtual void OnGUI() {
		float nowAlpha;
		if(alphaValue > 0f) {
			nowAlpha = getNowAlpha();
			float pixelRatio = (mainCamera.orthographicSize * 2) / mainCamera.pixelHeight;
			style.alignment = TextAnchor.UpperCenter;
			style.normal.textColor = new Color(1,1,1,nowAlpha);
			Rect guiRect;
		
			foreach(GUIInfo elem in guiInfos) {
				style.fontSize = (int) (elem.fontSize/pixelRatio);
				Vector3 screenPoint = getScreenPoint(elem.posX, elem.posY);
				guiRect = new Rect(screenPoint.x, Screen.height - (screenPoint.y), 0, 0);
				GUI.Box(guiRect, elem.text, style);
			}
		}
	}
	
	//Return screen point of target (in this case, text zone)
	protected virtual Vector3 getScreenPoint(float x, float y) {
		return mainCamera.WorldToScreenPoint(transform.position + new Vector3(x, y, 0));
	}
	
	protected float getNowAlpha() {
		float nowAlpha;
		switch(Game.instance.gameState) {
			case (int)GameState.PAUSE: 
				nowAlpha = 0f;
				break;
			case (int)GameState.GRADUAL_PAUSE: 
				nowAlpha = Mathf.Min(alphaValue, 1 - Game.instance.frontPlaneOpacity);
				break;
			default: 
				nowAlpha = alphaValue; 
				break;
		}
		return nowAlpha;
	}
}
