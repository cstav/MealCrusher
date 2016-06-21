using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScript : MonoBehaviour {

	public bool fatOn;
	public bool hotDogOn;
	public bool cigOn;
	public bool ingredOn;	
	public bool timeBased;
	public bool moveBased;
	public int ingredientHolders;
	public int gridWidth;
	public int gridHeight;




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int[,] GetFatPositions(){

		return null;
	}

	public List<Vector2> GetCigPositions(){

		return null;
	}

	public int[,] GetGridContent(){

		return null;
	}

	public void TimesUp(){
		
	}

	public void OutOfMoves(){
		
	}

	public int[,] GetGridLayout(){
		
	}


}
