using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	AudioSource audiosource;
	AudioClip[] sounds;
	// Use this for initialization
	void Awake(){
		audiosource = gameObject.GetComponent<AudioSource> ();
	}


	void Start () {
	
		sounds = new AudioClip[20];
		sounds[0] = Resources.Load ("Sounds/ciggy") as AudioClip;
		sounds[1] = Resources.Load ("Sounds/smoosh") as AudioClip;
		sounds[2] = Resources.Load ("Sounds/tap") as AudioClip;
		sounds[3] = Resources.Load ("Sounds/Swap") as AudioClip;
		sounds[4] = Resources.Load ("Sounds/boostNorm") as AudioClip;
		sounds[5] = Resources.Load ("Sounds/boosterSpec") as AudioClip;
		sounds[6] = Resources.Load ("Sounds/swapback") as AudioClip;
		sounds[7] = Resources.Load ("Sounds/explosion") as AudioClip;
		sounds[8] = Resources.Load ("Sounds/stayaway") as AudioClip;
		sounds[9] = Resources.Load ("Sounds/raygun") as AudioClip;
		sounds[10] = Resources.Load ("Sounds/grow") as AudioClip;
		sounds[11] = Resources.Load ("Sounds/tileDestroy") as AudioClip;


	}
		
	public void PlaySound(string sound){

		float vol = Random.Range (0.5f,0.9f);

		if (sound == "smoke") {
			audiosource.PlayOneShot (sounds [0], vol);
		} else if (sound == "swoosh") {
			audiosource.PlayOneShot (sounds [1], vol);
		}
		else if (sound == "land") {
			audiosource.PlayOneShot (sounds [2], vol);
		}
		else if (sound == "swap") {
			audiosource.PlayOneShot (sounds [3], vol);
		}
		else if (sound == "booster") {
			audiosource.PlayOneShot (sounds [4], vol);
		}
		else if (sound == "special") {
			audiosource.PlayOneShot (sounds [5], vol);
		}
		else if (sound == "swapback") {
			audiosource.PlayOneShot (sounds [6], vol);
		}
		else if (sound == "explosion") {
			audiosource.PlayOneShot (sounds [7], vol);
		}
		else if (sound == "stayaway") {
			audiosource.PlayOneShot (sounds [8], vol);
		}
		else if (sound == "raygun") {
			audiosource.PlayOneShot (sounds [9], vol);
		}
		else if (sound == "grow") {
			audiosource.PlayOneShot (sounds [10], vol);
		}
		else if (sound == "tileDestroy") {
			audiosource.PlayOneShot (sounds [11], vol);
		}
		//audiosource.PlayOneShot (smoke);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
