using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Level21 : GridManager {


	void Awake(){
		init ();
	}

	void init(){
		GridWidth = 8;
		GridHeight = 8;
		moves = 35;
		gameoverMessage = "Level 21 Game Over Message";
		cigOn = true;

	}
		

	public override void CheckCriteria(){

		if ((beersLeft <=0 && outOfMoves) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (22);
		}
		else if(outOfMoves && !gameEnded){
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
		beerPositions.Add (new Vector2(0,0));
		beerPositions.Add (new Vector2(1,0));
		beerPositions.Add (new Vector2(2,0));
		beerPositions.Add (new Vector2(3,0));
		beerPositions.Add (new Vector2(4,0));
		beerPositions.Add (new Vector2(5,0));
		beerPositions.Add (new Vector2(6,0));
		beerPositions.Add (new Vector2(7,0));
		beerPositions.Add (new Vector2(7,1));
		beerPositions.Add (new Vector2(7,2));
		beerPositions.Add (new Vector2(7,3));
		beerPositions.Add (new Vector2(7,4));
		beerPositions.Add (new Vector2(7,5));
		beerPositions.Add (new Vector2(7,6));
		beerPositions.Add (new Vector2(7,7));
		beerPositions.Add (new Vector2(0,7));
		beerPositions.Add (new Vector2(1,7));
		beerPositions.Add (new Vector2(2,7));
		beerPositions.Add (new Vector2(3,7));
		beerPositions.Add (new Vector2(4,7));
		beerPositions.Add (new Vector2(5,7));
		beerPositions.Add (new Vector2(6,7));
		beerPositions.Add (new Vector2(0,6));
		beerPositions.Add (new Vector2(0,5));
		beerPositions.Add (new Vector2(0,4));
		beerPositions.Add (new Vector2(0,3));
		beerPositions.Add (new Vector2(0,2));
		beerPositions.Add (new Vector2(0,1));


		playerinput.currentState = GameState.Animating;
		Grid = new GameObject[GridWidth, GridHeight];

		foreach (Vector2 cell in gridLayout) {
			int x = Mathf.RoundToInt (cell.x);
			int y = Mathf.RoundToInt (cell.y);

			if(beerPositions.Contains(new Vector2(x,y))){
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
