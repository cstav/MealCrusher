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

		currentWeek = GameManager.instance.prevViewedWeek;
		setWeek ();
	
		weeks = new List<Vector2> ();
		weeks.Add( new Vector2 (-13.7f,5.26f));
		weeks.Add(new Vector2 (-5.3f,5.26f));
		weeks.Add(new Vector2 (4.07f,5.26f));
		weeks.Add(new Vector2 (15.05f,5.26f));
		weeks.Add(new Vector2 (23f,5.26f));
		weeks.Add(new Vector2 (23f,-4.8f));
		weeks.Add(new Vector2 (15.27f,-4.8f));
		weeks.Add(new Vector2 (4.1f,-4.8f));
		weeks.Add(new Vector2 (-5.34f,-4.8f));
		weeks.Add(new Vector2 (-13.71f,-4.8f));

		MoveInstantlyTo (weeks[currentWeek]);

	}
		

	public void MoveTo(Vector2 pos){
		iTween.MoveTo (gameObject, iTween.Hash("x", pos.x, "y", pos.y, "time", 3));
	}

	public void MoveInstantlyTo(Vector2 pos){
		
		gameObject.transform.position = new Vector3(pos.x, pos.y, -10);
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
		GameManager.instance.prevViewedWeek = currentWeek;
	}

	public void prevLevel(){

		if (currentWeek > 0) {
			currentWeek--;
			setWeek ();
		}
		Debug.Log ("prev");
		MoveTo (weeks[currentWeek]);
		GameManager.instance.prevViewedWeek = currentWeek;
	}

	public void setWeek(){

		weekText.text = ""+ (currentWeek + 1);
	}
}
