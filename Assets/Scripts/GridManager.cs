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
	public GameObject ingredientHolder;
	List<Vector3> holderPositions;
	int fatLeft = 0;
	//to toggle fat blocks on and off
	bool fatOn = true;
	//to toggle ciggarettes on and off
	bool cigOn = true;
	//toggle clearing ingredients level
	bool ingredientsOn = true;
	//for the game to play by itself
	public bool automate = false;
	public GameObject[] feedback;
	public GameObject explosion;
	//number of cigarettes on the grid at a given time
	int cigCount = 0;
	public bool cigSpawnAllowed = false;


	//two tiles that were last swapped
	Vector2 lastTileMoved1;
	Vector2 lastTileMoved2;

	PlayerInput playerinput;
	ScoreHandler scorehandler;
	Sounds sounds;

	void Awake ()
	{
		if (fatOn == true) {
			CreateFatBlocks ();
		}
		if (ingredientsOn == true) {
			CreateIngredientHolders (4);
		}

		//set last tiles moved to a position not included in the grid
		lastTileMoved1 = new Vector2 (-1, -1);
		lastTileMoved2 = new Vector2 (-1, -1);

		scorehandler = GameObject.Find ("scoretext").GetComponent<ScoreHandler> ();
		playerinput = GameObject.Find ("GameController").GetComponent<PlayerInput> ();
		sounds = Camera.main.GetComponent<Sounds> ();

		source = gameObject.GetComponent<AudioSource> ();
		tileColours = new string[11];
		tileColours [0] = "beer";
		tileColours [1] = "strawberry";
		tileColours [2] = "cutlet";
		tileColours [3] = "hamburger";
		tileColours [4] = "milk";
		tileColours [5] = "pepper";
		tileColours [6] = "ciggy";
		tileColours [7] = "raspberry";
		tileColours [8] = "aubergine";
		tileColours [9] = "broccoli";
		tileColours [10] = "carrot";


		if (Application.loadedLevel == 2) {
			StartCoroutine (CreateGridTest ());
		} else {
			StartCoroutine (CreateGridTest ());
		}
	


	}

	public void CreateIngredientHolders (int amount)
	{

		holderPositions = new List<Vector3> ();
		float y = -1.3f;
		int startpos = 2;

		for (int x = startpos; x < amount + startpos; x++) {
			Instantiate (ingredientHolder, new Vector2 (x, y), Quaternion.identity);
			holderPositions.Add (new Vector3 (x, y, 0)); //z =0 indicates that holder is unoccupied
		}
	}

	public List<GameObject> getIngredients ()
	{

		List<GameObject> ingredients = new List<GameObject> ();

		//check the bottom row
		for (int x = 0; x < GridWidth; x++) {
			if (Grid [x, 0].GetComponent<MoveScript> ().isIngredient) {
				Debug.Log ("found pepper");
				ingredients.Add (Grid [x, 0]);
			}
		}
		return ingredients;
	}

	public void moveIntoHolder (GameObject ingredient)
	{
		int xPos = Mathf.RoundToInt (ingredient.transform.position.x);
		int yPos = Mathf.RoundToInt (ingredient.transform.position.y);

		for (int i = 0; i < holderPositions.Count; i++) {
			if (holderPositions [i].z == 0) {
				//move to pos with iTween
				Debug.Log ("Moving ingredient");

				sounds.PlaySound ("swoosh");
				ingredient.GetComponent<MoveScript> ().StopFlashing ();

				iTween.MoveTo (ingredient, iTween.Hash ("x", holderPositions [i].x, "y", holderPositions [i].y, "time", swapTime, "oncomplete", "FinishedMovingIntoHolder", "oncompletetarget", gameObject));

				//set holder pos as occupied
				holderPositions [i] = new Vector3 (holderPositions [i].x, holderPositions [i].y, 1);

				//set the grid positions to null so that they can be replaced
				Grid [xPos, yPos] = null;
				break;
			}
		}
	}

	public void FinishedMovingIntoHolder ()
	{
		//ReplaceTiles ();			//Replace these tiles
		//we need this to do another check so having replace tiles here is correct

		
	}

	public void Automatic ()
	{
		if (!automate) {
			automate = true;
		} else {
			automate = false;
		}

	}

	List<Vector2> GetAdjCigs (Vector2 tPos)
	{
		List<Vector2> adjacentCigs = new List<Vector2> ();
		int x = Mathf.RoundToInt (tPos.x);
		int y = Mathf.RoundToInt (tPos.y);

		//check above
		if (y < 7) {
			if (Grid [x, y + 1] != null && Grid [x, y + 1].GetComponent<MoveScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x, y + 1));
			}
		}
		//check below
		if (y > 0) {
			if (Grid [x, y - 1] != null && Grid [x, y - 1].GetComponent<MoveScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x, y - 1));
			}
		}
		//check right
		if (x < 7) {
			if (Grid [x + 1, y] != null && Grid [x + 1, y].GetComponent<MoveScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x + 1, y));
			}
		}
		//check left
		if (x > 0) {
			if (Grid [x - 1, y] != null && Grid [x - 1, y].GetComponent<MoveScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x - 1, y));
			}
		}

		return adjacentCigs;
	}

	void CreateFatBlocks ()
	{
		fatGrid = new GameObject[GridWidth, GridHeight];

		int[,] fatPositions = {

			{ 0, 0, 0, 0, 1, 1, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0 }
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
			
		//StartCoroutine (continousCheck ());
		StartCoroutine( Check ());

	}

	IEnumerator CreateGridTest ()
	{



		//L shape
		int[,] testGrid1 = {{ 4, 5, 6, 1, 2, 3, 0, 3 },
			{ 3, 4, 6, 1, 2, 5, 5, 3 },
			{ 2, 2, 4, 0, 1, 1, 5, 2 },
			{ 4, 5, 6, 1, 2, 3, 0, 3 },
			{ 5, 6, 5, 2, 2, 6, 5, 5 },
			{ 3, 2, 4, 2, 3, 5, 5, 0 },
			{ 4, 6, 2, 3, 5, 4, 6, 0 },
			{ 2, 0, 5, 5, 0, 5, 2, 4 }
		}; 


		//T shape
		int[,] testGrid2 = {{ 4, 5, 6, 4, 2, 3, 0, 3 },
			{ 3, 4, 6, 1, 2, 5, 5, 3 },
			{ 2, 2, 1, 0, 1, 1, 5, 2 },
			{ 4, 5, 6, 1, 2, 3, 0, 3 },
			{ 5, 6, 5, 2, 2, 6, 5, 5 },
			{ 3, 2, 4, 2, 3, 5, 5, 0 },
			{ 4, 6, 2, 3, 5, 4, 6, 0 },
			{ 2, 0, 5, 5, 0, 5, 2, 4 }
		}; 
					

		//5 in a row
		int[,] testGrid3 = {{ 4, 5, 6, 4, 2, 3, 0, 4 },
			{ 3, 4, 6, 1, 2, 1, 0, 3 },
			{ 2, 1, 1, 0, 1, 1, 2, 1 },
			{ 4, 5, 6, 2, 2, 3, 0, 3 },
			{ 4, 6, 1, 2, 2, 6, 0, 2 },
			{ 3, 5, 4, 0, 3, 4, 1, 0 },
			{ 4, 6, 2, 3, 2, 4, 6, 1 },
			{ 2, 0, 0, 1, 0, 3, 2, 4 }
		}; 

		
		//cigarette drop test
		int[,] testGrid4 = {{ 4, 5, 2, 6, 2, 3, 0, 3 },
			{ 3, 4, 4, 6, 2, 5, 5, 3 },
			{ 2, 1, 1, 6, 1, 1, 5, 2 },
			{ 2, 5, 2, 6, 2, 3, 0, 3 },
			{ 2, 3, 5, 6, 2, 1, 5, 5 },
			{ 2, 2, 4, 6, 3, 5, 5, 0 },
			{ 2, 0, 2, 6, 5, 4, 4, 0 },
			{ 4, 0, 5, 6, 0, 5, 2, 4 }
		}; 


		//no possible matches except special booster
		int[,] testGrid5 = {{ 1, 4, 2, 5, 3, 1, 4, 2 },
			{ 2, 5, 3, 1, 4, 2, 5, 4 },
			{ 3, 1, 4, 2, 5, 3, 1, 4 },
			{ 4, 2, 5, 3, 1, 4, 2, 4 },
			{ 5, 3, 1, 4, 2, 5, 3, 4 },
			{ 1, 4, 2, 5, 3, 1, 4, 4 },
			{ 2, 5, 3, 1, 4, 2, 5, 3 },
			{ 3, 1, 4, 2, 5, 3, 1, 4 }
		}; 

		//4 in a row
		int[,] testGrid6 = {{ 4, 5, 6, 4, 2, 3, 0, 3 },
			{ 3, 4, 6, 1, 2, 5, 5, 3 },
			{ 2, 4, 1, 0, 1, 1, 5, 2 },
			{ 4, 5, 6, 2, 2, 3, 0, 3 },
			{ 5, 6, 5, 2, 2, 6, 5, 5 },
			{ 3, 2, 4, 2, 3, 5, 5, 0 },
			{ 4, 6, 2, 3, 5, 4, 6, 0 },
			{ 2, 0, 5, 5, 0, 5, 2, 4 }
		}; 


		//4 after collapse
		int[,] testGrid7 = {{ 4, 5, 6, 4, 2, 3, 0, 3 },
			{ 3, 4, 6, 2, 2, 5, 5, 3 },
			{ 2, 1, 1, 0, 1, 1, 5, 2 },
			{ 4, 5, 6, 2, 2, 3, 0, 3 },
			{ 5, 6, 3, 2, 2, 6, 5, 5 },
			{ 3, 2, 4, 3, 3, 5, 5, 0 },
			{ 4, 6, 6, 3, 6, 6, 2, 0 },
			{ 2, 0, 5, 5, 0, 5, 2, 4 }
		}; 
					

		//2 4's at once
		int[,] testGrid8 = {{ 4, 5, 6, 4, 2, 3, 0, 3 },
			{ 3, 4, 6, 2, 2, 5, 5, 3 },
			{ 2, 1, 1, 5, 1, 2, 5, 2 },
			{ 4, 5, 5, 1, 5, 3, 0, 3 },
			{ 5, 6, 3, 2, 2, 6, 5, 5 },
			{ 3, 5, 4, 3, 3, 5, 5, 0 },
			{ 4, 5, 6, 3, 6, 6, 2, 0 },
			{ 2, 0, 5, 5, 0, 5, 2, 4 }
		}; 

		//Generic Test Grid
		int[,] testGrid9 = {{ 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 0, 1, 1, 1, 1, 3 },
			{ 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 0, 1, 1, 1, 1, 3 },
			{ 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1 }
		}; 

		//beer test
		int[,] testGrid10 = {{ 4, 5, 6, 4, 2, 3, 0, 3 },
			{ 3, 4, 6, 2, 2, 5, 5, 3 },
			{ 2, 1, 1, 5, 1, 2, 5, 2 },
			{ 4, 5, 5, 1, 5, 3, 0, 3 },
			{ 5, 6, 3, 2, 2, 6, 5, 5 },
			{ 3, 5, 4, 0, 3, 5, 5, 0 },
			{ 4, 5, 6, 0, 6, 6, 2, 0 },
			{ 2, 0, 5, 0, 0, 5, 2, 4 }
		}; 

		//Ultimate test
		int[,] testGrid11 = {{ 2, 8, 6, 5, 1, 1, 2, 1 },
							{ 2, 1, 6, 9, 5, 5, 1, 2 },
							{ 1, 2, 6, 2, 2, 0, 5, 5 },
							{ 1, 4, 6, 0, 1, 2, 4, 4 },
							{ 4, 1, 6, 3, 1, 0, 4, 2 },
							{ 1, 7, 6, 3, 10, 1, 2, 4 },
							{ 4, 2, 6, 2, 2, 1, 4, 2 },
							{ 4, 3, 6, 3, 2, 0, 4, 1 }
							}; 

		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];

		for (int y = GridHeight - 1; y >= 0; y--) {
			for (int x = 0; x < GridWidth; x++) {

				//change this back later----------------------------------------------
				int randomTile;
				if (Application.loadedLevel == 1) {
					randomTile = testGrid11[x, y];
				} else {
					randomTile = testGrid8 [x, y];
				}

				if (randomTile == 6) {
					cigCount++;
				}
				GameObject tile = Instantiate (TilePrefabs [randomTile], new Vector2 (x, y), Quaternion.identity) as GameObject;
				tile.GetComponent<MoveScript> ().tileIndex = randomTile;

				//if pepper will make an ingredient
				if (randomTile >=7) {
					tile.GetComponent<MoveScript> ().isIngredient = true;
				}
				Grid [x, y] = tile;

				//Assign the tile a name
				Grid [x, y].GetComponent<MoveScript> ().setName (tileColours [randomTile]);


				yield return new WaitForSeconds (.02f);
			}
		}


		//StartCoroutine (continousCheck ());
		StartCoroutine( Check ());

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

	bool CheckGridForSpecBooster ()
	{

		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {
				if (Grid [x, y].GetComponent<MoveScript> ().isSpecialBooster) {
					return true;
				}
			}
		}
		return false;
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
		
//		//print for debugging
//		string tiles = "Debugging";
//		foreach (Vector2 tile in rowCol) {
//			tiles += tile.x + ":" + tile.y + ", ";
//		}
//		Debug.Log (tiles);

		return rowCol;
	}

	public List<Vector2>[] getMatches (GameObject[,] gridToCheck)
	{
		List<Vector2> matchPositions = new List<Vector2> ();

		//contains sets of matching tiles
		List<Vector2>[] matchSets = new List<Vector2>[100]; 
		index = 0;

		string currentName = "none";


		//check horizontal matches
		for (int y = 0; y < GridHeight; y++) {
			for (int x = 0; x < GridWidth; x++) {
				
				if (currentName != gridToCheck [x, y].GetComponent<MoveScript> ().getName ()) {

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

					currentName = gridToCheck [x, y].GetComponent<MoveScript> ().getName ();
					matchPositions.Clear ();
					if (currentName != "ciggy" && currentName!= "beer")
						matchPositions.Add (new Vector2 (x, y));
				} else {
					if (currentName != "ciggy" && currentName!= "beer")
						matchPositions.Add (new Vector2 (x, y));
				}
			}
			//at the end set colour back to none
			currentName = "none";
		}

		//after scanning the whole grid we need to check if 3 or more tiles matched again..
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

				if (currentName != gridToCheck [x, y].GetComponent<MoveScript> ().getName ()) {
					
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
					
					currentName = gridToCheck [x, y].GetComponent<MoveScript> ().getName ();
					matchPositions.Clear ();
					if (currentName != "ciggy" && currentName!= "beer")
						matchPositions.Add (new Vector2 (x, y));
				} else {
					if (currentName != "ciggy" && currentName!= "beer")
						matchPositions.Add (new Vector2 (x, y));
				}
			}
			//at the end set colour back to none
			currentName = "none";
		}
			
		//after scanning the whole grid we need to check if 3 or more tiles matched again...
		if (matchPositions.Count >= 3) {

			matchSets [index] = new List<Vector2> ();
			foreach (Vector2 match in matchPositions) {

				if (!matchSets [index].Contains (match)) {
					matchSets [index].Add (match);
				}

				if (checkForBooster (match)) {
					//Debug.Log ("One of the tiles in the match was a booster: " + match.x + ":" + match.y);

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
						DestroyTile (tPos, true);	
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
						DestroyTile (tPos, true);	
					}
					break;
				default: 
					DestroyTile (tPos, true);	
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
				scorehandler.AddPoints (2000);
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


		if (cigOn) {
			if (!CigsDestroyed (matchSets)) {
				//Debug.Log ("Spawn a cigarette");
					
					SpawnCig (3, 0);
			}
		}

		DestroyTiles (matchSets);
		yield return new WaitForSeconds (0.2f);
		ReplaceTiles ();

	}

	public void DestroyTile (Vector2 tPos, bool explode)
	{
		
		if (explode) {
			GameObject e = Instantiate (explosion, new Vector2 (tPos.x, tPos.y), Quaternion.identity) as GameObject;
		} 

		if (getTileName (tPos) == "ciggy") {
			cigCount--;
		}

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
		Instantiate (feedback [type], new Vector2 (3.51f, 3.5f), Quaternion.identity);
	}

	public void CreateNormalBooster (Vector2 tPos)
	{
		DisplayFeedback (0);
		int tileType = (Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)]).GetComponent<MoveScript> ().tileIndex;

		DestroyTile (new Vector2 (tPos.x, tPos.y), true);
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
		DestroyTile (new Vector2 (tPos.x, tPos.y), true);
		//Destroy (Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)]);
		GameObject tile = Instantiate (boosterPrefabs [7], new Vector2 (Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)), Quaternion.identity) as GameObject;
		tile.GetComponent<MoveScript> ().setName ("water");
		tile.GetComponent<MoveScript> ().isSpecialBooster = true;
		Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = tile;
	}

	public void CreateCigarette (Vector2 tPos)
	{

		DestroyTile (new Vector2 (tPos.x, tPos.y), false);
		//Destroy (Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)]);
		GameObject tile = Instantiate (TilePrefabs [6], new Vector2 (Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)), Quaternion.identity) as GameObject;
		cigCount++;
		tile.GetComponent<MoveScript> ().setName ("ciggy");
		Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = tile;
		sounds.PlaySound ("smoke");
	}

	public void SwapTiles (GameObject tile1, GameObject tile2)
	{
		List<GameObject> tiles = new List<GameObject> ();
		tiles.Add (tile1);
		tiles.Add (tile2);

		lastTileMoved1 = new Vector2 (Mathf.RoundToInt (tile1.transform.position.x), Mathf.RoundToInt (tile1.transform.position.y));
		lastTileMoved2 = new Vector2 (Mathf.RoundToInt (tile2.transform.position.x), Mathf.RoundToInt (tile2.transform.position.y));

		//swap tiles on screen
		Vector2 tempPos = tile1.transform.position;
		//using iTween to move tiles, the oncomplete method is set to checkmatches once the tiles have been moved
		//iTween.MoveTo (tile1, iTween.Hash ("x", tile2.transform.position.x, "y", tile2.transform.position.y, "time", 1.0f, "oncomplete", "UpdateGridArray", "oncompletetarget", gameObject));
		iTween.MoveTo (tile1, iTween.Hash ("x", tile2.transform.position.x, "y", tile2.transform.position.y, "time", swapTime));
		iTween.MoveTo (tile2, iTween.Hash ("x", tempPos.x, "y", tempPos.y, "time", swapTime, "oncomplete", "FinishedSwapping", "oncompletetarget", gameObject, "oncompleteparams", tiles));

		/*
		Hashtable param = new Hashtable ();
		Hashtable someVals = new Hashtable ();
		someVals.Add ("myString", "a string");
		someVals.Add ("myvector3", new Vector3 (0, 0, 0));
		param.Add ("oncomplete", "UpdateState");
		param.Add ("oncompleteparams", someVals);
		iTween.MoveTo (tile1, param);
*/
	}

	public void SwapBack (GameObject tile1, GameObject tile2)
	{
		List<GameObject> tiles = new List<GameObject> ();
		tiles.Add (tile1);
		tiles.Add (tile2);

		lastTileMoved1 = new Vector2 (Mathf.RoundToInt (tile1.transform.position.x), Mathf.RoundToInt (tile1.transform.position.y));
		lastTileMoved2 = new Vector2 (Mathf.RoundToInt (tile2.transform.position.x), Mathf.RoundToInt (tile2.transform.position.y));

		//swap tiles on screen
		Vector2 tempPos = tile1.transform.position;
		//using iTween to move tiles, the oncomplete method is set to checkmatches once the tiles have been moved
		//iTween.MoveTo (tile1, iTween.Hash ("x", tile2.transform.position.x, "y", tile2.transform.position.y, "time", 1.0f, "oncomplete", "UpdateGridArray", "oncompletetarget", gameObject));
		iTween.MoveTo (tile1, iTween.Hash ("x", tile2.transform.position.x, "y", tile2.transform.position.y, "time", swapTime));
		iTween.MoveTo (tile2, iTween.Hash ("x", tempPos.x, "y", tempPos.y, "time", swapTime, "oncomplete", "FinishedSwappingBack", "oncompletetarget", gameObject, "oncompleteparams", "state"));

		UpdateGridArray ();									//update
		playerinput.currentState = GameState.None;
	}

	public void UpdateState (string state)
	{

		Debug.Log ("finished: " + state);
	}

	public void ReplaceTiles ()
	{

		Debug.Log ("Replacing Tiles..");

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
				int randomTileID = Random.Range (0, TilePrefabs.Length - 5);
				GameObject tile = Instantiate (TilePrefabs [randomTileID], new Vector2 (x, GridHeight + i), Quaternion.identity) as GameObject;
				tile.GetComponent<MoveScript> ().setName (tileColours [randomTileID]);
				tile.GetComponent<MoveScript> ().tileIndex = randomTileID;
				if (randomTileID >= 7) {
					tile.GetComponent<MoveScript> ().isIngredient = true;
				}
				newTiles.Add (tile);
			}
		}

		foreach (GameObject t in Grid) {

			//if (t != null && t.GetComponent<MoveScript>().getName() != "ciggy")
			if (t != null)
				t.GetComponent<MoveScript> ().GravityCheck (false);
		
		}

		//do a check on the last tile
		for (int i = 0; i < newTiles.Count; i++) {
			if (i == newTiles.Count - 1) {
				newTiles [i].GetComponent<MoveScript> ().GravityCheck (true);
			} else {
				newTiles [i].GetComponent<MoveScript> ().GravityCheck (false);
			}
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

	public string getTileName (Vector2 tPos)
	{
		//cigs are to destroyed in two places so first check if it is there
		if (Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] != null) {
			return Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)].GetComponent<MoveScript> ().getName ();
		} else {
			return "missing";
		}
	}

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
				DestroyTile (cigarette, true);
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

	public void LastTileFinished ()
	{
		Debug.Log ("--------------------finished--------------------------");
		StartCoroutine(Check ());
	}


	public IEnumerator Check ()
	{
		List<Vector2>[] matches;
		UpdateGridArray (); //update grid
		PrintGrid(Grid);
		matches = getMatches (Grid); //Retrieve any new matches

		List<GameObject> ingredients = getIngredients ();
		foreach (GameObject ingredient in ingredients) {
			moveIntoHolder (ingredient);
		}

		if (cigOn) {
			if (!CigsDestroyed (matches)) {
				if (cigSpawnAllowed) {	
					SpawnCig (3, 0);

				}
			}
		}

		//allow for only one cigspawn per tile move
		cigSpawnAllowed = false;
		DestroyTiles (matches);
		//wait for explosion effect
		yield return new WaitForSeconds(0.2f);
		ReplaceTiles ();


		//but what if the ingredient count is more than 0???????
		if (matches [0] == null && ingredients.Count == 0) {

			playerinput.currentState = GameState.None;


			Debug.Log ("checking");
			if (!checkForPossibleMoves ()) {
				ReplaceGrid ();
			}
		}


	}

	public IEnumerator continousCheck ()
	{
		bool firstTime = true;
		List<Vector2>[] matches;

		do {
			if (!firstTime || automate)//to prevent a double wait period
			yield return new WaitForSeconds (dropTime - 0.3f); //wait for new tiles to drop
			UpdateGridArray (); //update grid
			matches = getMatches (Grid); //Retrieve any new matches

			if (ingredientsOn) {
				List<GameObject> ingredients = getIngredients ();
				foreach (GameObject ingredient in ingredients) {
					moveIntoHolder (ingredient);
				}
			}
			//yield return new WaitForSeconds (0.4f);
			//UpdateGridArray (); //update grid

			if (matches [0] == null && !ingredientsOn) {
				break;
			}


			if (cigOn) {
				if (!CigsDestroyed (matches)) {
					//Debug.Log ("Spawn a cigarette");
					if (firstTime)
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
			
		PrintGrid (Grid);
		playerinput.currentState = GameState.None;

		//if no more moves are possible then we need to reshuffle/recreate the grid
		//also if there is a special booster we shouldnt reset the grid
		if (checkForPossibleMoves () == false && CheckGridForSpecBooster () == false) {
			ReplaceGrid ();
		}

		Debug.Log ("Number of cigs: " + cigCount);
	}

	bool checkForPossibleMoves ()
	{
		Debug.Log ("checking for moves");
		//this is where we should check if there are any more available moves
		//swap with neighbour to the right
		for (int x = 0; x <= 6; x++) {
			for (int y = 7; y >= 0; y--) {
				GameObject[,] TempGrid = new GameObject[GridWidth, GridHeight];
				CopyArray (Grid, TempGrid);
				//swap tiles
				GameObject tempTile = TempGrid [x, y];
				TempGrid [x, y] = TempGrid [x + 1, y];
				TempGrid [x + 1, y] = tempTile;



				List<Vector2>[] matches = getMatches (TempGrid);
				if (matches [0] != null) {
					//Debug.Log ("Swap " + x + ":" + y + "with " + (x + 1) + ":" + y);

					//now check that no fat surrounds the two tiles and that they arent cigarettes
					if (NoFatExists (new Vector2 (x, y), new Vector2 (x + 1, y))
					    && getTileName (new Vector2 (x, y)) != "ciggy"
					    && getTileName (new Vector2 (x + 1, y)) != "ciggy") {
						Debug.Log ("there are potential HORIZONTAL moves");
						DisplayMoves (Grid [x, y], Grid [x + 1, y]);
						return true;
					}
				} 
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

				List<Vector2>[] matches = getMatches (TempGrid);
				if (matches [0] != null) {
					//Debug.Log ("Swap " + x + ":" + y + "with " + x + ":" + (y - 1));

					//now check that no fat surrounds the two tiles
					if (NoFatExists (new Vector2 (x, y), new Vector2 (x, y - 1))
					    && getTileName (new Vector2 (x, y)) != "ciggy"
					    && getTileName (new Vector2 (x, y - 1)) != "ciggy") {
						Debug.Log ("there are potential Vertical moves");
						DisplayMoves (Grid [x, y], Grid [x, y - 1]);
						return true;
					}
				} else {
					//Debug.Log ("no more Vertical moves");
				}
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
				if (thisGrid [x, y] == null) {
					gridlayout += "-------, ";
				} else {
					gridlayout += (thisGrid [x, y].GetComponent<MoveScript> ().getName () + ", ");
				}
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
	selectionStarted,
	swappingTiles,
	idle
}
