using UnityEngine;
using System.Collections;

public class LoadGameFileText : FadeText {
	int saveFileToDisplay;
	
	protected override void OnEnable() {
		saveFileToDisplay = transform.parent.GetComponent<LoadGameKeyhole>().saveFile;
		if(guiInfos.Length > 2) {
			guiInfos[0].text = "File " + saveFileToDisplay;
			guiInfos[1].text = "";
			guiInfos[2].text = "";
		}
	}
}
