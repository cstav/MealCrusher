using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{

	GridManager gm;
	public LayerMask Tiles;
	private GameObject activeTile;
	public GameState currentState;
	public Text movesText;
	int movesLeft;


	// Use this for initialization
	void Start ()
	{
		gm = gameObject.GetComponent<GridManager> ();
		movesLeft = 20;
		movesText.text = "Moves: " + movesLeft;

	}

	// Update is called once per frame
	void Update ()
	{

		//dont let them select if the screen is animating
		if (currentState != GameState.Animating) {

			if (Input.GetKeyDown (KeyCode.Mouse0)) {

				if (activeTile == null) {
					SelectTile ();
				} 
			} else if (Input.GetKey (KeyCode.Mouse0)) {


				if (activeTile != null) {
					AttemptMove ();
				}
			} else if (Input.GetKeyUp (KeyCode.Mouse0)) {
				activeTile = null;
			}

			if (Input.GetKeyDown ("space")) {
				gm.SpaceBarFunction ();
			}
		}

	}

	void SelectTile ()
	{
		Vector2 rayPos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		RaycastHit2D hit = Physics2D.Raycast (rayPos, Vector2.zero, 0f);
		//Debug.Log ("hit: " + hit);
		if (hit) {	

			//Debug.Log ("hit tile");
			activeTile = hit.collider.gameObject;

			if (activeTile.GetComponent<MoveScript> ().getName () == "beer") {
				//swap with a cigarette and display feedback
				gm.CreateCigarette(activeTile.transform.position);
				gm.UpdateGridArray ();
				gm.DisplayFeedback (1);
				activeTile = null;
			} else if (activeTile.GetComponent<MoveScript> ().getName () == "ciggy") {
				//freeze screen for 3 seconds and display feedback
				gm.DisplayFeedback (2);
				activeTile = null;
			}
		}
	}




	void AttemptMove ()
	{



		//Debug.Log ("attempted move");
		Vector2 tilePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		RaycastHit2D hit = Physics2D.Raycast (tilePos, Vector2.zero, 0f);



		if (hit) {

			GameObject tile1 = activeTile;
			GameObject tile2 = hit.collider.gameObject;
			Vector2 tile1Pos = tile1.transform.position;
			Vector2 tile2Pos = hit.collider.gameObject.transform.position;

			//if statement stops from switching tiles that are surrounded in fat
			if (gm.NoFatExists (new Vector2 (tile1Pos.x, tile1Pos.y), new Vector2 (tile2Pos.x, tile2Pos.y))
			    && gm.getTileName (tile1Pos) != "ciggy" && gm.getTileName (tile2Pos) != "ciggy") {

				if (NeighborCheck (tile2)) {
					currentState = GameState.Animating;

					//swap tiles
					gm.SwapTiles (tile1, tile2);		//swap
					//yield return new WaitForSeconds (gm.swapTime);				//wait
					//gm.UpdateGridArray ();									//update


				}
			}
		}
	}

	void FinishedSwapping (List<GameObject> tiles)
	{

		gm.UpdateGridArray ();

		if (CheckForSpecialBooster (tiles [0], tiles [1]) == false) {
			//check for matches after grid has been updated
			List<Vector2>[] matches;
			matches = gm.getMatches (gm.Grid); //Retrieve any new matches


			//if no matches of atleast 3, swap tiles back
			if (matches [0] == null) {
				gm.SwapBack (tiles [0], tiles [1]); //swap back

			} else {
				Debug.Log ("there are matches");
				UpdateMovesLeft ();
				//StartCoroutine (gm.continousCheck ());
				StartCoroutine(	gm.Check());
			}

			activeTile = null; //set to null to allow next move
		}
	}

	void FinishedSwappingBack(){

		gm.UpdateGridArray ();
		currentState = GameState.None;

		activeTile = null; //redundant

	}

	void UpdateMovesLeft ()
	{

		if (movesLeft > 0)
			movesLeft--;
		movesText.text = "Moves: " + movesLeft;


	}

	bool CheckForSpecialBooster (GameObject tile1, GameObject tile2)
	{

		//logic for when a swap is made with a special booster
		if (tile1.GetComponent<MoveScript> ().getName () == "water") {
			gm.Grid [Mathf.RoundToInt (tile1.transform.position.x), Mathf.RoundToInt (tile1.transform.position.y)] = null;

			StartCoroutine( gm.DestroyTilesWithName (tile2.GetComponent<MoveScript> ().getName ()));
			Destroy (tile1);
			UpdateMovesLeft ();
			activeTile = null;
			return true;

		} else if (tile2.GetComponent<MoveScript> ().getName () == "water") {
			gm.Grid [Mathf.RoundToInt (tile2.transform.position.x), Mathf.RoundToInt (tile2.transform.position.y)] = null;

			StartCoroutine( gm.DestroyTilesWithName (tile1.GetComponent<MoveScript> ().getName ()));
			Destroy (tile2);
			UpdateMovesLeft ();
			activeTile = null;
			return true;
		} else {
			
			return false;
		}
	}

	bool NeighborCheck (GameObject objectToCheck)
	{

		float xDiff = Mathf.RoundToInt (Mathf.Abs (activeTile.transform.position.x - objectToCheck.transform.position.x));
		float yDiff = Mathf.RoundToInt (Mathf.Abs (activeTile.transform.position.y - objectToCheck.transform.position.y));

		//Debug.Log ("Diff: " + (xDiff + yDiff));

		if (xDiff + yDiff == 1) {
			return true;

		} else {
			return false;
		}

	}
}