using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {

	private GridManager gm;
	private string tileName;
	private float currentAlpha;
	float decrease = 0.1f;
	public bool isBooster = false;
	public bool isSpecialBooster = false;
	public bool isIngredient = false;
	public int tileIndex;
	List<Vector2> gridLayout;

	void Start(){
		
	}

	void Awake(){

		gm = GameObject.Find ("GameController").GetComponent<GridManager> ();
		gridLayout = gm.GetGridLayout ();
	}
		

	public void changeColor(Color c){

		GetComponent<SpriteRenderer> ().color = c;
	
	}

	public void setName(string colour){
		this.tileName = colour;
	}

	public string getName(){
		return tileName;
	}
		


	//each tile checks how many spots are empty below and moves down that many spot
	public void GravityCheck(bool lastile){

		int x = Mathf.RoundToInt(gameObject.transform.position.x);

		//when tiles are moved we want to stop them from flashing
		StopFlashing ();

		int missingTileCount = 0;
	
	

		for (int y = Mathf.RoundToInt(transform.position.y); y>=0; y--) {



				RaycastHit2D hit = Physics2D.Raycast (new Vector2 (transform.position.x, y), Vector2.zero, 0f);
				if (!hit) {


				if (gridLayout.Contains (new Vector2 (x, y))) {
					missingTileCount++;
				}
					
				}
			

		}




		if (missingTileCount > 0) {
			if (lastile) {
				//Debug.Log ("Last tile---------------------------");
				iTween.MoveTo (gameObject, iTween.Hash ("y", Mathf.RoundToInt (transform.position.y) - missingTileCount, "x", transform.position.x, "time", gm.dropTime, "oncomplete", "LastTileFinished", "oncompletetarget", GameObject.Find ("GameController")));
			} else {
				iTween.MoveTo (gameObject, iTween.Hash ("y", Mathf.RoundToInt (transform.position.y) - missingTileCount, "x", transform.position.x, "time", gm.dropTime));
			}
		}

	}

	public void Flash(){
		InvokeRepeating ("ReduceAlpha", 3, 0.1f);

	}

	public void StopFlashing(){
		gameObject.GetComponent<Renderer> ().material.color = new Color (1, 1, 1, 1);
		CancelInvoke ("ReduceAlpha");

	}

	void ReduceAlpha(){
		currentAlpha = gameObject.GetComponent<Renderer> ().material.color.a;
		//Debug.Log ("currentAlpha: " + currentAlpha);


		if (gameObject.GetComponent<Renderer> ().material.color.a <= 0.1f) {
			decrease = -0.1f;

		} else if(gameObject.GetComponent<Renderer> ().material.color.a >= 0.99f) {
			decrease = 0.1f;
		}

		gameObject.GetComponent<Renderer> ().material.color = new Color (1, 1, 1, currentAlpha - decrease);
	}
}
