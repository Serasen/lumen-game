using UnityEngine;
using System.Collections;

public class Friend : MonoBehaviour {
	public float maxRange;	
	public Texture2D sprite;
	
	private Level myLevel;
	private FadeText text;
	private float rangeStep = 2/16f;
	private Light friendLight;
	private float startRange;
	private bool unhappy = true;

	void Start() {
		transform.parent.gameObject.renderer.material.SetTexture("_MainTex", sprite);
		myLevel = Game.instance.levelManager.getCurrentLevel();
		text = GetComponent<FadeText>();
		friendLight = transform.parent.GetComponentInChildren<Light>();
		startRange = friendLight.range;
		OnEnable();
	}
	
	void OnEnable() {
		if(myLevel != null) {
			unhappy = !myLevel.getFriendStatus(transform.parent.name);
			if(!unhappy) {
				friendLight.range = maxRange;
				text.enabled = true;
				transform.parent.gameObject.renderer.material.SetTextureOffset("_MainTex", new Vector2(0,0));
			}
			else {
				friendLight.range = startRange;	
			}
		}
	}
	
	void OnTriggerEnter(Collider c) {
		if(c.tag == "Player" && unhappy) {
			StopCoroutine("FadeDark");
			StartCoroutine("FadeLight");
		}
	}
	
	void OnTriggerExit(Collider c) {
		if(c.tag == "Player" && unhappy) {
			StopCoroutine("FadeLight");
			StartCoroutine("FadeDark");
		}
	}
	
	private void GetHappy() {
		unhappy = false;
		myLevel.savedFriend(transform.parent.name);
		text.enabled = true;
		transform.parent.gameObject.renderer.material.SetTextureOffset("_MainTex", new Vector2(0,0));
		transform.parent.GetComponent<FriendBounce>().Bounce ();
	}
	
	IEnumerator FadeLight() {
		while(friendLight.range < maxRange) {
        	yield return new WaitForSeconds(rangeStep/2);
			friendLight.range += rangeStep;
		}
		friendLight.range = maxRange;
		GetHappy();
	}
	
	IEnumerator FadeDark() {
		while(friendLight.range > startRange) {
        	yield return new WaitForSeconds(rangeStep);
			friendLight.range -= rangeStep/2;
		}
		friendLight.range = startRange;
	}
}
