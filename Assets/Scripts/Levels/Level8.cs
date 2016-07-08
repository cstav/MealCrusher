using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Level8 : GridManager {

	void Awake(){

		init ();
	}

	void init(){

		GridWidth = 8;
		GridHeight = 8;
		moves = 13;
		gameoverMessage = "Level Eight Game Over Message";
		cigOn = true;
	}

	protected override List<Vector2> MakeCigarettes(){

		List<Vector2> cp = new List<Vector2> ();
		for (int x = 0; x < GridWidth; x++) {
			cp.Add (new Vector2 (x, 3));
		}

		return cp;
	
	}

	public override void CheckCriteria(){

		if ((cigCount <=0 && outOfMoves) && !gameEnded) {
			LevelPassed ();
			GameManager.instance.UnlockLevel (9);
		}
		else if(outOfMoves){
			OutOfMoves ();
		}
	}
}
