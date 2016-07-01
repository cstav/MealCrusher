using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

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
	List<Vector2> gridLayout;
	List<Vector2> cigPositions;
	public int moves;
	public int time;
	public int target;
	public int boostersNeeded;
	public bool objectiveFat = false;
	public bool objectiveBeer = false;
	public bool objectiveCiggy = false;
	public int specBoostersNeeded;
	public string gameoverMessage;
	public bool gameEnded = false;


	//extremely high number that is impossible to reach
	const int UNATTAINABLE = 1000000;

	//Game Over screens
	GameObject timesUp;
	GameObject outOfMoves;
	GameObject levelPassed;
	Button retry;
	Button menu;
	Text levelOverText;
	Text scoretext;

	//scripts
	ScoreHandler scorehandler;






	// Use this for initialization

	void LoadAssets(){

		timesUp = Resources.Load ("TIMES UP", typeof(GameObject)) as GameObject;
		retry = Resources.Load ("Retry", typeof(Button)) as Button;
		menu = Resources.Load ("Menu", typeof(Button)) as Button;
		levelOverText = Resources.Load ("Level Over Text", typeof(Text)) as Text;
		scoretext = Resources.Load ("Score Text", typeof(Text)) as Text;
		levelPassed = Resources.Load ("LEVEL PASSED", typeof(GameObject)) as GameObject;
		outOfMoves = Resources.Load ("OUT OF MOVES", typeof(GameObject)) as GameObject;


		scorehandler = GameObject.Find ("scoretext").GetComponent<ScoreHandler> ();
	}


	void Awake () {

		LoadAssets ();

		int level = Application.loadedLevel;

		switch (level) {

		case 2:
			//l1
			SetUpL1();
			break;
		case 3:
			//l2
			SetUpL2();
			break;
		case 4:
			//l3
			SetUpL3();
			break;
		case 5:
			//l4
			SetUpL4();
			break;
		case 6:
			//l5
			SetUpL5();
			break;
		case 7:
			//l6
			SetUpL6();
			break;
		case 8:
			//l7
			SetUpL7();
			break;
		case 9:
			//l8
			SetUpL8();
			break;
		case 10:
			SetUpL9();
			//l9
			break;
		case 11:
			//l10
			SetUpL10();
			break;

		}



	}

	void SetUpL1(){
		
		fatOn = false;
		hotDogOn = false;
		cigOn = false;
		ingredientsOn = true;
		ingredientHolders = 0;
		gridWidth = 8;
		gridHeight = 8;
		moves = 20;
		time = 5;
		target = 7000;
		boostersNeeded = UNATTAINABLE;
		specBoostersNeeded = UNATTAINABLE;
		gameoverMessage = "Did you know that you should drink about 8 glasses of water a day to keep healthy?";

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


		int[,] grid = {{  2, 1, 0, 4, 1, 1, 3, 0 },
			{ 2, 4, 2, 2, 4, 4, 3, 0 },
			{ 1, 2, 0, 2, 2, 0, 0, 3 },
			{ 1, 0, 2, 0, 1, 2, 3, 0 },
			{ 3, 1, 1, 3, 1, 0, 3, 0 },
			{ 1, 4, 2, 3, 0, 1, 0, 3 },
			{ 3, 2, 0, 2, 2, 1, 3, 0 },
			{ 3, 4, 2, 3, 0, 0, 3, 0 }
		}; 


		SetGridContent (grid);

		List<Vector2> cigPos = new List<Vector2> ();

		for (int x = 0; x < gridWidth; x++) {
			cigPos.Add (new Vector2 (x, 3));
		}

		SetCigPositions (cigPos);


	}
	void SetUpL2(){


		fatOn = false;
		hotDogOn = false;
		cigOn = false;
		ingredientsOn = false;
		ingredientHolders = 0;
		gridWidth = 8;
		gridHeight = 8;
		moves = 14; //14
		time = 0;
		target = UNATTAINABLE;
		boostersNeeded = 1;
		specBoostersNeeded = UNATTAINABLE;
		gameoverMessage = "Did you know that you should drink about 8 glasses of water a day to keep healthy?";




		int[,] grid = {{  0, 1, 0, 3, 1, 4, 2, 0 },
						{ 1, 4, 1, 4, 2, 0, 3, 1 },
						{ 2, 1, 2, 0, 3, 1, 4, 2 },
						{ 3, 0, 3, 1, 4, 2, 0, 3 },
						{ 4, 2, 4, 2, 0, 3, 1, 4 },
						{ 0, 3, 0, 3, 1, 4, 2, 0 },
						{ 1, 4, 1, 4, 2, 0, 3, 1 },
						{ 2, 0, 2, 0, 3, 1, 4, 2 }
					}; 


		SetGridContent (grid);


	}
	void SetUpL3(){

		fatOn = false;
		hotDogOn = false;
		cigOn = false;
		ingredientsOn = true;
		ingredientHolders = 3;
		gridWidth = 8;
		gridHeight = 8;
		moves = 15;
		time = 0;
		target = UNATTAINABLE;
		boostersNeeded = UNATTAINABLE;
		specBoostersNeeded = UNATTAINABLE;
		gameoverMessage = "Did you know that you should drink about 8 glasses of water a day to keep healthy?";



		int[,] grid = { { 2, 1, 0, 4, 1, 1, 5, 0 },
						{ 2, 4, 1, 2, 4, 4, 3, 0 },
						{ 1, 2, 3, 2, 2, 0, 0, 3 },
						{ 1, 4, 2, 0, 1, 2, 3, 8 },
						{ 3, 1, 0, 3, 1, 0, 3, 0 },
						{ 1, 3, 0, 3, 0, 1, 0, 3 },
						{ 3, 2, 1, 2, 2, 1, 3, 0 },
						{ 3, 4, 2, 3, 0, 0, 6, 0 }
					}; 


		SetGridContent (grid);


	}
	void SetUpL4(){

		fatOn = false;
		hotDogOn = false;
		cigOn = false;
		ingredientsOn = false;
		ingredientHolders = 0;
		gridWidth = 8;
		gridHeight = 8;
		moves = 13;
		time = 0;
		target = UNATTAINABLE;
		boostersNeeded = UNATTAINABLE;
		specBoostersNeeded = 1;
		gameoverMessage = "Did you know that you should drink about 8 glasses of water a day to keep healthy?";




		int[,] grid = {{  0, 1, 0, 3, 1, 4, 2, 0 },
						{ 1, 4, 1, 4, 2, 0, 3, 1 },
						{ 2, 1, 2, 0, 3, 1, 4, 2 },
						{ 3, 0, 0, 1, 4, 2, 0, 3 },
						{ 4, 3, 3, 2, 3, 3, 1, 4 },
						{ 0, 3, 0, 0, 1, 4, 2, 0 },
						{ 1, 4, 1, 4, 2, 0, 3, 1 },
						{ 2, 0, 2, 0, 3, 1, 4, 2 }
					}; 


		SetGridContent (grid);

	}
	void SetUpL5(){


		fatOn = false;
		hotDogOn = false;
		cigOn = false;
		ingredientsOn = true;
		ingredientHolders = 0;
		gridWidth = 8;
		gridHeight = 8;
		moves = UNATTAINABLE; //change to unlimited somehow
		time = 35;
		target = 7000;
		boostersNeeded = UNATTAINABLE;
		specBoostersNeeded = UNATTAINABLE;
		gameoverMessage = "Did you know that you should drink about 8 glasses of water a day to keep healthy?";





		int[,] grid = {{  2, 1, 0, 4, 1, 1, 3, 0 },
						{ 2, 4, 2, 2, 4, 4, 3, 0 },
						{ 1, 2, 0, 2, 2, 0, 0, 3 },
						{ 1, 0, 2, 0, 1, 2, 3, 0 },
						{ 3, 1, 1, 3, 1, 0, 3, 0 },
						{ 1, 4, 2, 3, 0, 1, 0, 3 },
						{ 3, 2, 0, 2, 2, 1, 3, 0 },
						{ 3, 4, 2, 3, 0, 0, 3, 0 }
					}; 


		SetGridContent (grid);


	}
	void SetUpL6(){

		fatOn = true;
		hotDogOn = false;
		cigOn = false;
		ingredientsOn = false;
		ingredientHolders = 0;
		gridWidth = 8;
		gridHeight = 8;
		moves = 20;
		time = 0;
		target = UNATTAINABLE;
		boostersNeeded = UNATTAINABLE;
		specBoostersNeeded = UNATTAINABLE;
		gameoverMessage = "Did you know that you should drink about 8 glasses of water a day to keep healthy?";
		objectiveFat = true;


		int[,] fatPos = {

			{ 0, 0, 0, 0, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 1, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 1, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 }
		};


		SetFatPositions (fatPos);

		int[,] grid = {{  2, 1, 0, 4, 1, 1, 3, 0 },
						{ 2, 4, 2, 2, 4, 4, 3, 0 },
						{ 1, 2, 0, 2, 2, 0, 0, 3 },
						{ 1, 0, 2, 0, 1, 2, 3, 0 },
						{ 3, 1, 1, 3, 1, 0, 3, 0 },
						{ 1, 4, 2, 3, 0, 1, 0, 3 },
						{ 3, 2, 0, 2, 2, 1, 3, 0 },
						{ 3, 4, 2, 3, 0, 0, 3, 0 }
					}; 


		SetGridContent (grid);


	}
	void SetUpL7(){
		fatOn = true;
		hotDogOn = true;
		cigOn = false;
		ingredientsOn = false;
		ingredientHolders = 0;
		gridWidth = 8;
		gridHeight = 8;
		moves = 34;
		time = 0;
		target = UNATTAINABLE;
		boostersNeeded = UNATTAINABLE;
		specBoostersNeeded = UNATTAINABLE;
		gameoverMessage = "Did you know that you should drink about 8 glasses of water a day to keep healthy?";
		objectiveFat = true;


		int[,] fatPos = {

			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 }
		};


		SetFatPositions (fatPos);

		int[,] grid = {{  2, 1, 0, 4, 1, 1, 3, 0 },
			{ 2, 4, 2, 2, 4, 4, 3, 0 },
			{ 1, 2, 0, 2, 2, 0, 0, 3 },
			{ 1, 0, 2, 0, 1, 2, 3, 0 },
			{ 3, 1, 1, 3, 1, 0, 3, 0 },
			{ 1, 4, 2, 3, 0, 1, 0, 3 },
			{ 3, 2, 0, 2, 2, 1, 3, 0 },
			{ 3, 4, 2, 3, 0, 0, 3, 0 }
		}; 


		SetGridContent (grid);

	}
	void SetUpL8(){

		fatOn = false;
		hotDogOn = false;
		cigOn = false;
		ingredientsOn = false;
		ingredientHolders = 0;
		gridWidth = 8;
		gridHeight = 8;
		moves = 15;
		time = 0;
		target = UNATTAINABLE;
		boostersNeeded = UNATTAINABLE;
		specBoostersNeeded = UNATTAINABLE;
		gameoverMessage = "Did you know that you should drink about 8 glasses of water a day to keep healthy?";
		objectiveBeer = true;



		int[,] grid = {{  2, 1, 0, 4, 1, 1, 3, 0 },
						{ 2, 4, 2, 2, 4, 4, 3, 0 },
						{ 1, 2, 0, 1, 9, 0, 0, 3 },
						{ 1, 0, 2, 1, 9, 2, 3, 0 },
						{ 3, 1, 1, 2, 9, 0, 3, 0 },
						{ 1, 4, 2, 1, 9, 1, 0, 3 },
						{ 3, 2, 0, 1, 2, 1, 3, 0 },
						{ 3, 4, 2, 3, 0, 0, 3, 0 }
					}; 

		SetGridContent (grid);
	}
	void SetUpL9(){


		fatOn = false;
		hotDogOn = false;
		cigOn = true;
		ingredientsOn = false;
		ingredientHolders = 0;
		gridWidth = 8;
		gridHeight = 8;
		moves = 13;
		time = 0;
		target = UNATTAINABLE;
		boostersNeeded = UNATTAINABLE;
		specBoostersNeeded = UNATTAINABLE;
		gameoverMessage = "Some message saying how bad cigarettes are for you";
		objectiveCiggy = true;



		int[,] grid = {{  2, 1, 0, 4, 1, 1, 3, 0 },
						{ 2, 4, 2, 2, 4, 4, 3, 0 },
						{ 1, 2, 0, 1, 1, 0, 0, 3 },
						{ 1, 0, 2, 1, 2, 2, 3, 0 },
						{ 3, 1, 1, 2, 3, 0, 3, 0 },
						{ 1, 4, 2, 1, 4, 1, 0, 3 },
						{ 3, 2, 0, 1, 2, 1, 3, 0 },
						{ 3, 4, 2, 3, 0, 0, 3, 0 }
					}; 

		SetGridContent (grid);

		List<Vector2> cigPos = new List<Vector2> ();

		for (int x = 0; x < gridWidth; x++) {
			cigPos.Add (new Vector2 (x, 3));
		}

		SetCigPositions (cigPos);

	}
	void SetUpL10(){


		fatOn = false;
		hotDogOn = false;
		cigOn = true;
		ingredientsOn = false;
		ingredientHolders = 0;
		gridWidth = 8;
		gridHeight = 8;
		moves = UNATTAINABLE;
		time = 40;
		target = UNATTAINABLE;
		boostersNeeded = UNATTAINABLE;
		specBoostersNeeded = UNATTAINABLE;
		gameoverMessage = "Some message saying how bad cigarettes are for you";
		objectiveCiggy = true;



		int[,] grid = {{  2, 1, 0, 4, 1, 1, 3, 0 },
			{ 2, 4, 2, 2, 4, 4, 3, 0 },
			{ 1, 2, 0, 1, 1, 0, 0, 3 },
			{ 1, 0, 2, 1, 2, 2, 3, 0 },
			{ 3, 1, 1, 2, 3, 0, 3, 0 },
			{ 1, 4, 2, 1, 4, 1, 0, 3 },
			{ 3, 2, 0, 1, 2, 1, 3, 0 },
			{ 3, 4, 2, 3, 0, 0, 3, 0 }
		}; 

		SetGridContent (grid);

		List<Vector2> cigPos = new List<Vector2> ();

		for (int x = 0; x < gridWidth; x++) {
			cigPos.Add (new Vector2 (x, 4));
		}

		SetCigPositions (cigPos);

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

	void SetGridLayout(List<Vector2> layout){

		gridLayout = layout;

	}

	public int GetTime(){
		return time;
	}
	public List<Vector2> GetGridLayout(){

		gridLayout = new List<Vector2> ();

		for (int x = 0; x < gridWidth; x++) {
			for (int y = 0; y < gridHeight; y++) {
				if (gridContent [x, y] >= 0) {
					gridLayout.Add (new Vector2 (x, y));
				}
			}
		}

		return gridLayout;
	}



	public void CreateMenuButtons(){

		Button r = Instantiate (retry, new Vector2 (-26, 0), Quaternion.identity) as Button;
		r.transform.SetParent (GameObject.Find("Canvas").transform, false);
		r.transform.localPosition = new Vector2 (54, -89);
		r.onClick.AddListener (delegate {
			ResetLevel ();

		});

		Button m =  Instantiate (menu, new Vector2 (-26, 0), Quaternion.identity) as Button;
		m.transform.SetParent (GameObject.Find("Canvas").transform, false);
		m.transform.localPosition = new Vector2 (-75, -89);
		m.onClick.AddListener (delegate {
			LoadLevel (1);

		});

		Text t = Instantiate (levelOverText, new Vector2 (-18, -8.8f), Quaternion.identity) as Text;
		t.transform.SetParent (GameObject.Find("Canvas").transform, false);
		t.transform.localPosition = new Vector2 (-18, -8.8f);
		t.text = gameoverMessage;

		Text s = Instantiate (scoretext, new Vector2 (-18, -8.8f), Quaternion.identity) as Text;
		s.transform.SetParent (GameObject.Find("Canvas").transform, false);
		s.transform.localPosition = new Vector2 (441, -70);
		s.text = "" + scorehandler.GetScore ();

	}

	public void ResetLevel(){
		Application.LoadLevel (Application.loadedLevel);	
	}

	public void LoadLevel(int level){
		Application.LoadLevel (level);
	}

	public void TimesUp(){
		Debug.Log ("TIMES UP!");

		GameObject tu = Instantiate (timesUp, new Vector2 (3.3f, -5), Quaternion.identity) as GameObject;
		iTween.MoveTo (tu, iTween.Hash("y", 3.39, "time", 1));
		Invoke ("CreateMenuButtons", 0.6f);
		gameEnded = true;
	}

	public void OutOfMoves(){

		GameObject oom = Instantiate (outOfMoves, new Vector2 (3.3f, -5), Quaternion.identity) as GameObject;
		iTween.MoveTo (oom, iTween.Hash("y", 3.39, "time", 1));
		Invoke ("CreateMenuButtons", 0.6f);
		Debug.Log ("OUT OF MOVESSS");
		gameEnded = true;

	}

	public void LevelPassed(){
		Debug.Log ("LEVEL PASSED");

		GameObject lp = Instantiate (levelPassed, new Vector2 (3.3f, -5), Quaternion.identity) as GameObject;
		iTween.MoveTo (lp, iTween.Hash("y", 3.39, "time", 1));
		Invoke ("CreateMenuButtons", 0.6f);
		gameEnded = true;
	}




}
