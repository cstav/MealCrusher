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
	public float swapTime = 0.2f;
	public float dropTime = 0.35f;
	public int index;
	public GameObject[,] fatGrid;
	public GameObject fattyBlock;
	int fatLeft = 0;
	//to toggle fat blocks on and off
	bool fatOn = false;
	//to toggle ciggarettes on and off
	bool cigOn = true;
	//for the game to play by itself
	bool automate = false;
	public GameObject[] feedback;
	public GameObject explosion;
	int cigCount = 2;
	//number of cigarettes on the grid at a given time

	//two tiles that were last swapped
	Vector2 lastTileMoved1;
	Vector2 lastTileMoved2;

	PlayerInput playerinput;
	ScoreHandler scorehandler;

	/*
Methods that i need for the ciggarettes

int cigsCount;
int surroundingCigs;
bool CigsNearby();


Maybe when you retreive the matches, we should loop through those matches and do a CheckForAdjCigs() on all of them
The int surroundingCigs can be incremented everytime checkForAdjCigs returns true.

if we have reached the end of the list and surroundcigs = 0 but cigCount > 0, then we need to respawn a new cigarette
We spawn a cigarette by deleting the existing tile and replacing it with a cigarette

When we respawn a new cigarette, we want it to be situated adjacent to an existing cigarette, and increment the cigs count
In order to keep these adjacent maybe we should:
		next available horizontal position
		next available vertical position
		

if we have reached the end of the list and surroundingcigs > 0, me must destroy the adjacent cigarretes and decrement the cigsCount
accordingly

After destroying these tiles, they will need to be replaced

**NOTE**
Cigarettes should never move position so they should never respond to gravity
This also means that, when other tiles are checking missing tiles below, cigarettes should act as a missing tile



*/

	List<Vector2> GetAdjCigs (Vector2 tPos)
	{
		List<Vector2> adjacentCigs = new List<Vector2> ();
		int x = Mathf.RoundToInt (tPos.x);
		int y = Mathf.RoundToInt (tPos.y);

		//check above
		if (y < 7) {
			if (Grid [x, y + 1].GetComponent<MoveScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x, y + 1));
			}
		}
		//check below
		if (y > 0) {
			if (Grid [x, y - 1].GetComponent<MoveScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x, y - 1));
			}
		}
		//check right
		if (x < 7) {
			if (Grid [x + 1, y].GetComponent<MoveScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x + 1, y));
			}
		}
		//check left
		if (x > 0) {
			if (Grid [x - 1, y].GetComponent<MoveScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x - 1, y));
			}
		}

		return adjacentCigs;
	}

	void Awake ()
	{
		if (fatOn == true) {
			CreateFatBlocks ();
		}

		//set last tiles moved to a position not included in the grid
		lastTileMoved1 = new Vector2 (-1, -1);
		lastTileMoved2 = new Vector2 (-1, -1);

		scorehandler = GameObject.Find ("scoretext").GetComponent<ScoreHandler> ();
		playerinput = GameObject.Find ("GameController").GetComponent<PlayerInput> ();

		source = gameObject.GetComponent<AudioSource> ();
		tileColours = new string[7];
		tileColours [0] = "beer";
		tileColours [1] = "strawberry";
		tileColours [2] = "cutlet";
		tileColours [3] = "hamburger";
		tileColours [4] = "milk";
		tileColours [5] = "pepper";
		tileColours [6] = "ciggy";

		if (Application.loadedLevel == 2) {
			StartCoroutine (CreateGridTest ());
		} else {
			StartCoroutine (CreateGrid ());
		}
	


	}

	void CreateFatBlocks ()
	{
		fatGrid = new GameObject[GridWidth, GridHeight];

		int[,] fatPositions = {

			{ 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 1, 1, 1, 1, 0, 0 },
			{ 0, 0, 1, 1, 1, 1, 0, 0 },
			{ 0, 0, 1, 1, 1, 1, 0, 0 },
			{ 0, 0, 1, 1, 1, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0 }
		};



		for (int x = 0; x < GridHeight; x++) {
			for (int y = 0; y < GridWidth; y++) {
				if (fatPositions [x, y] == 1) {
					GameObject fatblock = Instantiate (fattyBlock, new Vector2 (x, y), Quaternion.identity) as GameObject;
					fatGrid [x, y] = fatblock;
					fatLeft++;
				}

			}
		}


	}

	IEnumerator CreateGrid ()
	{
		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];
		
		for (int y = GridHeight - 1; y >= 0; y--) {
			for (int x = 0; x < GridWidth; x++) {
				int randomTile = Random.Range (0, TilePrefabs.Length - 1);

				GameObject tile = Instantiate (TilePrefabs [randomTile], new Vector2 (x, y), Quaternion.identity) as GameObject;
				tile.GetComponent<MoveScript> ().tileIndex = randomTile;
				Grid [x, y] = tile;

				//Assign the tile a name
				Grid [x, y].GetComponent<MoveScript> ().setName (tileColours [randomTile]);

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

		int[,] testGrid5 = {{ 4, 5, 6, 4, 2, 3, 0, 3 },
			{ 3, 4, 6, 1, 2, 5, 5, 3 },
			{ 2, 1, 1, 0, 1, 1, 5, 2 },
			{ 4, 5, 6, 2, 2, 3, 0, 3 },
			{ 5, 6, 5, 2, 2, 6, 5, 5 },
			{ 3, 2, 4, 0, 3, 5, 5, 0 },
			{ 4, 6, 2, 3, 5, 4, 6, 0 },
			{ 2, 0, 5, 5, 0, 5, 2, 4 }
		}; 


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

				//change this back later----------------------------------------------
				int randomTile;
				if (Application.loadedLevel == 1) {
					randomTile = testGrid5 [x, y];
				} else {
					randomTile = testGrid [x, y];
				}


				GameObject tile = Instantiate (TilePrefabs [randomTile], new Vector2 (x, y), Quaternion.identity) as GameObject;
				tile.GetComponent<MoveScript> ().tileIndex = randomTile;
				Grid [x, y] = tile;

				//Assign the tile a name
				Grid [x, y].GetComponent<MoveScript> ().setName (tileColours [randomTile]);


				yield return new WaitForSeconds (.02f);
			}
		}


		StartCoroutine (continousCheck ());

	}

	public void SpaceBarFunction ()
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
		//changed already-----------------------------------------------------------------------------------------
		//StartCoroutine (CreateGrid ());
		StartCoroutine (CreateGridTest ());
	}

	bool checkForBooster (Vector2 pos)
	{

		if (Grid [(int)pos.x, (int)pos.y].GetComponent<MoveScript> ().isBooster) {
			return true;
		} else {
			return false;
		}
	}

	public List<Vector2> getRowCol (Vector2 pos)
	{

		List<Vector2> rowCol = new List<Vector2> ();


		int col = (int)pos.x;

		//add column
		for (int r = 0; r < GridHeight; r++) {
			rowCol.Add (new Vector2 (col, r));
		}

		int row = (int)pos.y;

		//add row
		for (int c = 0; c < GridWidth; c++) {
			if (!rowCol.Contains (new Vector2 (c, row))) {
				rowCol.Add (new Vector2 (c, row));
			}
		}
		
		//print for debugging
		string tiles = "Debugging";
		foreach (Vector2 tile in rowCol) {
			tiles += tile.x + ":" + tile.y + ", ";
		}
		Debug.Log (tiles);

		return rowCol;
	}

	public List<Vector2>[] getMatches ()
	{
		List<Vector2> matchPositions = new List<Vector2> ();

		//contains sets of matching tiles
		List<Vector2>[] matchSets = new List<Vector2>[100]; 
		index = 0;

		string currentName = "none";


		//check horizontal matches
		for (int y = 0; y < GridHeight; y++) {
			for (int x = 0; x < GridWidth; x++) {
				
				if (currentName != Grid [x, y].GetComponent<MoveScript> ().getName ()) {

					if (matchPositions.Count >= 3) {

						matchSets [index] = new List<Vector2> ();
						foreach (Vector2 match in matchPositions) {

							if (!matchSets [index].Contains (match)) {
								matchSets [index].Add (match);
							}
							
							if (checkForBooster (match)) {
								//get col and row of booster
								List<Vector2> rowCol = getRowCol (match);
								foreach (Vector2 item in rowCol) {
									if (!matchSets [index].Contains (item)) {
										matchSets [index].Add (item);
									}
								}

							}

						}
						index++; //change the index
					}

					currentName = Grid [x, y].GetComponent<MoveScript> ().getName ();
					matchPositions.Clear ();
					if (currentName != "ciggy")
						matchPositions.Add (new Vector2 (x, y));
				} else {
					if (currentName != "ciggy")
						matchPositions.Add (new Vector2 (x, y));
				}

			}
			//at the end set colour back to none
			currentName = "none";

		}

		//after scanning the whole grid we need to check if 3 or more tiles matched again...
		Debug.Log ("current name: " + currentName);

		if (matchPositions.Count >= 3) {
			matchSets [index] = new List<Vector2> (); //create new list

			//add all matches to new list
			foreach (Vector2 match in matchPositions) {
				matchSets [index].Add (match);
			}

			index++; //change the index
		}



		//check vertical matches
		currentName = "none";
		
		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {

				if (currentName != Grid [x, y].GetComponent<MoveScript> ().getName ()) {
					
					if (matchPositions.Count >= 3) {

						matchSets [index] = new List<Vector2> ();
						foreach (Vector2 match in matchPositions) {

							//ensures there are no duplicates
							if (!matchSets [index].Contains (match)) {
								matchSets [index].Add (match);
							}

							if (checkForBooster (match)) {
								//get col and row of booster
								List<Vector2> rowCol = getRowCol (match);
								foreach (Vector2 item in rowCol) {
									if (!matchSets [index].Contains (item)) {
										matchSets [index].Add (item);
									}
								}

							}

						}
						index++; //change the index
					}
					
					currentName = Grid [x, y].GetComponent<MoveScript> ().getName ();
					matchPositions.Clear ();
					if (currentName != "ciggy")
						matchPositions.Add (new Vector2 (x, y));
				} else {
					if (currentName != "ciggy")
						matchPositions.Add (new Vector2 (x, y));
				}
				
			}
			//at the end set colour back to none
			currentName = "none";
			
		}
			
		//after scanning the whole grid we need to check if 3 or more tiles matched again...
		Debug.Log ("current name: " + currentName);

		if (matchPositions.Count >= 3) {

			matchSets [index] = new List<Vector2> ();
			foreach (Vector2 match in matchPositions) {

				if (!matchSets [index].Contains (match)) {
					matchSets [index].Add (match);
				}

				if (checkForBooster (match)) {
					//Debug.Log ("Yeah one of the tiles in the match was a booster: " + match.x + ":" + match.y);

					//get col and row of booster
					List<Vector2> rowCol = getRowCol (match);
					foreach (Vector2 item in rowCol) {
						if (!matchSets [index].Contains (item)) {
							matchSets [index].Add (item);
						}
					}

				}

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

		//printMatchSets (matchSets);

		if (matchSets [0] != null) {
			source.PlayOneShot (zap);
		}



		for (int i = 0; i < index; i++) {

	

			int matchCount = matchSets [i].Count;
			//Vector2 boosterPos = new Vector2 ();
			bool boosterAdded = false;

			//destroy all tiles in all sets
			foreach (Vector2 tPos in matchSets[i]) {


				//Boosters Code
				switch (matchSets [i].Count) {
				case 4:
					if (tPos == lastTileMoved1 || tPos == lastTileMoved2) {
						CreateNormalBooster (tPos);
						boosterAdded = true;
					} else if (tPos == matchSets [i].ElementAt (3) && boosterAdded == false) {
						CreateNormalBooster (tPos);
						boosterAdded = true; //should be redundant
					
					} else {
						DestroyTile (tPos);	
					}
					break;
				case 5:
					if (tPos == lastTileMoved1 || tPos == lastTileMoved2) {
						CreateSpecialBooster (tPos);
						boosterAdded = true;
					} else if (tPos == matchSets [i].ElementAt (4) && boosterAdded == false) {
						CreateSpecialBooster (tPos);
						boosterAdded = true; //should be redundant
					} else {
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
			case 15:
				//for a deleted row and col
				scorehandler.AddPoints (10000);
				break;
			default: 
				break;
			}
		}

		//reset last tiles moved
		lastTileMoved1 = new Vector2 (-1, -1);
		lastTileMoved2 = new Vector2 (-1, -1);
	}

	public IEnumerator DestroyTilesWithName (string name)
	{

		List<Vector2>[] matchSets = new List<Vector2>[100];
		index = 0;

		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {

				if (Grid [x, y] != null && Grid [x, y].GetComponent<MoveScript> ().getName () == name) {
					matchSets [index] = new List<Vector2> ();
					matchSets [index].Add (new Vector2 (x, y));
					index++;
				
				}
			}
		}

		//Debug.Log ("Number of same items: " + index);
		DestroyTiles (matchSets);
		yield return new WaitForSeconds (0.2f);
		ReplaceTiles ();
		yield return new WaitForSeconds (dropTime - 0.3f); //wait for new tiles to drop
		StartCoroutine (continousCheck ());
	}

	public void DestroyTile (Vector2 tPos)
	{
		

		GameObject e = Instantiate (explosion, new Vector2 (tPos.x, tPos.y), Quaternion.identity) as GameObject;

		Destroy (Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)]);
		Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = null;

		if (fatOn) {
			if (fatGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] != null) {
				Destroy (fatGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)]);
				fatGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = null;

			}
		}
	}

	public void DisplayFeedback (int type)
	{
		GameObject f = Instantiate (feedback [type], new Vector2 (3.5f, 3.5f), Quaternion.identity) as GameObject;
	}



	public void CreateNormalBooster (Vector2 tPos)
	{
		DisplayFeedback (0);
		int tileType = (Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)]).GetComponent<MoveScript> ().tileIndex;

		DestroyTile (new Vector2 (tPos.x, tPos.y));
		//Destroy (Grid [Mathf.RoundToInt(tPos.x),Mathf.RoundToInt(tPos.y)]);
		GameObject tile = Instantiate (boosterPrefabs [tileType], new Vector2 (Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)), Quaternion.identity) as GameObject;
		tile.GetComponent<MoveScript> ().setName (tileColours [tileType]);
		tile.GetComponent<MoveScript> ().isBooster = true;
		tile.GetComponent<MoveScript> ().tileIndex = tileType;
		Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = tile;


	}

	public void CreateSpecialBooster (Vector2 tPos)
	{
		DisplayFeedback (0);
		DestroyTile (new Vector2 (tPos.x, tPos.y));
		//Destroy (Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)]);
		GameObject tile = Instantiate (boosterPrefabs [7], new Vector2 (Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)), Quaternion.identity) as GameObject;
		tile.GetComponent<MoveScript> ().setName ("water");
		tile.GetComponent<MoveScript> ().isSpecialBooster = true;
		Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = tile;



	}

	public void CreateCigarette (Vector2 tPos)
	{
		
		DestroyTile (new Vector2 (tPos.x, tPos.y));
		//Destroy (Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)]);
		GameObject tile = Instantiate (TilePrefabs [6], new Vector2 (Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)), Quaternion.identity) as GameObject;
		tile.GetComponent<MoveScript> ().setName ("ciggy");
		Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = tile;

	}

	public void SwapTiles (GameObject tile1, GameObject tile2)
	{
		lastTileMoved1 = new Vector2 (Mathf.RoundToInt (tile1.transform.position.x), Mathf.RoundToInt (tile1.transform.position.y));
		lastTileMoved2 = new Vector2 (Mathf.RoundToInt (tile2.transform.position.x), Mathf.RoundToInt (tile2.transform.position.y));


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
				int randomTileID = Random.Range (0, TilePrefabs.Length - 1);
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
		Debug.Log ("Updating Grid...");

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

	string getTileName (Vector2 tPos)
	{

		return Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)].GetComponent<MoveScript> ().getName ();

	}

	//******* we shouldnt be checking a cigarette for adjacent cigs *********
	bool CigsDestroyed (List<Vector2>[] matches)
	{
		//change this name
		bool dontSpawnCig = true;

		//check if matches are near cigarettes

		List<Vector2> adjacentCigs;
		List<Vector2> cigsToDestroy = new List<Vector2> ();
		for (int i = 0; i < index; i++) {
			foreach (Vector2 tPos in matches[i]) {

				//if a cigarette is in matches we dont want to destroy its adjacent cigarettes
				if (getTileName (tPos) != "ciggy") {
					adjacentCigs = GetAdjCigs (tPos);
					foreach (Vector2 cigarette in adjacentCigs) {
						cigsToDestroy.Add (cigarette);
						//Debug.Log("cig @ " + cigarette.x + ":" + cigarette.y);
					}
				}
			}
		}


		if (cigsToDestroy.Count == 0 && cigCount > 0) {
				
			dontSpawnCig = false;

		} else {
			Debug.Log ("Delete a cigarette");
			foreach (Vector2 cigarette in cigsToDestroy) {
				DestroyTile (cigarette);
			}
		}
		

		return dontSpawnCig;
	}

	void SpawnCig (int row, int col)
	{

		//spawn a cig in the first available cel in the row
		for (int y = row; y < GridWidth; y++) {
			for (int x = col; x < GridHeight; x++) {
				if (Grid [x, y].GetComponent<MoveScript> ().getName () != "ciggy") {


					CreateCigarette (new Vector2 (x, y));


					goto Spawned;
				}
			}
		}

		Spawned: 
		Debug.Log ("");

	}

	public IEnumerator continousCheck ()
	{
		bool firstTime = true;
		List<Vector2>[] matches;

		//update grid
		//retrive matches

		//while matches exist
		//destroy the tiles
		//replace the tiles
		//update the grid





		do {
			if (!firstTime || automate)//to prevent a double wait period
			yield return new WaitForSeconds (dropTime - 0.3f); //wait for new tiles to drop
			UpdateGridArray (); //update grid
			matches = getMatches (); //Retrieve any new matches
			if (matches [0] == null)
				break;

			if (cigOn) {
				if (!CigsDestroyed (matches)) {
					//Debug.Log ("Spawn a cigarette");
					SpawnCig (3, 0);
				}
			}

			Debug.Log ("printing matchsets");
			printMatchSets (matches);
			DestroyTiles (matches);	//Destroy tiles from matches
			yield return new WaitForSeconds (0.2f);
			ReplaceTiles ();			//Replace these tiles
			firstTime = false;
		} while(matches [0] != null);




	





		//PrintGrid (Grid);
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
					//Debug.Log ("Swap " + x + ":" + y + "with " + (x + 1) + ":" + y);

					//now check that no fat surrounds the two tiles
					if (NoFatExists (new Vector2 (x, y), new Vector2 (x + 1, y))) {
						Debug.Log ("there are potential HORIZONTAL moves");
						DisplayMoves (Grid [x, y], Grid [x + 1, y]);
						return true;
					}
				} 
				Grid = fakeGrid;


			}
		}



		//now swap with neighbour underneath
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
					//Debug.Log ("Swap " + x + ":" + y + "with " + x + ":" + (y - 1));

					//now check that no fat surrounds the two tiles
					if (NoFatExists (new Vector2 (x, y), new Vector2 (x, y - 1))) {
						Debug.Log ("there are potential Vertical moves");
						DisplayMoves (Grid [x, y], Grid [x, y - 1]);
						return true;
					}
				} else {
					//Debug.Log ("no more Vertical moves");
				}
				Grid = fakeGrid;

			}
		}


		return false;
	}


	//checks if a fat block should stop the user from making a move
	public bool NoFatExists (Vector2 aPos, Vector2 bPos)
	{
		if (!fatOn) {
			return true;
		}

		if (fatGrid [Mathf.RoundToInt (aPos.x), Mathf.RoundToInt (aPos.y)] == null && fatGrid [Mathf.RoundToInt (bPos.x), Mathf.RoundToInt (bPos.y)] == null) {
			return true;
		}
		return false;

	}

	void DisplayMoves (GameObject firstTile, GameObject secondTile)
	{
		if (automate) {
			SwapTiles (firstTile, secondTile);
			StartCoroutine (continousCheck ());
		} else {
			firstTile.GetComponent<MoveScript> ().Flash ();
			secondTile.GetComponent<MoveScript> ().Flash ();
		}

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
