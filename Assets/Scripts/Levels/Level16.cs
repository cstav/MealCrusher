using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level16 : GridManager {
	int boostersNeeded;


	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		moves = 50; 
		boostersNeeded = 1;
		gameoverMessage = "Level Two Game Over Message";


	}

	public override void CheckCriteria(){

		if ((match5strawbs >= boostersNeeded) && !gameEnded) {
			LevelPassed ();
		} else if (outOfMoves) {
			OutOfMoves ();
		}
	
	}


}
