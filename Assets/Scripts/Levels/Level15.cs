using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Level15 : GridManager {
	int boostersNeeded;
	int target;
	List<GameObject> nutsList;


	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		time = 40; 
		ingredientsOn = true;
		ingredientHolders = 5;
		gameoverMessage = "Level Two Game Over Message";


	}

	void SetupNutsList(){

		nutsList.Add (TilePrefabs [11]);
		nutsList.Add (TilePrefabs [11]);
		nutsList.Add (TilePrefabs [11]);
		nutsList.Add (TilePrefabs [11]);
		nutsList.Add (TilePrefabs [11]);

	}

	public override void CheckCriteria(){

		if ((HoldersAreFull() && timesUp) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (16);
		} else if (timesUp && !gameEnded ) {
			OutOfMoves ();
		}

	}

	void CreateNuts(Vector2 position){

		int x = Mathf.RoundToInt (position.x);
		int y = Mathf.RoundToInt (position.y);

		GameObject nut = Instantiate (nutsList[0], new Vector2 (x, y), Quaternion.identity) as GameObject;
		nutsList.RemoveAt (0);
		nut.GetComponent<TileScript> ().isIngredient = true;

		Grid [x, y] = nut;
		Grid [x, y].GetComponent<TileScript> ().setName ("ingredient" + x+y);


	}

	protected override IEnumerator CreateGrid (List<Vector2> cigPositions)
	{
		nutsList = new List<GameObject> ();
		SetupNutsList ();

		List<Vector2> nutPositions = new List<Vector2> ();
		nutPositions.Add (new Vector2(2,3));
		nutPositions.Add (new Vector2(3,3));
		nutPositions.Add (new Vector2(4,3));
		nutPositions.Add (new Vector2(5,3));
		nutPositions.Add (new Vector2(6,3));

		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];

		foreach (Vector2 cell in gridLayout) {
			int x = Mathf.RoundToInt (cell.x);
			int y = Mathf.RoundToInt (cell.y);

			if (cigPositions.Contains (new Vector2 (x, y))) {
				CreateCigarette (new Vector2 (x, y));
			}
			else if(nutPositions.Contains(new Vector2(x,y))){
				CreateNuts (new Vector2(x,y));
			}
			else {

				//int tileType = gridContent [x, y];
				int tileType = UnityEngine.Random.Range(0,5);

				GameObject tile = Instantiate (TilePrefabs [tileType], new Vector2 (x, y), Quaternion.identity) as GameObject;
				tile.GetComponent<TileScript> ().tileIndex = tileType;

				Grid [x, y] = tile;

				//Assign the tile a name
				Grid [x, y].GetComponent<TileScript> ().setName (tileColours [tileType]);
			}
			yield return new WaitForSeconds (.02f);

		}

		StartCoroutine (Check ());
	}



}