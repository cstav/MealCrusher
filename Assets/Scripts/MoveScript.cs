﻿using UnityEngine;
using System.Collections;
using System.Threading;

public class MoveScript : MonoBehaviour {

	private GridManager gm;
	private string colour;
	public Vector2 position;
	private float currentAlpha;
	float decrease = 0.1f;
	public bool isBooster = false;
	public int tileIndex;


	void Awake(){

		gm = GameObject.Find ("GameController").GetComponent<GridManager> ();
	}

	public void Move(Vector2 destination){

		//Debug.Log ("Tried to change pos");


		iTween.MoveTo (gameObject, iTween.Hash("x", destination.x, "y", destination.y, "time", 1.0f, "onComplete", "printCompleted"));




		//gameObject.transform.position = destination;
	}

	public void changeColor(){

		GetComponent<SpriteRenderer> ().color = Color.blue;
	
	}

	public void setColour(string colour){
		this.colour = colour;
	}

	public string getColour(){
		return colour;
	}

	public void setGridPosition(Vector2 position){

	}

	public Vector2 getGridPosition(){
		return position;
	}


	//each tile checks how many spots are empty below and moves down that many spot
	public void GravityCheck(){

		//when tiles are moved we want to stop them from flashing
		StopFlashing ();

		int missingTileCount = 0;

		for (int y = (int)transform.position.y; y>=0; y--) {

			RaycastHit2D hit = Physics2D.Raycast (new Vector2(transform.position.x, y), Vector2.zero, 0f);
			if(!hit){
				missingTileCount++;
			}
		}

		if (missingTileCount > 0) {
			iTween.MoveTo (gameObject, iTween.Hash( "y", transform.position.y - missingTileCount, "x", transform.position.x, "time", gm.dropTime));
		}

		if (isBooster) {
			Debug.Log ("There were " + missingTileCount + " tiles missing underneath x:" + transform.position.x + " y: " + transform.position.y);

		}

		setGridPosition(new Vector2((int)transform.position.x,(int)transform.position.y - missingTileCount));
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