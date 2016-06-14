using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {
	PlayerInput playerinput;
	GridManager gm;
	Sounds sounds;
	ScoreHandler scorehandler;


	// Use this for initialization
	void Start () {
		playerinput = GameObject.Find ("GameController").GetComponent<PlayerInput> ();
		gm = GameObject.Find ("GameController").GetComponent<GridManager>();
		sounds = Camera.main.GetComponent<Sounds> ();
		scorehandler = GameObject.Find ("scoretext").GetComponent<ScoreHandler> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("Entered trigeer: " + playerinput.bs);
		if (playerinput.bs == BoosterState.Destroy) {
			gm.DestroyTile (other.gameObject.transform.position, true);
			sounds.PlaySound ("tileDestroy");
			scorehandler.AddPoints (20);
			Instantiate (gm.scorePrefabs[0], other.transform.position, Quaternion.identity);
			//Destroy (other.gameObject);
		}
	}
}
