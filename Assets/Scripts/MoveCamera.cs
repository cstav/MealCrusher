using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour {
	int currentWeek;
	List<Vector2> weeks;
	public Text weekText;
	// Use this for initialization
	void Start () {

		currentWeek = 0;
	
		weeks = new List<Vector2> ();
		weeks.Add( new Vector2 (-13.7f,5.26f));
		weeks.Add(new Vector2 (-5.3f,5.26f));
		weeks.Add(new Vector2 (4.07f,5.26f));
		weeks.Add(new Vector2 (15.05f,5.26f));
		weeks.Add(new Vector2 (23f,5.26f));
		weeks.Add(new Vector2 (23f,-5.28f));
		weeks.Add(new Vector2 (15.27f,-5.28f));
		weeks.Add(new Vector2 (4.1f,-5.28f));
		weeks.Add(new Vector2 (-5.34f,-5.28f));
		weeks.Add(new Vector2 (-13.71f,-5.28f));

	}
		

	public void MoveTo(Vector2 pos){
		iTween.MoveTo (gameObject, iTween.Hash("x", pos.x, "y", pos.y, "time", 3));
	}


	// Update is called once per frame
	void Update () {
	

		if (Input.GetKeyDown ("space")) {
			nextLevel ();
		}
	}


	public void nextLevel(){

		if (currentWeek < 9) {
			currentWeek++;
			setWeek ();
		}
		Debug.Log ("prev");
		MoveTo (weeks[currentWeek]);
	}

	public void prevLevel(){

		if (currentWeek > 0) {
			currentWeek--;
			setWeek ();
		}
		Debug.Log ("prev");
		MoveTo (weeks[currentWeek]);
	}

	public void setWeek(){

		weekText.text = ""+ (currentWeek + 1);
	}
}
