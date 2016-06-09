using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {
	PlayerInput playerinput;
	GridManager gm;

	// Use this for initialization
	void Start () {
		playerinput = GameObject.Find ("GameController").GetComponent<PlayerInput> ();
		gm = GameObject.Find ("GameController").GetComponent<GridManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("Entered trigeer: " + playerinput.bs);
		if (playerinput.bs == BoosterState.Destroy) {
			gm.DestroyTile (other.gameObject.transform.position, true);

			//Destroy (other.gameObject);
		}
	}
}
