using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {

	public Text scoreText;
	private int score;

	public void AddPoints(int points){
		score += points;
		scoreText.text = "SCORE\n" + score;
	}

	public void RemovePoints(int points){

		//to ensure score never drops below zero
		if (score >= points) {
			score -= points;
			scoreText.text = "SCORE\n" + score;
		}

	}


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
