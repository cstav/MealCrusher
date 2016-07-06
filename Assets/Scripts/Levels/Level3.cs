using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Level3 : GridManager {
	int boostersNeeded;
	int target;
	List<GameObject> ingredientsList;


	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		moves = 15; 
		ingredientsOn = true;
		ingredientHolders = 5;
		gameoverMessage = "Level Two Game Over Message";


	}

	void SetupIngredientsList(){

		ingredientsList.Add (TilePrefabs [5]);
		ingredientsList.Add (TilePrefabs [6]);
		ingredientsList.Add (TilePrefabs [7]);
		ingredientsList.Add (TilePrefabs [8]);
		ingredientsList.Add (TilePrefabs [5]);

	}

	public override void CheckCriteria(){

		if ((HoldersAreFull() && outOfMoves) && !gameEnded) {
			LevelPassed ();
		} else if (outOfMoves) {
			OutOfMoves ();
		}

	}

	void CreateIngredient(Vector2 position){

		int x = Mathf.RoundToInt (position.x);
		int y = Mathf.RoundToInt (position.y);

		GameObject ingred = Instantiate (ingredientsList[0], new Vector2 (x, y), Quaternion.identity) as GameObject;
		ingredientsList.RemoveAt (0);
		ingred.GetComponent<TileScript> ().isIngredient = true;
		Grid [x, y] = ingred;
		Grid [x, y].GetComponent<TileScript> ().setName ("ingredient" + x+y);


	}

	protected override IEnumerator CreateGrid (List<Vector2> cigPositions)
	{
		ingredientsList = new List<GameObject> ();
		SetupIngredientsList ();

		List<Vector2> ingredientPositions = new List<Vector2> ();
		ingredientPositions.Add (new Vector2(2,3));
		ingredientPositions.Add (new Vector2(3,3));
		ingredientPositions.Add (new Vector2(4,3));
		ingredientPositions.Add (new Vector2(5,3));
		ingredientPositions.Add (new Vector2(6,3));

		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];

		foreach (Vector2 cell in gridLayout) {
			int x = Mathf.RoundToInt (cell.x);
			int y = Mathf.RoundToInt (cell.y);

			if (cigPositions.Contains (new Vector2 (x, y))) {
				CreateCigarette (new Vector2 (x, y));
			}
			else if(ingredientPositions.Contains(new Vector2(x,y))){
				CreateIngredient (new Vector2(x,y));
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