using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Level25 : GridManager {
	int boostersNeeded;
	int specialBoostersNeeded;
	int target;


	void Awake(){

		init ();
	}

	void init(){

		dropTime = 0.1f;
		swapTime = 0.1f;

		GridWidth = 8;
		GridHeight = 8;
		time = 120;
		gameoverMessage = "Level 25 Game Over Message";
		fatOn = true;
		junkFoodOn = true;

		int[,] fatPos =  {

			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 1, 1, 1, 1, 0, 0, 0 },
			{ 0, 0, 1, 1, 1, 1, 0, 0, 0 },
			{ 0, 0, 1, 1, 1, 1, 0, 0, 0 },
			{ 0, 0, 1, 1, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 }
		};

		fatPositions = fatPos;


	}

	//change hotdog to candy
	protected override void CreateJunkFood ()
	{
		junkFoodGrid = new GameObject[GridWidth, GridHeight];


		for (int x = 0; x < GridHeight; x++) {
			for (int y = 0; y < GridWidth; y++) {
				if (fatPositions [x, y] == 1) {
					int random = UnityEngine.Random.Range (0, 2);

					if (random == 0) {
						GameObject c = Instantiate (candy, new Vector2 (x, y), Quaternion.identity) as GameObject;
						junkFoodGrid [x, y] = c;
					} else if (random == 1) {
						GameObject hd = Instantiate (hotdog, new Vector2 (x, y), Quaternion.identity) as GameObject;
						junkFoodGrid [x, y] = hd;
					}
				}

			}
		}
	}

	public override void CheckCriteria(){

		if ((fatLeft <=0 && beersLeft <=0 && timesUp) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (26);
		}
		else if(timesUp && !gameEnded){
			TimesUp ();
		}
			
		
	}


	void CreateBeer(Vector2 position){

		int x = Mathf.RoundToInt (position.x);
		int y = Mathf.RoundToInt (position.y);

		GameObject beer = Instantiate (TilePrefabs[9], new Vector2 (x, y), Quaternion.identity) as GameObject;
		Grid [x, y] = beer;
		Grid [x, y].GetComponent<TileScript> ().setName ("beer");
		beersLeft++;

	}

	protected override IEnumerator CreateGrid (List<Vector2> cigPositions)
	{

		List<Vector2> beerPositions = new List<Vector2> ();
		beerPositions.Add (new Vector2(7,0));
		beerPositions.Add (new Vector2(7,1));
		beerPositions.Add (new Vector2(7,2));
		beerPositions.Add (new Vector2(7,3));
		beerPositions.Add (new Vector2(7,4));
		beerPositions.Add (new Vector2(7,5));
		beerPositions.Add (new Vector2(7,6));
		beerPositions.Add (new Vector2(7,7));
		beerPositions.Add (new Vector2(0,0));
		beerPositions.Add (new Vector2(0,1));
		beerPositions.Add (new Vector2(0,2));
		beerPositions.Add (new Vector2(0,3));
		beerPositions.Add (new Vector2(0,4));
		beerPositions.Add (new Vector2(0,5));
		beerPositions.Add (new Vector2(0,6));
		beerPositions.Add (new Vector2(0,7));

		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];

		foreach (Vector2 cell in gridLayout) {
			int x = Mathf.RoundToInt (cell.x);
			int y = Mathf.RoundToInt (cell.y);

			if (cigPositions.Contains (new Vector2 (x, y))) {
				CreateCigarette (new Vector2 (x, y));
			}
			else if(beerPositions.Contains(new Vector2(x,y))){
				CreateBeer (new Vector2(x,y));
			}
			else {
				int tileType = UnityEngine.Random.Range(0,5);
				GameObject tile = Instantiate (TilePrefabs [tileType], new Vector2 (x, y), Quaternion.identity) as GameObject;
				tile.GetComponent<TileScript> ().tileIndex = tileType;
				Grid [x, y] = tile;
				Grid [x, y].GetComponent<TileScript> ().setName (tileColours [tileType]);
			}
			yield return new WaitForSeconds (.02f);

		}

		StartCoroutine (Check ());
	}



}
