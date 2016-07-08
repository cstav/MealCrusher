using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level4 : GridManager {
	int specialBoostersNeeded;

	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		moves = 13;
		specialBoostersNeeded = 1;
		gameoverMessage = "Level Four Game Over Message";


	}

	public override void CheckCriteria(){

		if ((specBoostersDestroyed >= specialBoostersNeeded && outOfMoves) && !gameEnded) {
			LevelPassed ();
		} else if (outOfMoves) {
			OutOfMoves ();
		}

	}


}
