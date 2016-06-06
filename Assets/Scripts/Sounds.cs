using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	AudioSource audiosource;
	public AudioClip[] sounds;
	// Use this for initialization
	void Start () {
	
		audiosource = gameObject.GetComponent<AudioSource> ();
	}


	public void PlaySound(string sound){

		if (sound == "smoke") {
			audiosource.PlayOneShot (sounds [0]);
		} else if (sound == "swoosh") {
			audiosource.PlayOneShot (sounds [1]);
		}
		//audiosource.PlayOneShot (smoke);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
