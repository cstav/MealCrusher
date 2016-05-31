using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CigMessage : MonoBehaviour {

	public Text CountDownText;
	Text textRef;
	PlayerInput playerinput;

	int time = 3;
	// Use this for initialization
	void Start () {
		playerinput = GameObject.Find ("GameController").GetComponent<PlayerInput> ();
		playerinput.currentState = GameState.Animating;
		Invoke ("RemoveFromScreen", 3);
		InvokeRepeating ("DecrementTime", 1,1);

		GameObject canvas = GameObject.Find ("Canvas");
		textRef = Instantiate (CountDownText, new Vector2(242,-40), Quaternion.identity) as Text;
		textRef.transform.SetParent (canvas.transform, false);

		//Instantiate (CountDownText, new Vector2(0,0), Quaternion.identity);

		SetTime ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetTime(){
		textRef.text = "" + time;
	}

	void RemoveFromScreen(){

		playerinput.currentState = GameState.None;
		Destroy (textRef);
		Destroy (gameObject);

	}

	void DecrementTime(){
		time--;
		SetTime ();
	}
}
