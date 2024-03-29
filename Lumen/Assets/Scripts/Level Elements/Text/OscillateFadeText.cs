using UnityEngine;
using System.Collections;

public class OscillateFadeText : FadeText {
	
	public float waitTime;
	protected int activeText;
	
	protected override void OnEnable() {
		activeText = 0;
		base.OnEnable();
	}
	
	protected override IEnumerator fadeInText() {
		while(this.enabled) {
			while(alphaValue < 1f) {
     	   		yield return new WaitForSeconds(0.01f);
				alphaValue += 0.1f;
			}
			yield return new WaitForSeconds(waitTime);
			while(alphaValue > 0f) {
      	  		yield return new WaitForSeconds(0.01f);
				alphaValue -= 0.1f;
			}
			yield return new WaitForSeconds(waitTime);
			
			activeText++;
			if(activeText == guiInfos.Length) activeText = 0;
		}
	}
	
	protected override void OnGUI() {
		float nowAlpha;
		if(alphaValue > 0f) {
			nowAlpha = getNowAlpha();
			float pixelRatio = (mainCamera.orthographicSize * 2) / mainCamera.pixelHeight;
			style.alignment = TextAnchor.UpperCenter;
			style.normal.textColor = new Color(1,1,1,nowAlpha);
			Rect guiRect;
			
			GUIInfo elem = guiInfos[activeText];
			
			style.fontSize = (int) (elem.fontSize/pixelRatio);
			Vector3 screenPoint = getScreenPoint(elem.posX, elem.posY);
			guiRect = new Rect(screenPoint.x, Screen.height - (screenPoint.y), 0, 0);
			GUI.Box(guiRect, elem.text, style);
		}
	}
}
