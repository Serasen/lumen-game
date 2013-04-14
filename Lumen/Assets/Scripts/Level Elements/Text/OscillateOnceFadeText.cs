using UnityEngine;
using System.Collections;

public class OscillateOnceFadeText : OscillateFadeText {

	protected override IEnumerator fadeInText() {
		while(activeText < guiInfos.Length) {
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
		}
	}
}
