// ICON ATTRIBUTIONS
//Water bottle icon made by Madebyoliver from flaticon.com
/* Place the attribution on the credits/description page of the application*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

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
	Vector2 lastTileMoved1;
	Vector2 lastTileMoved2;

	PlayerInput playerinput;
	ScoreHandler scorehandler;


	void Awake ()
	{

		//set last tiles moved to a position not included in the grid
		lastTileMoved1 = new Vector2 (-1, -1);
		lastTileMoved2 = new Vector2 (-1, -1);

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

		if (Application.loadedLevel == 2) {
			StartCoroutine (CreateGridTest ());
		} else {
			StartCoroutine (CreateGrid ());
		}
	


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
				Grid [x, y].GetComponent<MoveScript> ().setName (tileColours [randomTile]);
				//give it its position in the grid
				Grid [x, y].GetComponent<MoveScript> ().setGridPosition (new Vector2 (x, y));


				yield return new WaitForSeconds (.02f);
			}
		}


		StartCoroutine (continousCheck ());

	}

	IEnumerator CreateGridTest ()
	{
		//L shape
		/*
		int[,] testGrid = { {4,5,6,1,2,3,0,3},
							{3,4,6,1,2,5,5,3},
							{2,2,4,0,1,1,5,2},
							{4,5,6,1,2,3,0,3},
							{5,6,5,2,2,6,5,5},
							{3,2,4,2,3,5,5,0},
							{4,6,2,3,5,4,6,0},
							{2,0,5,5,0,5,2,4}}; 
							*/

		//T shape
		/*
		int[,] testGrid = { {4,5,6,4,2,3,0,3},
							{3,4,6,1,2,5,5,3},
							{2,2,1,0,1,1,5,2},
							{4,5,6,1,2,3,0,3},
							{5,6,5,2,2,6,5,5},
							{3,2,4,2,3,5,5,0},
							{4,6,2,3,5,4,6,0},
							{2,0,5,5,0,5,2,4}}; 
							*/

		//5 in a row
	/*
		int[,] testGrid = { {4,5,6,4,2,3,0,3},
							{3,4,6,1,2,5,5,3},
							{2,1,1,0,1,1,5,2},
							{4,5,6,2,2,3,0,3},
							{5,6,5,2,2,6,5,5},
							{3,2,4,2,3,5,5,0},
							{4,6,2,3,5,4,6,0},
							{2,0,5,5,0,5,2,4}}; 

*/
		//4 in a row
		/*
		int[,] testGrid = { { 4, 5, 6, 4, 2, 3, 0, 3 },
			{ 3, 4, 6, 1, 2, 5, 5, 3 },
			{ 2, 4, 1, 0, 1, 1, 5, 2 },
			{ 4, 5, 6, 2, 2, 3, 0, 3 },
			{ 5, 6, 5, 2, 2, 6, 5, 5 },
			{ 3, 2, 4, 2, 3, 5, 5, 0 },
			{ 4, 6, 2, 3, 5, 4, 6, 0 },
			{ 2, 0, 5, 5, 0, 5, 2, 4 }
		}; 
		*/

		//4 after collapse
		/*
		int[,] testGrid = { { 4, 5, 6, 4, 2, 3, 0, 3 },
							{ 3, 4, 6, 2, 2, 5, 5, 3 },
							{ 2, 1, 1, 0, 1, 1, 5, 2 },
							{ 4, 5, 6, 2, 2, 3, 0, 3 },
							{ 5, 6, 3, 2, 2, 6, 5, 5 },
							{ 3, 2, 4, 3, 3, 5, 5, 0 },
							{ 4, 6, 6, 3, 6, 6, 2, 0 },
							{ 2, 0, 5, 5, 0, 5, 2, 4 }
						}; 
						*/

		//2 4's at once

		int[,] testGrid = { { 4, 5, 6, 4, 2, 3, 0, 3 },
			{ 3, 4, 6, 2, 2, 5, 5, 3 },
			{ 2, 1, 1, 5, 1, 2, 5, 2 },
			{ 4, 5, 5, 1, 5, 3, 0, 3 },
			{ 5, 6, 3, 2, 2, 6, 5, 5 },
			{ 3, 5, 4, 3, 3, 5, 5, 0 },
			{ 4, 5, 6, 3, 6, 6, 2, 0 },
			{ 2, 0, 5, 5, 0, 5, 2, 4 }
		}; 



		/*
		int[,] testGrid = { {1,1,1,1,1,1,1,1},
							{1,1,0,1,1,1,1,3},
							{1,1,1,1,1,1,1,1},
							{1,1,0,1,1,1,1,3},
							{1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1}}; 
	*/

		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];

		for (int y = GridHeight - 1; y >= 0; y--) {
			for (int x = 0; x < GridWidth; x++) {
				int randomTile = testGrid [x, y];

				GameObject tile = Instantiate (TilePrefabs [randomTile], new Vector2 (x, y), Quaternion.identity) as GameObject;
				tile.GetComponent<MoveScript> ().tileIndex = randomTile;
				//float vol = Random.Range(0.2f,0.5f);
				//source.PlayOneShot (pop,vol);
				Grid [x, y] = tile;

				//Assign the tile its colour
				Grid [x, y].GetComponent<MoveScript> ().setName (tileColours [randomTile]);
				//give it its position in the grid
				Grid [x, y].GetComponent<MoveScript> ().setGridPosition (new Vector2 (x, y));


				yield return new WaitForSeconds (.02f);
			}
		}


		StartCoroutine (continousCheck ());

	}

	public void CheckPossibleMatches ()
	{
		//ReplaceGrid ();
		//StartCoroutine(DestroyColumn(3));
		if(playerinput.currentState == GameState.None)
		StartCoroutine(DestroyRow(3));

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
		List<Vector2>[] matchSets = new List<Vector2>[100]; 
		index = 0;

		string currentColour = "none";


		//check horizontal matches
		for (int y = 0; y < GridHeight; y++) {
			for (int x = 0; x < GridWidth; x++) {

				if (currentColour != Grid [x, y].GetComponent<MoveScript> ().getName ()) {

					if (matchPositions.Count >= 3) {

						matchSets [index] = new List<Vector2> ();
						for(int i = 0; i < matchPositions.Count; i++) {

							if (Grid [(int)matchPositions [i].x, (int)matchPositions [i].y].GetComponent<MoveScript> ().isBooster) {
								//add column to matchpositions
								for(int row = 0; row < GridHeight; row++){
									matchPositions.Add (new Vector2(row, (int)matchPositions[i].y));
								}
								//add row to matchpositions
								for(int col = 0; col < GridWidth; col++){
									matchPositions.Add(new Vector2((int)matchPositions [i].x, col));
								}
							}
							matchSets [index].Add (matchPositions[i]);
						}
						index++; //change the index
					}

					currentColour = Grid [x, y].GetComponent<MoveScript> ().getName ();
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
				
				if (currentColour != Grid [x, y].GetComponent<MoveScript> ().getName ()) {
					
					if (matchPositions.Count >= 3) {

						matchSets [index] = new List<Vector2> ();
						for(int i = 0; i < matchPositions.Count; i++) {

							if (Grid [(int)matchPositions [i].x, (int)matchPositions [i].y].GetComponent<MoveScript> ().isBooster) {
								//add column to matchpositions
								for(int row = 0; row < GridHeight; row++){
									matchPositions.Add (new Vector2(row, matchPositions[i].y));
								}
								//add row to matchpositions
								for(int col = 0; col < GridWidth; col++){
									matchPositions.Add (new Vector2(matchPositions [i].x, col));
								}
							}
							matchSets [index].Add (matchPositions[i]);
						}
						index++; //change the index
					}
					
					currentColour = Grid [x, y].GetComponent<MoveScript> ().getName ();
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

	public void printMatchSets (List<Vector2>[] matchSets)
	{

		for (int i = 0; i < index; i++) {

			string set = "";
			foreach (Vector2 tPos in matchSets[i]) {
				set += (tPos.x + ":" + tPos.y + ", ");
			}
			Debug.Log (set);
		}


	}

	public void DestroyTiles (List<Vector2>[] matchSets)
	{

		printMatchSets (matchSets);

		if (matchSets [0] != null) {
			source.PlayOneShot (zap);
		}



		for (int i = 0; i < index; i++) {

	

			int matchCount = matchSets [i].Count;
			Vector2 boosterPos = new Vector2 ();
			bool boosterAdded = false;

			//destroy all tiles in all sets
			foreach (Vector2 tPos in matchSets[i]) {


				//Boosters Code
				switch (matchSets [i].Count) {
				case 4:
					if (tPos == lastTileMoved1 || tPos == lastTileMoved2) {
						CreateNormalBooster (tPos);
						boosterAdded = true;
					} else if(tPos == matchSets[i].ElementAt(3) && boosterAdded == false){
						CreateNormalBooster (tPos);
						boosterAdded = true; //should be redundant
					
					}
					else {
						DestroyTile (tPos);	
					}
					break;
				case 5:
					if (tPos == lastTileMoved1 || tPos == lastTileMoved2) {
						CreateSpecialBooster (tPos);
						boosterAdded = true;
					} else if(tPos == matchSets[i].ElementAt(4) && boosterAdded == false){
						CreateSpecialBooster (tPos);
						boosterAdded = true; //should be redundant
					}
					else {
						DestroyTile (tPos);	
					}


					break;
				default: 
					DestroyTile (tPos);	
					break;
				}
			}

			//Award points for match
			switch (matchSets [i].Count) {
			case 1:
				scorehandler.AddPoints (20);
				break;
			case 3:
				scorehandler.AddPoints (100);
				break;
			case 4:
				scorehandler.AddPoints (200);
				break;
			case 5:
				scorehandler.AddPoints (500);
				break;
			case 8:
				//for a deleted row
				scorehandler.AddPoints (1000);
				break;
			default: 
				break;
			}
		}

		//reset last tiles moved
		lastTileMoved1 = new Vector2(-1,-1);
		lastTileMoved2 = new Vector2 (-1, -1);
	}

	public IEnumerator DestroyTilesWithName(string name){

		List<Vector2>[] matchSets = new List<Vector2>[100];
		index = 0;

		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {

				if (Grid[x,y] != null && Grid [x, y].GetComponent<MoveScript> ().getName() == name) {
					matchSets [index] = new List<Vector2> ();
					matchSets [index].Add (new Vector2 (x,y));
					index++;
				
				}
			}
		}

		Debug.Log ("Number of same items: " + index);
		DestroyTiles (matchSets);
		ReplaceTiles ();
		yield return new WaitForSeconds (dropTime - 0.3f); //wait for new tiles to drop
		StartCoroutine (continousCheck());
	}
		
	public void DestroyTile(Vector2 tPos){

		Destroy (Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)]);
		Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)] = null;
	}

	public IEnumerator DestroyColumn(int col){

		for (int row = 0; row < GridWidth; row++) {

			if (Grid [col, row] != null) {
				Destroy (Grid [col, row]);
				Grid [col, row] = null;
			}

		}
			
		ReplaceTiles ();

		yield return new WaitForSeconds (dropTime - 0.3f); //wait for new tiles to drop

		StartCoroutine (continousCheck());
	}
	public IEnumerator DestroyRow(int row){

		for (int col = 0; col < GridWidth; col++) {
			if (Grid [col, row] != null) {
				Destroy (Grid [col, row]);
				Grid [col, row] = null;
			}

		}

		ReplaceTiles ();
		yield return new WaitForSeconds (dropTime - 0.3f); //wait for new tiles to drop
		StartCoroutine (continousCheck());
	}


	public void CreateNormalBooster (Vector2 tPos)
	{

		int tileType = (Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)]).GetComponent<MoveScript> ().tileIndex;
		Destroy (Grid [Mathf.RoundToInt(tPos.x),Mathf.RoundToInt(tPos.y)]);
		GameObject tile = Instantiate (boosterPrefabs [tileType], new Vector2 (Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)), Quaternion.identity) as GameObject;
		tile.GetComponent<MoveScript> ().setName (tileColours [tileType]);
		tile.GetComponent<MoveScript> ().isBooster = true;
		//tile.GetComponent<MoveScript> ().changeColor (Color.blue);
		tile.GetComponent<MoveScript> ().tileIndex = tileType;
		Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)] = tile;


	}

	public void CreateSpecialBooster (Vector2 tPos)
	{

	
		Destroy (Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)]);
		GameObject tile = Instantiate (boosterPrefabs[7], new Vector2 (Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)), Quaternion.identity) as GameObject;
		tile.GetComponent<MoveScript> ().setName ("water");
		tile.GetComponent<MoveScript> ().isSpecialBooster= true;
		Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)] = tile;


	}

	public void SwapTiles (GameObject tile1, GameObject tile2)
	{
		lastTileMoved1 = new Vector2 (Mathf.RoundToInt(tile1.transform.position.x),Mathf.RoundToInt( tile1.transform.position.y));
		lastTileMoved2 = new Vector2 (Mathf.RoundToInt(tile2.transform.position.x), Mathf.RoundToInt(tile2.transform.position.y));


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
				tile.GetComponent<MoveScript> ().setName (tileColours [randomTileID]);
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
				gridlayout += (thisGrid [x, y].GetComponent<MoveScript> ().getName () + ", ");
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
