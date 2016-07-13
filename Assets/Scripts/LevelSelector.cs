using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
	
	private GameObject selectedStone;
	public GameObject[] stones;
	bool locked = false;
	// Use this for initialization
	void Start ()
	{
		
	
		for (int i = 0; i < 50; i++) {
			int level = i;
			stones [i].GetComponent<StoneScript> ().SetLevel (i+2);

			int week = level / GameManager.instance.levelsPerWeek;
			int weekLevel = level % GameManager.instance.levelsPerWeek;

			Debug.Log ("Current Level: " + i);

			stones [i].GetComponent<StoneScript>().scoreText.text = ""+ GameManager.instance.GetHighscore (level);

//			if (GameManager.instance.levelState [week, weekLevel] == locked) {
//				Destroy (stones [i]);
//			}
		}


		//stones [1].GetComponent<StoneScript> ().SetLevel (3);
	}

	void UpdateScoreLabel(GameObject stone){
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			SelectStone ();
		}
	}

	public void SelectStone ()
	{

		Vector2 rayPos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		RaycastHit2D hit = Physics2D.Raycast (rayPos, Vector2.zero, 0f);

		if (hit) {
			selectedStone = hit.collider.gameObject;
			Application.LoadLevel (selectedStone.GetComponent<StoneScript> ().GetLevel ());

		}
	}
}
