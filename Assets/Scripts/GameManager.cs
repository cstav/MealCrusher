using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public int[,] highscores;
	public int currentWeek;
	public int prevViewedWeek = 1;
	public bool[,] levelState; //locked or unlocked
	public List<Vector2> unlockedLevels; //retrieve from database

	public int numOfWeeks = 10;
	public int levelsPerWeek = 5;

	bool locked = false;
	bool unlocked = true;


	// Use this for initialization
	void Awake () {
	
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);


	}

	void Start(){

		//row: week	col: level in that week
		highscores = new int[numOfWeeks,levelsPerWeek];
		levelState = new bool[numOfWeeks,levelsPerWeek];

		//Set all levels to locked
		LockLevels();
		//unlock appropriate weeks
		currentWeek = 6;
		UnlockWeeks(currentWeek);
		//now unlock all levels that have been completed by the user
		unlockedLevels = RetrieveUnlockedLevels();
		UnlockCompletedLevels (unlockedLevels);

		UnlockLevel (29);
		UnlockLevel (30);

		PrintLevelStates ();

	}

	List<Vector2> RetrieveUnlockedLevels(){
		/* RETRIEVE LIST FROM DATABASE HERE */
		List<Vector2> dbList = new List<Vector2> ();
//		dbList.Add (new Vector2 (0, 0));
//		dbList.Add (new Vector2 (0, 1));
//		dbList.Add (new Vector2 (0, 2));
//		dbList.Add (new Vector2 (0, 3));

		return dbList;
	}

	public void SubmitUnlockedLevels(){
		/* STORE ALL UNLOCKED LEVELS IN DATABASE */
		List<Vector2> listForDatabase = new List<Vector2> ();

		for (int i = 0; i < numOfWeeks; i++) {
			for (int j = 0; j < levelsPerWeek; j++) {
				if (levelState [i, j] == unlocked) {
					listForDatabase.Add (new Vector2(i,j));
				}
			}
		}
	}

	public void LockLevels(){

		for (int i = 0; i < numOfWeeks; i++) {
			for (int j = 0; j < levelsPerWeek; j++) {
				levelState [i, j] = locked;
			}
		}
	}

	public void UnlockLevel(int level){

		if (level <= currentWeek * levelsPerWeek) {

			level--; //because we starting at 0

			int week = level / levelsPerWeek;
			int weekLevel = level % levelsPerWeek;

			levelState [week, weekLevel] = unlocked;
		}

	}

	public void UnlockWeeks(int currentWeek){

		//unlocks the first level of every week up to and including the current week
		for(int i = 0; i < currentWeek; i++){
			levelState [i, 0] = unlocked;
		}
	}

	public void UnlockCompletedLevels(List<Vector2> unlockedLevels){

		foreach (Vector2 level in unlockedLevels) {
			int week = Mathf.RoundToInt (level.x);
			int weekLevel = Mathf.RoundToInt (level.y);

			levelState [week, weekLevel] = unlocked;

		}
		
	}

	public void PrintLevelStates(){

		string s = "";
		int level = 0;
		for(int i = 0; i < numOfWeeks; i++){
			for (int j = 0; j < levelsPerWeek; j++) {
				level++;
				s += "Level " + level + ": " + levelState[i,j] + "\n";
			}
		}

		Debug.Log (s);
	}

	public void SaveMapPosition(int week){
		prevViewedWeek = week;
	
	}

	public int GetHighscore(int level){


		int week = level / levelsPerWeek;
		int weekLevel = level % levelsPerWeek;

		return highscores [week, weekLevel];
	}

	public void UpdateHighscore(int level, int score){

		level--; //because we starting at 0

		int week = level / levelsPerWeek;
		int weekLevel = level % levelsPerWeek;

		highscores [week, weekLevel] = score;

	}

	public string GetWeeklyScore(int week){

		int weeklyscore = 0;

		Debug.Log ("week: " + week);

		for (int i = 0; i < levelsPerWeek; i++) {
			weeklyscore += highscores [week, i];
		}

		return "" + weeklyscore;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
