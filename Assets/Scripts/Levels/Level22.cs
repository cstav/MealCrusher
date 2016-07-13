using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level22 : GridManager {
	
	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		moves = 20;
		gameoverMessage = "Level One Game Over Message";
		fatOn = true;
		junkFoodOn = true;

		int[,] fatPos =  {

			{ 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{ 1, 0, 0, 0, 0, 0, 0, 1, 0 },
			{ 1, 0, 0, 0, 0, 0, 0, 1, 0 },
			{ 1, 0, 0, 0, 0, 0, 0, 1, 0 },
			{ 1, 0, 0, 0, 0, 0, 0, 1, 0 },
			{ 1, 0, 0, 0, 0, 0, 0, 1, 0 },
			{ 1, 0, 0, 0, 0, 0, 0, 1, 0 },
			{ 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 }
		};

		fatPositions = fatPos;


	}

	public override void CheckCriteria(){

		if ((fatLeft <=0 && outOfMoves) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (23);
			UpdateHS ();
		}
		else if(outOfMoves && !gameEnded){
			OutOfMoves ();
			UpdateHS ();
		}
			
		
	}



}
