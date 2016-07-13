using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timeTF;
	int seconds;
	GridManager gm;
	public GameState currentState;


	// Use this for initialization
	void Start () {


		gm = GameObject.Find ("GameController").GetComponent<GridManager> ();

		seconds = gm.time;

		Debug.Log ("Time Left: " + seconds);
		setTimerText ();

		InvokeRepeating ("ReduceTime", 3, 1);

	}



	void ReduceTime (){

		if (seconds > 0) {
			seconds--;
			setTimerText ();
		} else {
			if (!gm.gameEnded) {
				gm.timesUp = true;
				gm.CheckCriteria ();
			}
		}



	}

	void setTimerText(){
		timeTF.text = "TIME\n" + seconds;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
