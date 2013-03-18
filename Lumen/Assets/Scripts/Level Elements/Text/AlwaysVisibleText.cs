using UnityEngine;
using System.Collections;

public class AlwaysVisibleText : Text {
	
	protected override void OnEnable() {
		mainCamera = Game.instance.levelManager.getCamera();
		base.OnEnable();
	}
	
}
