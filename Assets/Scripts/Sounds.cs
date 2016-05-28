using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	AudioSource audiosource;
	public AudioClip smoke;
	// Use this for initialization
	void Start () {
	
		audiosource = gameObject.GetComponent<AudioSource> ();
	}


	public void PlaySound(){

		audiosource.PlayOneShot (smoke);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
