using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level4 : GridManager {
	int specialBoostersNeeded;

	void Awake(){

		init ();
	}

	void init(){

		dropTime = 0;

		GridWidth = 8;
		GridHeight = 8;
		moves = 13;
		specialBoostersNeeded = 1;
		gameoverMessage = "Level Four Game Over Message";


	}

	public override void CheckCriteria(){

		if ((specBoostersDestroyed >= specialBoostersNeeded && outOfMoves) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (5);
			UpdateHS ();
		} else if (outOfMoves) {
			OutOfMoves ();
			UpdateHS ();
		}

	}


}
