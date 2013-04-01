using UnityEngine;
using System.Collections;

enum Audio {
	JUMP_BEGIN,
	JUMP_END
}

public class IloAudio : MonoBehaviour {
	public AudioClip jumpBegin;
	public AudioClip jumpEnd;
	
	float[] scale;
	const float chromaticScaleLength = 12;
	
	void Start() {
		makeChromaticScale();
	}
	
	public void makeChromaticScale() {
		scale = new float[12];
		for(int i = 0; i < scale.Length; i++) {
			scale[i] = 1/2f + i/24f;
		}		
	}
	
	int currentTone;
	
	public void playClip(int number) {
		switch(number) {
			case (int)Audio.JUMP_BEGIN: audio.clip = jumpBegin; 
				audio.pitch = scale[Random.Range(0, scale.Length - 1)];
				audio.volume = 0.1f;
				break;
			case (int)Audio.JUMP_END: audio.clip = jumpBegin; 
				audio.volume = 0.01f;
				break;
		}
		audio.Play();
	}
}
