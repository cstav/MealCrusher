using UnityEngine;
using System.Collections;
using System.Collections;
using UnityEngine.UI;

public class Level5 : GridManager {
	int boostersNeeded;
	int specialBoostersNeeded;
	int target;


	void Awake(){

		init ();
	}

	void init(){

		dropTime = 0;
		GridWidth = 8;
		GridHeight = 7;
		target = 7000;
		time = 35;
		gameoverMessage = "Level five Game Over Message";


	}

	public override void CheckCriteria(){

		if ((scorehandler.GetScore () >= target && timesUp) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (6);
		}
		else if(timesUp && !gameEnded){
			TimesUp ();
		}


	}



}
