using UnityEngine;
using System.Collections;

public class ShineZoneAudio : MonoBehaviour {
	
	float audioTime;
	
	void Start() {
		audioTime = 0f;	
		audio.loop = true;
	}
	
	void OnEnable() {
		audio.clip = Game.instance.getAudioClip();
		audio.time = Game.instance.getAudioTime();
		audio.Play();
	}
}
