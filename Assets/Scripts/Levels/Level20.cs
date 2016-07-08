using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Level20 : GridManager {
	List<GameObject> fruitList;


	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		time = 35; 
		ingredientsOn = true;
		ingredientHolders = 5;
		gameoverMessage = "Level twenty Game Over Message";


	}

	void SetupFruitList(){

		fruitList.Add (TilePrefabs [8]);
		fruitList.Add (TilePrefabs [8]);
		fruitList.Add (TilePrefabs [8]);
		fruitList.Add (TilePrefabs [8]);
		fruitList.Add (TilePrefabs [8]);

	}

	public override void CheckCriteria(){

		if ((HoldersAreFull() && timesUp) && !gameEnded) {
			LevelPassed ();
		} else if (timesUp && !gameEnded ) {
			OutOfMoves ();
		}

	}

	void CreateFruit(Vector2 position){

		int x = Mathf.RoundToInt (position.x);
		int y = Mathf.RoundToInt (position.y);

		GameObject fruit = Instantiate (fruitList[0], new Vector2 (x, y), Quaternion.identity) as GameObject;
		fruitList.RemoveAt (0);
		fruit.GetComponent<TileScript> ().isIngredient = true;

		Grid [x, y] = fruit;
		Grid [x, y].GetComponent<TileScript> ().setName ("ingredient" + x+y);


	}

	protected override IEnumerator CreateGrid (List<Vector2> cigPositions)
	{
		fruitList = new List<GameObject> ();
		SetupFruitList ();

		List<Vector2> fruitPositions = new List<Vector2> ();
		fruitPositions.Add (new Vector2(2,3));
		fruitPositions.Add (new Vector2(3,3));
		fruitPositions.Add (new Vector2(4,3));
		fruitPositions.Add (new Vector2(5,3));
		fruitPositions.Add (new Vector2(6,3));

		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];

		foreach (Vector2 cell in gridLayout) {
			int x = Mathf.RoundToInt (cell.x);
			int y = Mathf.RoundToInt (cell.y);

			if (cigPositions.Contains (new Vector2 (x, y))) {
				CreateCigarette (new Vector2 (x, y));
			}
			else if(fruitPositions.Contains(new Vector2(x,y))){
				CreateFruit (new Vector2(x,y));
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