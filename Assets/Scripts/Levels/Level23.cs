using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level23 : GridManager {
	int boostersNeeded;
	int target;


	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		moves = 30; //14
		boostersNeeded = 3;
		target = 15000;
		gameoverMessage = "Level 23 Game Over Message";


	}

	public override void CheckCriteria(){

		if ((boosterFromBread >= boostersNeeded && scorehandler.GetScore() >= target && outOfMoves) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (24);
		} else if (outOfMoves && !gameEnded) {
			OutOfMoves ();
		}
	
	}


}
