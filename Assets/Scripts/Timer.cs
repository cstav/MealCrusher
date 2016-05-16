using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timeTF;
	int seconds;

	// Use this for initialization
	void Start () {
	
		seconds = 40;
		setTimerText ();

		InvokeRepeating ("ReduceTime", 1, 1);

	}

	void ReduceTime (){

		seconds--;
		setTimerText ();

		if (seconds == 0) {
			CancelInvoke ("ReduceTime");
		}

	}

	void setTimerText(){
		timeTF.text = "TIME\n" + seconds;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
