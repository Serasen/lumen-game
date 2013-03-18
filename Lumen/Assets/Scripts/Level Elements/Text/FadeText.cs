using UnityEngine;
using System.Collections;

public class FadeText : Text {
	
	protected override void OnEnable() {
		alphaValue = 0f;
	}
	
	protected void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			if(mainCamera == null) {
				mainCamera = Game.instance.levelManager.getCamera();
			}
			StopCoroutine("fadeOutText");
			StartCoroutine("fadeInText");
		}
	}
	
	protected void OnTriggerExit(Collider collider) {
		if(collider.tag == "Player") {
			StopCoroutine("fadeInText");
			StartCoroutine("fadeOutText");
		}
	}
	
	protected IEnumerator fadeInText() {
		while(alphaValue < 1f) {
        	yield return new WaitForSeconds(0.01f);
			alphaValue += 0.1f;
		}
	}
	
	protected IEnumerator fadeOutText() {
		while(alphaValue > 0f) {
        	yield return new WaitForSeconds(0.01f);
			alphaValue -= 0.1f;
		}
	}
	
	protected override void OnGUI() {
		if(mainCamera != null) {
			base.OnGUI();	
		}
	}
}
