using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Level10 : GridManager {

	void Awake(){

		init ();
	}

	void init(){

		GridWidth = 8;
		GridHeight = 8;
		time = 40;
		gameoverMessage = "Level 10 Game Over Message";
		cigOn = true;
	}

	protected override List<Vector2> MakeCigarettes(){

		List<Vector2> cp = new List<Vector2> ();
		for (int x = 0; x < GridWidth; x++) {
			cp.Add (new Vector2 (x, 3));
			cp.Add (new Vector2 (x, 4));
			cp.Add (new Vector2 (x, 5));
		}

		return cp;
	
	}

	public override void CheckCriteria(){

		if ((cigCount <=0 && timesUp) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (11);
			UpdateHS ();
		}
		else if(timesUp && !gameEnded){
			OutOfMoves ();
			UpdateHS ();
		}
	}
}
