using UnityEngine;
using System.Collections;

public class NewGameFileText : FadeText {
	int saveFileToDisplay;
	
	protected override void OnEnable() {
		saveFileToDisplay = transform.parent.GetComponent<NewGameKeyhole>().saveFile;
		if(guiInfos.Length > 2) {
			guiInfos[0].text = "File " + saveFileToDisplay;
			if(Game.instance.dataManager.DataExists(saveFileToDisplay)) {
				guiInfos[1].text = "(Overwrite)";
				guiInfos[2].text = "";
			}
			else {
				guiInfos[1].text = "New!";
				guiInfos[2].text = "";
			}
		}
	}
}
