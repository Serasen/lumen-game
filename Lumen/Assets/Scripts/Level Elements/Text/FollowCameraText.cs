using UnityEngine;
using System.Collections;

public class FollowCameraText : OscillateOnceFadeText {
	
	//Return screen point of target (in this case, camera)
	protected override Vector3 getScreenPoint(float x, float y) {
		return mainCamera.WorldToScreenPoint(mainCamera.transform.position + new Vector3(x, y, 0));
	}
}
