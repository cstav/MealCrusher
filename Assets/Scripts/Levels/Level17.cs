using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level17 : GridManager {
	int boostersNeeded;
	int specialBoostersNeeded;
	int target;


	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		target = 15000;
		moves = 30;
		gameoverMessage = "Level 17 Game Over Message";



	}

	public override void CheckCriteria(){

		if ((scorehandler.GetScore () >= target && outOfMoves) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (18);
			UpdateHS ();
		}
		else if(outOfMoves && !gameEnded){
			OutOfMoves ();
			UpdateHS ();
		}
			
		
	}



}
