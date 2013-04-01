using UnityEngine;
using System.Collections;

public class ShineZoneAudio : MonoBehaviour {
	
	void Start() {
		audio.loop = true;
	}
	
	void OnEnable() {
		audio.clip = Game.instance.getAudioClip();
		audio.time = Game.instance.getAudioTime();
		audio.Play();
	}
}
