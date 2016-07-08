// ICON ATTRIBUTIONS
//Water bottle icon made by Madebyoliver from flaticon.com
/* Place the attribution on the credits/description page of the application*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
	public int GridWidth;
	public int GridHeight;
	public GameObject[,] Grid;
	protected List<Vector2> gridLayout;
	//public LayerMask Tiles;
	protected GameObject[] TilePrefabs;
	GameObject[] boosterPrefabs;
	protected string[] tileColours;
	public float swapTime = 0.2f;
	public float dropTime = 0.35f;
	public int index;
	public GameObject[,] fatGrid;
	public GameObject[,] junkFoodGrid;
	GameObject fattyBlock;
	protected GameObject hotdog;
	protected GameObject candy;
	GameObject ingredientHolder;
	List<Vector3> holderPositions;
	public int fatLeft = 0;
	protected int beersLeft = 0;
	//to toggle fat blocks on and off
	protected bool fatOn;
	//to toggle hamburgers on and off
	protected bool junkFoodOn;
	//to toggle ciggarettes on and off
	protected bool cigOn;
	//toggle clearing ingredients level
	protected bool ingredientsOn;
	//for the game to play by itself
	public bool automate = false;
	GameObject[] feedback;
	public GameObject[] scorePrefabs;
	GameObject explosion;
	//number of cigarettes on the grid at a given time
	GameObject cigaretteTile;
	protected int cigCount = 0;
	public bool cigSpawnAllowed = false;
	protected int boostersDestroyed = 0;
	protected int specBoostersDestroyed = 0;
	protected int specBoostersCreated = 0;
	public bool timesUp = false;
	public bool outOfMoves = false;
	protected int[,] fatPositions;
	protected List<Vector2> cigPositions;
	public int moves;
	public int time;
	public bool gameEnded;
	protected string gameoverMessage;
	protected int ingredientHolders;
	int emptyHolders;
	protected int boosterFromBread = 0;

	protected int match5strawbs = 0;



	//two tiles that were last swapped
	Vector2 lastTileMoved1;
	Vector2 lastTileMoved2;

	protected PlayerInput playerinput;
	protected ScoreHandler scorehandler;
	Sounds sounds;



	//Game Over screens
	protected GameObject TIMESUP;
	protected GameObject OUTOFMOVES;
	protected GameObject LEVELPASSED;
	protected Button retry;
	protected Button menu;
	protected Text levelOverText;
	protected Text scoretext;

	void Start ()
	{

		gridLayout = GetGridLayout ();

	



		LoadAssets ();

		if (fatOn == true) {
			CreateFatBlocks ();
		}
		if (junkFoodOn) {
			CreateJunkFood ();
		}

		if (ingredientsOn == true) {
			CreateIngredientHolders (ingredientHolders);
		}

		//set last tiles moved to a position not included in the grid
		lastTileMoved1 = new Vector2 (-1, -1);
		lastTileMoved2 = new Vector2 (-1, -1);

		scorehandler = GameObject.Find ("scoretext").GetComponent<ScoreHandler> ();
		playerinput = GameObject.Find ("GameController").GetComponent<PlayerInput> ();
		sounds = Camera.main.GetComponent<Sounds> ();


		tileColours = new string[11];
		tileColours [0] = "strawberry";
		tileColours [1] = "fish";
		tileColours [2] = "cheese";
		tileColours [3] = "carrot";
		tileColours [4] = "bread";
		tileColours [5] = "raspberry";
		tileColours [6] = "aubergine";
		tileColours [7] = "broccoli";
		tileColours [8] = "watermelon";
		tileColours [9] = "beer";
		//tileColours [10] = "ciggy";



		StartCoroutine (CreateGrid (MakeCigarettes()));
	}

	protected virtual List<Vector2> MakeCigarettes(){
		return new List<Vector2>();
	}

	public void LoadAssets ()
	{

		cigaretteTile = Resources.Load ("cigaretteIcon", typeof(GameObject)) as GameObject;
		fattyBlock = Resources.Load ("fatIcon", typeof(GameObject)) as GameObject;
		boosterPrefabs = new GameObject[6];
		boosterPrefabs [0] = Resources.Load ("boosterPrefabs/strawberry-glowing", typeof(GameObject)) as GameObject;
		boosterPrefabs [1] = Resources.Load ("boosterPrefabs/fish-glow", typeof(GameObject)) as GameObject;
		boosterPrefabs [2] = Resources.Load ("boosterPrefabs/cheese-glow", typeof(GameObject)) as GameObject;
		boosterPrefabs [3] = Resources.Load ("boosterPrefabs/carrot-glow", typeof(GameObject)) as GameObject;
		boosterPrefabs [4] = Resources.Load ("boosterPrefabs/bread-glow", typeof(GameObject)) as GameObject;
		boosterPrefabs [5] = Resources.Load ("boosterPrefabs/water", typeof(GameObject)) as GameObject;
		explosion = Resources.Load ("explosion", typeof(GameObject)) as GameObject;
		hotdog = Resources.Load ("hotdog") as GameObject;
		candy = Resources.Load ("candy") as GameObject;
		ingredientHolder = Resources.Load ("holder") as GameObject;
		scorePrefabs = new GameObject[8];
		scorePrefabs [0] = Resources.Load ("points/20") as GameObject;
		scorePrefabs [1] = Resources.Load ("points/100") as GameObject;
		scorePrefabs [2] = Resources.Load ("points/200") as GameObject;
		scorePrefabs [3] = Resources.Load ("points/500") as GameObject;
		scorePrefabs [4] = Resources.Load ("points/1000") as GameObject;
		scorePrefabs [5] = Resources.Load ("points/1500") as GameObject;
		scorePrefabs [6] = Resources.Load ("points/2000") as GameObject;
		scorePrefabs [7] = Resources.Load ("points/5000") as GameObject;
		TilePrefabs = new GameObject[12];
		TilePrefabs [0] = Resources.Load ("tiles/strawberry") as GameObject;
		TilePrefabs [1] = Resources.Load ("tiles/fish") as GameObject;
		TilePrefabs [2] = Resources.Load ("tiles/cheese") as GameObject;
		TilePrefabs [3] = Resources.Load ("tiles/carrot") as GameObject;
		TilePrefabs [4] = Resources.Load ("tiles/bread") as GameObject;
		TilePrefabs [5] = Resources.Load ("tiles/raspberry") as GameObject;
		TilePrefabs [6] = Resources.Load ("tiles/aubergine") as GameObject;
		TilePrefabs [7] = Resources.Load ("tiles/broccoli") as GameObject;
		TilePrefabs [8] = Resources.Load ("tiles/watermelon") as GameObject;
		TilePrefabs [9] = Resources.Load ("tiles/beer") as GameObject;
		TilePrefabs [10] = Resources.Load ("tiles/redfish") as GameObject;
		TilePrefabs [11] = Resources.Load ("tiles/nut") as GameObject;
		feedback = new GameObject[3];
		feedback [0] = Resources.Load ("feedback/nicework") as GameObject;
		feedback [1] = Resources.Load ("feedback/oops") as GameObject;
		feedback [2] = Resources.Load ("feedback/stayawayfromcigs1") as GameObject;

		//Gameover Screens
		TIMESUP = Resources.Load ("TIMES UP", typeof(GameObject)) as GameObject;
		retry = Resources.Load ("Retry", typeof(Button)) as Button;
		menu = Resources.Load ("Menu", typeof(Button)) as Button;
		levelOverText = Resources.Load ("Level Over Text", typeof(Text)) as Text;
		scoretext = Resources.Load ("Score Text", typeof(Text)) as Text;
		LEVELPASSED = Resources.Load ("LEVEL PASSED", typeof(GameObject)) as GameObject;
		OUTOFMOVES = Resources.Load ("OUT OF MOVES", typeof(GameObject)) as GameObject;
	}

	public void CreateIngredientHolders (int amount)
	{
		Debug.Log ("holders: " + amount);
		holderPositions = new List<Vector3> ();
		float y = -1.3f;
		int startpos = 0;

		for (int x = startpos; x < amount + startpos; x++) {
			Instantiate (ingredientHolder, new Vector2 (x, y), Quaternion.identity);
			holderPositions.Add (new Vector3 (x, y, 0)); //z =0 indicates that holder is unoccupied
		}
	}

	public List<GameObject> getIngredients ()
	{
		

		List<GameObject> ingredients = new List<GameObject> ();

		if (ingredientsOn) {

			//check the bottom row
			for (int x = 0; x < GridWidth; x++) {
				if (Grid [x, 0] != null && Grid [x, 0].GetComponent<TileScript> ().isIngredient) {
					//Debug.Log ("found pepper");
					ingredients.Add (Grid [x, 0]);
				}
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
				//Debug.Log ("Moving ingredient");

				sounds.PlaySound ("swoosh");
				ingredient.GetComponent<TileScript> ().StopFlashing ();

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




		for(int i = 0; i < holderPositions.Count; i++){
			if (holderPositions [i].z == 0) {
				emptyHolders++;
			}
		}
	}

	protected bool HoldersAreFull(){
		int emptyHolders = 0;

		for(int i = 0; i < holderPositions.Count; i++){
			if (holderPositions [i].z == 0) {
				emptyHolders++;
			}
		}

		if (emptyHolders <= 0) {
			return true;
		}

		return false;
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
		if (y < GridHeight - 1) {
			if (Grid [x, y + 1] != null && Grid [x, y + 1].GetComponent<TileScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x, y + 1));
			}
		}
		//check below
		if (y > 0) {
			if (Grid [x, y - 1] != null && Grid [x, y - 1].GetComponent<TileScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x, y - 1));
			}
		}
		//check right
		if (x < GridWidth - 1) {
			if (Grid [x + 1, y] != null && Grid [x + 1, y].GetComponent<TileScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x + 1, y));
			}
		}
		//check left
		if (x > 0) {
			if (Grid [x - 1, y] != null && Grid [x - 1, y].GetComponent<TileScript> ().getName () == "ciggy") {
				adjacentCigs.Add (new Vector2 (x - 1, y));
			}
		}

		return adjacentCigs;
	}

	public void DestroyAdjacentHotdogs (List<Vector2>[] matches)
	{

		for (int i = 0; i < index; i++) {
			foreach (Vector2 tPos in matches[i]) {

				if (Mathf.RoundToInt (tPos.y) < GridHeight - 1) {
					if (junkFoodGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y) + 1] != null) {
						Destroy (junkFoodGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y) + 1]);
					}
				}
				if (Mathf.RoundToInt (tPos.y) > 0) {
					if (junkFoodGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y) - 1] != null) {
						Destroy (junkFoodGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y) - 1]);
					}
				}
				if (Mathf.RoundToInt (tPos.x) < GridWidth - 1) {
					if (junkFoodGrid [Mathf.RoundToInt (tPos.x) + 1, Mathf.RoundToInt (tPos.y)] != null) {
						Destroy (junkFoodGrid [Mathf.RoundToInt (tPos.x) + 1, Mathf.RoundToInt (tPos.y)]);
					}
				}
				if (Mathf.RoundToInt (tPos.x) > 0) {
					if (junkFoodGrid [Mathf.RoundToInt (tPos.x) - 1, Mathf.RoundToInt (tPos.y)] != null) {
						Destroy (junkFoodGrid [Mathf.RoundToInt (tPos.x) - 1, Mathf.RoundToInt (tPos.y)]);
					}
				}
			}
		}
	}



	void CreateFatBlocks ()
	{
		fatGrid = new GameObject[GridWidth, GridHeight];


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

	protected virtual void CreateJunkFood ()
	{
		junkFoodGrid = new GameObject[GridWidth, GridHeight];


		for (int x = 0; x < GridHeight; x++) {
			for (int y = 0; y < GridWidth; y++) {
				if (fatPositions [x, y] == 1) {
					GameObject hd = Instantiate (hotdog, new Vector2 (x, y), Quaternion.identity) as GameObject;
					junkFoodGrid [x, y] = hd;
				}

			}
		}

	}

	IEnumerator CreateRandomGrid (List<Vector2> cigPositions, List<Vector2> beerPositions)
	{

		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];

		string gridInput = "";

		gridInput += "{";

		for (int y = GridHeight - 1; y >= 0; y--) {

			gridInput += "{";

			for (int x = 0; x < GridWidth; x++) {



				if (cigPositions.Contains (new Vector2 (x, y))) {
					CreateCigarette (new Vector2 (x, y));
				}
				else if (beerPositions.Contains (new Vector2 (x, y))) {
					GameObject tile = Instantiate (TilePrefabs [9], new Vector2 (x, y), Quaternion.identity) as GameObject;
					tile.GetComponent<TileScript> ().tileIndex = 9;
					Grid [x, y] = tile;
					Grid [x, y].GetComponent<TileScript> ().setName (tileColours [9]);

				}else {

					//change this back later----------------------------------------------
					int randomTile = UnityEngine.Random.Range (0, 5);

					if (x == GridWidth - 1) {
						gridInput += randomTile + "";
					} else {
						gridInput += randomTile + ", ";
					}


					GameObject tile = Instantiate (TilePrefabs [randomTile], new Vector2 (x, y), Quaternion.identity) as GameObject;
					tile.GetComponent<TileScript> ().tileIndex = randomTile;

					//if pepper will make an ingredient
					if (randomTile >= 6) {
						tile.GetComponent<TileScript> ().isIngredient = true;
					}
					Grid [x, y] = tile;

					//Assign the tile a name
					Grid [x, y].GetComponent<TileScript> ().setName (tileColours [randomTile]);

				}


				yield return new WaitForSeconds (.02f);
			}

			if (y == 0) {
				gridInput += "}\n";
			} else {
				gridInput += "},\n";
			}

		}

		gridInput += "};";

		Debug.Log ("grid input\n" + gridInput);

		StartCoroutine (Check ());
	}

	public List<Vector2> GetGridLayout(){

		gridLayout = new List<Vector2> ();

		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {
					gridLayout.Add (new Vector2 (x, y));
				
			}
		}

		return gridLayout;
	}

	protected virtual IEnumerator CreateGrid (List<Vector2> cigPositions)
	{



		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];

		foreach (Vector2 cell in gridLayout) {
			int x = Mathf.RoundToInt (cell.x);
			int y = Mathf.RoundToInt (cell.y);

			if (cigPositions.Contains (new Vector2 (x, y))) {
				CreateCigarette (new Vector2 (x, y));
			} else {
					
				//int tileType = gridContent [x, y];
				int tileType = UnityEngine.Random.Range(0,5);

				GameObject tile = Instantiate (TilePrefabs [tileType], new Vector2 (x, y), Quaternion.identity) as GameObject;
				tile.GetComponent<TileScript> ().tileIndex = tileType;

				//set ingredients
				if (tileType >= 5 && tileType <= 8) {
					tile.GetComponent<TileScript> ().isIngredient = true;
				} else if (tileType == 9) {
					beersLeft++;
				}
				Grid [x, y] = tile;

				//Assign the tile a name
				Grid [x, y].GetComponent<TileScript> ().setName (tileColours [tileType]);
			}
			yield return new WaitForSeconds (.02f);
				
		}
		
		StartCoroutine (Check ());

	}

	public void SpaceBarFunction ()
	{
		ReplaceGrid ();


	}

	List<Vector2> GetCigarettePositions ()
	{

		List<Vector2> cigPositions = new List<Vector2> ();

		foreach (Vector2 cell in gridLayout) {	
			int x = Mathf.RoundToInt (cell.x);
			int y = Mathf.RoundToInt (cell.y);


			if (Grid [x, y].GetComponent<TileScript> ().getName () == "ciggy") {
				cigPositions.Add (Grid [x, y].transform.position);
			}
		}

		
		return cigPositions;
	}

	List<Vector2> GetBeerPositions ()
	{

		List<Vector2> beerPositions = new List<Vector2> ();

		foreach (Vector2 cell in gridLayout) {	
			int x = Mathf.RoundToInt (cell.x);
			int y = Mathf.RoundToInt (cell.y);


			if (Grid [x, y].GetComponent<TileScript> ().getName () == "beer") {
				beerPositions.Add (Grid [x, y].transform.position);
			}
		}


		return beerPositions;
	}

	List<Vector2> GetIngredientPositions ()
	{

		List<Vector2> ingPositions = new List<Vector2> ();

		foreach (Vector2 cell in gridLayout) {	
			int x = Mathf.RoundToInt (cell.x);
			int y = Mathf.RoundToInt (cell.y);	

			if (Grid [x, y].GetComponent<TileScript> ().isIngredient) {
				ingPositions.Add (Grid [x, y].transform.position);
			}
		}
		
		return ingPositions;
	}

	public void ReplaceGrid ()
	{
		List<Vector2> cigPositions = GetCigarettePositions ();
		List<Vector2> ingPositions = GetIngredientPositions ();
		List<Vector2> beerPositions = GetBeerPositions ();


		for (int y = GridHeight - 1; y >= 0; y--) {
			for (int x = 0; x < GridWidth; x++) {
				Destroy (Grid [x, y]);
			}
		}
		//changed already-----------------------------------------------------------------------------------------
		//StartCoroutine (CreateGrid ());
		cigCount = 0; //reset this
		//StartCoroutine (CreateGridTest (cigPositions));
		StartCoroutine (CreateRandomGrid (cigPositions, beerPositions));
	}

	bool CheckGridForSpecBooster ()
	{

		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {
				if (Grid [x, y].GetComponent<TileScript> ().isSpecialBooster) {
					return true;
				}
			}
		}
		return false;
	}

	bool checkForBooster (Vector2 pos)
	{

		if (Grid [(int)pos.x, (int)pos.y].GetComponent<TileScript> ().isBooster) {

			//only play the sound in animation and not when we are checking for possible moves
			if (playerinput.currentState == GameState.Animating) {
				sounds.PlaySound ("explosion");
				scorehandler.AddPoints (1000); //this needs to be changed later------gets called twice-----------------
				Instantiate (scorePrefabs [6], pos, Quaternion.identity);
			}
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

				//dont deal with empty cells
				if (gridToCheck [x, y] != null) {


					if (currentName != gridToCheck [x, y].GetComponent<TileScript> ().getName ()) {

						if (matchPositions.Count >= 3) {

							matchSets [index] = new List<Vector2> ();
							foreach (Vector2 match in matchPositions) {

								//if (!matchSets [index].Contains (match)) {
								matchSets [index].Add (match);
								//}
								if (checkForBooster (match)) {

									//get col and row of booster
									List<Vector2> rowCol = getRowCol (match);
									foreach (Vector2 item in rowCol) {
										//if (!matchSets [index].Contains (item)) {
										matchSets [index].Add (item);
										//}
									}
								}
							}
							index++; //change the index
						}

						currentName = gridToCheck [x, y].GetComponent<TileScript> ().getName ();
						matchPositions.Clear ();
						if (currentName != "ciggy" && currentName != "beer")
							matchPositions.Add (new Vector2 (x, y));
					} else {
						if (currentName != "ciggy" && currentName != "beer")
							matchPositions.Add (new Vector2 (x, y));
					}
				}
			}
			//at the end set colour back to none
			currentName = "none";
		}

		//after scanning the whole grid we need to check if 3 or more tiles matched again...
		if (matchPositions.Count >= 3) {

			matchSets [index] = new List<Vector2> ();
			foreach (Vector2 match in matchPositions) {

				//if (!matchSets [index].Contains (match)) {
				matchSets [index].Add (match);
				//}

				if (checkForBooster (match)) {
					//get col and row of booster
					List<Vector2> rowCol = getRowCol (match);
					foreach (Vector2 item in rowCol) {
						//if (!matchSets [index].Contains (item)) {
						matchSets [index].Add (item);
						//}
					}
				}
			}
			index++; //change the index
		}



		//check vertical matches
		currentName = "none";
		
		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {

				if (gridToCheck [x, y] != null) {

					if (currentName != gridToCheck [x, y].GetComponent<TileScript> ().getName ()) {
					
						if (matchPositions.Count >= 3) {

							matchSets [index] = new List<Vector2> ();
							foreach (Vector2 match in matchPositions) {

								//ensures there are no duplicates
								//if (!matchSets [index].Contains (match)) {
								matchSets [index].Add (match);
								//}

								if (checkForBooster (match)) {
									//get col and row of booster
									List<Vector2> rowCol = getRowCol (match);
									foreach (Vector2 item in rowCol) {
										//if (!matchSets [index].Contains (item)) {
										matchSets [index].Add (item);
										//}
									}
								}
							}
							index++; //change the index
						}
					
						currentName = gridToCheck [x, y].GetComponent<TileScript> ().getName ();
						matchPositions.Clear ();
						if (currentName != "ciggy" && currentName != "beer")
							matchPositions.Add (new Vector2 (x, y));
					} else {
						if (currentName != "ciggy" && currentName != "beer")
							matchPositions.Add (new Vector2 (x, y));
					}
				}
			}
			//at the end set colour back to none
			currentName = "none";
		}
			
		//after scanning the whole grid we need to check if 3 or more tiles matched again...
		if (matchPositions.Count >= 3) {

			matchSets [index] = new List<Vector2> ();
			foreach (Vector2 match in matchPositions) {

				//if (!matchSets [index].Contains (match)) {
				matchSets [index].Add (match);
				//}

				if (checkForBooster (match)) {
					//Debug.Log ("One of the tiles in the match was a booster: " + match.x + ":" + match.y);
					//get col and row of booster
					List<Vector2> rowCol = getRowCol (match);
					foreach (Vector2 item in rowCol) {
						//	if (!matchSets [index].Contains (item)) {
						matchSets [index].Add (item);
						//	}
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


	public List<Vector2>[] SortByMatchSize (List<Vector2>[] matchSets)
	{

		int n = index;
		int k;
		for (int m = n; m >= 0; m--) {
			for (int i = 0; i < n - 1; i++) {
				k = i + 1;
				if (matchSets [i].Count < matchSets [k].Count) {
					//swap lists in array
					List<Vector2> temp;
					temp = matchSets [i];
					matchSets [i] = matchSets [k];
					matchSets [k] = temp;
				}
			}
		}
	
		return matchSets;
	}


	public void DestroyTiles (List<Vector2>[] matchSets)
	{
		List<Vector2> specialBoosters = new List<Vector2> ();

		//we want the largest lists firsts so that we can allocate the right boosters
		matchSets = SortByMatchSize (matchSets);

		printMatchSets (matchSets);

		if (matchSets [0] != null) {
			sounds.PlaySound ("tileDestroy");
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
						specialBoosters.Add (tPos);
						boosterAdded = true;
					} else if (tPos == matchSets [i].ElementAt (3) && boosterAdded == false) {
						CreateNormalBooster (tPos);
						specialBoosters.Add (tPos);
						boosterAdded = true; //should be redundant
					
					} else {
						if (!specialBoosters.Contains (tPos)) {
							DestroyTile (tPos, true);
						}
					}
					break;
				case 5:
					if (tPos == lastTileMoved1 || tPos == lastTileMoved2) {
						CreateSpecialBooster (tPos);
						specialBoosters.Add (tPos);
						boosterAdded = true;
					} else if (tPos == matchSets [i].ElementAt (4) && boosterAdded == false) {
						CreateSpecialBooster (tPos);
						specialBoosters.Add (tPos);
						boosterAdded = true; //should be redundant
					} else {
						if (!specialBoosters.Contains (tPos)) {
							DestroyTile (tPos, true);	
						}
					}
					break;
				default: 
					if (!specialBoosters.Contains (tPos)) {
						DestroyTile (tPos, true);
					}
					break;
				}
			}

			//Award points for match
			int count = matchSets [i].Count;

			Debug.Log ("matched: " + count);

			switch (count) {
			case 1:
				scorehandler.AddPoints (20);
				Instantiate (scorePrefabs [0], matchSets [i].ElementAt (count / 2), Quaternion.identity);
				break;
			case 3:
				scorehandler.AddPoints (100);
				Instantiate (scorePrefabs [1], matchSets [i].ElementAt (count / 2), Quaternion.identity);
				break;
			case 4:
				scorehandler.AddPoints (200);
				Instantiate (scorePrefabs [2], matchSets [i].ElementAt (count / 2), Quaternion.identity);
				break;
			case 5:
				scorehandler.AddPoints (500);
				Instantiate (scorePrefabs [3], matchSets [i].ElementAt (count / 2), Quaternion.identity);
				break;
			case 15:
				//for a deleted row and col
				//scorehandler.AddPoints (2000);
				//Instantiate (scorePrefabs[6], matchSets [i].ElementAt (count/4), Quaternion.identity);
				break;
			default: 
				Debug.Log ("Need a case for this");
				break;
			}
		}

		//reset last tiles moved
		lastTileMoved1 = new Vector2 (-1, -1);
		lastTileMoved2 = new Vector2 (-1, -1);
	}

	public void ReplaceWithBoosters (string name)
	{

		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {

				if (Grid [x, y] != null && Grid [x, y].GetComponent<TileScript> ().getName () == name) {
					CreateNormalBooster (new Vector2 (x, y));
				}
			}
		}
	}


	public IEnumerator DestroyTilesWithName (string name)
	{
		//just to wait fot the raygun sound, not necessarily needed
		yield return new WaitForSeconds (0.5f);
		List<Vector2>[] matchSets = new List<Vector2>[100];
		index = 0;

		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {

				if (Grid [x, y] != null && Grid [x, y].GetComponent<TileScript> ().getName () == name) {
					matchSets [index] = new List<Vector2> ();
					matchSets [index].Add (new Vector2 (x, y));
					index++;
				}
			}
		}

		if (junkFoodOn) {
			DestroyAdjacentHotdogs (matchSets);
		}

		if (cigOn) {
			if (!CigsDestroyed (matchSets)) {
				//Debug.Log ("Spawn a cigarette");
					
				SpawnCig (3, 0);
			}
		}

		DestroyTiles (matchSets);
		yield return new WaitForSeconds (0.2f); //could use invoke(replacetiles, 0.2f)
		ReplaceTiles ();

	}

	public void DestroyTile (Vector2 tPos, bool explode)
	{

		int x = Mathf.RoundToInt (tPos.x);
		int y = Mathf.RoundToInt (tPos.y);
		
		//dont destroy if it's an ingredient
		if (Grid [x,y] != null && Grid [x, y].GetComponent<TileScript> ().isIngredient) {

			//do nothing
		} else {
			if (explode) {
				GameObject e = Instantiate (explosion, new Vector2 (tPos.x, tPos.y), Quaternion.identity) as GameObject;
			} 

			if (getTileName (tPos) == "ciggy") {
				cigCount--;
			} else if (getTileName (tPos) == "beer") {
				beersLeft--;
			}

			if(Grid [x,y] != null && Grid [x, y].GetComponent<TileScript> ().isBooster){
				boostersDestroyed++;
			}
			if(Grid [x,y] != null && Grid [x, y].GetComponent<TileScript> ().isSpecialBooster){
				specBoostersDestroyed++;
			}


			Destroy (Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)]);
			Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = null;

			if (fatOn) {
				if (junkFoodOn && junkFoodGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] != null) {
					Destroy (junkFoodGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)]);
					junkFoodGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = null;
				} else if (fatGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] != null) {
					Destroy (fatGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)]);
					fatGrid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = null;
					fatLeft--;
				}
			}
		}
	}

	public void DestroyGridQuarter (Vector2 tPos)
	{
		scorehandler.AddPoints (1000);
		Instantiate (scorePrefabs [4], tPos, Quaternion.identity);

		int posX = Mathf.RoundToInt (tPos.x);
		int posY = Mathf.RoundToInt (tPos.y);
		int quarter = 0;

		if (posX < GridWidth / 2 && posY >= GridHeight / 2) {
			quarter = 1;
		} else if (posX >= GridWidth / 2 && posY >= GridHeight / 2) {
			quarter = 2;
		} else if (posX < GridWidth / 2 && posY < GridHeight / 2) {
			quarter = 3;
		} else if (posX >= GridWidth / 2 && posY < GridHeight / 2) {
			quarter = 4;
		}
			
		sounds.PlaySound ("explosion");

		if (quarter == 1) {

			for (int y = GridHeight / 2; y < GridHeight; y++) {
				for (int x = 0; x < GridWidth / 2; x++) {
					DestroyTile (new Vector2 (x, y), true);
				}
			}
		} else if (quarter == 2) {

			for (int y = GridHeight / 2; y < GridHeight; y++) {
				for (int x = GridWidth / 2; x < GridWidth; x++) {
					DestroyTile (new Vector2 (x, y), true);
				}
			}
		} else if (quarter == 3) {

			for (int y = 0; y < GridHeight / 2; y++) {
				for (int x = 0; x < GridWidth / 2; x++) {
					DestroyTile (new Vector2 (x, y), true);
				}
			}
		} else if (quarter == 4) {

			for (int y = 0; y < GridHeight / 2; y++) {
				for (int x = GridWidth / 2; x < GridWidth; x++) {
					DestroyTile (new Vector2 (x, y), true);
				}
			}
		}
		
	}

	public void DisplayFeedback (int type)
	{
		Instantiate (feedback [type], new Vector2 (3.51f, 3.5f), Quaternion.identity);
	}

	public void CreateNormalBooster (Vector2 tPos)
	{
		int x = Mathf.RoundToInt (tPos.x);
		int y = Mathf.RoundToInt (tPos.y);

		if (getTileName (new Vector2(x,y)) == "bread") {
			boosterFromBread++;
		}

		Debug.Log ("booster from bread: " + boosterFromBread);

		//we have to fix this issue at some stage---------------------------------------------------------------------------------
		if (Grid [x,y] != null) {

			sounds.PlaySound ("booster");
			DisplayFeedback (0);

			//sometimes tile can be part of a 3 match and a 4 match and couldve already been deleted in the 3 match
			int tileType = Grid [x, y].GetComponent<TileScript> ().tileIndex;

			DestroyTile (new Vector2 (tPos.x, tPos.y), true);
			//Destroy (Grid [Mathf.RoundToInt(tPos.x),Mathf.RoundToInt(tPos.y)]);
			GameObject tile = Instantiate (boosterPrefabs [tileType], new Vector2 (x, y), Quaternion.identity) as GameObject;
			tile.GetComponent<TileScript> ().setName (tileColours [tileType]);
			tile.GetComponent<TileScript> ().isBooster = true;
			tile.GetComponent<TileScript> ().tileIndex = tileType;
			Grid [x,y] = tile;

		}
	}

	public void CreateSpecialBooster (Vector2 tPos)
	{
		int x = Mathf.RoundToInt (tPos.x);
		int y = Mathf.RoundToInt (tPos.y);

		if (Grid[x,y] != null && getTileName(new Vector2(x,y)) == "strawberry") {
			match5strawbs++;
		}

		sounds.PlaySound ("special");
		DisplayFeedback (0);
		DestroyTile (new Vector2 (x,y), true);
		//Destroy (Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)]);
		GameObject tile = Instantiate (boosterPrefabs [5], new Vector2 (x,y), Quaternion.identity) as GameObject;
		tile.GetComponent<TileScript> ().setName ("water");
		tile.GetComponent<TileScript> ().isSpecialBooster = true;
		Grid [x,y] = tile;

		specBoostersCreated++;

	}

	public void CreateCigarette (Vector2 tPos)
	{
		DestroyTile (new Vector2 (tPos.x, tPos.y), false);
		//Destroy (Grid [Mathf.RoundToInt(tPos.x), Mathf.RoundToInt(tPos.y)]);
		//GameObject tile = Instantiate (TilePrefabs [10], new Vector2 (Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)), Quaternion.identity) as GameObject;
		GameObject tile = Instantiate (cigaretteTile, new Vector2 (Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)), Quaternion.identity) as GameObject;
		cigCount++;
		tile.GetComponent<TileScript> ().setName ("ciggy");
		Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)] = tile;
		sounds.PlaySound ("smoke");
	}

	public void SwapTiles (GameObject tile1, GameObject tile2)
	{
		sounds.PlaySound ("swap");
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
	}

	public void SwapBack (GameObject tile1, GameObject tile2)
	{
		sounds.PlaySound ("swapback");
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

		UpdateGridArray ();
	}

	public void ReplaceTiles ()
	{

		//Debug.Log ("Replacing Tiles..");

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
				int randomTileID = UnityEngine.Random.Range (0, 5);
				GameObject tile = Instantiate (TilePrefabs [randomTileID], new Vector2 (x, GridHeight + i), Quaternion.identity) as GameObject;
				tile.GetComponent<TileScript> ().setName (tileColours [randomTileID]);
				tile.GetComponent<TileScript> ().tileIndex = randomTileID;
				if (randomTileID >= 5 && randomTileID <= 8) {
					tile.GetComponent<TileScript> ().isIngredient = true;
				}
				newTiles.Add (tile);
			}
		}

		foreach (GameObject t in Grid) {

			//if (t != null && t.GetComponent<MoveScript>().getName() != "ciggy")
			if (t != null)
				t.GetComponent<TileScript> ().GravityCheck (false);
		
		}

		//do a check on the last tile
		for (int i = 0; i < newTiles.Count; i++) {
			if (i == newTiles.Count - 1) {
				newTiles [i].GetComponent<TileScript> ().GravityCheck (true);
			} else {
				newTiles [i].GetComponent<TileScript> ().GravityCheck (false);
			}
		}


	}



	public void UpdateGridArray ()
	{
		//Debug.Log ("Updating Grid...");

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
			return Grid [Mathf.RoundToInt (tPos.x), Mathf.RoundToInt (tPos.y)].GetComponent<TileScript> ().getName ();
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
			//Debug.Log ("Delete a cigarette");
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
				if (Grid [x, y] != null && Grid [x, y].GetComponent<TileScript> ().getName () != "ciggy" && Grid [x, y].GetComponent<TileScript> ().isIngredient == false) {

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
		//Debug.Log ("--------------------finished--------------------------");
		//sounds.PlaySound("land");
		StartCoroutine (Check ());
	}


	public IEnumerator Check ()
	{
		List<Vector2>[] matches;
		UpdateGridArray (); //update grid
		PrintGrid (Grid);
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

		if (junkFoodOn) {
			DestroyAdjacentHotdogs (matches);
		}

		//allow for only one cigspawn per tile move
		cigSpawnAllowed = false;
		DestroyTiles (matches);
		//wait for explosion effect
		yield return new WaitForSeconds (0.2f);
		ReplaceTiles ();


		//but what if the ingredient count is more than 0???????
		if (matches [0] == null && ingredients.Count == 0) {

			if (!gameEnded) {
				playerinput.currentState = GameState.None;
			}
			CheckCriteria ();


			Debug.Log ("beeers left: " + beersLeft);
			//Debug.Log ("checking");
			if (!checkForPossibleMoves ()) {
				ReplaceGrid ();
			}
		}

		Debug.Log ("Number of cigs: " + cigCount);
		if (cigCount > 32) {
			Debug.Log ("GAME OVER");
		}
	}

	public virtual void CheckCriteria(){
		//check if target has been reached so they can pass the level
//		if (((scorehandler.GetScore () >= leveldata.target && outOfMoves)
//			|| (scorehandler.GetScore () >= leveldata.target && timesUp)) && !leveldata.gameEnded) {
//			leveldata.LevelPassed ();
//		}
//
//		else if ((boostersDestroyed >= leveldata.boostersNeeded && outOfMoves) && !leveldata.gameEnded) {
//			leveldata.LevelPassed ();
//		}
//		else if (specBoostersDestroyed >= leveldata.specBoostersNeeded && outOfMoves && !leveldata.gameEnded) {
//			leveldata.LevelPassed ();
//		}
//		else if (fatLeft <= 0 && leveldata.objectiveFat && !leveldata.gameEnded) {
//			leveldata.LevelPassed ();
//		}
//		else if (beersLeft <= 0 && leveldata.objectiveBeer && !leveldata.gameEnded) {
//			leveldata.LevelPassed ();
//		}
//		else if (cigCount <= 0 && leveldata.objectiveCiggy && !leveldata.gameEnded) {
//			leveldata.LevelPassed ();
//		}
//		else if (outOfMoves && !leveldata.gameEnded) {
//			leveldata.OutOfMoves ();
//		}
//		else if (timesUp && !leveldata.gameEnded) {
//			leveldata.TimesUp ();
//		}
	}

	public bool QuickHorizCheck (GameObject[,] TempGrid, GameObject tile)
	{

		//if (tile != null) {

			//int x = Mathf.RoundToInt (tile.transform.position.x);
			int y = Mathf.RoundToInt (tile.transform.position.y);

			string name = tile.GetComponent<TileScript> ().getName ();

			int count = 0;

			for (int x = 0; x < GridWidth; x++) {
				if (TempGrid [x, y] != null && TempGrid [x, y].GetComponent<TileScript> ().getName () == name) {
					count++;
				} else {
					count = 0;
				}
				if (count >= 3) {
					return true;
				}
			}
		//}
		return false;
	}

	public bool QuickVertCheck (GameObject[,] TempGrid, GameObject tile)
	{

		//if (tile != null) {

			int x = Mathf.RoundToInt (tile.transform.position.x);
			//int y = Mathf.RoundToInt (tile.transform.position.y);

			string name = tile.GetComponent<TileScript> ().getName ();

			int count = 0;

			for (int y = 0; y < GridHeight; y++) {
				if (TempGrid [x, y] != null && TempGrid [x, y].GetComponent<TileScript> ().getName () == name) {
					count++;
				} else {
					count = 0;
				}
				if (count >= 3) {
					return true;
				}
			}
		//}
		return false;
	}

	public bool checkForPossibleMoves ()
	{
		//Debug.Log ("checking for moves");
		//this is where we should check if there are any more available moves
		//swap with neighbour to the right
		for (int x = 0; x < GridWidth - 1; x++) {
			for (int y = GridHeight - 1; y >= 0; y--) {
				GameObject[,] TempGrid = new GameObject[GridWidth, GridHeight];
				CopyArray (Grid, TempGrid);
				//swap tiles
				GameObject tempTile = TempGrid [x, y];
				TempGrid [x, y] = TempGrid [x + 1, y];
				TempGrid [x + 1, y] = tempTile;

				List<Vector2>[] matches = getMatches (TempGrid);
				if (matches [0] != null) {

				//if (QuickHorizCheck (TempGrid, TempGrid [x, y]) || QuickHorizCheck (TempGrid, TempGrid [x + 1, y]) || QuickVertCheck (TempGrid, TempGrid [x + 1, y]) || QuickVertCheck (TempGrid, TempGrid [x, y])) {
					//now check that no fat surrounds the two tiles and that they arent cigarettes
					if (NoFatExists (new Vector2 (x, y), new Vector2 (x + 1, y))
					    && getTileName (new Vector2 (x, y)) != "ciggy"
					    && getTileName (new Vector2 (x + 1, y)) != "ciggy"
					    && getTileName (new Vector2 (x, y)) != "beer") {
						DisplayMoves (Grid [x, y], Grid [x + 1, y]);
						return true;
					}
				}
			}
		}
			
		//now swap with neighbour underneath
		for (int x = 0; x < GridWidth; x++) {
			for (int y = GridHeight - 1; y >= 1; y--) {
				GameObject[,] TempGrid = new GameObject[GridWidth, GridHeight];
				CopyArray (Grid, TempGrid);
				//swap tiles
				GameObject tempTile = TempGrid [x, y];
				TempGrid [x, y] = TempGrid [x, y - 1];
				TempGrid [x, y - 1] = tempTile;

				List<Vector2>[] matches = getMatches (TempGrid);
				if (matches [0] != null) {

				//if (QuickVertCheck (TempGrid, TempGrid [x, y]) || QuickVertCheck (TempGrid, TempGrid [x, y - 1]) || QuickHorizCheck (TempGrid, TempGrid [x, y - 1]) || QuickHorizCheck (TempGrid, TempGrid [x, y])) {
					//now check that no fat surrounds the two tiles
					if (NoFatExists (new Vector2 (x, y), new Vector2 (x, y - 1))
					    && getTileName (new Vector2 (x, y)) != "ciggy"
					    && getTileName (new Vector2 (x, y - 1)) != "ciggy"
					    && getTileName (new Vector2 (x, y)) != "beer") {
						DisplayMoves (Grid [x, y], Grid [x, y - 1]);
						return true;
					}
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
			firstTile.GetComponent<TileScript> ().Flash ();
			secondTile.GetComponent<TileScript> ().Flash ();
		}

	}


	void CopyArray (GameObject[,] from, GameObject[,] to)
	{

		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {
				to [x, y] = from [x, y];
			}
		}

	}

	void PrintGrid (GameObject[,]  thisGrid)
	{
		
		string grid = "";
		
		for (int y = GridHeight - 1; y >= 0; y--) {
			
			grid += ("Row " + y + ": ");
			for (int x = 0; x < GridWidth; x++) {
				if (thisGrid [x, y] == null) {
					grid += "-------, ";
				} else {
					grid += (thisGrid [x, y].GetComponent<TileScript> ().getName () + ", ");
				}
			}
			
			grid += ("\n");
		}
		
		Debug.Log (grid);

	}

	public void TimesUp(){
		Debug.Log ("TIMES UP!");

		GameObject tu = Instantiate (TIMESUP, new Vector2 (3.3f, -5), Quaternion.identity) as GameObject;
		iTween.MoveTo (tu, iTween.Hash("y", 3.39, "time", 1));
		Invoke ("CreateMenuButtons", 0.6f);
		gameEnded = true;
		playerinput.currentState = GameState.Animating;
	}

	public void OutOfMoves(){

		GameObject oom = Instantiate (OUTOFMOVES, new Vector2 (3.3f, -5), Quaternion.identity) as GameObject;
		iTween.MoveTo (oom, iTween.Hash("y", 3.39, "time", 1));
		Invoke ("CreateMenuButtons", 0.6f);
		Debug.Log ("OUT OF MOVESSS");
		gameEnded = true;
		playerinput.currentState = GameState.Animating;
	}

	public void LevelPassed(){
		Debug.Log ("LEVEL PASSED");

		GameObject lp = Instantiate (LEVELPASSED, new Vector2 (3.3f, -5), Quaternion.identity) as GameObject;
		iTween.MoveTo (lp, iTween.Hash("y", 3.39, "time", 1));
		Invoke ("CreateMenuButtons", 0.6f);
		gameEnded = true;
		playerinput.currentState = GameState.Animating;
	}



	public void CreateMenuButtons(){

		Button r = Instantiate (retry, new Vector2 (-26, 0), Quaternion.identity) as Button;
		r.transform.SetParent (GameObject.Find("Canvas").transform, false);
		r.transform.localPosition = new Vector2 (54, -89);
		r.onClick.AddListener (delegate {
			ResetLevel ();

		});

		Button m =  Instantiate (menu, new Vector2 (-26, 0), Quaternion.identity) as Button;
		m.transform.SetParent (GameObject.Find("Canvas").transform, false);
		m.transform.localPosition = new Vector2 (-75, -89);
		m.onClick.AddListener (delegate {
			LoadLevel (1);

		});

		Text t = Instantiate (levelOverText, new Vector2 (-18, -8.8f), Quaternion.identity) as Text;
		t.transform.SetParent (GameObject.Find("Canvas").transform, false);
		t.transform.localPosition = new Vector2 (-18, -8.8f);
		t.text = gameoverMessage;

		Text s = Instantiate (scoretext, new Vector2 (-18, -8.8f), Quaternion.identity) as Text;
		s.transform.SetParent (GameObject.Find("Canvas").transform, false);
		s.transform.localPosition = new Vector2 (441, -70);
		s.text = "" + scorehandler.GetScore ();

	}

	public void ResetLevel(){
		Application.LoadLevel (Application.loadedLevel);	
	}

	public void LoadLevel(int level){
		Application.LoadLevel (level);
	}

	void grids ()
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


		//2 5's at once
		int[,] testGrid8 = {{ 4, 5, 6, 4, 2, 3, 5, 3 },
			{ 3, 4, 6, 2, 2, 5, 5, 3 },
			{ 2, 1, 1, 5, 1, 1, 3, 5 },
			{ 4, 5, 5, 1, 5, 5, 5, 3 },
			{ 5, 6, 3, 2, 2, 4, 2, 2 },
			{ 3, 5, 4, 3, 3, 4, 1, 0 },
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
		int[,] testGrid11 = {{ 2, 8, 0, 4, 1, 1, 3, 0 },
			{ 2, 4, 1, 8, 4, 4, 3, 0 },
			{ 1, 2, 9, 2, 2, 0, 0, 3 },
			{ 1, 7, 9, 0, 1, 2, 3, 0 },
			{ 3, 1, 9, 3, 1, 0, 3, 1 },
			{ 1, 6, 9, 3, 9, 1, 2, 3 },
			{ 3, 2, 5, 2, 2, 1, 3, 0 },
			{ 3, 4, 2, 3, 0, 0, 3, 0 }
		}; 

		//2 4's at once
		int[,] testGrid12 = {{ 4, 5, 6, 4, 2, 3, 0, 3 },
			{ 3, 4, 6, 2, 2, 5, 5, 3 },
			{ 2, 1, 1, 5, 1, 2, 5, 2 },
			{ 4, 5, 5, 1, 5, 3, 5, 3 },
			{ 5, 6, 3, 2, 2, 6, 5, 5 },
			{ 3, 5, 4, 3, 3, 5, 0, 0 },
			{ 4, 5, 6, 3, 6, 6, 2, 0 },
			{ 2, 0, 5, 5, 0, 5, 2, 4 }
		}; 

		//Ultimate test
		int[,] testGrid13 = {{ 2, 8, 0, 4, 1, 1, 3, 0 },
			{ 2, 4, 1, 8, 4, 4, 3, 0 },
			{ 1, 2, 9, 2, 2, 0, 0, 3 },
			{ 1, 7, 9, 0, 1, 2, 3, 0 },
			{ 3, 1, 9, 3, 1, 0, 3, 0 },
			{ 1, 6, 9, 3, 9, 1, 0, 3 },
			{ 3, 2, 5, 2, 2, 1, 3, 0 },
			{ 3, 4, 2, 3, 0, 0, 3, 0 }
		}; 

		//Ultimate test
		int[,] testGrid14 = {{ 2, 8, 2, 4, 1, 3, 1, 0 },
			{ 2, 4, 2, 8, 4, 3, 2, 0 },
			{ 1, 2, 1, 3, 3, 3, 3, 3 },
			{ 1, 7, 2, 0, 1, 3, 2, 2 },
			{ 3, 1, 3, 3, 1, 0, 3, 1 },
			{ 1, 6, 4, 3, 0, 1, 0, 3 },
			{ 3, 2, 5, 2, 2, 1, 3, 0 },
			{ 3, 4, 2, 3, 0, 0, 3, 0 }
		};
	}
}

public enum GameState
{
	None,
	Animating,
	selectionStarted,
	swappingTiles,
	destroyable,
	GameOver
}

public enum BoosterState
{

	Destroy,
	dontDestroy
}
