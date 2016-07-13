using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level24 : GridManager {
	int specialBoostersNeeded;

	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		moves = 25;
		specialBoostersNeeded = 3;
		gameoverMessage = "Level twenty Four Game Over Message";


	}

	public override void CheckCriteria(){

		if ((specBoostersCreated >= specialBoostersNeeded && outOfMoves) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (25);
			UpdateHS ();
		} else if (outOfMoves && !gameEnded) {
			OutOfMoves ();
			UpdateHS ();
		}

	}


}
