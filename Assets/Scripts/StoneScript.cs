using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoneScript : MonoBehaviour {

	public Text scoreText;
	int level = 0;
	// Use this for initialization

	public void SetLevel(int level){

		this.level = level;
	}

	public int GetLevel(){
		return level;
	}

	void OnMouseEnter(){
		iTween.ScaleTo (gameObject, iTween.Hash("x", 0.6f, "y", 0.6f, "time", 0.5f));
		gameObject.GetComponent<AudioSource> ().volume = 0.1f;
		gameObject.GetComponent<AudioSource> ().Play ();

	}

	void OnMouseExit(){
		iTween.ScaleTo (gameObject, iTween.Hash("x", 0.4f, "y", 0.4f, "time", 0.5f));
	}

	void Awake () {
	
		scoreText = gameObject.GetComponentInChildren<Text> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
