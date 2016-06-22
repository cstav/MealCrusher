using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class LevelScript : MonoBehaviour {

	public bool fatOn;
	public bool hotDogOn;
	public bool cigOn;
	public bool ingredientsOn;	
	public int ingredientHolders;
	public bool timeBased;
	public bool moveBased;
	public int gridWidth;
	public int gridHeight;
	int [,] gridContent;
	int [,] fatPositions;
	int [,] gridLayout;
	List<Vector2> cigPositions;
	int moves;



	void Awake(){
		
	}
	// Use this for initialization
	void Start () {

		int level = Application.loadedLevel;

		switch (level) {

		case 2:
			//l1
			SetUpL1();
			break;
		case 3:
			//l2
			break;
		case 4:
			//l3
			break;
		case 5:
			//l4
			break;
		case 6:
			//l5
			break;
		case 7:
			//l6
			break;
		case 8:
			//l7
			break;
		case 9:
			//l8
			break;
		case 10:
			//l9
			break;
		case 11:
			//l10
			break;

		}



	}

	void SetUpL1(){
		
		fatOn = false;
		hotDogOn = false;
		cigOn = true;
		ingredientsOn = true;
		ingredientHolders = 5;
		gridWidth = 9;
		gridHeight = 9;
		moves = 20;
		gridLayout = new int[gridWidth, gridHeight];

		int[,] fatPos = {

			{ 0, 0, 0, 0, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 }
		};

		SetFatPositions (fatPos);

		int[,] grid = {

			{ 2, 8, 2, 4, 1, 3, 1, 0, 0 },
			{ 2, 3, 2, 8, 4, 3, 2, 0, 1 },
			{ 1, 0, 1, 3, 3, 2, 3, 3, 2 },
			{ 1, 2, 2, 0, 1, 3, 2, 2, 3 },
			{ 3, 9, 2, 2, 1, 0, 3, 1, 4 },
			{ 1, 9, 4, 3, 0, 1, 0, 3, 5 },
			{ 3, 2, 5, 2, 2, 1, 3, 0, 1 },
			{ 3, 4, 2, 3, 0, 0, 3, 0, 2 },
			{ 2, 4, 2, 3, 0, 0, 1, 4, 2 }
		}; 

		SetGridContent (grid);

		List<Vector2> cigPos = new List<Vector2> ();

		for (int x = 0; x < gridWidth; x++) {
			cigPos.Add (new Vector2 (x, 3));
		}

		SetCigPositions (cigPos);


		int[,] layout = {

			{ 0, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 0 }
		}; 

		SetGridLayout (layout);

	}
	void SetUpL2(){

	}
	void SetUpL3(){

	}
	void SetUpL4(){

	}
	void SetUpL5(){

	}
	void SetUpL6(){

	}
	void SetUpL7(){

	}
	void SetUpL8(){

	}
	void SetUpL9(){

	}
	void SetUpL10(){

	}

	
	// Update is called once per frame
	void Update () {
	
	}

	void SetFatPositions(int[,] fatPositions){

		this.fatPositions = fatPositions;
		
	}

	public int[,] GetFatPositions(){

		return fatPositions;
	}


	void SetCigPositions(List<Vector2> cigPositions){
		this.cigPositions = cigPositions;
	}

	public List<Vector2> GetCigPositions(){
		return cigPositions;
	}
		
	void SetGridContent(int[,] gridContent){
		this.gridContent = gridContent;
	}

	public int[,] GetGridContent(){
		return gridContent;
	}

	void SetGridLayout(int[,] layout){

		Array.Copy (layout, gridLayout, layout.Length);


	}

	public int[,] GetGridLayout(){

		return gridLayout;
	}

	public void TimesUp(){
		
	}

	public void OutOfMoves(){
		
	}

	public void GameOver(){
		
	}




}
