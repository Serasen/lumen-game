using UnityEngine;
using System.Collections;

public class ShineZoneAudio : MonoBehaviour {
	
	float audioTime;
	
	void Start() {
		audioTime = 0f;	
		audio.loop = true;
	}
	
	void OnEnable() {
		audio.time = audioTime;
		audio.Play();
	}	
	
	void OnDisable() {
		audio.Pause();
	}
	
	// Update is called once per frame
	void Update () {
		audioTime = audio.time;
	}
}
