using UnityEngine;
using System.Collections;

public class Sludge : MonoBehaviour {
	public float duration;
	private IloController ic;
	private Material iloMat;
	private Vector2 offset = new Vector2(.5f,0);
	
	void Start () {
		Destroy (this.gameObject, duration);
	}
	
	void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player") {
			ic = collider.GetComponent<IloController>();
			iloMat = collider.renderer.material;
			collider.gameObject.transform.rotation = transform.rotation;
			Stickem();
		}
	}
	
	void OnDisable() {
		if(ic != null) {
			ic.enabled = true;
			iloMat.SetTextureOffset("_MainTex", iloMat.GetTextureOffset("_MainTex") - offset);
		}
	}
	
	void Stickem() {
		//yield return new WaitForSeconds(.1f);
		audio.Play ();
		ic.enabled = false;
		iloMat.SetTextureOffset("_MainTex", iloMat.GetTextureOffset("_MainTex") + offset);
	}

}
