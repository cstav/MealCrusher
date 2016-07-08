using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Level13 : GridManager {
	int boostersNeeded;
	int target;
	List<GameObject> fishList;


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

	void SetupFishList(){

		fishList.Add (TilePrefabs [10]);
		fishList.Add (TilePrefabs [10]);
		fishList.Add (TilePrefabs [10]);
		fishList.Add (TilePrefabs [10]);
		fishList.Add (TilePrefabs [10]);

	}

	public override void CheckCriteria(){

		if ((HoldersAreFull() && outOfMoves) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (14);
		} else if (outOfMoves) {
			OutOfMoves ();
		}

	}

	void CreateFish(Vector2 position){

		int x = Mathf.RoundToInt (position.x);
		int y = Mathf.RoundToInt (position.y);

		GameObject fish = Instantiate (fishList[0], new Vector2 (x, y), Quaternion.identity) as GameObject;
		fishList.RemoveAt (0);
		fish.GetComponent<TileScript> ().isIngredient = true;

		Grid [x, y] = fish;
		Grid [x, y].GetComponent<TileScript> ().setName ("ingredient" + x+y);


	}

	protected override IEnumerator CreateGrid (List<Vector2> cigPositions)
	{
		fishList = new List<GameObject> ();
		SetupFishList ();

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
				CreateFish (new Vector2(x,y));
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