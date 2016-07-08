using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level18 : GridManager {
	int boostersNeeded;
	int specialBoostersNeeded;
	int target;


	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		moves = 34;
		gameoverMessage = "Level Seven Game Over Message";
		fatOn = true;
		junkFoodOn = true;

		int[,] fatPos =  {

			{ 0, 0, 0, 0, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 1, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 1, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 }
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
					GameObject hd = Instantiate (candy, new Vector2 (x, y), Quaternion.identity) as GameObject;
					junkFoodGrid [x, y] = hd;
				}

			}
		}
	}

	public override void CheckCriteria(){

		if ((fatLeft <=0 && outOfMoves) && !gameEnded) {
			LevelPassed ();
		}
		else if(outOfMoves){
			OutOfMoves ();
		}
			
		
	}



}
