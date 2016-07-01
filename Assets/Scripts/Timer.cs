using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timeTF;
	LevelScript leveldata;
	int seconds;


	// Use this for initialization
	void Start () {

		leveldata = GameObject.Find ("LevelHandler").GetComponent<LevelScript> ();


		seconds = leveldata.GetTime();

		Debug.Log ("Time Left: " + seconds);
		setTimerText ();

		InvokeRepeating ("ReduceTime", 3, 1);

	}



	void ReduceTime (){

		if (seconds > 0) {
			seconds--;
			setTimerText ();
		} else {
			CancelInvoke ("ReduceTime");
			if (!leveldata.gameEnded) {
				leveldata.TimesUp ();
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
