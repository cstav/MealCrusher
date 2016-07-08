using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level2 : GridManager {
	int boostersNeeded;
	int target;


	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		moves = 14; //14
		boostersNeeded = 1;
		gameoverMessage = "Level Two Game Over Message";


	}

	public override void CheckCriteria(){

		if ((boostersDestroyed >= boostersNeeded && outOfMoves) && !gameEnded) {
			LevelPassed ();
		} else if (outOfMoves) {
			OutOfMoves ();
		}
	
	}


}
