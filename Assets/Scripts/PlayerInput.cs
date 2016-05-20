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
					StartCoroutine (AttemptMove ());
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
		}
	}

	IEnumerator AttemptMove ()
	{

		//Debug.Log ("attempted move");
		Vector2 tilePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		RaycastHit2D hit = Physics2D.Raycast (tilePos, Vector2.zero, 0f);

		if (hit) {
			if (NeighborCheck (hit.collider.gameObject)) {
				currentState = GameState.Animating;

				//swap tiles
				gm.SwapTiles (activeTile, hit.collider.gameObject);		//swap
				yield return new WaitForSeconds (gm.swapTime);				//wait
				gm.UpdateGridArray ();									//update

				if (CheckForSpecialBooster (activeTile, hit.collider.gameObject) == true) {
					
				} else {


					//check for matches after grid has been updated
					List<Vector2>[] matches;
					matches = gm.getMatches (); //Retrieve any new matches


					//if no matches of atleast 3, swap tiles back
					if (matches [0] == null) {
					
						gm.SwapTiles (activeTile, hit.collider.gameObject); 		//swap back
						yield return new WaitForSeconds (gm.swapTime);				//wait
						gm.UpdateGridArray ();									//update
						currentState = GameState.None;
					} else {

						UpdateMovesLeft ();
						StartCoroutine (gm.continousCheck ());
					}

					activeTile = null; //set to null to allow next move
	
				}
			}

		}
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
			gm.Grid[Mathf.RoundToInt(tile1.transform.position.x), Mathf.RoundToInt(tile1.transform.position.y)] = null;
			Destroy (tile1);
			StartCoroutine (gm.DestroyTilesWithName (tile2.GetComponent<MoveScript> ().getName ()));
			UpdateMovesLeft ();
			activeTile = null;
			return true;

		} else if (tile2.GetComponent<MoveScript> ().getName () == "water") {
			gm.Grid[Mathf.RoundToInt(tile2.transform.position.x), Mathf.RoundToInt(tile2.transform.position.y)] = null;
			Destroy (tile2);
			StartCoroutine (gm.DestroyTilesWithName (tile1.GetComponent<MoveScript> ().getName ()));
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
