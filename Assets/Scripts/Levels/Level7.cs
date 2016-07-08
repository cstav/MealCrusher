﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level7 : GridManager {
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

	public override void CheckCriteria(){

		if ((fatLeft <=0 && outOfMoves) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (8);
		}
		else if(outOfMoves){
			OutOfMoves ();
		}
			
		
	}



}
