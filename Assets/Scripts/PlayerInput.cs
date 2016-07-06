using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{

	GridManager gm;
	Sounds sounds;
	ScoreHandler scorehandler;
	public LayerMask Tiles;
	private GameObject activeTile;
	public GameState currentState;
	public BoosterState bs;
	public Text movesText;
	int movesLeft;
	LevelScript leveldata;

	void Start ()
	{
		leveldata = GameObject.Find ("LevelHandler").GetComponent<LevelScript> ();
		scorehandler = GameObject.Find ("scoretext").GetComponent<ScoreHandler> ();
		bs = BoosterState.dontDestroy;
		gm = gameObject.GetComponent<GridManager> ();
		sounds = Camera.main.GetComponent<Sounds> ();
		movesLeft = leveldata.moves;
		movesText.text = "MOVES\n" + movesLeft;

	}

		
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

			if (activeTile.GetComponent<MoveScript> ().getName () == "beer" && !leveldata.objectiveBeer) {
				//swap with a cigarette and display feedback
				gm.CreateCigarette (activeTile.transform.position);
				gm.UpdateGridArray ();
				gm.DisplayFeedback (1);
				activeTile = null;

				//if we replace a beer with a cigarette that was the last potential swap we need to check for possible moves again
				if (!gm.checkForPossibleMoves ()) {
					gm.ReplaceGrid ();
				}
			} else if (activeTile.GetComponent<MoveScript> ().getName () == "ciggy") {
				//freeze screen for 3 seconds and display feedback
				sounds.PlaySound ("stayaway");
				gm.DisplayFeedback (2);
				activeTile = null;
			}
		}
	}
		
	void AttemptMove ()
	{
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
					gm.cigSpawnAllowed = true;
					//swap tiles
					gm.SwapTiles (tile1, tile2);		//swap
				}
			}
		}
	}

	void FinishBoosterAnimation (GameObject tile)
	{
		//scorehandler.AddPoints (5000); put this in destroyer
		Destroy (tile);
		bs = BoosterState.dontDestroy;
		gm.ReplaceTiles ();
	}

	void MoveLeft (Hashtable values)
	{
		//Debug.Log ("moving left");
		GameObject tile = values ["Tile"] as GameObject;
		iTween.MoveBy (tile, iTween.Hash ("y", 5, "time", 0.8f, "oncomplete", "FinishBoosterAnimation", "oncompletetarget", GameObject.Find ("GameController"), "oncompleteparams", tile));
	}

	void MoveRight (Hashtable values)
	{
		//Debug.Log ("moving right");
		GameObject tile = values ["Tile"] as GameObject;
		tile.transform.position = new Vector2 (-2, (float)values ["TileY"]);

		iTween.MoveTo (tile, iTween.Hash ("x", 9, "time", 1, "oncomplete", "FinishBoosterAnimation", "oncompletetarget", GameObject.Find ("GameController"), "oncompleteparams", tile));
		sounds.PlaySound ("swoosh");
	}

	void MoveUp (Hashtable values)
	{
		//Debug.Log ("moving up");
		GameObject tile = values ["Tile"] as GameObject;
		iTween.MoveTo (tile, iTween.Hash ("y", 9, "time", 1f, "oncomplete", "MoveRight", "oncompletetarget", GameObject.Find ("GameController"), "oncompleteparams", values));
		sounds.PlaySound ("swoosh");
	}

	void MoveDown (GameObject tile)
	{
		//Debug.Log ("moving down");
		Hashtable values = new Hashtable ();
		values.Add ("Tile", tile);
		values.Add ("TileX", tile.transform.position.x);
		values.Add ("TileY", tile.transform.position.y);

		iTween.MoveTo (tile, iTween.Hash ("y", 0.66f, "time", 1, "oncomplete", "MoveUp", "oncompletetarget", GameObject.Find ("GameController"), "oncompleteparams", values));
		sounds.PlaySound ("swoosh");
	}

	void Grow (GameObject tile)
	{
		sounds.PlaySound ("grow");
		iTween.ScaleTo (tile, iTween.Hash ("y", 0.4, "x", 0.4f, "time", 1, "oncomplete", "MoveDown", "oncompletetarget", GameObject.Find ("GameController"), "oncompleteparams", tile));

	}

	void FinishedSwapping (List<GameObject> tiles)
	{
		
		gm.UpdateGridArray ();

		//if both tiles are normal boosters
		if (AreNormalBoosters (tiles [0], tiles [1])) {
			DecrementMoves ();
			bs = BoosterState.Destroy;
			Grow (tiles [0]);
			gm.Grid [Mathf.RoundToInt (tiles [0].transform.position.x), Mathf.RoundToInt (tiles [0].transform.position.y)] = null;
		} 

		//if both are special boosters (waterbottles)
		else if (AreSpecialBoosters (tiles [0], tiles [1])) {
			DecrementMoves ();
			gm.DestroyGridQuarter (tiles[1].transform.position);
			gm.Invoke ("ReplaceTiles", 0.3f);
		}

		//if one is a special booster and the other is a normal booster
		else if (AreSpecialAndNormalBoosters (tiles [0], tiles [1])) {
			DecrementMoves ();

			if (tiles [0].GetComponent<MoveScript> ().isBooster) {
				gm.ReplaceWithBoosters (tiles [0].GetComponent<MoveScript> ().getName ());
			} else {
				gm.ReplaceWithBoosters (tiles [1].GetComponent<MoveScript> ().getName ());
			}
			gm.DestroyTile (tiles [0].transform.position, true);
			gm.DestroyTile (tiles [1].transform.position, true);
			gm.Invoke ("ReplaceTiles", 0.3f);
			scorehandler.AddPoints (1500);
			Instantiate (gm.scorePrefabs[6], tiles [1].transform.position, Quaternion.identity);

		} 

		//if only one is a special booster and other is regular tile
		else if (CheckForSpecialBooster (tiles [0], tiles [1])) {
			DecrementMoves ();
			sounds.PlaySound ("raygun");
			if (tiles [0].GetComponent<MoveScript> ().isSpecialBooster) {
				StartCoroutine (gm.DestroyTilesWithName (tiles [1].GetComponent<MoveScript> ().getName ()));
				gm.DestroyTile (tiles [0].transform.position, false);
			} else {
				StartCoroutine (gm.DestroyTilesWithName (tiles [0].GetComponent<MoveScript> ().getName ()));
				gm.DestroyTile (tiles [1].transform.position, false);
			}
		}

		//if tiles are normal
		else {
			//check for matches after grid has been updated
			List<Vector2>[] matches;
			matches = gm.getMatches (gm.Grid); //Retrieve any new matches

			//if no matches of atleast 3, swap tiles back
			if (matches [0] == null) {
				activeTile = null;
				gm.SwapBack (tiles [0], tiles [1]); //swap back
			} else {
				DecrementMoves ();
				StartCoroutine (gm.Check ());
			}
		}
	
	}

	bool CheckForSpecialBooster (GameObject tile1, GameObject tile2)
	{
		if (tile1.GetComponent<MoveScript> ().isSpecialBooster) {
			return true;
		} else if (tile2.GetComponent<MoveScript> ().isSpecialBooster) {
			return true;
		}
		return false;
	}

	bool AreNormalBoosters (GameObject tile1, GameObject tile2)
	{
		if (tile1.GetComponent<MoveScript> ().isBooster && tile2.GetComponent<MoveScript> ().isBooster) {
			return true;
		} else {
			return false;
		}
	}

	bool AreSpecialBoosters (GameObject tile1, GameObject tile2)
	{

		if (tile1.GetComponent<MoveScript> ().isSpecialBooster && tile2.GetComponent<MoveScript> ().isSpecialBooster) {
			return true;
		} else {
			return false;
		}
	}

	bool AreSpecialAndNormalBoosters (GameObject tile1, GameObject tile2)
	{

		if (tile1.GetComponent<MoveScript> ().isSpecialBooster && tile2.GetComponent<MoveScript> ().isBooster) {
			return true;
		} else if (tile1.GetComponent<MoveScript> ().isBooster && tile2.GetComponent<MoveScript> ().isSpecialBooster) {
			return true;
		} else {
			return false;
		}

	}

	void FinishedSwappingBack ()
	{

		//activeTile = null; //redundant
		gm.UpdateGridArray ();
		currentState = GameState.None;



	}

	void DecrementMoves ()
	{
		movesLeft--;
		if (movesLeft >= 0) {
			movesText.text = "MOVES\n" + movesLeft;
		}
		if(movesLeft == 0 && !leveldata.gameEnded) {
			gm.outOfMoves = true;
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