using UnityEngine;
using System.Collections;

public class LevelSelector : MonoBehaviour {

	private GameObject selectedStone;
	public GameObject[] stones;
	// Use this for initialization
	void Start () {
	
		for (int i = 0; i < stones.Length; i++) {
			stones [i].GetComponent<StoneScript> ().SetLevel (2);
		}

		stones [1].GetComponent<StoneScript> ().SetLevel (3);
	}


	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
				SelectStone ();
		}
	}

	public void SelectStone(){

		Vector2 rayPos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		RaycastHit2D hit = Physics2D.Raycast (rayPos, Vector2.zero, 0f);

		if (hit) {
			selectedStone = hit.collider.gameObject;
			Application.LoadLevel (selectedStone.GetComponent<StoneScript>().GetLevel());

		}
	}
}
