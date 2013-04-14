using UnityEngine;
using System.Collections;

public class LevelDisplayText : FadeText {
	int levelToDisplay;
	
	protected override void OnEnable() {
		levelToDisplay = transform.parent.GetComponent<EnterLevelKeyhole>().levelToEnter;
		alphaValue = 0f;
		guiInfos[0].text = "Level " + levelToDisplay;
	}	
}
