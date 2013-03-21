using UnityEngine;
using System.Collections;

public class ToLoadSelectKeyhole : Keyhole {
	void OnEnable() {
		if(!Game.instance.dataManager.hasSaved()) {
			renderer.enabled = false;
			collider.enabled = false;
			foreach(Transform child in transform) {
				child.gameObject.SetActive(false);	
			}
		}
		else {
			renderer.enabled = true;
			collider.enabled = true;
			foreach(Transform child in transform) {
				child.gameObject.SetActive(true);	
			}			
		}
	}
}
