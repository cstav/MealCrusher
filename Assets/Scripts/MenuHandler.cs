using UnityEngine;
using System.Collections;

public class MenuHandler : MonoBehaviour {

	// Use this for initialization
	public void LoadLevel(int level){
		Application.LoadLevel (level);

	}

	public void QuitGame(){
		Application.Quit();
	
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
