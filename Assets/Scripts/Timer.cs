using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timeTF;
	LevelScript leveldata;
	int seconds;
	GridManager gm;
	public GameState currentState;


	// Use this for initialization
	void Start () {

		leveldata = GameObject.Find ("LevelHandler").GetComponent<LevelScript> ();
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
			if (!leveldata.gameEnded) {
				gm.timesUp = true;
				if (currentState == GameState.None) {
					leveldata.TimesUp ();
				}
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
