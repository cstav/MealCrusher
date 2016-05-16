using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class GridManager : MonoBehaviour
{
	public int GridWidth;
	public int GridHeight;
	public GameObject[,] Grid;
	public LayerMask Tiles;
	public GameObject[] TilePrefabs;
	public GameObject[] boosterPrefabs;
	private string[] tileColours;
	public AudioSource source;
	public AudioClip zap;
	public AudioClip pop;
	public float swapTime = 0.25f;
	public float dropTime = 0.5f;
	public int index;
	//number of match sets at each check

	//two tiles that were last swapped
	GameObject lastTileMoved1;
	GameObject lastTileMoved2;

	PlayerInput playerinput;
	ScoreHandler scorehandler;


	void Awake ()
	{

		scorehandler = GameObject.Find ("scoretext").GetComponent<ScoreHandler> ();
		playerinput = GameObject.Find ("GameController").GetComponent<PlayerInput> ();

		source = gameObject.GetComponent<AudioSource> ();
		tileColours = new string[7];
		tileColours [0] = "beer";
		tileColours [1] = "ciggy";
		tileColours [2] = "cutlet";
		tileColours [3] = "hamburger";
		tileColours [4] = "milk";
		tileColours [5] = "pepper";
		tileColours [6] = "strawberry";

		StartCoroutine (CreateGrid ());


	}

	IEnumerator CreateGrid ()
	{
		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];
		
		for (int y = GridHeight - 1; y >= 0; y--) {
			for (int x = 0; x < GridWidth; x++) {
				int randomTile = Random.Range (0, TilePrefabs.Length);

				GameObject tile = Instantiate (TilePrefabs [randomTile], new Vector2 (x, y), Quaternion.identity) as GameObject;
				tile.GetComponent<MoveScript> ().tileIndex = randomTile;
				//float vol = Random.Range(0.2f,0.5f);
				//source.PlayOneShot (pop,vol);
				Grid [x, y] = tile;

				//Assign the tile its colour
				Grid [x, y].GetComponent<MoveScript> ().setColour (tileColours [randomTile]);
				//give it its position in the grid
				Grid [x, y].GetComponent<MoveScript> ().setGridPosition (new Vector2 (x, y));


				yield return new WaitForSeconds (.02f);
			}
		}


		StartCoroutine (continousCheck ());

	}

	public void CheckPossibleMatches ()
	{
		ReplaceGrid ();

	}

	void ReplaceGrid ()
	{

		for (int y = GridHeight - 1; y >= 0; y--) {
			for (int x = 0; x < GridWidth; x++) {
				Destroy (Grid [x, y]);
			}
		}
		StartCoroutine (CreateGrid ());
	}

	




	public List<Vector2>[] getMatches ()
	{
		List<Vector2> matchPositions = new List<Vector2> ();

		//contains sets of matching tiles
		List<Vector2>[] matchSets = new List<Vector2>[10]; 
		index = 0;

		string currentColour = "none";


		//check horizontal matches
		for (int y = 0; y < GridHeight; y++) {
			for (int x = 0; x < GridWidth; x++) {

				if (currentColour != Grid [x, y].GetComponent<MoveScript> ().getColour ()) {

					if (matchPositions.Count >= 3) {

						matchSets [index] = new List<Vector2> ();
						foreach (Vector2 match in matchPositions) {
							matchSets [index].Add (match);
						}
						index++; //change the index
					}

					currentColour = Grid [x, y].GetComponent<MoveScript> ().getColour ();
					matchPositions.Clear ();
					matchPositions.Add (new Vector2 (x, y));
				} else {
					matchPositions.Add (new Vector2 (x, y));
				}

			}
			//at the end set colour back to none
			currentColour = "none";

		}

		//after scanning the whole grid we need to check if 3 or more tiles matched again...

		if (matchPositions.Count >= 3) {
			matchSets [index] = new List<Vector2> (); //create new list

			//add all matches to new list
			foreach (Vector2 match in matchPositions) {
				matchSets [index].Add (match);
			}

			index++; //change the index
		}



		//check vertical matches
		currentColour = "none";
		
		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {
				
				if (currentColour != Grid [x, y].GetComponent<MoveScript> ().getColour ()) {
					
					if (matchPositions.Count >= 3) {

						matchSets [index] = new List<Vector2> ();
						foreach (Vector2 match in matchPositions) {
							matchSets [index].Add (match);
						}
						index++; //change the index
					}
					
					currentColour = Grid [x, y].GetComponent<MoveScript> ().getColour ();
					matchPositions.Clear ();
					matchPositions.Add (new Vector2 (x, y));
				} else {
					matchPositions.Add (new Vector2 (x, y));
				}
				
			}
			//at the end set colour back to none
			currentColour = "none";
			
		}
			
		//after scanning the whole grid we need to check if 3 or more tiles matched again...

		if (matchPositions.Count >= 3) {
			matchSets [index] = new List<Vector2> (); //create new list

			//add all matches to new list
			foreach (Vector2 match in matchPositions) {
				matchSets [index].Add (match);
			}

			index++; //change the index
		}


		return matchSets;
	}


	public void DestroyTiles (List<Vector2>[] matchSets)
	{
		if (matchSets [0] != null) {
			source.PlayOneShot (zap);
		}



		for (int i = 0; i < index; i++) {

			//Award points for match
			switch (matchSets [i].Count) {
			case 3:
				scorehandler.AddPoints (100);
				break;
			case 4:
				scorehandler.AddPoints (200);
				break;
			case 5:
				scorehandler.AddPoints (500);
				break;
			default: 
				break;
			}

			int matchCount = matchSets [i].Count;
			Vector2 boosterPos = new Vector2();

			//destroy all tiles in all sets
			foreach (Vector2 tPos in matchSets[i]) {


				if ((Grid [(int)tPos.x, (int)tPos.y] == lastTileMoved1 || Grid [(int)tPos.x, (int)tPos.y] == lastTileMoved2) && matchCount == 4) {
					//store that position
					//boosterPos = new Vector2 ((int)tPos.x, (int)tPos.y);
					int tileType = (Grid [(int)tPos.x, (int)tPos.y]).GetComponent<MoveScript>().tileIndex;
					Destroy (Grid [(int)tPos.x, (int)tPos.y]);
					GameObject tile = Instantiate (TilePrefabs [tileType], new Vector2 ((int)tPos.x, (int)tPos.y), Quaternion.identity) as GameObject;
					tile.GetComponent<MoveScript> ().setColour (tileColours[tileType]);
					tile.GetComponent<MoveScript> ().isBooster = true;
					tile.GetComponent<MoveScript> ().changeColor ();
					tile.GetComponent<MoveScript> ().tileIndex = tileType;
					Grid [(int)tPos.x, (int)tPos.y] = tile;


				} else {
					Destroy (Grid [(int)tPos.x, (int)tPos.y]);
					Grid [(int)tPos.x, (int)tPos.y] = null;
				}

			}

		
			/*
			if (matchCount == 4 && boosterPos != null) {

				GameObject tile = Instantiate (boosterPrefabs [0], new Vector2 (boosterPos.x, boosterPos.y), Quaternion.identity) as GameObject;
				Grid [(int)boosterPos.x, (int)boosterPos.y] = tile;
			}
			*/
		}
	}

		
	public void CreateBooster(Vector2 Position){

		//chuck above code in here :)
	}

	public void SwapTiles (GameObject tile1, GameObject tile2)
	{
		lastTileMoved1 = tile1;
		lastTileMoved2 = tile2;


		//swap tiles on screen
		Vector2 tempPos = tile1.transform.position;
		//using iTween to move tiles, the oncomplete method is set to checkmatches once the tiles have been moved
		//iTween.MoveTo (tile1, iTween.Hash ("x", tile2.transform.position.x, "y", tile2.transform.position.y, "time", 1.0f, "oncomplete", "UpdateGridArray", "oncompletetarget", gameObject));
		iTween.MoveTo (tile1, iTween.Hash ("x", tile2.transform.position.x, "y", tile2.transform.position.y, "time", swapTime));
		iTween.MoveTo (tile2, iTween.Hash ("x", tempPos.x, "y", tempPos.y, "time", swapTime));


	}

	public void ReplaceTiles ()
	{

		List<GameObject> newTiles = new List<GameObject> ();

		for (int x = 0; x < GridWidth; x++) {
			int missingTileCount = 0;
			for (int y = 0; y < GridHeight; y++) {
				//Debug.Log ("gridvalue: " + Grid [x, y]);
				if (Grid [x, y] == null) {
					missingTileCount++;
				}
			}

			//instantiate new tiles
			for (int i = 0; i < missingTileCount; i++) {
				int randomTileID = Random.Range (0, TilePrefabs.Length);
				GameObject tile = Instantiate (TilePrefabs [randomTileID], new Vector2 (x, GridHeight + i), Quaternion.identity) as GameObject;
				tile.GetComponent<MoveScript> ().setColour (tileColours [randomTileID]);
				tile.GetComponent<MoveScript> ().tileIndex = randomTileID;
				newTiles.Add (tile);
			}
		}

		foreach (GameObject t in Grid) {

			if (t != null)
				t.GetComponent<MoveScript> ().GravityCheck ();
		
		}

		foreach (GameObject m in newTiles) {
			m.GetComponent<MoveScript> ().GravityCheck ();
			
		}
	}

	public void UpdateGridArray ()
	{

		for (int y = 0; y < GridHeight; y++) {
			for (int x = 0; x < GridWidth; x++) {
				
				RaycastHit2D hit = Physics2D.Raycast (new Vector2 (x, y), Vector2.zero, 0f);
				if (hit) {
					Grid [x, y] = hit.collider.gameObject as GameObject;
					//Debug.Log ("hit at: " + x + "," + y);
				}
			}
		}
	}

	public IEnumerator continousCheck ()
	{

		List<Vector2>[] matches;
		do {
			UpdateGridArray (); //update grid
			matches = getMatches (); //Retrieve any new matches
			DestroyTiles (matches);	//Destroy tiles from matches		
			ReplaceTiles ();			//Replace these tiles
			yield return new WaitForSeconds (dropTime - 0.3f); //wait for new tiles to drop
		} while(matches [0] != null);


		PrintGrid (Grid);
		playerinput.currentState = GameState.None;

		//if no more moves are possible then we need to reshuffle/recreate the grid
		if (checkForPossibleMoves () == false) {
			ReplaceGrid ();
		}



	
	}

	bool checkForPossibleMoves ()
	{


		//this is where we should check if there are any more available moves
		//PrintGrid (TempGrid);

		//swap with neighbour to the right
		for (int x = 0; x <= 6; x++) {
			for (int y = 7; y >= 0; y--) {
				GameObject[,] TempGrid = new GameObject[GridWidth, GridHeight];
				CopyArray (Grid, TempGrid);
				//swap tiles
				GameObject tempTile = TempGrid [x, y];
				TempGrid [x, y] = TempGrid [x + 1, y];
				TempGrid [x + 1, y] = tempTile;



				//for now, set the grid equal to the tempgrid so we can use the getmatches method, swap back straight away
				GameObject[,] fakeGrid = Grid;
				Grid = TempGrid;

				List<Vector2>[] matches = getMatches ();
				if (matches [0] != null) {
					Debug.Log ("Swap " + x + ":" + y + "with " + (x + 1) + ":" + y);
					Debug.Log ("there are potential HORIZONTAL moves");
					DisplayMoves (Grid [x, y], Grid [x + 1, y]);
					return true;
				} else {
					//Debug.Log ("no more HORIZONTAL moves");
				}
				Grid = fakeGrid;


			}
		}



		//now swap with neighbor underneath
		for (int x = 0; x <= 7; x++) {
			for (int y = 7; y >= 1; y--) {
				GameObject[,] TempGrid = new GameObject[GridWidth, GridHeight];
				CopyArray (Grid, TempGrid);
				//swap tiles
				GameObject tempTile = TempGrid [x, y];
				TempGrid [x, y] = TempGrid [x, y - 1];
				TempGrid [x, y - 1] = tempTile;

				//for now, set the grid equal to the tempgrid so we can use the getmatches method, swap back straight away
				GameObject[,] fakeGrid = Grid;

				Grid = TempGrid;
				List<Vector2>[] matches = getMatches ();
				if (matches [0] != null) {
					Debug.Log ("Swap " + x + ":" + y + "with " + x + ":" + (y - 1));
					Debug.Log ("there are potential Vertical moves");
					DisplayMoves (Grid [x, y], Grid [x, y - 1]);
					return true;
				} else {
					//Debug.Log ("no more Vertical moves");
				}
				Grid = fakeGrid;

			}
		}


		return false;
	}

	void DisplayMoves (GameObject firstTile, GameObject secondTile)
	{

		firstTile.GetComponent<MoveScript> ().Flash ();
		secondTile.GetComponent<MoveScript> ().Flash ();

	}



	void CopyArray (GameObject[,] from, GameObject[,] to)
	{

		for (int x = 0; x <= 7; x++) {
			for (int y = 0; y <= 7; y++) {
				to [x, y] = from [x, y];
			}
		}

	}

	void PrintGrid (GameObject[,]  thisGrid)
	{
		
		string gridlayout = "";
		
		for (int y = GridHeight - 1; y >= 0; y--) {
			
			gridlayout += ("Row " + y + ": ");
			for (int x = 0; x < GridWidth; x++) {
				gridlayout += (thisGrid [x, y].GetComponent<MoveScript> ().getColour () + ", ");
			}
			
			gridlayout += ("\n");
		}
		
		Debug.Log (gridlayout);

	}
}

public enum GameState
{

	None,
	Animating,
	selectionStarted
}
