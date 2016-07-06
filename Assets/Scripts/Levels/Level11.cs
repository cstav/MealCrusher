using UnityEngine;
using System.Collections;

public class Level11 : GridManager {
	int boostersNeeded;
	int specialBoostersNeeded;
	string gameOverMessage;
	int target;


	void Awake(){

		init ();
	}

	void init(){

		GridWidth = 8;
		GridHeight = 8;
		target = 7000;
		moves = 20;


	}

	public override void CheckCriteria(){

		if (fatLeft <= 0) {
			//leveldata.LevelPassed ();
		}
			
		
	}

}
