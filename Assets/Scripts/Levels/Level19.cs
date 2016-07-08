using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Level19 : GridManager {


	void Awake(){
		init ();
	}

	void init(){
		GridWidth = 8;
		GridHeight = 8;
		moves = 20;
		gameoverMessage = "Level Nineteen Game Over Message";
		cigOn = true;

	}
		

	public override void CheckCriteria(){

		if ((beersLeft <=0 && outOfMoves) && !gameEnded) {
			LevelPassed ();
		}
		else if(outOfMoves){
			OutOfMoves ();
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
		beerPositions.Add (new Vector2(1,6));
		beerPositions.Add (new Vector2(2,6));
		beerPositions.Add (new Vector2(3,6));
		beerPositions.Add (new Vector2(4,6));
		beerPositions.Add (new Vector2(5,6));
		beerPositions.Add (new Vector2(6,6));
		beerPositions.Add (new Vector2(1,4));
		beerPositions.Add (new Vector2(2,4));
		beerPositions.Add (new Vector2(3,4));
		beerPositions.Add (new Vector2(4,4));
		beerPositions.Add (new Vector2(5,4));
		beerPositions.Add (new Vector2(6,4));

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
