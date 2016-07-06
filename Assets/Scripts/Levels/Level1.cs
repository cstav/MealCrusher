using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level1 : GridManager {
	int boostersNeeded;
	int specialBoostersNeeded;
	int target;


	void Awake(){

		init ();
	}

	void init(){


		GridWidth = 8;
		GridHeight = 8;
		target = 7000;
		moves = 20;
		gameoverMessage = "Level One Game Over Message";


	}

	public override void CheckCriteria(){

		if ((scorehandler.GetScore () >= target && outOfMoves) && !gameEnded) {
			LevelPassed ();
		}
		else if(outOfMoves){
			OutOfMoves ();
		}
			
		
	}



}
